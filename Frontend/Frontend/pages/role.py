from Frontend import styles
from Frontend.templates import template
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.const.api import API_ROLE
from json import loads
from pandas import DataFrame

import reflex as rx

class RoleState(rx.State):
    roleData: DataFrame

    async def getRole(self):
        response = await api_call.get(API_ROLE)
        converter.to_data_table(loads(response.text)["data"])
        self.roleData = converter.to_data_table(loads(response.text)["data"])


@template(route="/role", title="Role", image="/github.svg")
def role() -> rx.Component:
    return rx.vstack(
        rx.data_table(
            data=RoleState.roleData,
            on_mount=RoleState.getRole(),
        )
    )
