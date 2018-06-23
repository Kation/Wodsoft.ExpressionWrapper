using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Wrap<T, M>(this IQueryable<M> queryable, WrapperContext context = null)
            where M : T
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            if (context == null)
                context = new WrapperContext();
            WrappedQueryableProvider<T, M> provider = new WrappedQueryableProvider<T, M>(queryable.Provider, context);
            return provider.CreateQuery<T>(queryable.Expression);
        }

        public static object Wrap(this object value)
        {
            throw new NotSupportedException("不支持直接使用该方法。");
        }

        public static IQueryable<M> Unwrap<T, M>(this IQueryable<T> queryable, WrapperContext context = null)
            where M : T
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            WrappedQueryable<T, M> wrapped = queryable as WrappedQueryable<T, M>;
            if (wrapped == null)
                throw new NotSupportedException("不支持的类型。");
            WrappedQueryableProvider<T, M> provider = wrapped.Provider;
            var visitor = new WrapperVisitor(context ?? provider.Context);
            var expression = visitor.Visit(wrapped.Expression);
            return provider.InnerQueryProvider.CreateQuery<M>(expression);
        }

        public static bool IsWrapped(this IQueryable queryable)
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            return queryable is IWrappedQueryable;
        }

        public static IQueryable<T> Wrap<T>(this IQueryable<T> queryable, WrapperContext context)
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            var visitor = new WrapperVisitor(context);
            return queryable.Provider.CreateQuery<T>(visitor.Visit(queryable.Expression));
        }
    }
}
