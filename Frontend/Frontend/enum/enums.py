from enum import Enum

class FormType(Enum):
    INPUT = "input"
    PASSWORD = "password"
    DATE = "date"
    SELECT = "select"

class CheckType(Enum):
    NUMERIC = 0
    STRING = 1

class Gender(Enum):
    MALE = 0
    FEMALE = 1