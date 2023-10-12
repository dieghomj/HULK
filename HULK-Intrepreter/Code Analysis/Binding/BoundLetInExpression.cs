namespace HULK.CodeAnalysis.Binding
{
    internal class BoundLetInExpression : BoundExpression
    {
        public BoundLetInExpression(List<BoundExpression> assignments, BoundExpression expression)
        {
            Assignments = assignments;
            Expression = expression;
        }

        public List<BoundExpression> Assignments { get; }
        public BoundExpression Expression { get; }

        public override Type Type => Expression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.LetInExpression;
    }
}