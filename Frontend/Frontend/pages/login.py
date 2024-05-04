from Frontend import styles
from Frontend.templates import ThemeState
from Frontend.const.api import API_USER_LOGIN
from Frontend.utilities import api_call, token
from Frontend.models.form_model import FormModel
from Frontend.components.dynamic_form import generate_form_field
from Frontend.enum.enums import FormType
from pickle import dumps

import reflex as rx

class LoginState(rx.State):
    userName: str = ""
    token: str = ""
    decoded_token: dict = {}
    role: str = ""
    login_form: list[FormModel] = [
        FormModel(
            name="userName",
            placeholder="Username",
            required=True,
            form_type=FormType.Input.value
        ),
        FormModel(
            name="password",
            placeholder="Password",
            required=True,
            form_type=FormType.Password.value
        ),
    ]

    @rx.var
    def is_authenticated(self) -> bool:
        return token != ""

    async def login(self, form_data: dict):
        error_messages, response_data = await api_call.post(API_USER_LOGIN, form_data)
        if response_data:
            self.decoded_token = token.decode(response_data[0]["token"])
            if(self.decoded_token != {}):
                self.userName = self.decoded_token["unique_name"]
                self.role = self.decoded_token["role"]
                self.token = response_data[0]["token"]
                rx.redirect("/")
        else:
            for error in error_messages:
                print(error)
                rx.window_alert(error)

    def logout(self):
        rx.window_alert(self.token)
        self.reset()
        rx.redirect("/login")


@rx.page(route="/login", title="Login", image="/github.svg")
def login() -> rx.Component:
    return rx.theme(
        rx.flex(
            rx.card(
                rx.form(
                    rx.vstack(
                        rx.foreach(
                            LoginState.login_form,
                            generate_form_field
                        ),
                        rx.button("Submit", type="submit"),
                    ),
                    on_submit=LoginState.login,
                    reset_on_submit=True,
                ),
            ),
            justify="center",
            align="center"
        ),
        accent_color=ThemeState.accent_color
    )