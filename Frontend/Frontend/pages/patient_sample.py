from Frontend import styles
from Frontend.templates import template
from Frontend.states.auth_state import AuthState
from Frontend.const.api import API_PATIENT_SAMPLE, API_SAMPLE_SERVICE, API_PATIENT
from Frontend.const.common_variables import TODAY_DATE_ONLY, TODAY_TIME_ONLY, TODAY_DATETIME
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.models.services_model import SampleServiceModel
from Frontend.enum.enums import FormType, UserRoles
from Frontend.components.crud_button import crud_button
from Frontend.components.dynamic_form import dynamic_form_dialog
from Frontend.components.table import table, sort_table, get_columns
from Frontend.components.multiple_selections import multiple_selections_checkbox
from .sample_category_services import SampleCategoryState
from .patient import PatientState
from datetime import datetime
from pandas import DataFrame
from re import findall

import reflex as rx


class PatientSampleState(rx.State):
    columns: list = []
    data: list = []
    dataFrame: DataFrame
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    sample_options: list[SampleServiceModel]
    selected_services: list[str]
    selected_patient_data: dict[str, str] = {}
    downloading: bool = False

    @rx.var
    def patient_sample_form(self) -> list[FormModel]:
        forms = [
            FormModel(
                name="sampleSchedule",
                placeholder="Sample Schedule",
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
    def patient_sample_result_form(self) -> list[FormModel]:
        forms = [
            FormModel(
                name="sampleNote",
                placeholder="Sample Note",
                required=True,
                form_type=FormType.Input.value,
            )
        ]
        if self.selected_data:
            for form in forms:
                form.default_value = self.selected_data[form.name]
        return forms

    @rx.var
    def selected_service_ids(self) -> list[str]:
        result: list[str] = []
        for service in self.sample_options:
            if service.selected:
                result.append(service.id)
        return result

    @rx.var
    def is_patient_data_empty(self) -> bool:
        return self.selected_patient_data == {}

    @rx.var
    def is_allowed_to_add(self) -> bool:
        return ((not self.is_patient_data_empty) and (self.selected_service_ids != []))

    @rx.var
    def get_columns(self) -> list[str]:
        return get_columns(self)
    
    def select_service(self, service_id: str, value):
        for service in self.sample_options:
            if service.id == service_id:
                service.selected = value

    def get_sample_service(self, service_id: str) -> SampleServiceModel:
        for service in self.sample_options:
            if service.id == service_id:
                return service

    async def get_data(self):
        patient_states = await self.get_state(PatientState)
        await patient_states.get_data()
        
        sample_categories_states = await self.get_state(SampleCategoryState)
        await sample_categories_states.get_data()
        self.sample_options = [SampleServiceModel(id=service["id"], name=service["name"]) for item in sample_categories_states.raw_data for service in item["sampleServices"]]

        self.selected_patient_data = patient_states.selected_data

        _, self.raw_data = await api_call.get(API_PATIENT_SAMPLE)
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "patient" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: [patient["name"] for patient in patient_states.raw_data if patient["id"] == data])
                if "sampleservice" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_sample_service(data).name)
            self.data = dataFrame.values.tolist()
            self.dataFrame = dataFrame
        self.loading = False
        self.updating = False
        self.selected_data = {}

    async def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_PATIENT_SAMPLE}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()

    async def add_data(self, form_data: dict):
        form_data["patientId"] = self.selected_patient_data["id"]
        for service_id in self.selected_service_ids:
            form_data["sampleServiceId"] = service_id
            await api_call.post(
                API_PATIENT_SAMPLE,
                payload=form_data
            )
        await self.get_data()

    async def submit_sample_data(self, form_data: dict):
        self.selected_data.update(form_data)
        self.selected_data["sampleTakenDate"] = TODAY_DATE_ONLY
        await api_call.post(
            f"{API_PATIENT_SAMPLE}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_PATIENT_SAMPLE}/{self.selected_data['id']}")
        await self.get_data()

    def sort_table(self, sort_key: str):
        sort_table(self, sort_key)

    async def download_label(self):
        if self.selected_data == {}:
            return
        self.downloading = True
        raw_result = await api_call.get(f"{API_PATIENT}/label/{self.selected_data['patientId']}", raw=True)
        file_name_data = findall(r"filename\*?=([^;]+)", raw_result.headers.get("content-disposition"))
        file_name = file_name_data[0].strip().strip('"')
        self.downloading = False
        return rx.download(filename=file_name, data=raw_result.content)

@template(route="/patient_sample", title="Patient Sample", image="/pipette.svg")
def patient_sample() -> rx.Component:
    return rx.vstack(
        rx.match(
            AuthState.is_regis_staff,
            (
                True, 
                rx.cond(
                    PatientSampleState.is_patient_data_empty,
                    rx.callout(
                        "No patient selected, please select patient first from patient page",
                        icon="info",
                        color_scheme="yellow",
                    ),
                    rx.fragment(
                        rx.flex(
                            rx.text("Selected Patient: "),
                            rx.badge(
                                PatientSampleState.selected_patient_data["name"],
                                variant="solid",
                                size="2",
                                radius="full"
                            ),
                            spacing="1",
                            align="center"
                        ),
                        multiple_selections_checkbox(PatientSampleState.sample_options, PatientSampleState.select_service),
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
                            "Patient Sample",
                            PatientSampleState,
                            PatientSampleState.patient_sample_form,
                            PatientSampleState.patient_sample_form,
                            ~PatientSampleState.is_allowed_to_add
                        ),
                        spacing="8"
                    ),
                ),
                (
                    UserRoles.Sampling.value,
                    dynamic_form_dialog(
                        ~PatientSampleState.updating,
                        "Submit Sample Result",
                        "Submit Sample",
                        PatientSampleState.patient_sample_result_form,
                        PatientSampleState.submit_sample_data
                    ),
                )
            ),
            rx.button(
                rx.icon("download", size=20),
                "Download Patient Label",
                on_click=PatientSampleState.download_label,
                disabled=~PatientSampleState.updating,
                radius="full",
                loading=PatientSampleState.downloading
            ),
            spacing="8"
        ),
        table(PatientSampleState, PatientSampleState.get_columns, PatientSampleState.sort_table),
        on_mount=PatientSampleState.get_data
    )