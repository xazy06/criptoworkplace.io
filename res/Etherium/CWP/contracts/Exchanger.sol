pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/ownership/Ownable.sol";
import "openzeppelin-solidity/contracts/token/ERC20/ERC20Basic.sol";

contract ICrowdsale {
  struct FixedRate {
    uint256 rate;
    uint256 time;
    uint256 amount;
  }
  mapping(address => FixedRate) public fixRate;
}


contract Exchanger is Ownable {

  address private _crowdsale;
  address private _buyer;
  ERC20Basic private _token;

  constructor(address crowdsale, ERC20Basic token, address buyer) public {
    _crowdsale = crowdsale;
    _token = token;
    _buyer = buyer;
  }

  function () external payable{
    BuyTokens();
  }

  function BuyTokens() onlyOwner() public
  {
    (,,uint256 amount) = ICrowdsale(_crowdsale).fixRate(_buyer);
    _crowdsale.transfer(amount);
    uint256 balance = _token.balanceOf(address(this));
    _token.transfer(_buyer, balance);
    _buyer.transfer(address(this).balance);
  }

}