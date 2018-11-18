pragma solidity 0.4.24;

import "./CWTPToken.sol";
import "openzeppelin-solidity/contracts/math/SafeMath.sol";
import "openzeppelin-solidity/contracts/crowdsale/emission/MintedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/CappedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/TimedCrowdsale.sol";
import "openzeppelin-solidity/contracts/crowdsale/validation/WhitelistedCrowdsale.sol";
import "openzeppelin-solidity/contracts/payment/PullPayment.sol";
import "openzeppelin-solidity/contracts/token/ERC20/CappedToken.sol";
import "./RBACWithAdmin.sol";

contract CWTPTokenSale is WhitelistedCrowdsale, MintedCrowdsale, RBACWithAdmin, TimedCrowdsale, PullPayment {

  using SafeMath for uint256;

  struct FixedRate {
    uint256 rate;
    uint256 time;
    uint256 amount;
  }

  string public constant ROLE_DAPP = "dapp";
  string public constant ROLE_SRV = "service";

  mapping(address => FixedRate) public fixRate;
  uint256 public tokenCap;
  uint256 public tokenSold;
  FixedRate private _currentFRate;

  constructor(uint256 _startTime, uint256 _endTime, address _wallet, CappedToken _tokenAddress) public
    TimedCrowdsale(_startTime, _endTime)
    Crowdsale(1, _wallet, _tokenAddress)
  {
    require(Ownable(_tokenAddress) != address(0));
    tokenCap = _tokenAddress.cap();
    tokenSold = _tokenAddress.totalSupply();

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
    return block.timestamp >= openingTime && block.timestamp <= closingTime;
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
   * @dev Checks whether the cap has been reached.
   * @return Whether the cap was reached
   */
  function capReached() public view returns (bool) {
    return tokenSold >= tokenCap;
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
    require(_weiAmount >= fixRate[_beneficiary].amount);
    _currentFRate = fixRate[_beneficiary];

    super._preValidatePurchase(_beneficiary, _weiAmount);
  }

  /**
   * @dev Override to extend the way in which ether is converted to tokens.
   * @param _weiAmount Value in wei to be converted into tokens
   * @return Number of tokens that can be purchased with the specified _weiAmount
   */
  function _getTokenAmount(uint256 _weiAmount)
    internal view returns (uint256)
  {
    //_weiAmount

    uint256 tokenAmount = _currentFRate.amount.mul(_currentFRate.rate).div(1 ether).mul(1 ether);
    require(tokenSold.add(tokenAmount) <= tokenCap);
    return tokenAmount;
  }

  /**
   * @dev Executed when a purchase has been validated and is ready to be executed. Not necessarily emits/sends tokens.
   * @param _beneficiary Address receiving the tokens
   * @param _tokenAmount Number of tokens to be purchased
   */
  function _processPurchase(
    address _beneficiary,
    uint256 _tokenAmount
  )
    internal
  {
    tokenSold = tokenSold.add(_tokenAmount);
    super._processPurchase(_beneficiary, _tokenAmount);
  }


  /**
   * @dev Determines how ETH is stored/forwarded on purchases.
   */
  function _forwardFunds() internal {
    uint256 refund = msg.value - _currentFRate.amount;
    weiRaised.sub(refund); 
    wallet.transfer(_currentFRate.amount);
    asyncTransfer(msg.sender, refund);
  }

  /**
   * @dev Validation of an executed purchase. Observe state and use revert statements to undo rollback when valid conditions are not met.
   * @param _beneficiary Address performing the token purchase
   * @param _weiAmount Value in wei involved in the purchase
   */
  function _postValidatePurchase(
    address _beneficiary,
    uint256 _weiAmount
  )
    internal
  {
    fixRate[_beneficiary].time = block.timestamp;
  }


  /**
   * @dev Allows the current owner to relinquish control of the contract.
   * @notice Disable renounce ownership
   */
  function renounceOwnership() public onlyOwner {
  }

  function transferTokenOwnership() onlyAdmin private
  {
    // solium-disable-next-line security/no-block-members
    require(hasClosed());
    Ownable(token).transferOwnership(msg.sender);
  }

  function forceTransferTokenOwnership() onlyOwner private
  {
    // solium-disable-next-line security/no-block-members
    Ownable(token).transferOwnership(msg.sender);
  }

  function CloseContract() onlyAdmin public {
    require(hasClosed());
    if(Ownable(token).owner() == address(this))
      transferTokenOwnership();
    //selfdestruct(msg.sender);
  }

  function ForceCloseContract() onlyOwner public {
    if(Ownable(token).owner() == address(this))
      forceTransferTokenOwnership();
    selfdestruct(msg.sender);
  }
}
