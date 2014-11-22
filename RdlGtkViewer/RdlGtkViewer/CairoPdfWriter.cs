// 
//  CairoPdfWriter.cs
//  
//  Author:
//       Krzysztof Marecki 
//
// Copyright (c) 2010-2011 Krzysztof Marecki 
//
// This file is part of the NReports project
// This file is part of the My-FyiReporting project 
//	
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.IO;
using fyiReporting.RDL;


namespace fyiReporting.RdlGtkViewer
{
    public class CairoPdfWriter
    {
        public CairoPdfWriter()
        {
        }

        #region IPdfWriter implementation

        public byte[] GetFileBytes(Report report)
        {
            var pages = report.BuildPages();
            int width = (int)report.PageWidthPoints;
            int height = (int)report.PageHeightPoints;
            string filename = string.Format("gen-{0}.pdf", Guid.NewGuid());
			
            try
            {
                using (Cairo.PdfSurface pdf = new Cairo.PdfSurface(filename, width, height))
                {
                    using (Cairo.Context g = new Cairo.Context(pdf))
                    {
						
                        var render = new  fyiReporting.RdlGtkViewer.RenderCairo(g);
                        render.RunPages(pages);
                    }
                }
                byte[] bytes = File.ReadAllBytes(filename);
                return bytes;
				
            }
            finally
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }

        #endregion
    }
}

