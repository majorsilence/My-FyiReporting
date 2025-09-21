
using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// A simple Lexer that is used by Parser.
	/// </summary>
	internal class Lexer
	{
		private LexTokenList tokens;
		private LexCharReader reader;
		internal char SeparatorChar=' ';
		internal bool SeparateDatetime=true;
		internal bool SeparateQuoted=true;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// TextReader to lex.
		/// </summary>
		/// <param name="source">A TextReader to lex.</param>
		internal Lexer(TextReader source)
		{
			// token queue
			tokens = new LexTokenList();

			// read the file contents
			reader = new LexCharReader(source);
		}

		/// <summary>
		/// Breaks the input stream onto the tokens list and returns it.
		/// </summary>
		/// <returns>The tokens list.</returns>
		internal LexTokenList Lex()
		{
			LexToken token = GetNextToken();
			while(true)
			{
				if(token != null)
					tokens.Add(token);
				else
				{
					tokens.Add(new LexToken(null, LexTokenTypes.EOF));
					return tokens;
				}

				token = GetNextToken();
			}
		}

		private LexToken GetNextToken()
		{
			while(!reader.EndOfInput())
			{
				char ch = reader.GetNext();

				// skipping whitespaces	at front of token
				if(Char.IsWhiteSpace(ch))
				{
					continue;
				}
				switch(ch)
				{
					case '"':
					case '\'':
						if (SeparateQuoted)
							return ReadQuoted(ch);
						break;
					case '[':
						if (SeparateDatetime)
							return ReadDateTime(']');
						break;
					default:
						break;
				} // end of switch
				return ReadToChar(ch, SeparatorChar);
			}
			return null;
		}

		// Quoted string like " asdf " or ' asdf '
		private LexToken ReadQuoted(char ch)
		{
			char qChar = ch;
			StringBuilder quoted = new StringBuilder();

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == '\\' && reader.Peek() == qChar)	// look for escaped '"'/"'"
					ch = reader.GetNext();				// got one skip escape char
				else if (ch == qChar)
					return new LexToken(quoted.ToString(), LexTokenTypes.STRING);

				quoted.Append(ch);
			}
			throw new Exception("Unterminated string!");
		}

		// Read string to specified character
		private LexToken ReadToChar(char firstc, char ch)
		{
			char qChar = ch;
			StringBuilder quoted = new StringBuilder();
			quoted.Append(firstc);

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == qChar)
					return new LexToken(quoted.ToString(), LexTokenTypes.STRING);

				quoted.Append(ch);
			}
			return new LexToken(quoted.ToString(), LexTokenTypes.STRING);
		}

		// Read a datetime field == denoted by [...]
		private LexToken ReadDateTime(char ch)
		{
			char qChar = ch;
			StringBuilder quoted = new StringBuilder();

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == qChar)
					break;

				quoted.Append(ch);
			}

			return new LexToken(quoted.ToString(), LexTokenTypes.DATETIME);
		}

	}
}
