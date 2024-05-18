from Frontend.const import allowed_path
from Frontend.enum.enums import UserRoles
from Frontend.const.api import API_USER
from Frontend.utilities import api_call

import reflex as rx


class AuthState(rx.State):
    userName: str = ""
    token: str = ""
    role: str = ""
    firstName: str = ""
    lastName: str = ""
    decoded_token: dict = {}

    @rx.var
    def full_name(self) -> str:
        return f"{self.firstName} {self.lastName}"

    @rx.var
    def is_admin(self) -> bool:
        return self.role == UserRoles.Admin.value

    @rx.var
    def is_regis_staff(self) -> bool:
        return self.role == UserRoles.Regis.value

    @rx.var
    def is_sampling_staff(self) -> bool:
        return self.role == UserRoles.Sampling.value

    @rx.var
    def is_lab_staff(self) -> bool:
        return self.role == UserRoles.Lab.value


    async def authentication_check(self):
        if self.token == "" and self.userName == "" and self.role == "":
            return rx.redirect("/login")

        if self.router.page.path not in allowed_path.path_config[self.role]:
            return rx.redirect(allowed_path.path_config[self.role][0])

        error_messages, response_data = await api_call.get(f"{API_USER}/{self.decoded_token['nameid']}")
        if response_data:
            self.firstName = response_data[0]["firstName"]
            self.lastName = response_data[0]["lastName"]

    def logout(self):
        self.reset()
        return rx.redirect("/login")