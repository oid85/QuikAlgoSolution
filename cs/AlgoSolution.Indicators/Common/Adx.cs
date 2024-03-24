using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Adx : Indicator
    {
        public Adx(IList<ICandle> values, int period)
        {
            Bars bars = Init(values);
            DataSeries indicator = WealthLab.Indicators.ADX.Series(bars, period);
            Values = ToIndicator(indicator);
        }
    }
}
