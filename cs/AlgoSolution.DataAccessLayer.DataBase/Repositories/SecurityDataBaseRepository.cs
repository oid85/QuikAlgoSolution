using System.Linq;
using AlgoSolution.DataAccessLayer.DataBase.Repositories.Specifications;
using AlgoSolution.Models.Securities;

namespace AlgoSolution.DataAccessLayer.DataBase.Repositories
{
    public class SecurityDataBaseRepository : DataBaseRepositoryBase<ISecurity>
    {
        private readonly IDataBaseSpecificationFactory _specificationFactory;

        public SecurityDataBaseRepository(IDataBaseSpecificationFactory specificationFactory)
        {
            _specificationFactory = specificationFactory;
        }

        public ISecurity ReadSecurity(string classCode, string securityCode)
        {
            var specification = _specificationFactory.CreateGetSecuritySpecification(classCode, securityCode);
            var security = Read(specification).First();

            return security;
        }
    }
}
