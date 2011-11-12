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
using fyiReporting.RDL;
using System.IO;
using System.Collections;

namespace fyiReporting.RDL
{
	
	///<summary>
	///The primary class to "run" a report to the supported output presentation types
	///</summary>

	public enum OutputPresentationType
	{
		HTML,
		PDF,
		XML,
		ASPHTML,
		Internal,
		MHTML,
        CSV,
        RTF,
        Excel,
        TIF,
        TIFBW                   // black and white tif
	}

	[Serializable]
	public class ProcessReport
	{
		Report r;					// report
		IStreamGen _sg;

		public ProcessReport(Report rep, IStreamGen sg)
		{
			if (rep.rl.MaxSeverity > 4)
				throw new Exception("Report has errors.  Cannot be processed.");

			r = rep;
			_sg = sg;
		}

		public ProcessReport(Report rep)
		{
			if (rep.rl.MaxSeverity > 4)
				throw new Exception("Report has errors.  Cannot be processed.");

			r = rep;
			_sg = null;
		}

		// Run the report passing the parameter values and the output
		public void Run(IDictionary parms, OutputPresentationType type)
		{
			r.RunGetData(parms);

			r.RunRender(_sg, type);

			return;
		}

	}
}
