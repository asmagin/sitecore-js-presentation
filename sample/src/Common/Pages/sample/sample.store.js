import { createStore, combineReducers } from 'redux';

import counter from '../../components/counter/counter.reducer';
import timer from '../../components/timer/timer.reducer';

const reducer = combineReducers({ counter, timer });

export const store = createStore(reducer);

