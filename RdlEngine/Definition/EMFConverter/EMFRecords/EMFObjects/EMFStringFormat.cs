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
   
    internal class EMFStringFormat : EMFRecordObject
    {
        public StringFormat myStringFormat;
       
        private EMFStringFormat(byte[] RecordData)        
        {
            ObjectType = EmfObjectType.stringformat;
            myStringFormat = new System.Drawing.StringFormat();
            //put the Data into a stream and use a binary reader to read the data
            MemoryStream _ms = new MemoryStream(RecordData);
            BinaryReader _br = new BinaryReader(_ms);           
            _br.ReadUInt32(); //Who cares about version..not me!            
            myStringFormat.FormatFlags = (StringFormatFlags)_br.ReadUInt32();
            _br.ReadBytes(4);//Language...Ignore for now!
            myStringFormat.LineAlignment = (StringAlignment)_br.ReadUInt32();
            myStringFormat.Alignment = (StringAlignment)_br.ReadUInt32();
            UInt32 DigitSubstitutionMethod = _br.ReadUInt32();
            UInt32 DigitSubstitutionLanguage = _br.ReadUInt32();
            myStringFormat.SetDigitSubstitution((int)DigitSubstitutionLanguage, (StringDigitSubstitute)DigitSubstitutionMethod);
            
            Single FirstTabOffSet = _br.ReadSingle();

            myStringFormat.HotkeyPrefix = (System.Drawing.Text.HotkeyPrefix) _br.ReadInt32();

             _br.ReadSingle();//leading Margin
             _br.ReadSingle();//trailingMargin           
             _br.ReadSingle();//tracking
            myStringFormat.Trimming = (StringTrimming)_br.ReadUInt32();           
            Int32 TabStopCount = _br.ReadInt32();
            Int32 RangeCount = _br.ReadInt32();
            //Next is stringformatdata...
            Single[] TabStopArray;           
            System.Drawing.CharacterRange[] RangeArray;

            if (TabStopCount > 0)
            {
                TabStopArray = new Single[TabStopCount];
                for (int i = 0; i < TabStopCount; i++)
                {
                    TabStopArray[i] = _br.ReadSingle();
                }
                myStringFormat.SetTabStops(FirstTabOffSet, TabStopArray);
            }

            if (RangeCount > 0)
            {
                RangeArray = new System.Drawing.CharacterRange[RangeCount];
                for (int i = 0; i < RangeCount; i++)
                {
                    RangeArray[i].First = _br.ReadInt32();
                    RangeArray[i].Length = _br.ReadInt32();
                }
                myStringFormat.SetMeasurableCharacterRanges(RangeArray);
            }
        }


        internal static EMFStringFormat getEMFStringFormat(byte[] RecordData)
        {
            return new EMFStringFormat(RecordData);         
        }      
               
    }
}
