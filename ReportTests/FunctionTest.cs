using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTests
{
    public class FunctionTest
    {
        [Test()]
        public void Test1()
        {

            var conn = new fyiReporting.Data.XmlConnection("RdlEngineconfig.Linux.xml");
            Assert.True(conn.Database == null);

        }
    }
}
