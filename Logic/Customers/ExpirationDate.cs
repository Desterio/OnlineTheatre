using System;
using Logic.Common;

namespace Logic.Customers
{
    public class ExpirationDate : ValueObject<ExpirationDate>
    {
        public static readonly ExpirationDate Infinite = new ExpirationDate(null);

        private ExpirationDate(DateTime? date)
        {
            Date = date;
        }

        public DateTime? Date { get; }

        public bool IsExpired => this != Infinite && Date < DateTime.UtcNow;

        protected override int GetHashCodeCore() => Date.GetHashCode();

        protected override bool EqualsCore(ExpirationDate other) => Date == other.Date;

        public static Result<ExpirationDate> Create(DateTime date) => Result.Ok(new ExpirationDate(date));

        public static implicit operator DateTime?(ExpirationDate date) => date.Date;

        public static ExpirationDate Of(DateTime? date) => date.HasValue ? Create(date.Value).Value : Infinite;
    }
}