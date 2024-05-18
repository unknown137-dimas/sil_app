from reflex.page import get_decorated_pages

path_config = {
    "Administrator": [page["route"] for page in get_decorated_pages() if page["route"] != "/login"],
    "Lab Staff": ["/patient_check", "/medical_tool", "/reagen", "/patient"],
    "Sampling Staff": ["/patient_sample"],
    "Registration Staff": ["/patient", "/patient_sample", "/patient_check"],
}
