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


namespace fyiReporting.RDL
{
	///<summary>
	///Data types
	///</summary>
	public class DataType
	{
        static public Type GetStyleType(string s)
        {
            TypeCode t = GetStyle(s, (ReportDefn)null);
            return XmlUtil.GetTypeFromTypeCode(t);
        }

		static internal TypeCode GetStyle(string s, ReportDefn r)
		{
			TypeCode rs;

			if (s.StartsWith("System."))
				s = s.Substring(7);

			switch (s)
			{		
				case "Boolean":
					rs = TypeCode.Boolean;
					break;
				case "DateTime":
					rs = TypeCode.DateTime;
					break;
				case "Decimal":
					rs = TypeCode.Decimal;
					break;
                case "Byte":
				case "Integer":
				case "Int16":
				case "Int32":
					rs = TypeCode.Int32;
					break;   
				case "Int64":
					rs = TypeCode.Int64;
					break;   
				case "Float":
				case "Single":
				case "Double":
					rs = TypeCode.Double;
					break;
				case "String":
				case "Char":
					rs = TypeCode.String;
					break;
				default:		// user error
					rs = TypeCode.Object;
                    if (r != null)
					    r.rl.LogError(4, string.Format("'{0}' is not a recognized type, assuming System.Object.", s));
					break;
			}
			return rs;
		}

		static internal bool IsNumeric(TypeCode tc)
		{
			switch (tc)
			{
		        case TypeCode.Byte:
				case TypeCode.Int64:
				case TypeCode.Int32:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:		// user error
					return false;
			}
		}
	}

}
