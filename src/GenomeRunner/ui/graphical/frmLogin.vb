''Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Public Class frmLogin

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If txtServer.Text <> vbNullString Then
            'SaveSetting("GenomeRunner", "Database", "uName", txtUsername.Text)
            'SaveSetting("GenomeRunner", "Database", "uPassword", txtPassword.Text)
            SaveSetting("GenomeRunner", "Database", "uServer", txtServer.Text)
            'SaveSetting("GenomeRunner", "Database", "uDatabase", txtDatabase.Text)
        End If
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'txtUsername.Text = GetSetting("GenomeRunner", "Database", "uName")
        'txtPassword.Text = GetSetting("GenomeRunner", "Database", "uPassword")
        txtServer.Text = GetSetting("GenomeRunner", "Database", "uServer")
        'txtDatabase.Text = GetSetting("GenomeRunner", "Database", "uDatabase")
    End Sub

    'Private Sub btnDefaultDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'txtUsername.Text = "genomerunner"
    'txtPassword.Text = "genomerunner"
    'txtServer.Text = "156.110.144.34"
    '    txtServer.Text = "E:\For Mikhail\Genome Runner\Development\Old Files\mm9.sqlite"
    'txtDatabase.Text = "hg19test"
    'End Sub

    Private Sub Browse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse.Click
        Dim dialog As New OpenFileDialog()
        If DialogResult.OK = dialog.ShowDialog Then
            txtServer.Text = dialog.FileName
        End If
    End Sub
End Class
