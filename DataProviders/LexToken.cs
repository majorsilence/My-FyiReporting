
using System;

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// Token class that used by Lexer.
	/// </summary>
	internal class LexToken
	{
		internal string Value;
		internal LexTokenTypes Type;

		/// <summary>
		/// Initializes a new instance of the Token class.
		/// </summary>
		internal LexToken(string value, LexTokenTypes type)
		{
			Value = value;
			Type = type;
		}

		/// <summary>
		/// Returns a string representation of the Token.
		/// </summary>
		public override string ToString()
		{
			return "<" + Type + "> " + Value;	
		}
	}
}
