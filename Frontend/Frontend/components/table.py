from Frontend import styles
from Frontend.templates.template import ThemeState
import reflex as rx

def sort_table(state: rx.State, sort_key: str):
    x = sort_key.split(" ")
    x[0] = x[0].lower()
    sort_key = "".join(x)
    state.dataFrame.sort_values(by=sort_key, inplace=True)
    state.dataFrame["no"] = [i + 1 for i in range(len(state.dataFrame["no"]))]
    state.data = state.dataFrame.values.tolist()

def table(state: rx.State, sorting: list[str] = [], sort_table: rx.event.EventHandler = None) -> rx.Component:
    return rx.cond(
            state.loading,
            rx.center(
                rx.spinner(
                    color=ThemeState.accent_color,
                    thickness=3,
                    size="3",
                ),
            ),
            rx.cond(
                state.data,
                rx.vstack(
                    rx.cond(
                        sorting != [],
                        rx.flex(
                            rx.select(
                                items=sorting,
                                placeholder="Sort By",
                                on_change=sort_table
                            ),
                            spacing="2"
                        ),
                        rx.flex()
                    ),
                    rx.data_editor(
                        columns=state.columns,
                        data=state.data,
                        on_cell_clicked=state.get_selected_data,
                        column_select="none",
                    ),
                ),
                rx.text("No Data"),
            )
        ),