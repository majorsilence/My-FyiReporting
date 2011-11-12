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
    internal class DrawCurve : DrawBase
    {
        internal DrawCurve(Single Xin, Single Yin, Single WidthIn, Single HeightIn, System.Collections.Hashtable ObjectTableIn)
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
                // if C = 1 Points are int16, 0 = Points are Float
              
                bool Compressed = ((RealFlags & (int)Math.Pow(2, 6)) == (int)Math.Pow(2, 6));
                             
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);

                float  Tension = _br.ReadSingle();// _br.ReadUInt32(); //20081110 PJR & GJL EMF+ Definition is WRONG!!!
                UInt32 Offset = _br.ReadUInt32();
                UInt32 NumberOfPoints = _br.ReadUInt32();
                UInt32 NumberOfSegments = _br.ReadUInt32();
                EMFPen Emfp = (EMFPen)ObjectTable[ObjectID];
                Pen p = Emfp.myPen;
                //EMFBrush b = p.myBrush;                
                if (Compressed)
                {
                    DoCompressed(NumberOfPoints, _br, p,Offset,NumberOfSegments, Tension);
                }               
                else
                {
                    DoFloat(NumberOfPoints, _br, p, Offset, NumberOfSegments, Tension);
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

        private void DoFloat(UInt32 NumberOfSegments, BinaryReader _br, Pen p, UInt32 Offset, UInt32 NumberOfPoints, float Tension)
        {
            System.Drawing.PointF[] Points = null;
            //bool first = true;
            for (int i = 0; i < NumberOfPoints; i++)
            {
                Points[i].X = _br.ReadSingle();
                Points[i].Y = _br.ReadSingle();
            }
            DoInstructions(Points, p, Offset, NumberOfPoints, Tension);
        }


        private void DoCompressed(UInt32 NumberOfSegments, BinaryReader _br, Pen p, UInt32 Offset, UInt32 NumberOfPoints, float Tension)
        {
            //bool first = true;
            PointF[] Points = new System.Drawing.PointF[NumberOfPoints];
            for (int i = 0; i < NumberOfPoints; i++)
            {
                Points[i].X = (float) _br.ReadInt16();
                Points[i].Y = (float) _br.ReadInt16();
            }
            DoInstructions(Points, p, Offset, NumberOfSegments, Tension);
        }

        private void DoInstructions(PointF[] points, Pen p, UInt32 Offset, UInt32 NumberOfPoints, float Tension)
        {
            BorderStyleEnum ls = getLineStyle(p);
            //Well we only draw lines at the moment.... 

            switch (p.Brush.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)p.Brush;
                    PageCurve pc = new PageCurve();
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i].X = X + points[i].X * SCALEFACTOR;
                        points[i].Y = Y + points[i].Y * SCALEFACTOR;
                    }
                    pc.Points = points;
                    pc.Offset = (int) Offset;
                    pc.Tension = Tension;
                    

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
                    pc.SI = SI;
                    items.Add(pc);
                    break;
                default:
                    break;
            }
        }
    }
}
