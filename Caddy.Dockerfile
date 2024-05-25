FROM caddy

COPY --from=local/sil_app /app/Frontend/.web/_static /srv
ADD Caddyfile /etc/caddy/Caddyfile