namespace FlexibleSpells.Operators
{
    public class Constant<T> : Operator
    {
        private readonly T _value;

        public Constant(T value)
        {
            _value = value;
        }

        public T Apply()
        {
            return _value;
        }
    }
}