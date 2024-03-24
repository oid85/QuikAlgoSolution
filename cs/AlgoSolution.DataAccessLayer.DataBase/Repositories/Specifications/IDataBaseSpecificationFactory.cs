namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public interface IDataBaseSpecificationFactory
    {
        IDataBaseSpecification CreateGetAccountSpecification(string accountName);
        IDataBaseSpecification CreateGetCandlesSpecification(int candlesLimit, string candlesDbTable);
        IDataBaseSpecification CreateGetSecuritySpecification(string classCode, string securityCode);
        IDataBaseSpecification CreateGetStrategySpecification(int strategyId);
    }
}
