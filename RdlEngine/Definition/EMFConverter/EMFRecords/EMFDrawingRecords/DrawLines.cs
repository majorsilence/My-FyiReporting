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
    //takes the record data and returns the instructions for drawing...I guess.    
    internal class DrawLines : DrawBase
    {
        internal DrawLines(Single Xin, Single Yin, Single WidthIn, Single HeightIn,System.Collections.Hashtable ObjectTableIn)
        {
            X = Xin;
            Y = Yin;
            Width = WidthIn;
            Height = HeightIn;          
            ObjectTable = ObjectTableIn;
            items = new List<PageItem>();
        }

        public List<PageItem> Process(int Flags,byte[] RecordData)
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
                // X X X P X L C X
                // if P = 1 Points are Relative to previous point (Ignore C)
                // if C = 1 Points are int16, 0 = Points are Float (ignore P)
                // if L = 1 Draw a line between last and first points
                bool Compressed = ((RealFlags & (int)Math.Pow(2, 6)) == (int)Math.Pow(2, 6));
                bool Relative = false;
                if (!Compressed)                
                    Relative = ((RealFlags & (int)Math.Pow(2, 3)) == (int)Math.Pow(2, 3));
                bool CloseShape = ((RealFlags & (int)Math.Pow(2, 5)) == (int)Math.Pow(2, 5)); 
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
                UInt32 NumberOfPoints = _br.ReadUInt32(); 
                EMFPen Emfp = (EMFPen)ObjectTable[ObjectID];
                Pen p = Emfp.myPen;               
                //EMFBrush b = p.myBrush;                
                if (Compressed)
                {
                    DoCompressed(NumberOfPoints, _br,p);
                }
                else if (Relative)
                {
                    DoRelative(NumberOfPoints, _br, p);
                }
                else
                {
                    DoFloat(NumberOfPoints, _br, p);
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

        private void DoFloat(UInt32 NumberOfPoints, BinaryReader _br, Pen p)
        {
            System.Drawing.PointF[] Points = new System.Drawing.PointF[NumberOfPoints];
            bool first = true;
            for (int i = 0; i < NumberOfPoints; i++)
            {
                Points[i].X = _br.ReadSingle();
                Points[i].Y = _br.ReadSingle();
            }
            PointF PointA = new PointF();
            PointF PointB = new PointF();
            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (first)
                {
                    PointA = Points[i];
                    first = false;
                }
                else
                {
                    PointB = Points[i];
                    DoInstructions(PointA.X, PointB.X, PointA.Y, PointB.Y,p);
                    PointA = PointB;
                }
            }
        }

        private void DoRelative(UInt32 NumberOfPoints, BinaryReader _br, Pen p)
        {
            throw new NotSupportedException("Relative Points are not supported");
        }

        private void DoCompressed(UInt32 NumberOfPoints, BinaryReader _br, Pen p)
        {
            bool first = true;
            Point[] Points = new System.Drawing.Point[NumberOfPoints];            
            for (int i = 0; i < NumberOfPoints; i++)
            {
                Points[i].X = _br.ReadInt16();
                Points[i].Y = _br.ReadInt16();
            }
            Point PointA= new Point();
            Point PointB = new Point();
            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (first)
                {
                    PointA = Points[i];
                    first = false;
                }
                else
                {
                    PointB = Points[i];
                    DoInstructions(PointA.X, PointB.X, PointA.Y, PointB.Y, p);
                    PointA = PointB;
                }
            }
        }

        private void DoInstructions(Single Xa,Single Xb, Single Ya, Single Yb,Pen p)
        {
            BorderStyleEnum ls = getLineStyle(p);
           
          
            switch (p.Brush.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)p.Brush;
                    PageLine pl = new PageLine();
                    pl.X = X + Xa * SCALEFACTOR;
                    pl.Y = Y + Ya * SCALEFACTOR;
                    pl.X2 = X + Xb * SCALEFACTOR;
                    pl.Y2 = Y + Yb * SCALEFACTOR;                    
                    
                    StyleInfo SI = new StyleInfo();
                    SI.Color = theBrush.Color;
                    SI.BColorTop = theBrush.Color;
                    SI.BStyleTop = ls;
                    SI.BWidthTop = p.Width * SCALEFACTOR;
                    SI.BColorBottom = theBrush.Color;
                    SI.BStyleBottom = ls;
                    SI.BWidthBottom = p.Width * SCALEFACTOR;
                    SI.BColorLeft = theBrush.Color;
                    SI.BStyleLeft = ls;
                    SI.BWidthLeft = p.Width * SCALEFACTOR;
                    SI.BColorRight = theBrush.Color;
                    SI.BStyleRight = ls;
                    SI.BWidthRight = p.Width * SCALEFACTOR; 
                    pl.SI = SI;
                    items.Add(pl);
                    break;
                default:
                    break;
            }
        }
    }
}
