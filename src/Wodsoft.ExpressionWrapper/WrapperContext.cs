using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public class WrapperContext
    {
        private ConcurrentDictionary<Type, Type> _TypeMapped;
        private ConcurrentDictionary<MethodInfo, MethodInfo> _MethodMapped;
        private ConcurrentDictionary<MemberInfo, MemberInfo> _MemberMapped;

        public WrapperContext()
        {
            _TypeMapped = new ConcurrentDictionary<Type, Type>();
            _MethodMapped = new ConcurrentDictionary<MethodInfo, MethodInfo>();
            _MemberMapped = new ConcurrentDictionary<MemberInfo, MemberInfo>();
        }
        
        public void Set(Type target, Type mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (mapped == null)
                throw new ArgumentNullException(nameof(mapped));
            _TypeMapped.AddOrUpdate(target, mapped, (o, c) => mapped);
        }

        public void Set(MethodInfo target, MethodInfo mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (mapped == null)
                throw new ArgumentNullException(nameof(mapped));
            _MethodMapped.AddOrUpdate(target, mapped, (o, c) => mapped);
        }

        public void Set(MemberInfo target, MemberInfo mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (mapped == null)
                throw new ArgumentNullException(nameof(mapped));
            _MemberMapped.AddOrUpdate(target, mapped, (o, c) => mapped);
        }

        public bool Get(Type target, out Type mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            return _TypeMapped.TryGetValue(target, out mapped);
        }

        public bool Get(MethodInfo target, out MethodInfo mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            return _MethodMapped.TryGetValue(target, out mapped);
        }

        public bool Get(MemberInfo target, out MemberInfo mapped)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            return _MemberMapped.TryGetValue(target, out mapped);
        }
    }
}
