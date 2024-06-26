from Frontend import styles
from Frontend.templates import template
from Frontend.states.auth_state import AuthState
from Frontend.const.api import API_PATIENT_CHECK, API_CHECK_SERVICE
from Frontend.const.common_variables import TODAY_DATE_ONLY, TODAY_TIME_ONLY, TODAY_DATETIME
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.models.services_model import CheckServicesModel, CheckServiceModel
from Frontend.enum.enums import FormType, ValidationStatus, CheckStatus, CheckType, UserRoles
from Frontend.components.crud_button import crud_button
from Frontend.components.dynamic_form import dynamic_form_dialog
from Frontend.components.table import table, sort_table, get_columns
from Frontend.components.multiple_selections import multiple_selections
from .check_category_services import CheckCategoryState
from .patient import PatientState
from datetime import datetime
from pandas import DataFrame
from re import findall

import numpy as np
import reflex as rx


class PatientCheckState(rx.State):
    columns: list = []
    data: list = []
    dataFrame: DataFrame
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    check_options: list[CheckServicesModel]
    check_services: list[CheckServicesModel]
    selected_services: list[str]
    selected_patient_data: dict[str, str] = {}
    service_detail: dict[str, str] = {}
    downloading: bool = False

    @rx.var
    def patient_check_form(self) -> list[FormModel]:
        forms = [
            FormModel(
                name="checkSchedule",
                placeholder="Check Schedule",
                required=True,
                form_type=FormType.Date.value,
                min_value=TODAY_DATE_ONLY
            )
        ]
        if self.selected_data:
            for form in forms:
                form.default_value = converter.to_date_input(self.selected_data[form.name])
        return forms
    
    @rx.var
    def patient_check_result_form(self) -> list[FormModel]:
        forms = []
        if self.selected_data:
            checkServiceType = self.get_check_service(self.selected_data["checkServiceId"]).normal_value_type
            if checkServiceType == CheckType.Numeric:
                forms.append(
                    FormModel(
                        name="numericResult",
                        placeholder="Result",
                        form_type=FormType.Input.value,
                        required=True
                    )
                )
            else:
                forms.append(
                    FormModel(
                        name="stringResult",
                        placeholder="Result",
                        form_type=FormType.Input.value,
                        required=True
                    )
                )

            for form in forms:
                form.default_value = self.selected_data[form.name]
        return forms

    @rx.var
    def patient_check_validate_result_form(self) -> list[FormModel]:
        forms = []
        if self.selected_data:
            forms.append(
                FormModel(
                    name="validationStatus",
                    placeholder="Validation Status",
                    required=True,
                    form_type=FormType.Select.value,
                    options=[converter.to_title_case(vs.name) for vs in ValidationStatus],
                    default_value=converter.to_title_case(ValidationStatus(self.selected_data["validationStatus"]).name)
                )
            )

            for form in forms:
                form.default_value = self.selected_data[form.name]
        return forms

    @rx.var
    def selected_service_ids(self) -> list[str]:
        result: list[str] = []
        for category in self.check_options:
            for service in category.services:
                if service.selected:
                    result.append(service.id)
        return result

    @rx.var
    def is_patient_data_empty(self) -> bool:
        return self.selected_patient_data == {}

    @rx.var
    def is_result_available(self) -> bool:
        return self.selected_data != {} and self.is_result_submitted

    @rx.var
    def is_allowed_to_add(self) -> bool:
        return ((not self.is_patient_data_empty) and (self.selected_service_ids != []))

    @rx.var
    def is_result_submitted(self) -> bool:
        if self.selected_data:
            return self.selected_data["checkStatus"] == CheckStatus.Done.value
        return False

    @rx.var
    def get_columns(self) -> list[str]:
        return get_columns(self)

    def select_service(self, service_id: str, value):
        for category in self.check_options:
            for service in category.services:
                if service.id == service_id:
                    service.selected = value

    def get_check_service(self, service_id: str) -> CheckServiceModel:
        for category in self.check_services:
            for service in category.services:
                if service.id == service_id:
                    return service

    async def get_data(self):
        patient_states = await self.get_state(PatientState)
        await patient_states.get_data()
        
        check_categories_states = await self.get_state(CheckCategoryState)
        await check_categories_states.get_data()
        self.check_services = [converter.to_check_services_model(item["name"], item["checkServices"]) for item in check_categories_states.raw_data]
        
        self.selected_patient_data = patient_states.selected_data
        if self.selected_patient_data:
            check_data = []
            for item in check_categories_states.raw_data:
                item["checkServices"] = [service for service in item["checkServices"] if service["gender"] == patient_states.selected_data["gender"]]
                check_data.append(item)
                        
            self.check_options = [converter.to_check_services_model(item["name"], item["checkServices"]) for item in check_data]

        _, self.raw_data = await api_call.get(API_PATIENT_CHECK)
        if self.raw_data:
            columns, _, dataFrame = converter.to_data_table(self.raw_data)
            dataFrame["result"] = np.where(dataFrame["checkService"].apply(lambda x: self.get_check_service(x).normal_value_type) == CheckType.Numeric, dataFrame["numericResult"], dataFrame["stringResult"])
            dataFrame["result"] = dataFrame["result"].astype("string")
            dataFrame.drop(columns=["numericResult", "stringResult"], inplace=True)
            for column in dataFrame.columns:
                if "patient" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: [patient["name"] for patient in patient_states.raw_data if patient["id"] == data])
                if "checkservice" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_check_service(data).name)
                if "validationstatus" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: converter.to_title_case(ValidationStatus(data).name))
                if "checkstatus" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: converter.to_title_case(CheckStatus(data).name))
            columns = [column for column in columns if "Result" not in column["title"]]
            columns.append(
                {
                    "title": "Result",
                    "type": "str",
                }
            )
            self.columns = columns
            self.data = dataFrame.values.tolist()
            self.dataFrame = dataFrame
        self.loading = False
        self.updating = False
        self.selected_data = {}

    async def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            _, self.service_detail = await api_call.get(f"{API_CHECK_SERVICE}/{self.selected_data['checkServiceId']}")
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_PATIENT_CHECK}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()

    async def add_data(self, form_data: dict):
        form_data["patientId"] = self.selected_patient_data["id"]
        for service_id in self.selected_service_ids:
            form_data["checkServiceId"] = service_id
            await api_call.post(
                API_PATIENT_CHECK,
                payload=form_data
            )
        await self.get_data()

    async def submit_check_data(self, form_data: dict):
        form_data["checkStatus"] = CheckStatus.Done.value
        if "validationStatus" not in form_data.keys():
            form_data["validationStatus"] = ValidationStatus.WaitingValidation.value
        else:
            form_data["validationStatus"] = ValidationStatus[form_data["validationStatus"].replace(" ","")].value
        self.selected_data.update(form_data)
        print(self.selected_data)
        await api_call.post(
            f"{API_PATIENT_CHECK}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_PATIENT_CHECK}/{self.selected_data['id']}")
        await self.get_data()

    def sort_table(self, sort_key: str):
        sort_table(self, sort_key)

    async def download_result(self):
        if self.selected_data == {}:
            return
        self.downloading = True
        raw_result = await api_call.get(f"{API_PATIENT_CHECK}/export-pdf", self.selected_data, True)
        file_name_data = findall(r"filename\*?=([^;]+)", raw_result.headers.get("content-disposition"))
        file_name = file_name_data[0].strip().strip('"')
        self.downloading = False
        return rx.download(filename=file_name, data=raw_result.content)

@template(route="/patient_check", title="Patient Check", image="/stethoscope.svg")
def patient_check() -> rx.Component:
    return rx.vstack(
        rx.match(
            AuthState.is_regis_staff,
            (
                True, 
                rx.cond(
                    PatientCheckState.is_patient_data_empty,
                    rx.callout(
                        "No patient selected, please select patient first from patient page",
                        icon="info",
                        color_scheme="yellow",
                    ),
                    rx.fragment(
                        rx.flex(
                            rx.text("Selected Patient: "),
                            rx.badge(
                                PatientCheckState.selected_patient_data["name"],
                                variant="solid",
                                size="2",
                                radius="full"
                            ),
                            spacing="1",
                            align="center"
                        ),
                        multiple_selections(PatientCheckState.check_options, PatientCheckState.select_service),
                    ),
                ),
            )
        ),
        rx.flex(
            rx.match(
                AuthState.role,
                (
                    UserRoles.Regis.value,
                    rx.flex(
                        rx.button(
                            rx.icon("chevron-left"),
                            "Back", 
                            on_click=rx.redirect("/patient"),
                            radius="full"
                        ),
                        crud_button(
                            "Patient Check",
                            PatientCheckState,
                            PatientCheckState.patient_check_form,
                            PatientCheckState.patient_check_form,
                            ~PatientCheckState.is_allowed_to_add
                        ),
                        spacing="8"
                    ),
                ),
                (
                    UserRoles.Lab.value,
                    rx.flex(
                        dynamic_form_dialog(
                            ~PatientCheckState.updating,
                            "Submit Check Result",
                            "Submit Result",
                            PatientCheckState.patient_check_result_form,
                            PatientCheckState.submit_check_data
                        ),
                        dynamic_form_dialog(
                            ~PatientCheckState.is_result_submitted,
                            "Validate Result",
                            "Validate Result",
                            PatientCheckState.patient_check_validate_result_form,
                            PatientCheckState.submit_check_data
                        ),
                        spacing="2"
                    )
                )
            ),
            rx.button(
                rx.icon("download", size=20),
                "Download Result",
                on_click=PatientCheckState.download_result,
                disabled=~PatientCheckState.is_result_available,
                radius="full",
                loading=PatientCheckState.downloading
            ),
            spacing="8"
        ),
        table(PatientCheckState, PatientCheckState.get_columns, PatientCheckState.sort_table),
        on_mount=PatientCheckState.get_data
    )