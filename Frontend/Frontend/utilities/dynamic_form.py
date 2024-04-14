import reflex as rx

# TODO
# 1. Design form model to include name, form type(input, select, checkbox, etc), required(true or false)
# 2. Parse from form model to reflex component
class DynamicFormState(rx.State):
    form_data: dict = {}
    form_fields: list[str] = []

    def handle_submit(self, form_data: dict):
        self.form_data = form_data

def dynamic_form(form_fields: list[str]) -> rx.Component:
    DynamicFormState.form_fields += form_fields
    return rx.vstack(
        rx.form(
            rx.vstack(
                rx.foreach(
                    DynamicFormState.form_fields,
                    lambda field: rx.input(
                        placeholder=field,
                        name=field,
                        required=True
                    ),
                ),
                rx.button("Submit", type="submit"),
            ),
            on_submit=DynamicFormState.handle_submit,
            reset_on_submit=True,
        )
    )