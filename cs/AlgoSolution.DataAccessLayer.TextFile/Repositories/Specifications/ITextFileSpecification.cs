using System.Collections;
using System.IO;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications
{
    public interface ITextFileSpecification
    {
        IEnumerable Execute(FileInfo fileInfo);
    }
}
