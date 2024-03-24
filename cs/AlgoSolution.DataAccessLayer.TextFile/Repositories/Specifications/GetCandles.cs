using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlgoSolution.Models.Candles;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications
{
    public class GetCandles : ITextFileSpecification
    {
        private readonly int _candlesLimit;

        public GetCandles(int candlesLimit)
        {
            _candlesLimit = candlesLimit;
        }

        public IEnumerable Execute(FileInfo fileInfo)
        {
            var separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            List<string> lines = File.ReadAllLines(fileInfo.FullName).Skip(1).ToList();

            int totalCandles = lines.Count();

            if (totalCandles > _candlesLimit)
                lines = lines.Skip(totalCandles - _candlesLimit).ToList();

            var candles = new List<ICandle>();

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');

                int year = Convert.ToInt32(parts[2].Substring(0, 4));
                int month = Convert.ToInt32(parts[2].Substring(4, 2));
                int day = Convert.ToInt32(parts[2].Substring(6, 2));

                int hour = Convert.ToInt32(parts[3].Substring(0, 2));
                int minute = Convert.ToInt32(parts[3].Substring(2, 2));
                int second = Convert.ToInt32(parts[3].Substring(4, 2));

                var datetime = new DateTime(year, month, day, hour, minute, second);
                
                var candle = new Candle
                {
                    DateTime = datetime,
                    TimeFrame = Convert.ToInt32(parts[1]),
                    Open = Convert.ToDouble(parts[4].Replace(",", separator).Replace(".", separator)),
                    High = Convert.ToDouble(parts[5].Replace(",", separator).Replace(".", separator)),
                    Low = Convert.ToDouble(parts[6].Replace(",", separator).Replace(".", separator)),
                    Close = Convert.ToDouble(parts[7].Replace(",", separator).Replace(".", separator)),
                    Volume = Convert.ToInt32(parts[8].Replace(",", separator).Replace(".", separator))
                };
                
                candles.Add(candle);
            }

            return candles.OrderBy(c => c.DateTime);
        }
    }
}
