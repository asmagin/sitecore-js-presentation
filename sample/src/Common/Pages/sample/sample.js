require('normalize.css');
require('./sample.styles.sass');

import React from 'react';
import ReactDOMServer from 'react-dom/server'
import { Provider } from 'react-redux';

import Header from '../../components/header/header.component';
import SimpleContent from '../../components/simple-content/simple-content.component';
import Counter from '../../components/counter/counter.component';
import Timer from '../../components/timer/timer.component';

import { store } from './sample.store';

const nestedElement = <Counter data={{title: 'Nested counter'}}/>
const someText = <p><strong>Some JSX component</strong><br/>Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. Mauris placerat eleifend leo.</p>

const App = () => (
  <Provider store={store} >
    <div>
      <Header/>
      <SimpleContent placeholders={{leftColumn: someText, rightColumn: nestedElement }}/>
      <Counter data={{title: 'Fully wired counter'}}/>
      <Timer data={{title: 'Timer ticking'}}/> 
    </div>
  </Provider>
)

export default App;
