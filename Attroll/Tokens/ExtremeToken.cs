namespace Atroll
{
    internal enum ExtremeType
    {
        Lowest,
        Highest
    }

    internal readonly struct ExtremeToken : IToken<ExtremeType>
    {
        #region Fields

        private readonly ExtremeType m_Type;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        public string Value => m_Type.ToString ();

        public ExtremeType Type => m_Type;

        public int Line => m_Line;

        public int Column => m_Column;

        #endregion

        #region Constructors

        public ExtremeToken ( ExtremeType type, int line, int column )
        {
            m_Type = type;
            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

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