using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTests
{
    public class DatabaseInfo
    {

        public static string Connection
        {
            get
            {
                string cwd = System.Environment.CurrentDirectory;
                string db = System.IO.Path.Combine(cwd, "northwindEF.db");

                string cn = String.Format("Data Source={0};Version=3;Pooling=True;Max Pool Size=100;", db);
                return cn;
            }
        }

    }
}
