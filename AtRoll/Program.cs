namespace AtRoll
{
    public sealed class Program
    {
        #region Fields

        private readonly List<NestableStatement> m_Statements;

        #endregion

        #region Constructors

        private Program ( IEnumerable<Statement> statements )
        {

            m_Statements = new List<NestableStatement> ();
            Queue<Statement> statementsL = new ( statements.Reverse () );

            NestableStatement nextStatement = null;

            while ( statementsL.Count > 0 )
            {
                Statement statement = statementsL.Dequeue ();

                if ( statement.Verb.Type is VerbType.If or VerbType.While or VerbType.For )
                {
                    nextStatement = new NestableStatement ( statement, nextStatement );
                }
                else
                {
                    if ( nextStatement != null )
                        m_Statements.Add ( nextStatement );
                    nextStatement = new NestableStatement ( statement, null );
                }
            }

            m_Statements.Add ( nextStatement );
            m_Statements.Reverse ();
        }

        public static Program Create ( Stream stream )
        {
            using ( StreamReader reader = new ( stream ) )
            {
                string program = reader.ReadToEnd ();
                return Create ( program );
            }
        }

        public static Program Create ( string program )
        {
            Tokenizer tokens = new ( program );
            StatementList statements = new ( tokens );
            return new ( statements );
        }

        #endregion

        #region Methods

        public int Evaluate ()
        {
            IEnumerable<int> results = null;

            foreach ( NestableStatement statement in m_Statements )
            {
                if ( statement == null )
                    throw new NullReferenceException ( "The statement is null" );
                results = statement.Evaluate ( results );
            }

            return results.Sum ();
        }

        #endregion
    }
}