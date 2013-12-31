using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTests
{
    public class Program
    {
         /// </summary>
        [STAThread]
        static void Main()
        {
            var test = new FunctionTest();
            test.Test1();
        }
    }
}
