import { Eth, ExchangeOperationStatus } from "./eth";
import { Database, ExchangeTransaction } from "./db";
import { Subscribe } from "web3/types";
import { BlockHeader } from "web3/eth/types";
import { DH_UNABLE_TO_CHECK_GENERATOR } from "constants";


class Startup {

    private _db: Database;
    public get db(): Database {
        if (!this._db)
            this._db = new Database(process.env.CWP_DATABASE_URL);
        return this._db;
    }

    private _eth: Eth;
    public get eth(): Eth {
        if (!this._eth)
            this._eth = new Eth("wss://ropsten.infura.io/_ws");
        return this._eth;
    }

    private monitored = {};

    private async nextStage(item: ExchangeTransaction): Promise<void> {
        if (item.startTx === item.currentTx) {
            let amount = await this.eth.getTransactionAmount(item.startTx);
            console.log('send', amount, 'to contract');
        }
        console.log('move to next transaction');
    }

    private async run(): Promise<void> {
        let res = await this.db.getActiveExchangeTransactions();
        res.forEach(item => this.monitored[item.startTx] = item);
        console.log(this.monitored);

        await this.eth.web3.eth.subscribe("newBlockHeaders",
            async () => {
                for (const key in this.monitored) {
                    if (this.monitored.hasOwnProperty(key)) {
                        const item: ExchangeTransaction = this.monitored[key];
                        this.processExchangeItem(item);
                    }
                }
            });

    }

    private async processExchangeItem(item: ExchangeTransaction): Promise<void> {
        let status: ExchangeOperationStatus = await this.eth.getTransactionStatus(item.currentTx);
        console.log('Tx:', item.currentTx, 'Status:', status);
        switch (status) {
            case ExchangeOperationStatus.Ok:
                await this.nextStage(item);
                break;

            case ExchangeOperationStatus.Skip:
                break;
            case ExchangeOperationStatus.Failed:
                await this.db.markAsFailed(item.id);
                break;

        }
    }

    public static main(): number {
        (async () => {
            try {
                console.log('begin');
                let startup = new Startup();
                await startup.run();
                console.log('done');
            } catch (e) {
                // Deal with the fact the chain failed
                console.error(e);
            }
        })();
        return 0;
    }
}

Startup.main();



