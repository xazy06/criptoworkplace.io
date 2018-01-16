var pCWTToken = artifacts.require("pCWTToken");

module.exports = function(deployer) {
  deployer.deploy(pCWTToken);
};
