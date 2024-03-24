using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Nrtr : Indicator
    {
        public Nrtr(IList<ICandle> values, int period, double mult)
        {
            Bars bars = Init(values);
            DataSeries indicator = WealthLab.Centaur.Indicators.Nrtr.Series(bars, period, mult);
            Values = ToIndicator(indicator);
        }
    }
}
