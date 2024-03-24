namespace AlgoSolution.DataAccessLayer.TextFile.Repositories
{
    public interface ITextFileRepositoryFactory
    {
        CandleTextFileRepository CreateCandleTextFileRepository(string path);
    }
}
