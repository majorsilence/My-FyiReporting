using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class TestParameterNameExtraction
    {
        [TestCase("=Parameters!Test.Value", "Test")]
        [TestCase("={?Test}", "Test")]
        public void ExtractNameFromParameterExpression(string expression, string expectedParameterName)
        {
            ReportNames t = new fyiReporting.RdlDesign.re
        }
    }
}
