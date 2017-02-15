const webpack = require('webpack');
const BowerWebpackPlugin = require('bower-webpack-plugin');

const omit = require('lodash/omit');
const defaults = require('./../utils/defaults');
const utils = require('../utils');

const config = {
  port: defaults.port,
  debug: true,
  entry: omit(utils.entries, 'app'),
  module: utils.getModules(utils.babelLoader, false),
  output: {
    path: defaults.assetsPath,
    library: '[name]__ext',
    filename: '[name].js',
    publicPath: defaults.publicPath
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  },
  plugins: utils.getPlugins([
    new webpack.optimize.CommonsChunkPlugin({
      name: ['vendor', 'shared'],
      minChunks: 2
    }),
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    })
  ])
};

module.exports = config;
