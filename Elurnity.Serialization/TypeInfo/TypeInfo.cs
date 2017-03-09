using System;

namespace Elurnity.Serialization
{
    public class TypeInfo
    {
        public readonly string Name;

        public readonly Action<object> Constructor;

        public readonly TypeMember[] members;
    }

    public partial class TypeInfo<T> : TypeInfo where T : class
    {
        public readonly Action<T> Constructor2;
    }
}
