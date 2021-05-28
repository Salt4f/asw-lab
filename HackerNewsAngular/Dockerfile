FROM node:latest as node

ARG ENV=prod
ARG APP=HackerNewsAngular

ENV ENV ${ENV}
ENV APP ${APP}

WORKDIR /app
COPY ./ /app/

# Instala y construye el Angular App
RUN npm ci
ENTRYPOINT ["npm", "start"]