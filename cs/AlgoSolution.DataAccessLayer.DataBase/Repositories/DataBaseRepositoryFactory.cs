using AlgoSolution.Models.Accounts;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.Securities;
using AlgoSolution.Models.Strategies;
using Unity;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class DataBaseRepositoryFactory : IDataBaseRepositoryFactory
    {
        private readonly IUnityContainer _container;

        public DataBaseRepositoryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public AccountDataBaseRepository CreateAccountDataBaseRepository()
        {
            return (AccountDataBaseRepository) _container.Resolve<IDataBaseRepository<IAccount>>();
        }

        public CandleDataBaseRepository CreateCandleDataBaseRepository()
        {
            return (CandleDataBaseRepository) _container.Resolve<IDataBaseRepository<ICandle>>();
        }

        public SecurityDataBaseRepository CreateSecurityDataBaseRepository()
        {
            return (SecurityDataBaseRepository) _container.Resolve<IDataBaseRepository<ISecurity>>();
        }

        public StrategyDataBaseRepository CreateStrategyDataBaseRepository()
        {
            return (StrategyDataBaseRepository) _container.Resolve<IDataBaseRepository<IStrategy>>();
        }
    }
}
