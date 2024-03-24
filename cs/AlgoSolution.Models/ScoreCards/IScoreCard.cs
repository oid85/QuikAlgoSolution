using System;
using System.Collections.Generic;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.Trades;

namespace AlgoSolution.Models.ScoreCards
{
    public interface IScoreCard
    {
        List<IPosition> Positions { get; }
        List<Tuple<DateTime, double>> EqiutyCurve { get; }
        List<Tuple<DateTime, double>> DrawdownCurve { get; }
        double StartMoney { get; }
        double EndMoney { get; }
        double ProfitFactor { get; }
        double RecoveryFactor { get; }
        double NetProfit { get; }
        double AverageProfit { get; }
        double AveragePercent { get; }
        double Drawdown { get; }
        double MaxDrawdown { get; }
        double MaxDrawdownPercent { get; }
        int NumberOfTrades { get; }
        int WinningTrades { get; }
        double WinningTradesPercent { get; }

        void AddTrade(ITrade trade);
        void SetTrades(List<ITrade> trades);
    }
}
