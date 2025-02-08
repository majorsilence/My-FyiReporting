using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Majorsilence.Reporting.Rdl
{
    public class CrossDelegate
    {
        public delegate string GetContent(string ContentSource);
        public GetContent SubReportGetContent=null;
    }
}
