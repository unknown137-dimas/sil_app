from enum import Enum, IntEnum

class FormType(Enum):
    Input = "input"
    Number = "number"
    Password = "password"
    Date = "date"
    Time = "time"
    Datetime = "datetime-local"
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

class CheckStatus(IntEnum):
    Processing = 0,
    Waiting = 1,
    Done = 2

class ValidationStatus(IntEnum):
    WaitingValidation = 0,
    Valid = 1,
    NotValid = 2

class UserRoles(Enum):
    Admin = "Administrator",
    Regis = "Registration Staff",
    Sampling = "Sampling Staff",
    Lab = "Lab Staff"