
using System;
using System.Collections;
using System.IO;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
	
	
	/// <summary>
	///Represents the pattern dictionary used in a pdf page
	/// </summary>
	/// 	
	internal class PdfPattern
	{
		PdfAnchor pa;
		Hashtable patterns;
        internal PdfPattern(PdfAnchor a)
		{
			pa = a;
			patterns = new Hashtable();
		}

        internal Hashtable Patterns
		{
			get { return patterns; }
		}

		internal string GetPdfPattern(string patternname)
		{
            PdfPatternEntry pe = (PdfPatternEntry)patterns[patternname];
			if (pe != null)
				return pe.pattern;

			string name = "P" + (patterns.Count + 1).ToString();
            pe = new PdfPatternEntry(pa, name,patternname);
            patterns.Add(patternname, pe);
			return pe.pattern;
		}		
		
		
		/// <summary>
		/// Gets the pattern entries to be written to the file
		/// </summary>
		/// <returns></returns>
		internal byte[] GetPatternDict(long filePos,out int size)
		{
			MemoryStream ms=new MemoryStream();
			int s;
			byte[] ba;
            foreach (PdfPatternEntry pe in patterns.Values)
            {
                ba = pe.GetUTF8Bytes(pe.patternDict, filePos, out s);
               filePos += s;
                ms.Write(ba, 0, ba.Length);
            }
			
			ba = ms.ToArray();
			size = ba.Length;
			return ba;
		}
	}
	
	internal class PatternObj:PdfBase
	{
		internal string Patternobj;
		
		
		internal PatternObj(PdfAnchor pa):base(pa)
		{
			Patternobj = string.Format("\r\n{0} 0 obj\r\n [ /Pattern /DeviceRGB ]\r\n endobj",this.objectNum);
		}
		
		internal byte[] GetPatternObj(long filePos,out int size)
		{
			return this.GetUTF8Bytes(Patternobj,filePos,out size);
		}
	}
	
	/// <summary>
	///Represents a pattern entry used in a pdf page
	/// </summary>
	internal class PdfPatternEntry:PdfBase
	{
		internal string patternDict;
		internal string pattern;

		/// <summary>
		/// Create the font Dictionary
		/// </summary>
		internal PdfPatternEntry(PdfAnchor pa,String patternName,String patternType):base(pa)
		{
            pattern = patternName;    
            switch (patternType)
            {
            	case "BackwardDiagonal":
            		patternDict = BackwardDiagonal();
            		break;
            	case "DarkDownwardDiagonal":
            		patternDict = DarkDownwardDiagonal();
            		break;
            	case "DarkHorizontal":
            		patternDict = DarkHorizontal();
            		break;
            	case "Vertical":
            		patternDict = Vertical();
            		break;
            	case "OutlinedDiamond":
            		patternDict = OutlinedDiamond();
            		break;
            	case "LargeConfetti":
            		patternDict = LargeConfetti();
            		break;
            	case "SmallConfetti":
            		patternDict = SmallConfetti();
            		break;
            	case "HorizontalBrick":
            		patternDict = HorizontalBrick();
            		break;
            	case "DiagonalBrick":
            		patternDict = DiagonalBrick();
            		break;
            	case "SolidDiamond":
            		patternDict = SolidDiamond();
            		break;
            	case "Cross":
            		patternDict = Cross();
            		break;
            	case "CheckerBoard":
            		patternDict = CheckerBoard();
            		break;            			
            }
        }
		
		internal string BackwardDiagonal()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();          
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 0 0 m\t7 7 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 7 7]");
            sb.Append("\r\n /XStep 7");
            sb.Append("\r\n /YStep 7");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!              
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string LargeConfetti()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();


            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 2 96 1.3 1.3 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 5 95 1.3 1.3 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 7 97 1.3 1.3 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 1 93 1.3 1.3 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 7 94 1.3 1.3 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 5 92 1.3 1.3 re\tf\t\t");
            
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 89 6 100]");
            sb.Append("\r\n /XStep 6");
            sb.Append("\r\n /YStep 6");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string SmallConfetti()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            
            
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 5 95 1 1 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 8 90 1 1 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 12 93 1 1 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 4 86 1 1 re\tf\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 10 85 1 1 re\tf\t\t");
            
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 85 15 100]");
            sb.Append("\r\n /XStep 11");
            sb.Append("\r\n /YStep 15");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string OutlinedDiamond()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 0 0 m\t7 7 l\tS\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 0 7 m\t7 0 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 8 8]");
            sb.Append("\r\n /XStep 7");
            sb.Append("\r\n /YStep 7");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string SolidDiamond()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();          
            stream.Append("\r\n\t 0.4 w\t [] 0 d\t 0 0 5 5 re\tf\t");
            stream.Append("\r\n\t 0.4 w\t [] 0 d\t 5 5 5 5 re\tf\t"); 
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 11 11]");
            sb.Append("\r\n /XStep 10");
            sb.Append("\r\n /YStep 10");
            sb.Append("\r\n /Matrix [.707 .707 -.707 .707 0 0]");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string CheckerBoard()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();          
            stream.Append("\r\n\t 0.4 w\t [] 0 d\t 0 0 5 5 re\tf\t");
            stream.Append("\r\n\t 0.4 w\t [] 0 d\t 5 5 5 5 re\tf\t"); 
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 11 11]");
            sb.Append("\r\n /XStep 10");
            sb.Append("\r\n /YStep 10");         
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string Cross()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 2 0 m\t2 4 l\tS\t\t");
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 0 2 m\t4 2 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 5 5]");
            sb.Append("\r\n /XStep 4");
            sb.Append("\r\n /YStep 4");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());
            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		
		
		internal string DarkDownwardDiagonal()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 0 7 m\t7 0 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 8 8]");
            sb.Append("\r\n /XStep 4");
            sb.Append("\r\n /YStep 4");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!        
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string DarkHorizontal()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            stream.Append("\r\n\t .6 w\t[] 0 d\t 0 3 m\t7 3 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 8 7]");
            sb.Append("\r\n /XStep 7");
            sb.Append("\r\n /YStep 6");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!          
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();
		}
		
		internal string HorizontalBrick()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            for (int i = 0; i <= 25; i++)
            {
            	stream.AppendFormat("\r\n\t .5 w\t[] 0 d\t {0} 50 m\t{0} 53 l\tS\t\t",i*4,i*4);
            }
            for (int i = 0; i <= 25; i++)
            {
            	stream.AppendFormat("\r\n\t .5 w\t[] 0 d\t {0} 53 m\t{0} 56 l\tS\t\t",i*4 + 2,i*4 + 2);
            }
           	stream.Append("\r\n\t .5 w\t[] 0 d\t 0 50 m\t100 50 l\tS\t\t");
            stream.Append("\r\n\t .5 w\t[] 0 d\t 0 53 m\t100 53 l\tS\t\t");
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 49 100 56]");
            sb.Append("\r\n /XStep 100");
            sb.Append("\r\n /YStep 6");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!           
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();

		}
		
		internal string DiagonalBrick()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            for (int i = 0; i <= 25; i++)
            {
            	stream.AppendFormat("\r\n\t .5 w\t[] 0 d\t {0} 50 m\t{0} 53 l\tS\t\t",i*4,i*4);
            }
            for (int i = 0; i <= 25; i++)
            {
            	stream.AppendFormat("\r\n\t .5 w\t[] 0 d\t {0} 53 m\t{0} 56 l\tS\t\t",i*4 + 2,i*4 + 2);
            }
           	stream.Append("\r\n\t .5 w\t[] 0 d\t 0 50 m\t100 50 l\tS\t\t");
            stream.Append("\r\n\t .5 w\t[] 0 d\t 0 53 m\t100 53 l\tS\t\t");
            
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 49 100 56]");
            sb.Append("\r\n /XStep 100");
            sb.Append("\r\n /YStep 6");
            sb.Append("\r\n /Matrix [.707 .707 -.707 .707 0 0]");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!           
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();

		}
		
		internal string Vertical()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            System.Text.StringBuilder stream = new System.Text.StringBuilder();
            stream.Append("\r\n\t 0.4 w\t[] 0 d\t 3 0 m\t3 7 l\tS\t\t");
            sb.AppendFormat("\r\n{0} 0 obj<</Type/Pattern",this.objectNum);
            sb.Append("\r\n /PatternType 1");
            sb.Append("\r\n /PaintType 2");
            sb.Append("\r\n /TilingType 3");
            sb.Append("\r\n /BBox [0 0 7 8]");
            sb.Append("\r\n /XStep 6");
            sb.Append("\r\n /YStep 7");
            sb.AppendFormat("\r\n /Resources {0} 0 R",this.objectNum);  //hmm we don't got no resources!!!       
            sb.AppendFormat("\r\n /Length {0}",stream.Length);
            sb.Append("\r\n >>");
            sb.Append("\r\n stream");
            sb.Append(stream.ToString());            
            sb.Append("\r\n endstream");
            sb.Append("\r\n endobj");
            return sb.ToString();

		}
		/// <summary>
		/// Get the font entry to be written to the file
		/// </summary>
		/// <returns></returns>
		internal byte[] GetPatternDict(long filePos,out int size)
		{
			return this.GetUTF8Bytes(patternDict,filePos,out size);
		}

	}
}
