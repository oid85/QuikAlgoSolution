using System;

namespace AlgoSolution.Models.Trades
{
    public interface ITrade
    {
        string ClassCode { get; set; }
        string SecurityCode { get; set; }
        DateTime DateTime { get; set; }
        int Quantity { get; set; }
        double Price { get; set; }
        int CandleIndex { get; set; }
    }
}
