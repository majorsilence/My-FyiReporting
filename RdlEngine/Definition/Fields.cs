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
using System.Collections.Specialized;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Collection of fields for a DataSet.
	///</summary>
	[Serializable]
	internal class Fields : ReportLink, ICollection
	{
		IDictionary _Items;			// dictionary of items

		internal Fields(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			Field f;
			if (xNode.ChildNodes.Count < 10)
				_Items = new ListDictionary();	// Hashtable is overkill for small lists
			else
				_Items = new Hashtable(xNode.ChildNodes.Count);

			// Loop thru all the child nodes
			int iCol=0;
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Field":
						f = new Field(r, this, xNodeLoop);
						f.ColumnNumber = iCol++;			// Assign the column number
						break;
					default:	
						f=null;	
						r.rl.LogError(4, "Unknown element '" + xNodeLoop.Name + "' in fields list."); 
						break;
				}
				if (f != null)
				{
					if (_Items.Contains(f.Name.Nm))
					{
						r.rl.LogError(4, "Field " + f.Name + " has duplicates."); 
					}
					else	
						_Items.Add(f.Name.Nm, f);
				}
			}
		}

		internal Field this[string s]
		{
			get 
			{
				return _Items[s] as Field;
			}
		}
		
		override internal void FinalPass()
		{
			foreach (Field f in _Items.Values)
			{
				f.FinalPass();
			}
			return;
		}

		internal IDictionary Items
		{
			get { return  _Items; }
		}
		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return _Items.Values.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return _Items.Values.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_Items.Values.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _Items.Values.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.Values.GetEnumerator();
		}

		#endregion

	}
}
