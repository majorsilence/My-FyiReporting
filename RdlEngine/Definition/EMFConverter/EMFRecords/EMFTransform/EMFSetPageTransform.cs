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

namespace fyiReporting.RDL
{
     internal class EMFSetPageTransform
    {
        internal System.Drawing.GraphicsUnit PageUnit;
        internal bool postMultiplyTransform;
        internal Single PageScale;
        internal static EMFSetPageTransform getTransform(int flags, byte[] RecordData)
        {
            return new EMFSetPageTransform(flags, RecordData);
        }

        internal EMFSetPageTransform(int flags, byte[] RecordData)
        {
            MemoryStream _fs = null;
            BinaryReader _fr = null;
            try
            {
                _fs = new MemoryStream(BitConverter.GetBytes(flags));
                _fr = new BinaryReader(_fs);

                //PageUnit...
                UInt16 PageU = _fr.ReadByte();
                PageUnit = (System.Drawing.GraphicsUnit)PageU;

                UInt16 RealFlags = _fr.ReadByte();
                //XXXXXAXX - if A = 1 the transform matrix is post-multiplied else pre-multiplied...
                //01234567
                postMultiplyTransform = ((RealFlags & (UInt16)Math.Pow(2, 5)) == Math.Pow(2, 5));
                PageScale = BitConverter.ToSingle(RecordData, 0);
                
            }
            finally
           {
               if (_fr != null)
                   _fr.Close();
               if (_fs != null)
                   _fs.Dispose();
               
           }
        }
    }
}
