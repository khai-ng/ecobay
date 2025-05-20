FROM node:24-alpine AS base

# Install dependencies only when needed
FROM base AS deps
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat
WORKDIR /app

# Install dependencies based on the preferred package manager
COPY package.json ./
COPY yarn.lock* ./
COPY nx.json ./
COPY tsconfig*.json ./

#COPY package.json yarn.lock* package-lock.json* pnpm-lock.yaml* .npmrc* ./
RUN \
  if [ -f yarn.lock ]; then yarn --frozen-lockfile; \
  elif [ -f package-lock.json ]; then npm ci; \
  elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm i --frozen-lockfile; \
  else echo "Lockfile not found." && exit 1; \
  fi

# Rebuild the source code only when needed
FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

ARG APP_NAME=host

RUN echo "Current directory contents:" && ls -la

# Install dependencies
RUN yarn


# Build the Next.js app
RUN yarn add sharp
RUN yarn nx build ${APP_NAME} --prod

RUN echo "Build output contents:" && ls -la app/apps/${APP_NAME} || true

# Production stage
FROM base AS runner
WORKDIR /app

ARG APP_NAME=host

# Copy built assets from builder
COPY --from=builder /app/apps/${APP_NAME}/.next ./.next
COPY --from=builder /app/apps/${APP_NAME}/public ./public
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./
COPY --from=builder /app/apps/${APP_NAME}/next.config.js ./

# Copy the docker environment file
COPY --from=builder /app/apps/${APP_NAME}/.env.docker ./.env

ENV NODE_ENV=production
ENV PORT=3000

EXPOSE 3000

CMD ["yarn", "next", "start"]
