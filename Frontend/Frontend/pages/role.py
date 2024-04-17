from Frontend import styles
from Frontend.templates import template
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.const.api import API_ROLE
from json import loads
from pandas import DataFrame

import reflex as rx

class RoleState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False

    async def get_data(self):
        response = await api_call.get(API_ROLE)
        self.raw_data = loads(response.text)["data"]
        self.columns, self.data, _ = converter.to_data_table(self.raw_data)

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_ROLE}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()

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
        rx.dialog.root(
            rx.dialog.trigger(
                rx.button("Add")
            ),
            rx.dialog.content(
                rx.dialog.title("Add Role"),
                rx.form(
                    rx.input(
                        placeholder="Enter role name",
                        name="name"
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
                    on_submit=RoleState.add_data,
                    reset_on_submit=True,
                ),
            ),
        ),
        rx.cond(
            RoleState.updating,
            rx.flex(
                rx.dialog.root(
                    rx.dialog.trigger(
                        rx.button("Update")
                    ),
                    rx.dialog.content(
                        rx.dialog.title("Update Role"),
                        rx.form(
                            rx.input(
                                default_value=RoleState.selected_data["name"],
                                placeholder="Enter role name",
                                name="name"
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
                            on_submit=RoleState.update_data,
                            reset_on_submit=True,
                        ),
                    ),
                ),
                rx.button("Delete", on_click=RoleState.delete_data),
                spacing="3",
            )
        ),
        rx.card(
            rx.data_editor(
                columns=RoleState.columns,
                data=RoleState.data,
                on_cell_clicked=RoleState.get_selected_data,
                column_select="none",
            ),
        ),
        on_mount=RoleState.get_data
    )

