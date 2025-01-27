using System.Collections;
using System.Linq.Expressions;

namespace CustomLinqSqlProvider.QueryProvider
{
    public class SqlQuery<T> : IQueryable<T>
    {
        private readonly SqlQueryProvider _provider;

        public SqlQuery(Expression expression, SqlQueryProvider provider)
        {
            Expression = expression;
            _provider = provider;
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }
        public IQueryProvider Provider => _provider;

        public IEnumerator<T> GetEnumerator()
        {
            return _provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }
    }
}