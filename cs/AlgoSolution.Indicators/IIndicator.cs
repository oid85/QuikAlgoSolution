using System.Collections.Generic;

namespace AlgoSolution.Indicators
{
    public interface IIndicator : IBarIndicator, IDataSeriesIndicator
    {
        IList<double> Values { get; set; }
    }
}
