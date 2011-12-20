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
using System.Diagnostics;

namespace fyiReporting.RDL
{
	/// <summary>
	/// Models the Catalog dictionary within a pdf file. This is the first created object. 
	/// It contains references to all other objects within the List of Pdf Objects.
	/// </summary>
	internal class PdfCatalog:PdfBase
	{
		private string catalog;
		private string lang;
		internal PdfCatalog(PdfAnchor pa, string l):base(pa)
		{
			if (l != null)
				lang = String.Format("/Lang({0})", l);
			else
				lang = "";
		}
		/// <summary>
		///Returns the Catalog Dictionary 
		/// </summary>
		/// <returns></returns>
		internal byte[] GetCatalogDict(int outline, int refPageTree,long filePos,out int size)
		{
			Debug.Assert(refPageTree >= 1);
			
            if (outline >= 0)
                catalog=string.Format("\r\n{0} 0 obj<</Type /Catalog{2}/Pages {1} 0 R /Outlines {3} 0 R>>\tendobj\t",
				    this.objectNum,refPageTree, lang, outline);
            else
                catalog = string.Format("\r\n{0} 0 obj<</Type /Catalog{2}/Pages {1} 0 R>>\tendobj\t",
                    this.objectNum, refPageTree, lang);
            
            return this.GetUTF8Bytes(catalog, filePos, out size);
		}

	}
}
