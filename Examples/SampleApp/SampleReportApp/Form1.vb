Public Class Form1

    Private file As String

    Private Sub ButtonSelectReport_Click(sender As Object, e As EventArgs) Handles ButtonSelectReport.Click

        If OpenFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then

            file = OpenFileDialog1.FileName

        End If

    End Sub

    Private Async Sub ButtonReloadReport_Click(sender As Object, e As EventArgs) Handles ButtonReloadReport.Click
        Await RdlViewer1.SetSourceFile(New Uri(file))
        RdlViewer1.Parameters = "ConnectionString=" & TextBox1.Text
        Await RdlViewer1.Rebuild()
    End Sub


    Private Sub RdlViewer1_PageNavigation(sender As Object, e As fyiReporting.RdlViewer.PageNavigationEventArgs) Handles RdlViewer1.PageNavigation
        MessageBox.Show(e.NewPage)
    End Sub
End Class
