using System;
using System.Collections;
using System.Collections.Generic;
using AlgoSolution.Models.Securities;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public class GetSecurity : IDataBaseSpecification
    {
        private readonly string _classCode;
        private readonly string _securityCode;

        public GetSecurity(string classCode, string securityCode)
        {
            _classCode = classCode;
            _securityCode = securityCode;
        }

        public IEnumerable Execute(MySqlConnection connection)
        {
            string query = $@"SELECT * FROM securities WHERE class_code='{_classCode}' AND security_code='{_securityCode}';";

            var securities = new List<ISecurity>();

            var command = new MySqlCommand(query, connection);

            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ISecurity security = new Security();

                security.SecurityCode = Convert.ToString(reader["security_code"]);
                security.ClassCode = Convert.ToString(reader["class_code"]);
                security.Go = Convert.ToDouble(reader["gaurant"]);

                securities.Add(security);
            }

            connection.Close();
            connection.Dispose();

            return securities;
        }
    }
}
