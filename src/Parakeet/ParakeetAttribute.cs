using System;
using System.Data;
using System.Runtime.InteropServices;

namespace Parakeet
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParakeetAttribute : Attribute
    {
        public string Name { get; set; }

        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public DbType? DbType { get; set; } = null;

        public int? Size { get; set; } = null;

        public byte? Precision { get; set; } = null;

        public byte? Scale { get; set; } = null;

        public string TableName { get; set; }

        internal bool IsEnumerableDataRecord { get; set; } = false;

        internal bool IsDataTable { get; set; } = false;

        public ParakeetAttribute() { }

        public ParakeetAttribute(string propertyName = null, ParameterDirection direction = ParameterDirection.Input, string tableName = null, int size = -1, byte precision = 0, byte scale = 0)
        {
            Name = propertyName;
            Direction = direction;
            TableName = tableName;

            if (size > 0) Size = size;
            if (precision > 0) Precision = precision;
            if (scale > 0) Scale = scale;
        }

        public ParakeetAttribute(DbType dbType, string propertyName = null, ParameterDirection direction = ParameterDirection.Input, string tableName = null, int size = -1, byte precision = 0, byte scale = 0)
        {
            Name = propertyName;
            Direction = direction;
            DbType = dbType;
            TableName = tableName;

            if (size > 0) Size = size;
            if (precision > 0) Precision = precision;
            if (scale > 0) Scale = scale;
        }
    }
}
