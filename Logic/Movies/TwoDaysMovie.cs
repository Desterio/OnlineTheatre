using System;
using Logic.Customers;

namespace Logic.Movies
{
    public class TwoDaysMovie : Movie
    {
        public override ExpirationDate GetExpirationDate()
        {
            return ExpirationDate.Of(DateTime.UtcNow.AddDays(2));
        }

        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(4);
        }
    }
}