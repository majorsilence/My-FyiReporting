/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace fyiReporting.RdlDesign
{
	internal class DGCBColumn : DataGridColumnStyle 
	{ 
		private ComboBox _cb = new ComboBox(); 
		private bool editing = false; 
		private string originalText; 
		CurrencyManager _CM;
		int _Row;

		public DGCBColumn() : this(ComboBoxStyle.DropDownList)
		{ 
		} 

		public DGCBColumn(ComboBoxStyle cbs) 
		{ 
			_cb.Visible = false; 
			_cb.DropDownStyle = cbs; 
		} 

		protected override void Abort(int row) 
		{ 
			RollBack(); 
			HideComboBox(); 
			EndEdit(); 
		} 

		internal ComboBox CB
		{
			get {return _cb;}
		}

		protected override bool Commit(CurrencyManager cm, int row) 
		{
//			if (!editing)
//				return true;

			try 
			{ 
				object o = _cb.Text; 
				if (NullText.Equals(o)) 
					o = System.Convert.DBNull; 


				SetColumnValueAtRow(cm, row, o); 
			} 
			catch 
			{ 
//				EndEdit();
				return false; 
			} 

//			HideComboBox();	  
//			EndEdit(); 
			return true; 
		} 

		protected override void ConcedeFocus() 
		{ 
			if (editing)
			{
				object o = _cb.Text; 
				if (NullText.Equals(o)) 
					o = System.Convert.DBNull; 


				SetColumnValueAtRow(_CM, _Row, o); 
			}
			HideComboBox(); 
			EndEdit();
		} 

		protected override void Edit(CurrencyManager cm, int row, Rectangle 
				rect, bool readOnly, string text, bool visible) 
		{
			_CM = cm;
			_Row = row;
			originalText = _cb.Text; 

			_cb.Text = string.Empty; 
			_cb.Bounds = rect; 
			_cb.RightToLeft = DataGridTableStyle.DataGrid.RightToLeft; 
			_cb.Visible = visible; 

			if (text != null) 
				_cb.Text = text; 
			else
			{
				string temp = GetText(GetColumnValueAtRow(cm, row)); 
				_cb.Text = temp; 
			}

			_cb.Select(_cb.Text.Length, 0); 

			if (_cb.Visible) 
				DataGridTableStyle.DataGrid.Invalidate(_cb.Bounds); 

			if (ReadOnly) 
				_cb.Enabled = false; 

			editing = true; 
		} 


		protected override int GetMinimumHeight() 
		{ 
			return _cb.PreferredHeight; 
		} 


		protected override int GetPreferredHeight(Graphics g, object o) 
		{ 
			return 0; 
		} 

		protected override Size GetPreferredSize(Graphics g, object o) 
		{ 
			return new Size(0, 0); 
		} 

		protected override void Paint(Graphics g, Rectangle rect, 
									CurrencyManager cm, int row) 
		{ 
			Paint(g, rect, cm, row, false); 
		} 

		protected override void Paint(Graphics g, Rectangle rect, 
					CurrencyManager cm, int row, bool alignToRight) 
		{ 
			string text = GetText(GetColumnValueAtRow(cm, row)); 
			PaintText(g, rect, text, alignToRight); 
		} 

		protected override void Paint(Graphics g, Rectangle rect, 
					CurrencyManager cm, int row, Brush backBrush, Brush foreBrush, bool alignToRight) 
		{ 
			string text = GetText(GetColumnValueAtRow(cm, row)); 
			PaintText(g, rect, text, backBrush, foreBrush, alignToRight); 
		} 


		protected override void SetDataGridInColumn(DataGrid dg) 
		{ 
			base.SetDataGridInColumn(dg); 

			if (_cb.Parent != dg) 
			{ 
				if (_cb.Parent != null) 
				{ 
					_cb.Parent.Controls.Remove(_cb); 
				} 
			} 

			if (dg != null) 
				dg.Controls.Add(_cb); 
		} 


		protected override void UpdateUI(CurrencyManager cm, int row, string text) 
		{ 
			_cb.Text = GetText(GetColumnValueAtRow(cm, row)); 

			if (text != null) 
				_cb.Text = text; 
		}                                                                                                                         


		private int DataGridTableGridLineWidth 
		{ 
			get 
			{ 
				return (DataGridTableStyle.GridLineStyle == DataGridLineStyle.Solid) ? 1 : 0; 
			} 
		} 


		public void EndEdit() 
		{ 
			editing = false; 

			Invalidate(); 
		} 


		private string GetText(object o) 
		{ 
			if (o == System.DBNull.Value) 
				return NullText; 

			if (o != null) 
				return o.ToString(); 
			else 
				return string.Empty; 
		} 


		private void HideComboBox() 
		{ 
			if (_cb.Focused) 
				DataGridTableStyle.DataGrid.Focus(); 

			_cb.Visible = false; 
		} 

		private void RollBack() 
		{ 
			_cb.Text = originalText; 
		} 


		protected virtual void PaintText(Graphics g, Rectangle rect, string text, bool alignToRight) 
		{ 
			Brush backBrush = new SolidBrush(DataGridTableStyle.BackColor); 
			Brush foreBrush = new SolidBrush(DataGridTableStyle.ForeColor); 

			PaintText(g, rect, text, backBrush, foreBrush, alignToRight); 
		} 


		protected virtual void PaintText(Graphics g, Rectangle rect, string text, 
			Brush backBrush, Brush foreBrush, bool alignToRight) 
		{       
			StringFormat format = new StringFormat(); 
			if (alignToRight) 
				format.FormatFlags = StringFormatFlags.DirectionRightToLeft; 

			switch (Alignment) 
			{ 
				case HorizontalAlignment.Left: 
					format.Alignment = StringAlignment.Near; 
				break; 
				case HorizontalAlignment.Right: 
					format.Alignment = StringAlignment.Far; 
				break; 
				case HorizontalAlignment.Center: 
					format.Alignment = StringAlignment.Center; 
				break; 
			} 

			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap; 

			g.FillRectangle(backBrush, rect); 
			g.DrawString(text, DataGridTableStyle.DataGrid.Font, foreBrush, rect, format); 

			format.Dispose(); 
		} 
	} 
}