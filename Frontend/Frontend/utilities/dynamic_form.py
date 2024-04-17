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

# class DynamicFormState(rx.State):
#     form_data: dict = {}
#     form_fields: list[FormModel]

#     def handle_submit(self, form_data: dict):
#         self.form_data = form_data

#     def add_form_field(self, name, placeholder, form_type, required, options = []):
#         self.form_fields.append(
#             FormModel(
#                 name = name,
#                 placeholder = placeholder,
#                 form_type = form_type,
#                 required = required,
#                 options = options
#             )
#         )
#         yield

def generate_form_field(field: FormModel):
    return rx.cond(
        field.form_type == FormType.SELECT.value,
        rx.select(
            name=field.name,
            placeholder=field.placeholder,
            required=field.required,
            items=field.options
        ),
        rx.input(
            name=field.name,
            placeholder=field.placeholder,
            required=field.required,
            type=field.form_type
        )
    )

# def dynamic_form(handler) -> rx.Component:
#     return rx.form(
#         rx.vstack(
#             rx.foreach(
#                 DynamicFormState.form_fields,
#                 generate
#             ),
#             rx.button("Submit", type="submit"),
#         ),
#         on_submit=handler,
#         reset_on_submit=True,
#     )