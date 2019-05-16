using System.Collections.Generic;
using System.Linq;
using Logic.Common;
using Logic.Utils;

namespace Logic.Customers
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IReadOnlyList<Customer> GetList()
        {
            return UnitOfWork
                .Query<Customer>()
                .ToList();
        }

        public Customer GetCustomerByEmail(CustomerEmail email)
        {
            return UnitOfWork.Query<Customer>()
                .SingleOrDefault(x => x.Email == email);
        }
    }
}