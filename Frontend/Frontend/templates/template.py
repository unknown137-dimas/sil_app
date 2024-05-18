"""Common templates used between pages in the app."""

from __future__ import annotations

from Frontend import styles
from Frontend.components.sidebar import sidebar
from typing import Callable
from Frontend.const.api import API_USER
from Frontend.utilities import api_call

import reflex as rx

# Meta tags for the app.
default_meta = [
    {
        "name": "viewport",
        "content": "width=device-width, shrink-to-fit=no, initial-scale=1",
    },
]


class ThemeState(rx.State):
    """The state for the theme of the app."""

    accent_color: str = "teal"

class AuthState(rx.State):
    userName: str = ""
    token: str = ""
    role: str = ""
    firstName: str = ""
    lastName: str = ""
    decoded_token: dict = {}

    @rx.var
    def FullName(self) -> str:
        return f"{self.firstName} {self.lastName}"

    async def authentication_check(self):
        if self.token == "" and self.userName == "" and self.role == "":
            return rx.redirect("/login")

        error_messages, response_data = await api_call.get(f"{API_USER}/{self.decoded_token['nameid']}")
        if response_data:
            self.firstName = response_data[0]["firstName"]
            self.lastName = response_data[0]["lastName"]

    def logout(self):
        self.reset()
        return rx.redirect("/login")

def template(
    route: str | None = None,
    title: str | None = None,
    image: str | None = None,
    description: str | None = None,
    meta: str | None = None,
    script_tags: list[rx.Component] | None = None,
    on_load: rx.event.EventHandler | list[rx.event.EventHandler] | None = None,
) -> Callable[[Callable[[], rx.Component]], rx.Component]:
    """The template for each page of the app.

    Args:
        route: The route to reach the page.
        title: The title of the page.
        image: The favicon of the page.
        description: The description of the page.
        meta: Additionnal meta to add to the page.
        on_load: The event handler(s) called when the page load.
        script_tags: Scripts to attach to the page.

    Returns:
        The template with the page content.
    """

    def decorator(page_content: Callable[[], rx.Component]) -> rx.Component:
        """The template for each page of the app.

        Args:
            page_content: The content of the page.

        Returns:
            The template with the page content.
        """
        # Get the meta tags for the page.
        all_meta = [*default_meta, *(meta or [])]

        def templated_page():
            return rx.hstack(
                sidebar(),
                rx.vstack(
                    rx.flex(
                        rx.spacer(),
                        rx.flex(
                            rx.avatar(rx.icon("user-round"), size="3", radius="full"),
                            rx.flex(
                                rx.text(AuthState.FullName, weight="bold", size="4"),
                                rx.text(AuthState.role, color_scheme="gray"),
                                direction="column",
                            ),
                            spacing="2",
                            align="center"
                        ),
                    ),
                    rx.box(
                        rx.box(
                            page_content(),
                            **styles.template_content_style,
                        ),
                        **styles.template_page_style,
                    ),
                ),
                align="start",
            )

        on_load = AuthState.authentication_check

        @rx.page(
            route=route,
            title=title,
            image=image,
            description=description,
            meta=all_meta,
            script_tags=script_tags,
            on_load=on_load,
        )
        def theme_wrap():
            return rx.theme(
                templated_page(),
                accent_color=ThemeState.accent_color,
            )

        return theme_wrap

    return decorator
