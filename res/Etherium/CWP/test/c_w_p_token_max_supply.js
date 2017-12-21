var CWPToken = artifacts.require("CWPToken");

contract('CWPToken', function(accounts) {
  it("should be deployed", async () => {
    try {
      let token = await CWPToken.deployed();
      assert.isDefined(token);
      assert.notEqual(token.address, 0x00, "Empty address");
    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("should minting", async () => {
    try {
      var account_one = accounts[0];
      var account_one_starting_balance;
      let token = await CWPToken.deployed();
      let amount = (await token.MAX_SUPPLY()).toNumber() - 1;

      account_one_starting_balance = (await token.balanceOf(account_one)).toNumber();
      assert.equal(account_one_starting_balance, 0, "Frst account initial balance must be zero");
      let transaction = await token.mint(account_one, amount);
      assert.isDefined(transaction);
      account_one_starting_balance = (await token.balanceOf(account_one)).toNumber();
      assert.equal(account_one_starting_balance, amount, "Frst account initial balance must be minted amount");

    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("should not mint when minting more, then MAX_SUPPLY", async () => {
    try {
      var account = accounts[0];
      var account_starting_balance;
      var account_ending_balance;
      var amount = 10;
      var throwException = false;

      let token = await CWPToken.deployed();

      account_starting_balance = (await token.balanceOf(account)).toNumber();
      let transaction = await token.finishMinting();
      assert.isDefined(transaction);
      try {
        var transaction2 = await token.mint(account, amount);
      } catch (e) {
        throwException = true;
      }
      assert.isUndefined(transaction2);
      assert.isTrue(throwException);
      account_ending_balance = (await token.balanceOf(account)).toNumber();
      assert.equal(account_starting_balance, account_ending_balance, "Balance on start and on end not the same");

    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }

  });

  it("should total supply count correctly", async () => {
    try {
      let token = await CWPToken.deployed();
      let supply = (await token.totalSupply()).toNumber();
      assert.equal(supply, (await token.MAX_SUPPLY()).toNumber() - 1, "Total Supply count incorrectly");

    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

});
