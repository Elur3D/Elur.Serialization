using System;

namespace Elurnity.Serialization
{
    public partial class TypeInfo<T>
    {
        public void Unwrap(ref T instance, Container container)
        {
            if (instance == null)
            {
                return;
            }

            foreach (var member in members)
            {
                var m = member as TypeMember<T>;
                m.Unwrap(ref instance, container);
            }
        }

        public void Wrap(ref T instance, Container container)
        {
            if (instance == null)
            {
                return;
            }

            foreach (var member in members)
            {
                var m = member as TypeMember<T>;
                m.Wrap(ref instance, container);
            }
        }
    }
}
