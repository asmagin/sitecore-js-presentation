const babelConfig = require('./babel-config');
const defaults = require('./defaults');

module.exports = {
  test: /\.(js|jsx)$/,
  loader: `babel-loader?${JSON.stringify(babelConfig)}`,
  include: [].concat(defaults.additionalPaths, defaults.srcPath)
};
