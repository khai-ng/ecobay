/// <reference types='vitest' />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { federation } from '@module-federation/vite';
import * as path from 'path';

// Read dependencies from root package.json
const rootPackageJson = require(path.resolve(__dirname, '../../package.json'));

export default defineConfig(() => ({
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
  plugins: [
    federation({
      name: 'host',
      remotes: {
        order: {
          type: 'module',
          name: 'order',
          entry: 'http://localhost:4201/remoteEntry.js',
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
    emptyOutDir: true,
    reportCompressedSize: true,
    commonjsOptions: {
      transformMixedEsModules: true,
    },
  },
}));
