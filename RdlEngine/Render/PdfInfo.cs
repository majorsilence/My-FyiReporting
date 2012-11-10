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

namespace fyiReporting.RDL
{
	/// <summary>
	///Store information about the document,Title, Author, Company, 
	/// </summary>
	internal class PdfInfo:PdfBase
	{
		private string info;
		internal PdfInfo(PdfAnchor pa):base(pa)
		{
			info=null;
		}
		/// <summary>
		/// Fill the Info Dict
		/// </summary>
		internal void SetInfo(string title,string author,string subject, string company)
		{
			info=string.Format("\r\n{0} 0 obj<</ModDate({1})/CreationDate({1})/Title({2})/Creator(My-FyiReporting)"+
				"/Author({3})/Subject ({4})/Producer(My-FyiReporting Software)/Company({5})>>\tendobj\t",
				this.objectNum,
				GetDateTime(),
				title==null?"":title,
				author==null?"":author,
				subject==null?"":subject,
				company==null?"":company);

		}
		/// <summary>
		/// Get the Document Information Dictionary
		/// </summary>
		/// <returns></returns>
		internal byte[] GetInfoDict(long filePos,out int size)
		{
			return GetUTF8Bytes(info,filePos,out size);
		}
		/// <summary>
		/// Get Date as Adobe needs ie similar to ISO/IEC 8824 format
		/// </summary>
		/// <returns></returns>
		private string GetDateTime()
		{
			DateTime universalDate=DateTime.UtcNow;
			DateTime localDate=DateTime.Now;
			string pdfDate=string.Format("D:{0:yyyyMMddhhmmss}", localDate);
			TimeSpan diff=localDate.Subtract(universalDate);
			int uHour=diff.Hours;
			int uMinute=diff.Minutes;
			char sign='+';
			if(uHour<0)
				sign='-';
			uHour=Math.Abs(uHour);
			pdfDate+=string.Format("{0}{1}'{2}'",sign,uHour.ToString().PadLeft(2,'0'),uMinute.ToString().PadLeft(2,'0'));
			return pdfDate;
		}

	}
}
