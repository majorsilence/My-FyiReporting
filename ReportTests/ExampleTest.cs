using System;
using NUnit.Framework;

namespace ReportTests
{
	public class ExampleTest
	{

		[Test()]
		public void Test1()
		{

			var conn = new fyiReporting.Data.XmlConnection("RdlEngineconfig.xml");
			Assert.That(conn.Database == null);

		}

	}
}

