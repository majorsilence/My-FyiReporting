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
    class DrawBase
    {
        protected const float SCALEFACTOR = 72f / 96f;
        protected System.Collections.Hashtable ObjectTable;
        protected Single X;
        protected Single Y;
        protected Single Width;
        protected Single Height;
        protected List<PageItem> items;

        protected static BorderStyleEnum getLineStyle(Pen p)
        {
            BorderStyleEnum ls = BorderStyleEnum.Solid;
            switch (p.DashStyle)
            {               
                case DashStyle.Dash:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.DashDot:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.DashDotDot:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.Dot: 
                    ls = BorderStyleEnum.Dotted;
                    break;
                case DashStyle.Solid:
                    ls = BorderStyleEnum.Solid;
                    break;
                case DashStyle.Custom:
                    ls = BorderStyleEnum.Solid;
                    break;
                default:                   
                    break;
            }  
            return ls;
        }

       
    }

    
}
