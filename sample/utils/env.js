const C = require('./constants');
const env = process.env.NODE_ENV;
const isDevServerMode = process.argv[1].indexOf('webpack-dev-server') !== -1;

module.exports = {
  isDevServerMode: isDevServerMode,

  environment: env,

  isBrowser: env === C.PROD || env === C.DEV,

  isServer: env === C.SERVER_DEV || env === C.SERVER_PROD
};
