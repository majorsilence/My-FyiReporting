/* ====================================================================

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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
    //takes the record data and returns the instructions for drawing...I guess.    
    internal class DrawString : DrawBase
    {
        internal DrawString(Single Xin, Single Yin, Single WidthIn, Single HeightIn,System.Collections.Hashtable ObjectTableIn)
        {
            X = Xin;
            Y = Yin;
            Width = WidthIn;
            Height = HeightIn;           
            ObjectTable = ObjectTableIn;
            items = new List<PageItem>();
        }

        public List<PageItem> Process(int Flags, byte[] RecordData)
        {
            MemoryStream _ms = null;
            BinaryReader _br = null;
            MemoryStream _fs = null;
            BinaryReader _fr = null;
            try
            {               
                _fs = new MemoryStream(BitConverter.GetBytes(Flags));
                _fr = new BinaryReader(_fs);
                //Byte 1 will be ObjectID - a font in the object table
                byte ObjectID = _fr.ReadByte();
                //Byte 2 is the real flags
                byte RealFlags = _fr.ReadByte();
                // 0 1 2 3 4 5 6 7
                // X X X X X X X S
                // if S = type of brush - if S then ARGB, else a brush object in object table

                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
                bool BrushIsARGB = ((RealFlags & (int)Math.Pow(2, 7)) == (int)Math.Pow(2, 7));
                Brush b;
                if (BrushIsARGB)
                {
                    byte A, R, G, B;
                    B = _br.ReadByte();
                    G = _br.ReadByte();
                    R = _br.ReadByte();
                    A = _br.ReadByte();
                    b = new SolidBrush(Color.FromArgb(A, R, G, B));
                }
                else
                {
                    UInt32 BrushID = _br.ReadUInt32();
                    EMFBrush EMFb = (EMFBrush)ObjectTable[(byte)BrushID];
                    b = EMFb.myBrush;
                }

                UInt32 FormatID = _br.ReadUInt32(); // Index of Optional stringFormatobject in Object Table...
                UInt32 StringLength = _br.ReadUInt32();
                //bounding of string...
                Single recX = _br.ReadSingle();
                Single recY = _br.ReadSingle();
                Single recWidth = _br.ReadSingle();
                Single recHeight = _br.ReadSingle();
                //Array of Chars...                 
                char[] StringData = new char[StringLength];
                System.Text.UnicodeEncoding d = new System.Text.UnicodeEncoding();
                d.GetChars(_br.ReadBytes((int)StringLength * 2), 0, (int)StringLength * 2, StringData, 0);
                EMFFont EF = (EMFFont)ObjectTable[(byte)ObjectID];
                Font f = EF.myFont;
                StringFormat sf;
                if (ObjectTable.Contains((byte)FormatID))
                {
                    EMFStringFormat ESF = (EMFStringFormat)ObjectTable[(byte)FormatID];
                    sf = ESF.myStringFormat;
                }
                else
                {
                    sf = new StringFormat();
                }

                DoInstructions(f, sf, b, recX, recY, recWidth, recHeight, new String(StringData));
                return items;
            }
            finally
            {
                if (_br != null)
                    _br.Close();
                if (_ms != null)
                    _ms.Dispose();
                if (_fr != null)
                    _fr.Close();
                if (_fs != null)
                    _fs.Dispose();
            }
        }

        private void DoInstructions(Font f, StringFormat sf, Brush br, Single recX, Single recY, Single recWidth, Single recHeight, String Text)
        {
            Color Col = Color.Black;
            if (br.GetType().Name.Equals("SolidBrush"))
            {
                SolidBrush sb = (SolidBrush)br;
                Col = sb.Color;
            }

            PageText pt = new PageText(Text);

            pt.X = X + recX * SCALEFACTOR;
            pt.Y = Y + recY * SCALEFACTOR;
            pt.W = recWidth * SCALEFACTOR;
            pt.H = recHeight * SCALEFACTOR;          
            StyleInfo SI = new StyleInfo();
            SI.Color = Col;           
            SI.Direction = DirectionEnum.LTR;
            SI.FontFamily = f.Name;
            SI.FontSize = f.Size * SCALEFACTOR; 
            if (f.Italic) {SI.FontStyle = FontStyleEnum.Italic;}
            if (f.Bold) { SI.FontWeight = FontWeightEnum.Bold; }
            if (f.Underline) { SI.TextDecoration = TextDecorationEnum.Underline; }
            if (sf.LineAlignment == StringAlignment.Center)
            {
                SI.TextAlign = TextAlignEnum.Center;
            }
            else if (sf.LineAlignment == StringAlignment.Far)
            {
                SI.TextAlign = TextAlignEnum.Right;
            }

            if (sf.Alignment == StringAlignment.Center)
            {
                SI.VerticalAlign = VerticalAlignEnum.Middle;
            }
            else if (sf.Alignment == StringAlignment.Far)
            {
                SI.VerticalAlign = VerticalAlignEnum.Bottom;
            }
            if ((sf.FormatFlags & StringFormatFlags.DirectionVertical) == StringFormatFlags.DirectionVertical)
            {
                SI.WritingMode = WritingModeEnum.tb_rl;
            }
            else
            {
                SI.WritingMode = WritingModeEnum.lr_tb;
            }
            pt.SI = SI;
            items.Add(pt);
            

            //r = Math.Round((r / 255), 3);
            //g = Math.Round((g / 255), 3);
            //b = Math.Round((b / 255), 3);

            //string pdfFont = fonts.GetPdfFont(f.Name);
            ////need a graphics object...
            //Bitmap bm = new Bitmap(1, 1);
            //Graphics gr = Graphics.FromImage(bm);
            //Font scaleFont = new Font(f.Name, (f.Size * ScaleY) * 1.5f, f.Style, f.Unit);
            //SizeF TextSize = gr.MeasureString(Text.Substring(0, Text.Length), scaleFont, (int)(recWidth * ScaleX), sf);
            //float textwidth = TextSize.Width;
            //float textHeight = TextSize.Height;
            //float startX = X + recX * ScaleX;
            //float startY = Y + Height - recY * ScaleY - (scaleFont.Size);
            //if ((sf.FormatFlags & StringFormatFlags.DirectionVertical) != StringFormatFlags.DirectionVertical)
            //{
            //    if (sf.LineAlignment == StringAlignment.Center)
            //    {
            //        startX = (startX + (recWidth * ScaleX) / 2) - (textwidth / 4);
            //    }
            //    else if (sf.LineAlignment == StringAlignment.Far)
            //    {
            //        startX = (startX + recWidth * ScaleX) - (textwidth / 1.8f);
            //    }
            //}
            //else
            //{
            //    startX += textwidth / 4;
            //    if (sf.LineAlignment == StringAlignment.Center)
            //    {
            //        startY = (startY - (recHeight * ScaleY) / 2) + (textHeight / 4);
            //    }
            //    else if (sf.LineAlignment == StringAlignment.Far)
            //    {
            //        startY = (startY - recHeight * ScaleY) + (textHeight / 1.8f);
            //    }
            //}

            //Lines.Append("\r\nq\t");

            //string newtext = PdfUtility.UTF16StringQuoter(Text);
            //if ((sf.FormatFlags & StringFormatFlags.DirectionVertical) != StringFormatFlags.DirectionVertical)
            //{
            //    Lines.AppendFormat(System.Globalization.NumberFormatInfo.InvariantInfo,
            //           "\r\nBT/{0} {1} Tf\t{5} {6} {7} rg\t{2} {3} Td \t({4}) Tj\tET\tQ\t",
            //           pdfFont, scaleFont.SizeInPoints, startX, startY, newtext, r, g, b);
            //}
            //else
            //{
            //    double rads = -283.0 / 180.0;
            //    double radsCos = Math.Cos(rads);
            //    double radsSin = Math.Sin(rads);

            //    Lines.AppendFormat(System.Globalization.NumberFormatInfo.InvariantInfo,
            //                "\r\nBT/{0} {1} Tf\t{5} {6} {7} rg\t{8} {9} {10} {11} {2} {3} Tm \t({4}) Tj\tET\tQ\t",
            //                pdfFont, scaleFont.SizeInPoints, startX, startY, newtext, r, g, b,
            //                radsCos, radsSin, -radsSin, radsCos);
            //}
        }
    }
}
