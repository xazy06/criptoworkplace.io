using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public class Database : IDisposable
    {
        private NpgsqlConnection _connection;
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }


        public async Task<List<ExchangeTransaction>> GetActiveExchangeTransactionsAsync()
        {
            List<ExchangeTransaction> result = new List<ExchangeTransaction>();

            using (var cmd = new NpgsqlCommand(@"
SELECT 
    es.id, 
    u.id as user_id, 
    concat('0x',encode(u.eth_address,'hex')) as eth_address, 
    es.start_tx, 
    es.current_tx,
    es.eth_amount, 
    es.token_amount
FROM exchange.exchange_status es
INNER JOIN identity.users u ON es.created_by_user_id = u.id 
WHERE es.is_ended = false AND es.is_failed = false", _connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(new ExchangeTransaction
                    {
                        Id = await reader.GetFieldValueAsync<string>(0),
                        UserId = await reader.GetFieldValueAsync<string>(1),
                        ETHAddress = await reader.GetFieldValueAsync<string>(2),
                        StartTx = await reader.GetFieldValueAsync<string>(3),
                        CurrentTx = await reader.GetFieldValueAsync<string>(4),
                        EthAmount = await reader.GetFieldValueAsync<string>(5),
                        TokenAmount = await reader.GetFieldValueAsync<string>(6),
                        Status = TXStatus.Ok
                    });
                }
                return result;
            }
        }

        /**
         * markAsFailed
         */
        public async Task MarkAsFailed(string id)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET is_failed = true WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        /**
         * markAsEnded
         */
        public async Task MarkAsEnded(string id)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET is_ended = true WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }

        }

        /**
         * setCurrentTransaction
         */
        public async Task SetCurrentTransaction(string id, string transaction)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET current_tx = @tx WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("tx", transaction);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Database() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
