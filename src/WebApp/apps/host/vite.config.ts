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
    cacheDir: '../../node_modules/.vite/apps/host',
    server: {
      port: 4200,
      host: 'localhost',
    },
    preview: {
      port: 4300,
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
        name: 'host',
        remotes: {
          order: {
            type: 'module',
            name: 'order',
            entry: env.VITE_REMOTE_ORDER_URL
              ? `${env.VITE_REMOTE_ORDER_URL}/remoteEntry.js`
              : 'http://localhost:4201/remoteEntry.js',
            entryGlobalName: 'order',
            shareScope: 'default',
          },
        },
        exposes: {
          // './auth': '@base/context',
        },
        filename: 'remoteEntry.js',
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
