import { Pool } from "pg";

export class Database {
  private _connectionString: string;

  private _pool: Pool;
  public get pool(): Pool {
    if (!this._pool)
      this._pool = new Pool({ connectionString: this._connectionString });
    return this._pool;
  }

  constructor(connectionString: string) {
    this._connectionString = connectionString;
  }

  /**
   * getActiveExchangeTransactions
   */
  public async getActiveExchangeTransactions(): Promise<Array<ExchangeTransaction>> {
    const res = await this.pool.query("SELECT es.id, u.id as user_id, concat('0x',encode(u.eth_address,'hex')) as eth_address, es.start_tx, es.current_tx FROM exchange.exchange_status es \
inner join identity.users u on es.created_by_user_id = u.id WHERE es.is_ended = false AND es.is_failed = false");
    await this.pool.end();

    return res.rows.map(r => new ExchangeTransaction(r.id, r.user_id, r.eth_address, r.start_tx, r.current_tx));
  }

  /**
   * markAsFailed
   */
  public async markAsFailed(id: string): Promise<void> {
    await this.pool.query("UPDATE exchange.exchange_status SET is_failed = true WHERE id = $1", [id]);
    await this.pool.end();
  }

  /**
   * markAsEnded
   */
  public async markAsEnded(id: string): Promise<void> {
    await this.pool.query("UPDATE exchange.exchange_status SET is_ended = true WHERE id = $1", [id]);
    await this.pool.end();
  }

  /**
   * setCurrentTransaction
   */
  public async setCurrentTransaction(id: string, transaction: string): Promise<void> {
    await this.pool.query("UPDATE exchange.exchange_status SET current_tx = $2 WHERE id = $1", [id, transaction]);
    await this.pool.end();
  }
}

export class ExchangeTransaction {
  constructor(id: string, userId: string, ethAddress: string, startTx: string, currentTx: string) {
    this.id = id;
    this.userId = userId;
    this.ethAddress = ethAddress;
    this.startTx = startTx;
    this.currentTx = currentTx;
  }

  private _id: string;
  public get id(): string {
    return this._id;
  }
  public set id(v: string) {
    this._id = v;
  }

  private _userId: string;
  public get userId(): string {
    return this._userId;
  }
  public set userId(v: string) {
    this._userId = v;
  }


  private _ethAddress: string;
  public get ethAddress(): string {
    return this._ethAddress;
  }
  public set ethAddress(v: string) {
    this._ethAddress = v;
  }


  private _startTx: string;
  public get startTx(): string {
    return this._startTx;
  }
  public set startTx(v: string) {
    this._startTx = v;
  }


  private _currentTx: string;
  public get currentTx(): string {
    return this._currentTx;
  }
  public set currentTx(v: string) {
    this._currentTx = v;
  }  
}