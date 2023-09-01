namespace Attroll
{
    internal enum VerbType
    {
        Roll, // this can only appear once, and it MUST be the first call of the program
        While,
        If,
        Reroll,
        Add,
        Drop,
        Mod
    }

    internal readonly struct VerbToken : IToken<VerbType>
    {
        #region Fields

        private readonly VerbType m_Type;
        private readonly int m_Line;
        private readonly int m_Column;

        #endregion

        #region Properties

        public string Value => m_Type.ToString ();

        public VerbType Type => m_Type;

        public int Line => m_Line;
        public int Column => m_Column;

        #endregion

        #region Constructors

        public VerbToken ( VerbType type, int line, int column )
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
                throw new InvalidOperationException ( $"Invalid Verb '{m_Type}' on line {m_Line} at column {m_Column}" );
            }
        }

        #endregion
    }
}