from Frontend import styles
import reflex as rx
from Frontend.models.services_model import ServicesModel


def multiple_selections(data: list[ServicesModel], callback: rx.event.EventHandler)  -> rx.Component:
    return rx.tabs.root(
        rx.tabs.list(
            rx.foreach(
                data,
                lambda items: rx.tabs.trigger(items.name, value=items.name)
            ),
        ),
        rx.foreach(
            data,
            lambda items: rx.tabs.content(
                rx.foreach(
                    items.services,
                    lambda item: rx.checkbox(
                        item.name,
                        checked=item.selected,
                        on_change=lambda value: callback(item.id, value)
                        )
                ),
                value=items.name
            )
        ),
        orientation="vertical",
    )
