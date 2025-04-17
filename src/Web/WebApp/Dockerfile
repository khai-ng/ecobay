## syntax=docker.io/docker/dockerfile:1
#
#FROM node:18-alpine AS base
#
## Install dependencies only when needed
#FROM base AS deps
## Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
#RUN apk add --no-cache libc6-compat
#WORKDIR /app
#
## Install dependencies based on the preferred package manager
#COPY src/Web/WebApp/package.json ./
#COPY src/Web/WebApp/yarn.lock* ./
##COPY package.json yarn.lock* package-lock.json* pnpm-lock.yaml* .npmrc* ./
#RUN \
  #if [ -f yarn.lock ]; then yarn --frozen-lockfile; \
  #elif [ -f package-lock.json ]; then npm ci; \
  #elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm i --frozen-lockfile; \
  #else echo "Lockfile not found." && exit 1; \
  #fi
#
#
## Rebuild the source code only when needed
#FROM base AS builder
#WORKDIR /app
#COPY --from=deps /app/node_modules ./node_modules
#COPY . .
#
## Next.js collects completely anonymous telemetry data about general usage.
## Learn more here: https://nextjs.org/telemetry
## Uncomment the following line in case you want to disable telemetry during the build.
#ENV NEXT_TELEMETRY_DISABLED=1
#
#RUN \
  #if [ -f yarn.lock ]; then yarn nx build host --prod; \
  #elif [ -f package-lock.json ]; then npm nx run host:build; \
  #elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm nx run host:build; \
  #else echo "Lockfile not found." && exit 1; \
  #fi
#
## Production image, copy all the files and run next
#FROM base AS runner
#WORKDIR /app
#
#ENV NODE_ENV=production
## Uncomment the following line in case you want to disable telemetry during runtime.
#ENV NEXT_TELEMETRY_DISABLED=1
#
#RUN addgroup --system --gid 1001 nodejs
#RUN adduser --system --uid 1001 nextjs
#
#COPY --from=builder /app/public ./public
#
## Automatically leverage output traces to reduce image size
## https://nextjs.org/docs/advanced-features/output-file-tracing
#COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
#COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static
#
#USER nextjs
#
#EXPOSE 3000
#
#ENV PORT=3000
#
## server.js is created by next build from the standalone output
## https://nextjs.org/docs/pages/api-reference/config/next-config-js/output
#ENV HOSTNAME="0.0.0.0"
#CMD ["node", "server.js"]

############
FROM node:18-alpine AS base

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

ENV NODE_ENV=production
ENV PORT=3000

EXPOSE 3000

CMD ["yarn", "next", "start"]
