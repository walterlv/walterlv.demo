namespace Walterlv.Demo.Patterns
{
    public struct Fantastic
    {
        private readonly IFantastic _value;
        private Fantastic(IFantastic value) => _value = value;
        public static implicit operator Fantastic(IFantastic value) => new Fantastic(value);
        public override string ToString() => $"{_value} is fantastic.";
    }

    public class IFantastic
    {
    }
}
