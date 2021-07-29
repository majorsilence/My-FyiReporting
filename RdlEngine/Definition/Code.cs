//This file has been modified based on forum user suggestions.

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
using System.Xml;
using System.Reflection;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using System.Linq;
using Microsoft.CodeAnalysis.Emit;

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
			_Source = xNode.InnerText;
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
			_Classname = "MyClass" + unique;
			lines.AddRange(new string[]
				{
					"Imports System",
					"Imports Microsoft.VisualBasic",
					"Imports System.Convert",
					"Imports System.Math",
					"Imports fyiReporting.RDL",
					"Namespace fyiReporting.vbgen",
					"Public Class " + _Classname,
					"Private Shared _report As CodeReport",
					"Sub New()",
					"End Sub",
					"Sub New(byVal def As Report)",
					_Classname + "._report = New CodeReport(def)",
					"End Sub",
					"Public Shared ReadOnly Property Report As CodeReport",
					"Get",
					"Return " + _Classname + "._report",
					"End Get",
					"End Property"
				});

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

			Console.WriteLine("Parsing the code into the SyntaxTree");

			SyntaxTree syntaxTree = VisualBasicSyntaxTree.ParseText(vbcode);

			string re;
			//AJM GJL 250608 - Try the Bin Directory too, for websites
			if (RdlEngineConfig.DirectoryLoadedFrom == null)
			{
				if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RdlEngine.dll")))
				{
					re = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RdlEngine.dll");   // this can fail especially in web scenarios
				}
				else
				{
					re = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");   // this can work especially in web scenarios
					re = Path.Combine(re, "RdlEngine.dll");
				}
			}
			else
            {
				re = Path.Combine(RdlEngineConfig.DirectoryLoadedFrom, "RdlEngine.dll");     // use RdlEngineConfig.xml directory when available
			}

			var tempFile = Path.GetTempFileName();

			string assemblyName = Path.GetFileNameWithoutExtension(tempFile);
			List<string> refPaths = new List<string>();

			refPaths.AddRange(new[] {
				typeof(System.Object).GetTypeInfo().Assembly.Location,
				typeof(System.Convert).GetTypeInfo().Assembly.Location,
				typeof(System.Math).GetTypeInfo().Assembly.Location,
				typeof(Microsoft.VisualBasic.Strings).GetTypeInfo().Assembly.Location,
				typeof(fyiReporting.RDL.Report).GetTypeInfo().Assembly.Location,
				Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
				re
			});

			if (this.OwnerReport.CodeModules != null)
			{
				foreach (CodeModule cm in this.OwnerReport.CodeModules.Items)
				{
					//Changed from Forum, User: solidstate http://www.fyireporting.com/forum/viewtopic.php?t=905
					string modulePath = Path.Combine(Path.GetDirectoryName(re), cm.CdModule);
					refPaths.Add(modulePath);
				}
			}

			MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

			Console.WriteLine("Compiling ...");

			VisualBasicCompilation compilation = VisualBasicCompilation.Create(
				assemblyName,
				syntaxTrees: new[] { syntaxTree },
				references: references,
				options: new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
			);

			EmitResult result = compilation.Emit(tempFile);

			if (!result.Success)
			{
				IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
					diagnostic.IsWarningAsError ||
					diagnostic.Severity == DiagnosticSeverity.Error);

				StringBuilder err = new StringBuilder($"Code element has { failures.Count() } error(s).  Line numbers are relative to Code element.");

				foreach (Diagnostic diagnostic in failures)
				{
					err.AppendLine($"{ diagnostic.Id }: { diagnostic.GetMessage() }");
				}
					
				OwnerReport.rl.LogError(4, err.ToString());
				return null;
			}

			Console.Write("Compilation successful! Now instantiating and executing the code ...");

			return Assembly.LoadFrom(tempFile);
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
