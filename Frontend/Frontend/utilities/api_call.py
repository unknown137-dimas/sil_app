import httpx

async def get(url: str) -> httpx.Response:
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        return response

async def post(url: str, payload: dict) -> httpx.Response:
    async with httpx.AsyncClient() as client:
        response = await client.post(url, json=payload)
        return response

async def delete(url: str) -> httpx.Response:
    async with httpx.AsyncClient() as client:
        response = await client.delete(url)
        return response