from Frontend import styles
from Frontend.templates.template import ThemeState
from Frontend.states.auth_state import AuthState
from Frontend.const import allowed_path
from Frontend.const.api import API_USER_LOGIN
from Frontend.utilities import api_call, token
from Frontend.models.form_model import FormModel
from Frontend.components.dynamic_form import generate_form_field
from Frontend.enum.enums import FormType
from pickle import dumps

import reflex as rx

class LoginState(AuthState):
    error_message: str = ""
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
    def is_error(self) -> bool:
        return self.error_message != ""

    async def login(self, form_data: dict):
        error_messages, response_data = await api_call.post(API_USER_LOGIN, form_data)
        if response_data:
            self.decoded_token = token.decode(response_data[0]["token"])
            if(self.decoded_token != {}):
                self.userName = self.decoded_token["unique_name"]
                self.role = self.decoded_token["role"]
                self.token = response_data[0]["token"]
                return rx.redirect(self.get_allowed_pages[0].route)
        else:
            self.error_message = error_messages[-1]

@rx.page(route="/login", title="Login")
def login() -> rx.Component:
    return rx.theme(
        rx.box(
            rx.vstack(
                rx.heading("Pelayanan Laboratorium RSUD Jogja", weight="bold"),
                rx.card(
                    rx.match(
                        LoginState.is_error,
                        (
                            True,
                            rx.callout(
                                LoginState.error_message,
                                icon="shield-x",
                                color_scheme="red",
                            ),
                        )
                    ),
                    rx.form(
                        rx.vstack(
                            rx.foreach(
                                LoginState.login_form,
                                generate_form_field
                            ),
                            rx.button(
                                "Login",
                                rx.icon("log-in", size=20),
                                type="submit",
                                radius="full"
                            ),
                            align="center"
                        ),
                        on_submit=LoginState.login,
                        reset_on_submit=True,
                    ),
                ),
                spacing="2",
                align="center"
            ),
            display="flex",
            justify_content="center",
            align_items="center",
            height="100vh",
        ),
        accent_color=ThemeState.accent_color
    )