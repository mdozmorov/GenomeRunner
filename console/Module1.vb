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
    Structure ConnectionSettings
        Dim host As String
        Dim user As String
        Dim password As String
        Dim database As String
    End Structure
    Dim connectSettings As New ConnectionSettings

    Sub Main(ByVal Argument As String())
        'Flags used : (path of features of interest file) (path of genomic features ids file)  a | [e [-m [mc [10] | an] [-p [ct | bd]] [-a [pc | po]] ]
        'TODO why is lower casing everything necessary? Should be input correctly do begin with. Also this is messing up db password input.
        'Dim args As String() = ConvertArrayToLowerCase(Argument)
        Dim args As String() = Argument
        Dim params As List(Of Parameters)
        Dim Settings As EnrichmentSettings
        Dim flags As New List(Of String)
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        Console.WriteLine("Welcome to GenomeRunner")
        PrintHelp()
        'TODO raise error if args(2) is out of range!
        params = GetParametersFromConfigFile(args(2))
        connectSettings = GetConnectionSettings(params)
        Dim ConnectionString As String = GetConnectionString()
        If args.Length >= 4 Then

            'Read genomic features file from args(1) into GenomicFeatureIDsToRun
            Dim GenomicFeatureIDsToRun As List(Of Integer) = GetIdsFromFile(args(1))

            'strand is needed for enrichment & annotation
            Dim strand As String = "both"
            If params.Any(Function(p) p.Name = "-st") Then
                strand = params.Find(Function(p) p.Name = "-st").arguments.First
            End If

            If args(3) = "-e" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ENRICHMENT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                AnalysisType = "Enrichment" 'Used to determine what to write for progress updates

                Dim featureOfInterestPath As New List(Of String)

                featureOfInterestPath.Add(args(0))
                'Parameters = GetParameters(args) 'get the parameters inputed by the command line and orginizes them into parameters
                Dim OutputDir As String = Path.GetDirectoryName(args(0)) & "\" & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & "\" 'sets what directory the results are to outputed to
                Settings = GetEnrichmentSettings(OutputDir, params) 'generates an enrichmentsettings classed based on the paramaters inputed by the user. 
                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, strand)
                Dim Analyzer As New EnrichmentAnalysis(Settings.ConnectionString, progStart, progUpdate, progDone)
                Dim Background As List(Of Feature) = GREngin.GetGenomeBackground(ConnectionString)
                Dim allAdjustments As Boolean = False
                If params.Any(Function(p) p.Name = "-all") Then allAdjustments = True
                Analyzer.RunEnrichmentAnlysis(featureOfInterestPath, GenomicFeaturesToRun, Background, Settings)

            ElseIf args(3) = "-a" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ANNOTATION
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim FeaturesOfInterest As New List(Of String)
                FeaturesOfInterest.Add(args(0))
                AnalysisType = "Annotation"                                                                                 'Used to determine what to write for progress updates
                
                'TODO need strand arg (currently hard coded below as "both"), but comes from config file. Will Annotation use config 
                '     file or just params?
                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, strand)
                'TODO get this above the loop now
                'Dim ConnectionString As String = GetConnectionString()
                Dim analyzer As New AnnotationAnalysis(ConnectionString)
                Dim OutputDir As String = Path.GetDirectoryName(FeaturesOfInterest(0)) & "\"
                Dim AnoSettings As New AnnotationSettings(5000, 1000, 0)
                Dim shortOnly As Boolean = False
                If params.Any(Function(p) p.Name = "-short") Then shortOnly = True
                analyzer.RunAnnotationAnalysis(FeaturesOfInterest, GenomicFeaturesToRun, OutputDir, AnoSettings, progStart, progUpdate, progDone, shortOnly)
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
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("Example: 'C:\FeatureOfInterest.bed' 'C:\GenomicFeatureIDsFile' e -m mc10 -p ct -a po")
        Console.WriteLine("")
        Console.WriteLine("*This will do an enrichment analysis on the features of interest contained in the first file against the genomic features identified by ID's in the GenomeRunnerTable in the MySQL database")
        Console.WriteLine("*This run will use the Monte-Carlo simulation with 10 runs to calculate the number of expected associations by random chance.")
        Console.WriteLine("*The p-value will be calculated using the Chi-Square test.")
        Console.WriteLine("*The matrix will be weighted by the percent overlap")
        Console.WriteLine("--------------------------------------------------------------------------------")
        Console.WriteLine("")
       
    End Sub

    Private Function GetIdsFromFile(ByRef filePath As String) As List(Of Integer)
        Dim GenomicFeatureIDsToRun As New List(Of Integer)
        Try
            Using sr As New StreamReader(filePath)
                While sr.EndOfStream = False
                    GenomicFeatureIDsToRun.Add(Split(sr.ReadLine(), vbTab)(0))
                End While
            End Using
        Catch
            Console.WriteLine("Please ensure that the file at '" & filePath & "' only integers and valid genomic feature ids")
        End Try
        Return GenomicFeatureIDsToRun
    End Function

    '####used to update progress of the analysis
    Private Sub HandleProgressStart(ByVal progressMaximum As Integer)

    End Sub

    Private Sub HandleProgressUpdate(ByVal currProgress As Integer, ByVal FeatureFileName As String, ByVal GenomicFeatureName As String, ByVal NumMonteCarloRunDone As Integer)
        Dim MonteCarloReport As String = ""
        If NumMonteCarloRunDone > 0 Then MonteCarloReport = " (MC #" & NumMonteCarloRunDone & ")"
        Console.WriteLine("Doing " & AnalysisType & " analysis for " & FeatureFileName & ": " & GenomicFeatureName & MonteCarloReport)
    End Sub

    Private Sub HandleProgressDone(ByVal OuputDir As String)
        Console.WriteLine(AnalysisType & " analysis complete.  Results outputed to: " & OuputDir)
        Console.WriteLine("Press enter to quit...")
        Console.ReadLine()
    End Sub

    Function GetParameters(ByVal Args As String()) As List(Of Parameters)
        Dim Parameters As New List(Of Parameters)
        Dim IsNewParameter As Boolean = False 'The first parameters are the paths of the files, these are skipped 
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

    Function GetParametersFromConfigFile(ByVal configFilePath As String) As List(Of Parameters)
        Dim params As New List(Of Parameters)
        Dim nParameter As New Parameters
        'read file
        Using sr As New StreamReader(configFilePath)
            Dim tempString As String = ""
            While sr.EndOfStream = False
                tempString = sr.ReadLine()
                If tempString <> "" Then
                    'If line starts with "-", get its params. Otherwise ignore.
                    If tempString IsNot Nothing And tempString(0) = "-" Then
                        Dim args As String() = Split(tempString, " ")
                        nParameter = New Parameters
                        nParameter.Name = args(0)
                        nParameter.arguments = New List(Of String)
                        For i As Integer = 1 To args.Count - 1 Step +1
                            nParameter.arguments.Add(args(i))
                        Next
                        params.Add(nParameter)
                    End If
                End If
            End While
        End Using
        Return params
    End Function

    Function GetConnectionSettings(ByVal params As List(Of Parameters)) As ConnectionSettings
        Dim connectSettings As New ConnectionSettings
        For Each param In params
            Select Case param.Name
                Case "-h"
                    connectSettings.host = param.arguments(0)
                Case "-u"
                    connectSettings.user = param.arguments(0)
                Case "-p"
                    connectSettings.password = param.arguments(0)
                Case "-db"
                    connectSettings.database = param.arguments(0)
            End Select
        Next
        Return connectSettings
    End Function

    Function GetGenomicFeaturesFromIDsInputed(ByVal listIDs As List(Of Integer), ByVal strand As String) As List(Of GenomicFeature)
        Dim GenomicFeaturesToRemove As New List(Of Integer)
        Dim ConnectionString As String = GetConnectionString()
        Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GREngin.GetGenomicFeaturesAvailable(ConnectionString) 'gets all of the genomic features available from the GenomeRunner Table
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

        'Get only specified strand if strand isn't "both", which is the default
        If strand.ToLower = "pos" Or strand.ToLower = "neg" Then
            If strand = "pos" Then
                strand = "+"
            Else
                strand = "-"
            End If
            GenomicFeaturesToRunWithEP = GREngin.GenerateGenomicFeaturesByStrand(GenomicFeaturesToRunWithEP, strand, ConnectionString)
        End If
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
        UseChiSquareTest As Boolean = True, UseTradMC As Boolean = False, UseBinomialDistribution As Boolean = False, _
PvalueThreshold As Double = 0.01, OutputPearsonsCoefficientWeightedMatrix As Boolean = False, _
OutputPercentOverlapWeightedMatrix As Boolean = False, SquarePercentOverlap As Boolean = False
        Dim FilterLevel As String = "none"
        Dim PearsonsAdjustment As Integer = 0
        Dim proximity As UInteger = 0
        Dim AllAdjustments = False

        'sets which method should be used to calculate the number of random associations
        For Each param In Parameters
            Select Case param.Name
                '...sets which method should be used to calculate the number of random associations
                Case Is = "-s"
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
                Case Is = "-pval"
                    For Each arg In param.arguments
                        If arg = "ct" Then
                            UseChiSquareTest = True
                            'If IsNumeric(param.arguments(param.arguments.IndexOf(arg) + 1)) Then
                            '    NumOfMCToRun = param.arguments(param.arguments.IndexOf(arg) + 1)
                            'End If
                        ElseIf arg = "tmc" Then
                            UseTradMC = True
                            UseChiSquareTest = False
                            'If IsNumeric(param.arguments(param.arguments.IndexOf(arg) + 1)) Then
                            '    NumOfMCToRun = param.arguments(param.arguments.IndexOf(arg) + 1)
                            'End If
                        ElseIf arg = "bd" Then
                            UseBinomialDistribution = True
                            UseChiSquareTest = False
                        End If
                    Next
                Case Is = "-a" 'Adjustments
                    For Each arg In param.arguments
                        If arg = "pc" Then
                            OutputPearsonsCoefficientWeightedMatrix = True
                            PearsonsAdjustment = arg.Remove(0, 2)
                        ElseIf arg = "po" Then
                            OutputPercentOverlapWeightedMatrix = True
                        ElseIf arg = "all" Then
                            AllAdjustments = True
                        End If
                        If arg = "sq" Then
                            SquarePercentOverlap = True
                        End If
                    Next
                Case Is = "-t" 'Threshold
                    For Each arg In param.arguments
                        If arg = "min" Then
                            FilterLevel = "Minimum"
                        ElseIf arg = "mean" Then
                            FilterLevel = "Mean"
                        End If
                    Next
                Case Is = "-st" 'Strand
                    For Each arg In param.arguments
                        If arg = "pos" Then
                            'TODO what changes for strand?
                            'Use GenerateGenomicFeaturesByStrand

                        End If
                    Next
                Case Is = "-pr" 'Proximity
                    For Each arg In param.arguments
                        proximity = arg
                    Next
            End Select
        Next
        Settings = New EnrichmentSettings(GetConnectionString(), "", outputDirectory, UseMonteCarlo, UseAnalytical, _
                                          UseTradMC, UseChiSquareTest, UseBinomialDistribution, _
                                          OutputPercentOverlapWeightedMatrix, SquarePercentOverlap, _
                                          OutputPearsonsCoefficientWeightedMatrix, PearsonsAdjustment, _
                                          AllAdjustments, connectSettings.database, False, NumOfMCToRun, PvalueThreshold, _
                                          FilterLevel, 2000, 0, proximity)
        Return Settings
    End Function


    Function GetConnectionString() As String
        Dim uName As String
        Dim uPassword As String
        Dim uServer As String
        Dim uDatabase As String
        Dim ConnectionString As String
ConnectionSettingsRetry:
        'TODO this is just finding db info from what's saved in the registry
        Try
            'gets the connection settings from the registry and builds a connection string
            uName = GetSetting("GenomeRunner", "Database", "uName")
            uPassword = GetSetting("GenomeRunner", "Database", "uPassword")
            uServer = GetSetting("GenomeRunner", "Database", "uServer")
            uDatabase = GetSetting("GenomeRunner", "Database", "uDatabase")

            'ConnectionString = "Server=" & uServer & ";Database=" & uDatabase & ";User ID=" & uName & ";password=" & uPassword
            ConnectionString = "Server=" & connectSettings.host & ";Database=" & connectSettings.database & ";User ID=" & connectSettings.user & ";password=" & connectSettings.password

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
