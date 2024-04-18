from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_MEDICAL_TOOL
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.utilities.dynamic_form import *
from Frontend.enum.enums import CheckType, Gender, FormType, CalibrationStatus
from json import loads

import reflex as rx

class MedicalToolState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    new_medical_tool_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Name",
            required=True,
            form_type=FormType.Input.value
        ),
        FormModel(
            name="code",
            placeholder="Code",
            required=True,
            form_type=FormType.Input.value
        ),
    ]
    medical_tool_form: list[FormModel] =  []

    async def get_data(self):
        response = await api_call.get(API_MEDICAL_TOOL)
        self.raw_data = loads(response.text)["data"]
        self.columns, self.data, dataFrame = converter.to_data_table(self.raw_data)

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.medical_tool_form = [
            FormModel(
                name="name",
                placeholder="Name",
                required=True,
                form_type=FormType.Input.value,
                default_value=self.selected_data["name"]
            ),
            FormModel(
                name="code",
                placeholder="Code",
                required=True,
                form_type=FormType.Input.value,
                default_value=self.selected_data["code"]
            ),
            FormModel(
                name="calibrationDate",
                placeholder="Calibration Date",
                required=True,
                form_type=FormType.Date.value,
                default_value=self.selected_data["calibrationDate"]
            ),
            FormModel(
                name="calibrationStatus",
                placeholder="Calibration Status",
                required=True,
                form_type=FormType.Select.value,
                options=[cs.name for cs in CalibrationStatus],
                default_value=CalibrationStatus(self.selected_data["calibrationStatus"]).name
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
        form_data["calibrationStatus"] = CalibrationStatus[form_data["calibrationStatus"]].value
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_MEDICAL_TOOL}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()
        self.updating = False

    async def add_data(self, form_data: dict):
        print(form_data)
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
        rx.flex(
            rx.dialog.root(
                rx.dialog.trigger(
                    rx.button("Add")
                ),
                rx.dialog.content(
                    rx.dialog.title("Add Medical Tool"),
                    rx.form(
                        rx.flex(
                            rx.foreach(
                                MedicalToolState.new_medical_tool_form,
                                generate_form_field
                            ),
                            spacing="3",
                            direction="column"
                        ),
                        rx.flex(
                            rx.dialog.close(
                                rx.button(
                                    "Cancel",
                                    color_scheme="gray",
                                    variant="soft",
                                ),
                            ),
                            rx.dialog.close(
                                rx.button(
                                    "Add",
                                    type="submit"
                                )
                            ),
                            spacing="3",
                            margin_top="16px",
                            justify="end",
                        ),
                        on_submit=MedicalToolState.add_data,
                        reset_on_submit=True,
                    ),
                ),
            ),
            rx.cond(MedicalToolState.updating,
                rx.dialog.root(
                    rx.dialog.trigger(
                        rx.button(
                            "Update",
                            )
                    ),
                    rx.dialog.content(
                        rx.dialog.title("Update Medical Tool"),
                        rx.form(
                            rx.flex(
                                rx.foreach(
                                    MedicalToolState.medical_tool_form,
                                    generate_form_field
                                ),
                                spacing="3",
                                direction="column"
                            ),
                            rx.flex(
                                rx.dialog.close(
                                    rx.button(
                                        "Cancel",
                                        color_scheme="gray",
                                        variant="soft",
                                    ),
                                ),
                                rx.dialog.close(
                                    rx.button(
                                        "Save",
                                        type="submit"
                                    )
                                ),
                                spacing="3",
                                margin_top="16px",
                                justify="end",
                            ),
                            on_submit=MedicalToolState.update_data,
                            reset_on_submit=True,
                        ),
                    ),
                ),
                rx.button("Update", disabled=True)
            ),
            rx.button(
                "Delete",
                on_click=MedicalToolState.delete_data,
                disabled=~MedicalToolState.updating,
                color_scheme="red"
            ),
            spacing="3",
        ),
        rx.data_editor(
            columns=MedicalToolState.columns,
            data=MedicalToolState.data,
            on_cell_clicked=MedicalToolState.get_selected_data,
            column_select="none",
        ),
        on_mount=MedicalToolState.get_data
    )