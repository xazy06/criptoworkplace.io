pragma solidity ^0.4.18;

import "./CWPToken.sol";
import "zeppelin-solidity/contracts/crowdsale/CappedCrowdsale.sol";
import "zeppelin-solidity/contracts/crowdsale/RefundableCrowdsale.sol";

contract CWPTokenSale is CappedCrowdsale, RefundableCrowdsale {
  uint256 public constant REFUND_CAP = 4000 * 1 ether;
  uint8 public constant RATE50 = 6;
  uint256 public constant CAP50 = 5000 * 1 ether;
  uint8 public constant RATE25 = 4;
  uint256 public constant CAP25 = CAP50 + (17500 * 1 ether);
  uint8 public constant RATE0 = 3;
  uint256 public constant CAP0 = CAP25 + (50000 * 1 ether);

  address public token; //this maybe must be private

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
   * @param  _token     Address of CWPToken contract, deployed to blockchain
   * @param _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param _duration  Token Sale duration in days
   * @param _wallet    Address of multisig wallet, using for refund if Token Sale failed
   */
  function CWPTokenSale(address _token, uint256 _startTime, uint8 _duration, address _wallet) public
    CappedCrowdsale(CAP0)
    FinalizableCrowdsale()
    RefundableCrowdsale(REFUND_CAP)
    Crowdsale(_startTime, _startTime + _duration * 1 days, RATE50, _wallet)
  {
    //As goal needs to be met for a successful crowdsale
    //the value needs to less or equal than a cap which is limit for accepted funds
    require(_token != address(0));
    token = _token;
  }

  function buyTokens(address beneficiary) checkNextRate public payable{
    super.buyTokens(beneficiary);
  }

  /*Use CWPToken for sale*/
  function createTokenContract() internal returns (MintableToken) {
    return CWPToken(token);
  }
}
