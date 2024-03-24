using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Sma : Indicator
    {
        public Sma(IList<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.SMA.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
