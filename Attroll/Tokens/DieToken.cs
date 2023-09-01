namespace Attroll
{
    internal readonly struct DieToken : IToken
    {
        #region Fields

        private readonly string m_Value;
        private readonly int m_Count;
        private readonly int m_Sides;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        public string Value => m_Value;

        public int Count => m_Count;

        public int Sides => m_Sides;

        public int Line => m_Line;

        public int Column => m_Column;

        #endregion

        #region Constructors

        public DieToken ( string token, int line, int column )
        {
            m_Value = token;

            string [] parts = token.Split ( 'd', StringSplitOptions.RemoveEmptyEntries );
            // this is confirmed valid prior to creating the token, so no need to check again
            m_Count = int.Parse ( parts [ 0 ] );
            m_Sides = int.Parse ( parts [ 1 ] );

            m_Line = line;
            m_Column = column;
        }

        #endregion

        #region Methods

        public void Validate ()
        {
            if ( m_Count < 0 )
            {
                throw new InvalidProgramException ( $"Invalid die count '{m_Count}' on line {m_Line} at column {m_Column}" );
            }

            if ( m_Sides is not ( 4 or 6 or 8 or 10 or 12 or 20 or 100 ) )
            {
                int col = m_Column + m_Count.ToString ().Length + 1;
                throw new InvalidProgramException ( $"Invalid die sides '{m_Sides}' on line {m_Line} at column {col}" );
            }
        }

        #endregion
    }
}