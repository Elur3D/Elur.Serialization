using System;
using System.Collections;
using System.Collections.Generic;

namespace Elurnity.IoC
{
    public sealed class Container : IEnumerable<KeyValuePair<Type, object>>
    {
        private Container _parentContainer;
        private Dictionary<Type, object> _container;

        public Container(Container parentContainer = null, IDictionary<Type, object> data = null)
        {
            _parentContainer = parentContainer;
            _container = data != null ? new Dictionary<Type, object>(data) : new Dictionary<Type, object>();
        }

        public bool TryGetValue(Type type, out object value)
        {
            var that = this;
            object obj;
            while (that != null)
            {
                if (that._container.TryGetValue(type, out obj))
                {
                    value = obj;
                    return true;
                }
                that = that._parentContainer;
            }

            value = null;
            return false;
        }

        public object this[Type type]
        {
            get
            {
                object obj;
                if (TryGetValue(type, out obj))
                {
                    return obj;
                }
                return null;
            }

            set
            {
                _container[type] = value;
            }
        }

        public T Get<T>() where T : class
        {
            return this[typeof(T)] as T;
        }

        public void Set<T>(T value)
        {
            this[typeof(T)] = value;
        }

        #region IEnumerable implementation

        IEnumerator<KeyValuePair<Type, object>> IEnumerable<KeyValuePair<Type, object>>.GetEnumerator()
        {
            var types = new HashSet<Type>();
            var that = this;

            while (that != null)
            {
                foreach (var entry in that._container)
                {
                    var type = entry.Key;
                    if (!types.Contains(type))
                    {
                        types.Add(type);
                        yield return entry;
                    }
                }
                that = this._parentContainer;
            }
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Type, object>>)this).GetEnumerator();
        }

        #endregion
    }
}