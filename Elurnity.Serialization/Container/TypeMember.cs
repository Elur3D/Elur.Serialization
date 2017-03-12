using System;
namespace Elurnity.Serialization
{
    public abstract partial class TypeMember<T> : TypeMember
    {
        public abstract void Unwrap(ref T instance, IContainer container);

        public abstract void Wrap(ref T instance, IContainer container);
    }

    public partial class ValueTypeMember<T, U> : TypeMember<T> where T : struct
    {
        public override void Unwrap(ref T instance, IContainer container)
        {
            U member;
            if (container.TryGetValue(this, out member))
            {
                Setter(ref instance, member);
            }
        }

        public override void Wrap(ref T instance, IContainer container)
        {
            container.SetValue(this, Getter(ref instance));
        }
    }

    public partial class TypeMember<T, U> : TypeMember<T> where T : class
    {
        public override void Unwrap(ref T instance, IContainer container)
        {
            U member;
            if (container.TryGetValue(this, out member))
            {
                if (Setter != null)
                {
                    Setter(instance as T, member);
                }
            }
        }

        public override void Wrap(ref T instance, IContainer container)
        {
            if (Getter != null)
            {
                container.SetValue(this, Getter(instance as T));
            }
        }
    }
}
