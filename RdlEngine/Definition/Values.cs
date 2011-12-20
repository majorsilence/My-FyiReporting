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

namespace fyiReporting.RDL
{
	///<summary>
	/// Ordered list of values used as a default for a parameter
	///</summary>
	[Serializable]
    internal class Values : ReportLink, System.Collections.Generic.ICollection<Expression>
	{
        List<Expression> _Items;			// list of expression items

		internal Values(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			Expression v;
            _Items = new List<Expression>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Value":
						v = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					default:	
						v=null;	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Value element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (v != null)
					_Items.Add(v);
			}
			if (_Items.Count > 0)
                _Items.TrimExcess();
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			foreach (Expression e in _Items)
			{	
				e.FinalPass();
			}
			return;
		}

        internal List<Expression> Items
		{
			get { return  _Items; }
		}
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		#endregion

        #region ICollection<Expression> Members

        public void Add(Expression item)
        {
            _Items.Add(item);
        }

        public void Clear()
        {
            _Items.Clear();
        }

        public bool Contains(Expression item)
        {
            return _Items.Contains(item);
        }

        public void CopyTo(Expression[] array, int arrayIndex)
        {
            _Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Expression item)
        {
            return _Items.Remove(item);
        }

        #endregion

        #region IEnumerable<Expression> Members

        IEnumerator<Expression> IEnumerable<Expression>.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        #endregion
    }
}
