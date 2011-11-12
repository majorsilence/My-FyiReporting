<%@ Page Language="C#" %>
<%@ OutputCache Duration="1" VaryByParam="*" NoStore="true" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp"
Assembly="RdlAsp"%>   
           
<script Runat="Server">      
    private RdlSilverViewer _Viewer = new RdlSilverViewer();
    private bool _DoHtml;  
   
void Page_Load(Object sender, EventArgs e)
{
    _DoHtml = false;                        // assume we don't do any HTML
      
    _Viewer.PassPhrase = "northwind";       // user should provide in some fashion (from web.config??)
    _Viewer.ReportDirectory = "Reports";
    _Viewer.ImageDirectory = "tempimages";
    _Viewer.Url = Request.RawUrl; 
    // Grab the arguments
    
    // Report file 
	string report = Request.QueryString["rs:url"];
	if (report != null && report != _Viewer.ReportFile)
		_Viewer.ReportFile = report;

    string dozip = Request.QueryString["rs:zip"];

    if (dozip != null && dozip.ToLower() == "yes")
    {
        Response.ContentType = "application/zip";
        byte[] ba = _Viewer.GetZip();
        if (ba == null)
        {
            Context.Response.StatusCode = 404;		// must have been thrown out of request
        }
        else
        {
            Response.BinaryWrite(ba as byte[]);
        }
    }
    else if (report != null)
    {
        // get the page number requested if any
        string spageno = Request.QueryString["rs:pageno"];
        int pageno;
        try  
        {
            pageno = spageno==null? 1: Convert.ToInt32(spageno);
        }
        catch     
        {  
            pageno = 1;
        }  
        string xaml = _Viewer.GetXaml(pageno);
        if (xaml == null || xaml.Length == 0)
            xaml = "<Canvas xmlns=\"http://schemas.microsoft.com/client/2007\"" +
                        " xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"" +
                        " Height=\"440\">" +
                        " <TextBlock FontFamily=\"Arial\"" +
                         " FontSize=\"20\">Failure!</TextBlock>" +
                    "</Canvas>"; 
        Response.ContentType = "application/xaml";
        Response.Write(xaml);
    }
    else
        _DoHtml = true;
}
   
</script>  
<%
if (_DoHtml) 
 {  
%>           
   
<html>
<head>
    <script type="text/javascript" src="\Silverlight.js/Silverlight.js"></script>
    <script type="text/javascript" src="ReportViewer.js"></script>
<script type="text/javascript">
window.onload=function() 
{
    // var args = GetArgs();       // get all arguments
}     
 </script>

<style type='text/css'>    
</style>
<title>fyiReporting: Silverlight Report viewing</title>
</head>     
   
<body>       
<h3>A Silverlight RDL Report Viewer Demonstration</h3>
<p>This simple page shows how to include a Silverlight Report Viewers on a page.</p>
<table>          
<tr>           
<td valign="top">  
<!-- Beginning of list directory  -->   
 <rdl:RdlListReports ID="RdlListReports1"
		  SilverlightViewer=true
		  Runat="Server" />
<!-- End of list directory    -->
</td>
<td valign="top">
    <div id="rsSilverlightPluginHost" style="border-style:solid;border-width:thin;border-color:Silver;">  
    </div> 
    <script type="text/javascript"> 
         
         
        // Retrieve the div element you created in the previous step.
        var parentElement = 
            document.getElementById("rsSilverlightPluginHost");
        
        // This function creates the Silverlight plug-in.
        createRdlSilverlightPlugin("RSSilverlightPlugin");
        
    </script>
</td>
</tr>
</table>
<h3>Instructions for Use</h3>
<div style="font-size:smaller;">
<ul>
<li>Item 1</li>
<li>Item 2</li> 
<li>Item 3</li>
</ul>
</div>
<!-- End of including the report    -->
</body>
</html>
<%
}
%>        

