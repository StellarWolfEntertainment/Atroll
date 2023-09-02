using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRoll
{
    internal class Statement : IEnumerable<IToken>
    {
        #region Fields

        private readonly List<IToken> m_Tokens;

        #endregion

        #region Properties

        public IToken this [ int index ] => m_Tokens [ index ];

        public int Count => m_Tokens.Count;

        public VerbToken Verb => (VerbToken) m_Tokens [ 0 ];

        #endregion

        #region Constructors

        public Statement ( IEnumerable<IToken> tokens )
        {
            m_Tokens = new List<IToken> ( tokens );

            int expected = 2;

            if ( m_Tokens.Count is 2 or 3 )
            {
                if ( m_Tokens [ 0 ] is VerbToken verb )
                {
                    for ( int i = 1; i < m_Tokens.Count; i++ )
                    {
                        if ( m_Tokens [ i ] is VerbToken )
                        {
                            throw new InvalidProgramException ( $"Verbs can only be placed at the start of a statement on line {verb.Line} at column {m_Tokens [ i ].Column}" );
                        }
                    }

                    IToken rule = m_Tokens [ 1 ];

                    if ( verb.Type is VerbType.Roll )
                    {
                        if ( rule is not (DieToken or PartialDieToken) )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by a Die declaration on line {verb.Line} at column {rule.Column}" );
                        }
                    }
                    else if ( verb.Type is VerbType.Drop )
                    {
                        if ( rule is not ExtremeToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Extreme declaration on line {verb.Line} at column {rule.Column}" );
                        }
                    }
                    else if ( verb.Type is VerbType.Reroll )
                    {
                        expected = 3;

                        if ( rule is not EqualityIntegerToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Integer declaration on line {verb.Line} at column {rule.Column}" );
                        }

                        if ( m_Tokens [ 2 ] is not PartialDieToken)
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls contain an Integer declaration followed by a PArtial Die declaration on line {verb.Line} at column {m_Tokens [ 2 ].Column}" );
                        }
                    }
                    else if ( verb.Type is VerbType.While or VerbType.If )
                    {
                        if ( rule is not ( EqualityIntegerToken or IntegerLiteralToken ) )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Equality Integer or Integer declaration on line {verb.Line} at column {rule.Column}" );
                        }
                    }
                    else if ( verb.Type is VerbType.For or VerbType.Add or VerbType.Sub or VerbType.Mul or VerbType.Div )
                    {
                        if ( rule is not IntegerLiteralToken )
                        {
                            throw new InvalidProgramException ( $"{verb.Type} calls must be followed by an Integer declaration on line {verb.Line} at column {rule.Column}" );
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