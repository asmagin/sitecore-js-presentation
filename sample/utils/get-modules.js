const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const srcPath = path.join(__dirname, '../src');

function getCssLoader(loader, isExtractCssMode) {
  return isExtractCssMode ? ExtractTextPlugin.extract('isomorphic-style', loader) : `isomorphic-style!${loader}`;
}

/**
 *
 * @param {Array|Object} [loaders]
 * @param {Boolean} [isExtractCssMode]
 * @returns {{preLoaders: *[], loaders: *[]}}
 */
module.exports = function getDefaultModules(loaders, isExtractCssMode) {
  const cssMode = (typeof isExtractCssMode === 'boolean') ? isExtractCssMode : true;
  return {
    preLoaders: [
      {
        test: /\.(js|jsx)$/,
        include: srcPath,
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
        loader: getCssLoader('css?minimize', cssMode)
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
        loader: getCssLoader('css!sass?outputStyle=expanded&indentedSyntax', cssMode)
      },
      {
        test: /\.scss$/,
        loader: getCssLoader('css!sass?outputStyle=expanded', cssMode)
      },
      {
        test: /\.less$/,
        loader: getCssLoader('css!less', cssMode)
      },
      {
        test: /\.(png|jpg|gif|woff|woff2)$/,
        loader: 'url-loader?limit=8192'
      },
      {
        test: /\.(mp4|ogg|svg)$/,
        loader: 'file-loader'
      },
      {
        test: /\.html$/,
        loader: "html"
      }
    ].concat(loaders || [])
  };
};
