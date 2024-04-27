import reflex as rx
from typing import Union

class FormModel(rx.Base):
    name: str
    placeholder: str = ""
    required: bool = False
    form_type: str = "input"
    options: list[str] = []
    default_value: str = ""
    min_value = ""
    max_value = ""
    min_length: int = 0
    max_length: int = 100
    pattern: str = "*"