# Elurnity.Serialization

A simple set of serialization helpers.

## Objectives
- Provide a simple container, and a base for building simple IoC containers or network message formatters.
- Provide fast accessors for fields and properties, without boxing when possible.
- Provide different backends:
  - Fast reflection using open delegates.
  - Compiled lambdas using expressions.
  - Code generation using T4 templates or IL weaving when AOT is required.
- Just provide some useful building blocks.

## API

An IContainer can be used to store or retrieve data for/from an instance. A IContainer could be a IoC container, a network message or a JSON file.

```
public interface IContainer
{
    bool TryGetValue<T>(TypeMember member, out T value);
    void SetValue<T>(TypeMember member, T value);
}
```

The TypeInfo<T> allows wrapping and unwrapping the data from the container without boxing.

```
public class TypeInfo<T>
{
    public void Unwrap(ref T instance, IContainer container);
    public void Wrap(ref T instance, IContainer container);
}
```

The TypeMembers provide getters and setters for a reference types and value types

```
public class TypeMember<T, U> : TypeMember<T> where T : class
{
    // Unboxed
    public Func<T, U> Getter;
    public Action<T, U> Setter;
}

public partial class ValueTypeMember<T, U> : TypeMember<T> where T : struct
{
    public RefFunc<T, U> Getter;
    public RefAction<T, U> Setter;
}
```

## Alternatives
- [mgravell's fast-member](https://github.com/mgravell/fast-member)
- [vexe's Fast.Reflection](https://github.com/vexe/Fast.Reflection)
- [fasterflect](https://fasterflect.codeplex.com/)
