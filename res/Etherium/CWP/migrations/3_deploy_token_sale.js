const pg = require('pg');
const url = require('url')

const params = url.parse(process.env.CWP_DATABASE_URL);
const auth = params.auth.split(':');
const pool = new pg.Pool({
  user: auth[0],
  password: auth[1],
  host: params.hostname,
  port: params.port,
  database: params.pathname.split('/')[1],
  ssl: params.protocol == 'https'
});

function getAddressesAsync() {
  return new Promise(function (resolve, reject) {
    pool.connect(function (err, client, done) {
      if (err) {
        reject(err);
      } else {
        client.query('select distinct encode(eth_address,\'hex\') as address from identity.users where eth_address is not null', [], function (err, result) {
          done()
          if (err) {
            reject(err);
          } else {
            resolve(result.rows);
          }
        })
      }
    });
  });
}


var CWTPToken = artifacts.require("CWTPToken");
var CWTPTokenSale = artifacts.require("CWTPTokenSale");

module.exports = function (deployer, network) {
  console.log("Start");
  if (network == "ropsten") {
    const walletAddr = "0x0a0C2318D11807d465C6886A5A4707872fbdd82F";
    var cwpInstance = CWTPToken.at(CWTPToken.address), //CWTPToken.at("0x1ae18c56f7e0c1f466b4471a3724bb6f465838b2"),
      min = 60,
      startTime = ((((Date.now() / 1000 | 0) + 2 * 600) / 600) | 0) * 600,
      endTime = startTime + 120 * min,
      tsale;

    deployer.chain
      .then(() => {
        console.log("deploy with parameters: \"" + startTime + "\", \"" + endTime + "\", \"" + walletAddr + "\", \"" + cwpInstance.address + "\"");
        return deployer.deploy(CWTPTokenSale, startTime, endTime, walletAddr, cwpInstance.address);
      })
      .then(() => {
        console.log("transfer Ownership to: " + CWTPTokenSale.address);
        tsale = CWTPTokenSale.at(CWTPTokenSale.address);
        return cwpInstance.transferOwnership(CWTPTokenSale.address);
      })
      .then(() => {
        console.log("Add dapp address");
        return tsale.registerDappAddress("0x0D302d9fFeb60c49F6f5C0Bc56B6b3Be04af6B05");
      })
      .then(() => getAddressesAsync())
      .then(rows => {
        console.log("Add whitelist: 0x"+rows[0].address);
        /* var p = tsale.addAddressToWhitelist('0x' + rows[0].address);
        for (var i = 1; i < rows.length; i++)
        {
          var currentRow = rows[i];
          p = p.then(() => {
            console.log("Add whitelist: 0x"+currentRow.address);
            return tsale.addAddressToWhitelist('0x' + currentRow.address);
          });
        }
        return p;

 */
        var p = tsale.addAddressToWhitelist('0x' + rows[0].address);
        for (var i = 1; i < rows.length; i++){
          
          (function (i) {
            var currentRow = rows[i];
            p = p.then(() => {
              console.log("Add whitelist: 0x" + currentRow.address);
              
              return tsale.addAddressToWhitelist('0x' + currentRow.address);
          });  
          })(i)
          
        }
        return p;
      });
    // .then(() => tsale.addCrowdsaleStep(startTime + 15 * min, 900000 * ether, 0.08 * ether))
    // .then(() => tsale.addCrowdsaleStep(startTime + 20 * min, 900000 * ether, 0.08 * ether))
    // // .then(() => tsale.addCrowdsaleStep(startTime + 25 * min, 900000 * ether, 0.09 * ether))
    // // .then(() => tsale.addCrowdsaleStep(startTime + 30 * min, 900000 * ether, 0.10 * ether))
    // // .then(() => tsale.addCrowdsaleStep(startTime + 35 * min, 900000 * ether, 0.11 * ether))
    // // .then(() => tsale.addCrowdsaleStep(startTime + 40 * min, 900000 * ether, 0.12 * ether))
    // .then(() => tsale.setEthUsdRate(500.50 * ether));
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