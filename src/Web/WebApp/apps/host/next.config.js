// eslint-disable-next-line @typescript-eslint/no-var-requires
const { composePlugins, withNx } = require('@nx/next');
const NextFederationPlugin = require("@module-federation/nextjs-mf");
const path = require('path');

const remotes = (isServer) => {
  const location = isServer ? "ssr" : "chunks";
  return {
    ordering: `ordering@http://localhost:3002/_next/static/${location}/remoteEntry.js`,
  };
};

/**
 * @type {import('@nx/next/plugins/with-nx').WithNxOptions}
 **/
const nextConfig = {
  nx: {
    // Set this to true if you would like to use SVGR
    // See: https://github.com/gregberge/svgr
    svgr: false,
  },
  webpack(config, option) {
    config.resolve.alias = {
      ...config.resolve.alias,
      '@app': path.join(__dirname, 'src'),
      '@base': path.join(__dirname, '../../shared'),
    };

    config.plugins.push(
      new NextFederationPlugin({
        name: "host",
        remotes: remotes(option.isServer),
        filename: "static/chunks/remoteEntry.js",
        exposes: {},
        extraOptions: {}
      })
    );
    return config;
  },
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'm.media-amazon.com',
        port: '',
        search: '',
      },
    ],
  },
};

const plugins = [
  // Add more Next.js plugins to this list if needed.
  withNx,
];

module.exports = composePlugins(...plugins)(nextConfig);
