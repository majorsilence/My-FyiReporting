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
namespace fyiReporting.RDL
{       
    internal class FillRects : DrawBase
    {
        internal FillRects(Single Xin, Single Yin, Single WidthIn, Single HeightIn,System.Collections.Hashtable ObjectTableIn)
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
                _fr.ReadByte();
                //Byte 2 is the real flags
                byte RealFlags = _fr.ReadByte();
                // 0 1 2 3 4 5 6 7
                // X X X X X X C S                
                // if C = 1 Data int16 else float!
                bool Compressed = ((RealFlags & (int)Math.Pow(2, 6)) == (int)Math.Pow(2, 6));
                bool BrushIsARGB = ((RealFlags & (int)Math.Pow(2, 7)) == (int)Math.Pow(2, 7));
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
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
                UInt32 NumberOfRects = _br.ReadUInt32();
                if (Compressed)
                {
                    DoCompressed(NumberOfRects, _br, b);
                }
                else
                {
                    DoFloat(NumberOfRects, _br, b);
                }
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

        private void DoFloat(UInt32 NumberOfRects, BinaryReader _br, Brush b)
        {
            for (int i = 0; i < NumberOfRects; i++)
            {
                Single recX = _br.ReadSingle();
                Single recY = _br.ReadSingle();
                Single recWidth = _br.ReadSingle();
                Single recHeight = _br.ReadSingle();
                DoInstructions(recX, recY, recWidth, recHeight, b);
            }
        }

        private void DoCompressed(UInt32 NumberOfRects, BinaryReader _br, Brush b)
        {
            for (int i = 0; i < NumberOfRects; i++)
            {
                Int16 recX = _br.ReadInt16();
                Int16 recY = _br.ReadInt16();
                Int16 recWidth = _br.ReadInt16();
                Int16 recHeight = _br.ReadInt16();
                DoInstructions(recX, recY, recWidth, recHeight, b);
            }
        }

        private void DoInstructions(Single recX, Single recY, Single recWidth, Single recHeight, Brush b)
        {
            PageRectangle pl = new PageRectangle();
            StyleInfo SI = new StyleInfo();            
    		pl.X = X + recX * SCALEFACTOR;
            pl.Y = Y + recY * SCALEFACTOR;
            pl.W = recWidth * SCALEFACTOR;
            pl.H = recHeight * SCALEFACTOR; 
                    
            switch (b.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)b;
                    SI.Color = theBrush.Color;                    
                    SI.BackgroundColor = theBrush.Color;
                    break;
                case "LinearGradientBrush":
                    System.Drawing.Drawing2D.LinearGradientBrush linBrush = (System.Drawing.Drawing2D.LinearGradientBrush)b;
                    SI.BackgroundGradientType = BackgroundGradientTypeEnum.LeftRight;
                    SI.BackgroundColor = linBrush.LinearColors[0];
                    SI.BackgroundGradientEndColor = linBrush.LinearColors[1];                    
                    break;
                case "HatchBrush":
                    System.Drawing.Drawing2D.HatchBrush hatBrush = (System.Drawing.Drawing2D.HatchBrush) b;
                    SI.BackgroundColor = hatBrush.BackgroundColor;
                    SI.Color = hatBrush.ForegroundColor;                    
                    SI.PatternType = StyleInfo.GetPatternType(hatBrush.HatchStyle);                    
                    break;
                default:
                    break;
            }
            
            pl.SI = SI;
            items.Add(pl);
        }
    }
}


