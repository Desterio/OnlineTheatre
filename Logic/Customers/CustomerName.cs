using System;
using Logic.Common;

namespace Logic.Customers
{
    public class CustomerName : ValueObject<CustomerName>
    {
        private CustomerName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        protected override bool EqualsCore(CustomerName other)
        {
            return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static Result<CustomerName> Create(string customerName)
        {
            customerName = (customerName ?? string.Empty).Trim();

            if (customerName.Length == 0)
                return Result.Fail<CustomerName>("Customer name cannot be empty");

            return customerName.Length > 100
                ? Result.Fail<CustomerName>("Customer name cannot exceed 100 characters.")
                : Result.Ok(new CustomerName(customerName));
        }
        
        public static CustomerName Of(string customerName) => Create(customerName).Value;

        public static implicit operator string(CustomerName customerName)
        {
            return customerName.Value;
        }

//        public static explicit operator CustomerName(string customerName)
//        {
//            return Create(customerName).Value;
//        }
    }
}