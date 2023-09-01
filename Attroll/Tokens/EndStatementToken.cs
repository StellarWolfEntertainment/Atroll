namespace Attroll
{
    internal readonly struct EndStatementToken : IToken
    {
        #region Fields

        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        public string Value => "\n";

        public int Line => m_Line;

        public int Column => m_Column;

        #endregion

        #region Constructors

        public EndStatementToken ( int line, int column )
        {
            m_Line = line;
            m_Column = column;
        }

        public void Validate () { }

        #endregion
    }
}