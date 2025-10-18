using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;
using ReportTests.Utils;

namespace ReportTests
{
    /// <summary>
    /// Tests for ReportDefn IDisposable implementation
    /// </summary>
    [TestFixture]
    public class ReportDefnDisposeTest
    {
        private Uri _reportFolder = null;

        [SetUp]
        public void Prepare2Tests()
        {
            _reportFolder = GeneralUtils.ReportsFolder();
            RdlEngineConfig.RdlEngineConfigInit();
        }

        [Test]
        public async Task Report_Dispose_ShouldNotThrow()
        {
            // Arrange
            Uri fileRdlUri = new Uri(_reportFolder, "testdata.rdl");
            
            // Act
            Report report = await RdlUtils.GetReport(fileRdlUri);
            
            // Assert - Should not throw when disposing Report
            Assert.DoesNotThrow(() => report.Dispose());
            
            // Call Dispose again to verify it handles multiple calls
            Assert.DoesNotThrow(() => report.Dispose());
        }

        [Test]
        public async Task Report_UsingStatement_ShouldNotThrow()
        {
            // Arrange
            Uri fileRdlUri = new Uri(_reportFolder, "testdata.rdl");
            
            // Act & Assert - Should not throw when disposing via using statement
            Assert.DoesNotThrowAsync(async () =>
            {
                using (Report report = await RdlUtils.GetReport(fileRdlUri))
                {
                    // Use the report
                    Assert.That(report, Is.Not.Null);
                }
            });
        }

        [Test]
        public async Task Report_WithMultipleReports_Dispose_ShouldNotThrow()
        {
            // Arrange
            Uri fileRdlUri1 = new Uri(_reportFolder, "testdata.rdl");
            Uri fileRdlUri2 = new Uri(_reportFolder, "ListReport.rdl");
            
            // Act
            Report report1 = await RdlUtils.GetReport(fileRdlUri1);
            Report report2 = await RdlUtils.GetReport(fileRdlUri2);
            
            // Assert - Should not throw when disposing multiple reports
            Assert.DoesNotThrow(() => 
            {
                report1.Dispose();
                report2.Dispose();
            });
        }

        [Test]
        public async Task Report_AfterDispose_SecondDispose_ShouldNotThrow()
        {
            // Arrange
            Uri fileRdlUri = new Uri(_reportFolder, "testdata.rdl");
            Report report = await RdlUtils.GetReport(fileRdlUri);
            
            // Act - Dispose multiple times
            report.Dispose();
            
            // Assert - Second dispose should not throw
            Assert.DoesNotThrow(() => report.Dispose());
        }
    }
}
