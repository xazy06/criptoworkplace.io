var CWTPToken = artifacts.require("CWTPToken");
var CWTPTokenSale = artifacts.require("CWTPTokenSale");

module.exports = function(deployer, network) {
    if (network == "ropsten") {
      const walletAddr = "0x41d2dd0e0bdb0c23c0ab51a428bfd57b3df6ea26";
      var pTokenInstance;
      deployer.chain
        .then(() => CWTPToken.deployed())
        .then((pToken) => {
          pTokenInstance = pToken;
          console.log("Using presale token address: "+pToken.address)
          return deployer.deploy(CWTPTokenSale, Date.now() / 1000 , 1517443199, walletAddr, pToken.address )
        })
        .then(() => {
          console.log("transfer Ownership to: " + CWTPTokenSale.address);
          return pTokenInstance.transferOwnership(CWTPTokenSale.address);
        });

    }
    else {
      throw 0;
    }
};
