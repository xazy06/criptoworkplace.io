pragma solidity ^0.4.18;

import "./SteppedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedCapCrowdsale
 * @dev Extension of Crowdsale contract that increases the price of tokens by steps. 
 */
contract SteppedCapCrowdsale is SteppedCrowdsale {
  using SafeMath for uint256;

  mapping (uint8 => uint256) private _caps;
  mapping (uint8 => uint256) private _tokensSold;

  function getStepCap(uint8 _step) public view returns(uint256) {
    require(_step <= getStepsCout());
    return _caps[_step];
  }

  function getStepTokenSold(uint8 _step) public view returns(uint256) {
    require(_step <= getStepsCout());
    return _tokensSold[_step];
  }

  function _setStepCap(uint8 _step, uint256 _cap) internal {
    require(_step <= getStepsCout());
    _caps[_step] = _cap;
  }

  function _setStepTokenSold(uint8 _step, uint256 _tokens) internal {
    require(_step <= getStepsCout());
    _tokensSold[_step] = _tokensSold[_step].add(_tokens);
  }

  function getCurrentCap() public view returns (uint256) {
    return getStepCap(getCurrentStep());
  }
  function getCurrentTokenSold() public view returns (uint256) {
    return getStepTokenSold(getCurrentStep());
  }

  /**
   * @dev Extend parent behavior requiring purchase to respect the funding cap.
   * @param _beneficiary Token purchaser
   * @param _weiAmount Amount of wei contributed
   */
  function _preValidatePurchase(address _beneficiary, uint256 _weiAmount) internal {
    super._preValidatePurchase(_beneficiary, _weiAmount);
    uint256 tokens = _getTokenAmount(_weiAmount);    
    require(getCurrentTokenSold().add(tokens) <= getCurrentCap());
  }

  function _updatePurchasingState(address _beneficiary, uint256 _weiAmount) internal {
    super._updatePurchasingState(_beneficiary, _weiAmount);
    uint256 tokens = _getTokenAmount(_weiAmount);   
    _setStepTokenSold(getCurrentStep(), tokens);
  }
}
