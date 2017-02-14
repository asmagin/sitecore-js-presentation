const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

const env = require('./env');
const isDevServerMode = env.isDevServerMode;
const defaults = require('./defaults');

/**
 * @param {Array} plugins
 * @returns {Array}
 */
module.exports = function getPlugins(plugins) {
  return (isDevServerMode ? [] : [new CleanWebpackPlugin([defaults.distPath], {root: process.cwd()})])
    .concat([
      new CopyWebpackPlugin([
        {
          from: path.join(defaults.srcPath, './favicon-32x32.png'),
          to: defaults.distPath
        },
        {
          from: path.join(defaults.srcPath, './favicon-16x16.png'),
          to: defaults.distPath
        },
        {
          from: path.join(defaults.srcPath, './common/static'),
          to: path.join(defaults.distPath, './common/static')
        }
      ])
    ])
    .concat(plugins || []);
};
