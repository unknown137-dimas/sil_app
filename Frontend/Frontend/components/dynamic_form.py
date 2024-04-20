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
                    f"{field.placeholder} must more than {field.min_length}",
                    match="tooShort",
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