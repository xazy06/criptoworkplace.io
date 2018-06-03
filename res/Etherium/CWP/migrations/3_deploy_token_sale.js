var CWTPToken = artifacts.require("CWTPToken");
var CWTPTokenSale = artifacts.require("CWTPTokenSale");

module.exports = function (deployer, network) {
  console.log("Start");
  if (network == "ropsten") {
    const walletAddr = "0x0a0C2318D11807d465C6886A5A4707872fbdd82F";
    var cwpInstance = CWTPToken.at("0x1ae18c56f7e0c1f466b4471a3724bb6f465838b2"),
      min = 60,
      ether = Math.pow(10, 18),
      startTime = ((((Date.now() / 1000 | 0) + 600) / 600) | 0) * 600,
      endTime = startTime + 45 * min,
      tsale;

    deployer.chain
      .then(() => deployer.deploy(CWTPTokenSale, startTime, endTime, 900000 * ether, 500 * ether, 0.12 * ether, walletAddr, cwpInstance.address))
      .then(() => {
        console.log("transfer Ownership to: " + CWTPTokenSale.address);
        tsale = CWTPTokenSale.at(CWTPTokenSale.address);
        return cwpInstance.transferOwnership(CWTPTokenSale.address);
      })
      .then(() => {
        console.log("Create steps");
        return tsale.addCrowdsaleStep(startTime + 5 * min, 900000 * ether, 0.07 * ether);
      })
      .then(() => tsale.addCrowdsaleStep(startTime + 10 * min, 900000 * ether, 0.07 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 15 * min, 900000 * ether, 0.08 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 20 * min, 900000 * ether, 0.08 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 25 * min, 900000 * ether, 0.09 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 30 * min, 900000 * ether, 0.10 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 35 * min, 900000 * ether, 0.11 * ether))
      .then(() => tsale.addCrowdsaleStep(startTime + 40 * min, 900000 * ether, 0.12 * ether))
      .then(() => tsale.setEthUsdRate(500.50 * ether));
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