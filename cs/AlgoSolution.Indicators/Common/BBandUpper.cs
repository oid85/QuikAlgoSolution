using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators.Common
{
    public class BBandUpper : Indicator
    {
        public BBandUpper(IList<double> values, int period, double stdDev)
        {
            DataSeries ds = Init(values);
            DataSeries indicator = WealthLab.Indicators.BBandUpper.Series(ds, period, stdDev);
            Values = ToIndicator(indicator);
        }
    }
}
