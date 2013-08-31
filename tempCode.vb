'Private Sub mnuCoordinatesToSNPs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCoordinatesToSNPs.Click
'    'Convert SNP Chr-Start coordinates into rs*** names
'    Dim DidWork As Integer, i As Integer
'    OpenFD.Title = "Select a tab-delimited file with SNP chromosome and start position"
'    OpenFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
'    OpenFD.FileName = ""
'    OpenFD.Filter = "All files|*.*|Text Files|*.txt|BED Files|*.bed"
'    DidWork = OpenFD.ShowDialog()
'    If DidWork = DialogResult.Cancel Then
'        MsgBox("Filename required!") : Exit Sub

'    Else
'        FileName = OpenFD.SafeFileName
'    End If
'    InputFileName = Path.GetFileNameWithoutExtension(OpenFD.FileName)

'    Dim SNPChr() As String, SNPStart() As String, SNPCount As UInteger
'    Dim line As String, sp() As String
'    Using reader As StreamReader = New StreamReader(OpenFD.FileName)
'        While Not reader.EndOfStream                                                'Input SNP coordinates, in format Chr-Tab-Start
'            line = reader.ReadLine : sp = line.Split(vbTab)
'            SNPCount += 1 : lblProgress.Text = "Processing SNP " & SNPCount         'Count
'            ReDim Preserve SNPChr(SNPCount) : SNPChr(SNPCount) = sp(0).ToString     'Allocate space and store Chromosome
'            ReDim Preserve SNPStart(SNPCount) : SNPStart(SNPCount) = sp(1).ToString 'Allocate space ans store Start
'        End While
'    End Using
'    OpenDatabase() : ProgressBar1.Maximum = SNPCount
'    Dim Add1 As UInteger = InputBox("UCSC snp130 table has 0-based coordinates." & vbCrLf & "Are your coordinates 0- or 1-based?", "Coordinate system", 1)
'    Using writer As StreamWriter = New StreamWriter(Path.GetDirectoryName(OpenFD.FileName) & InputFileName & "_SNP2Coords.txt")
'        For i = 1 To SNPCount                                                       'Query database to get SNP coordinates
'            ProgressBar1.Value = i : lblProgress.Text = "Converting SNP coordinates " & i : Application.DoEvents()
'            'Note SNPStart(i)-1 - coordinates in UCSC tables are 0 based. Add or remove -1
'            cmd = New MySqlCommand("SELECT name FROM snp130 WHERE chrom='" & SNPChr(i) & "' AND chromStart=" & SNPStart(i) - Add1 & ";", cn)
'            dr = cmd.ExecuteReader
'            If dr.HasRows Then
'                While dr.Read       'Output ALL mapped SNP names
'                    writer.WriteLine(i & vbTab & SNPChr(i) & vbTab & SNPStart(i) & vbTab & dr(0).ToString)
'                End While
'            Else                    'If nothing is detected, output coordinates with empty name field
'                writer.WriteLine(i & vbTab & SNPChr(i) & vbTab & SNPStart(i) & vbTab & "")
'            End If
'            dr.Close()
'        Next
'    End Using
'    MsgBox("SNP coordinates converted and saved in" & vbCrLf & Path.GetDirectoryName(OpenFD.FileName) & InputFileName & "_SNP2Coords.txt")
'End Sub

'Public Sub InputBackgroundSpot()
'    'Read in interval background file, each line defines genomic interval
'    Dim Line1 As String, sp() As String
'    FileOpen(100, OpenFD.FileName, OpenMode.Input, OpenAccess.Default, OpenShare.Shared)
'    bkgNum = 0 : BackgroundFeatures.Clear() 'If background opened several times, count of background features will reset for each
'    While Not EOF(100)
'        Dim feature As New Feature  'creates a new instance of the Feature class in order to store the background information
'        Line1 = LineInput(100) : sp = Line1.Split(vbTab)
'        If Mid(sp(0), 1, 3) = "chr" Then                            'First letters should define chromosome, other control and design features ignored
'            bkgNum += 1                                            'Total number of background features
'            feature.Chrom = sp(0) : feature.ChromStart = sp(1)
'            If sp.Length > 2 Then 'checks if there is a third column
'                If IsNumeric(sp(2)) = True And sp(2) <> "" Then 'checks if third column contains numbers and is not blank
'                    feature.ChromEnd = sp(2)
'                Else
'                    feature.ChromEnd = feature.ChromStart
'                End If
'            Else
'                feature.ChromEnd = feature.ChromStart
'            End If
'            BackgroundFeatures.Add(feature)  'adds the background interval to the list of background feature intervals
'        End If
'        '  lblProgress.Text = "Background processed " & bkgNum : Application.DoEvents()
'    End While
'    FileClose(100)
'    bkgNum -= 1  'makes the background count zero based
'End Sub

'Private Sub mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem.Click
'    Dim reply As DialogResult = MessageBox.Show("Loading whole snp130 DB takes ~3 hours" & vbCrLf & "Do you want to proceed?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
'    If reply = DialogResult.Yes Then
'        OpenDatabase()
'        bkgNum = 0 : BackgroundFeatures.Clear()                 'If background opened several times, count of background features will reset for each
'        cmd = New MySqlCommand("SELECT chrom, chromstart FROM snp130", cn)
'        ProgressBar1.Maximum = bkgNum : bkgNum = 0                  'Initialize progress bar and counter
'        dr = cmd.ExecuteReader
'        While dr.Read
'            Dim feature As New Feature
'            bkgNum += 1 'lblProgress.Text = "Background processed " & bkgNum
'            feature.Chrom = dr(0) : feature.ChromStart = dr(1) : feature.ChromEnd = feature.ChromStart  'sets the chromstart and chromend equal to each other
'            BackgroundFeatures.Add(feature)
'        End While
'        dr.Close()
'        MsgBox("Background loaded into memory")
'        bkgNum -= 1
'    Else
'        Exit Sub
'    End If
'End Sub


'Private Sub createRandomRegions(ByVal numRandomFeatures As Integer, ByVal FeatureLength As Integer)
'    'Generating random coordinates using Alglib package
'    Dim CurrBkgChr As UInteger, CurrBkgIntervalStart As Integer, CurrBkgIntervalEnd As Integer, CurrBkgIntervalLength As Integer, state As hqrndstate
'    Dim CurrBkgBufferEnd As Integer 'LC 6/20/11 is the max endpoint that can be selected that allows for the entire region to fit within the chromosome
'    Dim currFeature As Integer = 0 'used to cycle through first half of array to get values of the FOI 
'    ' ReDim RandChr(NumOfFeatures) : ReDim RandStart(NumOfFeatures) : ReDim RandEnd(NumOfFeatures)       'LC 6/20/11 Prepare arrays for random features: not needed as values are store in the feature array
'    'Erase randEventsCount : ReDim randEventsCount(NumOfFeatures + 1)        'Prepare array for counting features
'    hqrndrandomize(state)                                               'Initialize random number generator
'    For i As Integer = 0 To numRandomFeatures Step +1                                  'LC 6/20/11 Same number of random features as for experimental, start at second half of array 
'        Dim randomFeature As New Feature 'stores the random feature generated and is added to the list of Random Features
'        'For currFeature = 0 To NumOfFeatures Step +1 'LC 6/22/11 not needed  'LC 6/21/11 cycles through the first half of the array to get the lenghts of the regions
'        CurrBkgChr = hqrnduniformi(state, BackgroundFeatures.Count)                    'Select random chromosome from 0 through bkgNum-1
'        CurrBkgIntervalLength = FeatureLength  'gets the length of the FOI in order to create a random feature of the same length(this was calculated earlier and stored in an array before FIO start and end arrays were errased)

'        'Random intraval coordinate: random number from 0 through [End-Length]
'        CurrBkgBufferEnd = BackgroundFeatures(CurrBkgChr).ChromEnd - CurrBkgIntervalLength  'is a buffer that prevents the region from being larger than the chromosome
'        CurrBkgIntervalStart = hqrnduniformi(state, BackgroundFeatures(CurrBkgChr).ChromEnd - BackgroundFeatures(CurrBkgChr).ChromStart + 1) + BackgroundFeatures(CurrBkgChr).ChromStart
'        If CurrBkgBufferEnd >= 0 Then 'checks to see if the startpoint is not negative 
'            While CurrBkgIntervalStart > CurrBkgBufferEnd 'LC 6/20/11 added in case the random startpoint fell within the buffer region
'                CurrBkgIntervalStart = hqrnduniformi(state, BackgroundFeatures(CurrBkgChr).ChromEnd - BackgroundFeatures(CurrBkgChr).ChromStart + 1) + BackgroundFeatures(CurrBkgChr).ChromStart 'LC 6/20/11 changed to prevent random startpoint that falls within the end region "---" |......---|
'            End While
'            CurrBkgIntervalEnd = CurrBkgIntervalStart + CurrBkgIntervalLength  'LC 6/20/11 the endpoint is start+length of feature
'            randomFeature.Chrom = BackgroundFeatures(CurrBkgChr).Chrom                           'Store CurrBkgChr chromosome
'            randomFeature.ChromStart = CurrBkgIntervalStart                            'and corresponding random coordinate within it
'            randomFeature.ChromEnd = CurrBkgIntervalEnd                           'LC 6/20/11 added
'        Else 'LC 6/20/11 if the startpoint is negative, then the region is larger than the chromosome and so the region is set to be entire region of chromosome   
'            randomFeature.Chrom = BackgroundFeatures(CurrBkgChr).Chrom
'            randomFeature.ChromStart = BackgroundFeatures(CurrBkgChr).ChromStart
'            randomFeature.ChromEnd = BackgroundFeatures(CurrBkgChr).ChromEnd
'        End If
'        RandomFeatures.Add(randomFeature)  'adds the new random feature to the list
'        currFeature += 1
'    Next
'End Sub

'Private Sub SetRandFeatureInterval1(ByVal NumOfFeatures)
'    'LC 6/20/11 changed to generate random regions
'    'Generating random coordinates using Alglib package
'    Dim CurrBkgChr As UInteger, CurrBkgIntervalStart As Integer, CurrBkgIntervalEnd As Integer, CurrBkgIntervalLength As Integer, state As hqrndstate
'    Dim CurrBkgBufferEnd As Integer 'LC 6/20/11 is the max endpoint that can be selected that allows for the entire region to fit within the chromosome
'    Dim currFeature As Integer = 0 'used to cycle through first half of array to get values of the FOI 
'    hqrndrandomize(state)                                               'Initialize random number generator
'    For i As Integer = 0 To NumOfFeatures Step +1
'        Dim randomFeature As New Feature 'stores the random feature generated and is added to the list of Random Features
'        CurrBkgChr = hqrnduniformi(state, BackgroundFeatures.Count)                    'Select random chromosome from 0 through bkgNum-1
'        CurrBkgIntervalLength = Features(i).ChromEnd - Features(i).ChromStart  'gets the length of the FOI in order to create a random feature of the same length(this was calculated earlier and stored in an array before FIO start and end arrays were errased)

'        'Random intraval coordinate: random number from 0 through [End-Length]
'        CurrBkgBufferEnd = BackgroundFeatures(CurrBkgChr).ChromEnd - CurrBkgIntervalLength  'is a buffer that prevents the region from being larger than the chromosome
'        CurrBkgIntervalStart = hqrnduniformi(state, BackgroundFeatures(CurrBkgChr).ChromEnd - BackgroundFeatures(CurrBkgChr).ChromStart + 1) + BackgroundFeatures(CurrBkgChr).ChromStart
'        If CurrBkgBufferEnd >= 0 Then 'checks to see if the startpoint is not negative 
'            While CurrBkgIntervalStart > CurrBkgBufferEnd 'LC 6/20/11 added in case the random startpoint fell within the buffer region
'                CurrBkgIntervalStart = hqrnduniformi(state, BackgroundFeatures(CurrBkgChr).ChromEnd - BackgroundFeatures(CurrBkgChr).ChromStart + 1) + BackgroundFeatures(CurrBkgChr).ChromStart 'LC 6/20/11 changed to prevent random startpoint that falls within the end region "---" |......---|
'            End While
'            CurrBkgIntervalEnd = CurrBkgIntervalStart + CurrBkgIntervalLength  'LC 6/20/11 the endpoint is start+length of feature
'            randomFeature.Chrom = BackgroundFeatures(CurrBkgChr).Chrom                           'Store CurrBkgChr chromosome
'            randomFeature.ChromStart = CurrBkgIntervalStart                            'and corresponding random coordinate within it
'            randomFeature.ChromEnd = CurrBkgIntervalEnd                           'LC 6/20/11 added
'        Else 'LC 6/20/11 if the startpoint is negative, then the region is larger than the chromosome and so the region is set to be entire region of chromosome   
'            randomFeature.Chrom = BackgroundFeatures(CurrBkgChr).Chrom
'            randomFeature.ChromStart = BackgroundFeatures(CurrBkgChr).ChromStart
'            randomFeature.ChromEnd = BackgroundFeatures(CurrBkgChr).ChromEnd
'        End If
'        RandomFeatures.Add(randomFeature)  'adds the new random feature to the list
'        currFeature += 1
'    Next
'End Sub


'Private Sub Calculate_BackgroundBase_Region_Count(ByVal GenomicFeature As GenomicFeature, ByRef BackgroundIntervals As List(Of Feature), ByRef G As Double, ByRef B As Double, ByRef nB As Double)
'    Dim TableName As String
'    Dim ListChroms As New List(Of Feature)
'    Dim bkgIntervals As List(Of Feature) = OrginizeFeaturesByChrom(BackgroundIntervals)
'    G = 0 'the total number of base pairs = G 
'    B = 0 'the total number of base pairs covered by the feature
'    nB = 0 'the total number of regions = 
'    Dim bkgChr As String(), bkgStart As Integer(), bkgEnd As Integer()
'    bkgNum = 24 : ReDim bkgChr(bkgNum) : ReDim bkgStart(bkgNum) : ReDim bkgEnd(bkgNum) 'These arrays hold the length of each of the chromosomes.  
'    bkgChr = {"chr1", "chr10", "chr11", "chr12", "chr13", "chr14", "chr15", "chr16", "chr17", "chr18", "chr19", "chr2", "chr20", "chr21", "chr22", "chr3", "chr4", "chr5", "chr6", "chr7", "chr8", "chr9", "chrM", "chrX", "chrY"}
'    'bkgStart = {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
'    bkgStart = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
'    bkgEnd = {247249719, 135374737, 134452384, 132349534, 114142980, 106368585, 100338915, 88827254, 78774742, 76117153, 63811651, 242951149, 62435964, 46944323, 49691432, 199501827, 191273063, 180857866, 170899992, 158821424, 146274826, 140273252, 16571, 154913754, 57772954}

'    'gets one of each chromosome in the list of background intervals and stores it and it's total length
'    For Each interval In bkgIntervals
'        Dim currChrom As Integer = Array.IndexOf(bkgChr, interval.Chrom)
'        Dim chrom As New Feature With {.Chrom = interval.Chrom, .ChromEnd = bkgEnd(currChrom)}
'        'if the chromosome has not been added to the list it is now added
'        ListChroms.Add(chrom)
'    Next

'    OrginizeFeaturesByChrom(ListChroms) 'orginizes by chrom so that duplicates can be removed
'    Dim lastChrom As String = ""
'    'duplicates are removed
'    Dim listIndexesToRemove As New List(Of Integer)
'    For Each chrom In ListChroms
'        If chrom.Chrom = lastChrom Then
'            listIndexesToRemove.Add(ListChroms.IndexOf(chrom))
'        End If
'        lastChrom = chrom.Chrom
'    Next
'    For i As Integer = listIndexesToRemove.Count - 1 To 0 Step -1
'        ListChroms.RemoveAt(i)
'    Next


'    'gets the total number of base pairs of the interval.  Accounts for possible overlap of intervals
'    For Each chrom In ListChroms
'        Dim BasePairs(bkgEnd(Array.IndexOf(bkgChr, chrom.Chrom))) As Boolean            'creates a list to store whether the interval covers base pairs or not, the size being the length of the current chromosome
'        'sets all of the base pairs to false
'        For Each bp In BasePairs
'            bp = False
'        Next
'        'goes through each of the intervals and changes the base values from false to true for the bp that are covered by those intervals 
'        For Each interval In BackgroundIntervals
'            If interval.Chrom = chrom.Chrom Then
'                For currBase As Integer = interval.ChromStart To interval.ChromEnd
'                    BasePairs(currBase) = True
'                Next
'            End If
'        Next
'        'counts the total number of bp covered by the interval
'        For Each bp In BasePairs
'            If bp = True Then
'                G += 1
'            End If
'        Next
'    Next

'    OpenDatabase()
'    cmd = New MySqlCommand("SELECT featureTable FROM genomerunner WHERE id='" & GenomicFeature.id & "';", cn)
'    dr = cmd.ExecuteReader()
'    While dr.Read()
'        TableName = dr(0)
'    End While
'    dr.Close()


'    Dim ChromFeatures As New List(Of Feature)
'    For Each chrom In ListChroms
'        ChromFeatures.Clear()
'        Try
'            cmd = New MySqlCommand("SELECT chromStart,ChromEnd FROM " & TableName & " WHERE chrom='" & chrom.Chrom & "';", cn)
'            dr = cmd.ExecuteReader()
'        Catch
'            cmd = New MySqlCommand("SELECT txStart,txend FROM " & TableName & " WHERE chrom='" & chrom.Chrom & "';", cn)
'            dr = cmd.ExecuteReader()
'        End Try
'        While dr.Read()
'            ChromFeatures.Add(New Feature With {.ChromStart = dr(0), .ChromEnd = dr(1)})
'        End While
'        dr.Close()

'        'goes through each feature and changes the isHit values of the ChromBase list to true for all base
'        'pairs that are covered by the features region
'        Dim BasePairs(bkgEnd(Array.IndexOf(bkgChr, chrom.Chrom))) As Boolean             'creates a list to store whether basepair has a hit or not the size being the length of the current chromosome
'        For Each currFeature In ChromFeatures

'            For CurrBase As UInteger = currFeature.ChromStart To currFeature.ChromEnd
'                BasePairs(CurrBase) = True
'            Next
'        Next

'        'goes through each of the background intervals on the current chromosome and counts only those basepairs and regions that fall within that interval
'        For Each interval In BackgroundIntervals
'            Dim LastBaseHit As Boolean = False 'keeps track of whether the last base pair was a hit or not
'            If interval.Chrom = chrom.Chrom Then 'checks if interval is on the current chrom
'                For CurrBase As Integer = interval.ChromStart To interval.ChromEnd
'                    If BasePairs(CurrBase) = True Then
'                        B += 1
'                        If LastBaseHit = False Then
'                            nB += 1
'                            LastBaseHit = True
'                        End If
'                    Else
'                        LastBaseHit = False
'                    End If
'                Next
'            End If
'        Next
'    Next
'End Sub