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

namespace fyiReporting.RDL
{
	///<summary>
	/// A report object name.   CLS comliant identifier.
	///</summary>
	[Serializable]
	internal class Name
	{
		string _Name;			// name CLS compliant identifier; www.unicode.org/unicode/reports/tr15/tr15-18.html
	
		internal Name(string name)
		{
			_Name=name;
		}

		internal string Nm
		{
			get { return  _Name; }
			set {  _Name = value; }
		}

		public override string ToString()
		{
			return _Name;
		}
	}
}
