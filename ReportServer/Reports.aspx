<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ReportServer.Reports" %>
<%@ Register TagPrefix="rdl" Namespace="fyiReporting.RdlAsp" Assembly="RdlAsp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <rdl:RdlListReports
		Runat="Server" ID="RdlListReports1" />

</asp:Content>
