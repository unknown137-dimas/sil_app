from Frontend.enum.enums import FormType
from Frontend.models.form_model import FormModel
from Frontend.utilities.converter import to_title_case
import reflex as rx


def generate_form_field(field: FormModel) -> rx.Component:
    return rx.form.field(
        rx.flex(
            rx.form.label(
                field.placeholder,
            ),
            rx.match(
                field.form_type,
                (
                    FormType.Select.value,
                    rx.select(
                        name=field.name,
                        placeholder=field.placeholder,
                        required=field.required,
                        items=field.options,
                        default_value=field.default_value,
                    )
                ),
                (
                    FormType.Text.value,
                    rx.text_area(
                        name=field.name,
                        placeholder=field.placeholder,
                        required=field.required,
                        min_length=field.min_length,
                    )
                ),
                rx.form.control(
                    rx.input(
                        name=field.name,
                        placeholder=field.placeholder,
                        required=field.required,
                        type=field.form_type,
                        default_value=field.default_value,
                        min=field.min_value,
                        max=field.max_value,
                        min_length=field.min_length,
                        max_length=field.max_length,
                        pattern=field.pattern
                    ),
                    as_child=True
                )
            ),
            rx.cond(
                field.pattern != "*",
                rx.form.message(
                    f"Pattern mismatch",
                    match="patternMismatch",
                    color="var(--red-11)",
                ),
            ),
            rx.cond(
                field.min_length > 0,
                rx.form.message(
                    f"{field.placeholder} must more than or equal to {field.min_length}",
                    match="tooShort",
                    color="var(--red-11)",
                ),
            ),
            rx.cond(
                field.max_length > 0,
                rx.form.message(
                    f"{field.placeholder} must lest than {field.max_length}",
                    match="tooLong",
                    color="var(--red-11)",
                ),
            ),
            rx.cond(
                field.form_type == FormType.Number.value,
                rx.fragment(
                    rx.form.message(
                        f"Value must not more than {field.max_value}",
                        match="rangeOverflow",
                        color="var(--red-11)",
                    ),
                    rx.form.message(
                        f"Value must more than {field.min_value}",
                        match="rangeUnderflow",
                        color="var(--red-11)",
                    ),
                )
            ),
            rx.form.message(
                f"{field.placeholder} is required",
                match="valueMissing",
                color="var(--red-11)",
            ),
            direction="column",
        ),
        name=field.name,
    )

def dynamic_form(form_type: str, form: list[FormModel], submit_callback: rx.event.EventHandler) -> rx.Component:
    return rx.form.root(
        rx.flex(
            rx.foreach(
                form,
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
                    radius="full"
                ),
            ),
            rx.form.submit(
                rx.button(
                    form_type,
                    radius="full"
                ),
                as_child=True
            ),
            spacing="3",
            margin_top="16px",
            justify="end",
        ),
        on_submit=submit_callback,
    ),

def dynamic_form_dialog(disabled: bool, title: str, form_type: str, form: list[FormModel], callback: rx.event.EventHandler) -> rx.Component:
    return rx.cond(
        disabled,
        rx.button(
            rx.match(
                form_type,
                ("Add", rx.icon("circle-plus", size=20)),
                ("Update", rx.icon("pencil", size=20)),
            ),
            form_type,
            disabled=True,
            radius="full"
        ),
        rx.dialog.root(
            rx.dialog.trigger(
                rx.button(
                    rx.match(
                        form_type,
                        ("Add", rx.icon("circle-plus", size=20)),
                        ("Update", rx.icon("pencil", size=20)),
                    ),
                    form_type,
                    radius="full"
                )
            ),
            rx.dialog.content(
                rx.dialog.title(title),
                dynamic_form(form_type, form, callback),
            ),
        )
    )