using System.Collections;
using System.Text.RegularExpressions;

namespace AtRoll
{
    /// <summary>
    /// Represents a tokenizer for processing program code in the 'The New World' game.
    /// </summary>
    internal partial class Tokenizer : IEnumerable<IToken>
    {
        #region Fields

        private readonly List<IToken> m_Tokens;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tokenizer"/> class with the program code.
        /// </summary>
        /// <param name="program">The program code to tokenize.</param>
        public Tokenizer ( string program )
        {
            m_Tokens = new List<IToken> ();

            string [] lines = program.Split ( '\n', StringSplitOptions.RemoveEmptyEntries );

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
                    IToken token = ParseToken ( currentWord, lineNumber, columnNumber - currentWord.Length );
                    token.Validate ();
                    m_Tokens.Add ( token );
                }

                m_Tokens.Add ( new EndStatementToken ( lineNumber, columnNumber ) );
                lineNumber++;
            }

            if ( m_Tokens [ 0 ] is not VerbToken verbToken || verbToken.Type != VerbType.Roll )
                throw new InvalidProgramException ( "@Roll programs must start with a Roll call" );
        }

        #endregion

        #region Methods

        private IToken ParseToken ( string currentWord, int lineNumber, int columnNumber )
        {
            if ( int.TryParse ( currentWord, out int @int ) )
            {
                return new IntegerLiteralToken ( @int, lineNumber, columnNumber );
            }
            else if ( IntegerLiteralRegex ().IsMatch ( currentWord ) )
            {
                return new EqualityIntegerToken ( currentWord, lineNumber, columnNumber );
            }
            else if ( Enum.TryParse ( currentWord, true, out ExtremeType extreme ) )
            {
                return new ExtremeToken ( extreme, lineNumber, columnNumber );
            }
            else if ( Enum.TryParse ( currentWord, true, out VerbType verb ) )
            {
                return new VerbToken ( verb, lineNumber, columnNumber );
            }
            else if ( PartialDieRegex ().IsMatch ( currentWord ) )
            {
                return new PartialDieToken ( currentWord, lineNumber, columnNumber );
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

        /// <summary>
        /// Returns an enumerator that iterates through the tokens produced by the tokenizer.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the tokens.</returns>
        public IEnumerator<IToken> GetEnumerator () => m_Tokens.GetEnumerator ();

        /// <summary>
        /// Returns an enumerator that iterates through the tokens produced by the tokenizer.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the tokens.</returns>
        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        [GeneratedRegex ( "^((?:[-0-9]+)|(?:(?:!|<=|>=|<|>)[-0-9]+))$" )]
        private static partial Regex IntegerLiteralRegex ();

        [GeneratedRegex ( "^[-]{0,1}[0-9]+d[0-9]+$" )]
        private static partial Regex DieRegex ();

        [GeneratedRegex ( "^[-]{0,1}d[0-9]+$" )]
        private static partial Regex PartialDieRegex ();

        #endregion
    }
}
