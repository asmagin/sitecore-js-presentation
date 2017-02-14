require('./lodash-content.styles.scss');

import templateContent from './lodash-content.component.html';
import logo from '../../static/images/sample.png';

import _ from 'lodash';

class LodashContent {

  constructor(data, placeholders) {

    this.defaultProps = {
      data: {
        'title': '<strong>TITLE</strong>',
        'text': '<strong>TEXT</strong>',
        'image': '<img src="' + logo + '" alt="" width="250" height="200" />'
      },
      placeholders: {
        leftColumn: '<div>LEFT COLUMNT CONTENT</div>',
        rightColumn: '<div>RIGHT COLUMNT CONTENT</div>'
      }
    }

    this.props = Object.assign({}, this.defaultProps, data);
    this.props.placeholders = Object.assign(this.props.placeholders, placeholders);
  }

  render() {
    return _.template(templateContent)(this.props);
  }
}

// this function would be called by ReactJS.NET to identify placeholders avaliable in a component.
LodashContent.placeholders = [
  'leftColumn',
  'rightColumn'
];

// this function would be called by ReactJS.NET to identify placeholders
// avaliable in a component.
export default LodashContent;