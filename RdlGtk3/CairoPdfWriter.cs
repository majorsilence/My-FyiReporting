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
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.RdlGtk3
{
    public class CairoPdfWriter
    {
        public CairoPdfWriter()
        {
        }

        #region IPdfWriter implementation

        public async Task<byte[]> GetFileBytes(Report report)
        {
            var pages = await report.BuildPages();
            int width = (int)report.PageWidthPoints;
            int height = (int)report.PageHeightPoints;
            string filename = $"gen-{Guid.NewGuid()}.pdf";
			
            try
            {
                using(var pdf = new Cairo.PdfSurface(filename, width, height))
				using(var g = new Cairo.Context(pdf))
				using(var render = new RenderCairo(g))
                {
					render.RunPages(pages);
                }
                
                byte[] bytes = await File.ReadAllBytesAsync(filename);
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

