from Frontend import styles
from Frontend.models.form_model import FormModel
from .dynamic_form import dynamic_form_dialog
import reflex as rx


def delete_button(disabled: bool, callback: rx.event.EventHandler) -> rx.Component:
    return rx.button(
        "Delete",
        on_click=callback,
        disabled=disabled,
        color_scheme="red"
    )

def crud_button(title: str, state: rx.State, new_form: list[FormModel], update_form: list[FormModel]) -> rx.Component:
    return rx.flex(
        dynamic_form_dialog(True, f"Add {title}", "Add", new_form, state.add_data),
        dynamic_form_dialog(state.updating, f"Update {title}", "Update", update_form, state.update_data),
        delete_button(~state.updating, state.delete_data),
        spacing="3",
    ),