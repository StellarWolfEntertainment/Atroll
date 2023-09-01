using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atroll
{
    internal class NestableStatement
    {
        #region Fields

        private static readonly Random s_Random = new ();

        private readonly Statement m_Statment;
        private readonly NestableStatement m_Child;

        #endregion

        #region Properties

        public Statement Statment => m_Statment;

        public NestableStatement? Child => m_Child;

        #endregion

        #region Constructors

        public NestableStatement ( Statement statment, NestableStatement child = null )
        {
            m_Statment = statment;
            m_Child = child;
        }

        #endregion

        #region Methods

        public IEnumerable<int> Evaluate ( IEnumerable<int> previous )
        {
            VerbToken action = (VerbToken) m_Statment [ 0 ];
            IToken mod = m_Statment [ 1 ];

            switch ( action.Type )
            {
                case VerbType.Roll:
                    DieToken roll = (DieToken) mod;
                    previous = Enumerable.Range ( 0, roll.Count ).Select ( _ => s_Random.Next ( roll.Sides + 1 ) );
                    break;
                case VerbType.Add:
                    DieToken add = (DieToken) mod;
                    IEnumerable<int> next = Enumerable.Range ( 0, add.Count ).Select ( _ => s_Random.Next ( add.Sides + 1 ) );
                    previous = Enumerable.Concat ( previous, next );
                    break;
                case VerbType.Drop:
                    ExtremeToken drop = (ExtremeToken) mod;
                    int count = previous.Count () - 1;
                    previous = previous.OrderBy ( v => v );

                    if ( drop.Type == ExtremeType.Lowest )
                        previous = previous.Reverse ();

                    previous = previous.Take ( count );
                    break;
                case VerbType.If:
                    IntegerLiteralToken @if = (IntegerLiteralToken) mod;

                    if ( previous.Contains ( @if.Amount ) )
                    {
                        previous = m_Child?.Evaluate ( previous ) ?? throw new InvalidOperationException ( $"There is no action performed in this {action.Type} call on line {action.Line}" );
                    }

                    break;
                case VerbType.While:
                    IntegerLiteralToken @while = (IntegerLiteralToken) mod;

                    while ( previous.Contains ( @while.Amount ) )
                    {
                        previous = m_Child?.Evaluate ( previous ) ?? throw new InvalidOperationException ( $"There is no action performed in this {action.Type} call on line {action.Line}" );
                    }

                    break;
                case VerbType.Reroll:
                    IntegerLiteralToken reroll = (IntegerLiteralToken) mod;
                    IntegerLiteralToken rerolldie = (IntegerLiteralToken) m_Statment [ 2 ];

                    List<int> values = previous.ToList ();

                    for ( int i = 0; i < values.Count; i++ )
                    {
                        if ( values [ i ] == reroll.Amount )
                        {
                            values [ i ] = s_Random.Next ( rerolldie.Amount + 1 );
                        }
                    }

                    break;
                case VerbType.Mod:
                    IntegerLiteralToken modifier = (IntegerLiteralToken) mod;
                    int total = previous.Sum () + modifier.Amount;
                    previous = new List<int> () { total };
                    break;
            }

            return previous;
        }

        #endregion
    }
}