<%@ Page Language="C#" %>
<%@ Register TagPrefix="fyi" Namespace="fyiReporting.RdlAsp"
Assembly="RdlAsp"%>
<script Runat="Server">
private RdlReport _Report = new RdlReport();

void Page_Load(Object sender, EventArgs e)
{
	_Report.RenderType = "html";
	// ReportFile must be the last item set since it triggers the building of the report
	string report_url = Request.QueryString["rs:url"];
	if (report_url != null)
		_Report.ReportFile = report_url;
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
</Script>
<html>
<head>
<%=_Report.JavaScript%>
<style type='text/css'>
<%=_Report.CSS%>
</style>
<title>fyiReporting Server <%=_Report.ReportFile%></title>
<%=Meta%>
</head>

<body>
<table border="border">
<tr><td colspan="2" align="center">
<!-- Beginning of header  -->
This is the header
<!-- End of header    -->
</td></tr>
<tr>
<td valign="top">
<!-- Beginning of list directory  -->
 <fyi:RdlListReports
		  RunPage="Default.aspx"
		  Runat="Server" />
<!-- End of list directory    -->
</td> 
<td valign="top"> 
<!-- Beginning of including the report     -->
<table>
<tr><td><%=_Report.ParameterHtml%></td></tr>
<tr><td><%=_Report.Html%></td></tr>
</table>
<!-- End of including the report    -->
</td>
</tr>
<tr>
<td colspan="2" align="center">
<!-- Beginning of footer  -->
this is the footer
<!-- End of footer    -->
</td></tr>
</table>
</body>
</html>

