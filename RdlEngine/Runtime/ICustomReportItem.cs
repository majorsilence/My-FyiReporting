/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
	/// <summary>
	/// ICustomReportItem defines the protocol for implementing a CustomReportItem
	/// </summary>

	public interface ICustomReportItem : IDisposable    
	{
        bool IsDataRegion();                            // Does CustomReportItem require DataRegions
        void DrawImage(System.Drawing.Bitmap bm);       // Draw the image in the passed bitmap; do SetParameters first
        void DrawDesignerImage(System.Drawing.Bitmap bm);   // Design time: Draw the designer image in the passed bitmap;
        void SetProperties(IDictionary<string, object> parameters); // Set the runtime properties
        object GetPropertiesInstance(XmlNode node);     // Design time: return class representing properties
        void SetPropertiesInstance(XmlNode node, object inst);  // Design time: given class representing properties set the XML custom properties
        string GetCustomReportItemXml();                // Design time: return string with <CustomReportItem> ... </CustomReportItem> syntax for the insert
    }

}
