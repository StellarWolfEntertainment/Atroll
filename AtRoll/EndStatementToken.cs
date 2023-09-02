namespace AtRoll
{
    /// <summary>
    /// Represents a token indicating the end of a statement in a text document.
    /// </summary>
    internal readonly struct EndStatementToken : IToken
    {
        #region Fields

        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the line number where the end statement token is located.
        /// </summary>
        public int Line => m_Line;

        /// <summary>
        /// Gets the column number where the end statement token is located.
        /// </summary>
        public int Column => m_Column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EndStatementToken"/> struct.
        /// </summary>
        /// <param name="line">The line number where the end statement token is located.</param>
        /// <param name="column">The column number where the end statement token is located.</param>
        public EndStatementToken ( int line, int column )
        {
            m_Line = line;
            m_Column = column;
        }

        /// <summary>
        /// Validates the end statement token.
        /// </summary>
        public void Validate () { }

        #endregion
    }
}