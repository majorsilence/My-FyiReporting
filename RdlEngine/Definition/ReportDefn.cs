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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;

namespace fyiReporting.RDL
{
    /// <summary>
    /// Delegate used to ask for a Data Source Reference password used to decrypt the file.
    /// </summary>
    public delegate string NeedPassword();

	///<summary>
	/// Main Report definition; this is the top of the tree that contains the complete
	/// definition of a instance of a report.
	///</summary>
	[Serializable]
	public class ReportDefn				
	{
		internal int _ObjectCount=0;	// master object counter
		internal ReportLog rl;	// report log
		Name _Name;				// Name of the report
		string _Description;	// Description of the report
		string _Author;			// Author of the report
		int _AutoRefresh;		// Rate at which the report page automatically refreshes, in seconds.  Must be nonnegative.
		//    If omitted or zero, the report page should not automatically refresh.
		//    Max: 2147483647
		DataSourcesDefn _DataSourcesDefn;	// Describes the data sources from which
		internal NeedPassword GetDataSourceReferencePassword=null;

		//		data sets are taken for this report.
		DataSetsDefn _DataSetsDefn;	// Describes the data that is displayed as
									// part of the report
		Body _Body;				// Describes how the body of the report is structured
		ReportParameters _ReportParameters;	// Parameters for the report
		Custom _Customer;		// Custom information to be handed to the report engine
		RSize _Width;			// Width of the report
		PageHeader _PageHeader;	// The header that is output at the top of each page of the report.
		PageFooter _PageFooter;	// The footer that is output at the bottom of each page of the report.
		RSize _PageHeight;		// Default height for the report.  Default is 11 in.
		RSize _PageWidth;		// Default width for the report. Default is 8.5 in.
		RSize _LeftMargin;		// Width of the left margin. Default: 0 in
		RSize _RightMargin;		// Width of the right margin. Default: 0 in
		RSize _TopMargin;		// Width of the top margin. Default: 0 in
		RSize _BottomMargin;	// Width of the bottom margin. Default: 0 in
		EmbeddedImages _EmbeddedImages;	// Images embedded within the report
		Expression _Language;	// The primary language of the text. Default is server language.
		Code _Code;				// The <Code> element support; ie VB functions
		CodeModules _CodeModules;	// Code modules to make available to the
		//							report for use in expressions.
		Classes _Classes;		// Classes 0-1 Element Classes to instantiate during report initialization
		string _DataTransform;	// The location to a transformation to apply
		// to a report data rendering. This can be a full folder path (e.g. �/xsl/xfrm.xsl�),
		// relative path (e.g. �xfrm.xsl�).
		string _DataSchema;		// The schema or namespace to use for a report data rendering.
		string _DataElementName;	// Name of a top level element that
		//		represents the report data. Default: Report.
		DataElementStyleEnum _DataElementStyle;		//Indicates whether textboxes should
		//		render as elements or attributes.
		Subreport _Subreport;	// null if top level report; otherwise the subreport that loaded the report
		bool _ContainsSubreport;	// true if report contains a subreport

		int _DynamicNames=0;		// used for creating names on the fly during parsing
		// Following variables used for parsing/evaluating expressions
        List<ICacheData> _DataCache;	// contains all function that implement ICacheData
		IDictionary _LUGlobals;	// contains global and user properties
		IDictionary _LUUser;	// contains global and user properties
		IDictionary _LUReportItems;	// all TextBoxes in the report	IDictionary _LUGlobalsUser;		// contains global and user properties
		IDictionary _LUDynamicNames;	// for dynamic names
		IDictionary _LUAggrScope;	// Datasets, Dataregions, grouping names
		IDictionary _LUEmbeddedImages;	// Embedded images
		string _ParseFolder;			// temporary folder for looking up things during parse/finalpass
		Type _CodeType;			// used for parsing of expressions; DONT USE AT RUNTIME

		string _OverwriteConnectionString; // To overwrite ConnectionString
		bool _OverwriteInSubreport; // Overwrite ConnectionString in subreport too

        /// <summary>
        /// EBN 31/03/2014
        /// Cross object
        /// </summary>
        private CrossDelegate _SubReportGetContent = new CrossDelegate();
        public CrossDelegate SubReportGetContent
        {
            get { return _SubReportGetContent; }
            set { _SubReportGetContent = value; }
        }

		// Constructor
		internal ReportDefn(XmlNode xNode, ReportLog replog, string folder, NeedPassword getpswd, int objcount, CrossDelegate crossdel, string overwriteConnectionString, bool overwriteInSubreport)		// report has no parents
		{
			rl = replog;				// used for error reporting
			_ObjectCount = objcount;	// starting number for objects in this report; 0 other than for subreports
			GetDataSourceReferencePassword = getpswd;
			_ParseFolder = folder;
			_Description = null;
			_Author = null;		
			_AutoRefresh = -1;
			_DataSourcesDefn = null;
			_DataSetsDefn = null;	
			_Body = null;		
			_Width = null;		
			_PageHeader = null;	
			_PageFooter = null;	
			_PageHeight = null;	
			_PageWidth = null;	
			_LeftMargin = null;	
			_RightMargin = null;
			_TopMargin = null;	
			_BottomMargin = null;
			_EmbeddedImages = null;
			_Language = null;	
			_CodeModules = null;	
			_Code = null;
			_Classes = null;	
			_DataTransform = null;	
			_DataSchema = null;		
			_DataElementName = null;
			_DataElementStyle = DataElementStyleEnum.AttributeNormal;
			_LUReportItems = new Hashtable();		// to hold all the textBoxes
			_LUAggrScope = new ListDictionary();	// to hold all dataset, dataregion, grouping names
			_LUEmbeddedImages = new ListDictionary();	// probably not very many
			_LUDynamicNames = new Hashtable();
            _DataCache = new List<ICacheData>();
            
            // EBN 30/03/2014
            SubReportGetContent = crossdel;

			_OverwriteConnectionString = overwriteConnectionString;
			_OverwriteInSubreport = overwriteInSubreport;

			// Run thru the attributes
			foreach(XmlAttribute xAttr in xNode.Attributes)
			{
				switch (xAttr.Name)
				{
					case "Name":
						_Name = new Name(xAttr.Value);
						break;
				}
			}

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Description":
						_Description = xNodeLoop.InnerText;
						break;
					case "Author":
						_Author = xNodeLoop.InnerText;
						break;
					case "AutoRefresh":
						_AutoRefresh = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "DataSources":
						_DataSourcesDefn = new DataSourcesDefn(this, null, xNodeLoop);
						break;
					case "DataSets":
						_DataSetsDefn = new DataSetsDefn(this, null, xNodeLoop);
						break;
					case "Body":
						_Body = new Body(this, null, xNodeLoop);
						break;
					case "ReportParameters":
						_ReportParameters = new ReportParameters(this, null, xNodeLoop);
						break;
					case "Width":
						_Width = new RSize(this, xNodeLoop);
						break;
					case "PageHeader":
						_PageHeader = new PageHeader(this, null, xNodeLoop);
						break;
					case "PageFooter":
						_PageFooter = new PageFooter(this, null, xNodeLoop);
						break;
					case "PageHeight":
						_PageHeight = new RSize(this, xNodeLoop);
						break;
					case "PageWidth":
						_PageWidth = new RSize(this, xNodeLoop);
						break;
					case "LeftMargin":
						_LeftMargin = new RSize(this, xNodeLoop);
						break;
					case "RightMargin":
						_RightMargin = new RSize(this, xNodeLoop);
						break;
					case "TopMargin":
						_TopMargin = new RSize(this, xNodeLoop);
						break;
					case "BottomMargin":
						_BottomMargin = new RSize(this, xNodeLoop);
						break;
					case "EmbeddedImages":
						_EmbeddedImages = new EmbeddedImages(this, null, xNodeLoop);
						break;
					case "Language":
						_Language =  new Expression(this, null, xNodeLoop, ExpressionType.String);
						break;
					case "Code":
						_Code = new Code(this, null, xNodeLoop);
						break;
					case "CodeModules":
						_CodeModules = new CodeModules(this, null, xNodeLoop);
						break;
					case "Classes":
						_Classes = new Classes(this, null, xNodeLoop);
						break;
					case "DataTransform":
						_DataTransform = xNodeLoop.InnerText;
						break;
					case "DataSchema":
						_DataSchema = xNodeLoop.InnerText;
						break;
					case "DataElementName":
						_DataElementName = xNodeLoop.InnerText;
						break;
					case "DataElementStyle":
						_DataElementStyle = RDL.DataElementStyle.GetStyle(xNodeLoop.InnerText, this.rl);
						break;
					default:
						// don't know this element - log it
						this.rl.LogError(4, "Unknown Report element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}

			if (_Body == null)
				rl.LogError(8, "Body not specified for report.");

			if (_Width == null)
				rl.LogError(4, "Width not specified for report.  Assuming page width.");

			if (rl.MaxSeverity <= 4)	// don't do final pass if already have serious errors
			{
				FinalPass(folder);	// call final parser pass for expression resolution
			}

			// Cleanup any dangling resources
			if (_DataSourcesDefn != null)
				_DataSourcesDefn.CleanUp(null);
		}

		//
		void FinalPass(string folder)
		{
			// Now do some addition validation and final preparation

			// Create the Globals and User lookup dictionaries
			_LUGlobals = new ListDictionary();	// if entries grow beyond 10; make hashtable
			_LUGlobals.Add("PageNumber", new FunctionPageNumber());
			_LUGlobals.Add("TotalPages", new FunctionTotalPages());
			_LUGlobals.Add("ExecutionTime", new FunctionExecutionTime());
			_LUGlobals.Add("ReportFolder", new FunctionReportFolder());
			_LUGlobals.Add("ReportName", new FunctionReportName());
			_LUUser = new ListDictionary();		// if entries grow beyond 10; make hashtable
			_LUUser.Add("UserID", new FunctionUserID());
			_LUUser.Add("Language", new FunctionUserLanguage());
			if (_CodeModules != null)
			{
				_CodeModules.FinalPass();
				_CodeModules.LoadModules();
			}
			if (_Classes != null)
			{
				_Classes.FinalPass();
				// _Classes.Load();
			}
			if (_Code != null)
			{
				_Code.FinalPass();
				_CodeType = _Code.CodeType();
			}

			if (_ReportParameters != null)		// report parameters might be used in data source connection strings
				_ReportParameters.FinalPass();
			if (_DataSourcesDefn != null)
				_DataSourcesDefn.FinalPass();
			if (_DataSetsDefn != null)
				_DataSetsDefn.FinalPass();
			_Body.FinalPass();
			if (_PageHeader != null)
				_PageHeader.FinalPass();
			if (_PageFooter != null)
				_PageFooter.FinalPass();
			if (_EmbeddedImages != null)
				_EmbeddedImages.FinalPass();
			if (_Language != null)
				_Language.FinalPass();

            _DataCache.TrimExcess();	// reduce size of array of expressions that cache data
			return;
		}

		internal Type CodeType
		{
			get {return _CodeType;}
		}

		internal string ParseFolder
		{
			get {return _ParseFolder;}
		}

		internal int GetObjectNumber()
		{
			_ObjectCount++;
			return _ObjectCount;
		}

		internal void SetObjectNumber(int oc)
		{
			_ObjectCount = oc;
		}

		// Obtain the data for the report
		internal bool RunGetData(Report rpt, IDictionary parms)
		{
            bool bRows = false;
			// Step 1- set the parameter values for the runtime
            if (parms != null && ReportParameters != null)
            {
                ReportParameters.SetRuntimeValues(rpt, parms);	// set the parameters
            }

			// Step 2- prep the datasources (ie connect and execute the queries)
            if (this._DataSourcesDefn != null)
            {
                _DataSourcesDefn.ConnectDataSources(rpt);
            }

			// Step 3- obtain the data; applying filters
			if (_DataSetsDefn != null)
			{
				ResetCachedData(rpt);
				bRows = _DataSetsDefn.GetData(rpt);
			}

			// Step 4- cleanup any DB connections
            if (_DataSourcesDefn != null)
            {
                if (!this.ContainsSubreport)
                {
                    _DataSourcesDefn.CleanUp(rpt);	// no subreports means that nothing will use this transaction
                }
            }

			return bRows;
		}

		internal string CreateDynamicName(object ro)
		{
			_DynamicNames++;					// increment the name generator
			string name = "o" + _DynamicNames.ToString();
			_LUDynamicNames.Add(name, ro);
			return name;			
		}

		internal IDictionary LUDynamicNames
		{
			get { return _LUDynamicNames; }
		}
 
		private void ResetCachedData(Report rpt)
		{
            foreach (ICacheData icd in this._DataCache)
            {
                icd.ClearCache(rpt);
            }
		}

		internal void Run(IPresent ip)
		{
			if (_Subreport == null)
			{	// do true intialization
				ip.Start();
			}

			if (ip.IsPagingNeeded())
			{
				RunPage(ip);
			}
			else
			{
				if (_PageHeader != null && !(ip is RenderXml))
					_PageHeader.Run(ip, null);
				_Body.Run(ip, null);
				if (_PageFooter != null && !(ip is RenderXml))
					_PageFooter.Run(ip, null);
			}

			if (_Subreport == null)
				ip.End();

			if (_DataSourcesDefn != null)
				_DataSourcesDefn.CleanUp(ip.Report());	// datasets may not have been cleaned up
		}

		internal void RunPage(IPresent ip)
		{
			Pages pgs = new Pages(ip.Report());
			try
			{
				Page p = new Page(1);				// kick it off with a new page
				pgs.AddPage(p);

				// Create all the pages
				_Body.RunPage(pgs);

 				if (pgs.LastPage.IsEmpty()&& pgs.PageCount > 1)	// get rid of extraneous pages which
					pgs.RemoveLastPage();			//   can be caused by region page break at end

				// Now create the headers and footers for all the pages (as needed)
				if (_PageHeader != null)
					_PageHeader.RunPage(pgs);
				if (_PageFooter != null)
					_PageFooter.RunPage(pgs);

                pgs.SortPageItems();             // Handle ZIndex ordering of pages

				ip.RunPages(pgs);
			}
			finally
			{
				pgs.CleanUp();		// always want to make sure we clean this up since 
				if (_DataSourcesDefn != null)
					_DataSourcesDefn.CleanUp(pgs.Report);	// ensure datasets are cleaned up
			}

			return;
		}

		internal string Description
		{
			get { return  _Description; }
			set {  _Description = value; }
		}

		internal string Author
		{
			get { return  _Author; }
			set {  _Author = value; }
		}

		internal int AutoRefresh
		{
			get { return  _AutoRefresh; }
			set {  _AutoRefresh = value; }
		}

        internal List<ICacheData> DataCache
		{
			get { return _DataCache; }
		}

		internal DataSourcesDefn DataSourcesDefn
		{
			get { return  _DataSourcesDefn; }
		}

		internal DataSetsDefn DataSetsDefn
		{
			get { return  _DataSetsDefn; }
		}

		internal Body Body
		{
			get { return  _Body; }
			set {  _Body = value; }
		}

		internal Code Code
		{
			get {return _Code;}
		}

		internal ReportParameters ReportParameters
		{
			get { return  _ReportParameters; }
			set {  _ReportParameters = value; }
		}

		internal Custom Customer
		{
			get { return  _Customer; }
			set {  _Customer = value; }
		}

		internal string Name
		{
			get { return _Name == null? null: _Name.Nm; }
			set { _Name = new Name(value); }
		}

		internal RSize Width
		{
			get 
			{
				if (_Width == null)			// Shouldn't be need since technically Width is required (I let it slip)	
					_Width = PageWidth;		// Not specified; assume page width

				return  _Width; 
			}
			set {  _Width = value; }
		}

		internal PageHeader PageHeader
		{
			get { return  _PageHeader; }
			set {  _PageHeader = value; }
		}

		internal PageFooter PageFooter
		{
			get { return  _PageFooter; }
			set {  _PageFooter = value; }
		}

		internal RSize PageHeight
		{
			get 
			{
				if (this.Subreport != null)
					return Subreport.OwnerReport.PageHeight;

				if (_PageHeight == null)			// default height is 11 inches
					_PageHeight = new RSize(this, "11 in");
				return  _PageHeight; 
			}
			set {  _PageHeight = value; }
		}

		internal float PageHeightPoints
		{
			get 
			{
				return PageHeight.Points;
			}
		}

		internal RSize PageWidth
		{
			get 
			{
				if (this.Subreport != null)
					return Subreport.OwnerReport.PageWidth;

				if (_PageWidth == null)				// default width is 8.5 inches
					_PageWidth = new RSize(this, "8.5 in");

				return  _PageWidth; 
			}
			set {  _PageWidth = value; }
		}

		internal float PageWidthPoints
		{
			get 
			{
				return PageWidth.Points;
			}
		}

		internal RSize LeftMargin
		{
			get 
			{
                if (Subreport != null)
                {
                    if (Subreport.Left == null)
                        Subreport.Left = new RSize(this, "0 in");
                    return Subreport.Left + Subreport.OwnerReport.LeftMargin;
                }

				if (_LeftMargin == null)
					_LeftMargin = new RSize(this, "0 in");
				return  _LeftMargin; 
			}
			set {  _LeftMargin = value; }
		}

		internal RSize RightMargin
		{
			get 
			{ 
				if (Subreport != null)
					return Subreport.OwnerReport.RightMargin;

				if (_RightMargin == null)
					_RightMargin = new RSize(this, "0 in");
				return  _RightMargin; 
			}
			set {  _RightMargin = value; }
		}

		internal RSize TopMargin
		{
			get 
			{ 
				if (Subreport != null)
					return Subreport.OwnerReport.TopMargin;

				if (_TopMargin == null)
					_TopMargin = new RSize(this, "0 in");
				return  _TopMargin; 
			}
			set {  _TopMargin = value; }
		}

		internal float TopOfPage
		{
			get 
			{
				if (this.Subreport != null)
					return Subreport.OwnerReport.TopOfPage;

				float y = TopMargin.Points;
				if (this._PageHeader != null)
					y += _PageHeader.Height.Points;
				return y;
			}
		}

		internal RSize BottomMargin
		{
			get 
			{ 
				if (Subreport != null)
					return Subreport.OwnerReport.BottomMargin;

				if (_BottomMargin == null)
					_BottomMargin = new RSize(this, "0 in");
				return  _BottomMargin; 
			}
			set {  _BottomMargin = value; }
		}
 
		internal float BottomOfPage		// this is the y coordinate just above the page footer
		{
			get 
			{
				if (this.Subreport != null)
					return Subreport.OwnerReport.BottomOfPage;

				// calc size of bottom margin + footer
				float y = BottomMargin.Points;		
				if (this._PageFooter != null)
					y += _PageFooter.Height.Points;

				// now get the absolute coordinate
				y = PageHeight.Points - y;
				return y;
			}
		}

		internal EmbeddedImages EmbeddedImages
		{
			get { return  _EmbeddedImages; }
			set {  _EmbeddedImages = value; }
		}

		internal Expression Language
		{
			get { return  _Language; }
			set {  _Language = value; }
		}

		internal string EvalLanguage(Report rpt, Row r)
		{
			if (_Language == null)
			{
				CultureInfo ci = CultureInfo.CurrentCulture;
				return ci.Name;				
			}

			return _Language.EvaluateString(rpt, r);
		}

		internal CodeModules CodeModules
		{
			get { return  _CodeModules; }
			set {  _CodeModules = value; }
		}

		internal Classes Classes			  
		{
			get { return  _Classes; }
			set {  _Classes = value; }
		}

		internal string DataTransform
		{
			get { return  _DataTransform; }
			set {  _DataTransform = value; }
		}

		internal string DataSchema
		{
			get { return  _DataSchema; }
			set {  _DataSchema = value; }
		}

		internal string DataElementName
		{
			get 
			{
				return _DataElementName == null? "Report": _DataElementName;
			}
			set {  _DataElementName = value; }
		}

		internal DataElementStyleEnum DataElementStyle
		{
			get { return  _DataElementStyle; }
			set {  _DataElementStyle = value; }
		}

		internal IDictionary LUGlobals
		{
			get { return  _LUGlobals; }
		}

		internal IDictionary LUUser
		{
			get { return  _LUUser; }
		}
		
		internal IDictionary LUReportItems
		{
			get { return  _LUReportItems; }
		}
		
		internal IDictionary LUAggrScope
		{
			get { return  _LUAggrScope; }
		}

		internal IDictionary LUReportParameters
		{
			get 
			{
				if (_ReportParameters != null && 
					_ReportParameters.Items != null)
					return  _ReportParameters.Items; 
				else
					return null;
			}
		}

		internal IDictionary LUEmbeddedImages
		{
			get { return _LUEmbeddedImages; }
		}

		internal Subreport Subreport
		{
			get { return _Subreport; }
			set { _Subreport = value; }
		}

		internal bool ContainsSubreport
		{
			get {return _ContainsSubreport;}
			set {_ContainsSubreport = value;}
		}

		internal string OverwriteConnectionString
		{
			get {return _OverwriteConnectionString;}
			set {_OverwriteConnectionString = value;}
		}

		internal bool OverwriteInSubreport
		{
			get {return _OverwriteInSubreport;}
			set {_OverwriteInSubreport = value;}
		}

		internal int ErrorMaxSeverity
		{
			get 
			{
				if (this.rl == null)
					return 0;
				else
					return rl.MaxSeverity;
			}
		}

		internal IList ErrorItems
		{
			get
			{
				if (this.rl == null)
					return null;
				else
					return rl.ErrorItems;
			}
		}

		internal void ErrorReset()
		{
			if (this.rl == null)
				return;
			rl.Reset();
			return;
		}
	}
}
