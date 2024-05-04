import httpx
from json import loads

async def get(url: str) -> tuple[list[str], list[dict]]:
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        json_response = loads(response.text)
        return json_response["messages"], json_response["data"]

async def post(url: str, payload: dict) -> tuple[list[str], list[dict]]:
    async with httpx.AsyncClient() as client:
        response = await client.post(url, json=dict(payload))
        json_response = loads(response.text)
        return json_response["messages"], json_response["data"]

async def delete(url: str) -> tuple[list[str], list[dict]]:
    async with httpx.AsyncClient() as client:
        response = await client.delete(url)
        json_response = loads(response.text)
        return json_response["messages"], json_response["data"]