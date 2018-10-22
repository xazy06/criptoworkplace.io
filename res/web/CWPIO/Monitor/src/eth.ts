import Web3 from "web3"

const timeout = ms => new Promise(res => setTimeout(res, ms))

export class Eth {
  private _provString: string;
  
  private _web3: Web3;
  public get web3(): Web3 {
    if (!this._web3)
      this._web3 = new Web3(Web3.givenProvider || this._provString);
    return this._web3;
  }

  constructor(providerString: string) {
    this._provString = providerString;
  }

  private async waitForConnect() {
    while (!(<any>this.web3.currentProvider).connected) {
      await timeout(500);
    }
  }
  public async getTransactionStatus(txHash: string): Promise<ExchangeOperationStatus> {
    await this.waitForConnect();
    console.log('check transaction \'' + txHash + '\'');
    var tx = await this.web3.eth.getTransaction(txHash);
    if (tx === null || tx === undefined) {
      return ExchangeOperationStatus.Skip;
    }
    else {
      let result: boolean = tx.blockNumber !== null;
      if (result) {
        var receipt = await this.web3.eth.getTransactionReceipt(txHash);
        result = result && (!!receipt.status);
      }
      return result ? ExchangeOperationStatus.Ok : ExchangeOperationStatus.Failed;
    }
  }

  async getTransactionAmount(txHash: string): Promise<string> {
    await this.waitForConnect();
    var tx = await this.web3.eth.getTransaction(txHash);
    if (tx === null || tx === undefined) {
      return "0";
    }
    return Web3.utils.fromWei(tx.value, "ether");
  }
}


export enum ExchangeOperationStatus {
  Skip = 0,
  Ok = 1,
  Failed = 2
}
