from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_PATIENT_CHECK, API_CHECK_SERVICE
from Frontend.const.common_variables import TODAY_DATE_ONLY, TODAY_TIME_ONLY, TODAY_DATETIME
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.models.services_model import ServicesModel, ServiceModel
from Frontend.enum.enums import FormType
from Frontend.components.crud_button import crud_button
from Frontend.components.dynamic_form import dynamic_form_dialog
from Frontend.components.table import table
from Frontend.components.multiple_selections import multiple_selections
from .check_category_services import CheckCategoryState
from .patient import PatientState
from datetime import datetime

import reflex as rx


class PatientCheckState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    check_options: list[ServicesModel]
    selected_services: list[str]
    selected_patient_data: dict[str, str] = {}
    service_detail: dict[str, str] = {}

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
        for form in forms:
            if self.selected_data and self.selected_data[form.name]:
                form.default_value = converter.to_date_input(self.selected_data[form.name])
        return forms
    
    @rx.var
    def patient_check_result_form(self) -> list[FormModel]:
        forms = [
            FormModel(
                name="numericResult",
                placeholder="NumericResult",
                form_type=FormType.Input.value,
            ),
            FormModel(
                name="stringResult",
                placeholder="StringResult",
                form_type=FormType.Input.value,
            )
        ]
        for form in forms:
            if self.selected_data and self.selected_data[form.name]:
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

    
    def select_service(self, service_id: str, value):
        for category in self.check_options:
            for service in category.services:
                if service.id == service_id:
                    service.selected = value

    def get_check_service(self, service_id: str) -> ServiceModel:
        for category in self.check_options:
            for service in category.services:
                if service.id == service_id:
                    return service

    async def get_data(self):
        check_categories_states = await self.get_state(CheckCategoryState)
        await check_categories_states.get_data()
        self.check_options = [converter.to_services_model(item["name"], item["checkServices"]) for item in check_categories_states.raw_data]

        patient_states = await self.get_state(PatientState)
        await patient_states.get_data()
        if patient_states.selected_data:
            self.selected_patient_data = patient_states.selected_data

        _, self.raw_data = await api_call.get(API_PATIENT_CHECK)
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "patient" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: [patient["name"] for patient in patient_states.raw_data if patient["id"] == data])
                if "checkservice" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_check_service(data).name)
            self.data = dataFrame.values.tolist()
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
        print(form_data)
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
        for key, value in form_data.items():
            self.selected_data[key] = value if value else None
        await api_call.post(
            f"{API_PATIENT_CHECK}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_PATIENT_CHECK}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/patient_check", title="Patient Check")
def patient_check() -> rx.Component:
    return rx.vstack(
        rx.flex(rx.text(PatientCheckState.selected_patient_data["name"])),
        multiple_selections(PatientCheckState.check_options, PatientCheckState.select_service),
        rx.flex(
            rx.foreach(
                PatientCheckState.selected_service_ids,
                rx.badge
            ),
            spacing="2"
        ),
        rx.hstack(
            rx.button(rx.icon("chevron-left"), "Back", on_click=rx.redirect("/patient")),
            crud_button(
                "Patient Check",
                PatientCheckState,
                PatientCheckState.patient_check_form,
                PatientCheckState.patient_check_form,
                PatientCheckState.is_patient_data_empty
            ),
            dynamic_form_dialog(
                ~PatientCheckState.updating,
                "Submit Check Result",
                "Submit Check",
                PatientCheckState.patient_check_result_form,
                PatientCheckState.submit_check_data
            ),
            spacing="8"
        ),
        table(PatientCheckState),
        on_mount=PatientCheckState.get_data
    )