using System;

namespace Elurnity.IoC
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor, Inherited = true, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}
