FROM node:latest as node

ARG ENV=prod
ARG APP=HackerNewsAngular

ENV ENV ${ENV}
ENV APP ${APP}

WORKDIR /app
COPY ./ /app/

# Instala y construye el Angular App
RUN npm install
RUN npm run ng build --prod
RUN mv /app/dist/${APP}/* /app/dist/

# Angular app construida, la vamos a hostear un server production, este es Nginx

FROM nginx:1.13.8-alpine

COPY --from=node /app/dist/ /usr/share/nginx/html
COPY --from=node /app/node_modules/ /app/node_modules
COPY ./nginx.conf /etc/nginx/conf.d/default.conf