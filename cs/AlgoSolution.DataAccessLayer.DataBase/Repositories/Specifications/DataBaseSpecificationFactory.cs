using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using Unity;
using Unity.Resolution;

namespace AlgoSolution.DataAccessLayer.DataBase.Specifications.Factories
{
    public class DataBaseSpecificationFactory : IDataBaseSpecificationFactory
    {
        private readonly IUnityContainer _container;

        public DataBaseSpecificationFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IDataBaseSpecification CreateGetAccountSpecification(string accountName)
        {
            return _container.Resolve<IDataBaseSpecification>("GetAccount",
                new ParameterOverride("accountName", accountName));
        }

        public IDataBaseSpecification CreateGetCandlesSpecification(int candlesLimit, string candlesDbTable)
        {
            return _container.Resolve<IDataBaseSpecification>("GetCandles",
                new ParameterOverride("candlesLimit", candlesLimit),
                new ParameterOverride("candlesDbTable", candlesDbTable));
        }

        public IDataBaseSpecification CreateGetSecuritySpecification(string classCode, string securityCode)
        {
            return _container.Resolve<IDataBaseSpecification>("GetSecurity",
                new ParameterOverride("classCode", classCode),
                new ParameterOverride("securityCode", securityCode));
        }

        public IDataBaseSpecification CreateGetStrategySpecification(int strategyId)
        {
            return _container.Resolve<IDataBaseSpecification>("GetStrategy",
                new ParameterOverride("strategyId", strategyId));
        }
    }
}
