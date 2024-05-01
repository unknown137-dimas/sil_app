from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_MEDICAL_TOOL
from Frontend.const.common_variables import TODAY_DATE_ONLY
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.enum.enums import FormType, CalibrationStatus
from json import loads
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table

import reflex as rx

class MedicalToolState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    new_medical_tool_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Name",
            required=True,
            form_type=FormType.Input.value,
            min_length=5,
        ),
        FormModel(
            name="code",
            placeholder="Code",
            required=True,
            form_type=FormType.Input.value,
            pattern="[A-Z]{3}-[0-9]{3}",
        ),
    ]
    update_medical_tool_form: list[FormModel] =  []

    async def get_data(self):
        self.clear_selected_data()
        response = await api_call.get(API_MEDICAL_TOOL)
        self.raw_data = loads(response.text)["data"]
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "calibrationstatus" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: converter.to_title_case(CalibrationStatus(data).name))

            self.data = dataFrame.values.tolist()
        self.loading = False


    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_medical_tool_form = [
                FormModel(
                    name="name",
                    placeholder="Name",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=5,
                    default_value=self.selected_data["name"]
                ),
                FormModel(
                    name="code",
                    placeholder="Code",
                    required=True,
                    form_type=FormType.Input.value,
                    pattern="[A-Z]{3}-[0-9]{3}",
                    default_value=self.selected_data["code"]
                ),
                FormModel(
                    name="calibrationDate",
                    placeholder="Calibration Date",
                    required=True,
                    form_type=FormType.Date.value,
                    default_value=converter.to_date_input(self.selected_data["calibrationDate"]),
                    min_value=TODAY_DATE_ONLY
                ),
                FormModel(
                    name="calibrationStatus",
                    placeholder="Calibration Status",
                    required=True,
                    form_type=FormType.Select.value,
                    options=[converter.to_title_case(cs.name) for cs in CalibrationStatus],
                    default_value=converter.to_title_case(CalibrationStatus(self.selected_data["calibrationStatus"]).name)
                ),
                FormModel(
                    name="calibrationNote",
                    placeholder="Calibration Note",
                    required=True,
                    form_type=FormType.Input.value,
                    default_value=self.selected_data["calibrationNote"] if self.selected_data["calibrationNote"] != None else "" 
                ),
            ]
    
    async def update_data(self, form_data: dict):
        form_data["calibrationStatus"] = CalibrationStatus[form_data["calibrationStatus"].replace(" ","")].value
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_MEDICAL_TOOL}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()
        self.updating = False

    async def add_data(self, form_data: dict):
        form_data["calibrationNote"] = ""
        await api_call.post(
            API_MEDICAL_TOOL,
            payload=form_data
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_MEDICAL_TOOL}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/medical_tool", title="Medical Tool")
def medical_tool() -> rx.Component:
    return rx.vstack(
        crud_button(
            "Medical Tool",
            MedicalToolState,
            MedicalToolState.new_medical_tool_form,
            MedicalToolState.update_medical_tool_form,
        ),
        table(MedicalToolState),
        on_mount=MedicalToolState.get_data
    )