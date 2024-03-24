using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using WealthLab;

namespace AlgoSolution.Indicators
{
    public class Indicator : IIndicator
    {
        public IList<double> Values { get; set; }

        /// <summary>
        /// Инициализировать бары
        /// </summary>
        public Bars Init(IList<ICandle> candles)
        {
            var bars = new Bars("bars", BarScale.Minute, 1);

            foreach (ICandle candle in candles)
                bars.Add(candle.DateTime, candle.Open, candle.High, candle.Low, candle.Close, candle.Volume);
            return bars;
        }

        /// <summary>
        /// Инициализировать серию
        /// </summary>
        public DataSeries Init(IList<double> source)
        {
            var ds = new DataSeries("ds");

            for (int i = 0; i < source.Count; i++)
                ds.Add(source[i]);

            return ds;
        }

        /// <summary>
        /// Преобразовать индикатор из формата WealthLab
        /// </summary>
        public IList<double> ToIndicator(DataSeries dataSeries)
        {
            IList<double> indicator = new List<double>();
            for (int bar = 0; bar < dataSeries.Count; bar++)
                indicator.Add(dataSeries[bar]);
            return indicator;
        }
    }
}

