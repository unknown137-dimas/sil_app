from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_REAGEN
from Frontend.const.common_variables import TODAY_DATE_ONLY
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.enum.enums import FormType
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table

import reflex as rx

class ReagenState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    new_reagen_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Name",
            required=True,
            form_type=FormType.Input.value,
            min_length=5
        ),
        FormModel(
            name="code",
            placeholder="Code",
            required=True,
            form_type=FormType.Input.value,
            pattern="[A-Z]{3}-[0-9]{3}"
        ),
        FormModel(
            name="expiredDate",
            placeholder="Expired Date",
            required=True,
            form_type=FormType.Date.value,
            min_value=TODAY_DATE_ONLY
        ),
        FormModel(
            name="stock",
            placeholder="Stock",
            required=True,
            form_type=FormType.Number.value,
            min_value=0,
            max_value=1000
        ),
    ]
    update_reagen_form: list[FormModel] =  []


    async def get_data(self):
        _, self.raw_data = await api_call.get(API_REAGEN)
        self.columns, self.data, dataFrame = converter.to_data_table(self.raw_data)
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_reagen_form = [
                FormModel(
                    name="name",
                    placeholder="Name",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=5,
                    default_value=self.selected_data["name"],
                ),
                FormModel(
                    name="code",
                    placeholder="Code",
                    required=True,
                    form_type=FormType.Input.value,
                    pattern="[A-Z]{3}-[0-9]{3}",
                    default_value=self.selected_data["code"],
                ),
                FormModel(
                    name="expiredDate",
                    placeholder="Expired Date",
                    required=True,
                    form_type=FormType.Date.value,
                    min_value=TODAY_DATE_ONLY,
                    default_value=converter.to_date_input(self.selected_data["expiredDate"]),
                ),
                FormModel(
                    name="stock",
                    placeholder="Stock",
                    required=True,
                    form_type=FormType.Number.value,
                    min_value=0,
                    max_value=1000,
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
        form_data["calibrationNote"] = ""
        await api_call.post(
            API_REAGEN,
            payload=form_data
        )
        await self.get_data()

    async def delete_data(self):
        await api_call.delete(f"{API_REAGEN}/{self.selected_data['id']}")
        await self.get_data()

@template(route="/reagen", title="Reagen", image="/dna.svg")
def reagen() -> rx.Component:
    return rx.vstack(
        crud_button(
            "Reagen",
            ReagenState,
            ReagenState.new_reagen_form,
            ReagenState.update_reagen_form,
        ),
        table(ReagenState),
        on_mount=ReagenState.get_data
    )