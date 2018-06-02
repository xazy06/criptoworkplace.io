pragma solidity 0.4.24;

import "./CWTToken.sol";
import "./CWTPToken.sol";
import "openzeppelin-solidity/contracts/crowdsale/distribution/PostDeliveryCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";

contract CWPTokenSale is PostDeliveryCrowdsale, MintedCrowdsale  {

  /**
   * CWPTokenSale - Contract for CryptoWorkPlace Token Sale
   *
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _duration  Token Sale duration in days
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @param  _tokenAddress  Address of Pre-sale CWPToken contract, deployed to blockchain
   */
  constructor(uint256 _startTime, uint8 _duration, address _wallet, CWTToken _tokenAddress) public
    //CappedCrowdsale(123)
    TimedCrowdsale(_startTime, _startTime + _duration * 1 days)
    Crowdsale(1000, _wallet, _tokenAddress)
  {
    // require(_presaleTokenAddress != address(0));
    // presaleToken = _presaleTokenAddress;
  }
}
