using System.Collections;

namespace Attroll
{
    public sealed class Executable
    {
        #region Fields

        private readonly List<NestableStatement> m_Statements;

        #endregion

        #region Constructors

        private Executable ( IEnumerable<Statement> statements )
        {
            m_Statements = new List<NestableStatement> ();
            Queue<Statement> statementsL = new ( statements.Reverse () );

            NestableStatement nextStatement = null;

            while ( statementsL.Count > 0 )
            {
                Statement statement = statementsL.Dequeue ();

                if ( statement.Verb.Type is VerbType.If or VerbType.While )
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

        public static Executable Create ( Stream stream )
        {
            using ( StreamReader reader = new ( stream ) )
            {
                string program = reader.ReadToEnd ();
                return Create ( program );
            }
        }

        public   static Executable Create ( string program )
        {
            AtrollTokenizer tokens = new ( program );
            StatementList statements = new ( tokens );
            return new ( statements );
        }

        #endregion

        #region Methods

        public int Evaluate ()
        {
            IEnumerable<int> results = new List<int> ();

            foreach ( NestableStatement statement in m_Statements )
            {
                if ( statement == null )
                    throw new NullReferenceException ( "The statement is null" );
                results = statement.Evaluate ( results );
            }

            return results.Sum ();
        }

        internal IEnumerator<NestableStatement> GetEnumerator () => m_Statements.GetEnumerator ();

        #endregion
    }
}