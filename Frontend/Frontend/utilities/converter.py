from pandas import DataFrame, to_datetime
from datetime import datetime

def to_data_table(input_data: list) -> (list, list, DataFrame):

    if(input_data == []):
        return [], [], None
    ignored_columns = ["id"]
    table_columns = ["no"]
    table_columns += [column for column in list(input_data[0].keys()) if column not in ignored_columns]

    dataframe = DataFrame(input_data, columns=table_columns)
    
    for column in dataframe.columns:
        if "Date" in column:
            dataframe[column] = to_datetime(dataframe[column], format="mixed")
            dataframe[column] = dataframe[column].dt.strftime("%d %B %Y")

    dataframe["no"] = [i+1 for i in range(len(input_data))]
    dataframe.columns = dataframe.columns.str.capitalize()

    columns = []
    data = dataframe.values.tolist()
    
    for columnTitle, columnType in zip(dataframe.columns.tolist(), [type(x).__name__ for x in data[0]]):
        column = {
            "title": columnTitle,
            "type": columnType,
        }
        columns.append(column)

    return columns, data, dataframe

def to_date_only(date_string: str) -> str:
    return datetime.fromisoformat(date_string).date().strftime("%Y-%m-%d")