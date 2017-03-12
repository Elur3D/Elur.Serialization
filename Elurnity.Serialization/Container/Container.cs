using System;
using System.Collections.Generic;

namespace Elurnity.Serialization
{
    public interface IContainer
    {
        bool TryGetValue<T>(TypeMember member, out T value);
        void SetValue<T>(TypeMember member, T value);
    }

    public class Container : Dictionary<string, object>
    {
        public bool TryGetValue<T>(TypeMember member, out T value)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(TypeMember member, T value)
        {
            throw new NotImplementedException();
        }
    }
}
