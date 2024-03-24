using System.Collections.Generic;
using System.Drawing;

namespace AlgoSolution.Models.GraphElements
{
    public class GraphSeries : IGraphSeries
    {
        public string Name { get; set; }
        public IList<double> Values { get; set; }
        public Color Color { get; set; }
    }
}
