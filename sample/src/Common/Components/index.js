import React                      from 'react'
import ReactDOM                   from 'react-dom'
import ReactDOMServer             from 'react-dom/server'
import { Provider }               from 'react-redux'

import { Store }                  from './store';

import headerComp                 from './header/header.component';
import simpleContentComp          from './simple-content/simple-content.component';
import counterComp                from './counter/counter.component';
import timerComp                  from './timer/timer.component';

import lodashContentComp          from './lodash-content/lodash-content.component';

const createData = (props) => {
  var res = {};

  if (typeof(props) === 'undefined') {
    return {};
  }

  if (typeof(props.data) != 'undefined') {
    res['data'] = props.data;
  }

  if (typeof(props.placeholders) != 'undefined') {
    res['placeholders'] = props.placeholders;
  }

  return res;
}

const decorate = (element, isContainer) => (
  {
    element,
    renderToDOM(props, node) {
      var el = React.createElement(element, { data: props.data, placeholders: props.placeholders });
      if (isContainer) {
        el = React.createElement(Provider, { store: Store }, el);
      }
      ReactDOM.render(el, document.getElementById(node))
    },
    renderToString(props) {
      var el = React.createElement(element, { data: props.data, placeholders: props.placeholders });
      if (isContainer) {
        el = React.createElement(Provider, { store: Store }, el);
      }
      return ReactDOMServer.renderToString(el)
    },
    renderToStaticMarkup(props) {
      var el = React.createElement(element, { data: props.data, placeholders: props.placeholders });
      if (isContainer) {
        el = React.createElement(Provider, { store: Store }, el);
      }
      return ReactDOMServer.renderToStaticMarkup(el)
    },
    getPlaceholders(){
      if(typeof(element) === 'undefined'
        || typeof(element.placeholders) === 'undefined') {
          return '';
      }

      return JSON.stringify(element.placeholders)
    }
  }
)

// exports
export const header        = decorate(headerComp);
export const simpleContent = decorate(simpleContentComp);

export const counter       = decorate(counterComp, true);
export const timer         = decorate(timerComp, true);

const decorateLodash = (element) => (
  {
    element,
    renderToDOM(props, node) {
      var div = document.createElement('div');
      div.innerHTML = new element(createData(props)).render();
      document.getElementById(node).appendChild(div)
    },
    renderToString(props) {
      return new element(createData(props)).render();
    },
    renderToStaticMarkup(props) {
      return new element(createData(props)).render();
    },
    getPlaceholders(){
      if(typeof(element) === 'undefined'
        || typeof(element.placeholders) === 'undefined') {
          return '';
      }

      return JSON.stringify(element.placeholders)
    }
  }
)

export const lodashContent = decorateLodash(lodashContentComp);