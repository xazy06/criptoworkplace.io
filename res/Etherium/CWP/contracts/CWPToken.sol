pragma solidity ^0.4.18;

import "zeppelin-solidity/contracts/token/MintableToken.sol";

contract CWPToken is MintableToken {
  string public name = "CryptoWorkPlace Token";
  string public symbol = "CWP";
  uint public decimals = 18;
}
