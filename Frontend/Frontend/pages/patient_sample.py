from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_PATIENT_SAMPLE, API_PATIENT_SAMPLE_RESULT
from Frontend.const.common_variables import TODAY_DATE_ONLY, TODAY_TIME_ONLY
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.models.services_model import ServicesModel, ServiceModel
from Frontend.enum.enums import FormType
from json import loads
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table
from Frontend.components.multiple_selections import multiple_selections
from .sample_category_services import SampleCategoryState
from .patient import PatientState

import reflex as rx


class PatientSampleState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    sample_options: list[ServicesModel]
    selected_services: list[str]
    selected_patient_data: dict[str, str] = {}
    new_patient_sample_form: list[FormModel] = [
        FormModel(
            name="sampleSchedule",
            placeholder="Sample Schedule",
            required=True,
            form_type=FormType.Date.value,
            min_value=TODAY_DATE_ONLY
        ),
    ]
    update_patient_sample_form: list[FormModel] =  []

    @rx.var
    def selected_service_ids(self) -> list[str]:
        result: list[str] = []
        for category in self.sample_options:
            for service in category.services:
                if service.selected:
                    result.append(service.id)
        return result
    
    def select_service(self, service_id: str, value):
        for category in self.sample_options:
            for service in category.services:
                if service.id == service_id:
                    service.selected = value

    def get_sample_service(self, service_id: str) -> ServiceModel:
        for category in self.sample_options:
            for service in category.services:
                if service.id == service_id:
                    return service

    async def get_data(self):
        sample_categories_states = await self.get_state(SampleCategoryState)
        await sample_categories_states.get_data()
        self.sample_options = [converter.to_services_model(item["name"], item["sampleServices"]) for item in sample_categories_states.raw_data]

        patient_states = await self.get_state(PatientState)
        await patient_states.get_data()
        if patient_states.selected_data:
            self.selected_patient_data = patient_states.selected_data

        response = await api_call.get(API_PATIENT_SAMPLE)
        self.raw_data = loads(response.text)["data"]
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "patient" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: [patient["name"] for patient in patient_states.raw_data if patient["id"] == data])
                if "sampleservice" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_sample_service(data).name)
            self.data = dataFrame.values.tolist()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_patient_sample_form = [
                FormModel(
                    name="sampleSchedule",
                    placeholder="Sample Schedule",
                    required=True,
                    form_type=FormType.Datetime.value,
                    min_value=TODAY_DATE_ONLY
                ),
            ]
    
    async def update_data(self, form_data: dict):
        print(form_data)
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_PATIENT_SAMPLE}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()
        self.updating = False

    async def add_data(self, form_data: dict):
        form_data["patientId"] = self.selected_patient_data["id"]
        for service_id in self.selected_service_ids:
            form_data["sampleServiceId"] = service_id
            await api_call.post(
                API_PATIENT_SAMPLE,
                payload=form_data
            )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_PATIENT_SAMPLE}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/patient_sample", title="Patient Sample")
def patient_sample() -> rx.Component:
    return rx.vstack(
        rx.button(rx.icon("chevron-left"), "Back", on_click=rx.redirect("/patient")),
        rx.flex(rx.text(PatientSampleState.selected_patient_data["name"])),
        multiple_selections(PatientSampleState.sample_options, PatientSampleState.select_service),
        rx.flex(
            rx.foreach(
                PatientSampleState.selected_service_ids,
                rx.badge
            ),
            spacing="2"
        ),
        crud_button(
            "Patient Sample",
            PatientSampleState,
            PatientSampleState.new_patient_sample_form,
            PatientSampleState.update_patient_sample_form,
        ),
        table(PatientSampleState),
        on_mount=PatientSampleState.get_data
    )