from pandas import DataFrame, to_datetime
from numpy import NaN
from datetime import datetime
import re
from Frontend.models.services_model import ServicesModel, ServiceModel
from Frontend.enum.enums import CheckType

def to_data_table(input_data: list, ignored_columns: list = []) -> (list, list, DataFrame):

    if(input_data == []):
        return [], [], None
    ignored_columns = ["id"] + ignored_columns
    table_columns = [column for column in list(input_data[0].keys()) if column not in ignored_columns]

    dataframe = DataFrame(input_data, columns=table_columns)
    
    for column in dataframe.columns:
        if "date" in column.lower() or "schedule" in column.lower():
            dataframe[column] = to_datetime(dataframe[column], format="mixed")
            dataframe[column] = dataframe[column].dt.strftime("%d %B %Y")
        if "takendate" in column.lower():
            dataframe[column] = to_datetime(dataframe[column], format="mixed")
            dataframe[column] = dataframe[column].dt.strftime("%H:%M %d %B %Y")
        if "Id" in column:
            dataframe.rename(columns={column: column.replace("Id", "")}, inplace=True)

    dataframe.insert(0, "no", [i+1 for i in range(len(input_data))])
    dataframe.replace(NaN, "-", regex=True, inplace=True)
    dataframe.replace("", "-", regex=True, inplace=True)
    dataframe.fillna("-", inplace=True)

    columns = []
    data = dataframe.values.tolist()
    
    for columnTitle, columnType in zip(dataframe.columns.tolist(), [type(x).__name__ for x in data[0]]):
        column = {
            "title": to_title_case(columnTitle),
            "type": columnType,
        }
        columns.append(column)


    return columns, data, dataframe

def to_date_input(date_string: str) -> str:
    return datetime.fromisoformat(date_string).date().strftime("%Y-%m-%d")

def to_title_case(input: str) -> str:
    return re.sub(r"([A-Z])", r" \1", input).title().strip()

def to_services_model(category: str, services: list[dict]) -> ServicesModel:
    return ServicesModel(
        name=category,
        services=[ServiceModel(id=service["id"], name=service["name"], normal_value_type=CheckType(service["normalValueType"])) for service in services]
    )