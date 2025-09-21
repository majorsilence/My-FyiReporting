
using System;

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Represents an exception throwed by lexer and parser.
	/// </summary>
	internal class ParserException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the ParserException class with the
		/// specified message.
		/// </summary>
		/// <param name="message">A message.</param>
		internal ParserException(string message) : base(message)
		{
			// used base
		}
	}
}
