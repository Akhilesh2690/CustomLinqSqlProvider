using CustomLinqSqlProvider.QueryProvider;
using CustomLinqSqlProvider.Services;
using System.Collections;
using System.Linq.Expressions;

namespace CustomLinqSqlProvider.Models.Entities
{
    public class SqlQueryable<T> : IQueryable<T>
    {
        protected readonly Expression Expr;
        protected readonly IQueryProvider QueryProvider;

        public SqlQueryable(SqlQueryService client)
        {
            Expr = Expression.Constant(this);
            QueryProvider = new SqlQueryProvider(client);
        }

        public Type ElementType => typeof(T);
        public Expression Expression => Expr;
        public IQueryProvider Provider => QueryProvider;

        public IEnumerator<T> GetEnumerator()
        {
            return QueryProvider.Execute<IEnumerable<T>>(Expr).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return QueryProvider.Execute<IEnumerable>(Expr).GetEnumerator();
        }
    }
}