from Frontend import styles
from Frontend.templates import template
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.const.api import API_ROLE
from json import loads
from pandas import DataFrame

import reflex as rx

class RoleState(rx.State):
    columns: list
    data: list
    rawData: list
    selectedRow: int

    async def getRole(self):
        response = await api_call.get(API_ROLE)
        self.rawData = loads(response.text)["data"]
        self.columns, self.data = converter.to_data_table(self.rawData)

    def getData(self, pos):
        _, self.selectedRow = pos
    
    def printData(self):
        print(self.rawData[self.selectedRow])


@template(route="/role", title="Role", image="/github.svg")
def role() -> rx.Component:
    return rx.vstack(
        rx.context_menu.root(
            rx.context_menu.trigger(
                rx.card(
                    rx.data_editor(
                        columns=RoleState.columns,
                        data=RoleState.data,
                        on_cell_context_menu=RoleState.getData,
                        column_select="none",
                    )
                )
            ),
            rx.context_menu.content(
                rx.context_menu.item("Edit",
                on_click=RoleState.printData),
                rx.context_menu.separator(),
                rx.context_menu.item(
                    "Delete", color="red",
                ),
            ),
        ),
        on_mount=RoleState.getRole
    )
