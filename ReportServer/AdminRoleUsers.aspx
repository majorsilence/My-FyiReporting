<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminRoleUsers.aspx.cs" Inherits="ReportServer.AdminRoleUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server">

    <h1>User List&nbsp; </h1> 
        <asp:ListBox ID="ListBoxUserList" runat="server" Height="193px" Width="551px">
        </asp:ListBox>
        &nbsp;&nbsp;<asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
        <br />
        Roles:
        <asp:DropDownList ID="DropDownListRoles" runat="server" AutoPostBack="True" 
            Height="16px" Width="136px">
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="ButtonChangeUserRole" runat="server" 
            onclick="ButtonChangeUserRole_Click" 
            Text="Change Selected Users to Selected Role" />
        <br />
        <br />
    </asp:Panel>

</asp:Content>
