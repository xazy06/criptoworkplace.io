var CWPToken = artifacts.require("CWPToken");
var CWPTokenSale = artifacts.require("CWPTokenSale");

module.exports = function(deployer) {
  // deployment steps
  deployer.deploy(CWPTokenSale, (Date.now() / 1000 | 0)+100, (Date.now() / 1000 | 0)+500, 1,20,40, 0x5aeda56215b167893e80b4fe645ba6d5bab767de);
};
