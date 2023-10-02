namespace UI.Common.Button
{
    public interface IButtonParams
    {
    }

    public class NoButtonParams : IButtonParams
    {
    }

    public class ToggleButtonParams : IButtonParams
    {
        public bool Value { get; }

        public ToggleButtonParams(bool value)
        {
            Value = value;
        }

        public static implicit operator ToggleButtonParams(bool value)
        {
            return new ToggleButtonParams(value);
        }
    }
}