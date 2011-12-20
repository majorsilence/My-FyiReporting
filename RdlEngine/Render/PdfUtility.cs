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
using System.Text;
using System.Collections;
using System.IO;

namespace fyiReporting.RDL
{
	/// <summary>
	/// This class contains general Utility for the creation of pdf
	/// Creates the Header
	/// Creates XrefTable
	/// Creates the Trailer
	/// </summary>
    internal class PdfUtility
    {
        private int numTableEntries;
        PdfAnchor pa;
        internal PdfUtility(PdfAnchor p)
        {
            pa = p;
            numTableEntries = 0;
        }
        /// <summary>
        /// Creates the xref table using the byte offsets in the array.
        /// </summary>
        /// <returns></returns>
        internal byte[] CreateXrefTable(long fileOffset, out int size)
        {
            //Store the Offset of the Xref table for startxRef
            string table = null;
            try
            {
                ObjectList objList = new ObjectList(0, fileOffset);
                pa.offsets.Add(objList);
                pa.offsets.Sort();
                numTableEntries = (int)pa.offsets.Count;
                table = string.Format("\r\nxref {0} {1}\r\n0000000000 65535 f\r\n", 0, numTableEntries);
                for (int entries = 1; entries < numTableEntries; entries++)
                {
                    ObjectList obj = pa.offsets[entries];
                    table += obj.offset.ToString().PadLeft(10, '0');
                    table += " 00000 n\r\n";
                }
            }
            catch (Exception e)
            {
                Exception error = new Exception(e.Message + " In Utility.CreateXrefTable()");
                throw error;
            }
            return GetUTF8Bytes(table, out size);
        }
        /// <summary>
        /// Returns the Header
        /// </summary>
        /// <param name="version"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal byte[] GetHeader(string version, out int size)
        {
            string header = string.Format("%PDF-{0}\r%{1}\r\n", version, "\x82\x82");
            return GetUTF8Bytes(header, out size);
        }
        /// <summary>
        /// Creates the trailer and return the bytes array
        /// </summary>
        /// <returns></returns>
        internal byte[] GetTrailer(int refRoot, int refInfo, out int size)
        {
            string trailer = null;
            string infoDict;
            try
            {
                if (refInfo > 0)
                {
                    infoDict = string.Format("/Info {0} 0 R", refInfo);
                }
                else
                    infoDict = "";
                //The sorted array will be already sorted to contain the file offset at the zeroth position
                ObjectList objList = pa.offsets[0];
                trailer = string.Format("trailer\n<</Size {0}/Root {1} 0 R {2}" +
                    ">>\r\nstartxref\r\n{3}\r\n%%EOF\r\n"
                    , numTableEntries, refRoot, infoDict, objList.offset);

                pa.Reset();
            }
            catch (Exception e)
            {
                Exception error = new Exception(e.Message + " In Utility.GetTrailer()");
                throw error;
            }

            return GetUTF8Bytes(trailer, out size);
        }
        /// <summary>
        /// Converts the string to byte array in utf 8 encoding
        /// </summary>
        /// <param name="str"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        static internal byte[] GetUTF8Bytes(string str, out int size)
        {
            try
            {
                //byte[] ubuf = Encoding.Unicode.GetBytes(str);
                //Encoding enc = Encoding.GetEncoding(1252);
                //byte[] abuf = Encoding.Convert(Encoding.Unicode, enc, ubuf);

                byte[] ubuf = Encoding.Unicode.GetBytes(str);
                Encoding enc = Encoding.GetEncoding(65001); // utf-8
                byte[] abuf = Encoding.Convert(Encoding.Unicode, enc, ubuf);
                
                size = abuf.Length;
                return abuf;
            }
            catch (Exception e)
            {
                Exception error = new Exception(e.Message + " In Utility.GetUTF8Bytes()");
                throw error;
            }
        }

        /**
        * If necessary, encodes characters in a string in UTF-16, with a
        * starting 0xfe 0xff marker. This is the flag within a PDF string to
        * interpret the byte sequence as a string of UTF-16-encoded
        * characters. If the String contains no characters outside the
        * Unicode U+0000 � U+00ff range, no UTF-16 transformation is needed,
        * and we simply return the same String, with the exception that some
        * characters are quoted, as required by PDF, and any non-prinable
        * US-ASCII characters are encoded as escaped octals.
        *
        * While this method performs a form of UTF-16 encoding, it is a
        * particular quoted form intended for use in FDF/PDF documents. The
        * returned String will contain only characters in the range U+0020
        * through U+007D. It is intended that only the lower octet of each
        * character be used for output into the FDF document.
        *
        * Also note that this method is intended to process an entire PDF
        * string at once. That is, you should pass it the entire PDF string,
        * from the first character after the opening paren through the last
        * character immediately before the closing paren. This is because it
        * may UTF-16-encode the string, which PDF requires must be done on a
        * whole string at a time.
        *
        * @param baseString The string before any escaping has been
        * performed.
        *
        * @return 'baseString' encoded as needed.
        */
        internal static String UTF16StringQuoter(String baseString)
        {
            // First, see whether we need to UTF-16-encode the String,
            // along the way performing any quoting that will be needed if it
            // does not.
            bool needsEncoding = false;
            // In many cases, the output string will be unchanged from the
            // input string, so preallocate a StringBuilder of the same
            // length.

            StringBuilder buffer = new StringBuilder(baseString.Length);
            for (int i = 0; i < baseString.Length; i++)
            {
                // Uncomment code below to actually use Unicode encoding; 
                //    commented because the coding doesn't work (because of font issues??)
                //if (baseString[i] > 0x00ff)
                //{
                //    needsEncoding = true;
                //    break;
                //}
                appendQuotedOctet(buffer, (byte)baseString[i]);
            }
            if (!needsEncoding)
            {
                return buffer.ToString();
            }
            // OK. We do need to UTF-16 encode it.
            // We'll need at most eight characters to encode each Unicode
            // character, including the beginning Unicode marker. Preallocate
            // for efficiency.
            buffer = new StringBuilder((baseString.Length + 1) * 8);
            // Start with the Unicode marker characters
            buffer.Append("\\376\\377"); // 0xfe 0xff
            for (int i = 0; i < baseString.Length; i++)
            {
                char character = baseString[i];
                if (character > 0x0000ffff)
                {
                    // What can we do?! I haven't implemented handling for
                    // Unicode characters greater than U+FFFF
                    //System.err.println("Ack! The input string had a 4-byte "
                    //+ "Unicode character in it!");
                    buffer.Append("\\000 "); // Represent the char with a space
                }
                else
                {
                    appendQuotedOctet(buffer, (byte)((character >> 8) & 0x00ff));
                    appendQuotedOctet(buffer, (byte)(character & 0x00ff));
                }
            }
            return buffer.ToString();
        }
        /**
        * Append to the specified StringBuffer the octet value, quoted
        * appropriately for inclusion in an FDF (PDF) document. The
        * algorithm used here is intended to match the observed behavior in
        * Adobe Acrobat itself. That is, the octet is represented as a
        * backslash-escaped octal value if it is a non-printable ASCII
        * character, otherwise it is represented as a literal character,
        * unless it is one of \, (, or ), in which case it is
        * backslash-escaped.
        *
        * We take an octet, here, rather than a character, since escaping
        * occurs a byte at a time. If you need to encode a character whose
        * Unicode value is greater than U+00ff, then you'll need to break the
        * character into byte-sized chunks, first (and you'll presumably need
        * to encode the entire String as UTF-16).
        *
        * @param buffer The StringBuffer to which the
        * appropriately-escaped character will be
        * appended.
        * @param octet The octet to be quoted and appended.
        */
        private static void appendQuotedOctet(StringBuilder buffer, byte octet)
        {
            if (octet == '\\' || octet == '(' || octet == ')')
            {
                // PDF requires that we quote \, (, and ) characters with
                // a backslash.
                buffer.Append('\\');
                buffer.Append((char)octet);
            }
            else if (octet < ' ' || octet > '~')
            {
                // Escape any other non-printable ASCII characters using
                // octal notation.
                //FIX
                if (octet != 172)
                {
                    buffer.Append(escapeOctetOctal(octet));
                }
                else
                {
                    buffer.Append("\\200");
                }
                //END FIX 
            }
            else
            {
                // Printable ASCII characters are inserted as literals.
                buffer.Append((char)octet);
            }
        }
        /**
        * Return a string which contains the backslash-escaped octal
        * representation of the provided octet. For example, the octet 0x43,
        * which might represent the ASCII character 'C', would be returned as
        * "\103".
        *
        * @param octet The octet to be quoted.
        *
        * @return The backslash-escaped octal representation of 'octet'.
        */
        public static String escapeOctetOctal(byte octet)
        {
            return "\\"
            + (char)(0x30 + ((octet >> 6) & 0x0003))
            + (char)(0x30 + ((octet >> 3) & 0x0007))
            + (char)(0x30 + (octet & 0x0007));
        }
    }
	internal class Ascii85Encode
	{
		byte [] bain;
		readonly uint width = 72;	// max characters per line
		uint pos;			// tracks # of characters put out in line
		uint tuple = 0;
		int count=0;
		StringWriter sw;

		internal Ascii85Encode(byte [] ba)
		{
			bain = ba;
		}

		override public string ToString()
		{
			sw = new StringWriter();
			tuple = 0;
			count = 0;

			sw.Write("<~");
			pos = 2;

			byte b;
			for (int i =0; i < bain.Length; i++)
			{
				b = bain[i];
				switch (count++) 
				{
					case 0:	tuple |= ((uint)b << 24); break;
					case 1: tuple |= ((uint)b << 16); break;
					case 2:	tuple |= ((uint)b <<  8); break;
					case 3:
						tuple |= b;
						if (tuple == 0) 
						{
							sw.Write('z');
							if (pos++ >= width) 
							{
								pos = 0;
								sw.Write('\n');
							}
						} 
						else
						{
							encode(tuple, count);
						}
						tuple = 0;
						count = 0;
						break;
				}
			}
			// handle some clean up at end of processing
			if (count > 0)
				encode(tuple, count);
			if (pos + 2 > width)
				sw.Write('\n');

			sw.Write("~>\n");

			string baout = sw.ToString();
			sw.Close();
			sw=null;
			return baout;
		}
		
		void encode(uint tuple, int count) 
		{
			int j;
			char[] buf = new char[5];
			int s = 0;
			j = 5;
			do 
			{
				buf[s++] = (char) (tuple % 85);
				tuple /= 85;
			} while (--j > 0);
			j = count;
			do 
			{
				sw.Write((char) (buf[--s] + '!'));	// '!' == 32 
				if (pos++ >= width) 
				{
					pos = 0;
					sw.Write('\n');
				}
			} while (j-- > 0);
		}
	}


	internal class AsciiHexEncode
	{
		byte [] bain;
		readonly int width = 72;	// max characters per line

		internal AsciiHexEncode(byte [] ba)
		{
			bain = ba;
		}

		override public string ToString()
		{
			StringWriter sw = new StringWriter();
			int pos=0;

			for (int i =0; i < bain.Length; i++)
			{
				if (pos >= width)
				{
					sw.Write('\n');
					pos = 0;
				}

				string t = Convert.ToString(bain[i], 16);
				if (t.Length == 1)
					t = "0" + t;
				sw.Write(t);
				pos += 2;
			}

			string baout = sw.ToString();
			sw.Close();
			sw=null;
			return baout;
		}
	}

}
