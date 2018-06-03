pragma solidity 0.4.24;

import "./SteppedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedCapCrowdsale
 * @dev Extension of Crowdsale contract that increases the price of tokens by steps. 
 */
contract SteppedCapCrowdsale is SteppedCrowdsale {
  using SafeMath for uint256;

  event SetStepCap(uint256 indexed timestamp, uint256 step, uint256 oldCap, uint256 newCap);

  mapping (uint8 => uint256) private _caps;
  mapping (uint8 => uint256) private _tokensSold;

  uint256 private _initCap;
  uint256 private _minAmount;

  constructor (uint256 initCap, uint256 minAmount) public {
    _initCap = initCap;
    _minAmount = minAmount;
  }

  function getStepCap(uint8 _step) public view returns(uint256) {
    require(_step > 0 && _step <= getStepsCout());
    if (_caps[_step] == 0)
      return _initCap;
    return _caps[_step];
  }

  function getStepTokenSold(uint8 _step) public view returns(uint256) {
    require(_step > 0 && _step <= getStepsCout());
    return _tokensSold[_step];
  }

  function _setStepCap(uint8 _step, uint256 _cap) internal {
    require(_step > 0 && _step <= getStepsCout());
    uint256 oldCap = _caps[_step];
    _caps[_step] = _cap;
    // solium-disable-next-line security/no-block-members
    emit SetStepCap(block.timestamp, _step, oldCap, _caps[_step]);
  }

  function _setStepTokenSold(uint8 _step, uint256 _tokens) internal {
    require(_step > 0 && _step <= getStepsCout());
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
    require(getCurrentCap().sub(getCurrentTokenSold()) < _minAmount || tokens >= _minAmount);
    require(getCurrentTokenSold().add(tokens) <= getCurrentCap());
  }

  function _updatePurchasingState(address _beneficiary, uint256 _weiAmount) internal {
    super._updatePurchasingState(_beneficiary, _weiAmount);
    uint256 tokens = _getTokenAmount(_weiAmount);   
    uint256 dep = (tokens % 1 ether);
    _setStepTokenSold(getCurrentStep(), tokens.sub(dep));
  }
}
