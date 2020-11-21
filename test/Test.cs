using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PortTest1()
        {
            var t = new Ion.Type(Ion.TypeCode.Int, 32, 1);
            var port = new Ion.Port("v", t, 0);

        }

        [TestMethod]
        public void ParamTest1()
        {
            var param = new Ion.Param("x", "1");
        }

        [TestMethod]
        public void NodeTest1()
        {
            var node = new Ion.Node();
        }

        [TestMethod]
        public void RunTest()
        {
            var t = new Ion.Type(Ion.TypeCode.Int, 32, 1);

            var ip = new Ion.Port("input", t, 2);

            var v41 = new Ion.Param("v", "41");

            var b = new Ion.Builder();
            b.SetTarget("host");
            b.WithBBModule("ion-bb-test.dll");

            var n = b.Add("test_inc_i32x2").SetPort(ip).SetParam(v41);

            var pm = new Ion.PortMap();
            
            int[] sizes = { 4, 4 };
            var ibuf = new Ion.Buffer(t, sizes);
            var obuf = new Ion.Buffer(t, sizes);

            Int32[] idata = new Int32[4 * 4];
            Array.Fill(idata, 1);
            
            Int32[] odata = new Int32[4 * 4];
            Array.Fill(odata, 0);

            byte[] buf = new byte[4 * 4 * sizeof(Int32)];

            Buffer.BlockCopy(idata, 0, buf, 0, buf.Length);
            ibuf.Write(buf);
            
            Buffer.BlockCopy(odata, 0, buf, 0, buf.Length);
            obuf.Write(buf);

            pm.Set(ip, ibuf);
            pm.Set(n["output"], obuf);
            
            b.Run(pm);

            obuf.Read(buf);
            Buffer.BlockCopy(buf, 0, odata, 0, buf.Length);

            for (int i=0; i<4 * 4; ++i)
            {
                Assert.AreEqual(42, odata[i]);                          
            }
         }
    }
}
