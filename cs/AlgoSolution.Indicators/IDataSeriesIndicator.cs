using System.Collections.Generic;
using WealthLab;

namespace AlgoSolution.Indicators
{
    public interface IDataSeriesIndicator
    {
        DataSeries Init(IList<double> source);
    }
}
