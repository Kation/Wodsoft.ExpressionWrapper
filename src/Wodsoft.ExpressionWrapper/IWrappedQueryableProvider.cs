using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public interface IWrappedQueryableProvider : IQueryProvider
    {
        WrapperContext Context { get; }

        IQueryProvider InnerQueryProvider { get; }
    }
}
