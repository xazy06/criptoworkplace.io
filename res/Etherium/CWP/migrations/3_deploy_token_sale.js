var CWTPToken = artifacts.require("CWTPToken");
var CWPPTokenSale = artifacts.require("CWPPTokenSale");

module.exports = function(deployer, network) {
    if (network == "ropsten") {
      const walletAddr = "0x0a0c2318d11807d465c6886a5a4707872fbdd82f";
      var cwpInstance;

      deployer.chain
        .then(() => CWTPToken.deployed())
        .then(function(cwp) {
          cwpInstance = cwp;
          return deployer.deploy(CWPPTokenSale, (Date.now() / 1000 | 0) + 100, 1, walletAddr, cwpInstance.address)
            .then(function(t) {
              console.log("transfer Ownership to: " + CWPPTokenSale.address);
              return cwpInstance.transferOwnership(CWPPTokenSale.address);
            });
        });
    }
    else {
      throw 0;
    }
};
