'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports GenomeRunner.GenomeRunner

Public Class frmCustomizeFilters

    Public GenomicFeaturesWithCustomThresholds As List(Of GenomicFeature)
    Public PromoterUpstream As UInteger = 0
    Public PromoterDownstream As UInteger = 0

    Private Class TextboxThreshold
        Inherits TextBox
        Public Sub New(ByVal ID As Integer, ByVal Threshold As Integer, ByVal ThresholdMean As Integer, ByVal ThresholdMin As Integer, ByVal ThresholdMax As Integer)
            Me.GFID = ID
            Me.Text = Threshold
        End Sub
        Public GFID As Integer
    End Class

    Public Sub New(ByRef genomicFeatures As List(Of GenomicFeature), ByRef PromoterUpstream As UInteger, ByRef PromoterDownstream As UInteger)
        InitializeComponent()
        Me.PromoterDownstream = PromoterDownstream
        Me.PromoterUpstream = PromoterUpstream
        GenomicFeaturesWithCustomThresholds = genomicFeatures 'sets the list of genomic features in the Form equal to the genomic inherited from the parent form
        Dim currIndex As Integer = 0
        For Each GF In genomicFeatures
            'adds a text box and label for each feature that uses a query type of threshold
            If GF.QueryType = "Threshold" Then
                Dim lblThreshold As New Label
                lblThreshold.Text = GF.Name & " [" & GF.ThresholdMin & " - " & GF.ThresholdMax & "]"
                lblThreshold.AutoSize = True
                lblThreshold.AutoEllipsis = True
                lblThreshold.Margin = New System.Windows.Forms.Padding(0, 5, 0, 0)
                Dim txtThreshold As New TextboxThreshold(GF.id, GF.Threshold, GF.ThresholdMean, GF.ThresholdMin, GF.ThresholdMax)
                ToolTip2.SetToolTip(txtThreshold, "Mean - " & GF.ThresholdMean)
                TableLayoutPanel1.Controls.Add(lblThreshold, 0, currIndex)
                TableLayoutPanel1.Controls.Add(txtThreshold, 1, currIndex)
                currIndex += 1
            End If
        Next
    End Sub


    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        'Gets the indexes of all fo the  TextboxThreshold controls in the tablelayoutpanel
        Dim IndexesOftxtThresholds As New List(Of Integer)
        For i As Integer = 0 To TableLayoutPanel1.Controls.Count - 1
            If TypeOf TableLayoutPanel1.Controls(i) Is TextboxThreshold Then
                IndexesOftxtThresholds.Add(i)
            End If
        Next

        For Each Gf In GenomicFeaturesWithCustomThresholds
            'goes through each of the genomicfeature thresholds and sets them equal to their corrisponding textboxes
            For Each index In IndexesOftxtThresholds
                Dim txtThreshold As TextboxThreshold = TableLayoutPanel1.Controls(index)
                If Gf.id = txtThreshold.GFID Then
                    Gf.Threshold = txtThreshold.Text
                End If
            Next
        Next
        PromoterDownstream = txtPromoterDownstream.Value
        PromoterUpstream = txtPromoterUpstream.Value
        Me.Close()
    End Sub

    Public Overloads Function ShowDialog(ByRef genomicFeatures As List(Of GenomicFeature)) As List(Of GenomicFeature)
        Return genomicFeatures
    End Function

    Private Sub FormCustomizeFilters_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtPromoterUpstream.Value = PromoterUpstream
        txtPromoterDownstream.Value = PromoterDownstream
    End Sub
End Class