using System;
using System.Collections;
using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public class GetCandles : IDataBaseSpecification
    {
        private readonly int _candlesLimit;
        private readonly string _candlesDbTable;

        public GetCandles(int candlesLimit, string candlesDbTable)
        {
            _candlesLimit = candlesLimit;
            _candlesDbTable = candlesDbTable;
        }

        public IEnumerable Execute(MySqlConnection connection)
        {
            string query = $@"SELECT * FROM {_candlesDbTable} ORDER BY id DESC LIMIT {_candlesLimit};";

            var candles = new List<ICandle>();

            var command = new MySqlCommand(query, connection);

            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ICandle candle = new Candle();

                int date = Convert.ToInt32(reader["date"]);
                int time = Convert.ToInt32(reader["time"]);

                int year = Convert.ToInt32(date.ToString().Substring(0, 4));
                int month = Convert.ToInt32(date.ToString().Substring(4, 2));
                int day = Convert.ToInt32(date.ToString().Substring(6, 2));

                int hour = Convert.ToInt32(time.ToString().Substring(0, 2));
                int minute = Convert.ToInt32(time.ToString().Substring(2, 2));
                int second = Convert.ToInt32(time.ToString().Substring(4, 2));

                var datetime = new DateTime(year, month, day, hour, minute, second);

                candle.DateTime = datetime;
                candle.Open = Convert.ToDouble(reader["open"]);
                candle.Close = Convert.ToDouble(reader["close"]);
                candle.High = Convert.ToDouble(reader["high"]);
                candle.Low = Convert.ToDouble(reader["low"]);
                candle.Volume = Convert.ToInt32(reader["volume"]);
                candle.TimeFrame = Convert.ToInt32(reader["timeframe"]);

                candles.Add(candle);
            }

            candles.Reverse();

            connection.Close();
            connection.Dispose();

            return candles;
        }
    }
}
