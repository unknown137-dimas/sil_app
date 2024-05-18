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

    async def login(self, form_data: dict):
        error_messages, response_data = await api_call.post(API_USER_LOGIN, form_data)
        if response_data:
            self.decoded_token = token.decode(response_data[0]["token"])
            if(self.decoded_token != {}):
                self.userName = self.decoded_token["unique_name"]
                self.role = self.decoded_token["role"]
                self.token = response_data[0]["token"]
                return rx.redirect(allowed_path.path_config[self.role][0])
        else:
            for error in error_messages:
                print(error)
                rx.window_alert(error)

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
                        rx.button("Login", type="submit"),
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