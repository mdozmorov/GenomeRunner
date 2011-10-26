'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports System.IO
Imports System.Windows.Forms
Imports GenomeRunner.GenomeRunner

Public Class frmGenerateRandomFeatures
    Dim GREngine As New GenomeRunnerEngine
    Dim RandomFeatures As New List(Of Feature)

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DidWork As Integer

        SaveFD.Title = "Select a location to output the random features"
        SaveFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
        SaveFD.FileName = "Random.bed"
        DidWork = SaveFD.ShowDialog()
        If DidWork = DialogResult.Cancel Then
            MsgBox("Filename required!") : Exit Sub
        End If

        Dim Background As List(Of Feature) = GREngine.GetGenomeBackground(frmGenomeRunner.DatabaseConnection.ConnectionString) 'gets the entire genome as a background
        RandomFeatures = GenerateRandomFeatures(Background)
        Using writer As New StreamWriter(SaveFD.FileName)
            For Each ranFeature In RandomFeatures
                writer.WriteLine(ranFeature.Chrom & vbTab & ranFeature.ChromStart & vbTab & ranFeature.ChromEnd)
            Next
        End Using
        MessageBox.Show("Random Features Outputted to: " & SaveFD.FileName)
        Me.Close()
    End Sub

    Private Function GenerateRandomFeatures(ByVal Background As List(Of Feature)) As List(Of Feature)
        Dim listRandomFeatures As New List(Of Feature), state As hqrndstate
        Dim varianceAmount As Integer = txtMeanWidth.Value
        hqrndrandomize(state)                                                                           'Initialize random number generator
        For i As Integer = 1 To txtNumOfFeaturesToCreate.Value
            Dim RandomFeature As New Feature
            Dim currChrom As Integer = hqrnduniformi(state, Background.Count - 1)                       'selects a random number which corrisponds to a background interval, for the whole genome as a background each interval corrisponds to one chromosome
            RandomFeature.ChromStart = hqrnduniformi(state, Background(currChrom).ChromEnd)
            Dim RandomFeatureLength As Integer = hqrnduniformi(state, varianceAmount)                   'the variance ammount limits how wide the feature can be
            RandomFeature.ChromEnd = RandomFeature.ChromStart + RandomFeatureLength
            RandomFeature.Chrom = Background(currChrom).Chrom
            listRandomFeatures.Add(RandomFeature)
        Next
        Return listRandomFeatures
    End Function

    'generates randomfeatures based on the length of the FOIs passed on to it
    Private Function createRandomRegions(ByVal FeaturesOfInterest As List(Of Feature), ByVal BackgroundInterval As List(Of Feature), ByVal UseSpot As Boolean) As List(Of Feature)
        Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
        Dim RandomFeatures As New List(Of Feature)

        'Generating random coordinates using Alglib package
        Dim CurrBkgChr As UInteger, CurrBkgIntervalStart As Integer, CurrBkgIntervalEnd As Integer, CurrBkgIntervalLength As Integer, state As hqrndstate
        Dim CurrBkgBufferEnd As Integer                                                     'The max endpoint that can be selected that allows for the entire region to fit within the chromosome
        Dim currFeature As Integer = 0
        hqrndrandomize(state)                                               'Initialize random number generator
        For i As Integer = 0 To NumOfFeatures - 1 Step +1                                  'Same number of random features as for experimental, start at second half of array 
            Dim randomFeature As New Feature 'stores the random feature generated and is added to the list of Random Features
            CurrBkgChr = hqrnduniformi(state, BackgroundInterval.Count - 1)                    'Select random chromosome from 0 through bkgNum-1
            CurrBkgIntervalLength = FeaturesOfInterest(i).ChromEnd - FeaturesOfInterest(i).ChromStart  'gets the length of the FOI in order to create a random feature of the same length(this was calculated earlier and stored in an array before FIO start and end arrays were errased)

            'Random intraval coordinate: random number from 0 through [End-Length]
            CurrBkgBufferEnd = BackgroundInterval(CurrBkgChr).ChromEnd - CurrBkgIntervalLength  'is a buffer that prevents the region from being larger than the chromosome
            CurrBkgIntervalStart = hqrnduniformi(state, BackgroundInterval(CurrBkgChr).ChromEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart
            If CurrBkgBufferEnd >= 0 Then 'checks to see if the startpoint is not negative 
                While CurrBkgIntervalStart > CurrBkgBufferEnd 'LC 6/20/11 added in case the random startpoint fell within the buffer region
                    CurrBkgIntervalStart = hqrnduniformi(state, BackgroundInterval(CurrBkgChr).ChromEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart 'LC 6/20/11 changed to prevent random startpoint that falls within the end region "---" |......---|
                End While
                CurrBkgIntervalEnd = CurrBkgIntervalStart + CurrBkgIntervalLength  'the endpoint is start+length of feature
                randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom                           'Store CurrBkgChr chromosome
                randomFeature.ChromStart = CurrBkgIntervalStart                            'and corresponding random coordinate within it
                randomFeature.ChromEnd = CurrBkgIntervalEnd
            Else 'if the startpoint is negative, then the region is larger than the chromosome and so the region is set to be entire region of chromosome   
                randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom
                randomFeature.ChromStart = BackgroundInterval(CurrBkgChr).ChromStart
                randomFeature.ChromEnd = BackgroundInterval(CurrBkgChr).ChromEnd
            End If
            RandomFeatures.Add(randomFeature)  'adds the new random feature to the list
            currFeature += 1
        Next

        Return RandomFeatures
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class