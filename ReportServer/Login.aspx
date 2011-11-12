<%@ Page Language="C#" %>
<script language="C#" runat=server>

void bSignOn_Click(Object sender , EventArgs e) 
{
	if (IsValid)
	{
		if (FormsAuthentication.Authenticate(tbUser.Text, tbPswd.Text))
		{
			FormsAuthentication.RedirectFromLoginPage( tbUser.Text, false );
		}
		else
		{
			lResult.Text = "Unknown user name and/or pass phrase!";
		}
	}
}

</Script>
<html>
<head><title>Login</title></head>

<body>
<form Runat="Server">
<h2>Login</h2>
<asp:Label 
	ID="lResult"
	Runat="Server"
/>
<p>
User Name: <asp:Textbox ID="tbUser" Runat="Server"/>
<asp:RequiredFieldValidator ControlToValidate="tbUser" Text="Required!" Runat="Server"/>
<p>
Pass phrase: <asp:Textbox ID="tbPswd" Runat="Server"/>
<asp:RequiredFieldValidator ControlToValidate="tbPswd" Text="Required!" Runat="Server"/>
<p>
<asp:Button Text="Sign On" OnClick="bSignOn_Click" Runat="Server"/>
</form>
</body>
</html>
