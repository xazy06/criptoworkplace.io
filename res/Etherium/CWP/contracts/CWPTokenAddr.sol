contract CWPTokenAddr {
  address internal tokenAddress;

  function CWPTokenAddr(address _tokenAddress) public
  {
    require(_tokenAddress != address(0));
    tokenAddress = _tokenAddress;
  }
}
