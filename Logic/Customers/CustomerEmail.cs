using System;
using System.Text.RegularExpressions;
using Logic.Common;

namespace Logic.Customers
{
    public class CustomerEmail : ValueObject<CustomerEmail>
    {
        private CustomerEmail(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        protected override bool EqualsCore(CustomerEmail other)
        {
            return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static Result<CustomerEmail> Create(string customerEmail)
        {
            customerEmail = (customerEmail ?? string.Empty).Trim();

            if (customerEmail.Length == 0)
                return Result.Fail<CustomerEmail>("Customer email cannot be empty");

            return !Regex.IsMatch(customerEmail, @"^(.+)@(.+)$")
                ? Result.Fail<CustomerEmail>("Email is in an invalid format.")
                : Result.Ok(new CustomerEmail(customerEmail));
        }
        
        public static CustomerEmail Of(string customerEmail) => Create(customerEmail).Value;
        
        public static implicit operator string(CustomerEmail customerEmail)
        {
            return customerEmail.Value;
        }
    }
}