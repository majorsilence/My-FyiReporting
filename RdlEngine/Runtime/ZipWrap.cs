
using System;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Text;
using Majorsilence.Reporting.Rdl;

namespace Majorsilence.Reporting.Rdl
{
    /// <summary>
    /// ZipWrap loads the SharpZipLib dll and facilitates the wrapping of ZipOutputStream and ZipEntry 
    /// </summary>
    public class ZipWrap
    {
        // 	public static void Main(string[] args)
        // 	{
        // 		string[] filenames = Directory.GetFiles(args[0]);
        // 		byte[] buffer = new byte[4096];
        // 		
        // 		using ( ZipOutputStream s = new ZipOutputStream(File.Create(args[1])) ) {
        // 		
        // 			s.SetLevel(9); // 0 - store only to 9 - means best compression
        // 		
        //if (e.Name != sourceDirectory_) {
        //    ZipEntry entry = entryFactory_.MakeDirectoryEntry(e.Name);
        //    outputStream_.PutNextEntry(entry);
        // 			foreach (string file in filenames) {
        // 				ZipEntry entry = new ZipEntry(file);
        // 				s.PutNextEntry(entry);
        //
        // 				using (FileStream fs = File.OpenRead(file)) {
        //						StreamUtils.Copy(fs, s, buffer);
        // 				}
        // 			}
        // 		}
        // 	}

        static readonly string ZIPNAME = "ICSharpCode.SharpZipLib.dll";
        static string _ZipError = "Call ZipWrap.Init() before instantiating this class";
        static Assembly _Assembly = null;

        static public void Init()
        {
            if (_Assembly != null)
                return;
            lock (typeof(ZipWrap))
            {
                try
                {
                    _Assembly = XmlUtil.AssemblyLoadFrom(ZIPNAME);
                    _ZipError = string.Empty;
                }
                catch (Exception ex)
                {
                    _ZipError = ex.Message;      // record error for later
                }
            }
        }
        static public Assembly ZipAssembly
        {
            get { return _Assembly; }
        }
        static public string ZipError
        {
            get { return _ZipError; }
        }
        static public void PropertySettingByEnum(object classInstance,Type classType,string propertyName,string desiredValue)
        {
            PropertyInfo dstPropertyInfo = classType.GetProperty(propertyName);
            Type enumType = dstPropertyInfo.PropertyType;
            object value2change = Enum.Parse(enumType, desiredValue);
            dstPropertyInfo.SetValue(classInstance, value2change, null);
        }
    }

    public class ZipOutputStream
    {
        object _ZipOutputStream;
        MethodInfo _PutNextEntry;
        MethodInfo _Write;
        MethodInfo _Finish;
        MethodInfo _Close;

        public ZipOutputStream(Stream baseOutputStream)
        {
            if (ZipWrap.ZipAssembly == null)
                throw new ArgumentNullException(ZipWrap.ZipError);

            object[] args = new object[] { baseOutputStream };

            _ZipOutputStream = ZipWrap.ZipAssembly.CreateInstance("ICSharpCode.SharpZipLib.Zip.ZipOutputStream", false,
                BindingFlags.CreateInstance, null, args, null, null);
            
            Type theClassType= _ZipOutputStream.GetType();
            ZipWrap.PropertySettingByEnum(_ZipOutputStream, theClassType, "UseZip64","Off");
            this._PutNextEntry = theClassType.GetMethod("PutNextEntry");
            Type[] types = new Type[3];
            types[0] = typeof(byte[]);
            types[1] = typeof(int);
            types[2] = typeof(int);
            this._Write = theClassType.GetMethod("Write", types);
            types = new Type[0];
            this._Finish = theClassType.GetMethod("Finish", types);
            this._Close = theClassType.GetMethod("Close", types);
        }
        
        public Stream ZipStream
        {
            get { return _ZipOutputStream as Stream; }
        }
        public void PutNextEntry(ZipEntry ze)
        {
            object[] args = new object[] { ze.Value };
            _PutNextEntry.Invoke(_ZipOutputStream, args);
        }

        public void Write(string str)
        {
            byte[] ubuf = Encoding.Unicode.GetBytes(str);
            Encoding enc = Encoding.GetEncoding(65001); // utf-8
            byte[] abuf = Encoding.Convert(Encoding.Unicode, enc, ubuf);

            Write(abuf, 0, abuf.Length);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            object[] args = new object[] { buffer, offset, count };
            _Write.Invoke(_ZipOutputStream, args);
        }

        public void Finish()
        {
            _Finish.Invoke(_ZipOutputStream, null);
        }

        public void Close()
        {
            _Close.Invoke(_ZipOutputStream, null);
        }
    }

    public class ZipEntry
    {
        object _ZipEntry;

        public ZipEntry(string name)
        {
            if (ZipWrap.ZipAssembly == null)
                throw new ArgumentNullException(ZipWrap.ZipError);

            object[] args = new object[] { name };

            _ZipEntry = ZipWrap.ZipAssembly.CreateInstance("ICSharpCode.SharpZipLib.Zip.ZipEntry", false,
                BindingFlags.CreateInstance, null, args, null, null);
        }

        public object Value
        {
            get { return _ZipEntry; }
        }
    }
}
