const webpack = require('webpack');
const BowerWebpackPlugin = require('bower-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

const omit = require('lodash/omit');
const defaults = require('./../utils/defaults');
const utils = require('../utils');

let chunks = {
  name: ['vendor', 'shared'],
  minChunks: 2
};

const config = {
  port: defaults.port,
  debug: true,
  entry: omit(utils.entries, 'app'),
  module: utils.getModules(utils.babelLoader),
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
    new ExtractTextPlugin('[name].css?[hash]', {
      allChunks: true
    }),
    new webpack.optimize.CommonsChunkPlugin(chunks),
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    })
  ])
};

module.exports = config;
