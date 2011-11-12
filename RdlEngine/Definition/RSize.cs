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
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;


namespace fyiReporting.RDL
{
	///<summary>
	/// The Size definition.  Held in a normalized format but convertible to multiple measurements.
	///</summary>
	[Serializable]
	public class RSize
	{
		int _Size;					// Normalized size in 1/100,000 meters
		string _Original;			// save original string for recreation of syntax

		internal RSize(ReportDefn r, string t)
		{
			// Size is specified in CSS Length Units
			// format is <decimal number nnn.nnn><optional space><unit>
			// in -> inches (1 inch = 2.54 cm)
			// cm -> centimeters (.01 meters)
			// mm -> millimeters (.001 meters)
			// pt -> points (1 point = 1/72.27 inches)
			// pc -> Picas (1 pica = 12 points)
			_Original = t;					// Save original string for recreation
			t = t.Trim();
			int space = t.LastIndexOf(' '); 
			string n;						// number string
			string u;						// unit string
			decimal d;						// initial number
			try		// Convert.ToDecimal can be very picky
			{
				if (space != -1)	// any spaces
				{
					n = t.Substring(0,space).Trim();	// number string
					u = t.Substring(space).Trim();	// unit string
				}
				else if (t.Length >= 3)
				{
					n = t.Substring(0, t.Length-2).Trim();
					u = t.Substring(t.Length-2).Trim();
				}
				else
				{
					// Illegal unit
                    if (r != null)
					    r.rl.LogError(4, string.Format("Illegal size '{0}' specified, assuming 0 length.", t));
					_Size = 0;
					return;
				}
				if (!Regex.IsMatch(n, @"\A[ ]*[-]?[0-9]*[.]?[0-9]*[ ]*\Z"))
				{
					r.rl.LogError(4, string.Format("Unknown characters in '{0}' specified.  Number must be of form '###.##'.  Local conversion will be attempted.", t));
					d = Convert.ToDecimal(n, NumberFormatInfo.CurrentInfo);		// initial number
				}
				else
					d = Convert.ToDecimal(n, NumberFormatInfo.InvariantInfo);		// initial number
			}
			catch (Exception ex) 
			{
				// Illegal unit
                if (r != null)
				    r.rl.LogError(4, "Illegal size '" + t + "' specified, assuming 0 length.\r\n"+ex.Message);
				_Size = 0;
				return;
			}

			switch(u)			// convert to millimeters
			{
				case "in":
					_Size = (int) (d * 2540m);
					break;
				case "cm":
					_Size = (int) (d * 1000m);
					break;
				case "mm":
					_Size = (int) (d * 100m);
					break;
				case "pt":
					_Size = (int) (d * (2540m / POINTSIZEM));
					break;
				case "pc":
					_Size = (int) (d * (2540m / POINTSIZEM * 12m));
					break;
				default:	 
					// Illegal unit
                    if (r != null)
					    r.rl.LogError(4, "Unknown sizing unit '" + u + "' specified, assuming inches.");
					_Size = (int) (d * 2540m);
					break;
			}
			if (_Size > 160 * 2540)	// Size can't be greater than 160 inches according to spec
			{   // but RdlEngine supports higher values so just do a warning
                if (r != null)
				    r.rl.LogError(4, "Size '" + this._Original + "' is larger than the RDL specification maximum of 160 inches.");
//				_Size = 160 * 2540;     // this would force maximum to spec max of 160
			}
		}

        static public float PointSize(string v)
        {
            RSize rs = new RSize(null, v);
            return rs.Points;
        }

		internal RSize(ReportDefn r, XmlNode xNode):this(r, xNode.InnerText)
		{
		}

		internal int Size
		{
			get { return  _Size; }
			set {  _Size = value; }
		}

		// Return value as if specified as px
		internal int PixelsX
		{
			get
			{	// For now assume 96 dpi;  TODO: what would be better way; shouldn't use server display pixels
				decimal p = _Size;
				p = p / 2540m;		// get it in inches
				p = p * 96;				// 
                if (p != 0)
                    return (int) p;
                return (_Size > 0) ? 1 : (int) p;
			}
		}

		static internal readonly float POINTSIZED = 72.27f;
		static internal readonly decimal POINTSIZEM = 72.27m;

		static internal int PixelsFromPoints(float x)
		{
			int result = (int) (x * 96 / POINTSIZED);	// convert to pixels
            if (result == 0 && x > .0001f)
                return 1;
			return result;
		}

		static internal int PixelsFromPoints(Graphics g, float x)
		{
			int result = (int) (x * g.DpiX / POINTSIZED);	// convert to pixels
            if (result == 0 && x > .0001f)
                return 1;

			return result;
		}
		
		internal int PixelsY
		{
			get
			{	// For now assume 96 dpi
				decimal p = _Size;
				p = p / 2540m;		// get it in inches
				p = p * 96;				// 
                if (p != 0)
                    return (int)p;
                return (_Size > 0) ? 1 : (int)p;
			}
		}

		internal float Points
		{
			get
			{	
				return (float) ((double) _Size / 2540.0 * POINTSIZED);
			}
		}
        /// <summary>
        /// TWIPS is 1/20 th of the size in points
        /// </summary>
        internal int Twips
        {
            get
            {
                return (int) TwipsFromPoints(Points);
            }
        }

        static internal int TwipsFromPoints(float pt)
        {
            return (int)Math.Round(pt * 20, 0);
        }

		static internal float PointsFromPixels(Graphics g, int x)
		{
			float result = (float) ((x * POINTSIZED) / g.DpiX);	// convert to points from pixels

			return result;
		}

		static internal float PointsFromPixels(Graphics g, float x)
		{
			float result = (float) ((x * POINTSIZED) / g.DpiX);	// convert to points from pixels

			return result;
		}

		internal string Original
		{
			get { return  _Original; }
			set {  _Original = value; }
		}
        /// <summary>
        /// The original size is "almost" compatible with CSS - except CSS doesn't allow blanks.
        /// </summary>
        internal string CSS
        {
            get 
            {
                if (_Original.IndexOf(' ') >= 0)
                    return _Original.Replace(" ", "");

                return _Original; 
            }
        }
	}
}
