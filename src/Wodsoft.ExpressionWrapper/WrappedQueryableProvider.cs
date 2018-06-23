using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Wodsoft.ExpressionWrapper
{
    public class WrappedQueryableProvider<T, M> : IWrappedQueryableProvider
        where M : T
    {
        public WrappedQueryableProvider(IQueryProvider queryProvider, WrapperContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            InnerQueryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public WrapperContext Context { get; private set; }

        public IQueryProvider InnerQueryProvider { get; private set; }

        public IQueryable CreateQuery(Expression expression)
        {
            if (typeof(IOrderedQueryable).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo()))
                return new WrappedOrderedQueryable<T, M>(this, expression);
            else
                return new WrappedQueryable<T, M>(this, expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(TElement) != typeof(T))
                throw new NotSupportedException("不支持的元素类型。");
            if (typeof(IOrderedQueryable).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo()))
                return (IQueryable<TElement>)new WrappedOrderedQueryable<T, M>(this, expression);
            else
                return (IQueryable<TElement>)new WrappedQueryable<T, M>(this, expression);
        }

        public object Execute(Expression expression)
        {
            WrapperVisitor visitor = new WrapperVisitor(Context);
            return InnerQueryProvider.Execute(visitor.Visit(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            WrapperVisitor visitor = new WrapperVisitor(Context);
            return InnerQueryProvider.Execute<TResult>(visitor.Visit(expression));
        }
    }
}
