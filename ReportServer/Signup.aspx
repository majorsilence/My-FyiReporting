<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="ReportServer.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="Panel1" runat="server">
        New user sign up<br />
       
        <br />
        Email:
        <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
        <br />
        First Name:
        <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
        <br />
        Last Name:
        <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
        <br />
        Password:
        <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        Confirm Password:
        <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" 
            onclick="ButtonSubmit_Click" />
    </asp:Panel>
</asp:Content>
