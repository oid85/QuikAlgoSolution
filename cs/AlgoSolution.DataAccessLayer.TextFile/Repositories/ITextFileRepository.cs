using System.Collections.Generic;
using AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories
{
    public interface ITextFileRepository<T>
    {
        IEnumerable<T> Read(ITextFileSpecification specification);

        bool Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        bool IsExist(T entity);
    }
}
