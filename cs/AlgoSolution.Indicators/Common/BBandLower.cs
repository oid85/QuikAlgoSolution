using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class BBandLower : Indicator
    {
        public BBandLower(IList<double> values, int period, double stdDev)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.BBandLower.Series(ds, period, stdDev);
            Values = ToIndicator(indicator);
        }
    }
}
