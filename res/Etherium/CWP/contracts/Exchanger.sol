pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/ownership/Ownable.sol";
import "openzeppelin-solidity/contracts/token/ERC20/ERC20Basic.sol";


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
  }

  function BuyTokens(uint256 ethers) onlyOwner() public
  {
    _crowdsale.transfer(ethers);
    uint256 balance = _token.balanceOf(address(this));
    _token.transfer(_buyer, balance);
    _buyer.transfer(address(this).balance);
  }

}