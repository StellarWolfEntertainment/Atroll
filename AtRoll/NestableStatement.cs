namespace AtRoll
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

        public IEnumerable<int> Evaluate ( IEnumerable<int> previous = null )
        {
            VerbToken action = m_Statment.Verb;
            IToken rule = m_Statment [ 1 ];

            switch ( action.Type )
            {
                case VerbType.Roll:
                    int count;
                    int sides;
                    bool rollNegative;
                    if ( rule is PartialDieToken partialDieRoll )
                    {
                        count = 1;
                        sides = partialDieRoll.Sides;
                        rollNegative = partialDieRoll.Negative;
                    }
                    else
                    {
                        DieToken roll = (DieToken) rule;
                        count = roll.Count;
                        sides = roll.Sides;
                        rollNegative = roll.Negative;
                    }

                    if ( previous == null )
                    {
                        previous = Enumerable.Range ( 0, count ).Select ( _ =>
                        {
                            int result = s_Random.Next ( sides + 1 );
                            if ( rollNegative )
                                result = -result;
                            return result;
                        } );
                    }
                    else
                    {
                        IEnumerable<int> next = Enumerable.Range ( 0, count ).Select ( _ =>
                        {
                            int result = s_Random.Next ( sides + 1 );
                            if ( rollNegative )
                                result = -result;
                            return result;
                        } );
                        previous = Enumerable.Concat ( previous, next );
                    }

                    break;
                case VerbType.Drop:
                    ExtremeToken extremeDrop = (ExtremeToken) rule;
                    int length = previous.Count () - 1;
                    previous = previous.OrderBy ( v => v );

                    if ( extremeDrop.Type == ExtremeType.Lowest )
                        previous = previous.Reverse ();

                    previous = previous.Take ( length );
                    break;
                case VerbType.Reroll:
                    IntegerLiteralToken rerollCond = (IntegerLiteralToken) rule;
                    PartialDieToken rerollSides = (PartialDieToken) m_Statment [ 2 ];

                    List<int> values = previous.ToList ();

                    for ( int i = 0; i < values.Count; i++ )
                    {
                        if ( values [ i ] == rerollCond.Value )
                        {
                            int result = s_Random.Next ( rerollSides.Sides + 1 );

                            if ( rerollSides.Negative )
                                result = -result;

                            values [ i ] = result;
                        }
                    }

                    previous = values;
                    break;
                case VerbType.If:
                    if ( rule is EqualityIntegerToken eitIf )
                    {

                    }
                    else
                    {
                        IntegerLiteralToken iltIf = (IntegerLiteralToken) rule;
                        eitIf = new EqualityIntegerToken ( iltIf.Value.ToString (), 0, 0 ); // since this is only temporary theres no reason to give positional data
                    }

                    if ( previous.Any ( eitIf.IsInRange ) )
                    {
                        previous = m_Child?.Evaluate ( previous ) ?? throw new InvalidOperationException ( $"There is no action performed in this {action.Type} call on line {action.Line}" );
                    }
                    break;
                case VerbType.While:
                    if ( rule is EqualityIntegerToken eitWhile )
                    {

                    }
                    else
                    {
                        IntegerLiteralToken iltWhile = (IntegerLiteralToken) rule;
                        eitWhile = new EqualityIntegerToken ( iltWhile.Value.ToString (), 0, 0 ); // since this is only temporary theres no reason to give positional data
                    }

                    while ( previous.Any ( eitWhile.IsInRange ) )
                    {
                        previous = m_Child?.Evaluate ( previous ) ?? throw new InvalidOperationException ( $"There is no action performed in this {action.Type} call on line {action.Line}" );
                    }
                    break;
                case VerbType.For:
                    IntegerLiteralToken iltFor = (IntegerLiteralToken) rule;

                    for ( int f = 0; f < iltFor.Value; f++ )
                    {
                        previous = m_Child?.Evaluate ( previous ) ?? throw new InvalidOperationException ( $"There is no action performed in this {action.Type} call on line {action.Line}" );
                    }
                    break;
                case VerbType.Add or VerbType.Sub or VerbType.Mul or VerbType.Div:
                    IntegerLiteralToken iltMath = (IntegerLiteralToken) rule;
                    int sum = previous.Sum ();

                    if ( action.Type == VerbType.Add )
                        sum += iltMath.Value;
                    else if ( action.Type == VerbType.Sub )
                        sum -= iltMath.Value;
                    else if ( action.Type == VerbType.Mul )
                        sum *= iltMath.Value;
                    else if ( action.Type == VerbType.Div )
                        sum /= iltMath.Value;

                    previous = new List<int> () { sum };

                    break;
            }

            return previous;
        }

        #endregion
    }
}