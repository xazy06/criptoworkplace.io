var CWPToken = artifacts.require("CWPToken");

module.exports = function(deployer) {
  deployer.deploy(CWPToken);
};
