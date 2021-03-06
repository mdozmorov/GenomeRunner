﻿'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
'Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.IO
Imports System.Linq
Imports alglib
Namespace GenomeRunner

    Public Class GenomeRunnerEngine
        Dim cn As SQLiteConnection, cmd As SQLiteCommand, dr As SQLiteDataReader, cmd1 As SQLiteCommand, dr1 As SQLiteDataReader
        Dim cn1 As SQLiteConnection
        Dim UseInterval As Boolean = False
        Structure featureAvailable : Dim name As String, category As String, order As Integer : End Structure  'is used to store the featurenames and categories returned from the genomerunner table
        Dim GRFeaturesAvailable As New List(Of featureAvailable)
        Dim MCcount As UInteger                                                                             'Number of Monte-Carlo simulations
        Dim InputFileName As String, BackgroundFileName As String                        'File system information
        Dim bkgNum As UInteger            'keeps count of the number of background features added: is 0 based
        Dim RandChr() As String, RandStart() As Int64, RandEnd() As Int64                                   'Randomly picked features
        Dim ActualWithin As UInteger, ActualOutside As UInteger                                                   'How many features are within and outside regions of interest
        Dim ExpectedWithin As UInteger, ExpectedOutside As UInteger                                                 'How many random features are within and outside regions of interest
        Dim randWithinGene As UInteger, randWithinExon As UInteger, randWithinPromoter As UInteger, randWithinAltEvent As UInteger      'For counting spot background features
        Dim randOutsideGene As UInteger, randOutsideExon As UInteger, randOutsidePromoter As UInteger, randOutsideAltEvent As UInteger  'For counting spot background features
        Dim FeaturesToCalculatePValues As ArrayList
        Dim listFeatureSQLData As New List(Of FeatureSQLData) 'holds the entire feature table
        'is used to store a row of the genomic feature table that is retrived from the database   
        Dim arrayFeatureSQLData As Integer()
        Dim stopwatch As New System.Diagnostics.Stopwatch
        Public UseSpotBackground As Boolean = False  'by default spotbackground is not used as the whole genome is treated as an interval. 

        Private Sub OpenDatabase(ByVal ConnectionString As String)

            'KBean Changes
            'MsgBox("Before Connection Strin is: " & ConnectionString)
            'ConnectionString = "data source=E:\For Mikhail\Genome Runner\Development\Old Files\mm9.sqlite"

            'If IsNothing(cn) Then
            cn = New SQLiteConnection(ConnectionString) : cn.Open() 'lblProgress.Text = "Database open"
            'ElseIf cn.State = ConnectionState.Closed Then
            'cn = New MySqlConnection(ConnectionString) : cn.Open() 'lblProgress.Text = "Database open"
            'ElseIf ConnectionString <> cn.ConnectionString Then
            'cn.Close()
            'cn = New MySqlConnection(ConnectionString) : cn.Open() 'lblProgress.Text = "Database open"
            'End If
            'opens a second connection so that two reader objects can be used at once
            'If IsNothing(cn1) Then
            cn1 = New SQLiteConnection(ConnectionString) : cn1.Open() 'lblProgress.Text = "Database open"
            'ElseIf cn1.State = ConnectionState.Closed Then
            'cn1 = New MySqlConnection(ConnectionString) : cn1.Open() 'lblProgress.Text = "Database open"
            'End If
        End Sub

        'is a simple structure that stores both the id and name of the GF 
        Structure GFidName
            Public id As Integer
            Public tier As String
            Public name As String
            Public category As String
            Public orderofcategory As Integer
        End Structure

        'gets the IDs of all of the available features
        Public Function GetGenomicFeaturesAvailable(ByVal ConnectionString As String) As List(Of GenomicFeature)
            Dim GenomicFeatures As New List(Of GenomicFeature)
            OpenDatabase(ConnectionString)
            'gets the available features from the genomerunnertable and orders them by categories and then further orders the features in each category by the order they have been set to
            cmd = New SQLiteCommand("SELECT id,featurename, featuretable, queryType, thresholdtype,thresholdMin,thresholdMax,ThresholdMean, category, orderofcategory,tier  FROM genomerunner WHERE querytype != 'NA' ORDER BY category, orderofcategory ASC;", cn) 'gets a list of all of the features
            dr = cmd.ExecuteReader()
            While dr.Read()
                'creates a new genomic feature class and loads in the value for the Genome Feature
                Dim GF As New GenomicFeature(dr(0), dr(1), dr(2), dr(3), dr(4), 0, dr(5), dr(6), dr(7), dr(8), dr(9), Nothing, "", dr(10))
                GenomicFeatures.Add(GF)
                'MsgBox("O: " & dr(0) & " 1: " & dr(1) & " 2: " & dr(2) & " 3: " & dr(3) & " 4: " & dr(4) & " 5: " & dr(5) & " 6: " & dr(6) & " 7: " & dr(7) & " 8: " & dr(8) & " 9: " & dr(9) & " 10: " & dr(10))
            End While
            cmd.Dispose() : dr.Close()
            Return GenomicFeatures
        End Function

        'returns whether a column for name exists or not
        'Private Function NameColumnExists(ByVal TableName As String, ByVal ConnectionString As String) As Boolean
        '   OpenDatabase(ConnectionString)
        'cmd1 = New SQLiteCommand("SHOW COLUMNS FROM " & TableName, cn1)
        'KBean Changes
        '  cmd1 = New SQLiteCommand("PRAGMA table_info(" & TableName & ")", cn1)
        ' dr1 = cmd1.ExecuteReader()
        'Dim columnnames As String = ""
        '   While dr1.Read()
        '      columnnames &= dr1(0)
        ' End While
        'MsgBox("Column Names: " & columnnames)
        'dr1.Close() : cmd1.Dispose()
        'checks if name columne exists
        'Dim strandIndex As Integer = columnnames.ToLower().IndexOf("name")
        '   If strandIndex <> -1 Then
        '      Return True
        ' Else
        '    Return False
        'End If
        'End Function

        'returns whether a column for strand exists or not
        Private Function StrandColumnExists(ByVal TableName As String, ByVal ConnectionString As String) As Boolean
            Dim StrandColExists As Boolean = False
            OpenDatabase(ConnectionString)
            'KBean Changes
            'cmd1 = New SQLiteCommand("SHOW COLUMNS FROM " & TableName, cn1)
            cmd1 = New SQLiteCommand("PRAGMA table_info(" & TableName & ")", cn1)
            dr1 = cmd1.ExecuteReader()
            Dim columnnames As String = ""
            While dr1.Read()
                columnnames &= dr1(1)
            End While
            'MsgBox("Column Names: " & columnnames)
            dr1.Close() : cmd1.Dispose()
            'checks if strand columne exists
            Dim strandIndex As Integer = columnnames.ToLower().IndexOf("strand")
            If strandIndex <> -1 Then
                'checks if strand not blank
                cmd1 = New SQLiteCommand("SELECT strand FROM " & TableName & " LIMIT 1", cn1)
                dr1 = cmd1.ExecuteReader()
                Dim strandData = ""
                While dr1.Read()
                    strandData &= dr1(0)
                End While
                dr1.Close() : cmd1.Dispose()
                If strandData = "+" Or strandData = "-" Then
                    StrandColExists = True
                End If
            End If
            Return StrandColExists
        End Function

        'checks whether the strand column has any positive strands 
        Private Function StrandColumnHasPositiveStrand(ByVal GF As GenomicFeature, ByVal ConnectionString As String) As Boolean
            Dim PositiveHasRows As Boolean = False
            OpenDatabase(ConnectionString)
            cmd1 = New SQLiteCommand("SELECT strand FROM " & GF.TableName & " Where strand='" & "+' LIMIT 1;", cn1)
            dr1 = cmd1.ExecuteReader()
            If dr1.HasRows = True Then
                PositiveHasRows = True
                'MsgBox("Column has positive strand " & GF.TableName)
            End If
            dr1.Close() : cmd1.Dispose()
            Return PositiveHasRows
        End Function

        'checks whether the strand column has any negative strands 
        Private Function StrandColumnHasNegativeStrand(ByVal GF As GenomicFeature, ByVal ConnectionString As String) As Boolean
            Dim NegativeHasRows As Boolean = False
            OpenDatabase(ConnectionString)
            cmd1 = New SQLiteCommand("SELECT strand FROM " & GF.TableName & " Where strand='" & "-" & "' LIMIT 1;", cn1)
            dr1 = cmd1.ExecuteReader()
            If dr1.HasRows = True Then
                NegativeHasRows = True
                'MsgBox("Column has negative strand " & GF.TableName)
            End If
            dr1.Close() : cmd1.Dispose()
            Return NegativeHasRows
        End Function

        'adds a genomic feature for the seperate strands for GF that have a strand column in their data table
        Public Function GenerateGenomicFeaturesByStrand(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal StrandToAnalyze As String, ByVal ConnectionString As String) As List(Of GenomicFeature)
            Dim GenomicFeaturesByStrand As New List(Of GenomicFeature)
            For Each GF In GenomicFeatures
                Dim StrandExists As Boolean = StrandColumnExists(GF.TableName, ConnectionString) 'whether a strand column exists in the genomic feature's table
                If StrandToAnalyze = "Both" Then
                    GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "", GF.Tier))
                Else
                    If StrandExists = True Then
                        'checks if the strand column actually has any positive or negative strand data before adding the feature to the list
                        If StrandColumnHasPositiveStrand(GF, ConnectionString) = True And StrandToAnalyze = "+" Then
                            'GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, "Plus:" & GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "+", GF.Tier))
                            GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "+", GF.Tier))
                        End If
                        If StrandColumnHasNegativeStrand(GF, ConnectionString) = True And StrandToAnalyze = "-" Then
                            'GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, "Minus:" & GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "-", GF.Tier))
                            GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "-", GF.Tier))
                        End If
                    Else
                        GenomicFeaturesByStrand.Add(New GenomicFeature(GF.id, GF.Name, GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, "", GF.FilteredByName, GF.NamesToInclude, "", GF.Tier))
                    End If
                End If
            Next
            Return GenomicFeaturesByStrand
        End Function

        'Adds a genomic feature for each distinct name in the name column Genomic Feature if it exists
        Public Function GenerateGenomicFeaturesByName(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal connectionString As String) As List(Of GenomicFeature)
            Dim GenomicFeaturesByName As New List(Of GenomicFeature)
            For Each GF In GenomicFeatures
                OpenDatabase(connectionString)
                Dim NameColumn As String = vbNullString
                cmd = New SQLiteCommand("SELECT Name FROM genomerunner WHERE FeatureTable = '" & GF.TableName & "';", cn)
                dr = cmd.ExecuteReader()
                'Get name from the Name column
                If dr.HasRows Then dr.Read() : NameColumn = dr(0)
                'MsgBox("Name Column: " & dr(0))
                dr.Close() : cmd.Dispose()
                If NameColumn <> vbNullString And NameColumn <> "NULL" Then
                    Dim Names As New List(Of String)
                    cmd = New SQLiteCommand("SELECT DISTINCT " & NameColumn & " FROM " & GF.TableName, cn)
                    dr = cmd.ExecuteReader()
                    While dr.Read()
                        Names.Add(dr(0))
                        'MsgBox("Distinct Name: " & dr(0))
                    End While
                    dr.Close() : cmd.Dispose()
                    'creates a genomic feature to analyze for each name found which is set to filter by the name 
                    For Each name In Names
                        Dim lName As New List(Of String)
                        lName.Add(name) 'adds the single name to list of names to filter which is passed to the genomic feature. 
                        'Dim nGF As New GenomicFeature(GF.id, NameColumn & "." & lName(0), GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, GF.UICategory, GF.IUOrderInCategory, lName, GF.StrandToFilterBy, GF.Tier)
                        Dim nGF As New GenomicFeature(GF.id, NameColumn & "." & GF.TableName & "." & lName(0), GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, GF.UICategory, GF.IUOrderInCategory, lName, GF.StrandToFilterBy, GF.Tier)
                        GenomicFeaturesByName.Add(nGF)
                    Next
                Else
                    GenomicFeaturesByName.Add(GF)
                End If

                'If NameColumnExists(GF.TableName, connectionString) = True Then
                '    Dim Names As New List(Of String)
                '    'GenomicFeaturesByName.Add(GF) 'adds the initial feature with all names to be filtered 
                '    'Gets the unique names for the GF and adds them to a list
                '    OpenDatabase(connectionString)
                '    'TODO make this part that finds NameColumn into a private subroutine for clarity's sake.
                '    Dim NameColumn As String
                '    cmd = New MySqlCommand("SELECT Name FROM genomerunner WHERE FeatureTable = '" & GF.TableName & "';", cn)
                '    dr = cmd.ExecuteReader()
                '    While dr.Read()
                '        NameColumn = dr(0)
                '    End While
                '    dr.Close() : cmd.Dispose()
                '    cmd = New MySqlCommand("SELECT DISTINCT " & NameColumn & " FROM " & GF.TableName, cn)
                '    dr = cmd.ExecuteReader()
                '    While dr.Read()
                '        Names.Add(dr(0))
                '    End While
                '    dr.Close() : cmd.Dispose()
                '    'creates a genomic feature to analyze for each name found which is set to filter by the name 
                '    For Each name In Names
                '        Dim lName As New List(Of String)
                '        lName.Add(name) 'adds the single name to list of names to filter which is passed to the genomic feature. 
                '        Dim nGF As New GenomicFeature(GF.id, GF.Name & "." & lName(0), GF.TableName, GF.QueryType, GF.ThresholdType, GF.Threshold, GF.ThresholdMin, GF.ThresholdMax, GF.ThresholdMean, GF.UICategory, GF.IUOrderInCategory, lName, GF.StrandToFilterBy, GF.Tier)
                '        GenomicFeaturesByName.Add(nGF)
                '    Next
                'Else
                '    GenomicFeaturesByName.Add(GF)
                'End If
            Next
            Return GenomicFeaturesByName
        End Function

        Public Function GetGenomeBackground(ByVal ConnectionString As String) As List(Of Feature)
            Dim Background As New List(Of Feature)
            OpenDatabase(ConnectionString)
            cmd = New SQLiteCommand("SELECT * FROM background;", cn)
            dr = cmd.ExecuteReader()
            While dr.Read()
                Dim feature As New Feature
                feature.Chrom = dr(0)
                feature.ChromStart = dr(1)
                feature.ChromEnd = dr(2)
                Background.Add(feature)
            End While
            Return Background
        End Function

        Public Function GetChromInfo(ByVal ConnectionString As String) As List(Of Feature)
            Dim ChromInfo As New List(Of Feature)
            OpenDatabase(ConnectionString)
            cmd = New SQLiteCommand("SELECT * FROM chromInfo;", cn)
            dr = cmd.ExecuteReader()
            While dr.Read()
                'chromInfo table format:
                'chrom (string), size (int), filename (string)
                Dim feature As New Feature
                feature.Chrom = dr(0)
                feature.ChromStart = 0
                feature.ChromEnd = dr(1)
                ChromInfo.Add(feature)
            End While
            Return ChromInfo
        End Function

        'returns a list of features that can be used as a background, can be spot or interval
        Public Function GenerateCustomGenomeBackground(ByVal BackgroundFilePath As String) As List(Of Feature)
            Dim Background As New List(Of Feature)
            Using SR As New StreamReader(BackgroundFilePath)
                While SR.EndOfStream = False
                    Dim Interval As New Feature
                    Dim Line = SR.ReadLine().Split(vbTab)
                    Interval.Chrom = Line(0) : Interval.ChromStart = Line(1)
                    If Line.Length > 2 Then
                        Interval.ChromEnd = Line(2)
                    Else
                        Interval.Chrom = Interval.ChromStart
                    End If
                    Background.Add(Interval)
                End While
            End Using
            Return Background
        End Function

        'returns a list of features that can be used as a background, can be spot or interval
        'For large SNP file, predefined arrays are populated, then Background created
        Public Function GenerateCustomGenomeBackgroundSNP(ByVal BackgroundFilePath As String) As List(Of Feature)
            Dim Background As New List(Of Feature)
            Dim ArraySize = 33026120
            Dim backChr As String() : ReDim backChr(ArraySize)
            Dim backChrStart As UInteger() : ReDim backChrStart(ArraySize)
            Dim backChrEnd As UInteger() : ReDim backChrEnd(ArraySize)
            Dim counter As Long = 0
            Using SR As New StreamReader(BackgroundFilePath)
                For i = 0 To ArraySize
                    Dim Line = SR.ReadLine().Split(vbTab)
                    backChr(counter) = Line(0)
                    backChrStart(counter) = Line(1)
                    If Line.Length > 2 Then
                        backChrEnd(counter) = Line(2)
                    Else
                        backChrEnd(counter) = backChrStart(counter)
                    End If
                    counter += 1
                    If (counter Mod 100000) = 0 Then Debug.Print(counter & " lines processed") : GC.Collect()
                Next
                For i = 0 To counter - 1
                    Dim Interval As New Feature
                    Interval.Chrom = backChr(i) : Interval.ChromStart = backChrStart(i) : Interval.ChromEnd = backChrEnd(i)
                    Background.Add(Interval)
                Next
            End Using
            Return Background
        End Function

        Public Function GenerateGenomeBackground(ByVal ConnectionString As String, ByVal SelectedGFname As String) As List(Of Feature)
            Dim Background As List(Of Feature) = New List(Of Feature)
            Dim chrom As List(Of String) = New List(Of String)
            OpenDatabase(ConnectionString)
            cmd = New SQLiteCommand("SELECT DISTINCT chrom FROM " & SelectedGFname & ";", cn)
            dr = cmd.ExecuteReader
            While dr.Read
                chrom.Add(dr(0))
                'MsgBox("Read in Chrom: " & dr(0))
            End While
            dr.Close() : cmd.Dispose()

            For Each chromosome In chrom
                cmd = New SQLiteCommand("SELECT chromStart,chromEnd FROM " & SelectedGFname & " WHERE chrom='" & chromosome & "';", cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    Dim Interval As New Feature
                    Interval.Chrom = chromosome
                    Interval.ChromStart = dr(0)
                    Interval.ChromEnd = dr(1)

                    'MsgBox("Chromosome: " & chromosome & " Start: " & dr(0) & " End: " & dr(1))

                    Background.Add(Interval)
                End While
                dr.Close() : cmd.Dispose() : GC.Collect()
                Debug.Print(Now.TimeOfDay.ToString & " " & chromosome)
            Next
            Return Background
        End Function
        Public Sub New()
            'GetGenomeBackground() 'sets the default interval to cover the entire genome
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class
End Namespace