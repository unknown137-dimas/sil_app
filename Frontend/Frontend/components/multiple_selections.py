from Frontend import styles
import reflex as rx
from Frontend.models.services_model import ServicesModel


def multiple_selections(data: list[ServicesModel], callback: rx.event.EventHandler)  -> rx.Component:
    return rx.fragment(
        rx.flex(
            rx.foreach(
                data,
                lambda item: rx.foreach(
                    item.services,
                    lambda service:
                    rx.match(
                        service.selected,
                        (True, rx.badge(service.name))
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
                    rx.flex(
                        rx.foreach(
                            items.services,
                            lambda item: rx.checkbox(
                                item.name,
                                checked=item.selected,
                                on_change=lambda value: callback(item.id, value)
                                )
                        ),
                        direction="column",
                        spacing="2",
                        padding="10px"
                    ),
                    value=items.name
                )
            ),
            orientation="vertical",
        )
    )
