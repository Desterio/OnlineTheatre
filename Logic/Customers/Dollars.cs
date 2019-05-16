using Logic.Common;

namespace Logic.Customers
{
    public class Dollars : ValueObject<Dollars>
    {
        private const decimal MaxDollarAmount = 1_000_000;

        private Dollars(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }
        public bool IsZero => Value == 0;
        protected override int GetHashCodeCore() => Value.GetHashCode();

        protected override bool EqualsCore(Dollars other) => Value == other.Value;

        public static Result<Dollars> Create(decimal dollarAmount)
        {
            if (dollarAmount < 0)
                return Result.Fail<Dollars>("Dollar amount cannot be negative.");

            if (dollarAmount > MaxDollarAmount)
                return Result.Fail<Dollars>($"Dollar amount cannot exceed {MaxDollarAmount}");

            return dollarAmount % 0.01m > 0
                ? Result.Fail<Dollars>("Dollar amount cannot contain part of a penny.")
                : Result.Ok(new Dollars(dollarAmount));
        }

        public static Dollars operator *(Dollars dollars, decimal multiplier) => new Dollars(dollars.Value * multiplier);

        public static Dollars operator +(Dollars dollars1, Dollars dollars2) =>
            new Dollars(dollars1.Value + dollars2.Value);

        public static Dollars Of(decimal dollarAmount) => Create(dollarAmount).Value;

        public static implicit operator decimal(Dollars dollars) => dollars.Value;
    }
}