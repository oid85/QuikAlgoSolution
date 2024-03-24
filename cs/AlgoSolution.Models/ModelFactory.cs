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

namespace AlgoSolution.Models
{
    public class ModelFactory : IModelFactory
    {
        private readonly IUnityContainer _container;

        public ModelFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IAccount CreateAccount()
        {
            return _container.Resolve<IAccount>();
        }

        public ICandle CreateCandle()
        {
            return _container.Resolve<ICandle>();
        }

        public IGraphSeries CreateGraphSeries()
        {
            return _container.Resolve<IGraphSeries>();
        }

        public IPosition CreatePosition()
        {
            return _container.Resolve<IPosition>();
        }

        public ISecurity CreateSecurity()
        {
            return _container.Resolve<ISecurity>();
        }

        public IStrategy CreateStrategy()
        {
            return _container.Resolve<IStrategy>();
        }

        public ITrade CreateTrade()
        {
            return _container.Resolve<ITrade>();
        }

        public StopLimit CreateStopLimit()
        {
            return _container.Resolve<StopLimit>();
        }

        public TakeProfit CreateTakeProfit()
        {
            return _container.Resolve<TakeProfit>();
        }

        public IMoneyManagement CreateOptimalFMoneyManagement()
        {
            return _container.Resolve<IMoneyManagement>("OptimalF");
        }

        public IScoreCard CreateScoreCard()
        {
            return _container.Resolve<IScoreCard>("ScoreCard");
        }
    }
}
