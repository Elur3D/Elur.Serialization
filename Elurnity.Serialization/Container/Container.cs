using System;
using System.Collections.Generic;

namespace Elurnity.Serialization
{
    public class Container : Dictionary<string, object>
    {
        public bool TryGetValue<T>(string name, out T member)
        {
            throw new NotImplementedException();
        }

        public void SetValue<U>(string name, U u)
        {
            throw new NotImplementedException();
        }
    }
}
