pragma solidity ^0.4.18;

import "./CWTToken.sol";
import "./CWTPToken.sol";
import "zeppelin-solidity/contracts/token/MintableToken.sol";
import "zeppelin-solidity/contracts/crowdsale/CappedCrowdsale.sol";
import "zeppelin-solidity/contracts/crowdsale/RefundableCrowdsale.sol";
import "zeppelin-solidity/contracts/math/SafeMath.sol";

contract CWPTokenAddr {
  address internal tokenAddress;

  function CWPTokenAddr(address _tokenAddress) public
  {
    require(_tokenAddress != address(0));
    tokenAddress = _tokenAddress;
  }
}

contract CWPTokenSale is CWPTokenAddr, CappedCrowdsale, RefundableCrowdsale {
  uint256 public constant REFUND_CAP = 1802 * 1 ether;
  uint16 public constant RATE50 = 13320;
  uint256 public constant CAP50 = 2252 * 1 ether;
  uint16 public constant RATE25 = 8880;
  uint256 public constant CAP25 = CAP50 + (7883 * 1 ether);
  uint16 public constant RATE0 = 6660;
  uint256 public constant CAP0 = CAP25 + (22523 * 1 ether);
  pCWTToken public presaleToken;

  /**
   * event for token convert logging
   * @param converter who convert presale tokens
   * @param beneficiary who got the tokens
   * @param amount amount of tokens converted
   */
  event PresaleTokenConvert(address indexed converter, address indexed beneficiary, uint256 amount);

  modifier checkNextRate() {
    if (rate == RATE50 && weiRaised >= CAP50 * 1 ether) {
      rate = RATE25;
    } else if (rate == RATE25 && weiRaised >= CAP25 * 1 ether) {
      rate = RATE0;
    }
    _;
  }

  /**
   * CWPTokenSale - Contract for CryptoWorkPlace Token Sale
   *
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _duration  Token Sale duration in days
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @param  _tokenAddress  Address of Pre-sale CWPToken contract, deployed to blockchain
   */
  function CWPTokenSale(uint256 _startTime, uint8 _duration, address _wallet, address _tokenAddress, address _presaleTokenAddress) public
    CWPTokenAddr(_tokenAddress)
    CappedCrowdsale(CAP0)
    FinalizableCrowdsale()
    RefundableCrowdsale(REFUND_CAP)
    Crowdsale(_startTime, _startTime + _duration * 1 days, RATE50, _wallet)
  {
    require(_presaleTokenAddress != address(0));
    presaleToken = pCWTToken(_presaleTokenAddress);
  }

  function buyTokens(address beneficiary) checkNextRate public payable{
    super.buyTokens(beneficiary);
  }

  /*Use CWPToken for sale*/
  function createTokenContract() internal returns (MintableToken) {
    //return new MintableToken();
    return CWPToken(tokenAddress);
  }

  function convertFromPresale(uint256 _value) public returns (bool)
  {
    require(msg.sender != address(0));
    require(_value > 0);
    presaleToken.transferFrom(msg.sender, address(this), _value);
    token.mint(msg.sender, _value);
    PresaleTokenConvert(msg.sender, msg.sender, _value);
    return true;
  }
}
