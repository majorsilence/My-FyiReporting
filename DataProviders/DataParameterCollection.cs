
using System;
using System.Collections;
using System.Data;

namespace Majorsilence.Reporting.Data
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
