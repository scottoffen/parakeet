using System;
using System.Collections.Generic;
using System.Data;

namespace Parakeet
{
    public static class TypeExtensions
    {
        internal static List<Type> _types;

        static TypeExtensions()
        {
            _types = new List<Type>
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
        }

        internal static bool IsParakeetEligible(this Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return IsParakeetEligible(Nullable.GetUnderlyingType(type));
            }

            if (type.IsPrimitive) return true;
            if (_types.Contains(type) || type.IsEnum) return true;
            return false;
        }
    }
}