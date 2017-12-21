pragma solidity ^0.4.18;

import "zeppelin-solidity/contracts/token/CappedToken.sol";

contract CWPToken is CappedToken {
  string public constant name = "CryptoWorkPlace Token";
  string public constant symbol = "CWT";
  uint8 public constant decimals = 15;
  uint256 public constant MAX_SUPPLY = 500000000 * (uint256(10) ** decimals);

  function CWPToken() CappedToken(MAX_SUPPLY) public {
  }
}
