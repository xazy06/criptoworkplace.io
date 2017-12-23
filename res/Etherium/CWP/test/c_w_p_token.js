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

  it("name and symbol shoud be usable", async () => {
    try {
      let token = await CWPToken.deployed();
      assert.equal((await token.name()), "CryptoWorkPlace Token", "Name of contract is worng");
      assert.equal((await token.symbol()), "CWT", "Symbol of contract is worng");
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
      var amount = 20;

      let token = await CWPToken.deployed();

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

  it("should send coin correctly", async () => {
    try {
      // Get initial balances of first and second account.
      var account_one = accounts[0];
      var account_two = accounts[1];
      var account_one_starting_balance;
      var account_two_starting_balance;
      var account_one_ending_balance;
      var account_two_ending_balance;
      var amount = 10;

      let token = await CWPToken.deployed();
      account_one_starting_balance = (await token.balanceOf(account_one)).toNumber();
      account_two_starting_balance = (await token.balanceOf(account_two)).toNumber();
      await token.transfer(account_two, amount, {
        from: account_one
      });
      account_one_ending_balance = (await token.balanceOf(account_one)).toNumber();
      account_two_ending_balance = (await token.balanceOf(account_two)).toNumber();
      assert.equal(account_one_ending_balance, account_one_starting_balance - amount, "Amount wasn't correctly taken from the sender");
      assert.equal(account_two_ending_balance, account_two_starting_balance + amount, "Amount wasn't correctly sent to the receiver");
    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }

  });

  it("should approve spent", async () => {
    try {
      var user_approver = accounts[1];
      var user_spender = accounts[2];
      var account_one_starting_balance;
      var account_two_starting_balance;
      var account_one_ending_balance;
      var account_two_ending_balance;
      var amount = 5;
      let token = await CWPToken.deployed();

      account_one_starting_balance = (await token.balanceOf(user_approver)).toNumber();
      account_two_starting_balance = (await token.balanceOf(user_spender)).toNumber();

      let transaction = await token.approve(user_spender, amount, {
        from: user_approver
      });
      assert.isDefined(transaction);

      let allowed = (await token.allowance(user_approver, user_spender)).toNumber();
      assert.equal(allowed, amount, "Alowed not equal amount");
      let transaction2 = await token.transferFrom(user_approver, user_spender, amount, {
        from: user_spender
      });
      assert.isDefined(transaction2);
      allowed = (await token.allowance(user_approver, user_spender)).toNumber();
      assert.equal(allowed, 0, "Alowed not decreased after spent");
      account_one_ending_balance = (await token.balanceOf(user_approver)).toNumber();
      account_two_ending_balance = (await token.balanceOf(user_spender)).toNumber();
      assert.equal(account_one_ending_balance, account_one_starting_balance - amount, "Amount wasn't correctly taken from the sender");
      assert.equal(account_two_ending_balance, account_two_starting_balance + amount, "Amount wasn't correctly sent to the receiver");

    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("should not spent more, then approve", async () => {
    try {
      var user_approver = accounts[1];
      var user_spender = accounts[2];
      var account_one_starting_balance;
      var account_two_starting_balance;
      var account_one_ending_balance;
      var account_two_ending_balance;
      var amount = 5;
      var throwException = false;

      let token = await CWPToken.deployed();

      account_one_starting_balance = (await token.balanceOf(user_approver)).toNumber();
      account_two_starting_balance = (await token.balanceOf(user_spender)).toNumber();
      try {
        var transaction = await token.transferFrom(user_approver, user_spender, amount, {
          from: user_spender
        });

      } catch (e) {
        throwException = true;
      }
      assert.isUndefined(transaction);
      assert.isTrue(throwException);
      account_one_ending_balance = (await token.balanceOf(user_approver)).toNumber();
      account_two_ending_balance = (await token.balanceOf(user_spender)).toNumber();
      assert.equal(account_one_ending_balance, account_one_starting_balance, "Ballance of approver changed");
      assert.equal(account_two_ending_balance, account_two_starting_balance, "Ballance of spender changed");

    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("should not mint when minting finished", async () => {
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
      assert.equal(supply, 20, "Total Supply count incorrectly");

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
