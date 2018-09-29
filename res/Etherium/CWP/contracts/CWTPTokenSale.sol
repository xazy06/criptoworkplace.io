pragma solidity 0.4.24;

import "./CWTPToken.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/CappedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/TimedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/WhitelistedCrowdsale.sol";
import "./RBACWithAdmin.sol";

contract CWTPTokenSale is WhitelistedCrowdsale, MintedCrowdsale, RBACWithAdmin, CappedCrowdsale, TimedCrowdsale {

  struct FixedRate {
    uint256 rate;
    uint256 time;
    uint256 amount;
  }

  string public constant ROLE_DAPP = "dapp";
  string public constant ROLE_SRV = "service";

  mapping(address => FixedRate) public fixRate;

  constructor(uint256 _startTime, uint256 _endTime, uint256 _cap, address _wallet, ERC20 _tokenAddress) public
    TimedCrowdsale(_startTime, _endTime)
    CappedCrowdsale(_cap)
    Crowdsale(1, _wallet, _tokenAddress)
  {
    require(Ownable(_tokenAddress) != address(0));
    addRole(msg.sender, ROLE_DAPP);
    addRole(msg.sender, ROLE_SRV);
  }

  function registerDappAddress(address _dappAddr) public onlyRole(ROLE_ADMIN) {
    addRole(_dappAddr, ROLE_DAPP);
  }

/**
   * @dev Checks whether the period in which the crowdsale is open has already elapsed.
   * @return Whether crowdsale period has elapsed
   */
  function hasOpened() public view returns (bool) {
    // solium-disable-next-line security/no-block-members
    return block.timestamp > openingTime;
  }

  /**
   * @dev add an address to the whitelist
   * @param _operator address
   * @return true if the address was added to the whitelist, false if the address was already in the whitelist
   */
  function addAddressToWhitelist(address _operator) public onlyRole(ROLE_DAPP)
  {
    require(block.timestamp <= closingTime);
    addRole(_operator, ROLE_WHITELISTED);
  }

  function setRateForTransaction(uint256 newRate, address buyer, uint256 amount) public onlyIfWhitelisted(buyer) onlyRole(ROLE_DAPP) onlyWhileOpen
  {
    fixRate[buyer] = FixedRate(newRate, block.timestamp.add(15 minutes), amount);
  }

  /**
   * @dev Extend parent behavior requiring beneficiary to be in fix rate list.
   * @param _beneficiary Token beneficiary
   * @param _weiAmount Amount of wei contributed
   */
  function _preValidatePurchase(
    address _beneficiary,
    uint256 _weiAmount
  )
    internal
  {
    require(fixRate[_beneficiary].time > block.timestamp);
    require(fixRate[_beneficiary].amount == _weiAmount);
    rate = fixRate[_beneficiary].rate;

    super._preValidatePurchase(_beneficiary, _weiAmount);
  }

  /**
   * @dev Allows the current owner to relinquish control of the contract.
   * @notice Disable renounce ownership
   */
  function renounceOwnership() public onlyOwner {
  }

  function transferTokenOwnership() onlyAdmin public
  {
    // solium-disable-next-line security/no-block-members
    require(hasClosed());
    Ownable(token).transferOwnership(msg.sender);
  }

  function CloseContract() onlyAdmin public {
    require(hasClosed());
    if(Ownable(token).owner() == address(this))
      transferTokenOwnership();
    selfdestruct(msg.sender);
  }
}
