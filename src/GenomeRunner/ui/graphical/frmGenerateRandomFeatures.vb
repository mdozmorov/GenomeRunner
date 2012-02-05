'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports System.IO
Imports System.Windows.Forms
Imports GenomeRunner.GenomeRunner
Imports alglib

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
        Dim FOIs As New List(Of Feature) 'Dummy feature list, as template for random features creation
        For i = 0 To CInt(txtNumOfFeaturesToCreate.Value)
            Dim feature As New Feature
            feature.Chrom = "chrX"
            feature.ChromStart = 0
            feature.ChromEnd = feature.ChromStart + CInt(txtMeanWidth.Value)
            FOIs.Add(feature)
        Next

        Dim Background As List(Of Feature) = GREngine.GetChromInfo(frmGenomeRunner.DatabaseConnection) '.ConnectionString) 'gets the entire genome as a background
        RandomFeatures = createRandomRegions(FOIs, Background, frmGenomeRunner.GetUseSpot)
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
    'generates randomfeatures from the background based on the length of the FOIs passed on to it
    Public Function createRandomRegions(ByVal FeaturesOfInterest As List(Of Feature), ByVal BackgroundInterval As List(Of Feature), ByVal UseSpot As Boolean)
        Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
        Dim RandomFeatures As New List(Of Feature)
        Dim rand As New System.Random
        If UseSpot = False Then
            'Generating feature of interest form the interval background, which can be the entire genome
            Dim CurrBkgChr As UInteger, CurrBkgIntervalStart As Integer, CurrBkgIntervalEnd As Integer, CurrBkgIntervalLength As Integer, state As hqrndstate
            Dim CurrBkgBufferEnd As Integer                                                             'The max endpoint that can be selected that allows for the entire region to fit within the chromosome
            hqrndrandomize(state)                                                                       'Initialize random number generator
            For i As Integer = 0 To NumOfFeatures - 1 Step +1
                Dim randomFeature As New Feature                                                        'stores the random feature generated and is added to the list of Random Features
                CurrBkgChr = getWeightedRandomChromosome(state, BackgroundInterval)
                'CurrBkgChr = hqrnduniformi(state, BackgroundInterval.Count)                         'Select random interval from the background
                'CurrBkgChr = rand.Next(0, BackgroundInterval.Count - 1)
                CurrBkgIntervalLength = FeaturesOfInterest(i).ChromEnd - FeaturesOfInterest(i).ChromStart  'gets the length of the FOI in order to create a random feature of the same length(this was calculated earlier and stored in an array before FIO start and end arrays were errased)
                'Random interval coordinate: random number from 0 through [End-Length]
                CurrBkgBufferEnd = BackgroundInterval(CurrBkgChr).ChromEnd - CurrBkgIntervalLength      'is a buffer that prevents the region from being larger than the interval

                If CurrBkgBufferEnd - BackgroundInterval(CurrBkgChr).ChromStart >= 0 Then
                    'CurrBkgIntervalStart = rand.Next(BackgroundInterval(CurrBkgChr).ChromStart, CurrBkgBufferEnd)
                    CurrBkgIntervalStart = hqrnduniformi(state, CurrBkgBufferEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart 'LC 6/20/11 changed to prevent random startpoint that falls within the end region "---" |......---|
                    randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom                          'Store CurrBkgChr chromosome
                    randomFeature.ChromStart = CurrBkgIntervalStart                                     'and corresponding random coordinate within it
                    randomFeature.ChromEnd = CurrBkgIntervalStart + CurrBkgIntervalLength                   'the endpoint is start+length of feature
                Else
                    ' if it is negative, than the Feature is greater than the background interval and thus is set to be equal to the interval
                    randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom
                    randomFeature.ChromStart = BackgroundInterval(CurrBkgChr).ChromStart
                    randomFeature.ChromEnd = BackgroundInterval(CurrBkgChr).ChromEnd
                End If

                'Debug.Print(randomFeature.ChromEnd - randomFeature.ChromStart & vbTab & FeaturesOfInterest(i).ChromEnd - FeaturesOfInterest(i).ChromStart)


                ''CurrBkgIntervalStart = hqrnduniformi(state, BackgroundInterval(CurrBkgChr).ChromEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart
                ''CurrBkgIntervalStart = hqrnduniformi(state, CurrBkgBufferEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart
                'CurrBkgIntervalStart = rand.Next(BackgroundInterval(CurrBkgChr).ChromStart, CurrBkgBufferEnd)

                '' if the buffered end is not negative, a random feature is created.
                'If CurrBkgBufferEnd >= 0 Then
                '    'While CurrBkgIntervalStart > CurrBkgBufferEnd                                       'added in case the random startpoint fell within the buffer region
                '    '    CurrBkgIntervalStart = hqrnduniformi(state, BackgroundInterval(CurrBkgChr).ChromEnd - BackgroundInterval(CurrBkgChr).ChromStart + 1) + BackgroundInterval(CurrBkgChr).ChromStart 'LC 6/20/11 changed to prevent random startpoint that falls within the end region "---" |......---|
                '    'End While
                '    'CurrBkgIntervalEnd = CurrBkgIntervalStart + CurrBkgIntervalLength                   'the endpoint is start+length of feature
                '    'randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom                          'Store CurrBkgChr chromosome
                '    'randomFeature.ChromStart = CurrBkgIntervalStart                                     'and corresponding random coordinate within it
                '    'randomFeature.ChromEnd = CurrBkgIntervalEnd

                '    randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom                          'Store CurrBkgChr chromosome
                '    randomFeature.ChromStart = CurrBkgIntervalStart                                     'and corresponding random coordinate within it
                '    randomFeature.ChromEnd = CurrBkgIntervalStart + CurrBkgIntervalLength                   'the endpoint is start+length of feature
                '    Debug.Print(randomFeature.ChromEnd - randomFeature.ChromStart & vbTab & FeaturesOfInterest(i).ChromEnd - FeaturesOfInterest(i).ChromStart)
                'Else
                '    ' if it is negative, than the Feature is greater than the background interval and thus is set to be equal to the interval
                '    randomFeature.Chrom = BackgroundInterval(CurrBkgChr).Chrom
                '    randomFeature.ChromStart = BackgroundInterval(CurrBkgChr).ChromStart
                '    randomFeature.ChromEnd = BackgroundInterval(CurrBkgChr).ChromEnd
                'End If
                RandomFeatures.Add(randomFeature)                                                      'adds the new random feature to the list

                'Debug.Print(CurrBkgIntervalLength & vbTab & randomFeature.ChromEnd - randomFeature.ChromStart)
            Next
        Else
            'generates a ranomd Feature from the spot background
            Dim RandFeature As Integer, state As hqrndstate
            hqrndrandomize(state)                                                                      'Initialize random number generator
            For i As Integer = 0 To NumOfFeatures - 1
                Dim feature As New Feature
                RandFeature = hqrnduniformi(state, BackgroundInterval.Count)                           'Random position from the whole spectrum of spots
                feature.Chrom = BackgroundInterval(RandFeature).Chrom                                  'Store chromosome of random position
                feature.ChromStart = BackgroundInterval(RandFeature).ChromStart                        'and start coordinate
                feature.ChromEnd = BackgroundInterval(RandFeature).ChromEnd
                RandomFeatures.Add(feature)
            Next
            'Return RandomFeatures                                                                       'returns the list of randomly generated features
        End If
        Return RandomFeatures
    End Function

    Private Function getWeightedRandomChromosome(ByRef state As hqrndstate, ByVal background As List(Of Feature)) As UInteger
        'This function essentially goes from this:
        '|------chr1-----|---chr2---|--chr3--| etc.
        '0             1000       1500     1750
        '
        'To this:
        '|------chr1-----|---chr2---|--chr3--| etc.
        '0              .57        .86      1.0
        'Since total summing up chromosome length gets bigger than VB data types can handle,
        'these are handled as percentages instead.

        'Find combined length of all chromosomes from background.
        Dim combinedChromLength As ULong = 0
        For Each elem In background
            combinedChromLength += elem.ChromEnd
        Next

        'For calculation purposes, chrom will be assigned values between 0.0 & 1.0 based on their percentage of total length.
        Dim weightedChromPositions As New List(Of Double)
        weightedChromPositions.Add(background(0).ChromEnd / combinedChromLength)
        For i As Integer = 1 To background.Count - 1
            weightedChromPositions.Add((background(i).ChromEnd / combinedChromLength) + weightedChromPositions(i - 1))
        Next

        'Randomly select number between 0 & 1.0; find which chrom this number would be part of.
        'Dim randomIndex As Double = RandomClass.NextDouble()
        Dim randomIndex As Double = hqrnduniformr(state)
        Dim randChrom As Integer = -1
        Dim counter As Integer = 0
        While randChrom = -1
            If randomIndex <= weightedChromPositions(counter) Then
                randChrom = counter
            End If
            counter = counter + 1
        End While
        Return randChrom
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class