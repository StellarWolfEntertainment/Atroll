namespace Attroll
{
    internal readonly struct IntegerLiteralToken : IToken
    {
        #region Fields

        private readonly int m_Value;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        public string Value => m_Value.ToString ();
        public int Amount => m_Value;

        public int Line => m_Line;

        public int Column => m_Column;

        #endregion

        #region Constructors

        public IntegerLiteralToken ( int value, int line, int column )
        {
            m_Value = value;
            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        public void Validate () { }

        #endregion
    }
}