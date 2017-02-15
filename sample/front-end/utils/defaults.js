const path = require('path');
const assets = 'assets';

const additionalPaths = [];
const srcPath = path.join(__dirname, '..', 'src');
const distPath = path.join(__dirname, '..', 'dist');
const assetsPath = path.join(distPath, assets);

module.exports = {
  srcPath,
  distPath,
  assetsPath,
  additionalPaths,
  publicPath: `/${assets}/`,
  port: 8000
};
