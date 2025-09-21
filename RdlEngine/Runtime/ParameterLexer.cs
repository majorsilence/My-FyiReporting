
using System;
using System.IO;
using System.Collections;
using System.Text;
using Majorsilence.Reporting.RdlEngine.Resources;

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// A simple Lexer that is used to create a list of parameters.
	/// </summary>
	internal class ParameterLexer
	{
		private TokenList tokens;
		private CharReader reader;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// expression syntax to lex.
		/// </summary>
		/// <param name="expr">An expression to lex.</param>
		internal ParameterLexer(string expr)
			: this(new StringReader(expr))
		{
			// use this
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// TextReader to lex.
		/// </summary>
		/// <param name="source">A TextReader to lex.</param>
		internal ParameterLexer(TextReader source)
		{
			// token queue
			tokens = new TokenList();

			// read the file contents
			reader = new CharReader(source);
		}

		/// <summary>
		/// Breaks the input stream onto the tokens list and returns an ArrayList of strings.
		/// </summary>
		/// <returns>The array of items broken out from the string</returns>
		internal ArrayList Lex()
		{
			Token token = GetNextToken();
			while(true)
			{
				if(token != null)
					tokens.Add(token);
				else
				{
					tokens.Add(new Token(null, reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.EOF));
                    break;
				}

				token = GetNextToken();
			}
            // now just do a list of strings
            ArrayList ar = new ArrayList(tokens.Count);
            foreach (Token t in tokens)
            {
                if (t.Type == TokenTypes.QUOTE)
                    ar.Add(t.Value);
            }
            return ar;
		}

		private Token GetNextToken()
		{
			while(!reader.EndOfInput())
			{
				char ch = reader.GetNext();

				// skipping whitespaces
				if(Char.IsWhiteSpace(ch))
				{
					continue;
				}
				switch(ch)
				{
					case ',':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.COMMA);
					case '"':
					case '\'':
						return ReadQuoted(ch);
					default:
                        return ReadToComma(ch);
				} // end of swith
			}
			return null;
		}

		// Reads a decimal number with optional exponentiation 
		private Token ReadToComma(char ch)
		{
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
			while(!reader.EndOfInput() )
			{
				char cPeek = reader.Peek();
				if (cPeek == ',')
					break;
                cPeek = reader.GetNext();
                sb.Append(cPeek);
			}

			return new Token(sb.ToString(), TokenTypes.QUOTE);
		}

		// Quoted string like " asdf " or ' asdf '
		private Token ReadQuoted(char ch)
		{
			char qChar = ch;
			int startLine = reader.Line;
			int startCol = reader.Column;
			StringBuilder quoted = new StringBuilder();

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == '\\')
                {
                    char pChar = reader.Peek();
                    if (pChar == qChar)
                        ch = reader.GetNext();			// got one skip escape char
                    else if (pChar == 'n')
                    {
                        ch = '\n';
                        reader.GetNext();               // skip the character
                    }
                    else if (pChar == 'r')
                    {
                        ch = '\r';
                        reader.GetNext();               // skip the character
                    }
                }
				else if (ch == qChar)
					return new Token(quoted.ToString(), startLine, startCol, reader.Line, reader.Column, TokenTypes.QUOTE);

				quoted.Append(ch);
			}
			throw new ParserException(Strings.Lexer_ErrorP_UnterminatedString);
		}

	}
}
