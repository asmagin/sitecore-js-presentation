import React                      from 'react'
import { connect }                from 'react-redux'
import { Grid, Row, Col }         from 'react-bootstrap'
import { bindActionCreators }     from 'redux'
import * as actionCreators        from './counter.actions';

require('./counter.styles.scss');

class Counter extends React.Component  {
  render() {
    return (
      <Grid className="counter">
        <Row>
          <Col xs={12} md={12} style={{fontSize: '20px'}}>
            <h3><div dangerouslySetInnerHTML={{ __html: this.props.data.title }} /></h3>
            <button onClick={this.props.onDecrement} style={{padding: '0 10px'}}> - </button>
            <strong style={{padding: '0 20px'}}>{ this.props.counter }</strong>
            <button onClick={this.props.onIncrement} style={{padding: '0 10px'}}> + </button>
          </Col>
        </Row>
      </Grid>
    )
  }
}

Counter.defaultProps = {
  data: {
    'title': 'This is counter'
  }
}

function mapStateToProps(state) {
  return { counter: state.counter }
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(Counter);