using System.Collections.Generic;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public interface IDataBaseRepository<T>
    {
        IEnumerable<T> Read(IDataBaseSpecification specification);

        bool Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        bool IsExist(T entity);
    }
}
