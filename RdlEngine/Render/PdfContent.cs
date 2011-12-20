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
using System.IO;

namespace fyiReporting.RDL
{
	/// <summary>
	///Represents the general content stream in a Pdf Page. 
	///This is used only by the PageObjec 
	/// </summary>
	internal class PdfContent:PdfBase
	{
		private string content;
		private string contentStream;
		private bool CanCompress;
		internal PdfContent(PdfAnchor pa):base(pa)
		{
			CanCompress = pa.CanCompress;
			content=null;
//			contentStream="%stream\r";
			contentStream="";
		}
		/// <summary>
		/// Set the Stream of this Content Dict.
		/// Stream is taken from PdfElements Objects
		/// </summary>
		/// <param name="stream"></param>
		internal void SetStream(string stream)
		{
			if (stream == null)
				return;
			contentStream+=stream;
		}
		/// <summary>
		/// Content object
		/// </summary>
		/// <summary>
		/// Get the Content Dictionary
		/// </summary>
		internal byte[] GetContentDict(long filePos,out int size)
		{	
			// When no compression
			if (!CanCompress)
			{
				content=string.Format("\r\n{0} 0 obj<</Length {1}>>stream\r{2}\rendstream\rendobj\r",
					this.objectNum,contentStream.Length,contentStream);

				return GetUTF8Bytes(content,filePos,out size);
			}

			// Try to use compression; could still fail in which case fall back to uncompressed
			Stream strm=null;
			MemoryStream cs=null;
			try
			{
				CompressionConfig cc = RdlEngineConfig.GetCompression();
				cs = new MemoryStream();	// this will contain the content stream
				if (cc != null)
					strm = cc.GetStream(cs);

				if (strm == null)
				{	// can't compress string
					cs.Close();		

					content=string.Format("\r\n{0} 0 obj<</Length {1}>>stream\r{2}\rendstream\rendobj\r",
						this.objectNum,contentStream.Length,contentStream);

					return GetUTF8Bytes(content,filePos,out size);
				}

				// Compress the contents
				int cssize;
				byte[] ca = PdfUtility.GetUTF8Bytes(contentStream,out cssize);
				strm.Write(ca, 0, cssize);
				strm.Flush();
				cc.CallStreamFinish(strm);

				// Now output the PDF command
				MemoryStream ms=new MemoryStream();
				int s;
				byte[] ba;

				// get the compressed data;  we need the lenght now
				byte[] cmpData = cc.GetArray(cs);

				// write the beginning portion of the PDF object
				string ws = string.Format("\r\n{0} 0 obj<< /Filter /FlateDecode /Length {1}>>stream\r",
					this.objectNum, cmpData.Length);

				ba = GetUTF8Bytes(ws,filePos,out s);	// this will also register the object
				ms.Write(ba, 0, ba.Length);
				filePos += s;

				// write the Compressed data
				ms.Write(cmpData, 0, cmpData.Length);
				filePos += ba.Length;

				// write the end portion of the PDF object
				ba = PdfUtility.GetUTF8Bytes("\rendstream\rendobj\r", out s);
				ms.Write(ba, 0, ba.Length);
				filePos += s;

				// now the final output array
				ba = ms.ToArray();
				size = ba.Length;
				return ba;

			}
			finally
			{
				if (strm != null)
					strm.Close();
			}
		}
	}

}
