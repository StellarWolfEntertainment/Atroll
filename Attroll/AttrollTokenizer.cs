using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Atroll
{
    internal partial class AtrollTokenizer : IReadOnlyList<IToken>
    {
        #region Fields

        private readonly List<IToken> m_Tokens;

        #endregion

        #region Properties

        public IToken this [ int index ] => throw new NotImplementedException ();

        public int Count => throw new NotImplementedException ();

        #endregion

        #region Constructors

        public AtrollTokenizer ( string program )
        {
            m_Tokens = new List<IToken> ();

            string [] lines = program.Split ( "\n", StringSplitOptions.RemoveEmptyEntries );

            int lineNumber = 1;

            foreach ( string line in lines )
            {
                int columnNumber = 1;
                string currentWord = string.Empty;

                foreach ( char c in line )
                {
                    if ( char.IsWhiteSpace ( c ) )
                    {
                        if ( !string.IsNullOrEmpty ( currentWord ) )
                        {
                            IToken token = ParseToken ( currentWord, lineNumber, columnNumber - currentWord.Length );
                            token.Validate ();
                            m_Tokens.Add ( token );
                            currentWord = string.Empty;
                        }
                    }
                    else
                    {
                        currentWord += c;
                    }

                    columnNumber++;
                }

                if ( !string.IsNullOrEmpty ( currentWord ) )
                {
                    m_Tokens.Add ( ParseToken ( currentWord, lineNumber, columnNumber ) );
                }

                m_Tokens.Add ( new EndStatementToken () );
                lineNumber++;
                columnNumber = 1;
            }

            if ( m_Tokens [ 0 ] is not VerbToken verbToken || verbToken.Type != VerbType.Roll )
            {
                throw new InvalidProgramException ( "@roll programs must start with a Roll call" );
            }

            if ( m_Tokens.Count ( token => token is VerbToken verb && verb.Type == VerbType.Roll ) > 1 )
            {
                throw new InvalidProgramException ( "@roll programs can only call Roll once at the start of the program, did you mean to 'Add'?" );
            }
        }

        private IToken ParseToken ( string currentWord, int lineNumber, int columnNumber )
        {
            if ( int.TryParse ( currentWord, out int @int ) )
            {
                return new IntegerLiteralToken ( @int, lineNumber, columnNumber );
            }
            else if ( Enum.TryParse ( currentWord, true, out ExtremeType extreme ) )
            {
                return new ExtremeToken ( extreme, lineNumber, columnNumber );
            }
            else if ( Enum.TryParse ( currentWord, true, out VerbType verb ) )
            {
                return new VerbToken ( verb, lineNumber, columnNumber );
            }
            else if ( DieRegex ().IsMatch ( currentWord ) )
            {
                return new DieToken ( currentWord, lineNumber, columnNumber );
            }
            else
            {
                throw new InvalidProgramException ( $"Unrecognized Token '{currentWord}' on line {lineNumber} at column {columnNumber}" );
            }
        }

        #endregion

        #region Methods

        public IEnumerator<IToken> GetEnumerator () => m_Tokens.GetEnumerator ();

        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        [GeneratedRegex ( "^[0-9]+d[0-9]+" )]
        private static partial Regex DieRegex ();

        #endregion
    }
}