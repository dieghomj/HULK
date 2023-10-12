namespace HULK.CodeAnalysis.Binding
{
    internal class BoundIfElseExpression : BoundExpression
    {
        public BoundIfElseExpression(BoundExpression condition, BoundExpression trueExpression, BoundExpression falseExpression)
        {
            Condition = condition;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public BoundExpression Condition { get; }
        public BoundExpression TrueExpression { get; }
        public BoundExpression FalseExpression { get; }

        public override BoundNodeKind Kind => BoundNodeKind.IfElseExpression;
        public override Type Type => typeof(void);

    }
}