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
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace fyiReporting.RDL
{
	///<summary>
	/// StyleInfo (borders, fonts, background, padding, ...)
	///</summary>
	public class StyleInfo: ICloneable
	{
		// note: all sizes are expressed as points
		// _BorderColor
		/// <summary>
		/// Color of the left border
		/// </summary>
		public Color BColorLeft;		// (Color) Color of the left border
		/// <summary>
		/// Color of the right border
		/// </summary>
		public Color BColorRight;		// (Color) Color of the right border
		/// <summary>
		/// Color of the top border
		/// </summary>
		public Color BColorTop;		// (Color) Color of the top border
		/// <summary>
		/// Color of the bottom border
		/// </summary>
		public Color BColorBottom;	// (Color) Color of the bottom border
		// _BorderStyle
		/// <summary>
		/// Style of the left border
		/// </summary>
		public BorderStyleEnum BStyleLeft;	// (Enum BorderStyle) Style of the left border
		/// <summary>
		/// Style of the left border
		/// </summary>
		public BorderStyleEnum BStyleRight;	// (Enum BorderStyle) Style of the left border
		/// <summary>
		/// Style of the top border
		/// </summary>
		public BorderStyleEnum BStyleTop;		// (Enum BorderStyle) Style of the top border
		/// <summary>
		/// Style of the bottom border
		/// </summary>
		public BorderStyleEnum BStyleBottom;	// (Enum BorderStyle) Style of the bottom border
		// _BorderWdith
		/// <summary>
		/// Width of the left border. Max: 20 pt Min: 0.25 pt
		/// </summary>
		public float BWidthLeft;	//(Size) Width of the left border. Max: 20 pt Min: 0.25 pt
		/// <summary>
		/// Width of the right border. Max: 20 pt Min: 0.25 pt
		/// </summary>
		public float BWidthRight;	//(Size) Width of the right border. Max: 20 pt Min: 0.25 pt
		/// <summary>
		/// Width of the right border. Max: 20 pt Min: 0.25 pt
		/// </summary>
		public float BWidthTop;		//(Size) Width of the right border. Max: 20 pt Min: 0.25 pt
		/// <summary>
		/// Width of the bottom border. Max: 20 pt Min: 0.25 pt
		/// </summary>
		public float BWidthBottom;	//(Size) Width of the bottom border. Max: 20 pt Min: 0.25 pt

		/// <summary>
		/// Color of the background
		/// </summary>
		public Color BackgroundColor;			//(Color) Color of the background
        public string BackgroundColorText;		//(Textual Color) Color of the background
        /// <summary>
		/// The type of background gradient
		/// </summary>
		public BackgroundGradientTypeEnum BackgroundGradientType;	// The type of background gradient
		/// <summary>
		/// End color for the background gradient.
		/// </summary>
		
		/// <summary>
		/// The type of background pattern
		/// </summary>
		public patternTypeEnum PatternType;
		
		public Color BackgroundGradientEndColor;	//(Color) End color for the background gradient.
		/// <summary>
		/// A background image for the report item.
		/// </summary>
		public PageImage BackgroundImage;	// A background image for the report item.
		/// <summary>
		/// Font style Default: Normal
		/// </summary>
		public FontStyleEnum FontStyle;		// (Enum FontStyle) Font style Default: Normal
		/// <summary>
		/// Name of the font family Default: Arial
		/// </summary>
		private string _FontFamily;			//(string)Name of the font family Default: Arial -- allow comma separated value?
		/// <summary>
		/// Point size of the font
		/// </summary>
		public float FontSize;		//(Size) Point size of the font
		/// <summary>
		/// Thickness of the font
		/// </summary>
		public FontWeightEnum FontWeight;		//(Enum FontWeight) Thickness of the font
        /// <summary>
        /// Cell format in Excel07  Default: General
        /// </summary>
		public string _Format;			//WRP 28102008 Cell format string
		/// <summary>
		/// Special text formatting Default: none
		/// </summary>
		public TextDecorationEnum TextDecoration;	// (Enum TextDecoration) Special text formatting Default: none
		/// <summary>
		/// Horizontal alignment of the text Default: General
		/// </summary>
		public TextAlignEnum TextAlign;		// (Enum TextAlign) Horizontal alignment of the text Default: General
		/// <summary>
		/// Vertical alignment of the text Default: Top
		/// </summary>
		public VerticalAlignEnum VerticalAlign;	// (Enum VerticalAlign)	Vertical alignment of the text Default: Top
		/// <summary>
		/// The foreground color	Default: Black
		/// </summary>
		public Color Color;			// (Color) The foreground color	Default: Black
        public string ColorText;    // (Color-text)
        /// <summary>
		/// Padding between the left edge of the report item.
		/// </summary>
		public float PaddingLeft;	// (Size)Padding between the left edge of the report item.
		/// <summary>
		/// Padding between the right edge of the report item.
		/// </summary>
		public float PaddingRight;	// (Size) Padding between the right edge of the report item.
		/// <summary>
		/// Padding between the top edge of the report item.
		/// </summary>
		public float PaddingTop;		// (Size) Padding between the top edge of the report item.
		/// <summary>
		/// Padding between the bottom edge of the report item.
		/// </summary>
		public float PaddingBottom;	// (Size) Padding between the bottom edge of the report item.
		/// <summary>
		/// Height of a line of text.
		/// </summary>
		public float LineHeight;		// (Size) Height of a line of text
		/// <summary>
		/// Indicates whether text is written left-to-right (default)
		/// </summary>
		public DirectionEnum Direction;		// (Enum Direction) Indicates whether text is written left-to-right (default)
		/// <summary>
		/// Indicates the writing mode; e.g. left right top bottom or top bottom left right.
		/// </summary>
		public WritingModeEnum WritingMode;	// (Enum WritingMode) Indicates whether text is written
		/// <summary>
		/// The primary language of the text.
		/// </summary>
		public string Language;		// (Language) The primary language of the text.
		/// <summary>
		/// Unused.
		/// </summary>
		public UnicodeBiDirectionalEnum UnicodeBiDirectional;	// (Enum UnicodeBiDirection) 
		/// <summary>
		/// Calendar to use.
		/// </summary>
		public CalendarEnum Calendar;		// (Enum Calendar)
		/// <summary>
		/// The digit format to use.
		/// </summary>
		public string NumeralLanguage;	// (Language) The digit format to use as described by its
		/// <summary>
		/// The variant of the digit format to use.
		/// </summary>
		public int NumeralVariant;	//(Integer) The variant of the digit format to use.

		/// <summary>
		/// Constructor using all defaults for the style.
		/// </summary>
		public StyleInfo()
		{
			BColorLeft = BColorRight = BColorTop = BColorBottom = System.Drawing.Color.Black;	// (Color) Color of the bottom border
			BStyleLeft = BStyleRight = BStyleTop = BStyleBottom = BorderStyleEnum.None;
			// _BorderWdith
			BWidthLeft = BWidthRight = BWidthTop = BWidthBottom = 1;

			BackgroundColor = System.Drawing.Color.Empty;
            BackgroundColorText = string.Empty;
			BackgroundGradientType = BackgroundGradientTypeEnum.None;
			BackgroundGradientEndColor = System.Drawing.Color.Empty;
			BackgroundImage = null;

			FontStyle = FontStyleEnum.Normal;
			_FontFamily = "Arial";
            //WRP 291008 numFmtId should be 0 (Zero) for General format - will be interpreted as a string
            //It has default values in Excel07 as per ECMA-376 standard (SEction 3.8.30) for Office Open XML Excel07
            _Format = "General";  
			FontSize = 10;
			FontWeight = FontWeightEnum.Normal;

			PatternType = patternTypeEnum.None;
			TextDecoration = TextDecorationEnum.None;
			TextAlign = TextAlignEnum.General;
			VerticalAlign = VerticalAlignEnum.Top;
			Color = System.Drawing.Color.Black;
            ColorText = "Black";
            PaddingLeft = PaddingRight = PaddingTop = PaddingBottom = 0;
			LineHeight = 0;
			Direction = DirectionEnum.LTR;
			WritingMode = WritingModeEnum.lr_tb;
			Language = "en-US";
			UnicodeBiDirectional = UnicodeBiDirectionalEnum.Normal;
			Calendar = CalendarEnum.Gregorian;
			NumeralLanguage = Language;
			NumeralVariant=1;
		}
		/// <summary>
		/// Name of the font family Default: Arial
		/// </summary>
		public string FontFamily
		{
			get
			{
				int i = _FontFamily.IndexOf(",");
				return i > 0? _FontFamily.Substring(0, i): _FontFamily;
			}
			set { _FontFamily = value; }
		}
		/// <summary>
		/// Name of the font family Default: Arial.  Support list of families separated by ','.
		/// </summary>
		public string FontFamilyFull
		{
			get {return _FontFamily;}
		}
		/// <summary>
		/// Gets the FontFamily instance using the FontFamily string.  This supports lists of fonts.
		/// </summary>
		/// <returns></returns>
		public FontFamily GetFontFamily()
		{
			return GetFontFamily(_FontFamily);
		}

		/// <summary>
		/// Gets the FontFamily instance using the passed face name.  This supports lists of fonts.
		/// </summary>
		/// <returns></returns>
		static public FontFamily GetFontFamily(string fface)
		{
			string[] choices = fface.Split(',');
			FontFamily ff=null;
			foreach (string val in choices)
			{
				try 
				{
					string font=null;
					// TODO: should be better way than to hard code; could put in config file??
					switch (val.Trim().ToLower())
					{
						case "serif":
							font = "Times New Roman";
							break;
						case "sans-serif":
							font = "Arial";
							break;
						case "cursive":
							font = "Comic Sans MS";
							break;
						case "fantasy":
							font = "Impact";
							break;
						case "monospace":
                        case "courier":
							font = "Courier New";
							break;
						default:
							font = val;
							break;
					}
					ff = new FontFamily(font);
					if (ff != null)
						break;
				}
				catch {}	// if font doesn't exist we will go to the next
			}
			if (ff == null)
				ff = new FontFamily("Arial");
			return ff;
		}
		/// <summary>
		/// True if font is bold.
		/// </summary>
		/// <returns></returns>
		public bool IsFontBold()
		{
			switch(FontWeight)
			{
				case FontWeightEnum.Bold:
				case FontWeightEnum.Bolder:
				case FontWeightEnum.W500:
				case FontWeightEnum.W600:
				case FontWeightEnum.W700:
				case FontWeightEnum.W800:
				case FontWeightEnum.W900:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Gets the enumerated font weight.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		static public FontWeightEnum GetFontWeight(string v, FontWeightEnum def)
		{
			FontWeightEnum fw;

			switch(v.ToLower())
			{
				case "Lighter":
					fw = FontWeightEnum.Lighter;
					break;
				case "Normal":
					fw = FontWeightEnum.Normal;
					break;
				case "bold":
					fw = FontWeightEnum.Bold;
					break;
				case "bolder":
					fw = FontWeightEnum.Bolder;
					break;
				case "500":
					fw = FontWeightEnum.W500;
					break;
				case "600":
					fw = FontWeightEnum.W600;
					break;
				case "700":
					fw = FontWeightEnum.W700;
					break;
				case "800":
					fw = FontWeightEnum.W800;
					break;
				case "900":
					fw = FontWeightEnum.W900;
					break;
				default:
					fw = def;
					break;
			}
			return fw;
		}

		/// <summary>
		/// Returns the font style (normal or italic).
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static FontStyleEnum GetFontStyle(string v, FontStyleEnum def)
		{
			FontStyleEnum f;
			switch (v.ToLower())
			{
				case "normal":
					f = FontStyleEnum.Normal;
					break;
				case "italic":
					f = FontStyleEnum.Italic;
					break;
				default:
					f = def;
					break;
			}
			return f;
		}

		/// <summary>
		/// Gets the background gradient type.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		static public BackgroundGradientTypeEnum GetBackgroundGradientType(string v, BackgroundGradientTypeEnum def)
		{
			BackgroundGradientTypeEnum gt;
			switch(v.ToLower())
			{
				case "none":
					gt = BackgroundGradientTypeEnum.None;
					break;
				case "leftright":
					gt = BackgroundGradientTypeEnum.LeftRight;
					break;
				case "topbottom":
					gt = BackgroundGradientTypeEnum.TopBottom;
					break;
				case "center":
					gt = BackgroundGradientTypeEnum.Center;
					break;
				case "diagonalleft":
					gt = BackgroundGradientTypeEnum.DiagonalLeft;
					break;
				case "diagonalright":
					gt = BackgroundGradientTypeEnum.DiagonalRight;
					break;
				case "horizontalcenter":
					gt = BackgroundGradientTypeEnum.HorizontalCenter;
					break;
				case "verticalcenter":
					gt = BackgroundGradientTypeEnum.VerticalCenter;
					break;
				default:
					gt = def;
					break;
			}
			return gt;
		}

		/// <summary>
		/// Gets the text decoration.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static TextDecorationEnum GetTextDecoration(string v, TextDecorationEnum def)
		{
			TextDecorationEnum td;
			switch (v.ToLower())
			{
				case "underline":
					td = TextDecorationEnum.Underline;
					break;
				case "overline":
					td = TextDecorationEnum.Overline;
					break;
				case "linethrough":
					td = TextDecorationEnum.LineThrough;
					break;
				case "none":
					td = TextDecorationEnum.None;
					break;
				default:
					td = def;
					break;
			}
			return td;
		}

		/// <summary>
		/// Gets the text alignment.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static TextAlignEnum GetTextAlign(string v, TextAlignEnum def)
		{
			TextAlignEnum ta;
			switch(v.ToLower())
			{
				case "left":
					ta = TextAlignEnum.Left;
					break;
				case "right":
					ta = TextAlignEnum.Right;
					break;
				case "center":
					ta = TextAlignEnum.Center;
					break;
				case "general":
					ta = TextAlignEnum.General;
					break;
				default:
					ta = def;
					break;
			}
			return ta;
		}

		/// <summary>
		/// Gets the vertical alignment.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static VerticalAlignEnum GetVerticalAlign(string v, VerticalAlignEnum def)
		{
			VerticalAlignEnum va;
			switch (v.ToLower())
			{
				case "top":
					va = VerticalAlignEnum.Top;
					break;
				case "middle":
					va = VerticalAlignEnum.Middle;
					break;
				case "bottom":
					va = VerticalAlignEnum.Bottom;
					break;
				default:
					va = def;
					break;
			}
			return va;
		}

		/// <summary>
		/// Gets the direction of the text.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static DirectionEnum GetDirection(string v, DirectionEnum def)
		{
			DirectionEnum d;   
			switch(v.ToLower())
			{
				case "ltr":
					d = DirectionEnum.LTR;
					break;
				case "rtl":
					d = DirectionEnum.RTL;
					break;
				default:
					d = def;
					break;
			}
			return d;
		}
		/// <summary>
		/// Gets the writing mode; e.g. left right top bottom or top bottom left right.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static WritingModeEnum GetWritingMode(string v, WritingModeEnum def)
		{
			WritingModeEnum w;
			switch(v.ToLower())
			{
				case "lr-tb":
					w = WritingModeEnum.lr_tb;
					break;
				case "tb-rl":
					w = WritingModeEnum.tb_rl;
					break;
                default:
					w = def;
					break;
			}
			return w;
		}

		/// <summary>
		/// Gets the unicode BiDirectional.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static UnicodeBiDirectionalEnum GetUnicodeBiDirectional(string v, UnicodeBiDirectionalEnum def)
		{
			UnicodeBiDirectionalEnum u;
			switch (v.ToLower())
			{
				case "normal":
					u = UnicodeBiDirectionalEnum.Normal;
					break;
				case "embed":
					u = UnicodeBiDirectionalEnum.Embed;
					break;
				case "bidi-override":
					u = UnicodeBiDirectionalEnum.BiDi_Override;
					break;
				default:
					u = def;
					break;
			}
			return u;
		}

		/// <summary>
		/// Gets the calendar (e.g. Gregorian, GregorianArabic, and so on)
		/// </summary>
		/// <param name="v"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public static CalendarEnum GetCalendar(string v, CalendarEnum def)
		{
			CalendarEnum c;

			switch (v.ToLower())
			{
				case "gregorian":
					c = CalendarEnum.Gregorian;
					break;
				case "gregorianarabic":
					c = CalendarEnum.GregorianArabic;
					break;
				case "gregorianmiddleeastfrench":
					c = CalendarEnum.GregorianMiddleEastFrench;
					break;
				case "gregoriantransliteratedenglish":
					c = CalendarEnum.GregorianTransliteratedEnglish;
					break;
				case "gregoriantransliteratedfrench":
					c = CalendarEnum.GregorianTransliteratedFrench;
					break;
				case "gregorianusenglish":
					c = CalendarEnum.GregorianUSEnglish;
					break;
				case "hebrew":
					c = CalendarEnum.Hebrew;
					break;
				case "hijri":
					c = CalendarEnum.Hijri;
					break;
				case "japanese":
					c = CalendarEnum.Japanese;
					break;
				case "korea":
					c = CalendarEnum.Korea;
					break;
				case "taiwan":
					c = CalendarEnum.Taiwan;
					break;
				case "thaibuddhist":
					c = CalendarEnum.ThaiBuddhist;
					break;
				default:
					c = def;
					break;
			}
			return c;

		}
        // WRP 301008 return Excel07 format code as defined in section 3.8.30 of the ECMA-376 standard for Office Open XML Excel07 file formats
        public static int GetFormatCode (string val)
        {
            switch (val)
            {
                case "General":
                    return 0;
                case "0":
                    return 1;
                case "0.00":
                    return 2;
                case "#,##0":
                    return 3;
                case "#,##0.00":
                    return 4;
                case "0%":
                    return 9;
                case "0.00%":
                    return 10;
                case "0.00E+00":
                    return 11;
                case "# ?/?":
                    return 12;
                case " # ??/??":
                    return 13;
                case "mm-dd-yy":
                    return 14;
                case "d-mmm-yy":
                    return 15;
                case "d-mmm":
                    return 16;
                case "mmm-yy":
                    return 17;
                case "h:mm AM/PM":
                    return 18;
                case "h:mm:ss AM/PM":
                    return 19;
                case "h:mm":
                    return 20;
                case "h:mm:ss":
                    return 21;
                case "m/d/yy h:mm":
                    return 22;
                case "#,##0 ;(#,##0)":
                    return 37;
                case "#,##0 ;[Red](#,##0)":
                    return 38;
                case "#,##0.00;(#,##0.00)":
                    return 39;
                case "#,##0.00;[Red](#,##0.00)":
                    return 40;
                case "mm:ss":
                    return 45;
                case "[h]:mm:ss":
                    return 46;
                case "mmss.0":
                    return 47;
                case "##0.0E+0":
                    return 48;
                case "@":
                    return 49;
                default:
                    return 999;

            }
        }
		
		public static patternTypeEnum GetPatternType(System.Drawing.Drawing2D.HatchStyle hs)
		{
			switch (hs)
			{
				case HatchStyle.BackwardDiagonal:
					return patternTypeEnum.BackwardDiagonal;					
				case HatchStyle.Cross:
					return patternTypeEnum.Cross;				
				case HatchStyle.DarkDownwardDiagonal:
					return patternTypeEnum.DarkDownwardDiagonal;
				case HatchStyle.DarkHorizontal:
					return patternTypeEnum.DarkHorizontal;
				case HatchStyle.Vertical:
					return patternTypeEnum.Vertical;
				case HatchStyle.LargeConfetti:
					return patternTypeEnum.LargeConfetti;
				case HatchStyle.OutlinedDiamond:
					return patternTypeEnum.OutlinedDiamond;
				case HatchStyle.SmallConfetti:
					return patternTypeEnum.SmallConfetti;
				case HatchStyle.HorizontalBrick:
					return patternTypeEnum.HorizontalBrick;
				case HatchStyle.LargeCheckerBoard:
					return patternTypeEnum.CheckerBoard;
				case HatchStyle.SolidDiamond:
					return patternTypeEnum.SolidDiamond;
				case HatchStyle.DiagonalBrick:
					return patternTypeEnum.DiagonalBrick;
				default:
					return patternTypeEnum.None;					
			}
		}
		
		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
	
	/// <summary>
	/// The types of patterns supported.
	/// </summary>
	public enum patternTypeEnum
	{
		None,
		LargeConfetti,
		Cross,
		DarkDownwardDiagonal,
		OutlinedDiamond,
		DarkHorizontal,
		SmallConfetti,
		HorizontalBrick,
		CheckerBoard,
		Vertical,
		SolidDiamond,
		DiagonalBrick,
		BackwardDiagonal
	}

	/// <summary>
	/// The types of background gradients supported.
	/// </summary>
	public enum BackgroundGradientTypeEnum
	{
		/// <summary>
		/// No gradient
		/// </summary>
		None,
		/// <summary>
		/// Left Right gradient
		/// </summary>
		LeftRight,
		/// <summary>
		/// Top Bottom gradient
		/// </summary>
		TopBottom,
		/// <summary>
		/// Center gradient
		/// </summary>
		Center,
		/// <summary>
		/// Diagonal Left gradient
		/// </summary>
		DiagonalLeft,
		/// <summary>
		/// Diagonal Right gradient
		/// </summary>
		DiagonalRight,
		/// <summary>
		/// Horizontal Center gradient
		/// </summary>
		HorizontalCenter,
		/// <summary>
		/// Vertical Center
		/// </summary>
		VerticalCenter
	}
	/// <summary>
	/// Font styles supported
	/// </summary>
	public enum FontStyleEnum
	{
		/// <summary>
		/// Normal font
		/// </summary>
		Normal,
		/// <summary>
		/// Italic font
		/// </summary>
		Italic
	}

	/// <summary>
	/// Potential font weights
	/// </summary>
	public enum FontWeightEnum
	{
		/// <summary>
		/// Lighter font
		/// </summary>
		Lighter,
		/// <summary>
		/// Normal font
		/// </summary>
		Normal,
		/// <summary>
		/// Bold font
		/// </summary>
		Bold,
		/// <summary>
		/// Bolder font
		/// </summary>
		Bolder,
		/// <summary>
		/// W100 font
		/// </summary>
		W100,
		/// <summary>
		/// W200 font
		/// </summary>
		W200,
		/// <summary>
		/// W300 font
		/// </summary>
		W300,
		/// <summary>
		/// W400 font
		/// </summary>
		W400,
		/// <summary>
		/// W500 font
		/// </summary>
		W500,
		/// <summary>
		/// W600 font
		/// </summary>
		W600,
		/// <summary>
		/// W700 font
		/// </summary>
		W700,
		/// <summary>
		/// W800 font
		/// </summary>
		W800,
		/// <summary>
		/// W900 font
		/// </summary>
		W900
	}

	public enum TextDecorationEnum
	{
		Underline,
		Overline,
		LineThrough,
		None
	}

	public enum TextAlignEnum
	{
		Left,
		Center,
		Right,
		General
	}

	public enum VerticalAlignEnum
	{
		Top,
		Middle,
		Bottom
	}

	public enum DirectionEnum
	{
		LTR,				// left to right
		RTL					// right to left
	}

	public enum WritingModeEnum
	{
        lr_tb,				// left right - top bottom
		tb_rl				// top bottom - right left
	}

	public enum UnicodeBiDirectionalEnum
	{
		Normal,
		Embed,
		BiDi_Override
	}
		
	public enum CalendarEnum
	{
		Gregorian,
		GregorianArabic,
		GregorianMiddleEastFrench,
		GregorianTransliteratedEnglish,
		GregorianTransliteratedFrench,
		GregorianUSEnglish,
		Hebrew,
		Hijri,
		Japanese,
		Korea,
		Taiwan,
		ThaiBuddhist
	}
}
