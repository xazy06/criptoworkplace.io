pragma solidity ^0.4.18;

import "openzeppelin-solidity/contracts/crowdsale/validation/TimedCrowdsale.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";

/**
 * @title SteppedRateCrowdsale
 * @dev Extension of Crowdsale contract that count steps. 
 */
contract SteppedCrowdsale is TimedCrowdsale {
  using SafeMath for uint256;

  
  mapping (uint256 => uint8) private _stepsMap;
  uint256[] private _stepsKeyList;
  
  function _addStep(uint256 dueDate) internal {
    require(openingTime < dueDate);
    require(closingTime > dueDate);
    require(_stepsKeyList.length < 255);
    require(_stepsKeyList.length == 0 || _stepsMap[_stepsKeyList[_stepsKeyList.length - 1]] < dueDate);
    
    _stepsMap[dueDate] = uint8(_stepsKeyList.push(dueDate));
  }

  /**
   * @dev Returns current step number. 
   * @return Current step number
   */
  function getCurrentStep() public view returns (uint8) {
    uint256 key;
    for(uint8 i = 0; i < _stepsKeyList.length;i++)
    {
      key = _stepsKeyList[i];
      // solium-disable-next-line security/no-block-members
      if (block.timestamp < key)
        break;
    }
    
    return _stepsMap[key];
  }

  function getStepsCout() public view returns (uint8) {
    return uint8(_stepsKeyList.length);
  }
}
