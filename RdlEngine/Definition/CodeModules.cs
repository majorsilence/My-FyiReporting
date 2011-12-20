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
using System.Reflection;


namespace fyiReporting.RDL
{
	///<summary>
	/// CodeModules definition and processing.
	///</summary>
	[Serializable]
	internal class CodeModules : ReportLink, IEnumerable
	{
        List<CodeModule> _Items;			// list of code module

		internal CodeModules(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _Items = new List<CodeModule>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "CodeModule")
				{
					CodeModule cm = new CodeModule(r, this, xNodeLoop);
					_Items.Add(cm);
				}
				else
				{
					// don't know this element - log it
					OwnerReport.rl.LogError(4, "Unknown CodeModules element '" + xNodeLoop.Name + "' ignored.");
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For CodeModules at least one CodeModule is required.");
			else
                _Items.TrimExcess();
		}
		/// <summary>
		/// Return the Type given a class name.  Searches the CodeModules that are specified
		/// in the report.
		/// </summary>
		internal Type this[string s]
		{
			get 
			{
				Type tp=null;
                if (s == string.Empty)
                    return null;
				try
				{
					// loop thru all the codemodules looking for the assembly
					//  that contains this type
					foreach (CodeModule cm in _Items)
					{
						Assembly a = cm.LoadedAssembly();
						if (a != null)
						{
							tp = a.GetType(s,false,true);
							if (tp != null)
								break;
						}
					}
				}
				catch(Exception ex) 
				{
					OwnerReport.rl.LogError(4, string.Format("Exception finding type. {0}", ex.Message));
				}
				return tp;
			}
		}
		
		override internal void FinalPass()
		{
			foreach (CodeModule cm in _Items)
			{
				cm.FinalPass();
			}
			return;
		}

		internal void LoadModules()
		{
			foreach (CodeModule cm in _Items)
			{
				cm.LoadedAssembly();
			}
		}

        internal List<CodeModule> Items
		{
			get { return  _Items; }
		}
		
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		#endregion
	}
}
