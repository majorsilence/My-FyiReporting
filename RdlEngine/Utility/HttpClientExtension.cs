using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Rdl.Utility
{
    public static class HttpClientExtension
    {
        public static void AddMajorsilenceReportingUserAgent(this HttpClient client)
        {
            if (client.DefaultRequestHeaders.UserAgent.Count == 0)
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3 MajorsilenceReporting/1.0");
            }
        }
    }
}
