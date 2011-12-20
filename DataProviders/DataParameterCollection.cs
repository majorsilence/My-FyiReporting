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
using System.Data;

namespace fyiReporting.Data
{
	/// <summary>
	/// XmlDataParameter
	/// </summary>
	public class DataParameterCollection : IDataParameterCollection
	{
		ArrayList _List;		// parameter collection

		public DataParameterCollection()
		{
			_List = new ArrayList();
		}
		#region IDataParameterCollection Members

		public object this[string parameterName]
		{
			get
			{
				int index = IndexOf(parameterName);
				if (index < 0)
					return null;
				return _List[index];
			}
			set
			{
				IDbDataParameter dp = value as IDbDataParameter;
				if (dp == null)
					throw new ArgumentException("Object must be an IDbDataParameter");

				_List[this.IndexOf(parameterName)] = dp;				
			}
		}

		public void RemoveAt(string parameterName)
		{
			_List.RemoveAt(this.IndexOf(parameterName));
		}

		public bool Contains(string parameterName)
		{
			return IndexOf(parameterName) < 0? false: true;
		}

		public int IndexOf(string parameterName)
		{
			int i=0;
			foreach (IDbDataParameter dp in _List)
			{
				if (dp.ParameterName == parameterName)
					return i;
				i++;
			}
			return -1;
		}

		#endregion

		#region IList Members

		public bool IsReadOnly
		{
			get
			{
				return _List.IsReadOnly;
			}
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return _List[index];
			}
			set
			{
				_List[index] = value;
			}
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			_List.RemoveAt(index);
		}

		public void Insert(int index, object value)
		{
			_List.Insert(index, value);
		}

		public void Remove(object value)
		{
			_List.Remove(value);
		}

		bool System.Collections.IList.Contains(object value)
		{
			return _List.Contains(value);
		}

		public void Clear()
		{
			_List.Clear();
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return _List.IndexOf(value);
		}

		public int Add(object value)
		{
			return _List.Add(value);
		}

		public bool IsFixedSize
		{
			get
			{
				return _List.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return _List.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return _List.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_List.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _List.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return _List.GetEnumerator();
		}

		#endregion
	}
}
