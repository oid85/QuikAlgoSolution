using System;

namespace AlgoSolution.Models.Trades
{
    public class Trade : ITrade
    {
        public string ClassCode { get; set; }
        public string SecurityCode { get; set; }
        public DateTime DateTime { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CandleIndex { get; set; }
    }
}