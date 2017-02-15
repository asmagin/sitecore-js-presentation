const path = require('path');
const env = require('./env');
const defaults = require('./defaults');
const assign = require('lodash/assign');

const cacheDirectory = path.join(defaults.srcPath, '..', 'cache');

const babelConfig = assign({}, require('../package.json').babel, {
  babelrc: false,
  cacheDirectory
});

if (env.isDevServerMode) {
  babelConfig.plugins.unshift('react-hot-loader/babel');
}

module.exports = babelConfig;
