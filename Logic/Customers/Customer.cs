using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Common;
using Logic.Movies;

namespace Logic.Customers
{
    public class Customer : Entity
    {
        protected Customer()
        {
            _purchasedMovies = new List<PurchasedMovie>();
        }

        public Customer(CustomerName customerName, CustomerEmail customerEmail) : this()
        {
            _name = customerName ?? throw new ArgumentNullException(nameof(customerName));
            _email = customerEmail ?? throw new ArgumentNullException(nameof(customerEmail));

            MoneySpent = Dollars.Of(0);
            Status = CustomerStatus.Regular;
        }

        private string _name;

        public virtual CustomerName Name
        {
            get => CustomerName.Of(_name);
            set => _name = value;
        }

        private readonly string _email;
        public virtual CustomerEmail Email => CustomerEmail.Of(_email);

        public virtual CustomerStatus Status { get; protected set; }

        private decimal _moneySpent;

        public virtual Dollars MoneySpent
        {
            get => Dollars.Of(_moneySpent);
            protected set => _moneySpent = value;
        }

        private readonly IList<PurchasedMovie> _purchasedMovies;
        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

        public virtual void PurchaseMovie(Movie movie)
        {
            if (HasPurchasedMovie(movie))
                throw new Exception();

            var expirationDate = movie.GetExpirationDate();
            var price = movie.CalculatePrice(Status);
            var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
            _purchasedMovies.Add(purchasedMovie);
            MoneySpent += price;
        }

        public virtual Result CanPromote()
        {
            if (Status.IsAdvanced)
                return Result.Fail("The customer already has the advanced status.");

            if (PurchasedMovies.Count(x =>
                    x.ExpirationDate == ExpirationDate.Infinite ||
                    x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
                return Result.Fail("The customer has to have at least 2 active movies during the last 30 days");

            return PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1))
                       .Sum(x => x.Price) < 100M
                ? Result.Fail("The customer needs to have spent at least 100 dollars during the last year")
                : Result.Ok();
        }

        public virtual void Promote()
        {
            if (CanPromote().IsFailure)
                throw new Exception();

            Status = Status.Promote();
        }

        public virtual bool HasPurchasedMovie(Movie movie) =>
            PurchasedMovies.Any(x => x.Movie == movie && !x.ExpirationDate.IsExpired);
    }
}