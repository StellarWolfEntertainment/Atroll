namespace AtRoll
{
    /// <summary>
    /// Represents a nestable statement in the @Roll Language.
    /// </summary>
    internal sealed class NestableStatement
    {
        #region Fields

        private static readonly Random s_Random = new ();

        private readonly Statement m_Statement;
        private readonly NestableStatement m_Child;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the statement associated with this nestable statement.
        /// </summary>
        public Statement Statement => m_Statement;

        /// <summary>
        /// Gets the child nestable statement if one exists, otherwise null.
        /// </summary>
        public NestableStatement Child => m_Child;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NestableStatement"/> class.
        /// </summary>
        /// <param name="statement">The main statement of this nestable statement.</param>
        /// <param name="child">The child nestable statement if one exists.</param>
        public NestableStatement ( Statement statement, NestableStatement child = null )
        {
            m_Statement = statement;
            m_Child = child;
        }

        #endregion

        #region Methods

        private static IEnumerable<int> Generate ( int count, int sides, bool negative )
        {
            return Enumerable.Range ( 0, count ).Select ( _ =>
            {
                int result = s_Random.Next ( sides ) + 1;
                if ( negative )
                    result = -result;
                return result;
            } );
        }

        /// <summary>
        /// Evaluates the nestable statement and returns a sequence of integers based on the specified rules.
        /// </summary>
        /// <param name="previous">The previous sequence of integers (optional).</param>
        /// <returns>A sequence of integers resulting from the evaluation of the nestable statement.</returns>
        public IEnumerable<int> Evaluate ( IEnumerable<int> previous = null )
        {
            VerbToken action = m_Statement.Verb;
            IToken rule = m_Statement [ 1 ];

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
                        previous = Generate ( count, sides, rollNegative );
                    }
                    else
                    {
                        IEnumerable<int> next = Generate ( count, sides, rollNegative );
                        previous = Enumerable.Concat ( previous, next );
                    }

                    break;
                case VerbType.Drop:
                    ExtremeToken extremeDrop = (ExtremeToken) rule;
                    int length = previous.Count () - 1;
                    if ( m_Statement.Count == 3 )
                        length -= ( (IntegerLiteralToken) m_Statement [ 2 ] ).Value;
                    previous = previous.OrderBy ( v => v );

                    if ( extremeDrop.Type == ExtremeType.Lowest )
                        previous = previous.Reverse ();

                    previous = previous.Take ( length );
                    break;
                case VerbType.Keep:
                    ExtremeToken extremeKeep = (ExtremeToken) rule;
                    int lengthKeep = previous.Count () + 1;
                    if ( m_Statement.Count == 3 )
                        lengthKeep += ( (IntegerLiteralToken) m_Statement [ 2 ] ).Value;
                    previous = previous.OrderBy ( v => v );

                    if ( extremeKeep.Type == ExtremeType.Highest )
                        previous = previous.Reverse ();

                    previous = previous.Take ( lengthKeep );
                    break;
                case VerbType.Reroll:
                    if ( rule is EqualityIntegerToken rerollCond )
                    {

                    }
                    else
                    {
                        IntegerLiteralToken iltIf = (IntegerLiteralToken) rule;
                        rerollCond = new EqualityIntegerToken ( iltIf.Value.ToString (), 0, 0 ); // since this is only temporary theres no reason to give positional data
                    }

                    PartialDieToken rerollSides = (PartialDieToken) m_Statement [ 2 ];

                    List<int> values = previous.ToList ();

                    for ( int i = 0; i < values.Count; i++ )
                    {
                        if ( rerollCond.IsInRange ( values [ i ] ) )
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