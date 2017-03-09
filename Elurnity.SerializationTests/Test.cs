
using NUnit.Framework;

namespace Elurnity.Serialization.Tests
{
    [TestFixture]
    public class TestSerialization
    {
        public class ClassA
        {
            public int intField;
            public int intProperty { get; set; }
        }

        public struct StructA
        {
            public int intField;
            public int intProperty { get; set; }
        }

        [Test]
        public void TestWrapClass()
        {
            var wrap = TypeWrap.Wrap(new ClassA
            {
                intField = 100,
                intProperty = 200,
            });

            Assert.AreEqual(100, wrap["intField"]);
            Assert.AreEqual(200, wrap["intProperty"]);
        }

        [Test]
        public void TestUnwrapClass()
        {
            var wrap = new TypeWrap()
            {
                ["intField"] = 100,
                ["intProperty"] = 200,
            };

            var instance = System.Activator.CreateInstance(typeof(ClassA)) as ClassA;

            wrap.Unwrap(instance);

            Assert.AreEqual(100, instance.intField);
            Assert.AreEqual(200, instance.intProperty);
        }

        [Test]
        public void TestWrapStruct()
        {
            var wrap = TypeWrap.Wrap(new StructA
            {
                intField = 100,
                intProperty = 200,
            });

            Assert.AreEqual(100, wrap["intField"]);
            Assert.AreEqual(200, wrap["intProperty"]);
        }

        [Test]
        public void TestUnwrapStruct()
        {
            var wrap = new TypeWrap()
            {
                ["intField"] = 100,
                ["intProperty"] = 200,
            };

            var instance = (StructA)System.Activator.CreateInstance(typeof(StructA));

            wrap.Unwrap(instance);

            Assert.AreEqual(100, instance.intField);
            Assert.AreEqual(200, instance.intProperty);
        }
    }
}
