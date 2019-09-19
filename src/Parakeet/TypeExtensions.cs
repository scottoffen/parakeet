using System;
using System.Collections.Generic;
using System.Data;

namespace Parakeet
{
    public static class TypeExtensions
    {
        internal static List<Type> _types = new List<Type>
        {
            typeof(String),
            typeof(DataTable),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(TimeZoneInfo),
            typeof(decimal),
            typeof(Guid)
        };

        internal static bool IsParakeetEligible(this Type type)
        {
            if (type.IsPrimitive || type.IsEnum || _types.Contains(type)) return true;

            // Check for IEnumerable<T> where T : IDataRecord
            if (typeof(IEnumerable<IDataRecord>).IsAssignableFrom(type)) return true;

            // Check for nullable types
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return IsParakeetEligible(Nullable.GetUnderlyingType(type));
            }

            return false;
        }
    }
}