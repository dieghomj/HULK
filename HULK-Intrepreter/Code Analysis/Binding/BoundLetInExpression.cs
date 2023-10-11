namespace HULK.CodeAnalysis.Binding
{
    internal class BoundLetInExpression : BoundExpression
    {
        public BoundLetInExpression(BoundExpression assignment, BoundExpression expression)
        {
            Assignment = assignment;
            Expression = expression;
        }

        public BoundExpression Assignment { get; }
        public BoundExpression Expression { get; }

        public override Type Type => Expression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.LetInExpression;
    }
}