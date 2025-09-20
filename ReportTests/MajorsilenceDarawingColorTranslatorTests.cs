using NUnit.Framework;

namespace ReportTests
{
    [TestFixture]
    public class MajorsilenceDarawingColorTranslatorTests
    {
        private static readonly (string ColorString, Majorsilence.Drawing.Color ExpectedColor)[] TestColors =
        {
            ("Bisque", new Majorsilence.Drawing.Color(255, 228, 196)),
            ("Red", new Majorsilence.Drawing.Color(255, 0, 0)),
            ("#F00", new Majorsilence.Drawing.Color(255, 0, 0)),
            ("#FF0000", new Majorsilence.Drawing.Color(255, 0, 0)),
            ("#80FF0000", new Majorsilence.Drawing.Color(128, 255, 0)),
            ("#FF5733", new Majorsilence.Drawing.Color(255, 87, 51)),
            ("#80FF5733", new Majorsilence.Drawing.Color(128, 255, 87, 51)),
            ("#000000", new Majorsilence.Drawing.Color(0, 0, 0)),
            ("#FFFFFFFF", new Majorsilence.Drawing.Color(255, 255, 255)),
            ("#123456", new Majorsilence.Drawing.Color(18, 52, 86)),
            ("#7F123456", new Majorsilence.Drawing.Color(127, 18, 52))
        };
        
        [Test, TestCaseSource(nameof(TestColors))]
        public void FromHtml_ValidHexWithoutHash_ReturnsCorrectColor((string ColorString, Majorsilence.Drawing.Color ExpectedColor) testCase)
        {
            var color = Majorsilence.Drawing.ColorTranslator.FromHtml(testCase.ColorString);
            Assert.That(color.R, Is.EqualTo(testCase.ExpectedColor.R));
            Assert.That(color.G, Is.EqualTo(testCase.ExpectedColor.G));
            Assert.That(color.B, Is.EqualTo(testCase.ExpectedColor.B));
        }
    }
}