var pCWTToken = artifacts.require("pCWTToken");
var CWPToken = artifacts.require("CWPToken");
var CWPTokenSale = artifacts.require("CWPTokenSale");

module.exports = function(deployer, network) {
    if (network == "ropsten" && CWPToken.isDeployed() && pCWTToken.isDeployed()) //ropsten
    {
      const walletAddr = "0x0a0c2318d11807d465c6886a5a4707872fbdd82f";
      var cwpInstance;

      deployer.chain
        .then(() => CWPToken.deployed())
        .then(function(cwp) {
          cwpInstance = cwp;
          return deployer.deploy(CWPTokenSale, (Date.now() / 1000 | 0) + 100, 1, walletAddr, cwpInstance.address, pCWTToken.address)
            .then(function(t) {
              console.log("transfer Ownership to: " + CWPTokenSale.address);
              return cwpInstance.transferOwnership(CWPTokenSale.address);
            });
        })
        ;
    }
};
