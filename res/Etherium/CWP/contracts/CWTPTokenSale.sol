pragma solidity 0.4.20;

import "./CWTPToken.sol";
import "./SteppedRateCrowdsale.sol";
import "./SteppedCapCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/distribution/PostDeliveryCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "openzeppelin-solidity/contracts/ownership/rbac/RBAC.sol";

contract CWTPTokenSale is PostDeliveryCrowdsale, MintedCrowdsale, RBAC, SteppedRateCrowdsale, SteppedCapCrowdsale {

  string public constant ROLE_DAPP = "dapp";

  function CWTPTokenSale(uint256 _startTime, uint256 _endTime, address _wallet, ERC20 _tokenAddress) public
    TimedCrowdsale(_startTime, _endTime)
    Crowdsale(1, _wallet, _tokenAddress)
  {
    require(Ownable(_tokenAddress) != address(0));
    addRole(msg.sender, ROLE_DAPP);
    //addRole(address(0x000000000000000000000), ROLE_DAPP);
  }

  function addCrowdsaleStep(uint256 _timestamp, uint256 _cap, uint256 _rate) onlyRole(ROLE_DAPP) public{
    _addStep(_timestamp);
    _setStepCap(getStepsCout() - 1, _cap);
    _setStepRate(getStepsCout() - 1, _rate);
  }

  function transferTokenOwnership() onlyAdmin public
  {
    // solium-disable-next-line security/no-block-members
    require(closingTime < block.timestamp);
    Ownable(token).transferOwnership(msg.sender);
  }
}
