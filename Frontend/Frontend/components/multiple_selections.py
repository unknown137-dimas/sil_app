from Frontend import styles
import reflex as rx
from Frontend.models.services_model import ServicesModel, ServiceModel


def multiple_selections(data: list[ServicesModel], callback: rx.event.EventHandler)  -> rx.Component:
    return rx.fragment(
        rx.flex(
            rx.text("Selected Item: "),
            rx.foreach(
                data,
                lambda item: rx.foreach(
                    item.services,
                    lambda service:
                    rx.match(
                        service.selected,
                        (
                            True,
                            rx.badge(
                                service.name,
                                variant="solid",
                                size="2",
                                radius="full"
                            ),
                        )
                    )
                )
            ),
            spacing="1"
        ),
        rx.tabs.root(
            rx.tabs.list(
                rx.foreach(
                    data,
                    lambda items: rx.tabs.trigger(items.name, value=items.name)
                ),
            ),
            rx.foreach(
                data,
                lambda items: rx.tabs.content(
                    rx.grid(
                        rx.foreach(
                            items.services,
                            lambda item: rx.checkbox(
                                item.name,
                                checked=item.selected,
                                on_change=lambda value: callback(item.id, value)
                                )
                        ),
                        rows="3",
                        flow="column",
                        justify="between",
                        spacing="2",
                        padding="10px",
                        spacing_x="6"
                    ),
                    value=items.name
                )
            ),
            default_value=data[0].name,
            orientation="vertical",
        )
    )

def multiple_selections_checkbox(data: list[ServiceModel], callback: rx.event.EventHandler)  -> rx.Component:
    return rx.fragment(
        rx.flex(
            rx.text("Selected Item: "),
            rx.foreach(
                data,
                lambda service:
                rx.match(
                    service.selected,
                    (
                        True,
                        rx.badge(
                            service.name,
                            variant="solid",
                            size="2",
                            radius="full"
                        ),
                    )
                )
            ),
            spacing="1"
        ),
        rx.grid(
            rx.foreach(
                data,
                lambda item: rx.checkbox(
                    item.name,
                    checked=item.selected,
                    on_change=lambda value: callback(item.id, value)
                    )
            ),
            rows="3",
            flow="column",
            justify="between",
            spacing="2",
            spacing_x="6"
        )
    )
