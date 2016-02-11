
using System.Collections.Generic;

namespace Serialization
{
    public class TypeWrap : Dictionary<string, object>
    {
        public TypeWrap()
        {
        }

        public TypeWrap(IDictionary<string, object> table)
            : base(table)
        {
        }

        public static TypeWrap wrap(object instance)
        {
            TypeWrap wrap = new TypeWrap();

            foreach (var member in TypeInfo.getInfo(instance.GetType()).members)
            {
                if (member.getter != null)
                {
                    object value = member.getter(instance);

                    wrap[member.name] = value;
                }
            }

            return wrap;
        }

        public void unwrap(object instance)
        {
            if (instance == null)
            {
                return;
            }

            foreach (var member in TypeInfo.getInfo(instance.GetType()).members)
            {
                if (member.setter != null && this.ContainsKey(member.name))
                {
                    object value = this[member.name];

                    member.setter(instance, value);
                }
            }
        }
    }
}