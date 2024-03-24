using AlgoSolution.Models.GraphElements;
using System.Collections.Generic;

namespace AlgoSolution.Algorithms
{
    public interface IHasGraphics
    {
        IList<IGraphSeries> GraphSeries { get; set; }
    }
}
