
using System;
using System.Collections.Generic;

namespace Elurnity.IoC
{
    public class Binder
    {
        private Dictionary<Type, Type> dict = new Dictionary<Type, Type>();

        public void Register<T, U>()
        {
            if (!typeof(T).IsAssignableFrom(typeof(U)))
            {
                throw new Exception();
            }

            dict[typeof(U)] = typeof(T);
        }

        public void Register<T>()
        {
            dict[typeof(T)] = typeof(T);
        }

        public Container GetContainer(Container container = null)
        {
            container = container ?? new Container();

            var processedTypes = new HashSet<Type>();
            var binderTypes = new HashSet<Type>(dict.Keys);

            foreach (var type in dict.Keys)
            {
                ProcessType(type, binderTypes, processedTypes, container);
            }

            return container;
        }

        private object ProcessType(Type type, HashSet<Type> types, HashSet<Type> processing, Container container)
        {
            object obj = null;

            if (container.TryGetValue(type, out obj))
            {
                return obj;
            }

            else if (processing.Contains(type))
            {
                throw new CircularDependencyException();
            }

            else if (type.IsInterface || type.IsAbstract)
            {
                throw new Exception();
            }

            else if (!types.Contains(type))
            {
                throw new UnknownDependencyTypeException();
            }

            else
            {
                processing.Add(type);

                var typeInfo = TypeInfo.getInfo(type);
                var dependencies = typeInfo.ctorDependencies;
                var instances = new object[dependencies.Length];

                for (int i = 0; i < dependencies.Length; i++)
                {
                    instances[i] = ProcessType(dependencies[i], types, processing, container);
                }

                processing.Remove(type);

                var instance = TypeInfo.getInfo(type).injectorCtor(instances);

                if (instance is IInitializable)
                {
                    var initializable = instance as IInitializable;
                    initializable.Init();
                }

                container[type] = instance;

                if (container[dict[type]] == null)
                {
                    container[dict[type]] = instance;
                }

                return instance;
            }
        }
    }
}
