const path = require('path');
const BowerWebpackPlugin = require('bower-webpack-plugin');

const defaults = require('../utils/defaults');

const config = {
  devtool: 'inline-source-map',
  module: {
    preLoaders: [
      {
        test: /\.(js|jsx)$/,
        include: defaults.srcPath,
        loader: 'eslint-loader'
      }
    ],
    loaders: [
      {
        // normalize.css and bootstrap css
        test: /\.css$/,
        include: [
          path.join(__dirname, '../node_modules/bootstrap'),
          path.join(__dirname, '../node_modules/normalize.css')
        ],
        loader: 'style!css?minimize'
      },
      {
        // bootstrap fonts
        test: /\.(svg|ttf|eot|woff|woff2)$/,
        include: [
          path.join(__dirname, '../node_modules/bootstrap')
        ],
        loader: 'file?name=bootstrap/[name].[ext]'
      },
      {
        test: /\.sass$/,
        loader: 'style!css!sass?outputStyle=expanded&indentedSyntax'
      },
      {
        test: /\.scss$/,
        loader: 'style!css!sass?outputStyle=expanded&indentedSyntax'
      },
      {
        test: /\.less$/,
        loader: 'style!css!less'
      },
      {
        test: /\.(svg|png|jpg|gif|woff|woff2)$/,
        loader: 'url-loader?limit=8192'
      }
    ].concat(require('../utils/babel-loader'))
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  },
  plugins: [
    new BowerWebpackPlugin({
      searchResolveModulesDirectories: false
    })
  ]
};

module.exports = config;
