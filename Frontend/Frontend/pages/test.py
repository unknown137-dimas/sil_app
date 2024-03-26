"""The home page of the app."""

from Frontend import styles
from Frontend.templates import template

import reflex as rx


@template(route="/test", title="Test", image="/github.svg")
def test() -> rx.Component:
    """The home page.

    Returns:
        The UI for the home page.
    """
    with open("README.md", encoding="utf-8") as readme:
        content = readme.read()
    return rx.markdown(content, component_map=styles.markdown_style)
