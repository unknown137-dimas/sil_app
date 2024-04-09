from pandas import DataFrame

def to_data_table(input_data: list) -> DataFrame:

    ignored_columns = ["id"]
    table_columns = ["no"]
    table_columns += [column for column in list(input_data[0].keys()) if column not in ignored_columns]

    dataframe = DataFrame(input_data, columns=table_columns)
    dataframe["no"] = [i+1 for i in range(len(input_data))]
    dataframe.columns = dataframe.columns.str.capitalize()
    
    return dataframe
