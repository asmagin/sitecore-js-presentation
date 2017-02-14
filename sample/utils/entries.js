module.exports = {
  vendor: [
    'normalize.css/normalize.css',
    'bootstrap/dist/css/bootstrap.min.css',
    'babel-polyfill',
    'whatwg-fetch',
    'react',
    'react-dom',
    'lodash',
    'redux',
    'react-redux',
    'redux-logger',
    'redux-saga'
  ],
  common: ['./src/common/components'],
  app: ['./src/index.jsx']
};
