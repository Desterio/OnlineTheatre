using System;
using FluentNHibernate.Mapping;

namespace Logic.Customers
{
    public class PurchasedMovieMap : ClassMap<PurchasedMovie>
    {
        public PurchasedMovieMap()
        {
            Id(x => x.Id);

            Map(x => x.Price).CustomType<decimal>().Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.PurchaseDate);
            Map(x => x.ExpirationDate)
                .Nullable()
                .CustomType<DateTime?>().Access
                .CamelCaseField(Prefix.Underscore);

            References(x => x.Movie);
            References(x => x.Customer);
        }
    }
}