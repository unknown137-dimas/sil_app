FROM python:3.11-slim

WORKDIR /app
COPY ./Frontend ./Frontend
COPY ./Backend/appsettings.json ./Backend/appsettings.json

RUN apt update && apt install -y zip curl && pip install -r ./Frontend/requirements.txt

WORKDIR /app/Frontend

RUN reflex init

# Download all npm dependencies and compile frontend
RUN reflex export --frontend-only --no-zip

# Needed until Reflex properly passes SIGTERM on backend.
STOPSIGNAL SIGKILL

# Always apply migrations before starting the backend.
CMD [ -d alembic ] && reflex db migrate; reflex run --env prod