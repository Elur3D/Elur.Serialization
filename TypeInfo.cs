
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Serialization
{
    public class TypeInfo
    {
        public class Member
        {
            public Type type;
            public string name;
            public MemberInfo info;
            public Getter getter;
            public Setter setter;
        }

        public Constructor ctor;
        public List<Member> members;

        public delegate object Getter(object instance);
        public delegate void Setter(object instance, object value);
        public delegate object Constructor();

        public static TypeInfo getInfo(Type type)
        {
            List<Member> members = new List<Member>();

            Constructor ctor = () =>
            {
                return Activator.CreateInstance(type);
            };

            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var _info = info;

                Member member = new Member
                {
                    type = _info.PropertyType,
                    name = _info.Name,
                    info = _info,
                };

                if (info.CanRead && info.GetIndexParameters().Length == 0)
                {
                    member.getter = (instance) => _info.GetGetMethod().Invoke(instance, null);
                }

                if (info.CanWrite)
                {
                    member.setter = (instance, value) => _info.SetValue(instance, value, null);
                }

                members.Add(member);
            }

            foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var _info = info;

                members.Add(new Member
                {
                    type = _info.FieldType,
                    name = _info.Name,
                    info = _info,
                    getter = (instance) => _info.GetValue(instance),
                    setter = (instance, value) => _info.SetValue(instance, value),
                });
            }

            return new TypeInfo
            {
                ctor = ctor,
                members = members,
            };
        }
    }
}