using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public class WrappedEnumerator<T, M> : IEnumerator<T>
            where M : T
    {
        public WrappedEnumerator(IEnumerator<M> enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));
            InnerEnumerator = enumerator;
        }

        public IEnumerator<M> InnerEnumerator { get; private set; }

        public T Current { get { return InnerEnumerator.Current; } }

        object IEnumerator.Current { get { return InnerEnumerator.Current; } }
        public void Dispose()
        {
            InnerEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return InnerEnumerator.MoveNext();
        }

        public void Reset()
        {
            InnerEnumerator.Reset();
        }
    }
}
