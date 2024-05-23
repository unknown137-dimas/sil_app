import os
from dotenv import load_dotenv
from pathlib import Path

load_dotenv(Path("../../.env"))

API_HOST = os.getenv("API_HOST")
API_URL = API_HOST if API_HOST else "http://localhost:5056" + "/api"
API_USER = API_URL + "/user"
API_USER_LOGIN = API_USER + "/login"
API_ROLE = API_URL + "/role"
API_MEDICAL_TOOL = API_URL + "/medical-tool"
API_REAGEN = API_URL + "/reagen"
API_CHECK = API_URL + "/check"
API_CHECK_CATEGORY = API_CHECK + "/category"
API_CHECK_SERVICE = API_CHECK + "/service"
API_SAMPLE = API_URL + "/sample"
API_SAMPLE_CATEGORY = API_SAMPLE + "/category"
API_SAMPLE_SERVICE = API_SAMPLE + "/service"
API_PATIENT = API_URL + "/patient"
API_PATIENT_CHECK = API_PATIENT + "/check"
API_PATIENT_SAMPLE = API_PATIENT + "/sample"
