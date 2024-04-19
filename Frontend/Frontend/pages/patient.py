from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_PATIENT
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.enum.enums import FormType, Gender
from json import loads
from Frontend.components.crud_button import crud_button

import reflex as rx

class PatientState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    new_patient_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Name",
            required=True,
            form_type=FormType.Input.value
        ),
        FormModel(
            name="medicalRecordNumber",
            placeholder="Medical Record Number (No. RM)",
            required=True,
            form_type=FormType.Input.value
        ),
        FormModel(
            name="dateOfBirth",
            placeholder="Date of Birth",
            required=True,
            form_type=FormType.Date.value
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
            form_type=FormType.Input.value
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
            form_type=FormType.Input.value
        ),
        FormModel(
            name="address",
            placeholder="Address",
            required=True,
            form_type=FormType.Input.value
        ),
    ]
    update_patient_form: list[FormModel] =  []

    async def get_data(self):
        response = await api_call.get(API_PATIENT)
        self.raw_data = loads(response.text)["data"]
        self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
        if(dataFrame is not None):
            for column in dataFrame.columns:
                if "gender" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: converter.to_title_case(Gender(data).name))

            self.data = dataFrame.values.tolist()

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.update_patient_form = [
            FormModel(
                name="name",
                placeholder="Name",
                required=True,
                form_type=FormType.Input.value,
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
                default_value=self.selected_data["phoneNumber"]
            ),
            FormModel(
                name="address",
                placeholder="Address",
                required=True,
                form_type=FormType.Input.value,
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
        crud_button(
            "Patient",
            PatientState,
            PatientState.new_patient_form,
            PatientState.update_patient_form,
        ),
        rx.data_editor(
            columns=PatientState.columns,
            data=PatientState.data,
            on_cell_clicked=PatientState.get_selected_data,
            column_select="none",
        ),
        on_mount=PatientState.get_data
    )