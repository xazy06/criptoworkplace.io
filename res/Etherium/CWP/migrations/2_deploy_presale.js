var CWTPToken = artifacts.require("CWTPToken");

module.exports = function(deployer) {
  deployer.deploy(CWTPToken);
};
