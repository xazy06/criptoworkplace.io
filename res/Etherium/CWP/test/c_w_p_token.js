var CWPToken = artifacts.require("CWPToken");

//console.log(assert);

contract('CWPToken', function(accounts) {
    it("should be deployed", function() {
        CWPToken.deployed().catch(function(e){
            assert.fail();
        });
    });

    it("should send coin correctly",async function(){
        var token;
        accounts.should.have.length(2);
        // Get initial balances of first and second account.
        var account_one = accounts[0];
        var account_two = accounts[1];
        var account_one_starting_balance;
        var account_two_starting_balance;
        var account_one_ending_balance;
        var account_two_ending_balance;

        var amount = 10;

        return CWPToken.deployed().then(function(instance) {
            token = instance;
            return token.balanceOf(account_one);
        },function(e){ console.log("Token not deployed"); assert.fail() }).then(function(balance) {
            account_one_starting_balance = balance.toNumber();
            assert.equal(account_one_starting_balance,0,"Frst account initial balance must be zero");
            return token.mint(account_one,amount);
        },function(e){ console.log("Fail get balance of first account"); }).then(function(mintResult){
            return token.balanceOf(account_one);
        },function(e){
            console.log("Can't mint new tokens");
            assert.fail();
        }).then(function(balance){
            account_one_starting_balance = balance.toNumber();
            return token.balanceOf(account_two);
        },function(e){ console.log("Fail get balance of first account"); assert.fail() }).then(function(balance) {
            account_two_starting_balance = balance.toNumber();
            return token.transfer(account_two, amount,{from: account_one});
        },function(e){ console.log("Fail get balance of second account"); console.log(e); assert.fail() }).then(function() {
            return token.balanceOf(account_one);
        },function(e){ console.log("Fail transfer from account 1 to account 2");console.log(e); assert.fail(); }).then(function(balance) {
            account_one_ending_balance = balance.toNumber();
            return token.balanceOf(account_two);
        }).then(function(balance) {
            account_two_ending_balance = balance.toNumber();
            assert.equal(account_one_ending_balance, account_one_starting_balance - amount, "Amount wasn't correctly taken from the sender");
            assert.equal(account_two_ending_balance, account_two_starting_balance + amount, "Amount wasn't correctly sent to the receiver");
        });
    });
});
