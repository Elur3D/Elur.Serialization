# Elurnity.Serialization [![Build Status](https://travis-ci.org/jonanh/Elurnity.Serialization.svg?branch=master)](https://travis-ci.org/jonanh/Elurnity.Serialization)

A simple set of serialization helpers.

## Objectives
- Provide a simple container, and a base for building simple IoC containers or network message formatters.
- Provide fast accessors for fields and properties, without boxing whenever is possible.
- Provide different backends:
  - Fast reflection using open delegates.
  - Compiled lambdas using expressions.
  - Code generation using T4 templates or IL weaving when AOT is required.

## Status
- Work in progress.

## API

### Unboxed API

An IContainer can be used to store or retrieve data for/from an instance. A IContainer could be a IoC container, a network message or a JSON file.

```
public interface IContainer
{
    bool TryGetValue<T>(TypeMember member, out T value);
    void SetValue<T>(TypeMember member, T value);
}
```

The TypeInfo<T> allows wrapping and unwrapping an instance from a container without boxing.

```
public class TypeInfo<T>
{
    public void Unwrap(ref T instance, IContainer container);
    public void Wrap(ref T instance, IContainer container);
}
```

The TypeMembers provide the getters and setters for reference types and value types

```
public class TypeMember<T, U> : TypeMember<T> where T : class
{
    public Func<T, U> Getter;
    public Action<T, U> Setter;
}

public partial class ValueTypeMember<T, U> : TypeMember<T> where T : struct
{
    public RefFunc<T, U> Getter;
    public RefAction<T, U> Setter;
}
```

### Boxed API

```
public class TypeInfo
{
        public readonly string Name;
        public readonly Action<object> Constructor;
        
        
        public readonly TypeMember[] members;
}
```

```
    public abstract partial class TypeMember
    {
        public string Name;
        public Type type;
        public object Get(object instance);
        public object Set(object instance, object field);
    }
```

## Other considerations

- The implemented containers should take care of the type convertions/cohercions.
  - A JSON container for example will convert a data type into a set of primitive types, lists and dictionaries. 
- Be aware of object circular references while traversing objects, use a simple depth-first search for example :-)
- Dictionary look ups are expensive.

## Alternatives
- [mgravell's fast-member](https://github.com/mgravell/fast-member)
- [vexe's Fast.Reflection](https://github.com/vexe/Fast.Reflection)
- [fasterflect](https://fasterflect.codeplex.com/)

## References
- [Wire – Writing one of the fastest .NET serializers](https://rogerjohansson.blog/2016/08/16/wire-writing-one-of-the-fastest-net-serializers/)
