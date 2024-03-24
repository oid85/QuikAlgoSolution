namespace AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications
{
    public interface ITextFileSpecificationFactory
    {
        ITextFileSpecification CreateGetCandlesSpecification(int candlesLimit);
    }
}
