/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/

using System;
using System.Xml;

namespace fyiReporting.RDL
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
		
		override internal void FinalPass()
		{
			return;
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
