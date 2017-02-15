'use strict';

const path = require('path');
const C = require('./utils/constants');
const allowedEnvs = Object.keys(C);

const env = process.env.NODE_ENV || C.DEV;

if (allowedEnvs.indexOf(env) !== -1) {
  process.env.NODE_ENV = env;
} else {
  process.env.NODE_ENV = C.DEV;
}

module.exports = require(path.join(__dirname, 'cfg/' + env));
