import { createStore }  from 'redux';
import { combineReducers  } from 'redux';

import counter         from './counter/counter.reducer'
import timer           from './timer/timer.reducer'

const reducer = combineReducers ( { counter, timer } )

// exports
export const Store = (typeof(window) !== 'undefined')
    ? createStore(reducer, window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__())
    : createStore(reducer);

export const InitialState = JSON.stringify(Store.getState());