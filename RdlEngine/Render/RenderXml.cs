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
using fyiReporting.RDL;
using System.IO;
using System.Collections;
using System.Text;

namespace fyiReporting.RDL
{
	
	///<summary>
	///The primary class to "run" a report to XML
	///</summary>
	internal class RenderXml: IPresent
	{
		Report r;					// report
		TextWriter tw;				// where the output is going
		Stack stkReportItem;		// stack of nested report items
		Stack stkContainers;		// stack to hold container elements
		string rowstart=null;

		public RenderXml(Report rep, IStreamGen sg)
		{
			r = rep;
			tw = sg.GetTextWriter();
			stkReportItem = new Stack();
			stkContainers = new Stack();
		}

        public void Dispose() { } 

		public Report Report()
		{
			return r;
		}

		public bool IsPagingNeeded()
		{
			return false;
		}

		public void Start()		
		{
			tw.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");

			PushContainer(r.ReportDefinition.DataElementName);

			return;
		}

		public void End()
		{
			ContainerIO cio = (ContainerIO) stkContainers.Pop();	// this pop should empty the stack
			cio.WriteAttribute(">");
			tw.WriteLine(cio.attribute_sb);
			tw.WriteLine(cio.subelement_sb);
			tw.WriteLine("</" + r.ReportDefinition.DataElementName + ">");

			return;
		}

		// Body: main container for the report
		public void BodyStart(Body b)
		{
		}

		public void BodyEnd(Body b)
		{
		}
		
		public void PageHeaderStart(PageHeader ph)
		{
		}

		public void PageHeaderEnd(PageHeader ph)
		{
		}
 		
		public void PageFooterStart(PageFooter pf)
		{
		}

		public void PageFooterEnd(PageFooter pf)
		{
		}

		public void Textbox(Textbox tb, string t, Row row)
		{
			if (tb.DataElementOutput != DataElementOutputEnum.Output ||
				tb.DataElementName == null)
				return;
			
			if (rowstart != null)		// In case no items in row are visible
			{							//   we delay until we get one.
//				WriteElement(rowstart);
				rowstart = null;
			}
			t = XmlUtil.XmlAnsi(t);
			if (tb.DataElementStyle == DataElementStyleEnum.AttributeNormal)
			{	// write out as attribute
				WriteAttribute(" {0}='{1}'",
								tb.DataElementName, XmlUtil.EscapeXmlAttribute(t));
			}
			else
			{	// write out as element
				WriteElement("<{0}>{1}</{0}>", tb.DataElementName, t);
			}
		}
		
		public void DataRegionNoRows(DataRegion t, string noRowMsg)
		{
		}

		// Lists
		public bool ListStart(List l, Row r)
		{
			if (l.DataElementOutput == DataElementOutputEnum.NoOutput)
				return false;
			if (l.DataElementOutput == DataElementOutputEnum.ContentsOnly)
				return true;
			WriteElementLine("<{0}>", l.DataElementName);

			return true;	//want to continue
		}

		public void ListEnd(List l, Row r)
		{
			if (l.DataElementOutput == DataElementOutputEnum.NoOutput ||
				l.DataElementOutput == DataElementOutputEnum.ContentsOnly)
				return;

			WriteElementLine("</{0}>", l.DataElementName);
			return;
		}

		public void ListEntryBegin(List l, Row r)
		{
			string d;
			if (l.Grouping == null)
			{
				if (l.DataElementOutput != DataElementOutputEnum.Output)
					return;
				d = string.Format("<{0}", l.DataInstanceName);
			}
			else
			{
				Grouping g = l.Grouping;
				if (g.DataElementOutput != DataElementOutputEnum.Output)
					return;
				d = string.Format("<{0}", l.DataInstanceName);
			}
			PushContainer(l.DataInstanceName);

			return;
		}

		public void ListEntryEnd(List l, Row r)
		{
			if (l.DataElementOutput != DataElementOutputEnum.Output)
				return;

			PopContainer(l.DataInstanceName);

		}

		// Tables					// Report item table
		public bool TableStart(Table t, Row row)
		{
			if (t.DataElementOutput == DataElementOutputEnum.NoOutput)
				return false;

			PushContainer(t.DataElementName);

			stkReportItem.Push(t);
			string cName = TableGetCollectionName(t);
			if (cName != null)
				WriteAttributeLine("><{0}", cName);

			return true;
		}

		public void TableEnd(Table t, Row row)
		{
			if (t.DataElementOutput == DataElementOutputEnum.NoOutput)
				return;

			string cName = TableGetCollectionName(t);
			PopContainer(cName);

			WriteElementLine("</{0}>", t.DataElementName);
			stkReportItem.Pop();
			return;
		}

		string TableGetCollectionName(Table t)
		{
			string cName;
			if (t.TableGroups == null)
			{
				if (t.Details != null && t.Details.Grouping != null)
					cName = t.Details.Grouping.DataCollectionName;
				else
					cName = t.DetailDataCollectionName;
			}
			else
				cName = null;

			return cName;
		}
 
		public void TableBodyStart(Table t, Row row)
		{
		}

		public void TableBodyEnd(Table t, Row row)
		{
		}

		public void TableFooterStart(Footer f, Row row)
		{
		}

		public void TableFooterEnd(Footer f, Row row)
		{
		}

		public void TableHeaderStart(Header h, Row row)
		{
		}

		public void TableHeaderEnd(Header h, Row row)
		{
		}

		public void TableRowStart(TableRow tr, Row row)
		{
			string n = TableGetRowElementName(tr);
			if (n == null)
				return;
			PushContainer(n);
		}

		public void TableRowEnd(TableRow tr, Row row)
		{
			string n = TableGetRowElementName(tr);
			if (n == null)
				return;
			this.PopContainer(n);
		}

		string TableGetRowElementName(TableRow tr)
		{
			for (ReportLink rl = tr.Parent; !(rl is Table); rl = rl.Parent)
			{
				if (rl is Header || rl is Footer)
					return null;

				if (rl is TableGroup)
				{
					TableGroup tg = rl as TableGroup;
					Grouping g = tg.Grouping;
					return g.DataElementName;
				}

				if (rl is Details)
				{
					Table t = (Table) stkReportItem.Peek();

                    return t.DetailDataElementOutput == DataElementOutputEnum.NoOutput?
                        null: t.DetailDataElementName;
				}
			}

			return null;
		}

		public void TableCellStart(TableCell t, Row row)
		{
			return;
		}

		public void TableCellEnd(TableCell t, Row row)
		{
			return;
		}

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)				// called first
		{
			if (m.DataElementOutput != DataElementOutputEnum.Output)
				return false;
			tw.WriteLine("<" + (m.DataElementName == null? "Matrix": m.DataElementName) + ">");

			return true;
		}

		public void MatrixColumns(Matrix m, MatrixColumns mc)	// called just after MatrixStart
		{
		}

		public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
		{
		}

		public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
		{
		}

		public void MatrixRowStart(Matrix m, int row, Row r)
		{
		}

		public void MatrixRowEnd(Matrix m, int row, Row r)
		{
		}

		public void MatrixEnd(Matrix m, Row r)				// called last
		{
			tw.WriteLine("</" + (m.DataElementName == null? "Matrix": m.DataElementName) + ">");
		}

		public void Chart(Chart c, Row r, ChartBase cb)
		{
		}

		public void Image(Image i, Row r, string mimeType, Stream io)
		{
		}

		public void Line(Line l, Row r)
		{
		}

		public bool RectangleStart(RDL.Rectangle rect, Row r)
		{
			bool rc=true;
			switch (rect.DataElementOutput)
			{
				case DataElementOutputEnum.NoOutput:
					rc = false;
					break;
				case DataElementOutputEnum.Output:
					if (rowstart != null)		// In case no items in row are visible
					{							//   we delay until we get one.
						tw.Write(rowstart);
						rowstart = null;
					}
					PushContainer(rect.DataElementName);
					break;
				case DataElementOutputEnum.Auto:
				case DataElementOutputEnum.ContentsOnly:
				default:
					break;
			}

			return rc;
		}

		public void RectangleEnd(RDL.Rectangle rect, Row r)
		{
			if (rect.DataElementOutput != DataElementOutputEnum.Output)
				return;
			PopContainer(rect.DataElementName);
		}
		
		public void Subreport(Subreport s, Row r)
		{
			if (s.DataElementOutput != DataElementOutputEnum.Output)
				return;

			PushContainer(s.DataElementName);

			s.ReportDefn.Run(this);

			PopContainer(s.DataElementName);
			return;
		}
		public void GroupingStart(Grouping g)			// called at start of grouping
		{
			if (g.DataElementOutput != DataElementOutputEnum.Output)
				return;

			PushContainer(g.DataCollectionName);

		}
		public void GroupingInstanceStart(Grouping g)	// called at start for each grouping instance
		{
			if (g.DataElementOutput != DataElementOutputEnum.Output)
				return;
			PushContainer(g.DataElementName);

		}
		public void GroupingInstanceEnd(Grouping g)	// called at start for each grouping instance
		{
			if (g.DataElementOutput != DataElementOutputEnum.Output)
				return;
			PopContainer(g.DataElementName);
		}
		public void GroupingEnd(Grouping g)			// called at end of grouping
		{
			if (g.DataElementOutput != DataElementOutputEnum.Output)
				return;
			PopContainer(g.DataCollectionName);
		}

		public void RunPages(Pages pgs)	// we don't have paging turned on for xml
		{
			return;
		}

		void PopContainer(string name)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Pop();
			if (cio.bEmpty)
				return;
			cio.WriteAttribute(">");
			WriteElementLine(cio.attribute_sb.ToString());
			WriteElementLine(cio.subelement_sb.ToString());
			if (name != null)
				WriteElementLine("</{0}>", name);
		}

		void PushContainer(string name)
		{
			ContainerIO cio = new ContainerIO("<" + name);
			stkContainers.Push(cio);
		}

		void WriteElement(string format)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteElement(format);
		}

		void WriteElement(string format, params object[] arg)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteElement(format, arg);
		}

		void WriteElementLine(string format)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteElementLine(format);
		}

		void WriteElementLine(string format, params object[] arg)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteElementLine(format, arg);
		}

		void WriteAttribute(string format)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteAttribute(format);
		}

		void WriteAttribute(string format, params object[] arg)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteAttribute(format, arg);
		}

		void WriteAttributeLine(string format)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteAttributeLine(format);
		}

		void WriteAttributeLine(string format, params object[] arg)
		{
			ContainerIO cio = (ContainerIO) this.stkContainers.Peek();
			cio.WriteAttributeLine(format, arg);
		}

		class ContainerIO
		{
			internal StringBuilder attribute_sb;
			internal StringWriter attribute_sw;

			internal StringBuilder subelement_sb;
			internal StringWriter subelement_sw;

			internal bool bEmpty=true;

			internal ContainerIO(string begin)
			{
				subelement_sb = new StringBuilder();
				subelement_sw = new StringWriter(subelement_sb);

				attribute_sb = new StringBuilder(begin);
				attribute_sw = new StringWriter(attribute_sb);
			}

			internal void WriteElement(string format)
			{
				bEmpty = false;
				subelement_sw.Write(format);
			}

			internal void WriteElement(string format, params object[] arg)
			{
				bEmpty = false;
				subelement_sw.Write(format, arg);
			}

			internal void WriteElementLine(string format)
			{
				bEmpty = false;
				subelement_sw.WriteLine(format);
			}

			internal void WriteElementLine(string format, params object[] arg)
			{
				bEmpty = false;
				subelement_sw.WriteLine(format, arg);
			}

			internal void WriteAttribute(string format)
			{
				bEmpty = false;
				attribute_sw.Write(format);
			}

			internal void WriteAttribute(string format, params object[] arg)
			{
				bEmpty = false;
				attribute_sw.Write(format, arg);
			}

			internal void WriteAttributeLine(string format)
			{
				bEmpty = false;
				attribute_sw.WriteLine(format);
			}

			internal void WriteAttributeLine(string format, params object[] arg)
			{
				bEmpty = false;
				attribute_sw.WriteLine(format, arg);
			}

		}
	}

}
