using System.Collections.Generic;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.ScoreCards;
using AlgoSolution.Models.Trades;

namespace AlgoSolution.Algorithms
{
    public interface IAlgorithm : ITrading, IHasGraphics
    {
        IList<ICandle> Candles { get; set; }
        IList<ITrade> Trades { get; set; }
        IList<IPosition> Positions { get; }
        IScoreCard ScoreCard { get; set; }

        void Algorithm();
        void Execute();
    }
}
