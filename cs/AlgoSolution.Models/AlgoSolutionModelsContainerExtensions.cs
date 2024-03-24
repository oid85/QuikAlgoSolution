using AlgoSolution.Models.Accounts;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.GraphElements;
using AlgoSolution.Models.MoneyManagements;
using AlgoSolution.Models.Positions;
using AlgoSolution.Models.ScoreCards;
using AlgoSolution.Models.Securities;
using AlgoSolution.Models.Strategies;
using AlgoSolution.Models.Trades;
using Unity;
using Unity.Extension;

namespace AlgoSolution.Models
{
    public class AlgoSolutionModelsContainerExtensions : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IAccount, Account>();
            Container.RegisterType<ICandle, Candle>();
            Container.RegisterType<IGraphSeries, GraphSeries>();
            Container.RegisterType<IPosition, Position>();
            Container.RegisterType<ISecurity, Security>();
            Container.RegisterType<IStrategy, Strategy>();
            Container.RegisterType<ITrade, Trade>();

            Container.RegisterType<IMoneyManagement, OptimalF>("OptimalF");
            Container.RegisterType<IScoreCard, ScoreCard>("ScoreCard");

            Container.RegisterType<StopLimit>();
            Container.RegisterType<TakeProfit>();

            Container.RegisterType<IModelFactory, ModelFactory>();
        }
    }
}
