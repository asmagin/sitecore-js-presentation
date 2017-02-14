module.exports = function (config) {
  config.set({

    browsers: [
      'PhantomJS'
      // 'Firefox',
      // 'Chrome'
      // for IE @see https://www.npmjs.com/package/karma-ievms
      // 'IE8 - WinXP',
      // 'IE9 - Win7',
      // 'IE10 - Win7'
    ],

    singleRun: false,

    frameworks: ['mocha', 'sinon-chai'],

    plugins: [
      'karma-mocha',
      'karma-sinon-chai',
      'karma-webpack',
      'karma-phantomjs-launcher',
      'karma-firefox-launcher',
      'karma-chrome-launcher',
      'karma-ievms',
      'karma-sourcemap-loader',
      'karma-mocha-reporter'
    ],

    files: [
      'node_modules/babel-polyfill/browser.js',
      'node_modules/whatwg-fetch/fetch.js',
      './src/test.index.js'
    ],

    preprocessors: {
      './src/test.index.js': ['webpack', 'sourcemap']
    },

    reporters: [
      'mocha'
    ],

    autoWatch: true,

    webpack: require('./webpack.config'),

    webpackServer: {
      noInfo: true,
      quiet: false
    }
  });
};
