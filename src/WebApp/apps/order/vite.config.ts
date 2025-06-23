/// <reference types='vitest' />
import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import { federation } from '@module-federation/vite';
import * as path from 'path';

// Read dependencies from root package.json
const rootPackageJson = require(path.resolve(__dirname, '../../package.json'));

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, __dirname);

  return {
    root: __dirname,
    cacheDir: '../../node_modules/.vite/apps/order',
    server: {
      port: 4201,
      host: 'localhost',
    },
    preview: {
      port: 4301,
      host: 'localhost',
    },
    define: {
      'process.env': env,
    },
    base: './',
    resolve: {
      alias: {
        '@base/components': path.resolve(__dirname, '../../libs/components/src/index.ts'),
        '@base/utils': path.resolve(__dirname, '../../libs/utils/src/index.ts'),
      },
    },
    plugins: [
      federation({
        filename: 'remoteEntry.js',
        name: 'order',
        exposes: {
          './app': './src/app/app.tsx',
          './routes': './src/routes.tsx',
        },
        remotes: {
          host: {
            type: 'module',
            name: 'host',
            entry: env.VITE_REMOTE_HOST_URL
              ? `${env.VITE_REMOTE_HOST_URL}/remoteEntry.js`
              : 'http://localhost:4201/remoteEntry.js',
            entryGlobalName: 'host',
            shareScope: 'default',
          }
        },
        shared: {
          react: {
            singleton: true,
            requiredVersion: rootPackageJson.dependencies.react,
          },
          'react-dom': {
            singleton: true,
            requiredVersion: rootPackageJson.dependencies['react-dom'],
          },
          'react-router-dom': {
            singleton: true,
            requiredVersion: rootPackageJson.dependencies['react-router-dom'],
          },
          '@base/context': {
            singleton: true
          },
        },
      }),
      react()
    ],
    // Uncomment this if you are using workers.
    // worker: {
    //  plugins: [ nxViteTsPaths() ],
    // },
    build: {
      outDir: './dist',
      target: 'esnext',
      emptyOutDir: true,
      reportCompressedSize: true,
      commonjsOptions: {
        transformMixedEsModules: true,
      },
    },
  }
});
