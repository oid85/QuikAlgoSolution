using System.Collections.Generic;
using System.IO;
using AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications;
using AlgoSolution.Models.Candles;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories
{
    public class CandleTextFileRepository : TextFileRepositoryBase<ICandle>
    {
        public IEnumerable<ICandle> ReadCandles(int candlesLimit)
        {
            var specification = new GetCandles(candlesLimit);            
            return Read(specification);
        }

        public CandleTextFileRepository(string path)
        {
            FileInfo = new FileInfo(path);
        }
    }
}
