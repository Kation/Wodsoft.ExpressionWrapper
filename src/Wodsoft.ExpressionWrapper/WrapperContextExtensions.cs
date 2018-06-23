using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public static class WrapperContextExtensions
    {
        public static void Set<T, M>(this WrapperContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            context.Set(typeof(T), typeof(M));
        }

        public static void Set<T, P>(this WrapperContext context, Expression<Func<T, P>> target, Expression<Func<T, P>> mapped)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (mapped == null)
                throw new ArgumentNullException(nameof(mapped));
            if (target.Body.NodeType != ExpressionType.MemberAccess || ((MemberExpression)target.Body).Expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("目标只能为类型成员。");
            if (mapped.Body.NodeType != ExpressionType.MemberAccess || ((MemberExpression)mapped.Body).Expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("映射只能为类型成员。");
            context.Set(((MemberExpression)target.Body).Member, ((MemberExpression)mapped.Body).Member);
        }

        public static void Set<T, M, P>(this WrapperContext context, Expression<Func<T, P>> target, Expression<Func<M, P>> mapped)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (mapped == null)
                throw new ArgumentNullException(nameof(mapped));
            if (target.Body.NodeType != ExpressionType.MemberAccess || ((MemberExpression)target.Body).Expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("目标只能为类型成员。");
            if (mapped.Body.NodeType != ExpressionType.MemberAccess || ((MemberExpression)mapped.Body).Expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("映射只能为类型成员。");
            context.Set(((MemberExpression)target.Body).Member, ((MemberExpression)mapped.Body).Member);
        }

        public static void Set<T>(this WrapperContext context, string targetMethod, string mappedMethod)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var target = typeof(T).GetMethod(targetMethod);
            if (target == null)
                throw new ArgumentException(nameof(targetMethod), "方法不存在。");
            var mapped = typeof(T).GetMethod(mappedMethod);
            if (mapped == null)
                throw new ArgumentException(nameof(mappedMethod), "方法不存在。");
            context.Set(target, mapped);
        }

        public static void Set<T, M>(this WrapperContext context, string targetMethod, string mappedMethod)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var target = typeof(T).GetMethod(targetMethod);
            if (target == null)
                throw new ArgumentException(nameof(targetMethod), "方法不存在。");
            var mapped = typeof(M).GetMethod(mappedMethod);
            if (mapped == null)
                throw new ArgumentException(nameof(mappedMethod), "方法不存在。");
            context.Set(target, mapped);
        }


        public static bool Get<T>(this WrapperContext context, out Type mapped)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            return context.Get(typeof(T), out mapped);
        }
    }
}
