<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminRoleManagement.aspx.cs" Inherits="ReportServer.AdminRoleManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <h1>Create New Role</h1>
        <asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
        <br /> New Role Name:&nbsp;
        <asp:TextBox ID="TextBoxRoleName" runat="server"></asp:TextBox>
        <br />
        Role Description:<asp:TextBox ID="TextBoxRoleDescription" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonCreateRole" runat="server" 
            onclick="ButtonCreateRole_Click" Text="Create Role" />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server">
    <h1>Role Permissions</h1> Select Role:
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
            Height="20px" onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
            Width="216px">
        </asp:DropDownList>
        <br />
        Select/Unselect Role Permissions:<br />
        <asp:ListBox ID="ListBoxRoleTags" runat="server" Height="481px" Width="386px">
        </asp:ListBox>
        <br />
        <asp:Button ID="ButtonSaveRoleTags" runat="server" 
            onclick="ButtonSaveRoleTags_Click" Text="Save Role Tags" />
        <br />

    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
    <h1>Delete Role</h1>
       <br />
        <asp:ListBox ID="ListBoxRoleList" runat="server" Height="224px" 
            SelectionMode="Multiple" Width="281px"></asp:ListBox>
        <br />
        <asp:Button ID="ButtonDeleteSelectedRole" runat="server" 
            onclick="ButtonDeleteSelectedRole_Click" Text="Delete Selected Roles" />
    </asp:Panel>
</asp:Content>
