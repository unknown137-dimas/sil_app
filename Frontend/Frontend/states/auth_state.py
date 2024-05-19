from Frontend.const import allowed_path
from Frontend.enum.enums import UserRoles
from Frontend.models.page_model import PageModel
from Frontend.const.api import API_USER
from Frontend.utilities import api_call

import reflex as rx
from reflex.page import get_decorated_pages


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

    @rx.var
    def get_allowed_pages(self) -> list[PageModel]:
        if(self.role != ""):
            if(self.role != UserRoles.Admin.value):
                return [
                    PageModel(
                        title=page.get("title", page["route"].strip("/").capitalize()),
                        image=page.get("image", "/github.svg"),
                        route=page["route"]
                    ) for page in get_decorated_pages() if page["route"] in allowed_path.path_config[self.role]]
            return [
                PageModel(
                    title=page.get("title", page["route"].strip("/").capitalize()),
                    image=page.get("image", "/github.svg"),
                    route=page["route"]
                ) for page in get_decorated_pages() if page["route"] != "/login"]

    async def authentication_check(self):
        if self.token == "" and self.userName == "" and self.role == "":
            return rx.redirect("/login")

        if self.router.page.path not in [page.route for page in self.get_allowed_pages]:
            return rx.redirect(self.get_allowed_pages[0].route)

        error_messages, response_data = await api_call.get(f"{API_USER}/{self.decoded_token['nameid']}")
        if response_data:
            self.firstName = response_data[0]["firstName"]
            self.lastName = response_data[0]["lastName"]

    def logout(self):
        self.reset()
        return rx.redirect("/login")