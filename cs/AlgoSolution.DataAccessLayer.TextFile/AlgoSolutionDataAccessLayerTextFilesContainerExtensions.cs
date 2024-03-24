using AlgoSolution.DataAccessLayer.TextFile.Repositories;
using AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications;
using AlgoSolution.Models.Candles;
using Unity;
using Unity.Extension;

namespace AlgoSolution.DataAccessLayer.TextFile
{
    public class AlgoSolutionDataAccessLayerTextFilesContainerExtensions : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ITextFileRepository<ICandle>, CandleTextFileRepository>();

            Container.RegisterType<ITextFileSpecification, GetCandles>("GetCandles");

            Container.RegisterType<ITextFileRepositoryFactory, TextFileRepositoryFactory>();
            Container.RegisterType<ITextFileSpecificationFactory, TextFileSpecificationFactory>();
        }
    }
}
