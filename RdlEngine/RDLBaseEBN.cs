using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fyiReporting.RDL
{
    public class CrossDelegate
    {
        public delegate string GetContent(string ContentSource);
        public GetContent SubReportGetContent=null;
    }
}
