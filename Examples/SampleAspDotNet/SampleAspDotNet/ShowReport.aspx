<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReport.aspx.cs" Inherits="SampleAspDotNet.ShowReport" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp" Assembly="RdlAsp"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%=_Report.JavaScript%>
    <style type='text/css'>
        <%=_Report.CSS%>
    </style>
    <title><%=_Report.ReportFile%></title>
    <%=Meta%>
</head>
    <body>
        <h1 style="font-family: Tahoma"><%if (this.Name == null)
                                      { %>RUN REPORT<%}
                                                      else
                                                      {%> <%=this.Name%> <%} %></h1>
        <table>
        <tr><td><%=_Report.ParameterHtml%><hr /></td></tr>
        <tr><td>  
        <%if (!_Report.NoShow)
          { %>
        <%=_Report.Html%>
        <%} %>
        </td></tr>
        <%
            if (_Report.MaxErrorSeverity > 4 && !_Report.NoShow)
         {
        %>

        <tr><td><table width="100%" border="1" bordercolor="#000080"
	        bgcolor="#FFFFE0" 
	        style="border-style:solid; border-collapse:collapse;">
	        <thead bgcolor="#00FFFF" class="TableHeader">
	        <tr><th align="left">Errors</th></tr> </thead>
	        <%
		        foreach (string err in _Report.Errors)
		        {
	        %>
		        <tr>
		        <td><%=err%></td>
		        </tr>
	        <%
		        }
	        %>
        </table></td></tr>
        <%
         }
            else
            {
                if (_Report.NoShow)
                {%>
                    <tr><td><table width="100%" border="1" bordercolor="#000080"
                    bgcolor="#FFFFE0" 
	                style="border-style:solid; border-collapse:collapse;">
	 	            <tr><td>Please input any report parameters and then press the 'Run Report' button</td></tr>	 	
	                </table></td></tr>       
                 <%   
                }   
            }
        %>
        </table>
    </body>
</html>

