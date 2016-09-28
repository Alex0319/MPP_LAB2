using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace WeakDelegate
{
    public class DelegateFactory
    {
        private MethodInfo weakMethod;
        private WeakReference weakTarget;

        public Delegate CreateDelegate(MethodInfo weakMethod, WeakReference weakTarget)
        {
            this.weakMethod = weakMethod;
            this.weakTarget = weakTarget;
            return GetDelegate();
        }

        private Delegate GetDelegate()
        {
            if(weakMethod.ReturnType==typeof(void))
                return GetAction();
            else
                return GetFunc();
        }

        private Type GetDelegateType()
        {
            return Expression.GetDelegateType(weakMethod.GetParameters().Select(parameter => parameter.ParameterType).Concat(new[] { weakMethod.ReturnType}).ToArray());
        }

        private Delegate GetAction()
        {
            var argumentsType = GetArgumentsType();
            return Expression.Lambda(GetDelegateType(),GetBlockCall(argumentsType,CallAction(argumentsType)),argumentsType).Compile();
        }

        private ConditionalExpression GetBlockCall(ParameterExpression[] argumentsType,Expression[] callbackTarget)
        {
            return Expression.IfThen(Expression.IsTrue(GetCheckIsALive()), Expression.Block(GetVariables(argumentsType), callbackTarget));
        }

        private Delegate GetFunc()
        {
            var argumentsType = GetArgumentsType();
            var returnVariable = Expression.Variable(weakMethod.ReturnType);
            return Expression.Lambda(GetDelegateType(),Expression.Block(GetVariables(argumentsType).Concat(new [] {returnVariable}),
                GetBlockCall(argumentsType,CallFunc(argumentsType,returnVariable)),returnVariable),argumentsType).Compile();      
        }

        private Expression[] CallAction(ParameterExpression[] argumentsType)
        {
            return new Expression[] { CallDelegate(argumentsType) };
        }

        private Expression[] CallFunc(ParameterExpression[] argumentsType, ParameterExpression returnVariable)
        {
            return new Expression[] { Expression.Assign(returnVariable,CallDelegate(argumentsType))};
        }

        private MethodCallExpression CallDelegate(ParameterExpression[] argumentsType)
        {
            return Expression.Call(instance: Expression.Convert(GetTarget(), 
                weakMethod.DeclaringType), method: weakMethod, arguments: argumentsType);
        }

        private List<ParameterExpression> GetVariables(ParameterExpression[] argumentsType)
        {
            return new List<ParameterExpression>(argumentsType.Select(parameter=>Expression.Variable(parameter.Type)));
        }

        private MemberExpression GetTarget()
        {
            return Expression.Property(Expression.Convert(Expression.Constant(weakTarget),typeof(WeakReference)),"Target");
        }

        private MemberExpression GetCheckIsALive()
        {
            return Expression.Property(Expression.Convert(Expression.Constant(weakTarget),typeof(WeakReference)),"IsAlive"); 
        }

        private ParameterExpression[] GetArgumentsType()
        {
            return weakMethod.GetParameters().Select(parameter=>Expression.Parameter(parameter.GetType())).ToArray();
        }
    }
}
