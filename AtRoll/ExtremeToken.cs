namespace AtRoll
{
    /// <summary>
    /// Represents the type of an extreme value (either Lowest or Highest).
    /// </summary>
    internal enum ExtremeType
    {
        Lowest,
        Highest
    }

    /// <summary>
    /// Represents a token for an extreme value in a text document.
    /// </summary>
    internal readonly struct ExtremeToken : IToken<ExtremeType>
    {
        #region Fields

        private readonly ExtremeType m_Type;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string representation of the extreme value.
        /// </summary>
        public string Value => m_Type.ToString ();

        /// <summary>
        /// Gets the type of the extreme value.
        /// </summary>
        public ExtremeType Type => m_Type;

        /// <summary>
        /// Gets the line number where the extreme token is located.
        /// </summary>
        public int Line => m_Line;

        /// <summary>
        /// Gets the column number where the extreme token is located.
        /// </summary>
        public int Column => m_Column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtremeToken"/> struct.
        /// </summary>
        /// <param name="type">The type of the extreme value.</param>
        /// <param name="line">The line number where the extreme token is located.</param>
        /// <param name="column">The column number where the extreme token is located.</param>
        public ExtremeToken ( ExtremeType type, int line, int column )
        {
            m_Type = type;
            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the extreme token to ensure it represents a valid extreme value.
        /// </summary>
        public void Validate ()
        {
            if ( !Enum.IsDefined ( m_Type ) )
            {
                throw new InvalidOperationException ( $"Invalid Extreme '{m_Type}' on line {m_Line} at column {m_Column}" );
            }
        }

        #endregion
    }
}