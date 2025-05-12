using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoSuchStudio.Common
{
    /// <summary>
    /// This class contains extension methods on <see cref="object"/>,
    /// <see cref="ICollection{T}"/>, and
    /// <see cref="KeyValuePair{TKey, TValue}"/> for converting
    /// <see cref="ICollection{T}"/> and
    /// <see cref="KeyValuePair{TKey, TValue}"/> objects to string
    /// representations (stringification) in a way that is readable.
    /// </summary>
    public static class ReflectionToStringExts
    {
        /// <summary>
        /// A flag to control whether the type name is included in the
        /// string representation.
        /// </summary>
        public static bool verbose = false;

        /// <summary>
        /// Enable conversion of <see cref="ICollection{T}"/>s to string
        /// representations.
        /// Uses
        /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/linq/">
        /// Linq</see> to iterate through the
        /// <see cref="ICollection{T}"/> and convert each item to a
        /// string.
        /// Supports nested <see cref="ICollection{T}"/>s and
        /// <see cref="KeyValuePair{TKey, TValue}"/>s.
        /// </summary>
        /// <typeparam name="T">The <see cref="ICollection{T}"/> item type.</typeparam>
        /// <param name="obj">
        /// The <see cref="ICollection{T}"/> to stringify.
        /// </param>
        /// <param name="d">
        /// Recursive depth indicator for nested <see cref="ICollection{T}"/>s
        /// and <see cref="KeyValuePair{TKey, TValue}"/>s.
        /// </param>
        /// <returns>
        /// The string with the previous or no printed objects present,
        /// plus the stringification of argument <paramref name="obj"/>.
        /// </returns>
        public static string ToStringExtCollection<T>(this ICollection<T> obj, int d = 0)
        {
            return (verbose ? obj.GetType().Name + "<" + typeof(T).Name + ">" : "") + "{" + string.Join(", ", obj.Select(o => o.ToStringExt(d + 1))) + "}";
        }

        /// <summary>
        /// Enable conversion of
        /// <see cref="KeyValuePair{TKey, TValue}"/>s to string
        /// representations.
        /// Runs the key and the value through
        /// <see cref="ToStringExt(object, int)"/> to handle nested
        /// <see cref="KeyValuePair{TKey, TValue}"/>s, meaning the
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> could be a
        /// <see cref="KeyValuePair{TKey, TValue}"/>, and the
        /// <see cref="KeyValuePair{TKey, TValue}.Value"/> could also
        /// be a <see cref="KeyValuePair{TKey, TValue}"/>, and this
        /// will work to any depth, unless the CLR function call stack
        /// limit is reached (Supports boundless nested
        /// <see cref="KeyValuePair{TKey, TValue}"/>s).
        /// </summary>
        /// <typeparam name="U">The <see cref="KeyValuePair{TKey, TValue}"/> Key type.</typeparam>
        /// <typeparam name="V">The <see cref="KeyValuePair{TKey, TValue}"/> Value type.</typeparam>
        /// <param name="kvp">
        /// The <see cref="KeyValuePair{TKey, TValue}"/> to stringify.
        /// </param>
        /// <param name="d">
        /// Recursive depth indicator for nested <see cref="ICollection{T}"/>s
        /// and <see cref="KeyValuePair{TKey, TValue}"/>s.
        /// </param>
        /// <returns>
        /// The string with the previous or no printed objects present,
        /// plus the stringification of argument <paramref name="kvp"/>.
        /// </returns>
        public static string ToStringExtKeyValuePair<U, V>(this KeyValuePair<U, V> kvp, int d = 0)
        {
            return (verbose ? "KeyValuePair<" + typeof(U).Name + "," + typeof(V).Name  + ">" : "") +
                "(" + kvp.Key.ToStringExt(d + 1) +
                " => " +
                kvp.Value.ToStringExt(d + 1) + ")";
        }

        /// <summary>
        /// An extension method to convert an object to a string, that
        /// gracefully handles null, <see cref="ICollection{T}"/>s and
        /// <see cref="KeyValuePair{TKey, TValue}"/>s, using the
        /// appropriate extension method
        /// <see cref="ToStringExtCollection{T}(ICollection{T}, int)"/>
        /// or
        /// <see cref="ToStringExtKeyValuePair{U, V}(KeyValuePair{U, V}, int)"/>.
        /// Useful if you don't know the type of the object you're
        /// wanting to stringify.
        /// </summary>
        /// <param name="obj">The object to stringify.</param>
        /// <param name="d">Recursive depth indicator for nested <see cref="ICollection{T}"/>s and <see cref="KeyValuePair{TKey, TValue}"/>s.</param>
        /// <returns>Stringified <paramref name="obj"/>.</returns>
        public static string ToStringExt(this object obj, int d = 0)
        {
            if (obj == null)
            {
                return "null";
            }

            string res = null;

            Type TType = obj.GetType();
            if (TType.IsGenericType
                && (TType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
            {
                Type kvpType = typeof(KeyValuePair<,>).MakeGenericType(TType.GetGenericArguments()[0], TType.GetGenericArguments()[1]);
                res = ReflectionToStringExts.ToStringExtKeyValuePair(obj.CastToReflected(kvpType), d + 1);
            }
            else
            {
                foreach (var IType in TType.GetInterfaces()) 
                {
                    if (IType.IsGenericType
                        && (IType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    {
                        res = ReflectionToStringExts.ToStringExtCollection(obj.CastToReflected(IType), d + 1);
                        break;
                    }
                }
            }

            if (res == null)
            {
                res = obj.ToString();
            }

            return res;
        }

        /// <summary>
        /// Casts an arbitrary object to the generically-specified type
        /// using the C-style cast operator.
        /// </summary>
        /// <typeparam name="T">The type to cast the object to.</typeparam>
        /// <param name="o">The object to cast.</param>
        /// <returns>The object <paramref name="o"/> as a <typeparamref name="T"/>.</returns>
        public static T CastTo<T>(this object o) => (T)o;

        /// <summary>
        /// Calls <see cref="CastTo{T}(object)"/> with the specified type, non-generically.
        /// </summary>
        /// <param name="o">The object to cast.</param>
        /// <param name="type">The type to cast the object to.</param>
        /// <returns>The object <paramref name="o"/> as the type represented by <paramref name="type"/></returns>
        public static dynamic CastToReflected(this object o, Type type)
        {
            var methodInfo = typeof(ReflectionToStringExts).GetMethod(nameof(CastTo), BindingFlags.Static | BindingFlags.Public);
            var genericArguments = new[] { type };
            var genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);

            return genericMethodInfo?.Invoke(null, new[] { o });
        }
    }
}
