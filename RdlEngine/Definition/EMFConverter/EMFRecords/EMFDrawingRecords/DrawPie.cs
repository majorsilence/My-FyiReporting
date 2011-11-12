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
    internal class DrawPie : DrawBase
    {
        internal DrawPie(Single Xin, Single Yin, Single WidthIn, Single HeightIn, System.Collections.Hashtable ObjectTableIn)
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
                //Byte 1 will be ObjectID
                byte ObjectID = _fr.ReadByte();
                //Byte 2 is the real flags
                byte RealFlags = _fr.ReadByte();
                // 0 1 2 3 4 5 6 7
                // X X X X X X C X                
                // if C = 1 Data int16 else float!
                bool Compressed = ((RealFlags & (int)Math.Pow(2, 6)) == (int)Math.Pow(2, 6));
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);

                Single StartAngle = _br.ReadSingle();
                Single SweepAngle = _br.ReadSingle();
                EMFPen EMFp = (EMFPen)ObjectTable[ObjectID];
                Pen p = EMFp.myPen;
                if (Compressed)
                {
                    DoCompressed(StartAngle,SweepAngle, _br, p);
                }
                else
                {
                    DoFloat(StartAngle, SweepAngle, _br, p);
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

        private void DoFloat(Single StartAngle,Single SweepAngle, BinaryReader _br, Pen p)
        {
            Single recX = _br.ReadSingle();
            Single recY = _br.ReadSingle();
            Single recWidth = _br.ReadSingle();
            Single recHeight = _br.ReadSingle();
            DoInstructions(recX, recY, recWidth, recHeight, p,StartAngle,SweepAngle);
            
        }

        private void DoCompressed(Single StartAngle,Single SweepAngle, BinaryReader _br, Pen p)
        {           
            Int16 recX = _br.ReadInt16();
            Int16 recY = _br.ReadInt16();
            Int16 recWidth = _br.ReadInt16();
            Int16 recHeight = _br.ReadInt16();
            DoInstructions(recX, recY, recWidth, recHeight, p, StartAngle, SweepAngle);
        }

        private void DoInstructions(Single recX, Single recY, Single recWidth, Single recHeight, Pen p,Single StartAngle,Single SweepAngle)
        {
         
            BorderStyleEnum ls = getLineStyle(p);
            switch (p.Brush.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)p.Brush;
                    PagePie pl = new PagePie();
                    pl.StartAngle = StartAngle;
                    pl.SweepAngle = SweepAngle;
                    pl.X = X + recX * SCALEFACTOR;
                    pl.Y = Y + recY * SCALEFACTOR;
                    pl.W = recWidth * SCALEFACTOR;
                    pl.H = recHeight * SCALEFACTOR;

                    StyleInfo SI = new StyleInfo();
                    SI.Color = theBrush.Color;
                    SI.BColorTop = SI.BColorBottom = SI.BColorLeft = SI.BColorRight = theBrush.Color;
                    SI.BStyleTop = SI.BStyleBottom = SI.BStyleLeft = SI.BStyleRight = ls;
                    SI.BWidthTop = SI.BWidthBottom = SI.BWidthLeft = SI.BWidthRight = p.Width * SCALEFACTOR;
                    pl.SI = SI;
                    items.Add(pl);
                    break;
                default:
                    break;
            }
        }
    }

}


