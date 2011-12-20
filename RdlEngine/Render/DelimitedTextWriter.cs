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
using System.IO;
using System.Text;

namespace fyiReporting.RDL
{
    internal class DelimitedTextWriter 
    {
        private TextWriter textWriter;

        private string columnDelimiter;
        private bool firstColumn = true;

        private char quote = '"';

        public TextWriter TextWriter
        {
            get
            {
                return textWriter;
            }
        }

        public char Quote
        {
            get { return quote; }
        }

        public string ColumnDelimiter
        {
            get { return columnDelimiter; }
        }

        private string rowDelimiter = "\n";

        public string RowDelimiter
        {
            get { return rowDelimiter; }
        }

        protected void WriteQuote()
        {
            textWriter.Write(quote);
        }

        protected void WriteDelimeter()
        {
            if (!firstColumn)
            {
                textWriter.Write(columnDelimiter);
            }

            firstColumn = false;
        }

        public DelimitedTextWriter(TextWriter writer, string delimeter)
        {
            textWriter = writer;
            columnDelimiter = delimeter;
        }

        private void WriteQuoted(object value)
        {
            WriteDelimeter();
            WriteQuote();

            if ( value != null )
                textWriter.Write(value.ToString().Replace("\"", "\"\""));

            WriteQuote();
        }

        private void WriteUnquoted(object value)
        {
            WriteDelimeter();

            if ( value != null )
                textWriter.Write(value);
        }

        public void Write(object value)
        {
            bool isQuoted = true;

            if (value != null)
            {
                Type type = value.GetType();

                if (type.IsPrimitive &&
                    type != typeof(bool) && type != typeof(char))
                {
                    isQuoted = false;
                }
            }

            if (isQuoted)
                WriteQuoted(value);
            else
                WriteUnquoted(value);
        }

        public void Write(string format, params object[] arg)
        {
            WriteQuoted(string.Format(format, arg));
        }

        public void WriteLine()
        {
            textWriter.Write(rowDelimiter);
            firstColumn = true;
        }

        public void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(string format, params object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }
    }
}