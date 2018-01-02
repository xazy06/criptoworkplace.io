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
      host: "104.42.19.47",
      port: 8545,
      network_id: 3,
      from: "0xc57e3ac95b372619cb7cfa41674a9183d18e4ed1",
      gas: 4612388
    },
    main: {
        host: "104.42.106.59",
        port: 8545,
        network_id: 1,
        from: "0x00406eDfc8e186E2FFF7394f1a6e3796DCa4e59f",
        gas: 4612388,
        gasPrice:6000000000
    }
  }
};
