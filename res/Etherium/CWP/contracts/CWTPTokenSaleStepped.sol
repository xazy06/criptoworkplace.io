pragma solidity 0.4.24;

import "./CWTPToken.sol";
import "./SteppedRateCrowdsale.sol";
import "./SteppedCapCrowdsale.sol";
import "./PostDeliveryCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "./RBACWithAdmin.sol";

contract CWTPTokenSaleStepped is PostDeliveryCrowdsale, MintedCrowdsale, RBACWithAdmin, SteppedRateCrowdsale, SteppedCapCrowdsale {

  string public constant ROLE_DAPP = "dapp";
  string public constant ROLE_SRV = "service";

  mapping (address => uint256) public deposited;
  event Refunded(address indexed beneficiary, uint256 weiAmount);
  uint256 private _totalRefundDeposit;

  constructor(uint256 _startTime, uint256 _endTime, uint256 _cap, uint256 _minAmount, uint256 _rate, address _wallet, ERC20 _tokenAddress) public
    TimedCrowdsale(_startTime, _endTime)
    SteppedCapCrowdsale(_cap, _minAmount)
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

  function getPriceForTokens(uint256 amount) public view returns(uint256) {
    return amount.mul(getCurrentRate()).div(getEthUsdRate());
  }

  function transferTokenOwnership() onlyAdmin public
  {
    // solium-disable-next-line security/no-block-members
    require(closingTime < block.timestamp);
    Ownable(token).transferOwnership(msg.sender);
  }

  function _processPurchase(address _beneficiary, uint256 _tokenAmount) internal {
    uint256 dep = (_tokenAmount % 1 ether);
    uint256 currentRate = getCurrentRate();
    uint256 refund = dep.mul(currentRate).div(getEthUsdRate());
    deposited[msg.sender] = deposited[msg.sender].add(refund);
    _totalRefundDeposit = _totalRefundDeposit.add(refund);
    super._processPurchase(_beneficiary, _tokenAmount.sub(dep));
  }

  function _forwardFunds() internal {
    uint256 funds = msg.value;
    if (address(this).balance.sub(funds) < _totalRefundDeposit)
    {
      funds = funds.sub(_totalRefundDeposit.sub(address(this).balance.sub(funds)));
    }
    wallet.transfer(funds);
  }

  function refund(address investor) public {
    require(deposited[investor] > 0);
    uint256 depositedValue = deposited[investor];
    deposited[investor] = 0;
    investor.transfer(depositedValue);
    emit Refunded(investor, depositedValue);
  }

  function CloseContract() onlyAdmin public {
    require(hasClosed());
    _withdrawTokens();
    if(Ownable(token).owner() == address(this))
      transferTokenOwnership();
    selfdestruct(msg.sender);
  }
}
