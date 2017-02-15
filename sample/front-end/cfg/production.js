'use strict';

const webpack = require('webpack');
const BowerWebpackPlugin = require('bower-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

const defaults = require('./../utils/defaults');
const utils = require('../utils');

let chunks = {
  name: ['vendor', 'shared'],
  minChunks: 2
};

const config = {
  port: defaults.port,
  debug: true,
  devtool: 'source-map',
  entry: utils.entries,
  module: utils.getModules(utils.babelLoader),
  output: {
    path: defaults.assetsPath,
    library: '[name]__ext',
    filename: '[name].[hash].js',
    chunkFilename: '[id].[chunkhash].js',
    publicPath: defaults.publicPath
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  },
  plugins: utils.getPlugins([
    new ExtractTextPlugin('[name].[hash].css', {
      allChunks: true,
      disable: utils.isDevServerMode
    }),
    new webpack.optimize.DedupePlugin(),
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify('production')
    }),
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    }),
    new webpack.optimize.UglifyJsPlugin(),
    new webpack.optimize.OccurenceOrderPlugin(),
    new webpack.optimize.AggressiveMergingPlugin(),
    new webpack.NoErrorsPlugin(),
    new webpack.optimize.CommonsChunkPlugin(chunks),
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    })
  ])
};


module.exports = config;
