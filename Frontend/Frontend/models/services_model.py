import reflex as rx
from Frontend.enum.enums import CheckType


class ServiceModel(rx.Base):
    id: str
    name: str
    normal_value_type: CheckType
    selected: bool = False

class ServicesModel(rx.Base):
    name: str
    services: list[ServiceModel]
