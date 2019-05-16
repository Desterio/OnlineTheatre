using System;
using Logic.Common;

namespace Logic.Customers
{
    public class CustomerStatus : ValueObject<CustomerStatus>
    {
        public static readonly CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular, ExpirationDate.Infinite);

        private CustomerStatus()
        {
            
        }
        
        private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate): this()
        {
            _expirationDate = expirationDate ?? throw new ArgumentNullException(nameof(expirationDate));
            Type = type;
        }
        
        public CustomerStatusType Type { get; }

        private readonly DateTime? _expirationDate;

        public decimal GetDiscount() => IsAdvanced ? 0.25m : 0m;
        public ExpirationDate ExpirationDate => ExpirationDate.Of(_expirationDate);
        public bool IsAdvanced => Type == CustomerStatusType.Advanced && !ExpirationDate.IsExpired;

        public CustomerStatus Promote()
        {
            return new CustomerStatus(CustomerStatusType.Advanced, ExpirationDate.Of(DateTime.UtcNow.AddYears(1)));
        }

        protected override int GetHashCodeCore() =>
            Type.GetHashCode() ^ ExpirationDate.GetHashCode();

        protected override bool EqualsCore(CustomerStatus other) =>
            Type == other.Type && ExpirationDate == other.ExpirationDate;
    }
    
    public enum CustomerStatusType
    {
        Regular = 1,
        Advanced = 2
    }
}