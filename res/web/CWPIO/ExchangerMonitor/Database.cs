using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly object syncObject = new object();

        public Database(string connectionString, ILogger<Database> logger)
        {
            _logger = logger;
            _logger.LogInformation($"Connect to database: {connectionString}");
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
    u.temp_address,
    es.start_tx, 
    es.current_tx,
    es.eth_amount,
    es.current_step,
	es.rate,
	es.token_count,
    es.total_gas_count
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
                        TempAddress = await reader.GetFieldValueAsync<string>(3),
                        StartTx = await reader.GetFieldValueAsync<string>(4),
                        CurrentTx = await reader.GetFieldValueAsync<string>(5),
                        EthAmount = await reader.GetFieldValueAsync<string>(6),
                        CurrentStep = await reader.GetFieldValueAsync<int>(7),
                        Rate = await reader.GetFieldValueAsync<int>(8),
                        TokenCount = await reader.GetFieldValueAsync<int>(9),
                        TotalGasCount = await reader.IsDBNullAsync(10) ? 0 : await reader.GetFieldValueAsync<int>(10),
                        Status = TXStatus.Ok
                    });
                }
                return result;
            }
        }

        public async Task<string> GetAddressExchangerAsync(string addr)
        {
            using (var cmd = new NpgsqlCommand(@"SELECT exchanger FROM  exchange.addresses WHERE upper(address) = upper(@addr)", _connection))
            {
                cmd.Parameters.AddWithValue("addr", addr);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return await reader.GetFieldValueAsync<string>(0);
                    }
                    return "";
                }
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

        public async Task SetStep(string id, int step)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET current_step = @step WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("step", step);
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

        public async Task SetRefundTransaction(string id, string transaction)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET refund_tx = @tx WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("tx", transaction);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task SetExchanger(string userId, string exchanger)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE identity.users SET exchanger_contract = @tx WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", userId);
                cmd.Parameters.AddWithValue("tx", exchanger);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task SetTotalGasCount(string id, int gas)
        {
            using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET total_gas_count = @gas WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("gas", gas);
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
