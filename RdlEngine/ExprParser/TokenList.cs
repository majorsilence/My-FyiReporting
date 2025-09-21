

using System;
using System.Collections;
using System.Collections.Generic;

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Represents a list of the tokens.
	/// </summary>
	internal class TokenList : IEnumerable
	{
		private List<Token> tokens = null;

		internal TokenList()
		{
			tokens = new List<Token>();
		}

		internal void Add(Token token)
		{
			tokens.Add(token);
		}

		internal void Push(Token token)
		{
			tokens.Insert(0, token);
		}

		internal Token Peek()
		{
			return tokens[0];
		}

		internal Token Extract()
		{
			Token token = tokens[0];
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
