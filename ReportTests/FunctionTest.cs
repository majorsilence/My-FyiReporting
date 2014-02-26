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
            // disable test.  travis ci does not have x11
            //string cwd = System.Environment.CurrentDirectory;
           
            //var rdlView = new fyiReporting.RdlViewer.RdlViewer();
            //rdlView.SourceFile = new Uri(System.IO.Path.Combine(cwd,"Reports", "FunctionTest.rdl"));
            //rdlView.Parameters += string.Format("ConnectionString={0}", DatabaseInfo.Connection);
            //rdlView.Rebuild();

            ////foreach (string msg in rdlView.ErrorMessages)
            ////{
            ////    Assert.True(msg.Contains("expression") == false);
            ////}

            //string pdf = System.IO.Path.Combine(cwd, "Test1.pdf");

            //if (System.IO.File.Exists(pdf))
            //{
            //    System.IO.File.Delete(pdf);
            //}

            //rdlView.SaveAs(pdf, fyiReporting.RDL.OutputPresentationType.PDF);

            
            
            //Assert.True(System.IO.File.Exists(pdf));

        }
    }
}
