<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReport.aspx.cs" Inherits="ReportServer.ShowReport" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp" Assembly="RdlAsp"%>



<%
if (_Report.RenderType != "html" && error == false)
	return;
%>	
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=_Report.JavaScript%>
    <style type='text/css'>
        <%=_Report.CSS%>
    </style>
    <title><%=_Report.ReportFile%></title>
    <%=Meta%>
    <script language="javascript" src="datetimepicker.js" type="text/javascript"></script>
    <script language="javascript" src="limitinput.js"></script>
</head>
    <body>
        <h1 style="font-family: Tahoma"><%if (Name == null)
                                      { %>RUN REPORT<%}
                                                      else
                                                      {%> <%=Name%> <%} %></h1>
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
