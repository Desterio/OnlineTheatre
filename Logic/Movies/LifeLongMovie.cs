using Logic.Customers;

namespace Logic.Movies
{
    public class LifeLongMovie : Movie
    {
        public override ExpirationDate GetExpirationDate()
        {
            return ExpirationDate.Infinite;
        }

        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(8);
        }
    }
}