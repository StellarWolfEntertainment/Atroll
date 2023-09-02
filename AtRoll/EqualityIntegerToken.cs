namespace AtRoll
{
    /// <summary>
    /// Represents the type of equality for an integer literal token.
    /// </summary>
    internal enum EqualityType
    {
        Equal,
        Not,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual
    }

    /// <summary>
    /// Represents a token for an equality integer literal in a text document.
    /// </summary>
    internal readonly struct EqualityIntegerToken : IToken<EqualityType>
    {

        #region Fields

        private readonly EqualityType m_Type;
        private readonly int m_Value;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the equality type associated with the integer literal token.
        /// </summary>
        public EqualityType Type => m_Type;

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
        /// Initializes a new instance of the <see cref="EqualityIntegerToken"/> struct.
        /// </summary>
        /// <param name="token">The token representing the integer literal with an associated equality type.</param>
        /// <param name="line">The line number where the integer literal token is located.</param>
        /// <param name="column">The column number where the integer literal token is located.</param>
        public EqualityIntegerToken ( string token, int line, int column )
        {

            if ( token.StartsWith ( '!' ) )
            {
                m_Type = EqualityType.Not;
                token = token [ 1.. ];
            }
            else if ( token.StartsWith ( "<=" ) )
            {
                m_Type = EqualityType.LessThanOrEqual;
                token = token [ 2.. ];
            }
            else if ( token.StartsWith ( ">=" ) )
            {
                m_Type = EqualityType.GreaterThanOrEqual;
                token = token [ 2.. ];
            }
            else if ( token.StartsWith ( "<" ) )
            {
                m_Type = EqualityType.LessThan;
                token = token [ 1.. ];
            }
            else if ( token.StartsWith ( ">" ) )
            {
                m_Type = EqualityType.GreaterThan;
                token = token [ 1.. ];
            }
            else
            {
                m_Type = EqualityType.Equal;
            }

            m_Value = int.Parse ( token );
            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a given integer value is within the range specified by the token's equality type.
        /// </summary>
        /// <param name="value">The integer value to check.</param>
        /// <returns>True if the value is within the range; otherwise, false.</returns>
        public bool IsInRange ( int value )
        {
            return m_Type switch
            {
                EqualityType.Equal => value == m_Value,
                EqualityType.LessThan => value < m_Value,
                EqualityType.GreaterThan => value > m_Value,
                EqualityType.LessThanOrEqual => value <= m_Value,
                EqualityType.GreaterThanOrEqual => value >= m_Value,
                EqualityType.Not => value != m_Value,
                _ => false,
            };
        }

        /// <summary>
        /// Validates the equality integer literal token.
        /// </summary>
        public void Validate () { }

        #endregion
    }
}
