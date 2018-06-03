var CWTToken = artifacts.require("CWTToken");

module.exports = function (deployer, network) {
  throw 0;
  if (network == "ropsten") {
    deployer.deploy(CWTToken);
  } else {
    throw 0;
  }
};