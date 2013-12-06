/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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
using System.Xml;
using RdlEngine.Resources;


namespace fyiReporting.RDL
{
	///<summary>
	/// Error logging (parse and runtime) within report.
	///</summary>
	[Serializable]
	internal class ReportLog
	{
		List<string> _ErrorItems=null;			// list of report items
		int _MaxSeverity=0;				// maximum severity encountered				

		internal ReportLog()
		{
		}

		internal ReportLog(ReportLog rl)
		{
			if (rl != null && rl.ErrorItems != null)
			{
				_MaxSeverity = rl.MaxSeverity;
                _ErrorItems = new List<string>(rl.ErrorItems);
			}
		}

		internal void LogError(ReportLog rl)
		{
			if (rl.ErrorItems.Count == 0)
				return;
			LogError(rl.MaxSeverity, rl.ErrorItems);
		}

		internal void LogError(int severity, string item)
		{
			if (_ErrorItems == null)			// create log if first time
                _ErrorItems = new List<string>();

			if (severity > _MaxSeverity)
				_MaxSeverity = severity;

			var msg = Strings.ReportLog_Error_Severity + ": " + Convert.ToString(severity) + " - " + item;

			_ErrorItems.Add(msg);

			if (severity >= 12)		
				throw new Exception(msg);		// terminate the processing
		}

		internal void LogError(int severity, List<string> list)
		{
			if (_ErrorItems == null)			// create log if first time
                _ErrorItems = new List<string>();

			if (severity > _MaxSeverity)
				_MaxSeverity = severity;

			_ErrorItems.AddRange(list);
		}

		internal void Reset()
		{
			_ErrorItems=null;
			if (_MaxSeverity < 8)				// we keep the severity to indicate we can't run report
				_MaxSeverity=0;
		}

        internal List<string> ErrorItems
		{
			get { return  _ErrorItems; }
		}


		internal int MaxSeverity
		{
			get { return  _MaxSeverity; }
			set {  _MaxSeverity = value; }
		}
	}
}
