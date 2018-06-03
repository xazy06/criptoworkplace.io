pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/crowdsale/validation/TimedCrowdsale.sol";
import "openzeppelin-solidity/contracts/token/ERC20/ERC20.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";


/**
 * @title PostDeliveryCrowdsale
 * @dev Crowdsale that locks tokens from withdrawal until it ends.
 */
contract PostDeliveryCrowdsale is TimedCrowdsale {
  using SafeMath for uint256;

  mapping(address => uint256) public balances;
  address[] private _balancesList;

  /**
   * @dev Withdraw tokens only after crowdsale ends.
   */
  function _withdrawTokens() internal {
    require(hasClosed());
    for(uint256 i = 0; i < _balancesList.length; i++)
    {
      uint256 amount = balances[_balancesList[i]];
      require(amount > 0);
      balances[_balancesList[i]] = 0;
      _deliverTokens(_balancesList[i], amount);
    }    
  }

  /**
   * @dev Overrides parent by storing balances instead of issuing tokens right away.
   * @param _beneficiary Token purchaser
   * @param _tokenAmount Amount of tokens purchased
   */
  function _processPurchase(address _beneficiary, uint256 _tokenAmount) internal {
    if (balances[_beneficiary] == 0)
      _balancesList.push(_beneficiary);
    balances[_beneficiary] = balances[_beneficiary].add(_tokenAmount);
  }

}
