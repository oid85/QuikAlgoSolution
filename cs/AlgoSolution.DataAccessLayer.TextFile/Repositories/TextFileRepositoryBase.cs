using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlgoSolution.DataAccessLayer.TextFile.Repositories.Specifications;

namespace AlgoSolution.DataAccessLayer.TextFile.Repositories
{
    public class TextFileRepositoryBase<T> : ITextFileRepository<T>
    {
        public FileInfo FileInfo { get; set; }

        public IEnumerable<T> Read(ITextFileSpecification specification)
        {
            return specification.Execute(FileInfo).Cast<T>();
        }

        public virtual bool Create(T entity)
        {
            return false;
        }

        public virtual bool Update(T entity)
        {
            return false;
        }

        public virtual bool Delete(T entity)
        {
            return false;
        }

        public virtual bool IsExist(T entity)
        {
            return false;
        }

        public virtual bool CreateOrUpdate(T entity)
        {
            return IsExist(entity) 
                ? Update(entity) 
                : Create(entity);
        }
    }
}
