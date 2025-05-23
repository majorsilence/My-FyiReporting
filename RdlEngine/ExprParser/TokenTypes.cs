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

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Summary description for TokenTypes.
	/// </summary>
	internal enum TokenTypes
	{
		AND,
		OR,
		NOT,
		PLUS,
		PLUSSTRING,
		MINUS,
		LPAREN,
		RPAREN,
		QUOTE,
		IDENTIFIER,
		COMMA,
		NUMBER,
		DATETIME,
		DOUBLE,
		INTEGER,
		EQUAL,
		NOTEQUAL,
		GREATERTHAN,
		GREATERTHANOREQUAL,
		LESSTHAN,
		LESSTHANOREQUAL,
		FORWARDSLASH,
		BACKSLASH,
		STAR,
		EXP,
		MODULUS,
        DOT,                // dot operator
		OTHER,
		EOF
	}
}
