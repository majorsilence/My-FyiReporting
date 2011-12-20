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
using System.Collections;
using System.IO;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
	/// <summary>
	///Represents the font dictionary used in a pdf page
	/// </summary>
	internal class PdfFonts
	{
		PdfAnchor pa;
		Hashtable fonts;
		internal PdfFonts(PdfAnchor a)
		{
			pa = a;
			fonts = new Hashtable();
		}

		internal Hashtable Fonts
		{
			get { return fonts; }
		}

		internal string GetPdfFont(string facename)
		{
			PdfFontEntry fe = (PdfFontEntry) fonts[facename];
			if (fe != null)
				return fe.font;

			string name = "F" + (fonts.Count + 1).ToString();
			fe = new PdfFontEntry(pa, name, facename);
			fonts.Add(facename, fe);
			return fe.font;
		}

		internal string GetPdfFont(StyleInfo si)
		{
			string face = FontNameNormalize(si.FontFamily);
            if (face == "Times-Roman")
            {
                if (si.IsFontBold() && si.FontStyle == FontStyleEnum.Italic)
                    face = "Times-BoldItalic";
                else if (si.IsFontBold())
                    face = "Times-Bold";
                else if (si.FontStyle == FontStyleEnum.Italic)
                    face = "Times-Italic";
            }
			else if (si.IsFontBold() && 
				si.FontStyle == FontStyleEnum.Italic)	// bold and italic?
				face = face + "-BoldOblique";
			else if (si.IsFontBold())			// just bold?
				face = face + "-Bold";
			else if (si.FontStyle == FontStyleEnum.Italic)
				face = face + "-Oblique";

			return GetPdfFont(face);
		}
		
		internal string FontNameNormalize(string face)
		{
			string faceName;
			switch (face.ToLower())
			{
                case "times":
                case "times-roman":
                case "times roman":
                case "timesnewroman":
				case "times new roman":
				case "timesnewromanps":
				case "timesnewromanpsmt":
				case "serif":
					faceName = "Times-Roman";
					break;
				case "helvetica":
				case "arial":
				case "arialmt":
				case "sans-serif":
                case "sans serif":
                default:
					faceName = "Helvetica";
					break;
				case "courier":
				case "couriernew":
				case "courier new":
				case "couriernewpsmt":
				case "monospace":
					faceName = "Courier";
					break;
				case "symbol":
					faceName = "Symbol";
					break;
				case "zapfdingbats":
                case "wingdings":
                case "wingding":
                    faceName = "ZapfDingbats";
					break;
			}
			return faceName;
		}
		/// <summary>
		/// Gets the font entries to be written to the file
		/// </summary>
		/// <returns></returns>
		internal byte[] GetFontDict(long filePos,out int size)
		{
			MemoryStream ms=new MemoryStream();
			int s;
			byte[] ba;
			foreach (PdfFontEntry fe in fonts.Values)
			{
				ba = fe.GetUTF8Bytes(fe.fontDict, filePos, out s);
				filePos += s;
				ms.Write(ba, 0, ba.Length);
			}
			
			ba = ms.ToArray();
			size = ba.Length;
			return ba;
		}
	}

	/// <summary>
	///Represents a font entry used in a pdf page
	/// </summary>
	internal class PdfFontEntry:PdfBase
	{
		internal string fontDict;
		internal string font;

		/// <summary>
		/// Create the font Dictionary
		/// </summary>
		internal PdfFontEntry(PdfAnchor pa,string fontName,string fontFace):base(pa)
		{
			font=fontName;
            fontDict = string.Format("\r\n{0} 0 obj<</Type/Font/Name /{1}/BaseFont/{2}/Subtype/Type1/Encoding /WinAnsiEncoding>>\tendobj\t",
                this.objectNum, fontName, fontFace);
            //fontDict = string.Format("\r\n{0} 0 obj<</Type/Font/Name /{1}/BaseFont/{2}/Subtype/Type1>>\tendobj\t",
            //    this.objectNum, fontName, fontFace);
        }
		/// <summary>
		/// Get the font entry to be written to the file
		/// </summary>
		/// <returns></returns>
		internal byte[] GetFontDict(long filePos,out int size)
		{
			return this.GetUTF8Bytes(fontDict,filePos,out size);
		}

	}
}
