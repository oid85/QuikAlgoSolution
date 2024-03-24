using System.Collections.Generic;
using WealthLab;
using WealthLab.Indicators;

namespace AlgoSolution.Indicators.Common
{
    public class Ema : Indicator
    {
        public Ema(IList<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.EMA.Series(ds, period, EMACalculation.Modern);
            Values = ToIndicator(indicator);
        }
    }
}
