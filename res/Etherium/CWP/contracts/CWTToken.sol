pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/token/ERC20/CappedToken.sol";

contract CWTToken is CappedToken {
  string public constant name = "CryptoWorkPlace Token";
  string public constant symbol = "CWT";
  uint8 public constant decimals = 18;
  uint256 public constant MAX_SUPPLY = 500000000 * (uint256(10) ** decimals);

  constructor() CappedToken(MAX_SUPPLY) public {
  }
}
