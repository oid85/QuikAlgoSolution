using System;
using System.Linq;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using AlgoSolution.Models.Strategies;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class StrategyDataBaseRepository : DataBaseRepositoryBase<IStrategy>
    {
        private readonly IDataBaseSpecificationFactory _specificationFactory;

        public StrategyDataBaseRepository(IDataBaseSpecificationFactory specificationFactory)
        {
            _specificationFactory = specificationFactory;
        }

        public IStrategy ReadStrategy(int strategyId)
        {
            var specification = _specificationFactory.CreateGetStrategySpecification(strategyId);
            var strategy = Read(specification).First();

            return strategy;
        }

        public override bool Create(IStrategy entity)
        {
            var connection = CreateConnection();
            connection.Open();

            entity.OrderQuantity = 0;
            entity.OrderPrice = 0.0;
            entity.SignalName = "-";
            entity.SetActive();

            string percentMoney = entity.PercentMoney.ToString().Replace(",", ".");
            string optimalF = entity.OptimalF.ToString().Replace(",", ".");
            string orderPrice = entity.OrderPrice.ToString().Replace(",", ".");
            string netProfit = entity.NetProfit.ToString().Replace(",", ".");
            string currentNetProfit = entity.CurrentNetProfit.ToString().Replace(",", ".");

            string query = $@"INSERT INTO strategies (strategy_id, 
                                                      strategy_name, 
                                                      account, 
                                                      firm_id, 
                                                      class_code, 
                                                      security_code, 
													  pair_security_code, 
                                                      lot_size, 
                                                      percent_money, 
                                                      optimal_f, 
                                                      candles_db_table, 
													  pair_candles_db_table, 
                                                      candles_limit, 
                                                      order_quantity, 
                                                      order_price, 
                                                      signal_name, 
                                                      current_position, 
                                                      net_profit, 
                                                      current_net_profit, 
                                                      is_active, 
                                                      modifided_datetime) 
                                  VALUES ('{entity.StrategyId}', 
                                          '{entity.StrategyName}',
                                          '{entity.Account}',  
                                          '{entity.FirmId}', 
                                          '{entity.ClassCode}', 
                                          '{entity.SecurityCode}', 
                                          '{entity.LotSize}', 
                                          '{percentMoney}', 
                                          '{optimalF}', 
                                          '{entity.CandlesDbTable}', 
                                          '{entity.CandlesLimit}', 
                                          '{entity.OrderQuantity}', 
                                          '{orderPrice}', 
                                          '{entity.SignalName}', 
                                          '{entity.CurrentPosition}', 
                                          '{netProfit}', 
                                          '{currentNetProfit}', 
                                          '{entity.IsActive}', 
                                          '{DateTime.Now}')";

            var command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();

            connection.Close();
            connection.Dispose();

            return true;
        }

        public override bool Update(IStrategy entity)
        {
            var connection = CreateConnection();
            connection.Open();

            string percentMoney = entity.PercentMoney.ToString().Replace(",", ".");
            string optimalF = entity.OptimalF.ToString().Replace(",", ".");
            string orderPrice = entity.OrderPrice.ToString().Replace(",", ".");
            string netProfit = entity.NetProfit.ToString().Replace(",", ".");
            string currentNetProfit = entity.CurrentNetProfit.ToString().Replace(",", ".");

            string query = $@"UPDATE strategies
                                  SET strategy_name='{entity.StrategyName}', 
                                      account='{entity.Account}', 
                                      firm_id='{entity.FirmId}', 
                                      class_code='{entity.ClassCode}', 
                                      security_code='{entity.SecurityCode}', 
                                      lot_size='{entity.LotSize}', 
                                      percent_money='{percentMoney}', 
                                      optimal_f='{optimalF}', 
                                      candles_db_table='{entity.CandlesDbTable}', 
                                      candles_limit='{entity.CandlesLimit}', 
                                      order_quantity='{entity.OrderQuantity}', 
                                      order_price='{orderPrice}', 
                                      signal_name='{entity.SignalName}', 
                                      current_position='{entity.CurrentPosition}', 
                                      net_profit='{netProfit}', 
                                      current_net_profit='{currentNetProfit}', 
                                      is_active='{entity.IsActive}', 
                                      modifided_datetime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
                                  WHERE strategy_id='{entity.StrategyId}';";

            var command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();

            connection.Close();
            connection.Dispose();

            return true;
        }

        public override bool IsExist(IStrategy entity)
        {
            var connection = CreateConnection();
            connection.Open();

            string query = $@"SELECT * FROM strategies WHERE strategy_id='{entity.StrategyId}' LIMIT 1;";

            var command = new MySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            bool result = reader.HasRows;

            connection.Close();
            connection.Dispose();

            return result;
        }
    }
}
