using Unity;
using Unity.Resolution;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications
{
    public class TextFileSpecificationFactory : ITextFileSpecificationFactory
    {
        private readonly IUnityContainer _container;

        public TextFileSpecificationFactory(IUnityContainer container)
        {
            _container = container;
        }

        public ITextFileSpecification CreateGetCandlesSpecification(int candlesLimit)
        {
            return _container.Resolve<ITextFileSpecification>("GetCandles",
                new ParameterOverride("candlesLimit", candlesLimit));
        }
    }
}
