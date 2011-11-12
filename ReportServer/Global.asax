<script language="C#" runat=server>

void Session_Start(Object sender , EventArgs e) 
{
	fyiReporting.RdlAsp.RdlSession rs = 
		Application[fyiReporting.RdlAsp.RdlSession.SessionStat] as fyiReporting.RdlAsp.RdlSession;
	if ( rs == null ) 
	{
		Application[ "SessionStat" ] = rs = new fyiReporting.RdlAsp.RdlSession();
	}
	lock(rs) 
	{
		rs.Count++;
	}
}

void Session_End() 
{
	fyiReporting.RdlAsp.RdlSession rs = 
		Application[fyiReporting.RdlAsp.RdlSession.SessionStat] as fyiReporting.RdlAsp.RdlSession;
	if ( rs != null ) 
	{
		lock(rs)
		{
			rs.Count--;
		}
	}
}

</Script>
