using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using fyiReporting.RDL;
using NUnit.Framework;
using ReportTests.Utils;

namespace ReportTests
{
    [TestFixture]
    public class ExcelValetTests
    {

        private Uri _reportFolder=null;
        private Uri _outputFolder=null;
        private string _extOuput = ".xlsx";


        [SetUp]
        public void Prepare2Tests()
        {
            if (_outputFolder==null)
            {
                _outputFolder = GeneralUtils.OutputTestsFolder();
            }

            _reportFolder = GeneralUtils.ReportsFolder();

            Directory.CreateDirectory(_outputFolder.LocalPath);

            RdlEngineConfig.RdlEngineConfigInit();
        }

        private static readonly object[] TestCasesExcelValetFormat =
        {
            new object[]{"testdata.rdl",
                            "pl-PL",
                            "to_100items",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100) ) },

            new object[]{"testdata.rdl",
                            "pl-PL",
                            "to_100items_from_1_10000000_100",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100,1,10000000,100) ) },

             new object[]{"testdata.rdl",
                            "en-GB",
                            "to_100items_from_1_10000000_100",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100,1,10000000,100) ) },

            new object[]{"testdata.rdl",
                            "en-US",
                            "to_100items_from_1_10000000_100",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100,1,10000000,100) ) },

            new object[]{"testdata.rdl",
                            "ru-RU",
                            "to_100items_from_1_10000000_100",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100,1,10000000,100) ) },

            new object[]{"testdata.rdl",
                            "ru-RU",
                            "to_100items",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100) ) },

            new object[]{"testdata.rdl",
                            "en-GB",
                            "to_100items",
                            new Func<Dictionary<string, IEnumerable>>( () => SampleTestData(100) ) },

            new object[]{ "WorldFacts.rdl",
                            "pl-PL",
                            "general",
                            null } //Load data from xml file
            
        };

        [Test, TestCaseSource("TestCasesExcelValetFormat")]
        public void ExcelValet_Format(string file2test,
                                      string cultureName,
                                      string suffixFileName,
                                      Func<Dictionary<string, IEnumerable>> fillDatasets)
        {
            GeneralUtils.ChangeCurrentCultrue(cultureName);
            OneFileStreamGen sg = null;

            Uri fileRdlUri = new Uri(_reportFolder, file2test);
            Report rap = RdlUtils.GetReport(fileRdlUri);
            rap.Folder = _reportFolder.LocalPath;
            if (fillDatasets != null)
            {
                Dictionary<string, IEnumerable> dataSets = fillDatasets();

                foreach (var dataset in dataSets)
                {
                    rap.DataSets[dataset.Key].SetData(dataset.Value);
                }
            }
            rap.RunGetData(null);

            string fileNameOut = string.Format("{0}_{1}_{2}{3}",
                                                file2test,
                                                cultureName,
                                                suffixFileName,
                                                _extOuput);

            string fullOutputPath = System.IO.Path.Combine(_outputFolder.LocalPath, fileNameOut);
            sg = new OneFileStreamGen(fullOutputPath, true);
            rap.RunRender(sg, OutputPresentationType.ExcelTableOnly);


            
            Assert.IsTrue(OpenXmlUtils.ValidateSpreadsheetDocument(fullOutputPath));
            

            
        }

        public static Dictionary<string, IEnumerable> SampleTestData(int maxRows,
                                                                     int minValue=1,
                                                                     int maxValue=100,
                                                                     int denominator=100,
                                                                     string dataSetName = "Data")
        {
            List<ExcelTestData> values = new List<ExcelTestData>();
            Dictionary<string, IEnumerable> results = new Dictionary<string, IEnumerable>();

            Random random = new Random();
            RandomDateTime randomdt = new RandomDateTime(1990, 1, 1);

            for (int i = 0; i < maxRows; i++)
            {
                int r = random.Next(minValue, maxValue); 
                decimal result =Convert.ToDecimal( (double)r / denominator);
                values.Add(new ExcelTestData() {
                    decimalvalue = result,
                    datetimevalue = randomdt.Next()
                });
            }

            results.Add(dataSetName, values);
            return results;
        }


       
    }

    internal class ExcelTestData
    {
        public decimal decimalvalue;
        public DateTime datetimevalue;
    }

}
