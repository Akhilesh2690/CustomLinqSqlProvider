using System.Linq.Expressions;
using System.Text;

public class ExpressionToSqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _sqlBuilder = new StringBuilder();
    private readonly string _tableName;

    public ExpressionToSqlTranslator(string tableName)
    {
        _tableName = tableName;
    }

    public string Translate(Expression expression)
    {
        Visit(expression);
        return _sqlBuilder.ToString();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.Name == "Where")
        {
            var predicate = node.Arguments[1];

            if (predicate is LambdaExpression lambdaExpression)
            {
                _sqlBuilder.Append("SELECT * FROM ");
                _sqlBuilder.Append($"[{_tableName}] ");
                _sqlBuilder.Append("WHERE ");
                VisitExpression(lambdaExpression.Body);
            }
            else if (predicate is UnaryExpression unaryExpression && unaryExpression.Operand is LambdaExpression nestedLambda)
            {
                _sqlBuilder.Append("SELECT * FROM ");
                _sqlBuilder.Append($"[{_tableName}] ");
                _sqlBuilder.Append("WHERE ");
                VisitExpression(nestedLambda.Body);
            }
            else
            {
                throw new NotSupportedException($"Predicate of 'Where' is not a valid lambda expression.");
            }
        }
        else
        {
            throw new NotSupportedException($"Method '{node.Method.Name}' is not supported.");
        }

        return base.VisitMethodCall(node);
    }

    private void VisitExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                HandleBinaryExpression(binaryExpression);
                break;

            case MemberExpression memberExpression:
                HandleMemberExpression(memberExpression);
                break;

            case ConstantExpression constantExpression:
                HandleConstantExpression(constantExpression);
                break;

            case UnaryExpression unaryExpression:
                HandleUnaryExpression(unaryExpression);
                break;

            case LambdaExpression lambdaExpression:
                // For lambda expressions, we only need to visit the body
                VisitExpression(lambdaExpression.Body);
                break;

            default:
                throw new NotSupportedException($"Expression type {expression.GetType()} is not supported.");
        }
    }

    // Handle binary expressions like p.UnitPrice > 100
    private void HandleBinaryExpression(BinaryExpression node)
    {
        VisitExpression(node.Left);

        switch (node.NodeType)
        {
            case ExpressionType.GreaterThan:
                _sqlBuilder.Append(" > ");
                break;

            case ExpressionType.LessThan:
                _sqlBuilder.Append(" < ");
                break;

            case ExpressionType.Equal:
                _sqlBuilder.Append(" = ");
                break;

            case ExpressionType.AndAlso:
                _sqlBuilder.Append(" AND ");
                break;
            // Add other operators if needed (e.g., !=, >=, <=)
            default:
                throw new NotSupportedException($"Binary operator {node.NodeType} is not supported.");
        }

        VisitExpression(node.Right);
    }

    // Handle member expressions like p.UnitPrice
    private void HandleMemberExpression(MemberExpression node)
    {
        _sqlBuilder.Append($"[{node.Member.Name}]");
    }

    // Handle constant expressions (like numbers or strings)
    private void HandleConstantExpression(ConstantExpression node)
    {
        if (node.Value is string)
        {
            _sqlBuilder.Append($"'{node.Value}'"); // For string values, wrap in quotes
        }
        else
        {
            _sqlBuilder.Append(node.Value); // For numbers, no quotes
        }
    }

    // Handle unary expressions like !p.IsActive
    private void HandleUnaryExpression(UnaryExpression node)
    {
        switch (node.NodeType)
        {
            case ExpressionType.Not:
                _sqlBuilder.Append("NOT ");
                VisitExpression(node.Operand);
                break;

            case ExpressionType.Negate:
            case ExpressionType.NegateChecked:
                _sqlBuilder.Append("-");
                VisitExpression(node.Operand);
                break;

            default:
                throw new NotSupportedException($"Unary operator '{node.NodeType}' is not supported.");
        }
    }
}