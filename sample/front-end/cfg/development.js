'use strict';

const webpack = require('webpack');
const BowerWebpackPlugin = require('bower-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
var HtmlWebpackPlugin = require('html-webpack-plugin');

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
    filename: '[name].js?[hash]',
    chunkFilename: '[id].js?[chunkhash]',
    publicPath: defaults.publicPath
  },
  devServer: {
    contentBase: defaults.distPath,
    historyApiFallback: true,
    hot: true,
    port: defaults.port,
    publicPath: defaults.publicPath,
    noInfo: false,
    proxy: {
      '/api': {
        target: `http://localhost:3000`, //${defaults.port}`,
        secure: false
      }
    }
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  },
  plugins: utils.getPlugins([
    new HtmlWebpackPlugin({
      filename: `${defaults.distPath}/index.html`,
      template: `${defaults.srcPath}/index.html.tmpl`
    }),
    new ExtractTextPlugin('[name].css?[hash]', {
      allChunks: true,
      disable: utils.isDevServerMode
    }),
    new webpack.optimize.CommonsChunkPlugin(chunks),
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    })
  ])
};

if (utils.isDevServerMode) {
  config.entry.app.unshift(
    'react-hot-loader/patch',
    `webpack-dev-server/client?http://localhost:${defaults.port}`,
    'webpack/hot/only-dev-server'
  );
  config.plugins.push(new webpack.HotModuleReplacementPlugin());
}

module.exports = config;
