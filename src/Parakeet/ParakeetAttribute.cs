using System;
using System.Data;

namespace Parakeet
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParakeetAttribute : Attribute
    {
        public string Name { get; set; }

        public ParameterDirection? Direction { get; set; } = ParameterDirection.Input;

        public DbType? DbType { get; set; } = null;

        public int? Size { get; set; } = null;

        public byte? Precision { get; set; } = null;

        public byte? Scale { get; set; } = null;

        public ParakeetAttribute() { }

        public ParakeetAttribute(string name)
        {
            Name = name;
        }
    }
}
