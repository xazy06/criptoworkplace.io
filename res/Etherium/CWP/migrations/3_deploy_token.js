var CWTToken = artifacts.require("CWTToken");

module.exports = function(deployer, network) {
    if (network == "ropsten") {
      deployer.deploy(CWTToken);
    }
    else {
      throw 0;
    }
};
