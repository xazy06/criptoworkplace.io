var CWPToken = artifacts.require("CWPToken");
var CWPTokenSale = artifacts.require("CWPTokenSale");

module.exports = function(deployer, network) {
    if (network == "ropsten") //ropsten
    {
        const walletAddr = "0x0a0c2318d11807d465c6886a5a4707872fbdd82f";
        CWPToken.deployed().then(cwpt => deployer.deploy(CWPTokenSale, cwpt.address, (Date.now() / 1000 | 0) + 100, 1, walletAddr));
    }
};
