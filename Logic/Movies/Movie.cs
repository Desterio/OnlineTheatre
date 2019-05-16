using Logic.Common;
using Logic.Customers;

namespace Logic.Movies
{
    public abstract class Movie : Entity
    {
        public virtual string Name { get; protected set; }
        protected virtual LicensingModel LicensingModel { get; set; }

        public abstract ExpirationDate GetExpirationDate();
        
        public virtual Dollars CalculatePrice(CustomerStatus status)
        {
            var modifier = 1m - status.GetDiscount();

            return GetBasePrice() * modifier;
        }

        protected abstract Dollars GetBasePrice();
    }
}