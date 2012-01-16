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
        Dim Settings As New EnrichmentSettings
        Dim flags As New List(Of String)
        Dim progStart As ProgressStart : progStart = AddressOf HandleProgressStart
        Dim progUpdate As ProgressUpdate : progUpdate = AddressOf HandleProgressUpdate
        Dim progDone As ProgressDone : progDone = AddressOf HandleProgressDone
        'PrintHelp()
        'TODO raise error if args(2) is out of range!
        'TODO shouldn't need to use this params stuff any more. Plus it's sketchy because it uses args outside of the Main method.
        If args.Length = 4 Then

            Dim analysisType As String = args(0)
            Dim FOIFilePath As String = args(1)
            Dim GenomicFeatureIDsPath As String = args(2)
            Dim SettingsPath As String = args(3)

            'Read genomic features file into GenomicFeatureIDsToRun
            Dim GenomicFeatureIDsToRun As List(Of Integer) = GetIdsFromFile(GenomicFeatureIDsPath)

            If analysisType = "-e" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ENRICHMENT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                analysisType = "Enrichment" 'Used to determine what to write for progress updates

                Dim featureOfInterestPath As New List(Of String)

                featureOfInterestPath.Add(FOIFilePath)
                'Parameters = GetParameters(args) 'get the parameters inputed by the command line and orginizes them into parameters
                'Settings = GetEnrichmentSettings(OutputDir, params) 'generates an enrichmentsettings classed based on the paramaters inputed by the user. 

                'Deserialize XML file to a new object.
                Dim sr As New StreamReader(SettingsPath)
                Dim x As New XmlSerializer(Settings.GetType)
                Settings = x.Deserialize(sr) : sr.Close()
                Settings.OutputMerged = False 'Just a precaution, to prevent incorrect output. For command line it is always individual file processing
                'Dim OutputDir As String = Path.GetDirectoryName(FOIFilePath) & "\" & Strings.Replace(Date.Now, "/", "-").Replace(":", ",") & "\" 'sets what directory the results are to outputed to
                'Dim OutputDir As String = DateTime.Now.ToString("MM-dd-yyyy_hh.mm.sstt") & "_" & Path.GetFileNameWithoutExtension(FOIFilePath) & "\" 'sets what directory the results are to outputed to
                'Settings.OutputDir &= OutputDir 'Add current time subfolder

                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, Settings.ConnectionString, Settings.Strand)
                Dim Analyzer As New EnrichmentAnalysis(progStart, progUpdate, progDone)
                'Dim Background As List(Of Feature) = GREngin.GetGenomeBackground(Settings.ConnectionString)
                Dim Background As List(Of Feature) = GREngin.GetChromInfo(Settings.ConnectionString)
                Analyzer.RunEnrichmentAnlysis(featureOfInterestPath, GenomicFeaturesToRun, Background, Settings)

                'Copy original settings file to new directory.
                File.Delete(Settings.OutputDir & "EnrichmentSettings.xml")
                File.Copy(SettingsPath, Settings.OutputDir & "EnrichmentSettings.xml")
            ElseIf analysisType = "-a" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'ANNOTATION
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim FeaturesOfInterest As New List(Of String)
                FeaturesOfInterest.Add(FOIFilePath)
                analysisType = "Annotation"                                                                                 'Used to determine what to write for progress updates
                Dim AnoSettings As New AnnotationSettings()
                'Deserialize XML file to a new object.
                Dim sr As New StreamReader(SettingsPath)
                Dim x As New XmlSerializer(AnoSettings.GetType)
                AnoSettings = x.Deserialize(sr)
                sr.Close()

                Dim GenomicFeaturesToRun As List(Of GenomicFeature) = GetGenomicFeaturesFromIDsInputed(GenomicFeatureIDsToRun, AnoSettings.ConnectionString, AnoSettings.Strand)
                'TODO get this above the loop now
                'Dim ConnectionString As String = GetConnectionString()
                Dim analyzer As New AnnotationAnalysis()
                Dim OutputDir As String = Path.GetDirectoryName(FeaturesOfInterest(0)) & "\" '& DateTime.Now.ToString("MM-dd-yyyy_hh.mm.sstt") & "_" & Path.GetFileNameWithoutExtension(FOIFilePath) & "\" 'sets what directory the results are to outputed to
                analyzer.RunAnnotationAnalysis(FeaturesOfInterest, GenomicFeaturesToRun, OutputDir, AnoSettings, progStart, progUpdate, progDone)

                'Copy original settings file to new directory.
                File.Copy(SettingsPath, OutputDir & "AnnotationSettingsCopy.xml")

            End If
        ElseIf args.Length = 2 Then
            Dim AnalysisType = args(0)
            If AnalysisType = "-m" Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'MERGE LOG FILES
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim DirectoryToUse As String = args(1)
                Dim outputter As New Output(0)
                Dim dirInfo As New DirectoryInfo(DirectoryToUse)
                Dim Files As FileInfo() = dirInfo.GetFiles("*.gr")
                Dim filePaths As New List(Of String)

                For Each logFile In Files
                    filePaths.Add(logFile.FullName)
                Next
                Console.WriteLine("Merging files: " & Join(filePaths.ToArray, " "))
                outputter.OutputMergedLogFiles(filePaths)
            End If
        Else
            PrintHelp()
        End If
    End Sub

    Private Sub PrintHelp()
        Console.WriteLine("Welcome to GenomeRunner")
        Console.WriteLine("Usage: GenomeRunnerConsole.exe [-a|-e|-m] <Features_Of_Interest.bed> <Genomic_Features_IDs.txt> <Settings.xml>")
        Console.WriteLine()
        Console.WriteLine("Example: GenomeRunnerConsole.exe -e " & """" & "F:\GRtest\wgRNA-HAcaBox.bed" & """" & " " & """" & "F:\GRtest\GFs.txt" & """" & " " & """" & "F:\GRtest\EnrichmentSettings.xml" & """" & "")
        Console.WriteLine()
        Console.WriteLine("GenomeRunner – Annotation and Enrichment analysis of experimental Features Of Interest (FOIs) vs. Genomic Features (GFs)")
        Console.WriteLine()
        Console.WriteLine("Options:")
        Console.WriteLine("-a" & vbTab & "Annotation analysis. Returns <FOI.gr> tab-delimited file with Num_Of_GFs x Num_Of_FOIs table with number of times a FOI co-localized with and/or lies in the proximity of a GF. <Details> section provides full information about GFs.")
        Console.WriteLine()
        Console.WriteLine("-e" & vbTab & "Enrichment analysis. Returns <…_Matrix.gr> tab-delimited file with Num_Of_GFs x 1 table with p-values of a set of FOIs enrichment/depletion within and/or in proximity of a set of GFs, as compared with random chance. P-values are –log10 transformed, and a '-' is added to specify depletion. Multiple matrixes may be outputted, if <AllAdjustments> settings are set to TRUE, see <GenomeRunner – Supplemental.pdf> for explanation.")
        Console.WriteLine()
        Console.WriteLine("-m" & vbTab & "Merging enrichment analysis .gr matrixes from multiple runs into a single matrix. This switch accepts only a single following parameter - <C:\Path_To\Some_directory_of_.gr_files>. Matrixes should be from the runs using the same <Genomic_Features_IDs.txt> and copied into a single folder for merging.")
        Console.WriteLine()
        Console.WriteLine("<Features_Of_Interest.bed> - User provided set of Features Of Interest. A tab-delimited file containing chrom, chromStart, chromEnd, name, strand, the rest of the columns is ignored.")
        Console.WriteLine()
        Console.WriteLine("<Genomic_Features_IDs.txt> - A tab-delimited file with Genomic Features IDs. This file can be created by selecting desirable rows from 'genomerunner.txt'; pre-defined Tier 1,2,3 GF IDs files are provided.")
        Console.WriteLine()
        Console.WriteLine("<Settings.xml> - A text file containing database and analysis settings. <OutputDir> setting specifies the folder for outputting the results; for other settings see <GenomeRunner – Supplemental.pdf>.")
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
        'Console.WriteLine("Press enter to quit...")
        'Console.ReadLine()
        Console.WriteLine("*******************************")
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
                Dim nGFPromoter As New GenomicFeature(GF.id, GF.Name & "Promoter", GF.TableName & "Promoter", "Promoter", "NA", 0, "2000", "10000", "5000", "", 0, Nothing, "", 1)
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
