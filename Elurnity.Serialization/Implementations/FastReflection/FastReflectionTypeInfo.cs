using System;
using System.Collections.Generic;
using System.Reflection;

namespace Elurnity.Serialization
{
    public class FastReflectionTypeInfo
    {
        public List<TypeMember> members;

        protected FastReflectionTypeInfo()
        {
        }

        public static FastReflectionTypeInfo getInfo(Type type)
        {
            List<TypeMember> members = new List<TypeMember>();

            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var _info = info;

                MethodInfo getMethodInfo = null;
                MethodInfo setMethodInfo = null;

                if (info.CanRead && info.GetIndexParameters().Length == 0)
                {
                    getMethodInfo = _info.GetGetMethod();
                }

                if (info.CanWrite)
                {
                    setMethodInfo = _info.GetSetMethod();
                }

                var memberTypeInfo = FastReflectionTypeMember.CreateMember(getMethodInfo, setMethodInfo, type, info.PropertyType);
                members.Add(memberTypeInfo);
            }

            foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var _info = info;

                var memberTypeInfo = FastReflectionTypeMember.CreateMember(_info, type, info.FieldType);
                members.Add(memberTypeInfo);
            }

            return new FastReflectionTypeInfo
            {
                members = members,
            };
        }
    }
}
