using System.Linq;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using AlgoSolution.Models.Accounts;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class AccountDataBaseRepository : DataBaseRepositoryBase<IAccount>
    {
        private readonly IDataBaseSpecificationFactory _specificationFactory;

        public AccountDataBaseRepository(IDataBaseSpecificationFactory specificationFactory)
        {
            _specificationFactory = specificationFactory;
        }

        public IAccount ReadAccount(string accountName)
        {
            var specification = _specificationFactory.CreateGetAccountSpecification(accountName);
            var account = Read(specification).First();

            return account;
        }
    }
}
