using System.Collections.Generic;
using System.Drawing;

namespace AlgoSolution.Models.GraphElements
{
    public interface IGraphSeries
    {
        string Name { get; set; }
        IList<double> Values { get; set; }
        Color Color { get; set; }
    }
}
