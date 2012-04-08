// 
//  ReportViewer.cs
//  
//  Author:
//       Krzysztof Marecki
// 
//  Copyright (c) 2010 Krzysztof Marecki
//  Copyright (c) 2012 Peter Gill
// 
// This file is part of the NReports project
// This file is part of the My-FyiReporting project 
//	
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

using Gtk;
using fyiReporting.RDL;

namespace fyiReporting.RdlGtkViewer
{	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ReportViewer : Gtk.Bin
	{
		Report report;
		Pages pages;
		PrintOperation printing;

		public NeedPassword DataSourceReferencePassword = null;
		
		public string connstr_param_name = "connection_string";
		public string ConnectionStringParameterName {
			get { return connstr_param_name; }
			set { connstr_param_name = value; }
		}
		
		public string conntype_param_name = "connection_type";
		public string ConnectionTypeParameterName {
			get { return conntype_param_name; }
			set { conntype_param_name = value; }
		}
		
		public ListDictionary Parameters { get; private set; }
		
		bool show_errors;
		public bool ShowErrors {
			get {
				return show_errors;
			}
			set {
				show_errors = value;
				CheckVisibility ();
			}
		}
		
		bool show_params;
		public bool ShowParameters {
			get {
				return show_params;
			}
			set { 
				show_params = value; 
				CheckVisibility ();
			}
		}
		
		public Uri SourceFile { get; private set; }
		
		public ReportViewer ()
		{
			this.Build ();
			Parameters = new ListDictionary ();
			
			this.errorsAction.Toggled += OnErrorsActionToggled;
			DisableActions ();
			ShowErrors = false;
		}
		
		
		public void LoadReport (Uri sourcefile)
		{
			SourceFile = sourcefile;
			
			string xml = GetRdlSource ();
			RDLParser parser = new RDLParser (xml);
			parser.DataSourceReferencePassword = DataSourceReferencePassword;
			parser.Folder = System.IO.Path.GetDirectoryName (SourceFile.AbsolutePath);
			
			report = parser.Parse ();
			AddParameterControls ();
			RefreshReport ();
		}


		void OnErrorsActionToggled (object sender, EventArgs e)
		{
			ShowErrors = errorsAction.Active;
		}
		
		void CheckVisibility ()
		{
			if (ShowErrors) {
					scrolledwindowErrors.ShowAll ();
				} else {
					scrolledwindowErrors.HideAll ();
				}
			if (ShowParameters) {
				vboxParameters.ShowAll ();
			} else {
				vboxParameters.HideAll ();
			}	
		}
		
		protected override void OnShown ()
		{
			base.OnShown ();
			CheckVisibility ();
		}

		void DisableActions ()
		{
			PdfAction.Sensitive = false;
			refreshAction.Sensitive = false;
			printAction.Sensitive = false;
			ZoomInAction.Sensitive = false;
			ZoomOutAction.Sensitive = false;
		}
		
		void EnableActions ()
		{
			PdfAction.Sensitive = true;
			refreshAction.Sensitive = true;
			printAction.Sensitive = true;
			ZoomInAction.Sensitive = true;
			ZoomOutAction.Sensitive = true;
		}
		
		void AddParameterControls ()
		{
			foreach (Widget child in vboxParameters.Children) {
				vboxParameters.Remove (child);
			}
			foreach (UserReportParameter rp in report.UserReportParameters) {
				HBox hbox = new HBox ();
				Label labelPrompt = new Label ();
				labelPrompt.SetAlignment (0, 0.5f);
				labelPrompt.Text = string.Format ("{0} :", rp.Prompt);
				hbox.PackStart (labelPrompt, true, true, 0);
				Entry entryValue = new Entry ();
				if (Parameters.Contains (rp.Name)) {
					if (Parameters [rp.Name] != null) {
						entryValue.Text = Parameters [rp.Name].ToString ();
					}
				} else {
					if (rp.DefaultValue != null) {
						StringBuilder sb = new StringBuilder ();
						for (int i = 0; i < rp.DefaultValue.Length; i++) {
							if (i > 0)
								sb.Append (", ");
							sb.Append (rp.DefaultValue[i].ToString ());
						}
						entryValue.Text = sb.ToString ();
					}
				}
				hbox.PackStart (entryValue, false, false, 0);
				vboxParameters.PackStart (hbox, false, false, 0);
			}
		}
		
		void SetParametersFromControls ()
		{	
			int i = 0;
			foreach (UserReportParameter rp in report.UserReportParameters) {
				HBox hbox = (HBox) vboxParameters.Children[i];
				Entry entry = (Entry) hbox.Children [1];
				//parameters.Add (rp.Name, entry.Text);
				Parameters [rp.Name] = entry.Text;
				i++;
			}
		}

		void RefreshReport ()
		{
			SetParametersFromControls ();
			report.RunGetData (Parameters);
			pages = report.BuildPages ();
			
			reportarea.SetReport (report, pages);
			
			if (report.ErrorMaxSeverity > 0)
				SetErrorMessages (report.ErrorItems);
			
//			Title = string.Format ("RDL report viewer - {0}", report.Name);
			EnableActions ();
			CheckVisibility ();
		}

		string GetRdlSource ()
		{
			string xml = null;
			
			using (var fs = new StreamReader (SourceFile.AbsolutePath))
				xml = fs.ReadToEnd ();
			
			return xml;
		}

		void SetErrorMessages (IList errors)
		{
			textviewErrors.Buffer.Clear ();
			
			StringBuilder msgs = new StringBuilder ();
			foreach (var error in errors)
				msgs.AppendLine (error.ToString ());
			
			textviewErrors.Buffer.Text = msgs.ToString ();
		}

		protected virtual void OnPdfActionActivated (object sender, System.EventArgs e)
		{
			string reportname = System.IO.Path.GetFileNameWithoutExtension (SourceFile.AbsolutePath);
			string filename = string.Format ("{0}.pdf", reportname);
			int width = (int)report.PageWidthPoints;
			int height = (int)report.PageHeightPoints;
			
			using (Cairo.PdfSurface pdf = new Cairo.PdfSurface (filename, width, height))
				using (Cairo.Context g = new Cairo.Context (pdf)) {
					
					RenderCairo render = new RenderCairo (g);
					render.RunPages (pages);
					
					pdf.Finish ();
				}
		}

		protected virtual void OnPrintActionActivated (object sender, System.EventArgs e)
		{
			using (PrintContext context = new PrintContext (GdkWindow.Handle)) {
				
				printing = new PrintOperation ();
				printing.Unit = Unit.Points;
				printing.UseFullPage = false;
				
				printing.BeginPrint += HandlePrintBeginPrint;
				printing.DrawPage += HandlePrintDrawPage;
				printing.EndPrint += HandlePrintEndPrint;
				
				printing.Run (PrintOperationAction.PrintDialog, null);
			}
		}

		void HandlePrintBeginPrint (object o, BeginPrintArgs args)
		{
			printing.NPages = 1;
		}

		void HandlePrintDrawPage (object o, DrawPageArgs args)
		{
			Cairo.Context g = args.Context.CairoContext;
		
			RenderCairo render = new RenderCairo (g);
			render.RunPages (pages);	
		}

		void HandlePrintEndPrint (object o, EndPrintArgs args)
		{
			
		}

		protected virtual void OnZoomOutActionActivated (object sender, System.EventArgs e)
		{
			reportarea.Scale -= 0.1f;
		}

		protected virtual void OnZoomInActionActivated (object sender, System.EventArgs e)
		{
			reportarea.Scale += 0.1f;
		}

		protected virtual void OnRefreshActionActivated (object sender, System.EventArgs e)
		{
			RefreshReport ();
		}
		
		int hpanedWidth = 0;
		void SetHPanedPosition ()
		{
			int textviewWidth = scrolledwindowErrors.Allocation.Width +10;
			hpanedReport.Position = hpanedWidth - textviewWidth;
		}
		
		protected virtual void OnHpanedReportSizeAllocated (object o, Gtk.SizeAllocatedArgs args)
		{
			if (args.Allocation.Width != hpanedWidth) {
				hpanedWidth = args.Allocation.Width;
				SetHPanedPosition ();
			}
		}
	}
}

