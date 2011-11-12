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

namespace fyiReporting.RDL
{
    internal enum EMFBrushType
    {
        SolidColor = 0x00000000,
        HatchFill = 0x00000001,
        TextureFill = 0x00000002,
        PathGradient = 0x00000003,
        LinearGradient = 0x00000004
    }

    class EMFBrush : EMFRecordObject
    {
        //public EMFBrushType BrushType;
        public Brush myBrush;
        
        internal EMFBrush()
        {
            ObjectType = EmfObjectType.brush;
        }

        internal static EMFBrush getEMFBrush(byte[] RecordData)
        {
            return ProcessBrush(RecordData);
        }

        internal static EMFBrush ProcessBrush(byte[] RecordData)
        {
            //put the Data into a stream and use a binary reader to read the data
            MemoryStream _ms = null;
            BinaryReader _br = null;
            try
            {
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
                UInt32 Version = _br.ReadUInt32();
                UInt32 BrushType = _br.ReadUInt32();

                switch (BrushType)
                {
                    case (UInt32)EMFBrushType.SolidColor:                       
                        return new EmfSolidBrush(_br);
                       
                    case (UInt32)EMFBrushType.HatchFill:
                        return new EmfHatchFillBrush(_br);                      
                      
                    case (UInt32)EMFBrushType.TextureFill:
                        throw new NotSupportedException("TextureFill brush Not Supported Yet!");
                        
                    case (UInt32)EMFBrushType.PathGradient:
                        throw new NotSupportedException("PathGradient brush Not Supported Yet!");
                       
                    case (UInt32)EMFBrushType.LinearGradient:
                        return new EmfLinearGradientBrush(_br);
                       
                }
                return null;
            }
            finally
            {
                if (_br != null)
                    _br.Close();
                if (_ms != null)
                    _ms.Dispose();            
            }
        }
    }   
    
    internal class EmfLinearGradientBrush : EMFBrush
    {
        public EmfLinearGradientBrush(BinaryReader _br)
        {
            UInt32 BrushFlags = _br.ReadUInt32();
            Int32 WrapMode = _br.ReadInt32();
            Single X = _br.ReadSingle();
            Single Y = _br.ReadSingle();
            Single Width = _br.ReadSingle();
            Single Height = _br.ReadSingle();
            RectangleF rf = new RectangleF(X,Y,Width,Height);

            byte sA, sR, sG, sB;
            sB = _br.ReadByte();
            sG = _br.ReadByte();
            sR = _br.ReadByte();
            sA = _br.ReadByte();

            byte eA, eR, eG, eB;
            eB = _br.ReadByte();
            eG = _br.ReadByte();
            eR = _br.ReadByte();
            eA = _br.ReadByte();

            _br.ReadBytes(8);            
            System.Drawing.Drawing2D.LinearGradientBrush tmpB = new System.Drawing.Drawing2D.LinearGradientBrush(rf, Color.FromArgb(sA, sR, sG, sB), Color.FromArgb(eA, eR, eG, eB), 0f);
            tmpB.WrapMode = (System.Drawing.Drawing2D.WrapMode)WrapMode;
            

            bool BrushDataTransform = ((BrushFlags & 0x00000002) == 0x00000002);
            bool BrushDataPresetColors = ((BrushFlags & 0x00000002) == 0x00000004);
            bool BrushDataBlendFactorsH = ((BrushFlags & 0x00000002) == 0x00000008);
            bool BrushDataBlendFactorsV = ((BrushFlags & 0x00000002) == 0x00000010);
            bool BrushDataIsGammaCorrected = ((BrushFlags & 0x00000002) == 0x00000080);

            tmpB.GammaCorrection = BrushDataIsGammaCorrected;

            if (BrushDataTransform)
            {
                _br.ReadBytes(24); //Transform matrix (ignored for now)  
            }

            if (BrushDataPresetColors || BrushDataBlendFactorsH || BrushDataBlendFactorsV)
            {
               //there must be a blend pattern
                if (BrushDataPresetColors)
                {
                    //blend colors object
                }
                else
                {
                    if (!BrushDataBlendFactorsV)
                    {
                        //BlendFactors object on vertical (???)
                    }
                    else 
                    {
                        if (!BrushDataBlendFactorsH)
                        {
                            //Blendfactors on horizontal (???)
                        }
                        else
                        {
                            //BlendFactors on vertical and horizontal
                        }
                    }
                }
                
            }
            myBrush = tmpB;
        }
    }
    
    internal class EmfHatchFillBrush: EMFBrush
    {
    	
    	public EmfHatchFillBrush(BinaryReader _br)
    	{
    		Int32 HatchStyle = _br.ReadInt32();
    		
    		byte fA,fR,fG,fB;
            fB = _br.ReadByte();
            fG = _br.ReadByte();
            fR = _br.ReadByte();
            fA = _br.ReadByte(); 
            
            byte bA,bR,bG,bB;       
            bB = _br.ReadByte();
            bG = _br.ReadByte();
            bR = _br.ReadByte();
            bA = _br.ReadByte(); 
            
            myBrush = new System.Drawing.Drawing2D.HatchBrush((System.Drawing.Drawing2D.HatchStyle) HatchStyle,Color.FromArgb(fA,fR,fG,fB),Color.FromArgb(bA,bR,bG,bB));
            
            
    		
    	}
    }

    internal class EmfSolidBrush : EMFBrush
    {        
        public EmfSolidBrush(byte A, byte R, byte G, byte B)
        {            
            myBrush = new SolidBrush(Color.FromArgb(A, R, G, B));
        }
        public EmfSolidBrush(BinaryReader _br)
        {            
            byte A,R,G,B;       
            B = _br.ReadByte();
            G = _br.ReadByte();
            R = _br.ReadByte();
            A = _br.ReadByte();   
            myBrush = new SolidBrush(Color.FromArgb(A,R,G,B));
        }
    }
}
