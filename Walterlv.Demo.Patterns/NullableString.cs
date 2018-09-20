namespace Walterlv.Demo.Patterns
{
    public readonly struct NullableString
    {
        private readonly string _value;

        private NullableString(string value)
        {
            _value = value;
        }

        public static implicit operator NullableString? (string value)
        {
            return string.IsNullOrEmpty(value) ? (NullableString?)null : new NullableString(value);
        }

        public static implicit operator string(NullableString? nullableString)
        {
            return nullableString?.ToString() ?? string.Empty;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(_value) ? string.Empty : _value;
        }
    }
}
