pragma solidity 0.4.24;

import "openzeppelin-solidity/contracts/crowdsale/Crowdsale.sol";
import "openzeppelin-solidity/contracts/token/ERC20/ERC20.sol";
import "openzeppelin-solidity/contracts/token/ERC20/SafeERC20.sol";


contract ConvertTokens is Crowdsale {

  /**
   * event for token convert logging
   * @param token that converted
   * @param converter who convert tokens
   * @param beneficiary who got the tokens
   * @param amount amount of tokens converted
   */
  event TokenConvert(address indexed token, address indexed converter, address indexed beneficiary, uint256 amount);

  using SafeERC20 for ERC20;

  ERC20 internal _token;
  uint256 internal _convertRate;

  constructor(ERC20 token, uint256 convertRate) public
  {
    require(token != address(0));
    require(convertRate > 0);
    _token = token;
    _convertRate = convertRate;
  }

  function convert(uint256 _value) public returns (bool)
  {
    require(msg.sender != address(0));
    require(_value > 0);
    _token.safeTransferFrom(msg.sender, address(this), _value);
    _deliverTokens(msg.sender, _value);
    emit TokenConvert(_token, msg.sender, msg.sender, _value);
    return true;
  }
}