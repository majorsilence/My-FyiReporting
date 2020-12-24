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

namespace ReportTests.Utils
{
    [TestFixture]
    public class RenderPdf_iTextSharpTests
    {
        private Uri _reportFolder = null;
        private Uri _outputFolder = null;
        private string _extOuput = ".pdf";


        [SetUp]
        public void Prepare2Tests()
        {
            if (_outputFolder == null)
            {
                _outputFolder = GeneralUtils.OutputTestsFolder();
            }

            _reportFolder = GeneralUtils.ReportsFolder();

            Directory.CreateDirectory(_outputFolder.LocalPath);

            RdlEngineConfig.RdlEngineConfigInit();
        }

        private static readonly object[] TestCasesRenderPdf_iTextSharpDraw =
        {
            new object[]{"LineObjects.rdl",
                            "en-US",
                            "RenderPdf_iTextSharp",
                            null },
            new object[]{ "WorldFacts.rdl",
                            "pl-PL",
                            "RenderPdf_iTextSharp",
                            null }, //Load data from xml file
            new object[]{ "ChartTypes.rdl",
                            "en-US",
                            "RenderPdf_iTextSharp",
                            null } ,//Load data from sqlite
            new object[]{ "MatrixExample.rdl",
                            "en-US",
                            "RenderPdf_iTextSharp",
                            null }, //Load data from sqlite
            new object[]{ "ListReport.rdl",
                            "en-US",
                            "RenderPdf_iTextSharp",
                            null } //Load data from sqlite
      
      
      

        };

        [Test, TestCaseSource("TestCasesRenderPdf_iTextSharpDraw")]
        public void RenderPdf_iTextSharpDraw(string file2test,
                                      string cultureName,
                                      string suffixFileName,
                                      Func<Dictionary<string, IEnumerable>> fillDatasets)
        {
            GeneralUtils.ChangeCurrentCultrue(cultureName);
            OneFileStreamGen sg = null;

            Uri fileRdlUri = new Uri(_reportFolder, file2test);
            // We change dir so the SQL lite database is found
            System.IO.Directory.SetCurrentDirectory(_reportFolder.LocalPath);
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
            rap.RunRender(sg, OutputPresentationType.RenderPdf_iTextSharp);
        }

    }
}
