pragma solidity ^0.4.18;

import "./CWPToken.sol";
import "zeppelin-solidity/contracts/crowdsale/CappedCrowdsale.sol";
import "zeppelin-solidity/contracts/crowdsale/RefundableCrowdsale.sol";

contract CWPTokenSale is CappedCrowdsale,RefundableCrowdsale {

  address public token;

  function CWPTokenSale(address _token, uint256 _startTime, uint256 _endTime, uint256 _rate, uint256 _goal, uint256 _cap, address _wallet) public
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
