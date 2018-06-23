using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Wodsoft.ExpressionWrapper
{
    public class WrapperVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo _WrapMethod = typeof(QueryableExtensions).GetRuntimeMethod("Wrap", new Type[] { typeof(object) });

        public WrapperVisitor(WrapperContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _Parameters = new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public WrapperContext Context { get; private set; }
        
        protected override Expression VisitLambda<F>(Expression<F> node)
        {
            var lambdaType = typeof(F);
            if (Context.Get<F>(out var type))
                return Expression.Lambda(type, Visit(node.Body), node.Parameters.Select(t => (ParameterExpression)VisitParameter(t)).ToArray());
            if (lambdaType.IsConstructedGenericType)
            {
                var definition = lambdaType.GenericTypeArguments;
                for (int i = 0; i < definition.Length; i++)
                {
                    if (Context.Get(definition[i], out Type m))
                        definition[i] = m;
                }
                lambdaType = lambdaType.GetGenericTypeDefinition().MakeGenericType(definition);
                return Expression.Lambda(lambdaType, Visit(node.Body), node.Parameters.Select(t => (ParameterExpression)VisitParameter(t)).ToArray());
            }
            return base.VisitLambda<F>(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method == _WrapMethod)
            {
                Expression expression = node.Arguments[0];
                if (expression.NodeType == ExpressionType.Convert)
                    expression = ((UnaryExpression)expression).Operand;
                return Visit(expression);
            }
            else if (Context.Get(node.Method, out var method))
            {
                if (node.Object == null)
                    return Expression.Call(method, node.Arguments.Select(t => Visit(t)));
                else
                    return Expression.Call(Visit(node.Object), method, node.Arguments.Select(t => Visit(t)));
            }
            else if (node.Method.IsGenericMethod)
            {
                var args = node.Method.GetGenericArguments();
                for (int i = 0; i < args.Length; i++)
                {
                    if (Context.Get(args[i], out var mt))
                        args[i] = mt;
                }
                method = node.Method.GetGenericMethodDefinition();
                if (Context.Get(method, out var m))
                    method = m;
                method = method.MakeGenericMethod(args);
                if (node.Object == null)
                    return Expression.Call(method, node.Arguments.Select(t => Visit(t)));
                else
                    return Expression.Call(Visit(node.Object), method, node.Arguments.Select(t => Visit(t)));
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (Context.Get(node.Member, out var m))
                return Expression.MakeMemberAccess(Visit(node.Expression), m);
            else if (node.Expression is ConstantExpression && node.Expression.Type.Name.Contains("<>"))
            {
                var value = Expression.Lambda<Func<object>>(Expression.Convert(node, typeof(object))).Compile()();
                return Expression.Constant(value);
            }
            else if (node.Expression is MemberExpression && ((MemberExpression)node.Expression).Member.DeclaringType.Name.Contains("<>"))
            {
                var value = Expression.Lambda<Func<object>>(Expression.Convert(node, typeof(object))).Compile()();
                return Expression.Constant(value);
            }
            return base.VisitMember(node);
        }

        private Dictionary<ParameterExpression, ParameterExpression> _Parameters;
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (Context.Get(node.Type, out var m))
            {
                if (!_Parameters.TryGetValue(node, out var parameter))
                {
                    parameter = Expression.Parameter(m, node.Name);
                    _Parameters.Add(node, parameter);
                }
                return parameter;
            }
            return base.VisitParameter(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null && node.Type != node.Value.GetType())
                return Expression.Constant(node.Value);
            return base.VisitConstant(node);
        }

        //protected override Expression VisitBinary(BinaryExpression node)
        //{
        //    if (node.NodeType == ExpressionType.Equal)
        //    {
        //        Expression left = Visit(node.Left);
        //        Expression right = Visit(node.Right);
        //        return Expression.Equal(left, right);
        //    }
        //    else if (node.NodeType == ExpressionType.NotEqual)
        //    {
        //        Expression left = Visit(node.Left);
        //        Expression right = Visit(node.Right);
        //        return Expression.NotEqual(left, right);
        //    }
        //    return base.VisitBinary(node);
        //}
    }
}
