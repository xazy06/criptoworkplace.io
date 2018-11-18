 var HDWalletProvider = require("truffle-hdwallet-provider-privkey");
// var infura_apikey = "roht23j583p4SPym7gx6";

console.log(process.env.CWP_PK);
module.exports = {
  // See <http://truffleframework.com/docs/advanced/configuration>
  // to customize your Truffle configuration!
  networks: {
    development: {
      host: "localhost",
      port: 8545,
      network_id: "*" // Match any network id
    },
    ropsten: {
      host: "104.209.40.23",
      port: 8545,
      //provider: new HDWalletProvider("", "http://104.209.40.23:8545/"),
      network_id: 3,
      from: "0xc57e3ac95b372619cb7cfa41674a9183d18e4ed1",
      gas: 4612388,
      gasPrice:20000000000
    },
    main: {
        // host: "192.168.68.4",
        // port: 8545,
        provider: new HDWalletProvider("", "https://mainnet.infura.io/roht23j583p4SPym7gx6"),
        network_id: 1,
        gas: 4612388,
        gasPrice:8000000000
    }
  }
};
