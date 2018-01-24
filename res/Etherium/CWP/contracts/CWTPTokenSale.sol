pragma solidity ^0.4.18;

import "./CWTPToken.sol";
import "./CWPTokenAddr.sol";
import "zeppelin-solidity/contracts/crowdsale/Crowdsale.sol";
import "zeppelin-solidity/contracts/ownership/Ownable.sol";

contract CWTPTokenSale is CWPTokenAddr,Crowdsale, Ownable {
  string public constant name = "Pre-sale CryptoWorkPlace Token";
  string public constant symbol = "CWT-P";
  uint8 public constant decimals = 18;
  uint256 public constant MAX_SUPPLY = 15000000 * (uint256(10) ** decimals); //3% of 500 000 000

  function CWTPTokenSale(uint256 _startTime, uint256 _endTime, uint256 _rate, address _wallet, address _token) public
    Ownable()
    CWPTokenAddr(_token)
    Crowdsale(_startTime, _endTime, _rate, _wallet)
  {
  }

  function changeRate(uint256 _newRate) onlyOwner public {
    require(_newRate > 0);
    rate = _newRate;
  }

  function createTokenContract() internal returns (MintableToken) {
    return CWTPToken(tokenAddress);
  }
}
