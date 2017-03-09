
using NUnit.Framework;

namespace Elurnity.Serialization.Tests
{
    [TestFixture]
    public class TestSerialization2
    {
        public class ClassA
        {
            public int intField;
            public int intProperty { get; set; }
        }

        [Test]
        public void TestWrapClass()
        {
            var info = TypeInfoReflection.getInfo(typeof(ClassA));

            Assert.AreEqual(info.members.Count, 2);

            var classA = new ClassA
            {
                intField = 50,
                intProperty = 100,
            };

            Assert.AreEqual(info.members[0].Get(classA), 100);

            info.members[0].Set(classA, 200);

            Assert.AreEqual(info.members[0].Get(classA), 200);


            Assert.AreEqual(info.members[1].Get(classA), 50);

            info.members[1].Set(classA, 200);

            Assert.AreEqual(info.members[1].Get(classA), 200);
        }
    }
}
