using AlgoSolution.Models.Accounts;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.ScoreCards;
using AlgoSolution.Models.Securities;
using AlgoSolution.Models.Strategies;
using AlgoSolution.Models.Trades;

namespace AlgoSolution.Models
{
    public interface IModelFactory
    {
        IAccount CreateAccount();
        ICandle CreateCandle();
        IGraphSeries CreateGraphSeries();
        IPosition CreatePosition();
        ISecurity CreateSecurity();
        IStrategy CreateStrategy();
        ITrade CreateTrade();

        StopLimit CreateStopLimit();
        TakeProfit CreateTakeProfit();

        IMoneyManagement CreateOptimalFMoneyManagement();
        IScoreCard CreateScoreCard();
    }
}
