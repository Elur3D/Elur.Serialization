using System;

namespace Elurnity.Serialization
{
    public abstract partial class TypeMember
    {
        public string Name;
        public Type type;

        // Generic getter and setter with boxing/unboxing

        public abstract object Get(object instance);
        public abstract object Set(object instance, object field);
    }

    public abstract partial class TypeMember<T>
    {
    }

    public partial class ValueTypeMember<T, U> : TypeMember<T> where T : struct
    {
        /*
        public delegate U GetterDelegate(ref T instance);
        public delegate void SetterDelegate(ref T instance, U value);

        public GetterDelegate Getter;
        public SetterDelegate Setter;
        */

        public RefFunc<T, U> Getter;
        public RefAction<T, U> Setter;

        public override object Get(object instance)
        {
            var i = (T)instance;
            return Getter(ref i);
        }

        public override object Set(object instance, object field)
        {
            var i = (T)instance;
            Setter(ref i, (U)field);
            return i;
        }
    }

    public partial class TypeMember<T, U> : TypeMember<T> where T : class
    {
        public delegate U GetterDelegate(T instance);
        public delegate void SetterDelegate(T instance, U field);

        public Func<T, U> Getter;
        public Action<T, U> Setter;

        public override object Get(object instance)
        {
            return Getter((T)instance);
        }

        public override object Set(object instance, object field)
        {
            var i = (T)instance;
            Setter(i, (U)field);
            return i;
        }
    }
}
