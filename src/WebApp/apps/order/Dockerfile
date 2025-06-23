FROM node:24-alpine AS base
# Install dependencies only when needed
FROM base AS builder
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat gettext
WORKDIR /app

COPY package.json ./
COPY yarn.lock* ./
COPY nx.json ./
COPY tsconfig*.json ./
COPY eslint.config.mjs ./

COPY apps ./apps
COPY libs ./libs

RUN \
  if [ -f yarn.lock ]; then yarn --frozen-lockfile; \
  elif [ -f package-lock.json ]; then npm ci; \
  elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm i --frozen-lockfile; \
  else echo "Lockfile not found." && exit 1; \
  fi

ARG APP_NAME=order

RUN yarn
RUN yarn nx run ${APP_NAME}:build:production

FROM nginx:alpine

ARG APP_NAME=order
COPY --from=builder /app/apps/${APP_NAME}/dist /usr/share/nginx/html
RUN rm /etc/nginx/conf.d/default.conf
COPY --from=builder /app/apps/${APP_NAME}/nginx.conf /etc/nginx/conf.d/default.conf

COPY /env.sh /docker-entrypoint.d/env.sh
RUN dos2unix /docker-entrypoint.d/env.sh

EXPOSE 80

ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]