using AlgoSolution.DataAccessLayer.DataBase.Repositories;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using AlgoSolution.DataAccessLayer.DataBase.Specifications.Factories;
using AlgoSolution.Models.Accounts;
using AlgoSolution.Models.Candles;
using AlgoSolution.Models.Securities;
using AlgoSolution.Models.Strategies;
using Unity;
using Unity.Extension;

namespace AlgoSolution.DataAccessLayer.DataBase
{
    public class AlgoSolutionDataAccessLayerDataBaseContainerExtensions : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IDataBaseRepository<IAccount>, AccountDataBaseRepository>();
            Container.RegisterType<IDataBaseRepository<ICandle>, CandleDataBaseRepository>();
            Container.RegisterType<IDataBaseRepository<ISecurity>, SecurityDataBaseRepository>();
            Container.RegisterType<IDataBaseRepository<IStrategy>, StrategyDataBaseRepository>();

            Container.RegisterType<IDataBaseSpecification, GetAccount>("GetAccount");
            Container.RegisterType<IDataBaseSpecification, GetCandles>("GetCandles");
            Container.RegisterType<IDataBaseSpecification, GetSecurity>("GetSecurity");
            Container.RegisterType<IDataBaseSpecification, GetStrategy>("GetStrategy");

            Container.RegisterType<IDataBaseRepositoryFactory, DataBaseRepositoryFactory>();
            Container.RegisterType<IDataBaseSpecificationFactory, DataBaseSpecificationFactory>();
        }
    }
}
