'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Public Class FormMysqlQueryEditor

    Public originQuery As String = ""
    Public editedQuery As String = "" 'stores the query after editting is complete
    Public skipQuery As Boolean = False
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'For Each line In txtQuery.Text
        '    nQuery &= line
        'Next
        Dim sQuery() As String = Split(txtQuery.Text, vbLf) 'splits to remove any "vblf"
        Dim nQuery As String = Join(sQuery) 'rejoins the array to form the complete query
        editedQuery = nQuery
        Me.Close()
    End Sub

    Private Sub FormMysqlQueryEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtQuery.Text = originQuery
    End Sub


    Private Sub btnSkip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkip.Click
        skipQuery = True
        Me.Close()
    End Sub
End Class
