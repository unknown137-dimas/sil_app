from Frontend import styles
from Frontend.models.form_model import FormModel
from .dynamic_form import dynamic_form
import reflex as rx


def crud_button(title: str, state: rx.State, new_form: list[FormModel], update_form: list[FormModel]) -> rx.Component:
    return rx.flex(
        rx.dialog.root(
            rx.dialog.trigger(
                rx.button("Add")
            ),
            rx.dialog.content(
                rx.dialog.title(f"Add {title}"),
                dynamic_form("Add", new_form, state.add_data),
            ),
        ),
        rx.cond(
            state.updating,
            rx.dialog.root(
                rx.dialog.trigger(
                    rx.button(
                        "Update",
                        )
                ),
                rx.dialog.content(
                    rx.dialog.title(f"Update {title}"),
                    dynamic_form("Update", update_form, state.update_data),
                ),
            ),
            rx.button("Update", disabled=True)
        ),
        rx.button(
            "Delete",
            on_click=state.delete_data,
            disabled=~state.updating,
            color_scheme="red"
        ),
        spacing="3",
    ),