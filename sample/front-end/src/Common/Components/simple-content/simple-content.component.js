require('./simple-content.styles.sass');

import React from 'react';
import { Grid, Row, Col } from 'react-bootstrap'

import logo from '../../static/images/sample.png';

class SimpleContent extends React.Component {
  render() {
    return (
      <Grid className='simple-content'>
        <div style={{fontSize: '8px', color: '#444'}} >** react component **</div>
        <Row>
          <Col xs={12} md={4}>
          <div dangerouslySetInnerHTML={{ __html: this.props.data.image }} /></Col>
          <Col xs={12} md={8}>
            <h1><div dangerouslySetInnerHTML={{ __html: this.props.data.title }} /></h1>
            <div className='rte'>
              <div dangerouslySetInnerHTML={{ __html: this.props.data.text }} />
            </div>
          </Col>
        </Row>
        <Row>
          <Col xs={12} md={6} >
            {
              React.isValidElement(this.props.placeholders.leftColumn)
                ? <div className='placeholder'>{ this.props.placeholders.leftColumn }</div>
                : <div className='placeholder' dangerouslySetInnerHTML={{ __html: this.props.placeholders.leftColumn }} />
            }
          </Col>
          <Col xs={12} md={6} >
            {
              React.isValidElement(this.props.placeholders.rightColumn)
                ? <div className='placeholder'>{ this.props.placeholders.rightColumn }</div>
                : <div className='placeholder' dangerouslySetInnerHTML={{ __html: this.props.placeholders.rightColumn }} />
            }
          </Col>
        </Row>
      </Grid>
    )
  }
}

SimpleContent.propTypes = {
  data: React.PropTypes.shape({
    image: React.PropTypes.string,
    title: React.PropTypes.string,
    text: React.PropTypes.string
  }).isRequired
};

SimpleContent.defaultProps = {
  data: {
    'title': 'HTML Ipsum Presents',
    'text': '<p><strong>Pellentesque habitant morbi tristique</strong> senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. <em>Aenean ultricies mi vitae est.</em> Mauris placerat eleifend leo. Quisque sit amet est et sapien ullamcorper pharetra. Vestibulum erat wisi, condimentum sed, <code>commodo vitae</code>, ornare sit amet, wisi. Aenean fermentum, elit eget tincidunt condimentum, eros ipsum rutrum orci, sagittis tempus lacus enim ac dui. <a href="#">Donec non enim</a> in turpis pulvinar facilisis. Ut felis.</p>',
    'image': '<img src="'+ logo +'" alt="" height="200" />'
  },
  placeholders: {
    leftColumn: '<div><p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. Mauris placerat eleifend leo.</p></div>',
    rightColumn: '<div><ol><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li></ol></div>'
  }
};

// this function would be called by ReactJS.NET to identify placeholders avaliable in a component.
SimpleContent.placeholders = [
  'leftColumn',
  'rightColumn'
];

export default SimpleContent;