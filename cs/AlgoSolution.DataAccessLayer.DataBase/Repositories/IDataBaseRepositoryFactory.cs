namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public interface IDataBaseRepositoryFactory
    {
        AccountDataBaseRepository CreateAccountDataBaseRepository();
        CandleDataBaseRepository CreateCandleDataBaseRepository();
        SecurityDataBaseRepository CreateSecurityDataBaseRepository();
        StrategyDataBaseRepository CreateStrategyDataBaseRepository();
    }
}
