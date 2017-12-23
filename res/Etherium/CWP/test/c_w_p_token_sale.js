var CWPTokenSale = artifacts.require("CWPTokenSale");
var CWPToken = artifacts.require("CWPToken");

contract('CWPTokenSale', function(accounts) {
  it("should be deployed", async () => {
    try {
      let sale = await CWPTokenSale.deployed();
      assert.isDefined(sale);
      assert.notEqual(sale.address, 0x00, "Empty address");
    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("Token addres must be address of CWPToken", async () => {
    try {
      let sale = await CWPTokenSale.deployed();
      let token = await CWPToken.deployed();
      assert.equal((await sale.token()), token.address, "Token is not CWPToken contract");
    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("Shoud store cap, goal and rate", async () => {
    try {
      let sale = await CWPTokenSale.deployed();
      assert.equal((await sale.cap()).toNumber(), (await sale.CAP0()).toNumber(), "Cap is wrong");
      assert.equal((await sale.goal()).toNumber(), (await sale.REFUND_CAP()).toNumber(), "Goal is wrong");
    } catch (e) {
      if (e.name == "AssertionError") {
        throw e;
      } else {
        console.log(e);
        assert.fail();
      }
    }
  });

  it("Should save", async () => {
    try {

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
