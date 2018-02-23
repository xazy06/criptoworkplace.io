pragma solidity ^0.4.18;

import "./CWTPToken.sol";
import "./TokenAddr.sol";
import "zeppelin-solidity/contracts/crowdsale/CappedCrowdsale.sol";
import "zeppelin-solidity/contracts/ownership/Ownable.sol";

contract CWPPTokenSale is TokenAddr, CappedCrowdsale, Ownable {
    /**
   * CWPTokenSale - Contract for CryptoWorkPlace Token Sale
   *
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _duration  Token Sale duration in days
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @param  _tokenAddress  Address of Pre-sale CWPToken contract, deployed to blockchain
   */
  function CWPPTokenSale(uint256 _startTime, uint8 _duration, address _wallet, address _tokenAddress) public
    TokenAddr(_tokenAddress)
    CappedCrowdsale(1351 * 1 ether)
    Crowdsale(_startTime, _startTime + _duration * 1 days, 1000, _wallet)
  {
  }

  /*Use CWPToken for sale*/
  function createTokenContract() internal returns (MintableToken) {
    //return new MintableToken();
    return CWTPToken(tokenAddress);
  }

  function setRate(uint256 _value) onlyOwner public
  {
    require(_value > 0);
    rate = _value;
  }
}
