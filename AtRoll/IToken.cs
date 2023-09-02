namespace AtRoll
{
    /// <summary>
    /// Represents an interface for tokens in a text document.
    /// </summary>
    internal interface IToken
    {
        /// <summary>
        /// Gets the line number where the token is located.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Gets the column number where the token is located.
        /// </summary>
        int Column { get; }

        /// <summary>
        /// Validates the token.
        /// </summary>
        void Validate ();
    }

    /// <summary>
    /// Represents an interface for typed tokens in a text document.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type representing token types.</typeparam>
    internal interface IToken<TEnum> : IToken where TEnum : struct, Enum
    {
        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        TEnum Type { get; }
    }
}