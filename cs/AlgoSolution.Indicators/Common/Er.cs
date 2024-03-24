using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class Er : Indicator
    {
        public Er(IList<double> values, int period)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = Community.Indicators.ER.Series(ds, period);
            Values = ToIndicator(indicator);
        }
    }
}
