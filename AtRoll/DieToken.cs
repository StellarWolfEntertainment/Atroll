namespace AtRoll
{
    /// <summary>
    /// Represents a token for representing a die roll in a text document.
    /// </summary>
    internal readonly struct DieToken : IToken
    {
        #region Fields

        private readonly int m_Count;
        private readonly int m_Sides;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of dice to roll.
        /// </summary>
        public int Count => Math.Abs ( m_Count );

        /// <summary>
        /// Gets the number of sides on each die.
        /// </summary>
        public int Sides => m_Sides;

        public bool Negative => m_Count < 0;

        /// <summary>
        /// Gets the line number where the token is located.
        /// </summary>
        public int Line => m_Line;

        /// <summary>
        /// Gets the column number where the token is located.
        /// </summary>
        public int Column => m_Column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DieToken"/> struct.
        /// </summary>
        /// <param name="token">The token representing the die roll.</param>
        /// <param name="line">The line number where the token is located.</param>
        /// <param name="column">The column number where the token is located.</param>
        public DieToken ( string token, int line, int column )
        {
            string [] parts = token.Split ( 'd', StringSplitOptions.RemoveEmptyEntries );
            // This is confirmed valid prior to creating the token, so no need to check again
            m_Count = int.Parse ( parts [ 0 ] );
            m_Sides = int.Parse ( parts [ 1 ] );

            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the DieToken to ensure it represents a valid die roll.
        /// </summary>
        public void Validate ()
        {
            if ( m_Sides < 0 )
            {
                int col = m_Column + m_Count.ToString ().Length + 1;
                throw new InvalidProgramException ( $"Invalid die sides '{m_Sides}' on line {m_Line} at column {col}" );
            }
        }

        #endregion
    }
}
