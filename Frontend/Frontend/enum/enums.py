from enum import Enum, IntEnum

class FormType(Enum):
    Input = "input"
    Password = "password"
    Date = "date"
    Select = "select"

class CheckType(IntEnum):
    Numeric = 0
    Text = 1

class Gender(IntEnum):
    Male = 0
    Female = 1

class CalibrationStatus(IntEnum):
    Good = 0,
    NotOptimal = 1,
    Calibrating = 2,
    WaitingCalibration = 3,

class ValueType(IntEnum):
    Numeric = 0,
    Text = 1
