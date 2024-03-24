using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Lowest : Indicator
    {
        public Lowest(IList<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.Lowest.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
