import React                      from 'react'
import { connect }                from 'react-redux'
import { Grid, Row, Col }         from 'react-bootstrap'
import { bindActionCreators }     from 'redux'
import * as actionCreators        from './timer.actions';

require('./timer.styles.scss');

class Timer extends React.Component {

  componentDidMount() {
    this.interval = setInterval(this.props.onTick, 1000);
  }

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  render() {
    return (
      <Grid className="timer">
        <Row>
          <Col xs={12} md={12} style={{fontSize: '20px'}}>
            <h3><div dangerouslySetInnerHTML={{ __html: this.props.title }} /> { this.props.timer }s</h3>
          </Col>
        </Row>
      </Grid>
    )
  }
}

Timer.defaultProps = {
  'title': 'This is timer'
}

function mapStateToProps(state) {
  return { timer: state.timer }
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(Timer);