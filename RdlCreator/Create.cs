using fyiReporting.RDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RdlCreator
{
    public class Create
    {
        public fyiReporting.RDL.Report GenerateRdl(Report report)
        {
            // Create a new instance of the Report class
            fyiReporting.RDL.Report fyiReport;

            var serializer = new XmlSerializer(typeof(Report));
            string xml;
            using (var writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, report);
                xml = writer.ToString();
            }

            var rdlp = new RDLParser(xml);
            fyiReport = rdlp.Parse();
            return fyiReport;
        }
            
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

}
