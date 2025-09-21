

using System;
using System.Threading.Tasks;
using System.Xml;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	///The Custom element allows report design tools to pass information to report output components.
	///This element may contain any valid XML. The engine will simply pass the contents of Custom
	///unchanged. Client applications using the Custom element are recommended to place their
	///custom properties under their own single subelement of Custom, defining a namespace for that
	///node.
	///  Example: 
	///   <Table><Custom><HTML><SortAble>True</SortAble></HTML></Custom> .... </Table>
	///     The HTML renderer uses this information to generate JavaScript to allow
	///     user sorting of the table in the browser.
	///</summary>
	[Serializable]
	internal class Custom : ReportLink
	{
		//The Custom element allows report design tools to pass information to report output components.
		//This element may contain any valid XML. The engine will simply pass the contents of Custom
		//unchanged. Client applications using the Custom element are recommended to place their
		//custom properties under their own single subelement of Custom, defining a namespace for that
		//node.
		//  Example: 
		//   <Table><Custom><HTML><SortAble>True</SortAble></HTML> .... </Table>
		//     The HTML renderer uses this information to generate JavaScript to allow
		//     user sorting of the table in the browser.
		string _XML;	// custom information for report.
		XmlDocument _CustomDoc;		// XML document just for Custom subtree
	
		internal Custom(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_XML= xNode.OuterXml;	// this includes the "Custom" tag at the top level

			// Put the subtree into its own document
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = false;
			doc.LoadXml(_XML);
			_CustomDoc = doc;

		}
		
		override internal Task FinalPass()
		{
			return Task.CompletedTask;
		}

		internal string XML
		{
			get { return  _XML; }
			set {  _XML = value; }
		}

		internal XmlNode CustomXmlNode
		{
			get 
			{ 
				XmlNode xNode;
				xNode = _CustomDoc.LastChild;
				return  xNode; 
			}
		}
	}
}
