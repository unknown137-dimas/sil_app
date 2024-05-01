import reflex as rx


class ServiceModel(rx.Base):
    id: str
    name: str
    selected: bool = False

class ServicesModel(rx.Base):
    name: str
    services: list[ServiceModel]
