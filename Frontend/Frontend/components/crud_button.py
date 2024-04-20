from Frontend import styles
from Frontend.models.form_model import FormModel
from .dynamic_form import generate_form_field
import reflex as rx


def crud_button(title: str, state: rx.State, new_form: list[FormModel], update_form: list[FormModel]) -> rx.Component:
    return rx.flex(
        rx.button(
            rx.icon("rotate-cw"),
            on_click=state.get_data,
        ),
        rx.dialog.root(
            rx.dialog.trigger(
                rx.button("Add")
            ),
            rx.dialog.content(
                rx.dialog.title(f"Add {title}"),
                rx.form.root(
                    rx.flex(
                        rx.foreach(
                            new_form,
                            generate_form_field
                        ),
                        spacing="4",
                        direction="column"
                    ),
                    rx.flex(
                        rx.dialog.close(
                            rx.button(
                                "Cancel",
                                color_scheme="gray",
                                variant="soft",
                            ),
                        ),
                        rx.form.submit(
                            rx.button("Add"),
                            as_child=True
                        ),
                        spacing="3",
                        margin_top="16px",
                        justify="end",
                    ),
                    on_submit=state.add_data,
                    reset_on_submit=True,
                ),
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
                    rx.form.root(
                        rx.flex(
                            rx.foreach(
                                update_form,
                                generate_form_field
                            ),
                            spacing="4",
                            direction="column"
                        ),
                        rx.flex(
                            rx.dialog.close(
                                rx.button(
                                    "Cancel",
                                    color_scheme="gray",
                                    variant="soft",
                                ),
                            ),
                            rx.form.submit(
                                rx.button("Save"),
                                as_child=True
                            ),
                            spacing="3",
                            margin_top="16px",
                            justify="end",
                        ),
                        on_submit=state.update_data,
                        reset_on_submit=True,
                    ),
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