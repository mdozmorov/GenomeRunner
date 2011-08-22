'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports GenomeRunnerConsole.GenomeRunner
Imports System.IO
Imports MySql.Data.MySqlClient

Module Module1
    Dim cn As MySqlConnection, cmd As MySqlCommand, dr As MySqlDataReader
    Dim GREngin As New GenomeRunnerConsole.GenomeRunner.GenomeRunnerEngine
    Dim progStart As ProgressStart, progUpdate As ProgressUpdate, progDone As ProgressDone
    Dim AnalysisType As String = ""
    Structure Parameters
        Dim Name As String
        Dim arguments As List(Of String)
    End Structure

    Sub Main(ByVal Argument As String())
        'Flags used : (path of features of interest file) (path of genomic features ids file)  a | [e [-m [mc [10] | an] [-p [ct | bd]] [-a [pc | po]] ]
        Dim args As String() = ConvertArrayToLowerCase(Argument)
        Dim Parameters As List(Of Parameters)
        Dim Settings As EnrichmentSettings
        Dim flags As New List(Of String)
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        Console.WriteLine("Welcome to GenomeRunner")
        PrintHelp()
        If args.Length > 2 Then
            If args(2) = "e" Then
                AnalysisType = "Enrichment"                                                                                 'Used to determine what to write for progress updates
                Dim GenomicFeatureIDsToRun As New List(Of Integer)
                Dim featureOfInterestPath As New List(Of String)
                Try
                    Using sr As New StreamReader(args(1))
                        While sr.EndOfStream = False
                            GenomicFeatureIDsToRun.Add(sr.ReadLine())
                        End While
                    End Using
                Catch
                    Console.WriteLine("Please ensure that the file at '" & args(1) & "' only integers and valid genomic feature ids")
                End Try
                featureOfInterestPath.Add(args(0))
                Parameters = GetParameters(args)                                                                                 'get the parameters inputed by the command line and orginizes them into parameters
                Dim OutputDir As String = Path.GetDirectoryName(args(0)) & "\" & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & "\"  'sets what directory the results are to outputed to
                Settings = GetEnrichmentSettings(OutputDir, Parameters)                         'generates an enrichmentsettings classed based on the paramaters inputed by the user. 
                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun)
                Dim Analyzer As New EnrichmentAnalysis(Settings.ConnectionString, progStart, progUpdate, progDone)
                Dim Background As List(Of Feature) = GREngin.GetGenomeBackgroundHG18()
                Analyzer.RunEnrichmentAnlysis(featureOfInterestPath, GenomicFeaturesToRun, Background, Settings)

            ElseIf args(2) = "a" Then
                Dim FeaturesOfInterest As New List(Of String)
                FeaturesOfInterest.Add(args(0))
                Dim GenomicFeatureIDsToRun As New List(Of Integer)
                AnalysisType = "Annotation"                                                                                 'Used to determine what to write for progress updates
                Try
                    Using sr As New StreamReader(args(1))
                        While sr.EndOfStream = False
                            GenomicFeatureIDsToRun.Add(sr.ReadLine())
                        End While
                    End Using
                Catch
                    Console.WriteLine("Please ensure that the file at '" & args(1) & "' only integers and valid genomic feature ids")
                End Try
                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun)
                Dim ConnectionString As String = GetConnectionString()
                Dim analyzer As New AnnotationAnalysis(ConnectionString)
                Dim OutputDir As String = Path.GetDirectoryName(FeaturesOfInterest(0)) & "\"
                Dim AnoSettings As New AnnotationSettings(5000, 1000, 0)
                analyzer.RunAnnotationAnalysis(FeaturesOfInterest, GenomicFeaturesToRun, OutputDir, AnoSettings, progStart, progUpdate, progDone)
            End If
        End If
    End Sub

    Private Sub PrintHelp()
        'Flags used : (path of features of interest file) (path of genomic features ids file)  a | [e [-m [mc [10] | an] [-p [ct | bd]] [-a [pc | po]] ]
        Console.WriteLine("General argument structure is as follows: [Full path to features of interest file] [Full path to genomic feastures IDs file] [type of analysis ('e'nrichment|'a'nnotation)] [parameters: (-flag argument) (examples. -m mc)]")
        Console.WriteLine("")
        Console.WriteLine("=Annotation=")
        Console.WriteLine("")
        Console.WriteLine("Does not have any parameters, only 'a' to specify annotation analysis")
        Console.WriteLine("")
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("")
        Console.WriteLine("Example: 'C:\FeatureOfInterest.bed' 'C:\GenomicFeatureIDsFile' a")
        Console.WriteLine("")
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("=Enrichment=")
        Console.WriteLine("flag: -m : method to use to calculate expected random feature association")
        Console.WriteLine("Arguments: 'mc[#ofruns]' for monte carlo | 'an' analytical")
        Console.WriteLine("       -p : method to use to calculate p-value")
        Console.WriteLine("           'ct' for chi-square test | 'bd' for binomial distribution (does not work for monte-carlo)")
        Console.WriteLine("       -a : what type of audjustment should be done on the matrix p-values")
        Console.WriteLine("           'pc' to weight by Pearson's coefficient | 'po' to weight by percent of features of interest that overlap with genomic features")
        Console.WriteLine("")
        Console.WriteLine("If no parameters given, default are used (e -m mc10 -p ct)")
        'Console.WriteLine("Example: 'C:\FeatureOfInterest.bed' 'C:\GenomicFeatureIDsFile' e -m mc10 -p ct -a po")
        'Console.WriteLine("OR: ... e -m mc10 -p chisquare -a(no weighting is used)")
        'Console.WriteLine("")
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("Example: 'C:\FeatureOfInterest.bed' 'C:\GenomicFeatureIDsFile' e -m mc10 -p ct -a po")
        Console.WriteLine("")
        Console.WriteLine("*This will do an enrichment analysis on the features of interest contained in the first file against the genomic features identified by ID's in the GenomeRunnerTable in the MySQL database")
        Console.WriteLine("*This run will use the Monte-Carlo simulation with 10 runs to calculate the number of expected associations by random chance.")
        'Console.WriteLine("")
        Console.WriteLine("*The p-value will be calculated using the Chi-Square test.")
        'Console.WriteLine("")
        Console.WriteLine("*The matrix will be weighted by the percent overlap")
        'Console.WriteLine("")
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("")
       
    End Sub

    '####used to update progress of the analysis
    Private Sub HandleProgressStart(ByVal progressMaximum As Integer)

    End Sub

    Private Sub HandleProgressUpdate(ByVal currProgress As Integer, ByVal FeatureFileName As String, ByVal GenomicFeatureName As String)
        ' SetProgress_ThreadSafe(Me.ProgressBar1, currProgress)
        Console.WriteLine("Doing " & AnalysisType & " analysis for " & FeatureFileName & ": " & GenomicFeatureName)
        ' SetProgressLabel_ThreadSafe(lblProgress, "Doing enrichment analysis for " & FeatureFileName & ": " & GenomicFeatureName)
    End Sub

    Private Sub HandleProgressDone(ByVal OuputDir As String)
        ' SetProgress_ThreadSafe(Me.ProgressBar1, 0)
        Console.WriteLine(AnalysisType & " analysis complete.  Results outputed to: " & OuputDir)
        Console.WriteLine("Press enter to quit...")
        Console.ReadLine()
    End Sub
    '#

    Function GetParameters(ByVal Args As String()) As List(Of Parameters)
        Dim Parameters As New List(Of Parameters)
        Dim IsNewParameter As Boolean = False                                                                              'The first parameters are the paths of the files, these are skipped 
        Dim nParameter As New Parameters
        For Each arg In Args
            If arg(0) = "-" Then
                If IsNewParameter = True Then
                    Parameters.Add(nParameter)
                End If
                nParameter = New Parameters
                nParameter.arguments = New List(Of String)
                nParameter.Name = arg
                IsNewParameter = True
            ElseIf IsNewParameter = True And arg(0) <> "-" Then
                nParameter.arguments.Add(arg)
            End If
        Next
        Parameters.Add(nParameter)
        Return Parameters
    End Function


    Function GetGenomicFeaturesFromIDsInputed(ByVal listIDs As List(Of Integer)) As List(Of GenomicFeature)
        Dim GenomicFeaturesToRemove As New List(Of Integer)
        Dim ConnectionString As String = GetConnectionString()
        Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GREngin.GetGenomicFeaturesAvailable(ConnectionString)                                  'gets all of the genomic features available from the GenomeRunner Table
        'adds all of the genomic features ids in the GenomicFeaturesToRun list that are not included in the 
        'list genomic features to run for removal
        For i As Integer = 0 To GenomicFeaturesToRun.Count - 1
            If listIDs.IndexOf(GenomicFeaturesToRun(i).id) = -1 Then
                GenomicFeaturesToRemove.Add((GenomicFeaturesToRun(i).id))
            End If
        Next
        'removes the genomic features that are not selected by the user to be run.
        For i As Integer = GenomicFeaturesToRemove.Count - 1 To 0 Step -1
            For j As Integer = GenomicFeaturesToRun.Count - 1 To 0 Step -1
                If GenomicFeaturesToRun(j).id = GenomicFeaturesToRemove(i) Then
                    GenomicFeaturesToRun.RemoveAt(j)
                End If
            Next
        Next
        'adds promoter and exon to the list of genomic features to analyze for genomic features that are of type gene
        Dim GenomicFeaturesToRunWithEP As New List(Of GenomicFeature)
        For Each GF In GenomicFeaturesToRun
            If GF.QueryType = "Gene" Then
                GenomicFeaturesToRunWithEP.Add(GF)
                Dim nGFPromoter As New GenomicFeature(GF.id, GF.Name & "Promoter", GF.TableName, "Promoter", "NA", 0, "2000", "10000", "5000", "", 0, Nothing, "", 1)
                GenomicFeaturesToRunWithEP.Add(nGFPromoter)
                Dim nGFExon As New GenomicFeature(GF.id, GF.Name & "Exon", GF.TableName & "Exons", "Exon", "NA", 0, 0, 0, 0, "", 0, Nothing, "", 1)
                GenomicFeaturesToRunWithEP.Add(nGFExon)
            Else
                GenomicFeaturesToRunWithEP.Add(GF)
            End If
        Next
        Return GenomicFeaturesToRunWithEP
    End Function

    'converts the inputed array to lowercase values and returns the convereted array
    Function ConvertArrayToLowerCase(ByVal arrayToConvert As String()) As String()
        Dim ConvertedArray(arrayToConvert.Length - 1) As String
        For i As Integer = 0 To arrayToConvert.Length - 1
            ConvertedArray(i) = arrayToConvert(i).ToLower()
        Next
        Return ConvertedArray
    End Function

    Function GetEnrichmentSettings(ByVal outputDirectory As String, ByVal Parameters As List(Of Parameters)) As EnrichmentSettings
        Dim Settings As EnrichmentSettings
        Dim UseMonteCarlo As Boolean = True, UseAnalytical As Boolean = False, NumOfMCToRun As Integer = 10, _
        UseChiSquareTest As Boolean = True, UseBinomialDistribution As Boolean = False, _
PvalueThreshold As Double = 0.01, OutputPearsonsCoefficientWeightedMatrix As Boolean = False, OutputPercentOverlapWeightedMatrix As Boolean = False

        'sets which method should be used to calculate the number of random associations
        For Each param In Parameters
            Select Case param.Name
                '...sets which method should be used to calculate the number of random associations
                Case Is = "-m"
                    For Each arg In param.arguments
                        If arg = "an" Then
                            UseAnalytical = True
                            UseChiSquareTest = False
                        End If
                        If arg.Substring(0, 2) = "mc" Then
                            UseMonteCarlo = True
                            UseAnalytical = False
                            Dim numMonteCarloArg As Integer = arg.Remove(0, 2)
                            NumOfMCToRun = numMonteCarloArg
                        End If
                    Next
                    '...sets the method to use for the calculation of the p-value
                Case Is = "-p"
                    For Each arg In param.arguments
                        If arg = "ct" Then
                            UseChiSquareTest = True
                            If IsNumeric(param.arguments(param.arguments.IndexOf(arg) + 1)) Then
                                NumOfMCToRun = param.arguments(param.arguments.IndexOf(arg) + 1)
                            End If
                        End If
                        If arg = "bd" Then
                            UseBinomialDistribution = True
                            UseChiSquareTest = False
                        End If
                    Next
                Case Is = "-a"
                    For Each arg In param.arguments
                        If arg = "pc" Then
                            OutputPearsonsCoefficientWeightedMatrix = True
                        End If
                        If arg = "po" Then
                            OutputPercentOverlapWeightedMatrix = True
                        End If
                    Next
            End Select
        Next
        Settings = New EnrichmentSettings(GetConnectionString(), "", outputDirectory, UseMonteCarlo, UseAnalytical, UseChiSquareTest, UseBinomialDistribution, OutputPercentOverlapWeightedMatrix, True, OutputPearsonsCoefficientWeightedMatrix, 0, "hg18", False, NumOfMCToRun, PvalueThreshold, "none", 2000, 0, 0)
        Return Settings
    End Function


    Function GetConnectionString() As String
        Dim uName As String
        Dim uPassword As String
        Dim uServer As String
        Dim uDatabase As String
        Dim ConnectionString As String
ConnectionSettingsRetry:
        Try
            'gets the connection settings from the registry and builds a connection string
            uName = GetSetting("GenomeRunner", "Database", "uName")
            uPassword = GetSetting("GenomeRunner", "Database", "uPassword")
            uServer = GetSetting("GenomeRunner", "Database", "uServer")
            uDatabase = GetSetting("GenomeRunner", "Database", "uDatabase")
            ConnectionString = "Server=" & uServer & ";Database=" & uDatabase & ";User ID=" & uName & ";password=" & uPassword
            cn = New MySqlConnection(ConnectionString)
            cn.Open()
            cmd = New MySqlCommand("SELECT * from GenomeRunner limit 1", cn)
            dr = cmd.ExecuteReader()
            dr.Close() : cn.Close()
        Catch
            '...if the connection fails
            Console.WriteLine("Database connection settings were not valid.  Please re-enter the database settings:")
            Console.WriteLine("Enter the user name:")
            uName = Console.ReadLine()
            SaveSetting("GenomeRunner", "Database", "uName", uName)
            Console.WriteLine("Enter the password:")
            uPassword = Console.ReadLine()
            SaveSetting("GenomeRunner", "Database", "uPassword", uPassword)
            Console.WriteLine("Enter the database:")
            uDatabase = Console.ReadLine()
            SaveSetting("GenomeRunner", "Database", "uDatabase", uDatabase)
            Console.WriteLine("Enter the server ('localhost' for local databases)")
            uServer = Console.ReadLine()
            SaveSetting("GenomeRunner", "Database", "uServer", uServer)
            GoTo ConnectionSettingsRetry
        End Try
        Return ConnectionString
    End Function

End Module
