<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ReportServer.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        Old Password:
        <asp:TextBox ID="TextBoxOldPassword" runat="server" TextMode="Password"></asp:TextBox>
        &nbsp;
        <asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
        <br />
        New Password:
        <asp:TextBox ID="TextBoxNewPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        Confirm New Password:
        <asp:TextBox ID="TextBoxConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonChangePassword" runat="server" 
            onclick="ButtonChangePassword_Click" Text="Change Password" />
    </asp:Panel>
</asp:Content>
