const TruffleConfig = require('../truffle');
var Migrations = artifacts.require("./Migrations.sol");


module.exports = function(deployer, network, addresses) {
  deployer.deploy(Migrations);
};
