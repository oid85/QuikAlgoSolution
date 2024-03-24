using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Highest : Indicator
    {
        public Highest(IList<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.Highest.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
