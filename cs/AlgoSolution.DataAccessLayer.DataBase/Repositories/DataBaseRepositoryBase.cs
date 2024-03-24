using System.Collections.Generic;
using System.Linq;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class DataBaseRepositoryBase<T> : IDataBaseRepository<T>
    {
        private string _connectionString = @"server=localhost;user=root;database=algo;port=3306;password=1;";

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public IEnumerable<T> Read(IDataBaseSpecification specification)
        {
            var connection = CreateConnection();
            return specification.Execute(connection).Cast<T>();
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
