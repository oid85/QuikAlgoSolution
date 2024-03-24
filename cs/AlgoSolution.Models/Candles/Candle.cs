using System;

namespace AlgoSolution.Models.Candles
{
    public class Candle : ICandle
    {
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public long Volume { get; set; }
        public DateTime DateTime { get; set; }
        public int TimeFrame { get; set; }
    }
}