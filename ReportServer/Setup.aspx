<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="ReportServer.Setup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="PanelNotSetup" runat="server">
        Before using this site you must configure it.<br />
       
        <br />
        Admin Email:
        <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
        <br />
        First Name:
        <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
        <br />
        Last Name:
        <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
        <br />
        Password:
        <asp:TextBox ID="TextBoxPassword" runat="server"></asp:TextBox>
        <br />
        Confirm Password:
        <asp:TextBox ID="TextBoxConfirmPassword" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" 
            onclick="ButtonSubmit_Click" />
    </asp:Panel>
    <asp:Panel ID="PanelAlreadySetup" runat="server">
        This site has already had its initial configuration run.&nbsp; Please do further 
        configuration in the admin section.</asp:Panel>
</asp:Content>
