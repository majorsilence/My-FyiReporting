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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace fyiReporting.RDL
{
	///<summary>
	/// Utitlity class for reading and writing a DataSourceReference file
	///</summary>
	public sealed class DataSourceReference
	{
		const int IV_SIZE=16;
		const int KEY_SIZE=32;
		/// <summary>
		/// Create the named file containing the encrypted input data
		/// </summary>
		/// <param name="filename">Output file name</param>
		/// <param name="indata">Input data to place in file encrypted.</param>
		/// <param name="pswd">Password phrase to use when encrypting</param>
		static public void Create(string filename, string indata, string pswd) 
		{
			// create the salt
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] salt = new byte[IV_SIZE];
			rng.GetBytes(salt);

			// create the key from the password phrase
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(pswd, salt);
			byte[] key = pdb.GetBytes(KEY_SIZE);

			// Create an instance of the RijndaelManaged class
			RijndaelManaged rm = new RijndaelManaged();
			CryptoStream cs = null;
			MemoryStream ms = null;
			try
			{
				ms = new MemoryStream(1000);	// memory stream to put encrypted data to
				cs = new CryptoStream(ms, rm.CreateEncryptor(key, salt), CryptoStreamMode.Write);
				byte[] ta = Encoding.UTF8.GetBytes(indata);
				cs.Write(ta, 0, ta.Length);
			}
			finally
			{
				if (cs != null)
					cs.Close();
				if (ms != null)
					ms.Close();
			}

			FileStream fs = null;
			try
			{
				fs = File.OpenWrite(filename);
				// Write the salt to the beginning of the file
				fs.Write(salt, 0, salt.Length);
				// Write the encrypted data
				byte[] ba = ms.ToArray();
				fs.Write(ba, 0, ba.Length);
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			return;
		}
	
		/// <summary>
		/// Retrieve the string data from an encrypted file.   Retrieve assumes 
		/// that Create was used to create the file.
		/// </summary>
		/// <param name="filename">File name to retrieve encrypted data from.</param>
		/// <param name="pswd">Password phrase used when data was encrypted.</param>
		/// <returns>Unencrypted string contents of file</returns>
		static public string Retrieve(string filename, string pswd)
		{
			FileStream fs = null;
			byte[] salt = new byte[IV_SIZE];
			byte[] enc = null;
			try
			{
				fs = File.OpenRead(filename);
				fs.Read(salt, 0, salt.Length);
				enc = new byte[fs.Length - salt.Length];
				fs.Read(enc, 0, enc.Length);
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			// create the key from the password phrase
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(pswd, salt);
			byte[] key = pdb.GetBytes(KEY_SIZE);

			// Create an instance of the RijndaelManaged class
			RijndaelManaged rm = new RijndaelManaged();
			CryptoStream cs = null;
			MemoryStream ms = null;
			MemoryStream ms2 = null;
			string outdata=null;
			try
			{
				ms = new MemoryStream(enc);		// memory stream to read encrypted data from
				ms2 = new MemoryStream();		// decrypted memory stream
				cs = new CryptoStream(ms, rm.CreateDecryptor(key, salt), CryptoStreamMode.Read);
				byte[] ta = new byte[256];
				int count=0;
				while (true)
				{
					count = cs.Read(ta, 0, ta.Length);
					if (count <= 0)
						break;
					ms2.Write(ta, 0, count);
				}
				ta = ms2.ToArray();
				outdata = Encoding.UTF8.GetString(ta);
			}
			finally
			{
				if (cs != null)
					cs.Close();
				if (ms != null)
					ms.Close();
				if (ms2 != null)
					ms2.Close();
			}
			
			return outdata;
		}
	}
}
