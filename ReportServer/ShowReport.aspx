<%@ Page Language="C#" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp"
Assembly="RdlAsp"%>

<script Runat="Server">
private RdlReport _Report = new RdlReport();
//GJL 20080520 - Show report parameters without running it first (many line changes in this file)
bool error;
string Name;

void Page_Load(Object sender, EventArgs e)
{
    Name = Request.QueryString["rs:Name"];
    string FirstRun = Request.QueryString["rs:FirstRun"];
   
    if (FirstRun == "true")
    {
        _Report.NoShow = true;
    }
    else
    {
        _Report.NoShow = false;
    }
    
	string arg = Request.QueryString["rs:Format"];
	if (arg != null)
		_Report.RenderType = arg;
	else
		_Report.RenderType = "html";
    _Report.PassPhrase = "northwind";       // user should provide in some fashion (from web.config??)
	// ReportFile must be the last item set since it triggers the building of the report
	arg = Request.QueryString["rs:url"];
	if (arg != null)
		_Report.ReportFile = arg;
	
	switch (_Report.RenderType)
	{
		case "xml":
		
            if (_Report.Xml == null)
            {
                error = true; 
            }
            else
            {	
			Response.ContentType = "application/xml";
			Response.Write(_Report.Xml);
            }
			break;
		case "pdf":
            if (_Report.Object == null)
            {
                error = true;
            }
            else
            { 
			Response.ContentType = "application/pdf";
			Response.BinaryWrite(_Report.Object);
            }			
			break;
        case "csv":
          
            if (_Report.CSV == null)
            {
                error = true;
            }
            else
            {  
            Response.ContentType = "text/plain";
            Response.Write(_Report.CSV);
            }
            break;
        case "html":	// Rest of the page takes care of the general html
		default:
			break;
	}
}

string Meta
{
	get 
	{
		if (_Report.ReportFile == "statistics")
			return "<meta http-equiv=\"Refresh\" contents=\"10\"/>";
		else
			return "";
	}
}
</script>
<%
if (_Report.RenderType != "html" && error == false)
	return;
%>	
<html>
<head>
    <script language="javascript" src="datetimepicker.js"></script>
    <script language="javascript" src="limitinput.js"></script>
<%=_Report.JavaScript%>
<style type='text/css'>
<%=_Report.CSS%>
</style>
<title><%=_Report.ReportFile%></title>
<%=Meta%>
</head>
<body>
<h1 style="font-family: Tahoma"><%if (Name == null)
                                  { %>RUN REPORT<%}
                                                  else
                                                  {%> <%=Name%> <%} %></h1>
<table>
<tr><td><%=_Report.ParameterHtml%><hr></td></tr>
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
