// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFPocoAdapter.Internal;

namespace EFPocoAdapter
{
    #region FuncAdapter<>

    internal class FuncAdapter<TArg0, TResult> where TArg0 : ObjectContext
    {
        private Func<TArg0, TResult> _func;
        public FuncAdapter(Func<TArg0, TResult> func) { _func = func; }
        public TResult Execute(EFPocoContext context) { return _func((TArg0)context.PersistenceAwareContext); }
    }
    internal class FuncAdapter<TArg0, TArg1, TResult> where TArg0 : ObjectContext
    {
        private Func<TArg0, TArg1, TResult> _func;
        public FuncAdapter(Func<TArg0, TArg1, TResult> func) { _func = func; }
        public TResult Execute(EFPocoContext context, TArg1 arg1) { return _func((TArg0)context.PersistenceAwareContext, arg1); }
    }
    internal class FuncAdapter<TArg0, TArg1, TArg2, TResult> where TArg0 : ObjectContext
    {
        private Func<TArg0, TArg1, TArg2, TResult> _func;
        public FuncAdapter(Func<TArg0, TArg1, TArg2, TResult> func) { _func = func; }
        public TResult Execute(EFPocoContext context, TArg1 arg1, TArg2 arg2) { return _func((TArg0)context.PersistenceAwareContext, arg1, arg2); }
    }
    internal class FuncAdapter<TArg0, TArg1, TArg2, TArg3, TResult> where TArg0 : ObjectContext
    {
        private Func<TArg0, TArg1, TArg2, TArg3, TResult> _func;
        public FuncAdapter(Func<TArg0, TArg1, TArg2, TArg3, TResult> func) { _func = func; }
        public TResult Execute(EFPocoContext context, TArg1 arg1, TArg2 arg2, TArg3 arg3) { return _func((TArg0)context.PersistenceAwareContext, arg1, arg2, arg3); }
    }

    #endregion

    #region Helper

    static class Helper
    {
        public static void CompileEvaluatorMethod<TResult,TDelegateType>(QueryTranslator translator, LambdaExpression expression, Type funcTypeDefinition, Type funcAdapterTypeDefinition, out TDelegateType evaluator, out Func<EFPocoContext, object, TResult> adapter)
        {
            int numArguments = funcTypeDefinition.GetGenericArguments().Length;

            // translate query 
            Expression translatedExpressionBody = translator.Translate(expression.Body, out adapter);
            ParameterExpression[] translatedParameters = expression.Parameters.Select(c => translator.GetTranslatedParameter(c)).ToArray();
            Type[] funcTypeArguments = new Type[translatedParameters.Length + 1];
            funcTypeArguments[0] = translator.PersistenceAwareContextType;
            for (int i = 1; i < expression.Parameters.Count; ++i)
                funcTypeArguments[i] = expression.Parameters[i].Type;
            funcTypeArguments[funcTypeArguments.Length - 1] = typeof(object);
            Type funcType = funcTypeDefinition.MakeGenericType(funcTypeArguments);
            LambdaExpression lambda = Expression.Lambda(funcType, translatedExpressionBody, translatedParameters);

            MethodInfo compileMethodInfo = typeof(CompiledQuery).GetMethods().Where(c => c.Name == "Compile" && c.GetGenericArguments().Length == numArguments).Single();
            Delegate compiledDelegate = (Delegate)compileMethodInfo.MakeGenericMethod(funcTypeArguments).Invoke(null, new object[] { lambda });
            Type funcAdapterType = funcAdapterTypeDefinition.MakeGenericType(funcTypeArguments);
            object funcAdapterObject = Activator.CreateInstance(funcAdapterType, compiledDelegate);
            evaluator = (TDelegateType)(object)Delegate.CreateDelegate(typeof(TDelegateType), funcAdapterObject, "Execute");
        }
    }

    #endregion

    #region EFPocoCompiledQuery<TArg0, TResult>

    internal class EFPocoCompiledQuery<TArg0, TResult>
        where TArg0 : EFPocoContext
    {
        private LambdaExpression _expression;
        private Func<EFPocoContext, object, TResult> _adapter;
        private Func<EFPocoContext, object> _evaluator;

        public EFPocoCompiledQuery(LambdaExpression expression)
        {
            _expression = expression;
        }

        public TResult Execute(TArg0 context)
        {
            if (_evaluator == null)
            {
                Helper.CompileEvaluatorMethod(context.CreateUnboundQueryTranslator(), _expression,
                    typeof(Func<,>), typeof(FuncAdapter<,>), 
                    out _evaluator, out _adapter);
            }
            return _adapter(context, _evaluator(context));
        }
    }

    #endregion

    #region EFPocoCompiledQuery<TArg0, TArg1, TResult>

    internal class EFPocoCompiledQuery<TArg0, TArg1, TResult>
        where TArg0 : EFPocoContext
    {
        private LambdaExpression _expression;
        private Func<EFPocoContext, object, TResult> _adapter;
        private Func<EFPocoContext, TArg1, object> _evaluator;

        public EFPocoCompiledQuery(LambdaExpression expression)
        {
            _expression = expression;
        }

        public TResult Execute(TArg0 context, TArg1 arg1)
        {
            if (_evaluator == null)
            {
                Helper.CompileEvaluatorMethod(context.CreateUnboundQueryTranslator(), _expression,
                    typeof(Func<,,>), typeof(FuncAdapter<,,>),
                    out _evaluator, out _adapter);
            }
            return _adapter(context, _evaluator(context, arg1));
        }
    }

    #endregion

    #region EFPocoCompiledQuery<TArg0, TArg1, TArg2, TResult>

    internal class EFPocoCompiledQuery<TArg0, TArg1, TArg2, TResult>
        where TArg0 : EFPocoContext
    {
        private LambdaExpression _expression;
        private Func<EFPocoContext, object, TResult> _adapter;
        private Func<EFPocoContext, TArg1, TArg2, object> _evaluator;

        public EFPocoCompiledQuery(LambdaExpression expression)
        {
            _expression = expression;
        }

        public TResult Execute(TArg0 context, TArg1 arg1, TArg2 arg2)
        {
            if (_evaluator == null)
            {
                Helper.CompileEvaluatorMethod(context.CreateUnboundQueryTranslator(), _expression,
                    typeof(Func<,,,>), typeof(FuncAdapter<,,,>),
                    out _evaluator, out _adapter);
            }
            return _adapter(context, _evaluator(context, arg1, arg2));
        }
    }

    #endregion

    #region EFPocoCompiledQuery<TArg0, TArg1, TArg2, TArg3, TResult>

    internal class EFPocoCompiledQuery<TArg0, TArg1, TArg2, TArg3, TResult>
        where TArg0 : EFPocoContext
    {
        private LambdaExpression _expression;
        private Func<EFPocoContext, object, TResult> _adapter;
        private Func<EFPocoContext, TArg1, TArg2, TArg3, object> _evaluator;

        public EFPocoCompiledQuery(LambdaExpression expression)
        {
            _expression = expression;
        }

        public TResult Execute(TArg0 context, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (_evaluator == null)
            {
                Helper.CompileEvaluatorMethod(context.CreateUnboundQueryTranslator(), _expression,
                    typeof(Func<,,,,>), typeof(FuncAdapter<,,,,>),
                    out _evaluator, out _adapter);
            }
            return _adapter(context, _evaluator(context, arg1, arg2, arg3));
        }
    }

    #endregion

    #region EFPocoCompiledQuery

    public static class EFPocoCompiledQuery
    {
        public static Func<TArg0,TResult> Compile<TArg0,TResult>(Expression<Func<TArg0,TResult>> query)
            where TArg0 : EFPocoContext
        {
            return new EFPocoCompiledQuery<TArg0, TResult>(query).Execute;
        }

        public static Func<TArg0, TArg1, TResult> Compile<TArg0, TArg1, TResult>(Expression<Func<TArg0, TArg1, TResult>> query)
            where TArg0 : EFPocoContext
        {
            return new EFPocoCompiledQuery<TArg0, TArg1, TResult>(query).Execute;
        }

        public static Func<TArg0, TArg1, TArg2, TResult> Compile<TArg0, TArg1, TArg2, TResult>(Expression<Func<TArg0, TArg1, TArg2, TResult>> query)
            where TArg0 : EFPocoContext
        {
            return new EFPocoCompiledQuery<TArg0, TArg1, TArg2, TResult>(query).Execute;
        }

        public static Func<TArg0, TArg1, TArg2, TArg3, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TResult>(Expression<Func<TArg0, TArg1, TArg2, TArg3, TResult>> query)
            where TArg0 : EFPocoContext
        {
            return new EFPocoCompiledQuery<TArg0, TArg1, TArg2, TArg3, TResult>(query).Execute;
        }
    }

    #endregion
}
