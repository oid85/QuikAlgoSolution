using System.Collections;
using MySql.Data.MySqlClient;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications
{
    public interface IDataBaseSpecification
    {
        IEnumerable Execute(MySqlConnection connection);
    }
}
