using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atroll
{
    internal class StatementList : IReadOnlyList<Statement>
    {
        #region Fields

        private readonly List<Statement> m_Statements;

        #endregion

        #region Properties

        public Statement this [ int index ] => m_Statements [ index ];

        public int Count => m_Statements.Count;

        #endregion

        #region Constructors

        public StatementList ( IEnumerable<IToken> tokens )
        {
            m_Statements = new List<Statement> ();
            List<IToken> statementTokens = new();

            foreach ( IToken token in tokens )
            {
                if ( token is EndStatementToken )
                {
                    Statement statement = new ( statementTokens );
                    statement.Validate ();
                    m_Statements.Add ( statement );
                    statementTokens.Clear ();
                }
                else
                {
                    statementTokens.Add ( token );
                }
            }
        }

        public IEnumerator<Statement> GetEnumerator () => m_Statements.GetEnumerator ();
        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        #endregion

    }
}