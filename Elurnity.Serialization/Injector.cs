using System;
using System.Collections;
using System.Collections.Generic;

namespace Elurnity.Serialization
{
    public sealed class IoCContainer : Dictionary<Type, object>
    {
    }

    public sealed class Injector
    {
        private HashSet<object> visited = new HashSet<object>();

        public void inject(object instance, IoCContainer container)
        {
            visited.Clear();
            _inject(instance, container);
        }

        private void _inject(object instance, IoCContainer container)
        {
            if (instance == null || visited.Contains(instance))
            {
                return;
            }

            visited.Add(instance);

            if (instance is IEnumerable)
            {
                foreach (var iter in instance as IEnumerable)
                {
                    _inject(iter, container);
                }
            }

            foreach (var member in SimpleTypeInfo.getInfo(instance.GetType()).members)
            {
                object value = null;

                if (member.setter != null && container.TryGetValue(member.type, out value))
                {
                    if (value != null)
                    {
                        member.setter(instance, value);
                    }
                }
                else if (member.getter != null)
                {
                    value = member.getter(instance);
                    _inject(value, container);
                }
            }
        }
    }
}
