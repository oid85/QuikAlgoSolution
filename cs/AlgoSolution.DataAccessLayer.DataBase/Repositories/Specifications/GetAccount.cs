using System;
using System.Collections;
using System.Collections.Generic;
using AlgoSolution.Models.Accounts;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public class GetAccount : IDataBaseSpecification
    {
        private readonly string _accountName;        

        public GetAccount(string accountName)
        {
            _accountName = accountName;            
        }

        public IEnumerable Execute(MySqlConnection connection)
        {
            string query = $@"SELECT * FROM accounts WHERE account='{_accountName}';";

            var accounts = new List<IAccount>();

            var command = new MySqlCommand(query, connection);
            
            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                IAccount account = new Account();

                account.AccountName = Convert.ToString(reader["account"]);
                account.Money = Convert.ToDouble(reader["money"]);

                accounts.Add(account);
            }

            connection.Close();
            connection.Dispose();

            return accounts;
        }
    }
}
