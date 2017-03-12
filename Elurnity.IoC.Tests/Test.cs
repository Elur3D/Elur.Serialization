
using NUnit.Framework;

namespace Elurnity.IoC.Tests
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        // Simple instantiation

        public class A
        {
        }


        [Test]
        public void ShouldInstantiateSimpleClass()
        {
            var binder = new Binder();
            binder.Register<A, A>();
        }

        [Test]
        public void ShouldReturnSameSingletonInstance()
        {
        }

        [Test]
        public void ShouldThrowExceptionIncompatibleTypes()
        {
        }

        [Test]
        public void BasicSingletonInstantiation()
        {
            var binder = new Binder();
            binder.Register<A, A>();

            var container = binder.GetContainer();

            Assert.IsAssignableFrom<A>(container.Get<A>());

            Assert.AreSame(container.Get<A>(), container.Get<A>());
        }

        // Constructor injection

        public class B
        {
            public A a { get; private set; }

            [Inject]
            public B(A a)
            {
                this.a = a;
            }
        }

        [Test]
        public void ConstructorInjection()
        {
            var binder = new Binder();
            binder.Register<A>();
            binder.Register<B>();

            var container = binder.GetContainer();

            Assert.IsAssignableFrom<A>(container.Get<A>());

            Assert.IsAssignableFrom<A>(container.Get<B>().a);

            Assert.AreEqual(container.Get<A>(), container.Get<B>().a);
        }

        // Property injection

        public class B2
        {
            [Inject]
            public A a { get; set; }
        }

        [Test]
        public void PropertyInjection()
        {
            var binder = new Binder();
            binder.Register<A>();
            binder.Register<B>();

            var container = binder.GetContainer();

            Assert.IsAssignableFrom<A>(container.Get<A>());

            Assert.IsAssignableFrom<A>(container.Get<B>().a);

            Assert.Equals(container.Get<A>(), container.Get<B>().a);
        }

        // Circle between D and E

        public class D
        {
            [Inject]
            public D(E e)
            {
            }
        }

        public class E
        {
            [Inject]
            public E(D d)
            {
            }
        }

        [Test, ExpectedException(typeof(CircularDependencyException))]
        public void ShouldThrowExceptionOnCircularDependencies()
        {
            var binder = new Binder();
            binder.Register<D>();
            binder.Register<E>();

            binder.GetContainer();
        }
    }
}
