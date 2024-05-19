"""Sidebar component for the app."""

from Frontend import styles
from Frontend.states.auth_state import AuthState
from Frontend.const import allowed_path
from Frontend.enum.enums import UserRoles

import reflex as rx


def sidebar_header() -> rx.Component:
    """Sidebar header.

    Returns:
        The sidebar header component.
    """
    return rx.hstack(
        rx.icon("hospital", size=50),
        rx.spacer(),
        rx.heading(
            "Pelayanan Laboratorium",
            weight="bold",
        ),
        align="center",
        width="100%",
        border_bottom=styles.border,
        padding="1em",
    )


def sidebar_footer() -> rx.Component:
    """Sidebar footer.

    Returns:
        The sidebar footer component.
    """
    return rx.hstack(
        rx.spacer(),
        rx.button(
            rx.icon("log-out", size=20),
            "Logout",
            on_click=AuthState.logout,
            radius="full"
        ),
        width="100%",
        border_top=styles.border,
        padding="1em",
    )


def sidebar_item(text: str, icon: str, url: str) -> rx.Component:
    """Sidebar item.

    Args:
        text: The text of the item.
        icon: The icon of the item.
        url: The URL of the item.

    Returns:
        rx.Component: The sidebar item component.
    """
    # Whether the item is active.
    active = (rx.State.router.page.path == url) | (
        (rx.State.router.page.path == "/") & text == "Home"
    )

    return rx.link(
        rx.hstack(
            rx.image(
                src=icon,
                height="2.5em",
                padding="0.5em",
            ),
            rx.text(
                text,
            ),
            bg=rx.cond(
                active,
                styles.accent_color,
                "transparent",
            ),
            color=rx.cond(
                active,
                styles.accent_text_color,
                styles.text_color,
            ),
            align="center",
            border_radius=styles.border_radius,
            box_shadow=styles.box_shadow,
            width="100%",
            padding_x="1em",
        ),
        href=url,
        width="100%",
    )


def sidebar() -> rx.Component:
    """The sidebar.

    Returns:
        The sidebar component.
    """

    pages = AuthState.get_allowed_pages

    return rx.box(
        rx.vstack(
            sidebar_header(),
            rx.vstack(
                rx.foreach(
                    pages,
                    lambda page:
                    sidebar_item(
                        text=page.title,
                        icon=page.image,
                        url=page.route,
                    )
                ),
                width="100%",
                overflow_y="auto",
                align_items="flex-start",
                padding="1em",
            ),
            rx.spacer(),
            sidebar_footer(),
            height="100dvh",
        ),
        display=["none", "none", "block"],
        min_width=styles.sidebar_width,
        height="100%",
        position="sticky",
        top="0px",
        border_right=styles.border,
    )
