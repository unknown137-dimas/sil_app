from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_ROLE
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table
from Frontend.enum.enums import FormType
from json import loads
from pandas import DataFrame

import reflex as rx

class RoleState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    new_role_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Role Name",
            required=True,
            form_type=FormType.Input.value,
            min_length=5,
        ),
    ]
    update_role_form: list[FormModel] =  []

    async def get_data(self):
        self.clear_selected_data()
        response = await api_call.get(API_ROLE)
        self.raw_data = loads(response.text)["data"]
        self.columns, self.data, _ = converter.to_data_table(self.raw_data)
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_role_form = [
                FormModel(
                    name="name",
                    placeholder="Role Name",
                    required=True,
                    form_type=FormType.Input.value,
                    min_length=5,
                    default_value=self.selected_data["name"]
                ),
            ]
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_ROLE}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()
        self.updating = False

    async def delete_data(self):
        await api_call.delete(f"{API_ROLE}/{self.selected_data['id']}")
        await self.get_data()

    async def add_data(self, form_data: dict):
        await api_call.post(
            API_ROLE,
            payload=form_data
        )
        await self.get_data()
        
@template(route="/role", title="Role", image="/github.svg")
def role() -> rx.Component:
    return rx.vstack(
        crud_button(
            "Role",
            RoleState,
            RoleState.new_role_form,
            RoleState.update_role_form,
        ),
        table(RoleState),
        on_mount=RoleState.get_data
    )

