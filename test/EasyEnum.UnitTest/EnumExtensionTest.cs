
namespace EasyEnum.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Description = System.ComponentModel.DescriptionAttribute;

    [TestClass]
    public class EnumExtensionTest
    {
        [TestMethod]
        public void ToEnumeInfo()
        {
            var typeA = EnumTypeA.A;
            Assert.IsInstanceOfType(typeA.ToEnumInfo(), typeof(EnumExtension.EnumInfo<EnumTypeA>));
        }

        

        [TestClass]
        public class EnumInfoTest
        {
            [TestMethod]
            public void GetsFlags()
            {
                var typeA = EnumTypeA.A | EnumTypeA.B;
                var shouldBe = new[] { EnumTypeA.A, EnumTypeA.B };

                var flags = typeA.ToEnumInfo().Flags;
                Assert.IsTrue(flags[0].Equals(shouldBe[0]) && flags[1].Equals(shouldBe[1]));
            }

            [TestMethod]
            public void GetAttributes()
            {
                var info = EnumTypeA.A.ToEnumInfo();
                
                var typeA = info.GetAttributes<Description>();
                if (typeA.Count == 1)
                {
                    var shouldBe = "Two";
                    Assert.AreEqual(typeA[EnumTypeA.A].Description, shouldBe);
                }
                else
                    Assert.Fail();
            }

            [TestMethod]
            public void Cast()
            {
                var typeA = EnumTypeA.A;
                var shouldBe = EnumTypeB.B;

                var typeB = typeA.ToEnumInfo().Cast<EnumTypeB>();
                Assert.AreEqual(typeB, shouldBe);
            }
        }
    }

}
