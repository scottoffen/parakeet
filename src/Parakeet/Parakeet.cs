using Dapper;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Parakeet
{
    public static class Parakeet<T> where T : class
    {
        private static ConcurrentDictionary<string, Func<T, object>> PropertyGetterCache { get; } = new ConcurrentDictionary<string, Func<T, object>>();
        private static ConcurrentDictionary<string, ParakeetAttribute> AttributeCache { get; } = new ConcurrentDictionary<string, ParakeetAttribute>();

        static Parakeet()
        {
            var type = typeof(T);

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => (p.PropertyType.IsParakeetEligible() && !p.GetCustomAttributes<ParakeetIgnoreAttribute>().Any()) || p.GetCustomAttributes<ParakeetAttribute>().Any())
            .ToList();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes<ParakeetAttribute>().FirstOrDefault();
                if (attr == null) attr = new ParakeetAttribute { Name = property.Name };
                if (string.IsNullOrEmpty(attr.Name)) attr.Name = property.Name;

                var func = Parakeet.GenerateDelegate<T>(property.GetGetMethod());

                AttributeCache.AddOrUpdate(attr.Name, attr, (t, p) => attr);
                PropertyGetterCache.AddOrUpdate(attr.Name, func, (t, f) => func);
            }
        }

        public static DynamicParameters Generate(T obj, bool removeUnused = false)
        {
            var parameters = new DynamicParameters(obj);

            foreach (var name in PropertyGetterCache.Keys)
            {
                var value = PropertyGetterCache[name](obj);
                var attr = AttributeCache[name];

                if (value is DataTable dataTablePropertyValue)
                {
                    var tableName = !string.IsNullOrEmpty(attr.TableName) ? attr.TableName : dataTablePropertyValue.TableName;
                    parameters.Add(name, dataTablePropertyValue.AsTableValuedParameter(tableName), attr.DbType, attr.Direction, attr.Size, attr.Precision, attr.Scale);
                }
                else
                {
                    parameters.Add(name, value, attr.DbType, attr.Direction, attr.Size, attr.Precision, attr.Scale);
                }
            }

            parameters.RemoveUnused = removeUnused;
            return parameters;
        }
    }

    internal static class Parakeet
    {
        public static Func<T, object> GenerateDelegate<T>(MethodInfo method) where T : class
        {
            // First fetch the generic form
            MethodInfo genericHelper = typeof(Parakeet).GetMethod("GenerateDelegateHelper", BindingFlags.Static | BindingFlags.NonPublic);

            // Now supply the type arguments
            MethodInfo constructedHelper = genericHelper.MakeGenericMethod(typeof(T), method.ReturnType);

            // Now call it. The null argument is because it's a static method.
            object ret = constructedHelper.Invoke(null, new object[] { method });

            // Cast the result to the right kind of delegate and return it
            return (Func<T, object>)ret;
        }

        private static Func<TTarget, object> GenerateDelegateHelper<TTarget, TReturn>(MethodInfo method) where TTarget : class
        {
            // Convert the slow MethodInfo into a fast, strongly typed, open delegate
            Func<TTarget, TReturn> func = (Func<TTarget, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), method);

            // Now create a more weakly typed delegate which will call the strongly typed one
            Func<TTarget, object> ret = (TTarget target) => func(target);
            return ret;
        }

    }
}