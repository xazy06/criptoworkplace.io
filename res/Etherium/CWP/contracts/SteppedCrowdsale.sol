pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/crowdsale/validation/TimedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedRateCrowdsale
 * @dev Extension of Crowdsale contract that count steps. 
 */
contract SteppedCrowdsale is TimedCrowdsale {
  using SafeMath
  for uint256;

  event AddStep(uint256 indexed timestamp, uint256 step, uint256 dueDate);

  mapping(uint256 => uint8) private _stepsMap;
  uint256[] private _stepsKeyList;
  mapping(uint8 => uint256) public steps;

  constructor() public {
    _stepsMap[closingTime] = uint8(1);
    steps[uint8(1)] = closingTime;
  }

  function _addStep(uint256 dueDate) internal returns(uint8) {
    require(openingTime < dueDate);
    require(closingTime > dueDate);
    require(_stepsKeyList.length < 255);
    require(_stepsKeyList.length == 0 || _stepsMap[_stepsKeyList[_stepsKeyList.length - 1]] < dueDate);

    _stepsMap[dueDate] = uint8(_stepsKeyList.push(dueDate));
    _stepsMap[closingTime] = uint8(_stepsKeyList.length + 1);
    steps[_stepsMap[dueDate]] = dueDate;
    steps[_stepsMap[closingTime]] = closingTime;
    // solium-disable-next-line security/no-block-members
    emit AddStep(block.timestamp, _stepsMap[dueDate], dueDate);
    return _stepsMap[dueDate];
  }

  /**
   * @dev Returns current step number. 
   * @return Current step number
   */
  function getCurrentStep() public view returns(uint8) {
    uint256 key;
    for (uint8 i = 0; i < _stepsKeyList.length; i++) {
      key = _stepsKeyList[i];
      // solium-disable-next-line security/no-block-members
      if (block.timestamp < key)
        break;
    }

    // solium-disable-next-line security/no-block-members
    if (block.timestamp > key)
    {
      key = closingTime;
    }

    return _stepsMap[key];
  }

  function getStepsCout() public view returns(uint8) {
    return uint8(_stepsKeyList.length + 1);
  }
}