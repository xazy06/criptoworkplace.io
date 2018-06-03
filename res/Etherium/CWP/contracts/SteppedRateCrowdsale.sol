pragma solidity 0.4.24;

import "./SteppedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedRateCrowdsale
 * @dev Extension of Crowdsale contract that increases the price of tokens by steps. 
 */
contract SteppedRateCrowdsale is SteppedCrowdsale {
  using SafeMath for uint256;

  event UsdRateUpdated(uint256 indexed timestamp, uint256 oldRate, uint256 newRate);
  event SetStepRate(uint256 indexed timestamp, uint256 step, uint256 oldRate, uint256 newRate);

  mapping (uint8 => uint256) private _rates;
  uint256 private _ETH_USD;

  function getStepRate(uint8 step) public view returns(uint256) {
    require(step > 0 && step <= getStepsCout());
    if (_rates[step] == 0)
      return rate;
    return _rates[step];
  }

  function _setStepRate(uint8 step, uint256 rate) internal {
    require(step > 0 && step <= getStepsCout());
    uint256 oldRate = _rates[step];
    _rates[step] = rate;
    // solium-disable-next-line security/no-block-members
    emit SetStepRate(block.timestamp, step, oldRate, _rates[step]);
  }

  /**
   * @dev Returns the rate of tokens per wei at the present time. 
   * Note that, as price _increases_ with time, the rate _decreases_. 
   * @return The number of tokens a buyer gets per wei at a given time
   */
  function getCurrentRate() public view returns (uint256) {
    return getStepRate(getCurrentStep());
  }

  /**
   * @dev Overrides arent method taking into account variable rate.
   * @param _weiAmount The value in wei to be converted into tokens
   * @return The number of tokens _weiAmount wei will buy at present time
   */
  function _getTokenAmount(uint256 _weiAmount) internal view returns (uint256) {
    uint256 currentRate = getCurrentRate();
    uint256 tokens = _weiAmount.mul(_ETH_USD).div(currentRate);
    return tokens;
  }

  function getEthUsdRate() public view returns(uint256) {
    return _ETH_USD;
  }

  function _setEthUsdRate(uint256 usdRate) internal {
    uint256 oldRate = _ETH_USD;
    _ETH_USD = usdRate;
    // solium-disable-next-line security/no-block-members
    emit UsdRateUpdated(block.timestamp, oldRate, _ETH_USD);
  }
}
