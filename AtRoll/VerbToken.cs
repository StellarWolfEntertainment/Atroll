namespace AtRoll
{
    /// <summary>
    /// Represents the type of verb used in a program, such as Roll, Reroll, Drop, etc.
    /// </summary>
    internal enum VerbType
    {
        Roll, // MUST be the first call of the program, subsequent calls 'add' to the current set
        Reroll,
        Drop,
        Keep,

        // Nesting Calls
        While,
        If,
        For,

        // Summation calls
        Add,
        Sub,
        Mul,
        Div, // Div will truncate the result since this language has no concept of floating point numbers
    }

    /// <summary>
    /// Represents a token for a verb type in a text document.
    /// </summary>
    internal readonly struct VerbToken : IToken<VerbType>
    {
        #region Fields

        private readonly VerbType m_Type;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the verb.
        /// </summary>
        public VerbType Type => m_Type;

        /// <summary>
        /// Gets the line number where the verb token is located.
        /// </summary>
        public int Line => m_Line;

        /// <summary>
        /// Gets the column number where the verb token is located.
        /// </summary>
        public int Column => m_Column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbToken"/> struct.
        /// </summary>
        /// <param name="type">The type of the verb.</param>
        /// <param name="line">The line number where the verb token is located.</param>
        /// <param name="column">The column number where the verb token is located.</param>
        public VerbToken ( VerbType type, int line, int column )
        {
            m_Type = type;
            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the verb token to ensure it represents a valid verb type.
        /// </summary>
        public void Validate ()
        {
            if ( !Enum.IsDefined ( m_Type ) )
            {
                throw new InvalidOperationException ( $"Invalid Verb '{m_Type}' on line {m_Line} at column {m_Column}" );
            }
        }

        #endregion
    }
}
