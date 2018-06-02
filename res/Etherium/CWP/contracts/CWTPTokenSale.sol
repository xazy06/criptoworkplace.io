pragma solidity 0.4.24;

import "./CWTPToken.sol";
import "./SteppedRateCrowdsale.sol";
import "./SteppedCapCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/distribution/PostDeliveryCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "openzeppelin-solidity/contracts/ownership/rbac/RBACWithAdmin.sol";

contract CWTPTokenSale is PostDeliveryCrowdsale, MintedCrowdsale, RBACWithAdmin, SteppedRateCrowdsale, SteppedCapCrowdsale {

  string public constant ROLE_DAPP = "dapp";
  string public constant ROLE_SRV = "service";

  constructor(uint256 _startTime, uint256 _endTime, uint256 _cap, uint256 _rate, address _wallet, ERC20 _tokenAddress) public
    TimedCrowdsale(_startTime, _endTime)
    SteppedCapCrowdsale(_cap)
    Crowdsale(_rate, _wallet, _tokenAddress)
  {
    require(Ownable(_tokenAddress) != address(0));
    addRole(msg.sender, ROLE_DAPP);
    addRole(msg.sender, ROLE_SRV);
    //addRole(address(0x000000000000000000000), ROLE_DAPP);
  }

  function addCrowdsaleStep(uint256 _timestamp, uint256 _cap, uint256 _rate) onlyRole(ROLE_DAPP) public{
    uint8 step = _addStep(_timestamp);
    _setStepCap(step, _cap);
    _setStepRate(step, _rate);
  }

  function setEthUsdRate(uint256 usdRate) onlyRole(ROLE_SRV) public{
    _setEthUsdRate(usdRate);
  }

  function transferTokenOwnership() onlyAdmin public
  {
    // solium-disable-next-line security/no-block-members
    require(closingTime < block.timestamp);
    Ownable(token).transferOwnership(msg.sender);
  }

  function _getTokenAmount(uint256 _weiAmount) internal view returns (uint256) {
    uint256 tokens = super._getTokenAmount(_weiAmount);
    require((tokens % 1 ether) == 0);
    return tokens;    
  }
}
