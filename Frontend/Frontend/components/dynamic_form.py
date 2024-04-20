from Frontend.enum.enums import FormType
from Frontend.models.form_model import FormModel
import reflex as rx


def generate_form_field(field: FormModel):
    return rx.flex(
        rx.text(
            field.placeholder,
            align="center",
        ),
        rx.cond(
            field.form_type == FormType.Select.value,
            rx.select(
                name=field.name,
                placeholder=field.placeholder,
                required=field.required,
                items=field.options,
                default_value=field.default_value
            ),
            rx.input(
                name=field.name,
                placeholder=field.placeholder,
                required=field.required,
                type=field.form_type,
                default_value=field.default_value,
                min=field.min_value,
                max=field.max_value,
                min_length=field.min_length,
                pattern=field.pattern
            )
        ),
        spacing="3",
    )