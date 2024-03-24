using AlgoSolution.Models.Candles;
using Unity;
using Unity.Resolution;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories
{
    public class TextFileRepositoryFactory : ITextFileRepositoryFactory
    {
        private readonly IUnityContainer _container;

        public TextFileRepositoryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public CandleTextFileRepository CreateCandleTextFileRepository(string path)
        {
            return (CandleTextFileRepository) _container.Resolve<ITextFileRepository<ICandle>>(new ParameterOverride("path", path));
        }
    }
}
