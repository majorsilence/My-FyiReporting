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
using System.Xml;
using System.Reflection;

namespace fyiReporting.RDL
{
	///<summary>
	/// CodeModule definition and processing.
	///</summary>
	[Serializable]
	internal class CodeModule : ReportLink
	{
		string _CodeModule;	// Name of the code module to load
		[NonSerialized] Assembly _LoadedAssembly=null;	// 
		[NonSerialized] bool bLoadFailed=false;
	
		internal CodeModule(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_CodeModule=xNode.InnerText;
            //Added from Forums, User: Solidstore http://www.fyireporting.com/forum/viewtopic.php?t=905
            if (!_CodeModule.Contains(","))
            { // if not a full assembly reference 
                if ((!_CodeModule.ToLower().EndsWith(".dll")) && ((!_CodeModule.ToLower().EndsWith(".exe"))))
                { // check .dll ending 
                _CodeModule += ".dll"; 
                }
            }
		}

		internal Assembly LoadedAssembly()
		{
			if (bLoadFailed)		// We only try to load once.
				return null;

			if (_LoadedAssembly == null)
			{
				try
				{
					_LoadedAssembly = XmlUtil.AssemblyLoadFrom(_CodeModule);
				}
				catch (Exception e)
				{
					OwnerReport.rl.LogError(4, String.Format("CodeModule {0} failed to load.  {1}",
						_CodeModule, e.Message));
					bLoadFailed = true;
				}
			}
			return _LoadedAssembly;
		}

		override internal void FinalPass()
		{
			return;
		}

		internal string CdModule
		{
			get { return  _CodeModule; }
			set {  _CodeModule = value; }
		}
	}
}
