using System.Collections.Generic;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using AlgoSolution.Models.Candles;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class CandleDataBaseRepository : DataBaseRepositoryBase<ICandle>
    {
        private readonly IDataBaseSpecificationFactory _specificationFactory;

        public CandleDataBaseRepository(IDataBaseSpecificationFactory specificationFactory)
        {
            _specificationFactory = specificationFactory;
        }

        public IEnumerable<ICandle> ReadCandles(int candlesLimit, string candlesDbTable)
        {
            var specification = _specificationFactory.CreateGetCandlesSpecification(candlesLimit, candlesDbTable);  
            var candles = Read(specification);

            return candles;
        }
    }
}
