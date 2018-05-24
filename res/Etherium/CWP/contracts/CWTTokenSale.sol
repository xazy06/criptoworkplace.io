pragma solidity ^0.4.23;

import "./CWTToken.sol";
import "./CWTPToken.sol";
import "openzeppelin-solidity/contracts/crowdsale/distribution/PostDeliveryCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/CappedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

contract CWPTokenSale is CappedCrowdsale, PostDeliveryCrowdsale, MintedCrowdsale  {
  
  CWTPToken public presaleToken;

  /**
   * event for token convert logging
   * @param converter who convert presale tokens
   * @param beneficiary who got the tokens
   * @param amount amount of tokens converted
   */
  event PresaleTokenConvert(address indexed converter, address indexed beneficiary, uint256 amount);

  /**
   * CWPTokenSale - Contract for CryptoWorkPlace Token Sale
   *
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _duration  Token Sale duration in days
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @param  _tokenAddress  Address of Pre-sale CWPToken contract, deployed to blockchain
   */
  constructor(uint256 _startTime, uint8 _duration, address _wallet, CWTToken _tokenAddress, CWTPToken _presaleTokenAddress) public
    CappedCrowdsale(123)
    TimedCrowdsale(_startTime, _startTime + _duration * 1 days)
    Crowdsale(1000, _wallet, _tokenAddress)
  {
    require(_presaleTokenAddress != address(0));
    presaleToken = _presaleTokenAddress;
  }

  function convertFromPresale(uint256 _value) public returns (bool)
  {
    require(msg.sender != address(0));
    require(_value > 0);
    presaleToken.transferFrom(msg.sender, address(this), _value);
    _deliverTokens(msg.sender, _value);
    emit PresaleTokenConvert(msg.sender, msg.sender, _value);
    return true;
  }
}
