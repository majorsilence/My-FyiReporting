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
using System.Xml;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace fyiReporting.RDL
{
	///<summary>
	/// Code represents the Code report element. 
	///</summary>
	[Serializable]
	internal class Code : ReportLink
	{
		string _Source;			// The source code
		string _Classname;		// Class name of generated class
		Assembly _Assembly;		// the compiled assembly
	
		internal Code(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Source=xNode.InnerText;
			_Assembly = GetAssembly();
		}
		
		override internal void FinalPass()
		{
			return;
		}

		private Assembly GetAssembly()
		{
			// Generate the proxy source code
            List<string> lines = new List<string>();		// hold lines in array in case of error

			VBCodeProvider vbcp =  new VBCodeProvider();
			StringBuilder sb = new StringBuilder();
			//  Generate code with the following general form

			//Imports System
            //Imports Microsoft.VisualBasic
            //Imports System.Convert
            //Imports System.Math 
            //Namespace fyiReporting.vbgen
			//Public Class MyClassn	   // where n is a uniquely generated integer
			//Sub New()
			//End Sub
			//  ' this is the code in the <Code> tag
			//End Class
			//End Namespace
			string unique = Interlocked.Increment(ref Parser.Counter).ToString();
			lines.Add("Imports System");
            lines.Add("Imports Microsoft.VisualBasic");
            lines.Add("Imports System.Convert");
            lines.Add("Imports System.Math");
            lines.Add("Imports fyiReporting.RDL");
            lines.Add("Namespace fyiReporting.vbgen");
			_Classname = "MyClass" + unique;
			lines.Add("Public Class " + _Classname);
            lines.Add("Private Shared _report As CodeReport");
			lines.Add("Sub New()");
			lines.Add("End Sub");
            lines.Add("Sub New(byVal def As Report)");
            lines.Add(_Classname + "._report = New CodeReport(def)");
            lines.Add("End Sub");
            lines.Add("Public Shared ReadOnly Property Report As CodeReport");
            lines.Add("Get");
            lines.Add("Return " + _Classname + "._report");
            lines.Add("End Get");
            lines.Add("End Property");
			// Read and write code as lines
			StringReader tr = new StringReader(_Source);
			while (tr.Peek() >= 0)
			{
				string line = tr.ReadLine();
				lines.Add(line);
			}
			tr.Close();
			lines.Add("End Class");
			lines.Add("End Namespace");
            foreach (string l in lines)
            {
                sb.Append(l);
                sb.Append("\r\n");
            }

			string vbcode = sb.ToString();

			// debug code !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//						StreamWriter tsw = File.CreateText(@"c:\temp\vbcode.txt");
//						tsw.Write(vbcode);
//						tsw.Close();
			// debug code !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   

			// Create Assembly
			CompilerParameters cp = new CompilerParameters();
			cp.ReferencedAssemblies.Add("System.dll");
            string re;
            //AJM GJL 250608 - Try the Bin Directory too, for websites
            if (RdlEngineConfig.DirectoryLoadedFrom == null) {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "RdlEngine.dll")) {
                    re = AppDomain.CurrentDomain.BaseDirectory + "RdlEngine.dll";   // this can fail especially in web scenarios
                }
                else
                {
                    re = AppDomain.CurrentDomain.BaseDirectory + "Bin\\RdlEngine.dll";   // this can work especially in web scenarios
                }
            }
            else
                re = RdlEngineConfig.DirectoryLoadedFrom + "RdlEngine.dll";     // use RdlEngineConfig.xml directory when available

			cp.ReferencedAssemblies.Add(re);
            // also allow access to classes that have been added to report
            if (this.OwnerReport.CodeModules != null)
            {
                foreach(CodeModule cm in this.OwnerReport.CodeModules.Items)
                {
                    cp.ReferencedAssemblies.Add(cm.CdModule);
                }
            }
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = false;			// just loading into memory causes problems when instantiating
			cp.IncludeDebugInformation = false; 
			CompilerResults cr = vbcp.CompileAssemblyFromSource(cp, vbcode);
			if(cr.Errors.Count > 0)
			{
				StringBuilder err = new StringBuilder(string.Format("Code element has {0} error(s).  Line numbers are relative to Code element.", cr.Errors.Count));
				foreach (CompilerError ce in cr.Errors)
				{
					string l;
					if (ce.Line >= 1 && ce.Line <= lines.Count)
						l = lines[ce.Line - 1] as string;
					else
						l = "Unknown";
					err.AppendFormat("\r\nLine {0} '{1}' : {2} {3}", ce.Line - 5, l, ce.ErrorNumber, ce.ErrorText);
				}
				this.OwnerReport.rl.LogError(4, err.ToString());
				return null;
			}

			return Assembly.LoadFrom(cr.PathToAssembly);	// We need an assembly loaded from the file system
			//   or instantiation of object complains
		}

		internal Type CodeType()
		{
			if (_Assembly == null)
				return null;

			Type t=null;
			try
			{
				object instance = _Assembly.CreateInstance("fyiReporting.vbgen." + this._Classname, false); 
				t = instance.GetType();
			}
			catch (Exception e)
			{
				OwnerReport.rl.LogError(4, 
					string.Format("Unable to load instance of Code\r\n{0}", e.Message));
			}
			return t;
		}

		internal object Load(Report rpt)
		{
			WorkClass wc = GetWC(rpt);
			if (wc.bCreateFailed)		// We only try to create once.
				return wc.Instance;

			if (wc.Instance != null)	// Already loaded
				return wc.Instance;

			if (_Assembly == null)
			{
				wc.bCreateFailed = true;	// we don't have an assembly
				return null;
			}

			// Load an instance of the object
			string err="";
			try
			{
                object[] args = new object[1];
                args[0] = rpt;
				wc.Instance = _Assembly.CreateInstance("fyiReporting.vbgen." + this._Classname, false, BindingFlags.CreateInstance, null, args, null, null); 
			}
			catch (Exception e)
			{
				wc.Instance = null;
				err = e.Message;
			}

			if (wc.Instance == null)
			{
				string e = String.Format("Unable to create instance of local code class.\r\n{0}", err);
				if (rpt == null)
					OwnerReport.rl.LogError(4, e);
				else
					rpt.rl.LogError(4, e);
				wc.bCreateFailed = true;
			}
			return wc.Instance;			
		}

		internal string Source
		{
			get { return  _Source; }
		}

		internal object Instance(Report rpt)
		{
			return Load(rpt);			// load if necessary
		}

		private WorkClass GetWC(Report rpt)
		{
			if (rpt == null)
				return new WorkClass();

			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void RemoveWC(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		class WorkClass
		{
			internal object Instance;
			internal bool bCreateFailed;
			internal WorkClass()
			{
				Instance=null;	// 
				bCreateFailed=false;
			}
		}
	}

    // The CodeReport code was donated to the RDL Project by "solidstore" of the forum.
    // The classes defined below are for use by the VB code runtime.

    /// <summary>
    /// This class is only for use with the VB code generation.  
    /// </summary>
    public class CodeReport
    {
        private readonly CodeGlobals globals;
        private readonly CodeParameters parameters;
        private readonly CodeUser user;

        public CodeReport(Report report)
        {
            this.globals = new CodeGlobals(report);
            this.parameters = new CodeParameters(report);
            this.user = new CodeUser(report);
        }

        public CodeGlobals Globals
        {
            get
            {
                return this.globals;
            }
        }

        public CodeParameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public CodeUser User
        {
            get
            {
                return this.user;
            }
        }
    }

    /// <summary>
    /// This class is only for use with the VB code generation.  
    /// </summary>
    public class CodeParameters
    {
        private readonly Report report;

        public CodeParameters(Report report)
        {
            this.report = report;
        }

        public object this[string key]
        {
            get
            {
                foreach (UserReportParameter p in report.UserReportParameters)
                {
                    if (p.Name.Equals(key))
                    {
                        return p;
                    }
                }
                return null;
            }
        }
    }

    /// <summary>
    /// This class is only for use with the VB code generation.  
    /// </summary>
    public class CodeUser
    {
        private readonly Report report;

        public CodeUser(Report report)
        {
            this.report = report;
        }

        public object this[string key]
        {
            get
            {
                switch (key.ToLower())
                {
                    case "userid":
                        return report.UserID;
                    case "language":
                        return report.ClientLanguage;
                }
                return null;
            }
        }

        public string Language
        {
            get
            {
                return report.ClientLanguage;
            }
        }

        public string UserID
        {
            get
            {
                return report.UserID;
            }
        }
    }
/// <summary>
/// This class is only for use with the VB code generation.  
/// </summary>
    public class CodeGlobals
    {
        private readonly Report report;

        public CodeGlobals(Report report)
        {
            this.report = report;
        }

        public object this[string key]
        {
            get
            {
                switch (key.ToLower())
                {
                    case "pagenumber":
                        return report.PageNumber;
                    case "totalpages":
                        return report.TotalPages;
                    case "executiontime":
                        return report.ExecutionTime;
                    case "reportfolder":
                        return report.Folder;
                    case "reportname":
                        return report.Name;
                }
                return null;
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                return report.ExecutionTime;
            }
        }
        public int PageNumber
        {
            get
            {
                return report.PageNumber;
            }
        }

        public string ReportFolder
        {
            get
            {
                return report.Folder;
            }
        }

        public string ReportName
        {
            get
            {
                return report.Name;
            }
        }

        public int TotalPages
        {
            get
            {
                return report.TotalPages;
            }
        }
    } 

}
