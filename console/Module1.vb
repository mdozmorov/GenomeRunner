'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports GenomeRunnerConsole.GenomeRunner
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Xml.Serialization

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
        Dim Settings As New EnrichmentSettings
        Dim flags As New List(Of String)
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        Console.WriteLine("Welcome to GenomeRunner")
        PrintHelp()
        'TODO raise error if args(2) is out of range!
        params = GetParametersFromConfigFile(args(2))
        If args.Length >= 4 Then

            'Read genomic features file from args(1) into GenomicFeatureIDsToRun
            Dim GenomicFeatureIDsToRun As List(Of Integer) = GetIdsFromFile(args(1))

            If args(3) = "-e" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ENRICHMENT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                AnalysisType = "Enrichment" 'Used to determine what to write for progress updates

                Dim featureOfInterestPath As New List(Of String)

                featureOfInterestPath.Add(args(0))
                'Parameters = GetParameters(args) 'get the parameters inputed by the command line and orginizes them into parameters
                Dim OutputDir As String = Path.GetDirectoryName(args(0)) & "\" & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & "\" 'sets what directory the results are to outputed to
                'Settings = GetEnrichmentSettings(OutputDir, params) 'generates an enrichmentsettings classed based on the paramaters inputed by the user. 

                'Deserialize XML file to a new object.
                Dim sr As New StreamReader(args(2))
                Dim x As New XmlSerializer(Settings.GetType)
                Settings = x.Deserialize(sr)
                sr.Close()

                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, Settings.ConnectionString, Settings.Strand)
                Dim Analyzer As New EnrichmentAnalysis(progStart, progUpdate, progDone)
                Dim Background As List(Of Feature) = GREngin.GetGenomeBackground(Settings.ConnectionString)
                Dim allAdjustments As Boolean = False
                If params.Any(Function(p) p.Name = "-all") Then allAdjustments = True
                Analyzer.RunEnrichmentAnlysis(featureOfInterestPath, GenomicFeaturesToRun, Background, Settings)

                'Serialize EnrichmentSettings!
                'Dim objStreamWriter As New StreamWriter("C:\Ryan Projects\EnrichmentSettings.xml")
                'Dim x As New XmlSerializer(Settings.GetType)
                'x.Serialize(objStreamWriter, Settings)
                'objStreamWriter.Close()

                
            ElseIf args(3) = "-a" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ANNOTATION
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim FeaturesOfInterest As New List(Of String)
                FeaturesOfInterest.Add(args(0))
                AnalysisType = "Annotation"                                                                                 'Used to determine what to write for progress updates
                Dim AnoSettings As New AnnotationSettings()

                'Deserialize XML file to a new object.
                Dim sr As New StreamReader(args(2))
                Dim x As New XmlSerializer(AnoSettings.GetType)
                AnoSettings = x.Deserialize(sr)
                sr.Close()

                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, AnoSettings.ConnectionString, AnoSettings.Strand)
                'TODO get this above the loop now
                'Dim ConnectionString As String = GetConnectionString()
                Dim analyzer As New AnnotationAnalysis()
                Dim OutputDir As String = Path.GetDirectoryName(FeaturesOfInterest(0)) & "\"
                Dim shortOnly As Boolean = False
                If params.Any(Function(p) p.Name = "-short") Then shortOnly = True
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

    Function GetGenomicFeaturesFromIDsInputed(ByVal listIDs As List(Of Integer), ByVal ConnectionString As String, ByVal strand As String) As List(Of GenomicFeature)
        Dim GenomicFeaturesToRemove As New List(Of Integer)
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

End Module
