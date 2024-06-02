from Frontend import styles
from Frontend.models.form_model import FormModel
from .dynamic_form import dynamic_form_dialog
import reflex as rx


def delete_button(disabled: bool, callback: rx.event.EventHandler) -> rx.Component:
    return rx.cond(
        disabled,
        rx.button(
            rx.icon("trash-2", size=20),
            "Delete",
            disabled=disabled,
            color_scheme="red",
            radius="full"
        ),
        rx.alert_dialog.root(
            rx.alert_dialog.trigger(
                rx.button(
                    rx.icon("trash-2", size=20),
                    "Delete",
                    color_scheme="red",
                    radius="full"
                )
            ),
            rx.alert_dialog.content(
                rx.alert_dialog.title("Delete Item"),
                rx.alert_dialog.description(
                    "Are you sure want to delete this item?",
                ),
                rx.flex(
                    rx.alert_dialog.cancel(
                        rx.button(
                            "Cancel",
                            variant="soft",
                            color_scheme="gray",
                            radius="full"
                        ),
                    ),
                    rx.alert_dialog.action(
                        rx.button(
                            "Delete",
                            on_click=callback,
                            color_scheme="red",
                            radius="full"
                        ),
                    ),
                    spacing="3",
                    margin_top="16px",
                    justify="end",
                ),
            ),
        )
    )
    

def crud_button(title: str, state: rx.State, new_form: list[FormModel], update_form: list[FormModel], disable_add_button: bool = False) -> rx.Component:
    return rx.flex(
        dynamic_form_dialog(disable_add_button, f"Add {title}", "Add", new_form, state.add_data),
        dynamic_form_dialog(~state.updating, f"Update {title}", "Update", update_form, state.update_data),
        delete_button(~state.updating, state.delete_data),
        spacing="3",
    ),