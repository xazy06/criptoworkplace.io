var CWPToken = artifacts.require("CWPToken");

module.exports = function(deployer, network) {
    if (network == "ropsten") {
      deployer.deploy(CWPToken);
    }
};
