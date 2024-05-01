import jwt
from Frontend.utilities.app_config import app_config

secret_key = app_config()["Jwt"]["Key"]

def decode(token: str) -> dict[str, any]:
    return jwt.decode(token, secret_key, algorithms=["HS512"], audience="LIS")

# def is_allowed(token: str, page_path: str) -> bool:
#     jwt_token = decode(token)