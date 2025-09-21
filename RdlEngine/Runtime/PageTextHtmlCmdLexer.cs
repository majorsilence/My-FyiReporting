
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Majorsilence.Reporting.RdlEngine.Resources;

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// A simple lexer that is used by PageTextHtml to break an HTML command into name value pairs.
	/// </summary>
	internal class PageTextHtmlCmdLexer
	{
		private CharReader reader;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// TextReader to lex.
		/// </summary>
		/// <param name="source">A TextReader to lex.</param>
		internal PageTextHtmlCmdLexer(string htmlcmd)
		{
			// read the file contents
			reader = new CharReader(htmlcmd);
		}

		/// <summary>
		/// Breaks the input stream onto the tokens list and returns a hash table with the name value pairs.
		/// </summary>
		/// <returns>The hash table.</returns>
		internal Hashtable Lex()
		{
            Hashtable ht = new Hashtable();
			while(true)
			{
                string cmd = GetNextToken();
                if (cmd == null)
                    return ht;

                if (GetNextToken() != "=")
                    return ht;

                string val = GetNextToken();
                if (val == null)
                    return ht;

                ht.Add(cmd, val);
			}
		}

		private string GetNextToken()
		{
			while(!reader.EndOfInput())
			{
				char ch = reader.GetNext();

				// skipping whitespaces	at front of token
				if(Char.IsWhiteSpace(ch))
					continue;

				switch(ch)
				{
					case '\'':
                    case '"':
						return ReadQuoted(ch);

					default:
						return ReadWord(ch);
				} // end of switch
			}
			return null;
		}

		// Read html command string
		private string ReadQuoted(char firstc)
		{
			char qChar = firstc;
			StringBuilder lt = new StringBuilder();

			while(!reader.EndOfInput())
			{
				char ch = reader.GetNext();
                if (ch == qChar)
                    break;
                lt.Append(ch);
			}
			return lt.ToString();
		}

		// Read to next white space or to specified character
		private string ReadWord(char ch)
		{
            if (ch == '=')
                return "=";

			StringBuilder lt = new StringBuilder();
            lt.Append(ch);

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();

                if (Char.IsWhiteSpace(ch) || ch == '=')
				{
					reader.UnGet();		// put it back on the stack
					break;
				}

				lt.Append(ch);
			}
			return lt.ToString();
		}

		class CharReader
		{
			string file = null;
			int    ptr  = 0;

			/// <summary>
			/// Initializes a new instance of the CharReader class.
			/// </summary>
			internal CharReader(string text)
			{
				file = text;
			}
		
			/// <summary>
			/// Returns the next char from the stream.
			/// </summary>
			internal char GetNext()
			{
				if (EndOfInput()) 
				{
					return '\0';
				}
				char ch = file[ptr++];

				return ch;
			}
		
			/// <summary>
			/// Returns the next char from the stream without removing it.
			/// </summary>
			internal char Peek()
			{
				if (EndOfInput()) // ok to peek at end of file
					return '\0';

				return file[ptr];
			}
		
			/// <summary>
			/// Undoes the extracting of the last char.
			/// </summary>
			internal void UnGet()
			{
				--ptr;
				if (ptr < 0) 
					throw new Exception(Strings.CharReader_Error_FileReaderUnGetFirstChar);
			
				char ch = file[ptr];
			}
		
			/// <summary>
			/// Returns True if end of input was reached; otherwise False.
			/// </summary>
			internal bool EndOfInput()
			{
				return ptr >= file.Length;
			}
		}
	}
}
