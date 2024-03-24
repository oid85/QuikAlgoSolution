using System;

namespace AlgoSolution.Models.Candles
{
    /// <summary>
    /// Свеча
    /// </summary>
    public interface ICandle
    {
        double Open { get; set; }
        double Close { get; set; }
        double High { get; set; }
        double Low { get; set; }
        long Volume { get; set; }
        DateTime DateTime { get; set; }
        int TimeFrame { get; set; }
    }
}
