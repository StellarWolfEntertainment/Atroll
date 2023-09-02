namespace AtRoll
{
    /// <summary>
    /// Represents a token for an integer literal in a text document.
    /// </summary>
    internal readonly struct IntegerLiteralToken : IToken
    {

        #region Fields

        private readonly int m_Value;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer value of the token.
        /// </summary>
        public int Value => m_Value;

        /// <summary>
        /// Gets the line number where the integer literal token is located.
        /// </summary>
        public int Line => m_Line;

        /// <summary>
        /// Gets the column number where the integer literal token is located.
        /// </summary>
        public int Column => m_Column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerLiteralToken"/> struct.
        /// </summary>
        /// <param name="token">The token representing the integer literal.</param>
        /// <param name="line">The line number where the integer literal token is located.</param>
        /// <param name="column">The column number where the integer literal token is located.</param>
        public IntegerLiteralToken ( int token, int line, int column )
        {
            m_Value = token;
            m_Line = line;
            m_Column = column;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Validates the integer literal token.
        /// </summary>
        public void Validate () { }

        #endregion
    }
}
