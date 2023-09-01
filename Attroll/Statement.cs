using System.Collections;

namespace Atroll
{
    internal readonly struct Statement : IReadOnlyList<IToken>
    {
        #region Fields

        private readonly List<IToken> m_Tokens;

        #endregion

        #region Properties

        public readonly IToken this [ int index ] => m_Tokens [ index ];

        public readonly int Count => m_Tokens.Count;

        public readonly VerbToken Verb => (VerbToken) m_Tokens [ 0 ];

        #endregion

        #region Constructors

        public Statement ( IEnumerable<IToken> tokens ) => m_Tokens = new ( tokens );

        #endregion

        #region Methods

        public readonly IEnumerator<IToken> GetEnumerator () => m_Tokens.GetEnumerator ();

        readonly IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        public void Validate ()
        {
            int expected;
            if ( m_Tokens != null && m_Tokens.Count is 2 or 3 )
            {
                if ( m_Tokens [ 0 ] is VerbToken verb )
                {
                    if ( m_Tokens [ 1 ] is VerbToken )
                    {
                        throw new InvalidProgramException ( $"Verbs can only be placed at the start of a statement on line {verb.Line} at column {m_Tokens [ 1 ].Column}" );
                    }

                    IToken rule = m_Tokens [ 1 ];

                    if ( verb.Type is VerbType.Roll or VerbType.Add )
                    {
                        expected = 2;
                        if ( rule is not DieToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by a Die declaration on line {verb.Line} at column {rule.Column}" );
                        }
                    }
                    else if ( verb.Type is VerbType.While or VerbType.If or VerbType.Reroll or VerbType.Mod )
                    {
                        expected = 2;

                        if ( rule is not IntegerLiteralToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Integer declaration on line {verb.Line} at column {rule.Column}" );
                        }

                        if ( verb.Type is VerbType.Reroll )
                        {
                            expected = 3;
                            if ( m_Tokens [ 2 ] is not IntegerLiteralToken reroll || reroll.Amount < 0 )
                            {
                                throw new InvalidProgramException ( $"{verb.Type} calls contain an Integer declaration followed by a valid Die sides declaration on line {verb.Line} at column {m_Tokens [ 2 ].Column}" );
                            }
                        }
                    }
                    else if ( verb.Type is VerbType.Drop )
                    {
                        expected = 2;
                        if ( rule is not ExtremeToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Extreme declaration on line {verb.Line} at column {rule.Column}" );
                        }
                    }
                    else
                    {
                        throw new InvalidProgramException ( $"Honestly I don't know how this is possible, but this statement doesn't contain a valid Verb on line {verb.Line}" );
                    }
                }
                else
                {
                    throw new InvalidProgramException ( $"Statements must begin with a Verb on line {m_Tokens [ 0 ].Line}" );
                }
            }
            else
            {
                throw new InvalidProgramException ( $"Honestly I don't know how this is possible, but this statement contains no tokens" );
            }

            if ( m_Tokens.Count != expected )
            {
                throw new InvalidProgramException ( $"{expected} tokens expected, {m_Tokens.Count} found on line {m_Tokens [ 0 ].Line}" );
            }
        }

        #endregion
    }
}