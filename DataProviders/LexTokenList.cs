

using System;
using System.Collections;

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// Represents a list of the tokens.
	/// </summary>
	internal class LexTokenList : IEnumerable
	{
		private ArrayList tokens = null;

		internal LexTokenList()
		{
			tokens = new ArrayList();
		}

		internal void Add(LexToken token)
		{
			tokens.Add(token);
		}

		internal void Push(LexToken token)
		{
			tokens.Insert(0, token);
		}

		internal LexToken Peek()
		{
			return (LexToken)tokens[0];
		}

		internal LexToken Extract()
		{
			LexToken token = (LexToken)tokens[0];
			tokens.RemoveAt(0);
			return token;
		}

		internal int Count
		{
			get
			{
				return tokens.Count;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return tokens.GetEnumerator();
		}
	}
}
