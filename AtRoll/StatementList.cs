using System.Collections;

namespace AtRoll
{
    /// <summary>
    /// Represents a list of statements within the @Roll language
    /// </summary>
    internal class StatementList : IReadOnlyList<Statement>
    {
        #region Fields

        private readonly List<Statement> m_Statements;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the statement at the specified index within the statement list.
        /// </summary>
        /// <param name="index">The index of the statement to retrieve.</param>
        /// <returns>The statement at the specified index.</returns>
        public Statement this [ int index ] => m_Statements [ index ];

        /// <summary>
        /// Gets the number of statements in the statement list.
        /// </summary>
        public int Count => m_Statements.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementList"/> class with a collection of tokens.
        /// </summary>
        /// <param name="tokens">The collection of tokens representing the statement list.</param>
        public StatementList ( IEnumerable<IToken> tokens )
        {
            m_Statements = new List<Statement> ();
            List<IToken> statementTokens = new ();

            foreach ( IToken token in tokens )
            {
                if ( token is EndStatementToken )
                {
                    Statement statement = new ( statementTokens );
                    m_Statements.Add ( statement );
                    statementTokens.Clear ();
                }
                else
                {
                    statementTokens.Add ( token );
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the statements in the statement list.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the statements.</returns>
        public IEnumerator<Statement> GetEnumerator () => m_Statements.GetEnumerator ();

        /// <summary>
        /// Returns an enumerator that iterates through the statements in the statement list.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the statements.</returns>
        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        #endregion
    }
}