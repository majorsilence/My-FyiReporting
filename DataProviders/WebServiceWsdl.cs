
using System;
using System.Xml;
using System.Data;
using System.Collections;
#if !NETSTANDARD2_0 && !NET6_0_OR_GREATER
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
#endif
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using Microsoft.CSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// WebServiceWsdl handles generation and caching of Assemblies containing WSDL proxies
	///   It also will invoke proxies with the proper arguments.  These arguments must be 
	///   provided as a WebServiceParameter.
	/// </summary>
	public class WebServiceWsdl
	{
		// Cache the compiled assemblies
		const string _Namespace = "fyireporting.ws";
		static Hashtable _cache = Hashtable.Synchronized(new Hashtable());	
		string _url;					// url for this assembly
		Assembly _WsdlAssembly;			// Assembly ready for invokation

		static internal WebServiceWsdl GetWebServiceWsdl(string url)
		{
			WebServiceWsdl w = _cache[url] as WebServiceWsdl;
			if (w != null)
				return w;

			return new WebServiceWsdl(url);
		}

		static public void ClearCache()
		{
			_cache.Clear();
		}

		public MethodInfo GetMethodInfo(string service, string operation)
		{
			// Create an instance of the service object proxy   
			object o = _WsdlAssembly.CreateInstance(_Namespace + "." + service, false);
			if (o == null)
				throw new Exception(string.Format("Unable to create instance of service '{0}'.", service));

			// Get information about the method
			MethodInfo mi = o.GetType().GetMethod(operation);
			if (mi == null)
				throw new Exception(string.Format("Unable to find operation '{0}' in service '{1}'.", operation, service));

			return mi;
		}

		// Invoke the operation for the requested service
		public object Invoke(string service, string operation, DataParameterCollection dpc, int timeout)
		{
			// Create an instance of the service object proxy
			object o = _WsdlAssembly.CreateInstance(_Namespace + "." + service, false);
			if (o == null)
				throw new Exception(string.Format("Unable to create instance of service '{0}'.", service));

			// Get information about the method
			MethodInfo mi = o.GetType().GetMethod(operation);
			if (mi == null)
				throw new Exception(string.Format("Unable to find operation '{0}' in service '{1}'.", operation, service));

			// Go thru the parameters building up an object array with the proper parameters
			ParameterInfo[] pis = mi.GetParameters();
			object[] args = new object[pis.Length];
			int ai=0;
			foreach (ParameterInfo pi in pis)
			{
				BaseDataParameter dp = dpc[pi.Name] as BaseDataParameter;
				if (dp == null)		// retry with '@' in front!
					dp = dpc["@"+pi.Name] as BaseDataParameter;
				if (dp == null || dp.Value == null)
					args[ai] = null;
				else if (pi.ParameterType == dp.Value.GetType())
					args[ai] = dp.Value;
				else	// we need to do conversion
					args[ai] = Convert.ChangeType(dp.Value, pi.ParameterType);
				ai++;
			}
#if !NETSTANDARD2_0 && !NET6_0_OR_GREATER
            SoapHttpClientProtocol so = o as SoapHttpClientProtocol;
			if (so != null && timeout != 0)
				so.Timeout = timeout;
			return mi.Invoke(o, args);
#else
            throw new NotImplementedException("Some classes are not available on .NET STANDARD");
#endif
		}

		// constructor
		private WebServiceWsdl(string url)
		{
			_url = url;						
			_WsdlAssembly = GetAssembly();
			_cache.Add(url, this);
		}

		private Assembly GetAssembly()
		{
#if !NETSTANDARD2_0 && !NET6_0_OR_GREATER
            ServiceDescription sd = GetServiceDescription();

			// ServiceDescriptionImporter provide means for generating client proxy classes for XML Web services 
			CodeNamespace cns = new CodeNamespace(_Namespace);
			ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
			sdi.AddServiceDescription(sd, null, null);
			sdi.ProtocolName = "Soap";
			sdi.Import(cns, null);

			// Generate the proxy source code
			CSharpCodeProvider cscp = new CSharpCodeProvider();
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			cscp.GenerateCodeFromNamespace(cns, sw, null);
			string proxy = sb.ToString();
			sw.Close();

			// debug code !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//			StreamWriter tsw = File.CreateText(@"c:\temp\proxy.cs");
//			tsw.Write(proxy);
//			tsw.Close();
			// debug code !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   

			// Create Assembly
			CompilerParameters cp = new CompilerParameters();
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.Xml.dll");
			cp.ReferencedAssemblies.Add("System.Web.Services.dll");
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = false;			// just loading into memory causes problems when instantiating
			cp.IncludeDebugInformation = false; 
			CompilerResults cr = cscp.CompileAssemblyFromSource(cp, proxy);
			if(cr.Errors.Count > 0)
			{
				StringBuilder err = new StringBuilder(string.Format("WSDL proxy compile has {0} error(s).", cr.Errors.Count));
				foreach (CompilerError ce in cr.Errors)
				{
					err.AppendFormat("\r\n{0} {1}", ce.ErrorNumber, ce.ErrorText);
				}
				throw new Exception(err.ToString()); 
			}

			return Assembly.LoadFrom(cr.PathToAssembly);	// We need an assembly loaded from the file system
															//   or instantiation of object complains
#else
            throw new NotImplementedException("Some classes are missing on .NET STANDARD");
#endif
		}

        async Task<Stream> GetStream()
        {
            string fname = _url;
            Stream strm = null;

            if (fname.StartsWith("http:") || fname.StartsWith("file:") || fname.StartsWith("https:"))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.AddMajorsilenceReportingUserAgent();
                    HttpResponseMessage response = await client.GetAsync(fname);
                    if (response.IsSuccessStatusCode)
                    {
                        strm = await response.Content.ReadAsStreamAsync();
                    }
                    else
                    {
                        throw new Exception($"Failed to get response from {fname}. Status code: {response.StatusCode}");
                    }
                }
            }
            else
            {
                strm = new FileStream(fname, System.IO.FileMode.Open, FileAccess.Read);
            }

            return strm;
        }

	}
}
