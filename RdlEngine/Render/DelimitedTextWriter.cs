

using System;
using System.IO;
using System.Text;

namespace Majorsilence.Reporting.Rdl
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