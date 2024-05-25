# This docker file is intended to be used with docker compose to deploy a production
# instance of a Reflex app.

# Stage 1: init
FROM python:3.11 as init

ARG uv=/root/.cargo/bin/uv

# Install `uv` for faster package boostrapping
ADD --chmod=755 https://astral.sh/uv/install.sh /install.sh
RUN /install.sh && rm /install.sh

# Copy local context to `/app` inside container (see .dockerignore)
WORKDIR /app
COPY ./Frontend ./Frontend
COPY ./Backend/appsettings.json ./Backend/appsettings.json
RUN mkdir -p /app/Frontend/data /app/Frontend/uploaded_files

WORKDIR /app/Frontend

# Create virtualenv which will be copied into final container
ENV VIRTUAL_ENV=/app/Frontend/.venv
ENV PATH="$VIRTUAL_ENV/bin:$PATH"
RUN $uv venv

# Install app requirements and reflex inside virtualenv
RUN $uv pip install -r requirements.txt

# Define api url
ENV API_URL="https://app-backend.burbee.duckdns.org"

# Export static copy of frontend to /app/.web/_static
RUN reflex export --frontend-only --no-zip

# Copy static files out of /app to save space in backend image
RUN mv .web/_static /tmp/_static
RUN rm -rf .web && mkdir .web
RUN mv /tmp/_static .web/_static

# Stage 2: copy artifacts into slim image 
FROM python:3.11-slim
WORKDIR /app
RUN adduser --disabled-password --home /app/Frontend reflex
COPY --chown=reflex --from=init /app /app
USER reflex
ENV PATH="/app/Frontend/.venv/bin:$PATH"

WORKDIR /app/Frontend

# Needed until Reflex properly passes SIGTERM on backend.
STOPSIGNAL SIGKILL

# Always apply migrations before starting the backend.
CMD reflex db migrate && reflex run --env prod --backend-only