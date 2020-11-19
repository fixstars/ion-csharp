using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Ion.Port port = new Ion.Port("test", new Ion.Type(Ion.TypeCode.Uint, 1, 1));
        }
    }
}
