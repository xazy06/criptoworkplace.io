pragma solidity ^0.4.18;

import "zeppelin-solidity/contracts/token/CappedToken.sol";

contract CWTToken is CappedToken {
  string public constant name = "CryptoWorkPlace Token";
  string public constant symbol = "CWT";
  uint8 public constant decimals = 18;
  uint256 public constant MAX_SUPPLY = 500000000 * (uint256(10) ** decimals);

  function CWTToken() CappedToken(MAX_SUPPLY) public {
  }
}
