import json

def app_config() -> dict[str, str]:
    with open("../Backend/appsettings.json", "r") as config_file:
        return json.load(config_file)