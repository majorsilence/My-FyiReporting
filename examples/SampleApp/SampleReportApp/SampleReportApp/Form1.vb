Public Class Form1

    Private file As String

    Private Sub ButtonSelectReport_Click(sender As Object, e As EventArgs) Handles ButtonSelectReport.Click

        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            file = OpenFileDialog1.FileName

        End If

    End Sub

    Private Sub ButtonReloadReport_Click(sender As Object, e As EventArgs) Handles ButtonReloadReport.Click
        RdlViewer1.SourceFile = New Uri(file)
        RdlViewer1.Parameters = "ConnectionString=" & TextBox1.Text
        RdlViewer1.Rebuild()
    End Sub


End Class
