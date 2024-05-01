from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_PATIENT
from Frontend.const.common_variables import TODAY_DATE_ONLY
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.enum.enums import FormType, Gender
from json import loads
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table
from Frontend.base_state import BaseState

import reflex as rx

class PatientState(BaseState):
    new_patient_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Name",
            required=True,
            form_type=FormType.Input.value,
            min_length=5,
        ),
        FormModel(
            name="medicalRecordNumber",
            placeholder="Medical Record Number (No. RM)",
            required=True,
            form_type=FormType.Input.value,
        ),
        FormModel(
            name="dateOfBirth",
            placeholder="Date of Birth",
            required=True,
            form_type=FormType.Date.value,
            max_value=TODAY_DATE_ONLY,
        ),
        FormModel(
            name="gender",
            placeholder="Gender",
            required=True,
            form_type=FormType.Select.value,
            options=[g.name for g in Gender]
        ),
        FormModel(
            name="identityNumber",
            placeholder="Identity Number (NIK)",
            required=True,
            form_type=FormType.Input.value,
            min_length=16,
            max_length=16
        ),
        FormModel(
            name="healthInsuranceNumber",
            placeholder="Health Insurance Number",
            required=True,
            form_type=FormType.Input.value
        ),
        FormModel(
            name="phoneNumber",
            placeholder="Phone Number",
            required=True,
            form_type=FormType.Input.value,
            min_length=10,
        ),
        FormModel(
            name="address",
            placeholder="Address",
            required=True,
            form_type=FormType.Input.value,
            min_length=10,
        ),
    ]
    update_patient_form: list[FormModel] =  []

    async def get_data(self):
        self.clear_selected_data()
        response = await api_call.get(API_PATIENT)
        self.raw_data = loads(response.text)["data"]
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "gender" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: converter.to_title_case(Gender(data).name))

            self.data = dataFrame.values.tolist()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_patient_form = [
                FormModel(
                    name="name",
                    placeholder="Name",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=5,
                    default_value=self.selected_data["name"]
                ),
                FormModel(
                    name="medicalRecordNumber",
                    placeholder="Medical Record Number (No. RM)",
                    required=True,
                    form_type=FormType.Input.value,
                    default_value=self.selected_data["medicalRecordNumber"]
                ),
                FormModel(
                    name="dateOfBirth",
                    placeholder="Date of Birth",
                    required=True,
                    form_type=FormType.Date.value,
                    max_value=TODAY_DATE_ONLY,
                    default_value=converter.to_date_input(self.selected_data["dateOfBirth"])
                ),
                FormModel(
                    name="gender",
                    placeholder="Gender",
                    required=True,
                    form_type=FormType.Select.value,
                    options=[g.name for g in Gender],
                    default_value=Gender(self.selected_data["gender"]).name
                ),
                FormModel(
                    name="identityNumber",
                    placeholder="Identity Number (NIK)",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=16,
                    max_length=16,
                    default_value=self.selected_data["identityNumber"]
                ),
                FormModel(
                    name="healthInsuranceNumber",
                    placeholder="Health Insurance Number",
                    required=True,
                    form_type=FormType.Input.value,
                    default_value=self.selected_data["healthInsuranceNumber"]
                ),
                FormModel(
                    name="phoneNumber",
                    placeholder="Phone Number",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=10,
                    default_value=self.selected_data["phoneNumber"]
                ),
                FormModel(
                    name="address",
                    placeholder="Address",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=10,
                    default_value=self.selected_data["address"]
                ),
            ]
    
    async def update_data(self, form_data: dict):
        form_data["gender"] = Gender[form_data["gender"]].value
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_PATIENT}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()
        self.updating = False

    async def add_data(self, form_data: dict):
        form_data["gender"] = Gender[form_data["gender"]].value
        await api_call.post(
            API_PATIENT,
            payload=form_data
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_PATIENT}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/patient", title="Patient")
def patient() -> rx.Component:
    return rx.vstack(
        rx.hstack(
            crud_button(
                "Patient",
                PatientState,
                PatientState.new_patient_form,
                PatientState.update_patient_form,
            ),
            rx.button(
                "Request Sample",
                disabled=~PatientState.updating,
                on_click=rx.redirect("/patient_sample")
            ),
            spacing="8"
        ),
        table(PatientState),
        on_blur=PatientState.clear_selected_data,
        on_mount=PatientState.get_data,
    )