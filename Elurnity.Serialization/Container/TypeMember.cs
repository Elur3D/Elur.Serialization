using System;
namespace Elurnity.Serialization
{
    public abstract partial class TypeMember<T> : TypeMember
    {
        public abstract void Unwrap(ref T instance, Container container);

        public abstract void Wrap(ref T instance, Container container);
    }

    public partial class ValueTypeMember<T, U> : TypeMember<T> where T : struct
    {
        public override void Unwrap(ref T instance, Container container)
        {
            U member;
            if (container.TryGetValue(Name, out member))
            {
                Setter(ref instance, member);
            }
        }

        public override void Wrap(ref T instance, Container container)
        {
            container.SetValue(Name, Getter(ref instance));
        }
    }

    public partial class TypeMember<T, U> : TypeMember<T> where T : class
    {
        public override void Unwrap(ref T instance, Container container)
        {
            U member;
            if (container.TryGetValue(Name, out member))
            {
                if (Setter != null)
                {
                    Setter(instance as T, member);
                }
            }
        }

        public override void Wrap(ref T instance, Container container)
        {
            if (Getter != null)
            {
                container.SetValue(Name, Getter(instance as T));
            }
        }
    }
}
