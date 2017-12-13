var CWPToken = artifacts.require("CWPToken");

//console.log(assert);

contract('CWPToken', function(accounts) {
    it("should be deployed", async() => {
      try {
        await CWPToken.deployed();
        return true;
      } catch (e) {
        console.log(e);
        assert.fail();
      }
    });

    it("should send coin correctly",async () => {
        assert.isTrue(accounts.length >= 2, "We have only one account");
        // Get initial balances of first and second account.
        var account_one = accounts[0];
        var account_two = accounts[1];
        var account_one_starting_balance;
        var account_two_starting_balance;
        var account_one_ending_balance;
        var account_two_ending_balance;

        let amount = 10;
        let token = await CWPToken.deployed();
        account_one_starting_balance = (await token.balanceOf(account_one)).toNumber();
        assert.equal(account_one_starting_balance, 0, "Frst account initial balance must be zero");
        await token.mint(account_one,amount);
        account_one_starting_balance = (await token.balanceOf(account_one)).toNumber();
        assert.equal(account_one_starting_balance, amount, "Frst account initial balance must be minted amount");
        account_two_starting_balance = (await token.balanceOf(account_two)).toNumber();
        assert.equal(account_two_starting_balance, 0, "Second account initial balance must be zero");
        await token.transfer(account_two, amount,{from: account_one});

        account_one_ending_balance = (await token.balanceOf(account_one)).toNumber();
        account_two_ending_balance = (await token.balanceOf(account_two)).toNumber();
        assert.equal(account_one_ending_balance, account_one_starting_balance - amount, "Amount wasn't correctly taken from the sender");
        assert.equal(account_two_ending_balance, account_two_starting_balance + amount, "Amount wasn't correctly sent to the receiver");
    });
});
