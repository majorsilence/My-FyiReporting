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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
    ///<summary> 
    /// Handle SQL configuration and connections 
    ///</summary> 
    public class RdlEngineConfig
    {
        static internal IDictionary SqlEntries = null;   // list of entries 
        static internal Dictionary<string, CustomReportItemEntry> CustomReportItemEntries = null;
        static DateTime _InitFileCreationTime = DateTime.MinValue;

        // Compression entries 
        static CompressionConfig _Compression = null;
        static string _DirectoryLoadedFrom = null;

        static public string DirectoryLoadedFrom
        {
            get
            {
                return _DirectoryLoadedFrom;
            }
        }
        // initializes when no init available 
        static public void RdlEngineConfigInit()
        {
            string d1, d2;
            d1 = AppDomain.CurrentDomain.BaseDirectory;
            d2 = AppDomain.CurrentDomain.RelativeSearchPath;
            if (d2 != null && d2 != string.Empty)
                d2 = (d2.Contains(":") ? "" : AppDomain.CurrentDomain.BaseDirectory) + d2 + Path.DirectorySeparatorChar;
            RdlEngineConfigInit(d1, d2);
        }

        // initialize configuration 
        static public void RdlEngineConfigInit(params string[] dirs)
        {
            bool bLoaded = false;
            XmlDocument xDoc = new XmlDocument();
            xDoc.PreserveWhitespace = false;
            string file = null;
            DateTime fileTime = DateTime.MinValue;

            foreach (string dir in dirs)
            {
                if (dir == null)
                    continue;
                file = dir + "RdlEngineConfig.xml";
                try
                {
                    FileInfo fi = new FileInfo(file);
                    fileTime = fi.CreationTime;
                    if (_InitFileCreationTime == fileTime && SqlEntries != null)
                        return;         // we've already inited with this file 
                    xDoc.Load(file);
                    bLoaded = true;
                    _DirectoryLoadedFrom = dir;
                }
                catch (Exception ex)
                {   // can't do much about failures; no place to report them 
                    System.Console.WriteLine("Error opening RdlEngineConfig.xml: {0}", ex.Message);
                }
                if (bLoaded)
                    break;
            }
            if (!bLoaded)   // we couldn't find the configuration so we'll use internal one 
            {
                if (SqlEntries != null)         // we don't need to reinit with internal one 
                    return;
                xDoc.InnerXml = @" 
<config> 
   <DataSources> 
      <DataSource> 
         <DataProvider>SQL</DataProvider> 
         <TableSelect>SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY 2, 1</TableSelect> 
         <Interface>SQL</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>ODBC</DataProvider> 
         <TableSelect>SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY 2, 1</TableSelect> 
         <Interface>SQL</Interface> 
         <ReplaceParameters>true</ReplaceParameters> 
      </DataSource> 
      <DataSource> 
         <DataProvider>OLEDB</DataProvider> 
         <TableSelect>SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY 2, 1</TableSelect> 
         <Interface>SQL</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>XML</DataProvider> 
         <CodeModule>DataProviders.dll</CodeModule> 
         <ClassName>fyiReporting.Data.XmlConnection</ClassName> 
         <TableSelect></TableSelect> 
         <Interface>File</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>WebService</DataProvider> 
         <CodeModule>DataProviders.dll</CodeModule> 
         <ClassName>fyiReporting.Data.WebServiceConnection</ClassName> 
         <TableSelect></TableSelect> 
         <Interface>WebService</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>WebLog</DataProvider> 
         <CodeModule>DataProviders.dll</CodeModule> 
         <ClassName>fyiReporting.Data.LogConnection</ClassName> 
         <TableSelect></TableSelect> 
         <Interface>File</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>Text</DataProvider> 
         <CodeModule>DataProviders.dll</CodeModule> 
         <ClassName>fyiReporting.Data.TxtConnection</ClassName> 
         <TableSelect></TableSelect> 
         <Interface>File</Interface> 
      </DataSource> 
      <DataSource> 
         <DataProvider>FileDirectory</DataProvider> 
         <CodeModule>DataProviders.dll</CodeModule> 
         <ClassName>fyiReporting.Data.FileDirConnection</ClassName> 
         <TableSelect></TableSelect> 
         <Interface>File</Interface> 
      </DataSource> 
   </DataSources> 
    <Compression> 
      <CodeModule>ICSharpCode.SharpZipLib.dll</CodeModule> 
      <ClassName>ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream</ClassName> 
      <Finish>Finish</Finish> 
      <Enable>true</Enable> 
   </Compression> 
  <CustomReportItems> 
    <CustomReportItem> 
      <Type>BarCode</Type> 
      <CodeModule>RdlCri.dll</CodeModule> 
      <ClassName>fyiReporting.CRI.BarCode</ClassName> 
    </CustomReportItem> 
  </CustomReportItems> 
</config>";
            }
            XmlNode xNode;
            xNode = xDoc.SelectSingleNode("//config");
             
            IDictionary dsDir = new ListDictionary();
            Dictionary<string, CustomReportItemEntry> crieDir =
                new Dictionary<string, CustomReportItemEntry>();   // list of entries 

            // Loop thru all the child nodes 
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {
                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                switch (xNodeLoop.Name)
                {
                    case "DataSources":
                        GetDataSources(dsDir, xNodeLoop);
                        break;
                    case "Compression":
                        GetCompression(xNodeLoop);
                        break;
                    case "CustomReportItems":
                        GetCustomReportItems(crieDir, xNodeLoop);
                        break;
                    default:
                        break;
                }
            }

            SqlEntries = dsDir;
            CustomReportItemEntries = crieDir;
            _InitFileCreationTime = fileTime;           // save initialization time 

            return;
        }

        internal static CompressionConfig GetCompression()
        {
            if (SqlEntries == null)
                RdlEngineConfigInit();      // init if necessary 

            return _Compression;
        }

        static void GetCompression(XmlNode xNode)
        {
            // loop thru looking to process all the datasource elements 
            string cm = null;
            string cn = null;
            string fn = null;
            bool bEnable = true;
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {

                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                switch (xNodeLoop.Name)
                {
                    case "CodeModule":
                        if (xNodeLoop.InnerText.Length > 0)
                            cm = xNodeLoop.InnerText;
                        break;
                    case "ClassName":
                        if (xNodeLoop.InnerText.Length > 0)
                            cn = xNodeLoop.InnerText;
                        break;
                    case "Finish":
                        if (xNodeLoop.InnerText.Length > 0)
                            fn = xNodeLoop.InnerText;
                        break;
                    case "Enable":
                        if (xNodeLoop.InnerText.ToLower() == "false")
                            bEnable = false;
                        break;
                }

            }
            if (bEnable)
                _Compression = new CompressionConfig(cm, cn, fn);
            else
                _Compression = null;
        }

        static void GetDataSources(IDictionary dsDir, XmlNode xNode)
        {
            // loop thru looking to process all the datasource elements 
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {
                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                if (xNodeLoop.Name != "DataSource")
                    continue;
                GetDataSource(dsDir, xNodeLoop);
            }
        }

        static void GetDataSource(IDictionary dsDir, XmlNode xNode)
        {
            string provider = null;
            string codemodule = null;
            string cname = null;
            string inter = "SQL";
            string tselect = null;
            bool replaceparameters = false;
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {
                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                switch (xNodeLoop.Name)
                {
                    case "DataProvider":
                        provider = xNodeLoop.InnerText;
                        break;
                    case "CodeModule":
                        codemodule = xNodeLoop.InnerText;
                        break;
                    case "Interface":
                        inter = xNodeLoop.InnerText;
                        break;
                    case "ClassName":
                        cname = xNodeLoop.InnerText;
                        break;
                    case "TableSelect":
                        tselect = xNodeLoop.InnerText;
                        break;
                    case "ReplaceParameters":
                        if (xNodeLoop.InnerText.ToLower() == "true")
                            replaceparameters = true;
                        break;
                    default:
                        break;
                }
            }
            if (provider == null)
                return;         // nothing to do if no provider specified 

            SqlConfigEntry sce;
            try
            {   // load the module early; saves problems with concurrency later 
                string msg = null;
                Assembly la = null;
                if (codemodule != null && cname != null)
                {
                    // check to see if the DLL has been previously loaded 
                    //   many of the DataProvider done by fyiReporting are in a single code module 
                    foreach (SqlConfigEntry sc in dsDir.Values)
                    {
                        if (sc.FileName == codemodule &&
                            sc.CodeModule != null)
                        {
                            la = sc.CodeModule;
                            break;
                        }
                    }
                    if (la == null)
                        la = XmlUtil.AssemblyLoadFrom(codemodule);
                    if (la == null)
                        msg = string.Format("{0} could not be loaded", codemodule);
                }
                sce = new SqlConfigEntry(provider, codemodule, cname, la, tselect, msg);
                dsDir.Add(provider, sce);
            }
            catch (Exception e)
            {      // keep exception;  if this DataProvided is ever useed we will see the message 
                sce = new SqlConfigEntry(provider, codemodule, cname, null, tselect, e.Message);
                dsDir.Add(provider, sce);
            }
            sce.ReplaceParameters = replaceparameters;
        }

        public static IDbConnection GetConnection(string provider, string cstring)
        {
            IDbConnection cn = null;
            switch (provider.ToLower())
            {
                case "sql":
                    // can't connect unless information provided; 
                    //   when user wants to set the connection programmatically this they should do this 
                    if (cstring.Length > 0)
                        cn = new SqlConnection(cstring);
                    break;
                case "odbc":
                    cn = new OdbcConnection(cstring);
                    break;
                case "oledb":
                    cn = new OleDbConnection(cstring);
                    break;
                default:
                    if (SqlEntries == null)         // if never initialized; we should init 
                        RdlEngineConfigInit();
                    SqlConfigEntry sce = SqlEntries[provider] as SqlConfigEntry;
                    if (sce == null || sce.CodeModule == null)
                    {
                        if (sce != null && sce.ErrorMsg != null)   // error during initialization?? 
                            throw new Exception(sce.ErrorMsg);
                        break;
                    }
                    object[] args = new object[] { cstring };
                    Assembly asm = sce.CodeModule;
                    object o = asm.CreateInstance(sce.ClassName, false,
                       BindingFlags.CreateInstance, null, args, null, null);
                    if (o == null)
                        throw new Exception(string.Format("Unable to create instance of '{0}' for provider '{1}'", sce.ClassName, provider));
                    cn = o as IDbConnection;
                    break;
            }

            return cn;
        }

        static public string GetTableSelect(string provider)
        {
            return GetTableSelect(provider, null);
        }

        static public bool DoParameterReplacement(string provider, IDbConnection cn)
        {
            if (SqlEntries == null)
                RdlEngineConfigInit();
            SqlConfigEntry sce = SqlEntries[provider] as SqlConfigEntry;
            return sce == null ? false : sce.ReplaceParameters;
        }

        static public string GetTableSelect(string provider, IDbConnection cn)
        {
            if (SqlEntries == null)
                RdlEngineConfigInit();
            SqlConfigEntry sce = SqlEntries[provider] as SqlConfigEntry;
            if (sce == null)
            {
                if (cn != null)
                {
                    OdbcConnection oc = cn as OdbcConnection;
                    if (oc != null && oc.Driver.ToLower().IndexOf("my") >= 0)   // not a good way but ... 
                        return "show tables";               // mysql syntax is non-standard 
                }
                return "SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY 2, 1";
            }
            if (cn != null)
            {
                OdbcConnection oc = cn as OdbcConnection;
                if (oc != null && oc.Driver.ToLower().IndexOf("my") >= 0)   // not a good way but ... 
                    return "show tables";               // mysql syntax is non-standard 
            }
            return sce.TableSelect;
        }

        static public string[] GetProviders()
        {
            if (SqlEntries == null)
                RdlEngineConfigInit();
            if (SqlEntries.Count == 0)
                return null;
            string[] items = new string[SqlEntries.Count];
            int i = 0;
            foreach (SqlConfigEntry sce in SqlEntries.Values)
            {
                items[i++] = sce.Provider;
            }
            return items;
        }

        static public string[] GetCustomReportTypes()
        {
            if (CustomReportItemEntries == null)
                RdlEngineConfigInit();
            if (CustomReportItemEntries.Count == 0)
                return null;
            string[] items = new string[CustomReportItemEntries.Count];
            int i = 0;
            foreach (CustomReportItemEntry crie in CustomReportItemEntries.Values)
            {
                items[i++] = crie.ItemName;
            }
            return items;
        }

        static void GetCustomReportItems(Dictionary<string, CustomReportItemEntry> crieDir, XmlNode xNode)
        {
            // loop thru looking to process all the datasource elements 
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {
                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                if (xNodeLoop.Name != "CustomReportItem")
                    continue;
                GetCustomReportItem(crieDir, xNodeLoop);
            }
        }

        static void GetCustomReportItem(Dictionary<string, CustomReportItemEntry> crieDir, XmlNode xNode)
        {
            string friendlyTypeName = null;
            string codemodule = null;
            string classname = null;
            foreach (XmlNode xNodeLoop in xNode.ChildNodes)
            {
                if (xNodeLoop.NodeType != XmlNodeType.Element)
                    continue;
                switch (xNodeLoop.Name)
                {
                    case "Type":
                        friendlyTypeName = xNodeLoop.InnerText;
                        break;
                    case "CodeModule":
                        codemodule = xNodeLoop.InnerText;
                        break;
                    case "ClassName":
                        classname = xNodeLoop.InnerText;
                        break;
                    default:
                        break;
                }
            }
            if (friendlyTypeName == null)
                return;         // nothing to do if no provider specified 

            CustomReportItemEntry crie;
            try
            {   // load the module early; saves problems with concurrency later 
                string msg = null;
                Type dotNetType = null;
                Assembly la = null;
                if (codemodule != null && classname != null)
                {
                    // Check to see if previously loaded.  Many CustomReportItems share same CodeModule. 
                    Assembly[] allLoadedAss = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly ass in allLoadedAss)
                        if (ass.Location.Equals(codemodule, StringComparison.CurrentCultureIgnoreCase))
                        {
                            la = ass;
                            break;
                        }

                    if (la == null)     // not previously loaded? 
                        la = XmlUtil.AssemblyLoadFrom(codemodule);
                    if (la == null)
                        msg = string.Format("{0} could not be loaded", codemodule);
                    else
                        dotNetType = la.GetType(classname);
                }

                crie = new CustomReportItemEntry(friendlyTypeName, dotNetType, msg);
                crieDir.Add(friendlyTypeName, crie);
            }
            catch (Exception e)
            {      // keep exception;  if this CustomReportItem is ever used we will see the message 
                crie = new CustomReportItemEntry(friendlyTypeName, null, e.Message);
                crieDir.Add(friendlyTypeName, crie);
            }
        }

        public static ICustomReportItem CreateCustomReportItem(string friendlyTypeName)
        {
            CustomReportItemEntry crie = null;
            if (!CustomReportItemEntries.TryGetValue(friendlyTypeName, out crie))
                throw new Exception(string.Format("{0} is not a known CustomReportItem type", friendlyTypeName));
            if (crie.Type == null)
                throw new Exception(crie.ErrorMsg ??
               string.Format("{0} is not a known CustomReportItem type", friendlyTypeName));

            ICustomReportItem item = (ICustomReportItem)Activator.CreateInstance(crie.Type);
            return item;
        }

        public static void DeclareNewCustomReportItem(string itemName, Type type)
        {
            if (!typeof(ICustomReportItem).IsAssignableFrom(type))
                throw new ArgumentException("The type does not implement the ICustomReportItem interface: " +
                   type == null ? "null" : type.Name);

            if (CustomReportItemEntries == null)
                RdlEngineConfigInit();

            // Let's manage doublons, if any. 
            CustomReportItemEntry item;
            if (!CustomReportItemEntries.TryGetValue(itemName, out item))
                CustomReportItemEntries[itemName] = new CustomReportItemEntry(itemName, type, null);
            else if (!item.Type.Equals(type))
                throw new ArgumentException("A different type of CustomReportItem with the same has already been declared.");
        }
    }

    internal class CompressionConfig
    {
        int _UseCompression = -1;
        Assembly _Assembly = null;
        string _CodeModule;
        string _ClassName;
        string _Finish;
        MethodInfo _FinishMethodInfo;   //   if there is a finish method 
        string _ErrorMsg;            // error encountered loading compression 

        internal CompressionConfig(string cm, string cn, string fn)
        {
            _CodeModule = cm;
            _ClassName = cn;
            _Finish = fn;
            if (cm == null || cn == null || fn == null)
                _UseCompression = 2;
        }

        internal bool CanCompress
        {
            get
            {
                if (_UseCompression >= 1)   // we've already successfully inited 
                    return true;
                if (_UseCompression == 0)   // we've tried to init and failed 
                    return false;
                Init();                  // initialize compression 
                return _UseCompression == 1;   // and return the status 
            }
        }

        internal void CallStreamFinish(Stream strm)
        {
            if (_UseCompression == 2)
            {
                strm.Close();
                return;
            }

            if (_FinishMethodInfo == null)
                return;

            object returnVal = _FinishMethodInfo.Invoke(strm, null);

            return;
        }

        internal byte[] GetArray(MemoryStream ms)
        {
            byte[] cmpData = ms.ToArray();

            if (_UseCompression == 1)
                return cmpData;

            // we're using the MS routines;  need to prefix by 2 bytes; 
            //  see http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=97064 
            //  see http://www.faqs.org/rfcs/rfc1950.html 
            //   bit of magic: 
            // DeflateStream follows RFC1951, so the info I have is based on what I've read about RFC1950. 
            //First byte: 
            //Bits 0-3: those will always be 8 for the Deflate compression method. 
            //Bits 4-7: Our window size is 8K so I think this should be 5. The compression info value is 
            // log base 2 of the window size minus 8 => 8K = 2^13, so 13-8. If you're having problems with 
            // this, note that I've seen some comments indicating this is interpreted as max window size, 
            // so you might also try setting this to 7, corresponding to a window size of 32K. 
            //The next byte consists of some checksums and flags which may be optional? 

            byte[] wa = new byte[cmpData.Length + 2 + 4];   // length = data length + prefix length + checksum length 
            // These values don't actally work for all since some compressions go wrong??? 
            //   (may have been corrected: kas 9/16/08 but didn't do exhaustive tests since ziplib is better in any case) 
            wa[0] = 0x58;   // 78 
            wa[1] = 0x85;   // 9c 
            cmpData.CopyTo(wa, 2);

            uint c = adler(cmpData);       // this is the checksum 
            wa[2 + cmpData.Length + 0] = (byte)(c >> 24);
            wa[2 + cmpData.Length + 1] = (byte)(c >> 16);
            wa[2 + cmpData.Length + 2] = (byte)(c >> 8);
            wa[2 + cmpData.Length + 3] = (byte)(c);

            return wa;
        }

        /// <summary> 
        /// Adler 32 checksum routine comes from http://en.wikipedia.org/wiki/Adler-32 
        /// </summary> 
        /// <param name="cmpData"></param> 
        /// <returns></returns> 
        private uint adler(byte[] cmpData)
        {
            const int MOD_ADLER = 65521;
            int len = cmpData.Length;
            uint a = 1, b = 0;
            int i = 0;
            while (len > 0)
            {
                int tlen = len > 5552 ? 5552 : len;
                len -= tlen;
                do
                {
                    a += cmpData[i++];
                    b += a;
                } while (--tlen > 0);

                a %= MOD_ADLER;
                b %= MOD_ADLER;
            }

            return (b << 16) | a;
        }

        internal Stream GetStream(Stream str)
        {
            if (_UseCompression == 2)
            {   // use the built-in compression .NET 2 provides 
                //System.IO.Compression.GZipStream cs = 
                //    new System.IO.Compression.GZipStream(str, System.IO.Compression.CompressionMode.Compress); 
                System.IO.Compression.DeflateStream cs =
                    new System.IO.Compression.DeflateStream(str, System.IO.Compression.CompressionMode.Compress);
                return cs;
            }

            if (_UseCompression == 0)
                return null;
            if (_UseCompression == -1)   // make sure we're init'ed 
            {
                Init();
                if (_UseCompression != 1)
                    return null;
            }

            try
            {
                object[] args = new object[] { str };

                Stream so = _Assembly.CreateInstance(_ClassName, false,
                   BindingFlags.CreateInstance, null, args, null, null) as Stream;
                return so;
            }
            catch
            {
                return null;
            }
        }

        internal string ErrorMsg
        {
            get { return _ErrorMsg; }
        }

        void Init()
        {
            lock (this)
            {
                if (_UseCompression != -1)
                    return;
                _UseCompression = 0;      // assume failure; and use the builtin MS routines 

                try
                {
                    // Load the assembly 
                    _Assembly = XmlUtil.AssemblyLoadFrom(_CodeModule);

                    // Load up a test stream to make sure it will work 
                    object[] args = new object[] { new MemoryStream() };

                    Stream so = _Assembly.CreateInstance(_ClassName, false,
                       BindingFlags.CreateInstance, null, args, null, null) as Stream;

                    if (so != null)
                    {      // we've successfully inited 
                        so.Close();
                        _UseCompression = 1;
                    }
                    else
                        _Assembly = null;

                    if (_Finish != null)
                    {
                        Type theClassType = so.GetType();
                        this._FinishMethodInfo = theClassType.GetMethod(_Finish);
                    }
                }
                catch (Exception e)
                {
                    _ErrorMsg = e.InnerException == null ? e.Message : e.InnerException.Message;
                    _UseCompression = 0;      // failure; force use the builtin MS routines 
                }

            }
        }
    }

    internal class SqlConfigEntry
    {
        internal string Provider;
        internal Assembly CodeModule;
        internal string ClassName;
        internal string TableSelect;
        internal string ErrorMsg;
        internal bool ReplaceParameters;
        internal string FileName;
        internal SqlConfigEntry(string provider, string file, string cname, Assembly codemodule, string tselect, string msg)
        {
            Provider = provider;
            CodeModule = codemodule;
            ClassName = cname;
            TableSelect = tselect;
            ErrorMsg = msg;
            ReplaceParameters = false;
            FileName = file;
        }
    }

    internal class CustomReportItemEntry
    {
        internal string ItemName;
        internal Type Type;
        internal string ErrorMsg;
        internal CustomReportItemEntry(string itemName, Type type, string msg)
        {
            Type = type;
            ItemName = itemName;
            ErrorMsg = msg;
        }
    }

}
