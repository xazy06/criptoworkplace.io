pragma solidity ^0.4.18;

import "./SteppedCrowdsale.sol";
import "./oraclizeAPI_0.4.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedRateCrowdsale
 * @dev Extension of Crowdsale contract that increases the price of tokens by steps. 
 */
contract SteppedRateCrowdsale is SteppedCrowdsale, usingOraclize {
  using SafeMath for uint256;

  event UsdRateUpdated(uint256 indexed timestamp, uint256 oldRate, uint256 newRate);

  mapping (uint8 => uint256) private _rates;
  uint256 public ETH_USD;

  function getStepRate(uint8 step) public view returns(uint256) {
    require(step <= getStepsCout());
    return _rates[step];
  }

  function _setStepRate(uint8 step, uint256 rate) internal {
    require(step <= getStepsCout());
    _rates[step] = rate;
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
   * @dev Overrides parent method taking into account variable rate.
   * @param _weiAmount The value in wei to be converted into tokens
   * @return The number of tokens _weiAmount wei will buy at present time
   */
  function _getTokenAmount(uint256 _weiAmount) internal view returns (uint256) {
    uint256 currentRate = getCurrentRate();
    return currentRate.mul(_weiAmount);
  }

  function __callback(bytes32 myid, string result) public {
    if (msg.sender != oraclize_cbAddress()) revert();
    //UsdRateUpdated(block.timestamp,ETH_USD, result);
    //ETH_USD = result;
    _updatePrice();
  }

  function _updatePrice() internal {
    if (oraclize_getPrice("URL") <= this.balance) {
      oraclize_query("1800","URL", "json(https://api.coinmarketcap.com/v2/ticker/1027/?convert=USD).data.quotes.USD.price");
    }
  }
}
