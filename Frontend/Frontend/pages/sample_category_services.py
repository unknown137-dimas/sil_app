from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_SAMPLE_CATEGORY, API_SAMPLE_SERVICE
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table
from Frontend.enum.enums import FormType, Gender, ValueType
from json import loads
from pandas import DataFrame
from Frontend.base_state import BaseState

import reflex as rx


class SampleCategoryState(BaseState):
    new_sample_category_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Sample Category Name",
            required=True,
            form_type=FormType.Input.value
        ),
    ]
    update_sample_category_form: list[FormModel] =  []

    async def get_data(self):
        response = await api_call.get(API_SAMPLE_CATEGORY)
        self.raw_data = loads(response.text)["data"]
        if self.raw_data:
            self.columns, self.data, _ = converter.to_data_table(self.raw_data, ["sampleServices"])
            sample_service_state = await self.get_state(SampleServicesState)
            await sample_service_state.get_data()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.update_sample_category_form = [
            FormModel(
                name="name",
                placeholder="Category Name",
                required=True,
                form_type=FormType.Input.value,
                default_value=self.selected_data["name"]
            ),
        ]
    
    async def update_data(self, form_data: dict):
        self.selected_data.update(form_data)
        del self.selected_data["sampleServices"]
        await api_call.post(
            f"{API_SAMPLE_CATEGORY}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()
        self.updating = False

    async def delete_data(self):
        await api_call.delete(f"{API_SAMPLE_CATEGORY}/{self.selected_data['id']}")
        await self.get_data()

    async def add_data(self, form_data: dict):
        await api_call.post(
            API_SAMPLE_CATEGORY,
            payload=form_data
        )
        await self.get_data()

class SampleServicesState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    sample_categories_raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    update_sample_services_form: list[FormModel] =  []

    @rx.var
    def new_sample_services_form(self) -> list[FormModel]:
        return [
            FormModel(
                name="name",
                placeholder="Sample Services Name",
                required=True,
                form_type=FormType.Input.value
            ),
            FormModel(
                name="sampleCategoryId",
                placeholder="Sample Category",
                required=True,
                form_type=FormType.Select.value,
                options=self.sample_categories_name
            ),
        ]

    @rx.var
    def sample_categories_name(self) -> list[str]:
        return [cc["name"] for cc in self.sample_categories_raw_data]

    def get_sample_category_name_by_id(self, category_id: str) -> str:
        for cc in self.sample_categories_raw_data:
            if cc["id"] == category_id:
                return cc["name"]

    def get_sample_category_id_by_name(self, category_name: str) -> str:
        for cc in self.sample_categories_raw_data:
            if cc["name"] == category_name:
                return cc["id"]

    async def get_data(self):
        sample_categories = await self.get_state(SampleCategoryState)
        self.sample_categories_raw_data = sample_categories.raw_data
        response = await api_call.get(API_SAMPLE_SERVICE)
        self.raw_data = loads(response.text)["data"]
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "samplecategory" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_sample_category_name_by_id(data))

            self.data = dataFrame.values.tolist()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.update_sample_services_form = [
            FormModel(
                name="name",
                placeholder="Sample Services Name",
                required=True,
                form_type=FormType.Input.value,
                default_value=self.selected_data["name"]
            ),
            FormModel(
                name="sampleCategoryId",
                placeholder="Sample Category",
                required=True,
                form_type=FormType.Select.value,
                options=self.sample_categories_name,
                default_value=self.get_sample_category_name_by_id(self.selected_data["sampleCategoryId"])
            ),
        ]
    
    async def update_data(self, form_data: dict):
        form_data["sampleCategoryId"] = self.get_sample_category_id_by_name(form_data["sampleCategoryId"])
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_SAMPLE_SERVICE}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()
        self.updating = False

    async def delete_data(self):
        await api_call.delete(f"{API_SAMPLE_SERVICE}/{self.selected_data['id']}")
        await self.get_data()

    async def add_data(self, form_data: dict):
        form_data["sampleCategoryId"] = self.get_sample_category_id_by_name(form_data["sampleCategoryId"])
        await api_call.post(
            API_SAMPLE_SERVICE,
            payload=form_data
        )
        await self.get_data()

        
@template(route="/sample_category_services", title="Sample Category & Services", image="/github.svg")
def sample_category_services() -> rx.Component:
    return rx.vstack(
        rx.vstack(
            crud_button(
                "Sample Category",
                SampleCategoryState,
                SampleCategoryState.new_sample_category_form,
                SampleCategoryState.update_sample_category_form,
            ),
            table(SampleCategoryState),
            on_mount=SampleCategoryState.get_data(),
        ),
        rx.vstack(
            crud_button(
                "Sample Services",
                SampleServicesState,
                SampleServicesState.new_sample_services_form,
                SampleServicesState.update_sample_services_form,
            ),
            table(SampleServicesState),
            on_mount=SampleServicesState.get_data(),
        ),
    )

