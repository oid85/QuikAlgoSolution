using System;
using System.Collections;
using System.Collections.Generic;
using AlgoSolution.Models.Strategies;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public class GetStrategy : IDataBaseSpecification
    {
        private readonly int _strategyId;

        public GetStrategy(int strategyId)
        {
            _strategyId = strategyId;
        }

        public IEnumerable Execute(MySqlConnection connection)
        {
            string query = $@"SELECT * FROM strategies WHERE strategy_id='{_strategyId}';";

            var strategies = new List<IStrategy>();

            var command = new MySqlCommand(query, connection);

            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                IStrategy strategy = new Strategy();

                strategy.StrategyId = Convert.ToInt32(reader["strategy_id"]);
                strategy.StrategyName = Convert.ToString(reader["strategy_name"]);
                strategy.Account = Convert.ToString(reader["account"]);
                strategy.FirmId = Convert.ToString(reader["firm_id"]);
                strategy.ClassCode = Convert.ToString(reader["class_code"]);
                strategy.SecurityCode = Convert.ToString(reader["security_code"]);
                strategy.LotSize = reader["lot_size"] == DBNull.Value ? 0 : Convert.ToInt32(reader["lot_size"]);
                strategy.PercentMoney = reader["percent_money"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["percent_money"]);
                strategy.OptimalF = reader["optimal_f"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["optimal_f"]);
                strategy.CandlesDbTable = Convert.ToString(reader["candles_db_table"]);
                strategy.CandlesLimit = reader["candles_limit"] == DBNull.Value ? 0 : Convert.ToInt32(reader["candles_limit"]);
                strategy.OrderQuantity = reader["order_quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["order_quantity"]);
                strategy.OrderPrice = reader["order_price"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["order_price"]);
                strategy.SignalName = reader["signal_name"] == DBNull.Value ? "" : Convert.ToString(reader["signal_name"]);
                strategy.CurrentPosition = reader["current_position"] == DBNull.Value ? 0 : Convert.ToInt32(reader["current_position"]);
                strategy.NetProfit = reader["net_profit"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["net_profit"]);
                strategy.CurrentNetProfit = reader["current_net_profit"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["current_net_profit"]);
                strategy.IsActive = Convert.ToInt32(reader["is_active"]);

                strategies.Add(strategy);
            }

            connection.Close();
            connection.Dispose();

            return strategies;
        }
    }
}
