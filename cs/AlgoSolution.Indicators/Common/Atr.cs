using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Atr : Indicator
    {
        public Atr(IList<ICandle> values, int period)
        {
            Bars bars = Init(values);
            DataSeries indicator = WealthLab.Indicators.ATR.Series(bars, period);
            Values = ToIndicator(indicator);
        }
    }
}
