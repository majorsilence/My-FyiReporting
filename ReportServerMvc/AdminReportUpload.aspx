﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminReportUpload.aspx.cs" Inherits="ReportServer.AdminReportUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/fileuploader.css" rel="stylesheet" type="text/css">	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server">
    <h1>Upload Reports</h1>
    Uploaded reports will automatically have tags added to permissions table. You can assign permissions to 
    roles in the <a href="AdminRoleManagement.aspx">Role Management</a> page.
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <p>To upload a file, click on the button below. Drag-and-drop is supported in FF, Chrome.</p>
	    <p>Progress-bar is supported in FF3.6+, Chrome6+, Safari4+</p>

        <div id="file-uploader2">		
		<noscript>			
			<p>Please enable JavaScript to use file uploader.</p>
			<!-- or put a simple form for upload here -->
		</noscript>         
	</div>

    <div class="qq-upload-extra-drop-area">Drop files here too</div>
    
    <script src="Javascript/fileuploader.js" type="text/javascript"></script>
    <script type="text/javascript">
        function createUploader() {
            var uploader = new qq.FileUploader({
                element: document.getElementById('file-uploader2'),
                action: 'AdminReportUploadHandler.ashx',
                debug: true,
                allowedExtensions: ['rdl'],
                extraDropzones: [qq.getByClass(document, 'qq-upload-extra-drop-area')[0]]
            });
        }

        // in your app create uploader as soon as the DOM is ready
        // don't wait for the window to load  
        window.onload = createUploader;     
    </script>

    </asp:Panel>
</asp:Content>
