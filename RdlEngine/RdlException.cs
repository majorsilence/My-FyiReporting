using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Rdl
{
    public class RdlException : Exception
    {
        public RdlException(string message) : base(message)
        {
        }

        public RdlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
