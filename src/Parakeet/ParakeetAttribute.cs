using System;
using System.Data;

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

        public ParakeetAttribute(string name)
        {
            Name = name;
        }

        public ParakeetAttribute(ParameterDirection direction)
        {
            Direction = direction;
        }

        public ParakeetAttribute(DbType dbType)
        {
            DbType = dbType;
        }

        public ParakeetAttribute(string name, ParameterDirection direction)
        {
            Name = name;
            Direction = direction;
        }

        public ParakeetAttribute(string name, DbType dbType)
        {
            Name = name;
            DbType = dbType;
        }

        public ParakeetAttribute(ParameterDirection direction, DbType dbType)
        {
            Direction = direction;
            DbType = dbType;
        }

        public ParakeetAttribute(string name, ParameterDirection direction, DbType dbType)
        {
            Name = name;
            Direction = direction;
            DbType = dbType;
        }

        public ParakeetAttribute(string name, ParameterDirection direction, DbType dbType, int size)
        {
            Name = name;
            Direction = direction;
            DbType = dbType;
            Size = size;
        }

        public ParakeetAttribute(string name, ParameterDirection direction, DbType dbType, int size, byte precision)
        {
            Name = name;
            Direction = direction;
            DbType = dbType;
            Size = size;
            Precision = precision;
        }

        public ParakeetAttribute(string name, ParameterDirection direction, DbType dbType, int size, byte precision, byte scale)
        {
            Name = name;
            Direction = direction;
            DbType = dbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public ParakeetAttribute(DbType dbType, int size, byte precision, byte scale)
        {
            DbType = dbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public ParakeetAttribute(string name, DbType dbType, int size, byte precision, byte scale)
        {
            Name = name;
            DbType = dbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public ParakeetAttribute(ParameterDirection direction, DbType dbType, int size, byte precision, byte scale)
        {
            Direction = direction;
            DbType = dbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }
    }
}
