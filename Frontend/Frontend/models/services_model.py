import reflex as rx
from Frontend.enum.enums import CheckType, Gender


class ServiceModel(rx.Base):
    id: str
    name: str
    selected: bool = False

class ServicesModel(rx.Base):
    name: str
    services: list[ServiceModel]

class CheckServiceModel(ServiceModel):
    unit: str
    gender: Gender
    min_normal_value: int
    max_normal_value: int
    normal_value: str
    normal_value_type: CheckType

class CheckServicesModel(rx.Base):
    name: str
    services: list[CheckServiceModel]

class SampleServiceModel(ServiceModel):
    pass

class SampleServicesModel(ServicesModel):
    pass




