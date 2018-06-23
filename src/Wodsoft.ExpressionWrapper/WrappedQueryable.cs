using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public class WrappedQueryable<T, M> : IQueryable<T>, IWrappedQueryable
        where M : T
    {
        public WrappedQueryable(WrappedQueryableProvider<T, M> provider, Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Type ElementType { get { return typeof(T); } }

        public Expression Expression { get; private set; }

        public WrappedQueryableProvider<T, M> Provider { get; private set; }

        System.Linq.IQueryProvider IQueryable.Provider { get { return Provider; } }

        public IEnumerator<T> GetEnumerator()
        {
            WrapperVisitor visitor = new WrapperVisitor(Provider.Context);
            return new WrappedEnumerator<T, M>(Provider.InnerQueryProvider.CreateQuery<M>(visitor.Visit(Expression)).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            WrapperVisitor visitor = new WrapperVisitor(Provider.Context);
            return new WrappedEnumerator<T, M>(Provider.InnerQueryProvider.CreateQuery<M>(visitor.Visit(Expression)).GetEnumerator());
        }
    }

    public class WrappedOrderedQueryable<T, M> : WrappedQueryable<T, M>, IOrderedQueryable<T>
        where M : T
    {
        public WrappedOrderedQueryable(WrappedQueryableProvider<T, M> provider, Expression expression)
            : base(provider, expression)
        {
        }
    }

    internal interface IWrappedQueryable { }
}
