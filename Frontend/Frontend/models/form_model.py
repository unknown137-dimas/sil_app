import reflex as rx

class FormModel(rx.Base):
    name: str
    placeholder: str
    required: bool
    form_type: str
    options: list[str] = []
    default_value: str = ""