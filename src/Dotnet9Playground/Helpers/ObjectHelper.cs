using System;
using System.Reflection;

namespace Dotnet9Playground.Helpers
{
    internal static class ObjectHelper
    {
        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        internal static object Field(this object obj, string fieldName, BindingFlags flags)
        {
            var type = obj.GetType();
            var fieldInfo = type.GetField(fieldName, flags);
            return fieldInfo?.GetValue(obj);
        }

        internal static void ExecuteVoid(this object obj, string methodName, BindingFlags flags, params object[] args)
        {
            var type = obj.GetType();
            var methodInfo = type.GetMethod(methodName, flags);
            methodInfo?.Invoke(obj, args);
        }

        internal static T ExecuteWithReturn<T>(this object obj, string methodName, BindingFlags flags,
            params object[] args)
        {
            var type = obj.GetType();
            var methodInfo = type.GetMethod(methodName, flags);
            var result = methodInfo?.Invoke(obj, args);
            return (T)result;
        }

        internal static void Execute(Type staticClassName, string methodName, BindingFlags flags, params object[] args)
        {
            var methodInfo = staticClassName.GetMethod(methodName, flags);
            methodInfo?.Invoke(null, args);
        }
    }
}