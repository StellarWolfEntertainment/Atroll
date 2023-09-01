namespace Atroll
{
    internal interface IToken
    {
        string Value { get; }

        int Line { get; }

        int Column { get; }

        void Validate ();
    }

    internal interface IToken<TEnum> : IToken where TEnum : struct, Enum
    {
        TEnum Type { get; }
    }
}