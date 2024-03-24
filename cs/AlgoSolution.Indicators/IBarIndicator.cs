using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using WealthLab;

namespace AlgoSolution.Indicators
{
    public interface IBarIndicator
    {
        Bars Init(IList<ICandle> candles);
    }
}
