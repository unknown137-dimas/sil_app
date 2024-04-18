from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_REAGEN
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.enum.enums import FormType, CalibrationStatus
from json import loads
from Frontend.components.crud_button import crud_button

import reflex as rx

class ReagenState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    new_reagen_form: list[FormModel] = [
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
        FormModel(
            name="expiredDate",
            placeholder="Expired Date",
            required=True,
            form_type=FormType.Date.value
        ),
        FormModel(
            name="stock",
            placeholder="Stock",
            required=True,
            form_type=FormType.Input.value
        ),
    ]
    update_reagen_form: list[FormModel] =  []
    date_test: str

    async def get_data(self):
        response = await api_call.get(API_REAGEN)
        self.raw_data = loads(response.text)["data"]
        self.columns, self.data, dataFrame = converter.to_data_table(self.raw_data)

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.update_reagen_form = [
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
                name="expiredDate",
                placeholder="Expired Date",
                required=True,
                form_type=FormType.Date.value,
                default_value=converter.to_date_only(self.selected_data["expiredDate"])
            ),
            FormModel(
                name="stock",
                placeholder="Stock",
                required=True,
                form_type=FormType.Input.value,
                default_value=self.selected_data["stock"]
            ),
        ]
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_REAGEN}/{self.selected_data['id']}",
            payload=dict(self.selected_data)
        )
        await self.get_data()
        self.updating = False

    async def add_data(self, form_data: dict):
        print(form_data)
        form_data["calibrationNote"] = ""
        await api_call.post(
            API_REAGEN,
            payload=form_data
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_REAGEN}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/reagen", title="Reagen")
def reagen() -> rx.Component:
    return rx.vstack(
        crud_button(
            "Reagen",
            ReagenState,
            ReagenState.new_reagen_form,
            ReagenState.update_reagen_form,
        ),
        rx.data_editor(
            columns=ReagenState.columns,
            data=ReagenState.data,
            on_cell_clicked=ReagenState.get_selected_data,
            column_select="none",
        ),
        on_mount=ReagenState.get_data
    )