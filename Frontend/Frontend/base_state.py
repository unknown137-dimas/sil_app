import reflex as rx

class BaseState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list = []
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True