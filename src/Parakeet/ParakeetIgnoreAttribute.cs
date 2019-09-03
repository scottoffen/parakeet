using System;

namespace Parakeet
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParakeetIgnoreAttribute : Attribute { }
}