var CWTPToken = artifacts.require("CWTPToken");
var CWTPTokenSale = artifacts.require("CWTPTokenSale");

module.exports = function (deployer, network) {
  if (network == "ropsten") {
    const walletAddr = "0x0a0c2318d11807d465c6886a5a4707872fbdd82f";
    var cwpInstance = CWTPToken.at("0x1ae18c56f7e0c1f466b4471a3724bb6f465838b2"),
      startTime = (Date.now() / 1000 | 0) + 10,
      endTime = startTime + 450,
      tsale;

    deployer.chain
      .then(() => deployer.deploy(CWTPTokenSale, startTime, endTime, 900000, 0.12 * Math.pow(10, 18), walletAddr, cwpInstance.address))
      .then(() => {
        console.log("transfer Ownership to: " + CWTPTokenSale.address);
        tsale = CWTPTokenSale.at(CWTPTokenSale.address);
        return cwpInstance.transferOwnership(CWTPTokenSale.address);
      })
      .then(() => {
        console.log("Create steps ");
        return tsale.addCrowdsaleStep(startTime + 50, 900000, 0.07 * Math.pow(10, 18));
      })
      .then(() => tsale.addCrowdsaleStep(startTime + 100, 900000, 0.07 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 150, 900000, 0.08 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 200, 900000, 0.08 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 250, 900000, 0.09 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 300, 900000, 0.10 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 350, 900000, 0.11 * Math.pow(10, 18)))
      .then(() => tsale.addCrowdsaleStep(startTime + 400, 900000, 0.12 * Math.pow(10, 18)))
      .then(() => tsale.setEthUsdRate(500.50 * Math.pow(10, 18)));
  } else {
    const walletAddr = "0x312c8cdb4520196c9000036b58296cad2579f541";
    var cwpInstance;

    deployer.chain
      .then(() => CWTPToken.deployed())
      .then(function (cwp) {
        cwpInstance = cwp;
        return deployer.deploy(CWTPTokenSale, (Date.now() / 1000 | 0) + 60, 1522540799, walletAddr, cwpInstance.address)
          .then(function (t) {
            console.log("transfer Ownership to: " + CWTPTokenSale.address);
            return cwpInstance.transferOwnership(CWTPTokenSale.address);
          });
      });
  }
};