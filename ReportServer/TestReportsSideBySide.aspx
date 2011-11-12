<%@ Page Language="C#" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp"
Assembly="RdlAsp"%>

<html>
<%=_Report.JavaScript%>
<style type='text/css'>
<%=_Report.CSS%>
<%=_Report2.CSS%>
</style>
<head><title>Side by Side reports</title>

</head>
<body>
<h1>Side by Side Reports</h1>

<table>
<tr><td><h2>Drilldown Report</h2></td><td><h2>RSS Feeds</h2></td></tr>
<tr><td>
<rdl:RdlReport
  ID="_Report"
  ReportFile="DrilldownTest.rdl"
  Runat="Server" />
</td>
<td>
<rdl:RdlReport
  ID="_Report2"
  ReportFile="RssShort.rdl"
  Runat="Server" />
</td>
</tr></table>
</body>
</html>
