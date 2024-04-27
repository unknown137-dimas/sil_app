from Frontend import styles
from Frontend.templates.template import ThemeState
import reflex as rx

def table(state: rx.State) -> rx.Component:
    return rx.cond(
            state.loading,
            rx.center(
                rx.chakra.spinner(
                    color=ThemeState.accent_color,
                    thickness=3,
                    size="lg",
                ),
            ),
            rx.cond(
                state.data,
                rx.data_editor(
                    columns=state.columns,
                    data=state.data,
                    on_cell_clicked=state.get_selected_data,
                    column_select="none",
                ),
                rx.text("No Data"),
            )
        ),