using CustomLinqSqlProvider.Helper;
using CustomLinqSqlProvider.Services;
using System.Linq.Expressions;

namespace CustomLinqSqlProvider.QueryProvider
{
    public class SqlQueryProvider : IQueryProvider
    {
        private readonly SqlQueryService _sqlClient;

        public SqlQueryProvider(SqlQueryService client)
        {
            _sqlClient = client;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new SqlQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Type itemType = TypeHelper.GetElementType(expression.Type);
            var translator = new ExpressionToSqlTranslator(itemType.Name);
            string queryString = translator.Translate(expression);
            return (TResult)_sqlClient.ExecuteSqlQuery(queryString);
        }
    }
}