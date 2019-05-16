namespace Logic.Common
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (!(obj is T valueObject))
                return false;

            return GetType() == obj.GetType() && EqualsCore(valueObject);
        }

        public override int GetHashCode() => GetHashCodeCore();

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !(a == b);

        protected abstract int GetHashCodeCore();
        protected abstract bool EqualsCore(T other);
    }
}