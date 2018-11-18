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
      startTime = 1542380400,
      endTime = 1546300800,
      tsale;

    deployer.chain
      .then(() => CWTPTokenSale.deployed())
      .then((instance) => {        
        console.log("check deployed old contract");
        if (instance !== undefined)
        {
          console.log("Force close old contract");
          return instance.ForceCloseContract();
        }
        return true;
      })
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
        console.log("Add whitelist");
        return tsale.addAddressesToWhitelist(rows.map(r=>'0x'+r.address));
      });
  } else {
    const walletAddr = "0x312c8cdb4520196c9000036b58296cad2579f541";
    const dappAddr = "0x7987A3Bb820736Dbf28B8a10DCA2BFa1514d33F7"
    var cwpInstance = CWTPToken.at(CWTPToken.address), //CWTPToken.at("0x1ae18c56f7e0c1f466b4471a3724bb6f465838b2"),
      startTime = 1542380400,
      endTime = 1546300800,
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
        return tsale.registerDappAddress(dappAddr);
      })
      .then(() => getAddressesAsync())
      .then(rows => {
        console.log("Add whitelist");
        return tsale.addAddressesToWhitelist(rows.map(r=>'0x'+r.address));
      });
  }
};