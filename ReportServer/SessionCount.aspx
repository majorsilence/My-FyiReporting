<%@ Page Language="C#" %>
<script language="C#" runat=server>

void Page_Load(Object sender , EventArgs e) 
{
	fyiReporting.RdlAsp.RdlSession rs = Application[ "SessionCount" ] as fyiReporting.RdlAsp.RdlSession;
	lblSessionCount.Text = rs.Count.ToString();
}

</Script>

<html>
<head><title>SessionCount.aspx</title></head>
<body>

Current Sessions:
<asp:Label
  ID="lblSessionCount"
  Runat="Server" />

</body>
</html>
