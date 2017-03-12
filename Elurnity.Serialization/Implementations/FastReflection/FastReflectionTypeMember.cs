using System;
using System.Reflection;

namespace Elurnity.Serialization
{
    public static class FastReflectionTypeMember
    {
        internal class ValueTypeMemberReflection<T, U> : ValueTypeMember<T, U> where T : struct
        {
            public ValueTypeMemberReflection(MethodInfo getMethod, MethodInfo setMethod)
            {
                if (getMethod != null)
                {
                    Getter = getMethod.CreateDelegate(typeof(RefFunc<T, U>)) as RefFunc<T, U>;
                }

                if (setMethod != null)
                {
                    Setter = getMethod.CreateDelegate(typeof(RefAction<T, U>)) as RefAction<T, U>;
                }
            }

            public ValueTypeMemberReflection(Delegate getMethod, Delegate setMethod)
            {
                if (getMethod != null)
                {
                    Getter = getMethod as RefFunc<T, U>;
                }

                if (setMethod != null)
                {
                    Setter = setMethod as RefAction<T, U>;
                }
            }
        }

        internal class TypeMemberReflection<T, U> : TypeMember<T, U> where T : class
        {
            public TypeMemberReflection(MethodInfo getMethod, MethodInfo setMethod)
            {
                if (getMethod != null)
                {
                    Getter = getMethod.CreateDelegate(typeof(Func<T, U>)) as Func<T, U>;
                }

                if (setMethod != null)
                {
                    Setter = setMethod.CreateDelegate(typeof(Action<T, U>)) as Action<T, U>;
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
