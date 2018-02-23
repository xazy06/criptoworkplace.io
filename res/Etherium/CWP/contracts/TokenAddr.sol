pragma solidity ^0.4.18;

contract TokenAddr {
  address internal tokenAddress;

  function TokenAddr(address _tokenAddress) public
  {
    require(_tokenAddress != address(0));
    tokenAddress = _tokenAddress;
  }
}
