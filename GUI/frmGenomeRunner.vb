'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Linq
Imports GenomeRunner.GenomeRunner

Public Class frmGenomeRunner
    Dim cn As MySqlConnection, cmd As MySqlCommand, dr As MySqlDataReader, cmd1 As MySqlCommand, dr1 As MySqlDataReader
    Dim FileName As String
    Dim NumOfFeatures As Integer 'stores the information for the FOIs inputed by the user: is zero based

    Structure featureAvailable : Dim name As String, category As String, order As Integer : End Structure  'is used to store the featurenames and categories returned from the genomerunner table

    'Dim GRFeaturesToAnalyze As New List(Of GenomicFeature)
    Dim Features As New List(Of Feature), RandomFeatures As New List(Of Feature), BackgroundFeatures As New List(Of Feature)
    Dim featuresIndexes As New List(Of Integer)
    Dim MCcount As UInteger                                                                             'Number of Monte-Carlo simulations
    Dim currDir As String, InputFileName As String, BackgroundFileName As String                        'File system information
    Dim bkgNum As UInteger            'keeps count of the number of background features added: is 0 based  
    Dim arrayFeatureSQLData As Integer()
    Dim stopwatch As New System.Diagnostics.Stopwatch
    Dim kgIDToGeneSymbolDict As New Dictionary(Of String, String) 'is a dictionary that is loaded with the values from the kgxref (kgID, geneSymbol) to convert the gene name into standard form
    Dim GREngine As GenomeRunnerEngine
    Dim Background As New List(Of Feature)
    Dim BackgroundName As String = ""
    Dim UseSpotBackground As Boolean = False
    Dim ConnectionString As String
    Dim progStart As ProgressStart, progUpdate As ProgressUpdate, progDone As ProgressDone
    Dim AnalysisType As String = ""                                 'used to alert the user as to what type of analysis is being run
    Dim PromoterUpstream As UInteger = 0
    Dim PromoterDownstream As UInteger = 0

    'a listview item that also stores the id as a variable
    Public Class ListItemGenomicFeature
        Inherits ListViewItem
        Public GenomicFeature As GenomicFeature
        Public Sub New(ByVal GenomicFeature As GenomicFeature)
            Me.GenomicFeature = GenomicFeature
        End Sub
    End Class
    'stores the filepath as a variable for later use
    Private Class ListItemFile
        Inherits ListViewItem
        Public filPath As String
        Public fileDir As String
        Public fileName As String
    End Class


    Private Sub OpenDatabase()
        Dim uName As String = ""
        Dim uPassword As String = ""
        Dim uServer As String = ""
        Dim uDatabase As String = ""

        ConnectionString = GetConnectionSettings(uName, uPassword, uServer, uDatabase)
        'checks again to see if the values are still empty, if they are the connection is left blank and a connection is not oppened. 
        Dim ConnectionWorks As Boolean = False
        While ConnectionWorks = False
            Try
                cn = New MySqlConnection(ConnectionString) : cn.Open()
                cmd = New MySqlCommand("SELECT id FROM genomerunner limit 1", cn)
                dr = cmd.ExecuteReader()
                ConnectionWorks = True
                dr.Close() : cmd.Dispose()
            Catch
                frmLogin.ShowDialog()
                ConnectionString = GetConnectionSettings(uName, uPassword, uServer, uDatabase)
            End Try
        End While

    End Sub

    Private Function GetConnectionSettings(ByRef uName As String, ByRef uPassword As String, ByRef uServer As String, ByRef uDatabase As String) As String
        'gets the connection string values from the registry if they exist
        Dim connectionString As String
        Try
            uName = GetSetting("GenomeRunner", "Database", "uName")
            uPassword = GetSetting("GenomeRunner", "Database", "uPassword")
            uServer = GetSetting("GenomeRunner", "Database", "uServer")
            uDatabase = GetSetting("GenomeRunner", "Database", "uDatabase")
        Catch
            SaveSetting("GenomeRunner", "Database", "uName", "genomerunner")
            SaveSetting("GenomeRunner", "Database", "uPassword", "genomerunner")
            SaveSetting("GenomeRunner", "Database", "uServer", "156.110.144.34")
            SaveSetting("GenomeRunner", "Database", "uDatabase", "hg18test")
            uName = GetSetting("GenomeRunner", "Database", "uName")
            uPassword = GetSetting("GenomeRunner", "Database", "uPassword")
            uServer = GetSetting("GenomeRunner", "Database", "uServer")
            uDatabase = GetSetting("GenomeRunner", "Database", "uDatabase")
        End Try
        connectionString = "Server=" & uServer & ";Database=" & uDatabase & ";User ID=" & uName & ";Password=" & uPassword
        Return connectionString
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'tests to see if the connection works
        'Try
        '    OpenDatabase()
        'Catch
        '    frmLogin.ShowDialog()
        '    OpenDatabase()
        'End Try
        OpenDatabase()
        GREngine = New GenomeRunnerEngine()
        SetGenomeRunnerDefaults()
        Me.Location = New Point(10, 10)     'Manually set window location    
        cmbMatrixWeighting.SelectedIndex = 0
        cmbStrandsToAnalyze.SelectedIndex = 0
    End Sub

    'used to update progress of the analysis
    Private Sub HandleProgressStart(ByVal progressMaximum As Integer)
        SetProgress_ThreadSafe(ProgressBar1, 0)
    End Sub

    Private Sub HandleProgressUpdate(ByVal currProgress As Integer, ByVal FeatureFileName As String, ByVal GenomicFeatureName As String)
        SetProgress_ThreadSafe(Me.ProgressBar1, currProgress)
        SetProgressLabel_ThreadSafe(lblProgress, "Doing " & AnalysisType & " Analysis for " & FeatureFileName & ": " & GenomicFeatureName)
    End Sub

    Private Sub HandleProgressDone(ByVal OuputDir As String)
        SetProgress_ThreadSafe(Me.ProgressBar1, 0)
        MessageBox.Show(AnalysisType & " Analysis complete.  Results outputed to: " & OuputDir)
    End Sub

    'sets the default values of genome runner
    Private Sub SetGenomeRunnerDefaults()
        cmbFilterLevels.SelectedIndex = 0
        'sets the background to be the entire genome
        Background = GREngine.GetGenomeBackgroundHG18()
        BackgroundName = "hg18"
    End Sub

    Private Sub btnLoadPOIs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPOIs.Click
        mnuFileOpen_Click(sender, e)
        Try
            If currDir = vbNullString Then Exit Try
            SaveSetting("GenomeRunner", "Startup", "Folder", currDir)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub mnuFileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileOpen.Click
        Dim DidWork As Integer
        OpenFD.Title = "Select a tab-delimited file with FEATURE chromosome and start position"
        OpenFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
        OpenFD.FileName = ""
        OpenFD.Multiselect = True
        OpenFD.Filter = "All files|*.*|BED Files|*.bed|Text Files|*.txt"
        DidWork = OpenFD.ShowDialog()
        If DidWork = DialogResult.Cancel Then
            Exit Sub
        Else
            FileName = OpenFD.SafeFileName
        End If
        Dim FeatureFiles As String() = OpenFD.FileNames
        SaveSetting("GenomeRunner", "Startup", "Folder", Path.GetDirectoryName(OpenFD.FileName))
        For Each File In FeatureFiles
            Dim nFeatureFile As New ListItemFile
            nFeatureFile.filPath = Path.GetFullPath(File)
            nFeatureFile.fileDir = Path.GetDirectoryName(OpenFD.FileName) & "\"
            Dim fileName As String() = Strings.Split(File, "\")
            nFeatureFile.Text = fileName(fileName.Length - 1)
            nFeatureFile.fileName = Path.GetFileName(File)
            listFeatureFiles.Items.Add(nFeatureFile)
            InputFileName = Path.GetFileNameWithoutExtension(OpenFD.FileName)
        Next
    End Sub

    Private Sub mnuOpenBackgroundFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenBackgroundFileToolStripMenuItem.Click
        Dim DidWork As Integer
        OpenFD.Title = "Select a tab-delimited file with SPOT BACKGROUND"
        OpenFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
        OpenFD.FileName = ""
        OpenFD.Filter = "All files|*.*|BED Files|*.bed|Text Files|*.txt"
        DidWork = OpenFD.ShowDialog()
        If DidWork = DialogResult.Cancel Then
            MsgBox("Filename required!") : Exit Sub
        Else
            BackgroundName = OpenFD.SafeFileName
        End If
        BackgroundFileName = Path.GetFileNameWithoutExtension(OpenFD.FileName)
        Background = GREngine.GenerateCustomGenomeBackground(OpenFD.FileName)
        UseSpotBackground = True
        lblBackground.Text = "Using '" & OpenFD.SafeFileName & "' as spot background"
    End Sub

    Private Sub OpenBackgroundFileIntervalsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenBackgroundFileIntervalsToolStripMenuItem.Click
        Dim DidWork As Integer

        OpenFD.Title = "Select a tab-delimited file with INTERVAL BACKGROUND"
        OpenFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
        OpenFD.FileName = ""
        OpenFD.Filter = "All files|*.*|BED Files|*.bed|Text Files|*.txt"
        DidWork = OpenFD.ShowDialog()
        If DidWork = DialogResult.Cancel Then
            MsgBox("Filename required!") : Exit Sub
        Else
            BackgroundName = OpenFD.SafeFileName
        End If
        'Background.Clear()
        ''loads all of the background intervals 
        'Dim sr As New StreamReader(OpenFD.FileName)
        'While sr.EndOfStream = False
        '    Dim ArrayLine As String() = sr.ReadLine.Split(vbTab)
        '    Dim interval As New Feature With {.Chrom = ArrayLine(0), .ChromStart = ArrayLine(1), .ChromEnd = ArrayLine(2)}
        '    Background.Add(interval)
        'End While
        Background = GREngine.GenerateCustomGenomeBackground(OpenFD.FileName)
        BackgroundName = Path.GetFileName(OpenFD.FileName)
        UseSpotBackground = False
        rbUseMonteCarlo.Checked = True
        rbUseAnalytical.Enabled = False
        lblBackground.Text = "Using '" & OpenFD.SafeFileName & "' as interval background"
    End Sub


    Private Sub ComboBoxTier_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTier.SelectedIndexChanged
        If cmbTier.SelectedIndex <> -1 Then
            ListFeaturesAvailable.Items.Clear() 'clears all of the features available 
            LoadAvailableGenomicFeatures() 're-add all of the features of interest

            'removes all of the genomic features that are not included in the tier filter
            For j As Integer = ListFeaturesAvailable.Items.Count - 1 To 0 Step -1
                Dim selectedTier As Integer = cmbTier.SelectedItem.Replace("Tier", "").Replace("TFBS", "")
                Dim lvGF As ListItemGenomicFeature = ListFeaturesAvailable.Items.Item(j)
                If (lvGF.GenomicFeature.Tier > selectedTier) Or (lvGF.GenomicFeature.Tier <> selectedTier And selectedTier >= 100) Then
                    ListFeaturesAvailable.Items(j).Remove()
                End If
            Next
        End If
    End Sub

    Public Sub LoadAvailableGenomicFeatures()
        Dim GenomicFeatures As List(Of GenomicFeature) = GREngine.GetGenomicFeaturesAvailable(ConnectionString) 'gets all of the genomic features and adds them to a list
        Dim strCurrCate As String = ""
        Dim CurrCateIndex As Integer = -1
        Dim category As ListViewGroup
        'goes through each genomic feature and attaches it to a listview item and puts the listview item in the appropriate category
        For Each GRfeature In GenomicFeatures
            If GRfeature.QueryType(0) <> "*" Then                                                            'if query type contains '*' Genome runner can't recognize and won't run the genomic feature.
                If strCurrCate <> GRfeature.UICategory Then                                                 'checks if a new category has been reached
                    CurrCateIndex += 1                                                                      'moves the current group index to the index of the newly created cat
                    Dim arrayCat = Split(GRfeature.UICategory, "|")                                         'splits the catagory results in order to take off the numerical indexegory()
                    category = New ListViewGroup
                    category.Name = GRfeature.UICategory : category.Header = arrayCat(1) 'sets a new instance of a header level, the name is set equal to the category in order for it to be found later
                    Dim feature As New ListItemGenomicFeature(GRfeature) : feature.Text = GRfeature.Name : feature.Group = category : feature.GenomicFeature = GRfeature
                    ListFeaturesAvailable.Groups.Add(category)
                    feature.Name = GRfeature.Name : feature.Group = category : feature.ToolTipText = GRfeature.TableName
                    ListFeaturesAvailable.Items.Add(feature)
                    strCurrCate = GRfeature.UICategory
                Else
                    Dim feature As New ListItemGenomicFeature(GRfeature) : feature.Text = GRfeature.Name : feature.Name = GRfeature.Name : feature.Group = category : feature.ToolTipText = GRfeature.TableName
                    ListFeaturesAvailable.Items.Add(feature)
                End If
            End If
        Next
    End Sub


    Private Sub btnAddFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFeature.Click
        'adds all of the features available to the list of features to run
        For Each lvGF As ListItemGenomicFeature In ListFeaturesAvailable.SelectedItems
            Dim nGF As New ListItemGenomicFeature(lvGF.GenomicFeature)
            nGF.Text = lvGF.GenomicFeature.Name
            listFeaturesToRun.Items.Add(nGF)
            'checks if the GenomicFeature is a gene, if so a promotter and exon genomic feature are generated as well
            If lvGF.GenomicFeature.QueryType = "Gene" Then
                Dim nGFPromoter As New GenomicFeature(lvGF.GenomicFeature.id, lvGF.GenomicFeature.Name & "Promoter", lvGF.GenomicFeature.TableName, "Promoter", "NA", 0, "1000", "11000", "3000", "", 0, Nothing, "", 1)
                Dim nlvGFPromoter As New ListItemGenomicFeature(nGFPromoter)
                nlvGFPromoter.Text = lvGF.GenomicFeature.Name & "Promoter"
                listFeaturesToRun.Items.Add(nlvGFPromoter)
                Dim nGFExon As New GenomicFeature(lvGF.GenomicFeature.id, lvGF.GenomicFeature.Name & "Exon", lvGF.GenomicFeature.TableName & "Exons", "Exon", "NA", 0, 0, 0, 0, "", 0, Nothing, "", 1)
                Dim nlvGFExon As New ListItemGenomicFeature(nGFExon)
                nlvGFExon.Text = lvGF.GenomicFeature.Name & "Exon"
                listFeaturesToRun.Items.Add(nlvGFExon)
            End If
        Next
        cmbFilterLevels.SelectedIndex = 0
        SetFilterValues()
    End Sub


    Private Sub btnAddAllFeatures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAllFeatures.Click
        'adds all of the features available to the list of features to run
        For Each lvGF As ListItemGenomicFeature In ListFeaturesAvailable.Items
            Dim nlvGF As New ListItemGenomicFeature(lvGF.GenomicFeature)
            nlvGF.Text = lvGF.GenomicFeature.Name
            'nlvGF.ToolTipText = "Table name: " & lvGF.GenomicFeature.TableName
            listFeaturesToRun.Items.Add(nlvGF)
            'checks if the GenomicFeature is a gene, if so a promotter and exon genomic feature are generated as well
            If lvGF.GenomicFeature.QueryType = "Gene" Then
                Dim nGFPromoter As New GenomicFeature(lvGF.GenomicFeature.id, lvGF.GenomicFeature.Name & "Promoter", lvGF.GenomicFeature.TableName, "Promoter", "NA", 0, "1000", "11000", "3000", "", 0, Nothing, "", 1)
                Dim nlvGFPromoter As New ListItemGenomicFeature(nGFPromoter)
                nlvGFPromoter.Text = lvGF.GenomicFeature.Name & "Promoter"
                listFeaturesToRun.Items.Add(nlvGFPromoter)
                Dim nGFExon As New GenomicFeature(lvGF.GenomicFeature.id, lvGF.GenomicFeature.Name & "Exon", lvGF.GenomicFeature.TableName & "Exons", "Exon", "NA", 0, 0, 0, 0, "", 0, Nothing, "", 1)
                Dim nlvGFExon As New ListItemGenomicFeature(nGFExon)
                nlvGFExon.Text = lvGF.GenomicFeature.Name & "Exon"
                listFeaturesToRun.Items.Add(nlvGFExon)
            End If
        Next
        cmbFilterLevels.SelectedIndex = 0
        SetFilterValues()
    End Sub


    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim GRFeaturesToAnalyze As New List(Of GenomicFeature)
        Dim FeatureOfInterestFilePaths As New List(Of String)
        Dim AnoSettings As New AnnotationSettings(PromoterUpstream, PromoterDownstream, txtproximity.Value)
        Dim Analyzer As New AnnotationAnalysis(ConnectionString)
        AnalysisType = "Annotation"
        If listFeaturesToRun.Items.Count = 0 Then
            MessageBox.Show("Please select genomic features to analyze")
            Exit Sub
        End If
        btnRun.Enabled = False
        btnPValue.Enabled = False
        'Goes through each of the listview items in the list of features to run and adds each of the genomicfeatures that are attache to them to a list of GF to analyze
        For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
            GRFeaturesToAnalyze.Add(lvGF.GenomicFeature)
        Next

        ProgressBar1.Maximum = GRFeaturesToAnalyze.Count
        For Each FeatureFile As ListItemFile In listFeatureFiles.Items                                                          'goes through each of the files to annotate
            FeatureOfInterestFilePaths.Add(FeatureFile.filPath)
        Next
        Dim firstFile As ListItemFile = listFeatureFiles.Items(0)                                                               'gets the file information of the features of interest file
        Dim AnnotationOutputDir As String = firstFile.fileDir & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & " " & txtJobName.Text & "\"  'sets what directory the results are to outputed to

        Dim args As New AnnotationArguments(ConnectionString, GRFeaturesToAnalyze, FeatureOfInterestFilePaths, AnnotationOutputDir, AnoSettings)
        BackgroundWorkerAnnotationAnalysis.RunWorkerAsync(args)
    End Sub

    Private Sub BackgroundWorkerAnnotationAnalysis_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerAnnotationAnalysis.DoWork
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        'Get the argument
        Dim args As AnnotationArguments = e.Argument
        'starts the enrichment analysis
        Dim Analyzer As New AnnotationAnalysis(ConnectionString)
        Analyzer.RunAnnotationAnalysis(args.FeatureFilePaths, args.GenomicFeatures, args.OutputDir, args.AnnotationSettings, progStart, progUpdate, progDone)
    End Sub

    Private Sub BackgroundWorkerAnnotationAnalysis_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerAnnotationAnalysis.RunWorkerCompleted
        btnRun.Enabled = True
        btnPValue.Enabled = True
    End Sub

    'rearanges the FOI by chromosome
    Private Function OrginizeFeaturesByChrom(ByVal feature As List(Of Feature)) As List(Of Feature)
        'gets one of each chromosome that contain FOIs
        Dim UniqueChroms As New HashSet(Of String)
        For Each f In feature
            If Not UniqueChroms.Contains(f.Chrom) Then
                UniqueChroms.Add(f.Chrom)
            End If
        Next
        'Temporarily holds the Features as they are rearanged.   
        Dim tempChrom As New List(Of String)
        Dim tempChromStart As New List(Of Integer)
        Dim tempChromEnd As New List(Of Integer)
        Dim tempName As New List(Of String)
        For Each uniqueChrom In UniqueChroms
            For j As Integer = 0 To Features.Count - 1
                If feature(j).Chrom = uniqueChrom Then
                    featuresIndexes.Add(j) 'stores the index position of the FOI so that it can be reordered after analysis
                    tempChrom.Add(feature(j).Chrom)
                    tempChromStart.Add(feature(j).ChromStart)
                    tempChromEnd.Add(feature(j).ChromEnd)
                    If feature(j).Name IsNot Nothing Then : tempName.Add(feature(j).Name) : End If 'adds the feature name if it exists
                End If
            Next
        Next
        'replaces the FOI in feature with the FOI orginized by chrom
        For i As Integer = 0 To feature.Count - 1
            feature(i).Chrom = tempChrom(i)
            feature(i).ChromStart = tempChromStart(i)
            feature(i).ChromEnd = tempChromEnd(i)
            If tempName.Count <> 0 Then : feature(i).Name = tempName(i) : End If
        Next
        Return feature
    End Function

    'calcualtes the pvalue.  can do this using montecarlo or the analytic method 
    Private Sub btnPValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPValue.Click
        Dim GenomicFeaturesToAnalyze As New List(Of GenomicFeature)
        Dim FeatureFilePaths As New List(Of String) 'stores the filepaths of the files containing the FOIs to do an enrichment analysis on
        Dim Analyzer As New EnrichmentAnalysis(ConnectionString, progStart, progUpdate, progDone)
        AnalysisType = "Enrichment"
        If listFeatureFiles.Items.Count <> 0 Then
            btnRun.Enabled = False
            btnPValue.Enabled = False
            'Goes through each of the listview items in the list of features to run and adds each of the genomicfeatures that are attache to them to a list of GF to analyze
            For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
                GenomicFeaturesToAnalyze.Add(lvGF.GenomicFeature)
            Next
            Me.ProgressBar1.Maximum = GenomicFeaturesToAnalyze.Count
            'goes through each Feature Of Interest file that is loaded by the user and adds their paths to a list that is passed on to the Enrichment Analyzer
            For Each FeatureFile As ListItemFile In listFeatureFiles.Items
                FeatureFilePaths.Add(FeatureFile.filPath)
            Next

            Dim Settings As EnrichmentSettings = GetUserSettings()                                                   'gets the settigns set by the user in the user isnterface and adds them to a encrichmentsettings class which is pased on to the enrichment analyzer
            Dim args As EnrichmentArgument = New EnrichmentArgument(GenomicFeaturesToAnalyze, Settings, FeatureFilePaths, Background)
            BackgroundWorkerEnrichmentAnalysis.RunWorkerAsync(args)
            'Analyzer.RunEnrichmentAnlysis(FeatureFilePaths, GenomicFeaturesToAnalyze, Background, Settings)
            ' MessageBox.Show("Enrichment analysis results saved in: " & vbCrLf & Settings.OutputDir)
        Else
            MessageBox.Show("Please add features of interest to run")
        End If
    End Sub

    'runs the enrichment analysis on a seperate thread
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerEnrichmentAnalysis.DoWork
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        'Get the argument
        Dim args As EnrichmentArgument = e.Argument
        'starts the enrichment analysis
        Dim Analyzer As New EnrichmentAnalysis(args.Settings.ConnectionString, progStart, progUpdate, progDone)
        Analyzer.RunEnrichmentAnlysis(args.FeatureFilePaths, args.GenomicFeatures, args.Background, args.Settings)
    End Sub

    Private Sub BackgroundWorkerEnrichmentAnalysis_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerEnrichmentAnalysis.RunWorkerCompleted
        btnRun.Enabled = True
        btnPValue.Enabled = True
    End Sub

    'gets the settings from the user interface and adds them to a EnrichmentSettings class which is passed on to the enrichment analyzer
    Private Function GetUserSettings() As EnrichmentSettings
        Dim JobName As String = " " & txtJobName.Text
        Dim UseMonteCarlo As Boolean, UseAnalytical As Boolean, UseChiSquare As Boolean, UseBinomialDistrobution As Boolean
        Dim OutputPearsonsCoefficientWeightedMatrix As Boolean
        Dim OutputPercentOverlapWeightedMatrix As Boolean
        If rbChiSquareTest.Checked = True Then
            UseChiSquare = True
        ElseIf rbBinomialDistrobution.Checked = True Then
            UseBinomialDistrobution = True
        End If
        If rbUseMonteCarlo.Checked = True Then
            UseMonteCarlo = True
        ElseIf rbUseAnalytical.Checked = True Then
            UseAnalytical = True
        End If
        'this is where settings for what type of matrix is to be generated are set
        Select Case cmbMatrixWeighting.SelectedIndex
            Case Is = 0
                OutputPearsonsCoefficientWeightedMatrix = False
                OutputPercentOverlapWeightedMatrix = False
            Case Is = 1
                OutputPercentOverlapWeightedMatrix = True
                OutputPearsonsCoefficientWeightedMatrix = False
            Case Is = 2
                OutputPercentOverlapWeightedMatrix = False
                OutputPearsonsCoefficientWeightedMatrix = True
        End Select
        Dim featureFile As ListItemFile = listFeatureFiles.Items(0)
        Dim PeasonsAudjustmentConst As Integer = txtPearsonAudjustmentConstant.Value
        Dim PValueOutputDir As String = featureFile.fileDir & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & JobName & "\"  'sets what directory the results are to outputed to
        Dim Settings As New EnrichmentSettings(ConnectionString, txtJobName.Text, PValueOutputDir, UseMonteCarlo, UseAnalytical, UseChiSquare, UseBinomialDistrobution, OutputPercentOverlapWeightedMatrix, rbSquared.Checked, OutputPearsonsCoefficientWeightedMatrix, PeasonsAudjustmentConst, BackgroundName, UseSpotBackground, txtNumMCtoRun.Text, txtPvalueThreshold.Text, cmbFilterLevels.Text, PromoterUpstream, PromoterDownstream, txtproximity.Value)
        Return Settings
    End Function


    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFilterLevels.SelectedIndexChanged
        SetFilterValues()
    End Sub

    'sets the filter values of the Genomic Features (such as threshold, number of base pairs for promoter regions etc. )
    Private Sub SetFilterValues()
        If cmbFilterLevels.Text = "Minimum" Then
            For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
                lvGF.GenomicFeature.Threshold = lvGF.GenomicFeature.ThresholdMin
            Next
            PromoterUpstream = 2000
            PromoterDownstream = 0
        ElseIf cmbFilterLevels.Text = "Mean" Then
            For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
                lvGF.GenomicFeature.Threshold = lvGF.GenomicFeature.ThresholdMean
            Next
            PromoterUpstream = 5000
            PromoterDownstream = 0
        ElseIf cmbFilterLevels.Text = "Middle" Then
            For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
                lvGF.GenomicFeature.Threshold = lvGF.GenomicFeature.ThresholdMin + (lvGF.GenomicFeature.ThresholdMax - lvGF.GenomicFeature.ThresholdMin) / 2
            Next
        ElseIf cmbFilterLevels.Text = "Custom" Then

        End If
    End Sub

    Private Sub frmGenomeRunner_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If currDir = vbNullString Then Exit Try
            SaveSetting("GenomeRunner", "Startup", "Folder", currDir)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEnrichmentForAllNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnrichmentForAllNames.Click
        lblProgress.Text = "Generating Genomic Features for all names" : Application.DoEvents()
        Dim GenomicFeaturesToAnalyze As New List(Of GenomicFeature)
        For Each listGenomicFeature As ListItemGenomicFeature In listFeaturesToRun.Items
            GenomicFeaturesToAnalyze.Add(listGenomicFeature.GenomicFeature)
        Next
        GenomicFeaturesToAnalyze = GREngine.GenerateGenomicFeaturesByName(GenomicFeaturesToAnalyze, ConnectionString)
        listFeaturesToRun.Items.Clear() 'clears the list of features to run.  
        'The new features sorted by name are added
        For Each GF In GenomicFeaturesToAnalyze
            Dim nListGenomicFeature As New ListItemGenomicFeature(GF)
            nListGenomicFeature.Text = GF.Name
            listFeaturesToRun.Items.Add(nListGenomicFeature)
        Next
        lblProgress.Text = "Done" : Application.DoEvents()
    End Sub

    Private Sub listFeatureFiles_DragEnter_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles listFeatureFiles.DragEnter
        If (e.Data.GetDataPresent(DataFormats.FileDrop) = True) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub listFeatureFiles_DragDrop_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles listFeatureFiles.DragDrop
        If (e.Data.GetDataPresent(DataFormats.FileDrop) = True) Then
            For Each oneFile As String In _
              e.Data.GetData(DataFormats.FileDrop)
                Dim FileName_Ext() As String = Strings.Split(oneFile, "\")
                Dim nFile As New ListItemFile
                nFile.filPath = oneFile
                nFile.Text = FileName_Ext(FileName_Ext.Length - 1)
                nFile.fileDir = oneFile.Replace(nFile.Text, "")
                Dim FileName() As String = Strings.Split(FileName_Ext(FileName_Ext.Length - 1), ".")
                nFile.fileName = FileName(0)
                listFeatureFiles.Items.Add(nFile)
            Next oneFile
        End If
    End Sub

    Private Sub btnRemoveFOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFOI.Click
        'adds all of the selected features to a list of indexes to be removed
        Dim indexOffeaturesToRemove As New List(Of Integer)
        For Each index In listFeatureFiles.SelectedIndices
            indexOffeaturesToRemove.Add(index)
        Next
        'goes through the list backwords and removes the features. 
        For i As Integer = indexOffeaturesToRemove.Count - 1 To 0 Step -1
            listFeatureFiles.Items.RemoveAt(indexOffeaturesToRemove(i))
        Next
    End Sub

    'loads custom genomic feature tracks from the tab deliminated file that the user inputs.    
    Private Sub btnLoadCustomGenomicFeatures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadCustomGenomicFeatures.Click
        Dim DidWork As Integer
        OpenFD.Title = "Select a tab-delimited file with FEATURE chromosome and start position"
        OpenFD.InitialDirectory = GetSetting("GenomeRunner", "Startup", "Folder", "C:\")
        OpenFD.FileName = ""
        OpenFD.Multiselect = True
        OpenFD.Filter = "All files|*.*|BED Files|*.bed|Text Files|*.txt"
        DidWork = OpenFD.ShowDialog()
        If DidWork = DialogResult.Cancel Then
            Exit Sub
        Else
            FileName = OpenFD.SafeFileName
        End If
        Dim FeatureFiles As String() = OpenFD.FileNames

        For Each File In FeatureFiles
            Dim nGFName As String = Path.GetFileName(File)
            'adds a new genomic feature to the list of GF to run 
            Dim nGF As New GenomicFeature(-1, nGFName, Path.GetFullPath(File), "Custom", "NA", 0, 0, 0, 0, "Custom", 0, Nothing, "", 1)
            'the feature is added to the listview of GF to run as a visual indicator to the user that the feature is going to run
            Dim ListViewGF As New ListItemGenomicFeature(nGF) With {.Text = nGFName}
            listFeaturesToRun.Items.Add(ListViewGF)
            btnRemoveAllFeaturesToRun.Enabled = False 'button to remove features is dissabled to keep the ListView of GF to run consistant with the features list of GF to analyze
        Next
    End Sub


    Private Sub btnCustomizeFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustomizeFilters.Click
        Dim GRFeaturesToAnalyze As New List(Of GenomicFeature)
        'puts all of the GenomicFeatures into a list 
        For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
            GRFeaturesToAnalyze.Add(lvGF.GenomicFeature)
        Next
        'passes on this list of GF to a form that edits the values of the filters
        Dim form As New frmCustomizeFilters(GRFeaturesToAnalyze, PromoterUpstream, PromoterDownstream)
        form.ShowDialog(Me)
        PromoterDownstream = form.PromoterDownstream
        PromoterUpstream = form.PromoterUpstream
        cmbFilterLevels.Text = "Custom"
        'sets the Gf of the listitems to the new changed GF
        Dim CurrGF As Integer = 0
        For Each lvGF As ListItemGenomicFeature In listFeaturesToRun.Items
            lvGF.GenomicFeature = GRFeaturesToAnalyze(CurrGF)
            CurrGF += 1
        Next
    End Sub

    Private Sub btnClearFeaturesToRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveAllFeaturesToRun.Click
        listFeaturesToRun.Items.Clear()
    End Sub

    Private Sub btnRemoveFeaturesToRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFeaturesToRun.Click
        'removes the selected genomic features from the list of features to run
        For j As Integer = listFeaturesToRun.SelectedIndices.Count - 1 To 0 Step -1
            listFeaturesToRun.Items(listFeaturesToRun.SelectedIndices(j)).Remove()
        Next
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub rbUseMonteCarlo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUseMonteCarlo.CheckedChanged
        If rbUseMonteCarlo.Checked = True Then
            txtNumMCtoRun.Enabled = True
        Else
            txtNumMCtoRun.Enabled = False
        End If
        If rbUseMonteCarlo.Checked = True And rbBinomialDistrobution.Checked = True Then
            MessageBox.Show("Binomial distribution not available for Monte Carlo simulation")
            rbChiSquareTest.Checked = True
        End If
    End Sub

    Private Sub cmbStrandsToAnalyze_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStrandsToAnalyze.SelectedIndexChanged
        Dim GFeaturesToAnalyze As New List(Of GenomicFeature)

        'gets the genomic feature classes attached to the genomic feature
        For Each listItemGF As ListItemGenomicFeature In listFeaturesToRun.Items
            GFeaturesToAnalyze.Add(listItemGF.GenomicFeature)
        Next
        'generates Genomic Features to run that for the Genomic Features that have the selected strand data
        GFeaturesToAnalyze = GREngine.GenerateGenomicFeaturesByStrand(GFeaturesToAnalyze, cmbStrandsToAnalyze.SelectedItem, ConnectionString)
        'adds the new genomic features to the list of genomic features to run that the user can see
        listFeaturesToRun.Items.Clear()
        For Each GF In GFeaturesToAnalyze
            Dim nListItemGF As New ListItemGenomicFeature(GF)
            nListItemGF.Text = GF.Name
            listFeaturesToRun.Items.Add(nListItemGF)
        Next
    End Sub

    Private Sub mnuGenerateListOfRandomCoordinates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGenerateListOfRandomCoordinates.Click
        frmGenerateRandomFeatures.ShowDialog()
    End Sub

    Private Sub cmbMatrixWeighting_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMatrixWeighting.SelectedIndexChanged
        If cmbMatrixWeighting.SelectedIndex = 0 Then
            GroupBoxPearsons.Visible = False
            GroupBoxPercentAudjustment.Visible = False
        ElseIf cmbMatrixWeighting.SelectedIndex = 1 Then
            GroupBoxPearsons.Visible = False
            GroupBoxPercentAudjustment.Visible = True
        ElseIf cmbMatrixWeighting.SelectedIndex = 2 Then
            GroupBoxPearsons.Visible = True
            GroupBoxPercentAudjustment.Visible = False
        End If
        If cmbMatrixWeighting.SelectedIndex = 2 And rbBinomialDistrobution.Checked = True Then
            MessageBox.Show("Pearson's Contingency Coefficient not available for the Chi Square Test")
            cmbMatrixWeighting.SelectedIndex = 0
        End If
    End Sub

    Private Sub rbBinomialDistrobution_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBinomialDistrobution.CheckedChanged
        If cmbMatrixWeighting.SelectedIndex = 2 And rbBinomialDistrobution.Checked = True Then
            MessageBox.Show("Pearson's Contingency Coefficient not available for the Chi Square Test")
            cmbMatrixWeighting.SelectedIndex = 0
        End If
        If rbUseMonteCarlo.Checked = True And rbBinomialDistrobution.Checked = True Then
            MessageBox.Show("Binomial distribution not available for Monte Carlo simulation")
            rbChiSquareTest.Checked = True
        End If
    End Sub

    Private Sub SetDatabaseConnectionSettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetDatabaseConnectionSettingsToolStripMenuItem.Click
        frmLogin.ShowDialog()
        'OpenDatabase()
    End Sub

    Private Sub CreateLocalGenomeRunnerTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateLocalGenomeRunnerTableToolStripMenuItem.Click
        Dim form As New frmCreateGenomeRunnerDatabase(ConnectionString)
        form.ShowDialog()
    End Sub

    ' The delegate needed to work between the UI thread and the background worker
    Delegate Sub SetProgressValue_Delegate(ByVal [ProgressBar] As ProgressBar, ByVal [Value] As Integer)
    Delegate Sub SetProgressLabel_Delegate(ByVal [Label] As Label, ByVal [Text] As String)

    ' The delegates subroutine which is thread safe and can be used by the background worker to update the UI
    Private Sub SetProgress_ThreadSafe(ByVal [ProgressBar] As ProgressBar, ByVal [Value] As Integer)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [ProgressBar].InvokeRequired Then
            Dim DelegateSetProgress As New SetProgressValue_Delegate(AddressOf SetProgress_ThreadSafe)
            Me.Invoke(DelegateSetProgress, New Object() {[ProgressBar], [Value]})
        Else
            ProgressBar1.Value = [Value]
        End If
    End Sub

    Private Sub SetProgressLabel_ThreadSafe(ByVal [Label] As Label, ByVal [Text] As String)
        If [Label].InvokeRequired Then
            Dim DelegateSetProgressLabel As New SetProgressLabel_Delegate(AddressOf SetProgressLabel_ThreadSafe)
            Me.Invoke(DelegateSetProgressLabel, New Object() {Label, [Text]})
        Else
            lblProgress.Text = [Text]
        End If
    End Sub


    Private Sub mnuProgramInterface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuProgramInterface.Click
        System.Diagnostics.Process.Start("http://wren.omrf.org/GenomeRunner/GenomeRunner_Supplemental.htm")
    End Sub

    Private Sub mnuGenomeFeatures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGenomeFeatures.Click
        System.Diagnostics.Process.Start("http://genome.ucsc.edu/cgi-bin/hgTracks?clade=mammal&org=Human&db=hg18&position=chr16%3A70%2C289%2C971-70%2C290%2C104&hgt.suggest=&hgt.suggestTrack=knownGene&pix=800&hgsid=207715539&hgTracksConfigPage=configure+tracks+and+display&hgt.newJQuery=1")
    End Sub

    Private Sub UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem.Click
        Background = GREngine.GetGenomeBackgroundHG18()
        lblBackground.Text = "Using NCBI36/hg18 genome assembly as genomic background"
        rbUseMonteCarlo.Enabled = True
        UseSpotBackground = False
        rbUseAnalytical.Enabled = True
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub
End Class

'these settings are passed onto the background worker as arguments
Public Class EnrichmentArgument
    Public GenomicFeatures As List(Of GenomicFeature)
    Public Settings As EnrichmentSettings
    Public FeatureFilePaths As List(Of String)
    Public Background As List(Of Feature)
    Public Sub New(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal Settings As EnrichmentSettings, ByVal FeatureFilePaths As List(Of String), ByVal Background As List(Of Feature))
        Me.GenomicFeatures = GenomicFeatures
        Me.Settings = Settings
        Me.FeatureFilePaths = FeatureFilePaths
        Me.Background = Background
    End Sub
End Class

Public Class AnnotationArguments
    Public GenomicFeatures As List(Of GenomicFeature)
    Public FeatureFilePaths As List(Of String)
    Public OutputDir As String
    Public ConnectionString As String
    Public AnnotationSettings As AnnotationSettings
    Public Sub New(ByVal ConnectionString As String, ByVal GenomicFeatures As List(Of GenomicFeature), ByVal FeatureFilePaths As List(Of String), ByVal OutputDir As String, ByVal AnnotationSettings As AnnotationSettings)
        Me.GenomicFeatures = GenomicFeatures
        Me.FeatureFilePaths = FeatureFilePaths
        Me.OutputDir = OutputDir
        Me.ConnectionString = ConnectionString
        Me.AnnotationSettings = AnnotationSettings
    End Sub
End Class