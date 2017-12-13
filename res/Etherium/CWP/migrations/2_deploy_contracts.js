var CWPToken = artifacts.require("CWPToken");
var CWPTokenSale = artifacts.require("CWPTokenSale");

module.exports = function(deployer) {
  deployer.deploy(CWPToken)
    .then(() => deployer.deploy(CWPTokenSale, CWPToken.address, (Date.now() / 1000 | 0)+100, (Date.now() / 1000 | 0)+500, 1,20,40, 0x5AEDA56215b167893e80B4fE645BA6d5Bab767DE))
};
