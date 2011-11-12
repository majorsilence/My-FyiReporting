<%@ Page Language="C#" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp"
Assembly="RdlAsp"%>

<script Runat="Server">

void Page_Load(Object sender, EventArgs e)
{
	string type = Request.QueryString["type"]; 
	string unique = Request.QueryString["unique"];
	string key = string.Format("ShowFile.aspx?type={0}&unique={1}", type, unique); 
		
	object buffer = Context.Session[key] ;
	if (buffer == null)
	{
		Context.Response.StatusCode = 404;		// must have been thrown out of request
		return;
	}
	Context.Session.Remove(key);
	
	Response.ContentType = GetMimeType(type);
	Response.BinaryWrite(buffer as byte[]);
		
}

private string GetMimeType(string type)
{
	switch (type.ToLower())
	{
		case "bmp":
			return "image/bmp";
		case "jpeg":
		case "jpe":
		case "jpg":
		case "jfif":
			return "image/jpeg";
		case "gif":
			return "image/gif";
		case "png":
			return "image/png";
		case "tif":
		case "tiff":
			return "image/tiff";
		case "pdf":
			return "application/pdf";
		case "xml":
		case "rdl":
			return "application/xml";
		default:
			return "";
	}
}

</script>
