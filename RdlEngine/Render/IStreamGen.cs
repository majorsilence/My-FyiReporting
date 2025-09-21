

using System;
using Majorsilence.Reporting.Rdl;
using System.IO;
using System.Collections;

namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Interface for obtaining streams for generation of reports
	/// </summary>

	public interface IStreamGen
	{
		Stream GetStream();								// get the main writer if not using TextWriter
		TextWriter GetTextWriter();						// gets the main text writer
		Stream GetIOStream(out string relativeName, string extension);	// get an IO stream, providing relative name as well- to main stream
		void CloseMainStream();							// closes the main stream
	}
}
