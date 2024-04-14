from pandas import DataFrame

def to_data_table(input_data: list) -> (list, list):

    ignored_columns = ["id"]
    table_columns = ["no"]
    table_columns += [column for column in list(input_data[0].keys()) if column not in ignored_columns]

    dataframe = DataFrame(input_data, columns=table_columns)
    dataframe["no"] = [i+1 for i in range(len(input_data))]
    dataframe.columns = dataframe.columns.str.capitalize()

    columns = []
    data = dataframe.values.tolist()
    
    for columnTitle, columnType in zip(dataframe.columns.tolist(), [type(x).__name__ for x in data[0]]):
        column = {
            "title": columnTitle,
            "type": columnType,
            # "width": 25
        }
        columns.append(column)

    return columns, data
