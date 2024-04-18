from Frontend.enum.enums import FormType
import reflex as rx

# TODO
# 1. Design form model to include name, form type(input, select, checkbox, etc), required(true or false)
# 2. Parse from form model to reflex component

class FormModel(rx.Base):
    name: str
    placeholder: str
    required: bool
    form_type: str
    options: list[str] = []
    default_value: str = ""

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
                default_value=field.default_value
            )
        ),
        spacing="3",
    )