pragma solidity ^0.4.18;

import "./CWPToken.sol";
import "./CappedMintCrowdsale.sol";
import "zeppelin-solidity/contracts/crowdsale/RefundableCrowdsale.sol";

contract CWPTokenSale is CappedCrowdsale,RefundableCrowdsale {

  address public token; //this maybe must be private



  /**
   * CWPTokenSale - Contract for CryptoWorkPlace Token Sale
   *
   * @param  {type} address _token     Address of CWPToken contract, deployed to blockchain
   * @param  {type} uint256 _startTime UNIX Timestamp in seconds, when Token Sale start
   * @param  {type} uint256 _duration  Token Sale duration in days
   * @param  {type} uint256 _rate      Token rate to ETH (1 Token = rate * 1 wei)
   * @param  {type} uint256 _goal      Token Sale Goal in ETH
   * @param  {type} uint256 _cap       Token Sale Cap in tokens
   * @param  {type} address _wallet    Address of multisig wallet, using for refund if Token Sale failed
   * @return {type}
   */
  function CWPTokenSale(address _token, uint256 _startTime, uint256 _duration, uint256 _rate, uint256 _goal, uint256 _cap, address _wallet) public
    CappedCrowdsale(_cap)
    FinalizableCrowdsale()
    RefundableCrowdsale(_goal)
    Crowdsale(_startTime, _endTime, _rate, _wallet)
  {
    //As goal needs to be met for a successful crowdsale
    //the value needs to less or equal than a cap which is limit for accepted funds
    require(_goal <= _cap);
    require(_token != address(0));
    token = _token;
  }

  /*Use CWPToken for sale*/
  function createTokenContract() internal returns (MintableToken) {
    return CWPToken(token);
  }
}
