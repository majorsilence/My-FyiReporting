using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace fyiReporting.CRI
{
    public class XmlHelpers
    {

        /// <summary>
        /// Get the child element with the specified name.  Return the InnerText
        /// value if found otherwise return the passed default.
        /// </summary>
        /// <param name="xNode">Parent node</param>
        /// <param name="name">Name of child node to look for</param>
        /// <param name="def">Default value to use if not found</param>
        /// <returns>Value the named child node</returns>
        public static string GetNamedElementValue(XmlNode xNode, string name, string def)
        {
            if (xNode == null)
                return def;

            foreach (XmlNode cNode in xNode.ChildNodes)
            {
                if (cNode.NodeType == XmlNodeType.Element &&
                    cNode.Name == name)
                    return cNode.InnerText;
            }
            return def;
        }

        public static void CreateChild(XmlNode node, string nm, string val)
        {
            XmlDocument xd = node.OwnerDocument;
            XmlNode cp = xd.CreateElement("CustomProperty");
            node.AppendChild(cp);

            XmlNode name = xd.CreateElement("Name");
            name.InnerText = nm;
            cp.AppendChild(name);

            XmlNode v = xd.CreateElement("Value");
            v.InnerText = val;
            cp.AppendChild(v);
        }
    }
}
