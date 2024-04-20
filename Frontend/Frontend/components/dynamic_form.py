from Frontend.enum.enums import FormType
from Frontend.models.form_model import FormModel
from Frontend.utilities.converter import to_title_case
import reflex as rx


def generate_form_field(field: FormModel):
    return rx.form.field(
        rx.flex(
            rx.form.label(
                field.placeholder,
            ),
            rx.cond(
                field.form_type == FormType.Select.value,
                rx.select(
                    name=field.name,
                    placeholder=field.placeholder,
                    required=field.required,
                    items=field.options,
                    default_value=field.default_value,
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
                        pattern=field.pattern
                    ),
                    as_child=True
                )
            ),
            rx.form.message(
                f"A valid {field.placeholder} is required",
                match="valueMissing",
                color="var(--red-11)",
            ),
            direction="column",
            spacing="2"
        ),
        name=field.name,
    )