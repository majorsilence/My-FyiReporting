using Majorsilence.Reporting.RdlDesign;
using NUnit.Framework;
using System;

namespace ReportDesigner.Tests
{
    [TestFixture]
    public class DesignerUtilityTests
    {
        [TestCase("=Parameters!Test.Value", "Test")]
        [TestCase("={?Test}", "Test")]
        public void ExtractParameterNameFromExpression(string expression, string expectedname)
        {
            var result = DesignerUtility.ExtractParameterNameFromParameterExpression(expression);
            Assert.That(result.Equals(expectedname));
        }
    }
}
