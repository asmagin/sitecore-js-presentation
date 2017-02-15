import React from 'react';
import ReactDOM from 'react-dom';
import {AppContainer} from 'react-hot-loader';
import App from './common/pages/sample/sample';

require('whatwg-fetch');
require('babel-polyfill');

function render(node) {
  if (typeof document === 'undefined') {
    return null;
  } else if (module.hot) {
    ReactDOM.render(
      <AppContainer><App/></AppContainer>, node);

    module
      .hot
      .accept('./common/pages/sample/sample', () => {
        const App = require('./common/pages/sample/sample').default;

        ReactDOM.render(
          <AppContainer>
            <App/>
          </AppContainer>, document.getElementById('app'));
      });
  }
  ReactDOM.render(
    <App/>, node);
}

render(document.getElementById('app'));