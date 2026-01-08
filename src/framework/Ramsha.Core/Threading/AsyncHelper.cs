using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Nito.AsyncEx;

namespace Ramsha
{
    public static class AsyncHelper
    {

        public static bool IsAsync([NotNull] this MethodInfo method)
        {

            return method.ReturnType.IsTaskOrTaskOfT();
        }

        public static bool IsTaskOrTaskOfT([NotNull] this Type type)
        {
            return type == typeof(Task) || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
        }

        public static bool IsTaskOfT([NotNull] this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }


        public static Type UnwrapTask([NotNull] Type type)
        {
            if (type == typeof(Task))
            {
                return typeof(void);
            }

            if (type.IsTaskOfT())
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }


        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}