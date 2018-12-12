using ExchangerMonitor.Model;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangerMonitor.Services
{
    public class DatabaseService : IDatabaseService
    {
        //private NpgsqlConnection connection;
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DatabaseService(string connectionString, ILogger<DatabaseService> logger)
        {
            _logger = logger;
            _logger.LogInformation($"Connect to database: {connectionString}");
            _connectionString = connectionString;
        }


        public async Task<List<ExchangeTransaction>> GetActiveExchangeTransactionsAsync()
        {
            List<ExchangeTransaction> result = new List<ExchangeTransaction>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
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
WHERE es.is_ended = false AND es.is_failed = false", connection))
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
        }

        public async Task<string> GetAddressExchangerAsync(string addr)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"SELECT exchanger FROM  exchange.addresses WHERE upper(address) = upper(@addr)", connection))
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
        }

        /**
         * markAsFailed
         */
        public async Task MarkAsFailed(string id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET is_failed = true WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /**
         * markAsEnded
         */
        public async Task MarkAsEnded(string id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET is_ended = true WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task SetStep(string id, int step)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET current_step = @step WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("step", step);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /**
         * setCurrentTransaction
         */
        public async Task SetCurrentTransaction(string id, string transaction)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET current_tx = @tx WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("tx", transaction);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //public async Task SetRefundTransaction(string id, string transaction)
        //{
        //    using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET refund_tx = @tx WHERE id = @id", _connection))
        //    {
        //        cmd.Parameters.AddWithValue("id", id);
        //        cmd.Parameters.AddWithValue("tx", transaction);
        //        await cmd.ExecuteNonQueryAsync();
        //    }
        //}

        //public async Task SetExchanger(string userId, string exchanger)
        //{
        //    using (var cmd = new NpgsqlCommand(@"UPDATE identity.users SET exchanger_contract = @tx WHERE id = @id", _connection))
        //    {
        //        cmd.Parameters.AddWithValue("id", userId);
        //        cmd.Parameters.AddWithValue("tx", exchanger);
        //        await cmd.ExecuteNonQueryAsync();
        //    }
        //}

        public async Task SetTotalGasCount(string id, int gas)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE exchange.exchange_status SET total_gas_count = @gas WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("gas", gas);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
