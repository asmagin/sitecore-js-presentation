/* global describe, it */

import * as chai from 'chai';

const {expect} = chai;

const lolex = require('lolex');
const fakedMethods = ['setTimeout', 'clearTimeout', 'setImmediate', 'clearImmediate', 'setInterval',
  'clearInterval', 'Date'];

describe('Sample tests scaffolding', () => {
  describe('Verify simple case', () => {
    it('1 should be equals to 1', () => {
      expect(1).to.equal(1);
    });
  });

  describe('Simple async case with setTimeout', () => {
    let clock;

    beforeEach(() => {
      clock = lolex.install((new Date()).getTime(), fakedMethods);
    });

    afterEach(() => {
      clock.uninstall();
    });

    it('timestamp before timer should be less than before', done => {
      let d = Date.now();
      setTimeout(() => {
        expect(Date.now()).to.greaterThan(d + 1000);
        done();
      }, 1005);
      clock.tick(1010);
    });
  });
});
