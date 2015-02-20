using System;

namespace dotless.Core.Utils
{
    public static class ObjectExtensions 
    {
        /// <summary>
        /// Helper extension for chaining intermediate actions into expressions. 
        /// Invokes <paramref name="action" /> on <paramref name="obj"/> and then returns obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Do<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}