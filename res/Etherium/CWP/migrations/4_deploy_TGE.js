var CWTPToken = artifacts.require("CWTPToken");
var CWTToken = artifacts.require("CWTToken");
var CWPTokenSale = artifacts.require("CWPTokenSale");

module.exports = function(deployer, network) {
    if (network == "ropsten" && CWTToken.isDeployed() && CWTPToken.isDeployed()) //ropsten
    {
      const walletAddr = "0x0a0c2318d11807d465c6886a5a4707872fbdd82f";
      var cwpInstance;

      deployer.chain
        .then(() => CWTToken.deployed())
        .then(function(cwp) {
          cwpInstance = cwp;
          return deployer.deploy(CWPTokenSale, (Date.now() / 1000 | 0) + 100, 1, walletAddr, cwpInstance.address, CWTPToken.address)
            .then(function(t) {
              console.log("transfer Ownership to: " + CWPTokenSale.address);
              return cwpInstance.transferOwnership(CWPTokenSale.address);
            });
        });
    }
    else {
      const walletAddr = "0x312c8cdb4520196c9000036b58296cad2579f541"; //multisig
      throw 0;
    }
};
