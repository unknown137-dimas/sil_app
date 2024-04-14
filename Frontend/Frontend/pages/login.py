from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_USER_LOGIN
from Frontend.utilities import api_call
from json import loads

import reflex as rx

class LoginState(rx.State):
    userName: str
    token: str
    role: str

    async def login(self, form_data: dict):
        response = await api_call.post(API_USER_LOGIN, form_data)
        response_data = loads(response.text)["data"]
        if(response_data):
            self.token = response_data[0]["token"]
        else:
            error_message = loads(response.text)["messages"]

@template(route="/login", title="Login", image="/github.svg")
def login() -> rx.Component:
    return rx.card(
        rx.vstack(
        rx.form(
            rx.vstack(
                rx.input(
                    placeholder="Username",
                    name="userName",
                    required=True
                ),
                rx.input(
                    placeholder="Password",
                    name="password",
                    required=True
                ),
                rx.button("Submit", type="submit"),
            ),
            on_submit=LoginState.login,
            reset_on_submit=True,
        )
    )
    )