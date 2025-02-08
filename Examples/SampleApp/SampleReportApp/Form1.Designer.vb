<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.RdlViewer1 = New fyiReporting.RdlViewer.RdlViewer()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ButtonSelectReport = New System.Windows.Forms.Button()
        Me.ButtonReloadReport = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'RdlViewer1
        '
        Me.RdlViewer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RdlViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RdlViewer1.Folder = Nothing
        Me.RdlViewer1.HighlightAll = False
        Me.RdlViewer1.HighlightAllColor = System.Drawing.Color.Fuchsia
        Me.RdlViewer1.HighlightCaseSensitive = False
        Me.RdlViewer1.HighlightItemColor = System.Drawing.Color.Aqua
        Me.RdlViewer1.HighlightPageItem = Nothing
        Me.RdlViewer1.HighlightText = Nothing
        Me.RdlViewer1.Location = New System.Drawing.Point(15, 86)
        Me.RdlViewer1.Name = "RdlViewer1"
        Me.RdlViewer1.PageCurrent = 1
        Me.RdlViewer1.Parameters = ""
        Me.RdlViewer1.ReportName = Nothing
        Me.RdlViewer1.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous
        Me.RdlViewer1.SelectTool = False
        Me.RdlViewer1.ShowFindPanel = False
        Me.RdlViewer1.ShowParameterPanel = True
        Me.RdlViewer1.ShowWaitDialog = True
        Me.RdlViewer1.Size = New System.Drawing.Size(642, 295)
        Me.RdlViewer1.TabIndex = 0
        Me.RdlViewer1.UseTrueMargins = True
        Me.RdlViewer1.Zoom = 0.7681401!
        Me.RdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Set Connection String"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(157, 10)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(470, 20)
        Me.TextBox1.TabIndex = 2
        '
        'ButtonSelectReport
        '
        Me.ButtonSelectReport.Location = New System.Drawing.Point(12, 47)
        Me.ButtonSelectReport.Name = "ButtonSelectReport"
        Me.ButtonSelectReport.Size = New System.Drawing.Size(141, 23)
        Me.ButtonSelectReport.TabIndex = 3
        Me.ButtonSelectReport.Text = "Select Report"
        Me.ButtonSelectReport.UseVisualStyleBackColor = True
        '
        'ButtonReloadReport
        '
        Me.ButtonReloadReport.Location = New System.Drawing.Point(176, 47)
        Me.ButtonReloadReport.Name = "ButtonReloadReport"
        Me.ButtonReloadReport.Size = New System.Drawing.Size(141, 23)
        Me.ButtonReloadReport.TabIndex = 4
        Me.ButtonReloadReport.Text = "Reload Report"
        Me.ButtonReloadReport.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(669, 393)
        Me.Controls.Add(Me.ButtonReloadReport)
        Me.Controls.Add(Me.ButtonSelectReport)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RdlViewer1)
        Me.Name = "Form1"
        Me.Text = "Sample Report App"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RdlViewer1 As fyiReporting.RdlViewer.RdlViewer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSelectReport As System.Windows.Forms.Button
    Friend WithEvents ButtonReloadReport As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog

End Class
