pragma solidity ^0.4.18;

import "./CWTPToken.sol";
import "./TokenAddr.sol";
import "zeppelin-solidity/contracts/crowdsale/CappedCrowdsale.sol";
import "zeppelin-solidity/contracts/ownership/Ownable.sol";

contract CWTPTokenSale is TokenAddr, CappedCrowdsale, Ownable {
    /**
   * CWTPTokenSale - Contract for CryptoWorkPlace Token Pre-Sale
   *
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _endTime  Token Sale end time in seconds
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @param  _tokenAddress  Address of Pre-sale CWPToken contract, deployed to blockchain
   */
  function CWTPTokenSale(uint256 _startTime, uint256 _endTime, address _wallet, address _tokenAddress) public
    TokenAddr(_tokenAddress)
    CappedCrowdsale(1351 * 1 ether)
    Crowdsale(_startTime, _endTime, 1000, _wallet)
  {
  }

  /*Use CWTPToken for sale*/
  function createTokenContract() internal returns (MintableToken) {
    return CWTPToken(tokenAddress);
  }

  function setRate(uint256 _value) onlyOwner public
  {
    require(_value > 0);
    rate = _value;
  }

  function mint(address beneficiary, uint256 tokens) onlyOwner public
  {
    token.mint(beneficiary, tokens);
  }

  function getBackMyTokenPlz() onlyOwner public
  {
    require(endTime < now);
    token.transferOwnership(owner);
  }
}
