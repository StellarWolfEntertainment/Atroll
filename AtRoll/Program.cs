namespace AtRoll
{
    /// <summary>
    /// Represents the main program for processing statements in the @Roll Language.
    /// </summary>
    public sealed class Program
    {
        #region Fields

        private readonly List<NestableStatement> m_Statements;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor for creating a Program instance from a collection of statements.
        /// </summary>
        /// <param name="statements">The collection of statements to process.</param>
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

        /// <summary>
        /// Creates a Program instance from a stream containing program statements.
        /// </summary>
        /// <param name="stream">The input stream containing program statements.</param>
        /// <returns>A Program instance representing the parsed program.</returns>
        public static Program Create ( Stream stream )
        {
            using ( StreamReader reader = new ( stream ) )
            {
                string program = reader.ReadToEnd ();
                return Create ( program );
            }
        }

        /// <summary>
        /// Creates a Program instance from a string containing program statements.
        /// </summary>
        /// <param name="program">The input string containing program statements.</param>
        /// <returns>A Program instance representing the parsed program.</returns>
        public static Program Create ( string program )
        {
            Tokenizer tokens = new ( program );
            StatementList statements = new ( tokens );
            return new Program ( statements );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the evaluation chain of statements and returns the results.
        /// </summary>
        /// <returns>An IEnumerable of integers representing the results of evaluating the statements.</returns>
        public IEnumerable<int> GetEvaluationChain ()
        {
            IEnumerable<int> results = null;

            foreach ( NestableStatement statement in m_Statements )
            {
                if ( statement == null )
                    throw new NullReferenceException ( "The statement is null" );
                results = statement.Evaluate ( results );
            }

            return results;
        }

        /// <summary>
        /// Evaluates the program and returns the sum of the results.
        /// </summary>
        /// <returns>The sum of the results of evaluating the program's statements.</returns>
        public int Evaluate () => GetEvaluationChain ().Sum ();

        #endregion
    }
}