
namespace EasyEnum.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [TestClass]
    public class EnumTest
    {

        public dynamic @enum = new Dynamic.Enum("dynamic", seperator: ",")
        {
            "A",
            ("B", 1 << 1),
            ("C", 1 << 2)
        };

        [TestMethod]
        public void GetName()
        {
            var a = @enum.A.ToString();
            var shouldBe = "A";
            Assert.AreEqual(a, shouldBe);
        }

        [TestMethod]
        public void GetValue()
        {
            var b = (int)@enum.B;
            var shouldBe = 2;
            Assert.AreEqual(b, shouldBe);
        }

        [TestMethod]
        public void Output()
        {
            var ab = (@enum.A | @enum.B).ToString();
            var shouldBe = "A,B";
            Assert.AreEqual(ab, shouldBe);
        }
    }
}
