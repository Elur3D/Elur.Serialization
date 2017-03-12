using System;
using System.Reflection;

namespace Elurnity.Serialization
{
    public static class ReflectionTypeMember
    {
        internal class ValueTypeMemberReflection<T, U> : ValueTypeMember<T, U> where T : struct
        {
            public ValueTypeMemberReflection(MethodInfo getMethod, MethodInfo setMethod)
            {
                if (getMethod != null)
                {
                    Getter = (ref T instance) => (U)getMethod.Invoke(instance, null);
                }

                if (setMethod != null)
                {
                    //Setter = (ref T instance, U value) => setMethod.Invoke(instance, new [] { value });
                }
            }
        }

        internal class TypeMemberReflection<T, U> : TypeMember<T, U> where T : class
        {
            public TypeMemberReflection(MethodInfo getMethod, MethodInfo setMethod)
            {
                if (getMethod != null)
                {
                    //Getter = getMethod.CreateDelegate(typeof(GetterDelegate)) as GetterDelegate;
                }

                if (setMethod != null)
                {
                    //Setter = setMethod.CreateDelegate(typeof(SetterDelegate)) as SetterDelegate;
                }
            }

            public TypeMemberReflection(FieldInfo fieldInfo)
            {
                var getter = (Func<T, object>)fieldInfo.GetValue;

                // TODO: split and add class restriction to U
                //var setter = (Action<T, U>)fieldInfo.SetValue;

                Getter = (instance) => (U)getter(instance);
                Setter = (instance, field) => fieldInfo.SetValue(instance, field);
            }
        }

        public static TypeMember CreateMember(MethodInfo getMethod, MethodInfo setMethod, Type instanceType, Type memberType)
        {
            var typeArguments = new[] { instanceType, memberType };
            var type = typeof(TypeMemberReflection<,>).MakeGenericType(typeArguments);
            return Activator.CreateInstance(type, new[] { getMethod, setMethod }) as TypeMember;
        }

        public static TypeMember CreateValueMember(MethodInfo getMethod, MethodInfo setMethod, Type instanceType, Type memberType)
        {
            var typeArguments = new[] { instanceType, memberType };
            var type = typeof(ValueTypeMemberReflection<,>).MakeGenericType(typeArguments);
            return Activator.CreateInstance(type, new[] { getMethod, setMethod }) as TypeMember;
        }

        public static TypeMember CreateMember(FieldInfo fieldInfo, Type instanceType, Type memberType)
        {
            var typeArguments = new[] { instanceType, memberType };
            var type = typeof(TypeMemberReflection<,>).MakeGenericType(typeArguments);
            return Activator.CreateInstance(type, fieldInfo) as TypeMember;
        }

        public static TypeMember CreateValueMember(FieldInfo fieldInfo, Type instanceType, Type memberType)
        {
            var typeArguments = new[] { instanceType, memberType };
            var type = typeof(ValueTypeMemberReflection<,>).MakeGenericType(typeArguments);
            return Activator.CreateInstance(type, fieldInfo) as TypeMember;
        }
    }
}
