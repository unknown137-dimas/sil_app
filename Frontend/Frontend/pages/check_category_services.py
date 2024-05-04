from Frontend import styles
from Frontend.templates import template
from Frontend.const.api import API_CHECK_CATEGORY, API_CHECK_SERVICE
from Frontend.utilities import api_call
from Frontend.utilities import converter
from Frontend.models.form_model import FormModel
from Frontend.components.crud_button import crud_button
from Frontend.components.table import table
from Frontend.enum.enums import FormType, Gender, ValueType
from pandas import DataFrame

import reflex as rx


class CheckCategoryState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    new_check_category_form: list[FormModel] = [
        FormModel(
            name="name",
            placeholder="Check Category Name",
            required=True,
            form_type=FormType.Input.value
        ),
    ]
    update_check_category_form: list[FormModel] =  []

    async def get_data(self):
        _, self.raw_data = await api_call.get(API_CHECK_CATEGORY)
        self.columns, self.data, _ = converter.to_data_table(self.raw_data, ["checkServices"])
        check_service_state = await self.get_state(CheckServicesState)
        await check_service_state.get_data()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        if self.selected_data:
            self.update_check_category_form = [
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
        del self.selected_data["checkServices"]
        await api_call.post(
            f"{API_CHECK_CATEGORY}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()
        self.updating = False

    async def delete_data(self):
        await api_call.delete(f"{API_CHECK_CATEGORY}/{self.selected_data['id']}")
        await self.get_data()

    async def add_data(self, form_data: dict):
        await api_call.post(
            API_CHECK_CATEGORY,
            payload=form_data
        )
        await self.get_data()

class CheckServicesState(rx.State):
    columns: list = []
    data: list = []
    raw_data: list
    check_categories_raw_data: list
    selected_data: dict[str, str] = {}
    updating: bool = False
    loading: bool = True
    update_check_services_form: list[FormModel] =  []

    @rx.var
    def new_check_services_form(self) -> list[FormModel]:
        return [
            FormModel(
                name="name",
                placeholder="Check Services Name",
                required=True,
                form_type=FormType.Input.value,
                min_length=3
            ),
            FormModel(
                name="checkCategoryId",
                placeholder="Check Category",
                required=True,
                form_type=FormType.Select.value,
                options=self.check_categories_name
            ),
            FormModel(
                name="normalValueType",
                placeholder="Value Type",
                required=True,
                form_type=FormType.Select.value,
                options=[v.name for v in ValueType]
            ),
            FormModel(
                name="checkUnit",
                placeholder="Check Unit",
                form_type=FormType.Input.value
            ),
            FormModel(
                name="gender",
                placeholder="Gender",
                form_type=FormType.Select.value,
                options=[g.name for g in Gender]
            ),
            FormModel(
                name="minNormalValue",
                placeholder="Min Normal Value",
                form_type=FormType.Input.value,
                min_value=0,
                max_value=200
            ),
            FormModel(
                name="maxNormalValue",
                placeholder="Max Normal Value",
                form_type=FormType.Input.value,
                min_value=0,
                max_value=200
            ),
            FormModel(
                name="normalValue",
                placeholder="Normal Value",
                form_type=FormType.Input.value
            ),
        ]

    @rx.var
    def check_categories_name(self) -> list[str]:
        return [cc["name"] for cc in self.check_categories_raw_data]

    def get_check_category_name_by_id(self, category_id: str) -> str:
        for cc in self.check_categories_raw_data:
            if cc["id"] == category_id:
                return cc["name"]

    def get_check_category_id_by_name(self, category_name: str) -> str:
        for cc in self.check_categories_raw_data:
            if cc["name"] == category_name:
                return cc["id"]

    async def get_data(self):
        check_categories = await self.get_state(CheckCategoryState)
        self.check_categories_raw_data = check_categories.raw_data
        _, self.raw_data = await api_call.get(API_CHECK_SERVICE)
        if self.raw_data:
            self.columns, _, dataFrame = converter.to_data_table(self.raw_data)
            for column in dataFrame.columns:
                if "gender" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: Gender(data).name)
                if "valuetype" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: ValueType(data).name)
                if "checkcategory" in column.lower():
                    dataFrame[column] = dataFrame[column].apply(lambda data: self.get_check_category_name_by_id(data))

            self.data = dataFrame.values.tolist()
        self.loading = False

    def get_selected_data(self, pos):
        self.updating = True
        _, selectedRow = pos
        self.selected_data = self.raw_data[selectedRow]
        self.normalValueType: str = ""
        self.update_check_services_form = [
            FormModel(
                name="name",
                placeholder="Check Services Name",
                required=True,
                form_type=FormType.Input.value,
                min_length=3,
                default_value=self.selected_data["name"]
            ),
            FormModel(
                name="checkCategoryId",
                placeholder="Check Category",
                required=True,
                form_type=FormType.Select.value,
                options=self.check_categories_name,
                default_value=self.get_check_category_name_by_id(self.selected_data["checkCategoryId"])
            ),
            FormModel(
                name="normalValueType",
                placeholder="Value Type",
                required=True,
                form_type=FormType.Select.value,
                options=[v.name for v in ValueType],
                default_value=ValueType(self.selected_data["normalValueType"]).name
            ),
            FormModel(
                name="checkUnit",
                placeholder="Check Unit",
                form_type=FormType.Input.value,
                default_value=self.selected_data["checkUnit"]
            ),
            FormModel(
                name="gender",
                placeholder="Gender",
                form_type=FormType.Select.value,
                options=[g.name for g in Gender],
                default_value=Gender(self.selected_data["gender"]).name
            ),
            FormModel(
                name="minNormalValue",
                placeholder="Min Normal Value",
                form_type=FormType.Input.value,
                min_value=0,
                max_value=200,
                default_value=self.selected_data["minNormalValue"]
            ),
            FormModel(
                name="maxNormalValue",
                placeholder="Max Normal Value",
                form_type=FormType.Input.value,
                min_value=0,
                max_value=200,
                default_value=self.selected_data["maxNormalValue"]
            ),
            FormModel(
                name="normalValue",
                placeholder="Normal Value",
                form_type=FormType.Input.value,
                default_value=self.selected_data["normalValue"]
            ),
        ]
    
    async def update_data(self, form_data: dict):
        form_data["gender"] = Gender[form_data["gender"]].value
        form_data["normalValueType"] = ValueType[form_data["normalValueType"]].value
        form_data["checkCategoryId"] = self.get_check_category_id_by_name(form_data["checkCategoryId"])
        self.selected_data.update(form_data)
        await api_call.post(
            f"{API_CHECK_SERVICE}/{self.selected_data['id']}",
            payload=self.selected_data
        )
        await self.get_data()
        self.updating = False

    async def delete_data(self):
        await api_call.delete(f"{API_CHECK_SERVICE}/{self.selected_data['id']}")
        await self.get_data()

    async def add_data(self, form_data: dict):
        form_data["gender"] = Gender[form_data["gender"]].value
        form_data["normalValueType"] = ValueType[form_data["normalValueType"]].value
        form_data["checkCategoryId"] = self.get_check_category_id_by_name(form_data["checkCategoryId"])
        await api_call.post(
            API_CHECK_SERVICE,
            payload=form_data
        )
        await self.get_data()

        
@template(route="/check_category_services", title="Check Category & Services", image="/github.svg")
def check_category_services() -> rx.Component:
    return rx.vstack(
        rx.vstack(
            crud_button(
                "Check Category",
                CheckCategoryState,
                CheckCategoryState.new_check_category_form,
                CheckCategoryState.update_check_category_form,
            ),
            table(CheckCategoryState),
            on_mount=CheckCategoryState.get_data(),
        ),
        rx.vstack(
            crud_button(
                "Check Services",
                CheckServicesState,
                CheckServicesState.new_check_services_form,
                CheckServicesState.update_check_services_form,
            ),
            table(CheckServicesState),
            on_mount=CheckServicesState.get_data(),
        ),
    )

