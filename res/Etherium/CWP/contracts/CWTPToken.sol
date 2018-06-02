pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/token/ERC20/CappedToken.sol";

contract CWTPToken is CappedToken {
  string public constant name = "Pre-sale CryptoWorkPlace Token";
  string public constant symbol = "CWT-P";
  uint8 public constant decimals = 18;
  uint256 public constant MAX_SUPPLY = 15000000 * (uint256(10) ** decimals); //3% of 500 000 000

  constructor() public CappedToken(MAX_SUPPLY) {}
}