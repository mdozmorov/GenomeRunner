'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports MySql.Data.MySqlClient
Imports System.IO
Imports alglib

Namespace GenomeRunner

    Public Class AnnotationSettings
        Public ConnectionString As String
        Public PromoterUpstream As UInteger = 0                                          'the numbr of base pairs that the promoters are defined to start upstream of the gene start site
        Public PromoterDownstream As UInteger = 0                                        'the number of base pairs that the promoters are defined to go past the gene start site
        Public proximity As UInteger = 0                                                 'the number of base pairs that a feature of interest can be from a genomic feature and still be considered a hit
        Public FilterLevel As String
        Public Strand As String
        Public ShortOnly As Boolean

        Public Sub New(ByVal ConnectionString As String, ByVal PromoterUpstream As UInteger, ByVal PromoterDownstream As UInteger, ByVal proximity As UInteger, ByVal FilterLevel As String, ByVal Strand As String, ByVal ShortOnly As Boolean)
            Me.ConnectionString = ConnectionString
            Me.PromoterUpstream = PromoterUpstream
            Me.PromoterDownstream = PromoterDownstream
            Me.proximity = proximity
            Me.FilterLevel = FilterLevel
            Me.Strand = Strand
            Me.ShortOnly = ShortOnly
        End Sub

        Public Sub New()
            'Constructor with no parameters is used for XML serialization
        End Sub
    End Class

    Public Class AnnotationAnalysis
        Dim cn As MySqlConnection, cmd As MySqlCommand, dr As MySqlDataReader, cmd1 As MySqlCommand, dr1 As MySqlDataReader, cn1 As MySqlConnection
        Dim kgIDToGeneSymbolDict As New Dictionary(Of String, String)                   'is a dictionary that is loaded with the values from the kgxref (kgID, geneSymbol) to convert the gene name into standard form
        Dim Background As List(Of Feature)                                              'the background from which random features of interest are generated for the Monte Carlo simulation
        Dim ConnectionString As String                                                  'stores settings which are used by the annotation analysis
        
        Public Sub New()

        End Sub

        'opens a connection to the database
        Private Sub OpenDatabase()
            If IsNothing(cn) Then
                cn = New MySqlConnection(ConnectionString) : cn.Open()
            End If
            If cn.State = ConnectionState.Closed Then
                cn = New MySqlConnection(ConnectionString) : cn.Open()
            End If
            'opens a second connection so that two reader objects can be used at once
            If IsNothing(cn1) Then
                cn1 = New MySqlConnection(ConnectionString) : cn1.Open()
            End If
            If cn1.State = ConnectionState.Closed Then
                cn1 = New MySqlConnection(ConnectionString) : cn1.Open()
            End If
        End Sub


        'loads the FOIs from the filepath
        Private Function LoadFeatureOfInterests(ByVal FeaturesOfInterestFilePath As String) As List(Of Feature)
            Dim FeaturesOfInterest As New List(Of Feature)
            Dim Line As String, LineArray() As String
            'Open the file with genomic regions of interest
            Dim sr As New StreamReader(FeaturesOfInterestFilePath)

            While sr.EndOfStream = False
                Dim feature As New Feature
                Line = sr.ReadLine()
                LineArray = Line.Split(vbTab)
                feature.Chrom = LineArray(0)
                feature.ChromStart = LineArray(1)
                If LineArray.Length > 2 Then 'checks if there is a third column
                    If IsNumeric(LineArray(2)) = True And LineArray(2) <> "" Then 'If the third column is not blank and contains a number, it's used as the chrom end, otherwise the end is set equal to the startpoint and the feature is treated as a SNP
                        feature.ChromEnd = LineArray(2)
                    Else
                        feature.ChromEnd = feature.ChromStart
                    End If
                Else
                    feature.ChromEnd = feature.ChromStart
                End If
                If LineArray.Length > 3 Then 'checks if there is a fourth column which should contain the name of the FOI
                    feature.Name = LineArray(3)
                End If
                FeaturesOfInterest.Add(feature)
            End While
            Return FeaturesOfInterest
        End Function

        'runs annotation anlysis on all of the files passed as paths
        Public Sub RunAnnotationAnalysis(ByVal FeatureOfInterestPath As List(Of String), ByVal GenomicFeatures As List(Of GenomicFeature), ByVal OutputDir As String, ByVal AnnotationSettings As AnnotationSettings, ByVal Progstart As ProgressStart, ByVal ProgUpdate As ProgressUpdate, ByVal ProgDone As ProgressDone)
            Directory.CreateDirectory(OutputDir)
            ConnectionString = AnnotationSettings.ConnectionString

            For Each FeatureFilePath In FeatureOfInterestPath
                Dim FeaturesOfInterest As List(Of Feature) = LoadFeatureOfInterests(FeatureFilePath)
                Dim FeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(FeaturesOfInterest, AnnotationSettings.proximity)
                Dim FeatureFileName As String = GetFeatureFileName(FeatureFilePath)
                Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
                Dim OutputPath As String = OutputDir & FeatureFileName & ".gr"
                Dim output As New Output(NumOfFeatures)
                Progstart.Invoke(GenomicFeatures.Count)
                Dim CurrGF As Integer = 0
                For Each GF In GenomicFeatures
                    ProgUpdate(CurrGF, Path.GetFileName(FeatureFilePath), GF.Name, 0)
                    GF = Feature_Analysis(GF, FeaturesOfInterestproximity, FeaturesOfInterest, AnnotationSettings)
                    CurrGF += 1
                Next
                output.OutputAnnotationAnalysis(OutputPath, GenomicFeatures, FeaturesOfInterest)
            Next
            ProgDone(OutputDir)
        End Sub

        Private Function GetFeatureFileName(ByVal FeaturePath As String) As String
            Dim FeatureFilePathSplit As String() = FeaturePath.Split("\")
            Dim ExtensionStartIndex As String = FeatureFilePathSplit(FeatureFilePathSplit.Length - 1).LastIndexOf(".")
            Dim FeatureFileName As String = FeatureFilePathSplit(FeatureFilePathSplit.Length - 1).Remove(ExtensionStartIndex)
            Return FeatureFileName
        End Function

        'creates features of interest with extended start and endpoints for proximity analysis
        Private Function CreateproximityFeaturesOfInterest(ByVal FeaturesOfInterest As List(Of Feature), ByVal proximity As UInteger) As List(Of Feature)
            Dim FeaturesOfInterestproximity As New List(Of Feature)
            For Each FOI In FeaturesOfInterest
                Dim nFOI As New Feature
                nFOI.Chrom = FOI.Chrom
                nFOI.ChromStart = FOI.ChromStart - proximity
                nFOI.ChromEnd = FOI.ChromEnd + proximity
                FeaturesOfInterestproximity.Add(nFOI)
            Next
            Return FeaturesOfInterestproximity
        End Function

        'Checks which Features overlap with the GF and returns the meta data for the genomic feature
        Function Feature_Analysis(ByVal GenomicFeature As GenomicFeature, ByVal proximityFeaturesOfInterest As List(Of Feature), ByVal FeaturesOfInterest As List(Of Feature), ByVal Settings As AnnotationSettings) As GenomicFeature
            ConnectionString = Settings.ConnectionString
            Dim NumOfFeatures As Integer = proximityFeaturesOfInterest.Count - 1
            Dim listFeatureSQLData As List(Of FeatureSQLData)
            GenomicFeature.FeatureReturnedData.Clear() 'clears the feature class of any past metadata (returned GF information)
            'goes through and finds all of the unique chroms in the FOI
            Dim UniqueChroms As New HashSet(Of String)
            For Each f In proximityFeaturesOfInterest
                If Not UniqueChroms.Contains(f.Chrom) Then
                    UniqueChroms.Add(f.Chrom)
                End If
                'adds an empty slot for each FOI in the returned hits part 
                Dim hit As New FeaturesReturnedHits
                hit.CountData = 0
                GenomicFeature.FeatureReturnedData.Add(hit)
            Next

            Select Case GenomicFeature.QueryType
                Case Is = "Threshold" 'occurs in the case where the threshold (can be set to score or signal value) is used in filtering results     

                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, GenomicFeature.Threshold, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                    Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                     Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next


                Case Is = "General"
                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next

                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next

                Case Is = "OutputScore"
                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    Dim debugFeatureSQLdatacount As Integer = 0
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                            debugFeatureSQLdatacount += listFeatureSQLData.Count
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next
                    Debug.Print("Total rows returned for annotaitonanalysis" & debugFeatureSQLdatacount)

                Case Is = "GeneOnly"
                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next

                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next

                Case Is = "Gene"
                    Dim fthreshold As String
                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    Dim debugFeatureSQLdatacount As Integer = 0
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                            debugFeatureSQLdatacount += listFeatureSQLData.Count
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next
                    Debug.Print("Total rows returned for annotaitonanalysis" & debugFeatureSQLdatacount)

                Case Is = "Promoter"
                    Dim fthreshold As String
                    'goes through each feature of interest and decides if it falls within the genomic region of interest
                    Dim lastChrom As String = ""
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits 'creates an instance of the feature hit class which contains all of the hits for the current FIO
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                'Dim matchesName As Boolean = True
                                'If genomicFeature.FilteredByName = True And genomicFeature.NamesToInclude.Count <> 0 Then 'if the GR feature is supposed to filtered by name, then the hit is further checked to see if its name matches that of one of the names to be included
                                '    matchesName = False
                                '    For Each nameToInclude In genomicFeature.NamesToInclude
                                '        If featureRow.Name = nameToInclude Then
                                '            matchesName = True 'if a match is found than the GR featurerow metadata is stored as a returned hit
                                '        End If
                                '    Next
                                'End If
                                'If matchesName = True Then
                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                                'End If
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next

                Case Is = "Exon"
                    Dim lastChrom As String = ""
                    Dim debugFeatureSQLdatacount As Integer = 0
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            debugFeatureSQLdatacount += listFeatureSQLData.Count
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then
                                Dim matchesName As Boolean = True
                                If GenomicFeature.FilteredByName = True And GenomicFeature.NamesToInclude.Count <> 0 Then 'if the GR feature is supposed to filtered by name, then the hit is further checked to see if its name matches that of one of the names to be included
                                    matchesName = False
                                    For Each nameToInclude In GenomicFeature.NamesToInclude
                                        If featureRow.Name = nameToInclude Then
                                            matchesName = True 'if a match is found than the GR featurerow metadata is stored as a returned hit
                                        End If
                                    Next
                                End If

                                If matchesName = True Then
                                    featureHit.CountData += 1
                                    featureHit.fStartData.Add(featureRow.ChromStart)
                                    featureHit.fEndData.Add(featureRow.ChromEnd)
                                    featureHit.StrandData.Add(featureRow.Strand)
                                    featureHit.NameData.Add(featureRow.Name)
                                    featureHit.ThresholdData.Add(featureRow.Threshold)
                                    featureHit.OverLapTypeData.Add(Nothing)
                                    featureHit.OverLapAmountData.Add(Nothing)
                                    GenomicFeature.FeatureReturnedData(x) = featureHit
                                End If
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next
                    Debug.Print("Total rows returned for annotaitonanalysis" & debugFeatureSQLdatacount)
                Case Is = "Custom"
                    Dim lastChrom As String = ""
                    For x As Integer = 0 To NumOfFeatures Step +1
                        Dim featureHit As New FeaturesReturnedHits
                        If proximityFeaturesOfInterest(x).Chrom <> lastChrom Then 'if the current FOI is on a diff chrom than the last the GR features is reloded for that chrom
                            listFeatureSQLData = Feature_Load_GRFeature_In_Memory(GenomicFeature, proximityFeaturesOfInterest(x).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads the GR feature data of GR features on the same chrom as the FOI from the mysql database into memory
                            lastChrom = proximityFeaturesOfInterest(x).Chrom
                        End If
                        'compares the feature to the entire listfeaturedata for regions that fall within
                        For Each featureRow In listFeatureSQLData
                            If (proximityFeaturesOfInterest(x).ChromStart >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromStart <= featureRow.ChromEnd) _
                                Or (proximityFeaturesOfInterest(x).ChromEnd >= featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd <= featureRow.ChromEnd) _
                                 Or (proximityFeaturesOfInterest(x).ChromStart < featureRow.ChromStart And proximityFeaturesOfInterest(x).ChromEnd > featureRow.ChromEnd) Then

                                featureHit.CountData += 1
                                featureHit.fStartData.Add(featureRow.ChromStart)
                                featureHit.fEndData.Add(featureRow.ChromEnd)
                                featureHit.StrandData.Add(featureRow.Strand)
                                featureHit.NameData.Add(featureRow.Name)
                                featureHit.ThresholdData.Add(featureRow.Threshold)
                                featureHit.OverLapTypeData.Add(Nothing)
                                featureHit.OverLapAmountData.Add(Nothing)
                                GenomicFeature.FeatureReturnedData(x) = featureHit
                            End If
                        Next
                        'NO OVERLAP: if data for this FOI & GenomicFeature has no hits, find closest GF to it & track its location.
                        If 0 = GenomicFeature.FeatureReturnedData(x).CountData And listFeatureSQLData.Count > 0 Then
                            featureHit = FindNearestRegion(proximityFeaturesOfInterest(x), listFeatureSQLData, Settings.ShortOnly)
                            GenomicFeature.FeatureReturnedData(x) = featureHit
                        End If
                    Next
            End Select
            Process_Regions(GenomicFeature, FeaturesOfInterest)
            Return GenomicFeature
        End Function


        'Processses the regions and classifies what type of overlap has occured.  Also returns the number of base pairs that the genomic feature and the feature of interest overlap
        Private Function Process_Regions(ByVal GenomicFeatures As GenomicFeature, ByRef FeaturesOfInterest As List(Of Feature)) As GenomicFeature
            Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
            For currFeature = 0 To NumOfFeatures - 1 Step +1 'cycles through the FOI
                Dim FOIstart As Integer = FeaturesOfInterest(currFeature).ChromStart                    'stores the chromStart of the current feature of interest
                Dim FOIEnd As Integer = FeaturesOfInterest(currFeature).ChromEnd                        'stores the chromEnd of the current feature of interest   
                Dim AmountOverlapStart As Integer 'amount that the feature overlaps at the beginning  
                Dim AmountOverlapEnd As Integer 'amount tha the feature overlaps at the end

                If GenomicFeatures.FeatureReturnedData(currFeature) IsNot Nothing Then
                    For currHit As Integer = 0 To GenomicFeatures.FeatureReturnedData(currFeature).CountData - 1 Step +1 'Cycles through the sub-arrays of the EvoFold in the case of multiple GF being returned for a region
                        Dim GFStart As Integer = GenomicFeatures.FeatureReturnedData(currFeature).fStartData(currHit)
                        Dim GFEnd As Integer = GenomicFeatures.FeatureReturnedData(currFeature).fEndData(currHit)
                        Dim FeatureLength As Integer = Val(FOIEnd) - Val(FOIstart) 'length of the feature
                        'Exact match
                        If FOIstart = GFStart And FOIEnd = GFEnd Then
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = FOIEnd - FOIstart + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "Exact"
                            'occures when startpoint = notContained, endpoint=contained
                        ElseIf FOIstart < GFStart And FOIEnd >= GFStart And FOIEnd < GFEnd Then
                            AmountOverlapStart = Val(FOIEnd) - Val(GFStart) + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = AmountOverlapStart
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "Overlap"
                            'occurs when startpoint = contained, endpoint=notContained
                        ElseIf FOIstart <= GFEnd And FOIstart > GFStart And FOIEnd > GFEnd Then
                            AmountOverlapEnd = Val(GFEnd) - Val(FOIstart) + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = AmountOverlapEnd
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "Overlap"
                            'occurs when both startpoint and endpoint are contained by the genomic feature
                        ElseIf FOIstart <> FOIEnd And FOIstart >= GFStart And FOIstart <= GFEnd And FOIEnd <= GFEnd Then
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = FOIEnd - FOIstart + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "Within"
                            'occurs when the the startpoint and endpoint are not contained or the feature end or startpoint is equal to the genomic feature start or endpoint while the other point is not contained 
                        ElseIf (FOIstart < GFStart And FOIEnd > GFEnd) Or _
                            (FOIstart = GFStart And FOIEnd > GFEnd) Or _
                            (FOIstart < GFStart And FOIEnd = GFEnd) Then
                            Dim AmountOverlapTotal As Integer = GFEnd - GFStart + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = AmountOverlapTotal
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "Overhang"
                            'occurs when there is only a SNP
                        ElseIf FOIstart = FOIEnd And (FOIstart >= GFStart And FOIEnd <= GFEnd) Then
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "SNPWithin"
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = 1
                            'occurs when the feature of interest is does not overlap with the GF and is less than genomic feature's startpoint
                        ElseIf (FOIstart < GFStart And FOIEnd < GFStart) Then
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "LeftNoOverlap"
                            AmountOverlapStart = GFStart - FOIEnd + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = -AmountOverlapStart
                            'occurs when the feature of interest is does not overlap with the GF and is greater than it's endpoint
                        ElseIf (FOIstart > GFEnd And FOIEnd > GFEnd) Then
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "RightNoOverlap"
                            AmountOverlapEnd = FOIstart - GFEnd + 1
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = -AmountOverlapEnd
                        Else
                            'TODO find info about nearest GF.
                            '     Mikhail's idea: if it gets to this point, put the current FOI into list of GF's then sort them.
                            '                     look at the GF's before & after this one to find which is closer.
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapTypeData(currHit) = "NA"
                            GenomicFeatures.FeatureReturnedData(currFeature).OverLapAmountData(currHit) = 0
                        End If
                    Next
                End If
            Next
            Return GenomicFeatures
        End Function

        'loads the GR feature data from the mysql database into memory for the GR features on the same chrom as the FOI.  The query is setup to only bring those features back which satisfy
        'the filters.
        Public Function Feature_Load_GRFeature_In_Memory(ByRef GFeature As GenomicFeature, ByVal Chrom As String, ByVal Threshold As Integer, ByVal PromoterUpstream As UInteger, ByVal PromoterDownstream As UInteger) As List(Of FeatureSQLData)
FeatureLoadStart:
            Try
                Dim GenomicFeatureDataBaseData As New List(Of FeatureSQLData) 'clears the list of data returned from the mysql database
                If GFeature.QueryType <> "Custom" Then 'custom tracks do not use the database

                    Dim StrandQuery As String = ""
                    Dim NameQuery As String = ""
                    GC.Collect()
                    'checks if the strand and name column exist
                    OpenDatabase()
                    'A workaround Promoter is not a table. Remove "Promoter" from tablename and use it to get strand/name, and to get actual Promoter data ("Promoter" query)
                    Dim GFeature_TableName As String = Replace(GFeature.TableName, "Promoter", vbNullString)
                    cmd = New MySqlCommand("SHOW COLUMNS FROM " & GFeature_TableName, cn)
                    dr = cmd.ExecuteReader()

                    'controls whether strand or should be outputed or not
                    Dim useNameUseStrand As String = "nonamenostrand"
                    Dim columnnames As String
                    While dr.Read()
                        columnnames &= dr(0)
                    End While
                    dr.Close() : cmd.Dispose()
                    Dim nameIndex As Integer = columnnames.ToLower().IndexOf("name")
                    Dim strandIndex As Integer = columnnames.ToLower().IndexOf("strand")
                    Dim repnameIndex As Integer = columnnames.ToLower().IndexOf("repname") 'In case of rmsk table we have repName column
                    If repnameIndex <> -1 Then nameIndex = -1 'If detected, name should not be used, set nameIndex manually to -1
                    If nameIndex <> -1 And strandIndex <> -1 Then : useNameUseStrand = "yesnameyesstrand" : End If
                    If nameIndex <> -1 And strandIndex = -1 Then : useNameUseStrand = "yesnamenostrand" : End If
                    If nameIndex = -1 And strandIndex <> -1 Then : useNameUseStrand = "nonameyesstrand" : End If
                    If nameIndex = -1 And strandIndex = -1 Then : useNameUseStrand = "nonamenostrand" : End If

                    'sets up the strand part fo the query if the genomic feature is set to be filtered by it
                    If GFeature.StrandToFilterBy <> "" Then
                        StrandQuery = " AND strand='" & GFeature.StrandToFilterBy & "'"
                    End If

                    'assembles all of the names to be analyzed into a query
                    If GFeature.NamesToInclude.Count <> 0 Then
                        NameQuery = "AND name IN ("
                        For Each name In GFeature.NamesToInclude
                            NameQuery &= "'" & name & "',"
                        Next
                        NameQuery = NameQuery.Remove(NameQuery.Length - 1) 'removes the last comma
                        NameQuery &= ")"
                    End If

                    'runs a different query depending on the query type that the feature is set to in the genomerunner table
                    Select Case GFeature.QueryType
                        Case Is = "Threshold"
                            If useNameUseStrand = "yesnameyesstrand" Then
                                OpenDatabase()
                                'returns the entire database and enters the values into the listFeatureData
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd," & GFeature.ThresholdType & ",strand,name FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & " AND " & GFeature.ThresholdType & ">='" & Threshold _
                                                       & "'" & StrandQuery & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Threshold = dr(3)
                                        data.Strand = dr(4)
                                        data.Name = dr(5)
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonameyesstrand" Then
                                'this is for the case that the table does not have a strand column
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd," & GFeature.ThresholdType & ",strand FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & " AND " & GFeature.ThresholdType & ">='" & Threshold _
                                                       & "'" & StrandQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Threshold = dr(3)
                                        data.Strand = dr(4)
                                        data.Name = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "yesnamenostrand" Then
                                'this is for the case that the table does not have a strand column
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd," & GFeature.ThresholdType & ",name FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & " AND " & GFeature.ThresholdType & ">='" & Threshold _
                                                       & "' " & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Threshold = dr(3)
                                        data.Strand = ""
                                        data.Name = dr(4)
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonamenostrand" Then
                                'this is for the case that the table does not have a strand column
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd," & GFeature.ThresholdType & " FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & " AND " & GFeature.ThresholdType & ">='" & Threshold _
                                                       & "';", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Name = ""
                                        data.Strand = ""
                                        data.Threshold = dr(3)
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            End If


                        Case Is = "General"
                            If useNameUseStrand = "yesnameyesstrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,strand,name,chromStart,chromEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Strand = dr(1)
                                        data.Name = dr(2)
                                        data.ChromStart = dr(3)
                                        data.ChromEnd = dr(4)
                                        data.Threshold = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "yesnamenostrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,name,chromStart,chromEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Name = dr(1)
                                        data.ChromStart = dr(2)
                                        data.ChromEnd = dr(3)
                                        data.Strand = ""
                                        data.Threshold = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonameyesstrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,strand,chromStart,chromEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Strand = dr(1)
                                        data.ChromStart = dr(2)
                                        data.ChromEnd = dr(3)
                                        data.Name = ""
                                        data.Threshold = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonamenostrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "';", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Strand = ""
                                        data.Name = ""
                                        data.Threshold = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            End If

                        Case Is = "OutputScore"
                            If useNameUseStrand = "yesnameyesstrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,strand,name,chromStart,chromEnd," & GFeature.ThresholdType & " FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Strand = dr(1)
                                        data.Name = dr(2)
                                        data.ChromStart = dr(3)
                                        data.ChromEnd = dr(4)
                                        data.Threshold = dr(5)
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "yesnamenostrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,name,chromStart,chromEnd," & GFeature.ThresholdType & " FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & NameQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Name = dr(1)
                                        data.ChromStart = dr(2)
                                        data.ChromEnd = dr(3)
                                        data.Threshold = dr(4)
                                        data.Strand = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonameyesstrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,strand,chromStart,chromEnd," & GFeature.ThresholdType & " FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.Strand = dr(1)
                                        data.ChromStart = dr(2)
                                        data.ChromEnd = dr(3)
                                        data.Threshold = dr(4)
                                        data.Name = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            ElseIf useNameUseStrand = "nonamenostrand" Then
                                OpenDatabase()
                                cmd = New MySqlCommand("SELECT chrom,chromStart,chromEnd," & GFeature.ThresholdType & " FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & ";", cn)
                                dr = cmd.ExecuteReader()
                                If dr.HasRows Then
                                    While dr.Read()
                                        Dim data As New FeatureSQLData
                                        data.Chrom = dr(0)
                                        data.ChromStart = dr(1)
                                        data.ChromEnd = dr(2)
                                        data.Threshold = dr(3)
                                        data.Name = ""
                                        data.Strand = ""
                                        GenomicFeatureDataBaseData.Add(data)
                                    End While
                                End If
                                dr.Close() : cmd.Dispose()
                            End If

                        Case Is = "GeneOnly"
                            OpenDatabase()
                            'returns the entire database and enters the values into the listFeatureData
                            cmd = New MySqlCommand("SELECT tName,tStart,tEnd FROM " & GFeature.TableName & " WHERE tName='" & Chrom & "'" & StrandQuery & ";", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    Dim data As New FeatureSQLData
                                    data.Chrom = dr(0)
                                    data.Strand = ""
                                    data.Name = ""
                                    data.ChromStart = dr(1)
                                    data.ChromEnd = dr(2)
                                    data.Threshold = ""
                                    GenomicFeatureDataBaseData.Add(data)
                                End While
                            End If
                            dr.Close() : cmd.Dispose()

                        Case Is = "Gene"
                            OpenDatabase()
                            'returns the entire database and enters the values into the listFeatureData
                            cmd = New MySqlCommand("SELECT chrom,strand,name,txStart,txEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & NameQuery & ";", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    Dim data As New FeatureSQLData
                                    data.Chrom = dr(0)
                                    data.Strand = dr(1)
                                    data.Name = dr(2)
                                    data.ChromStart = dr(3)
                                    data.ChromEnd = dr(4)
                                    data.Threshold = ""
                                    GenomicFeatureDataBaseData.Add(data)
                                End While
                            End If
                            dr.Close() : cmd.Dispose()

                            'populates the dictionary of gene name conversions
                            kgIDToGeneSymbolDict.Clear()
                            OpenDatabase()
                            cmd = New MySqlCommand("Select kgID,geneSymbol,mRNA FROM kgxref;", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    kgIDToGeneSymbolDict.Add(dr(0), dr(1)) 'sets the gene name equal to the result
                                End While
                            End If
                            dr.Close()
                            cn.Close() : cmd.Dispose()
                            'sets the start and endpoint of the promoter region
                            For Each featuresql In GenomicFeatureDataBaseData
                                'compares the idname of the gene against the kgxref table and finds the standard gene name
                                Dim containskey As Boolean = kgIDToGeneSymbolDict.ContainsKey(featuresql.Name)
                                If containskey = True Then
                                    featuresql.Threshold = kgIDToGeneSymbolDict(featuresql.Name)
                                End If
                            Next
                        Case Is = "Promoter"
                            OpenDatabase()
                            'returns the entire database and enters the values into the listFeatureData
                            cmd = New MySqlCommand("SELECT chrom,strand,name,txStart,txEnd FROM " & GFeature_TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & NameQuery & ";", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    Dim data As New FeatureSQLData
                                    data.Chrom = dr(0)
                                    data.Strand = dr(1)
                                    data.Name = dr(2)
                                    data.ChromStart = dr(3)
                                    data.ChromEnd = dr(4)
                                    data.Threshold = ""
                                    GenomicFeatureDataBaseData.Add(data)
                                End While
                            End If
                            dr.Close() : cmd.Dispose()
                            'sets the promoter region's start and endpoint based on the user's input

                            'populates the dictionary of gene name conversions
                            kgIDToGeneSymbolDict.Clear()
                            OpenDatabase()
                            cmd = New MySqlCommand("Select kgID,geneSymbol,mRNA FROM kgxref;", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()

                                    kgIDToGeneSymbolDict.Add(dr(0), dr(1)) 'sets the gene name equal to the result
                                End While
                            End If
                            dr.Close()
                            cn.Close() : cmd.Dispose()
                            Dim fstart As Integer 'stores the startpoint 
                            Dim fend As Integer 'stores the endpoint before the data is overwritten

                            'sets the start and endpoint of the promoter region based off of the gene's start and endpoint
                            For Each featuresql In GenomicFeatureDataBaseData
                                If featuresql.Strand = "+" Then
                                    fstart = featuresql.ChromStart
                                    featuresql.ChromEnd = fstart + PromoterDownstream
                                    featuresql.ChromStart = fstart - PromoterUpstream
                                    If featuresql.ChromStart < 0 Then
                                        featuresql.ChromStart = 0
                                    End If
                                    'featuresql.ChromEnd = fstart + 1000
                                    'featuresql.ChromStart = fstart - 5000
                                    ''checks if the featurestart is less than 0, if so than it makes the startpoint = 0
                                    'If featuresql.ChromStart < 0 Then
                                    '    featuresql.ChromStart = 0
                                    'End If
                                ElseIf featuresql.Strand = "-" Then
                                    fend = featuresql.ChromEnd
                                    featuresql.ChromEnd = fend + PromoterUpstream
                                    featuresql.ChromStart = fend - PromoterDownstream
                                    If featuresql.ChromStart < 0 Then
                                        featuresql.ChromStart = 0
                                    End If
                                    'featuresql.ChromEnd = fend + GFFeature.Threshold
                                    'featuresql.ChromStart = fend - 1000
                                    'featuresql.ChromEnd = fend + 5000
                                    'If featuresql.ChromStart < 0 Then
                                    '    featuresql.ChromStart = 0
                                    'End If
                                End If
                                'compares the genebank name to the Gene name in the kgxref  table and saves it to the threshold
                                Dim containskey As Boolean = kgIDToGeneSymbolDict.ContainsKey(featuresql.Name)
                                If containskey = True Then
                                    featuresql.Threshold = kgIDToGeneSymbolDict(featuresql.Name)
                                End If
                            Next

                        Case Is = "Exon"
                            OpenDatabase()
                            GenomicFeatureDataBaseData.Clear() 'clears the list of data returned from the mysql database
                            GC.Collect()
                            'returns the entire database and enters the values into the listFeatureData
                            cmd = New MySqlCommand("SELECT chrom,strand,name,exonStart,exonEnd FROM " & GFeature.TableName & " WHERE chrom='" & Chrom & "'" & StrandQuery & NameQuery & ";", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    Dim data As New FeatureSQLData
                                    data.Chrom = dr(0)
                                    data.Strand = dr(1)
                                    data.Name = dr(2)
                                    data.ChromStart = dr(3)
                                    data.ChromEnd = dr(4)
                                    data.Threshold = ""
                                    GenomicFeatureDataBaseData.Add(data)
                                End While
                            End If
                            dr.Close() : cmd.Dispose()

                            'populates the dictionary of gene name conversions
                            kgIDToGeneSymbolDict.Clear()

                            OpenDatabase()
                            cmd = New MySqlCommand("Select kgID,geneSymbol,mRNA FROM kgxref;", cn)
                            dr = cmd.ExecuteReader()
                            If dr.HasRows Then
                                While dr.Read()
                                    kgIDToGeneSymbolDict.Add(dr(0), dr(1)) 'sets the gene name equal to the result
                                End While
                            End If
                            dr.Close()
                            cn.Close() : cmd.Dispose()
                            'sets the start and endpoint of the promoter region
                            For Each featuresql In GenomicFeatureDataBaseData
                                'compares the idname of the gene against the kgxref table and finds the standard gene name
                                Dim containskey As Boolean = kgIDToGeneSymbolDict.ContainsKey(featuresql.Name)
                                If containskey = True Then
                                    featuresql.Threshold = kgIDToGeneSymbolDict(featuresql.Name)
                                End If
                            Next
                    End Select
                Else
                    'reads the .bed file and loads the Genomic Feature for the current chrom into memory 
                    Dim SR As New StreamReader(GFeature.TableName)
                    While SR.EndOfStream = False
                        Dim lineArray As String() = SR.ReadLine.Split(vbTab)
                        Dim data As New FeatureSQLData
                        If lineArray(0) = Chrom Then 'checks if the current GF region is on the chrom being analyzed
                            data.Chrom = lineArray(0)
                            data.ChromStart = lineArray(1)
                            data.ChromEnd = lineArray(2)
                            data.Strand = ""
                            data.Name = ""
                            data.Threshold = ""
                            GenomicFeatureDataBaseData.Add(data)
                        End If
                    End While
                End If
                Return GenomicFeatureDataBaseData
            Catch e As Exception
                If cn.State = ConnectionState.Open Then : cn.Close() : End If           'closes the connection if it's open
                'MessageBox.Show("There was an error retrieving the genomic feature data from the server." & vbCrLf & "Retrying to load data" & vbCrLf & e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                GoTo FeatureLoadStart
            End Try
        End Function

        Function FindNearestRegion(ByVal FOI As Feature, ByVal SQLData As List(Of FeatureSQLData), ByVal shortOnlyChecked As Boolean) As FeaturesReturnedHits
            'put FOI into list of rows, keep track of where it is
            Dim FOIasFeatureSQLDataType As New FeatureSQLData
            FOIasFeatureSQLDataType.Chrom = FOI.Chrom
            FOIasFeatureSQLDataType.ChromEnd = FOI.ChromEnd
            FOIasFeatureSQLDataType.ChromStart = FOI.ChromStart
            FOIasFeatureSQLDataType.Name = FOI.Name
            SQLData.Add(FOIasFeatureSQLDataType)
            'sort rows by start point
            SQLData.Sort(Function(r1, r2) r1.ChromStart.CompareTo(r2.ChromStart))

            'look at rows before & after
            Dim foiIndex As Integer = SQLData.IndexOf(FOIasFeatureSQLDataType)

            'add info for closest left & right regions
            Dim featureHit As New FeaturesReturnedHits
            Dim leftRegions As New List(Of FeatureSQLData) 'need to sort these by end point rather than start point like the rest.
            Dim leftRegion As New FeatureSQLData
            Dim rightRegion As New FeatureSQLData
            Dim leftOverlap As Integer = -1
            Dim rightOverlap As Integer = -1
            If (foiIndex - 1) >= 0 Then
                leftRegions.Clear()
                For i As Integer = 0 To foiIndex - 1 Step +1
                    leftRegions.Add(SQLData(i))
                Next
                'sort rows by end point since we're on left side; want to find end pt closest to FOI.
                leftRegions.Sort(Function(r1, r2) r1.ChromEnd.CompareTo(r2.ChromEnd))
                leftRegion = leftRegions.Last
                leftOverlap = FOIasFeatureSQLDataType.ChromStart - leftRegion.ChromEnd
                featureHit.fEndData.Add(leftRegion.ChromEnd)
                featureHit.fStartData.Add(leftRegion.ChromStart)
                featureHit.NameData.Add(leftRegion.Name)
                featureHit.OverLapTypeData.Add("Left-No overlap")
                featureHit.OverLapAmountData.Add(leftOverlap)
                featureHit.StrandData.Add(leftRegion.Strand)
                featureHit.ThresholdData.Add(leftRegion.Threshold)
            End If
            If (foiIndex + 1) < (SQLData.Count - 1) Then
                'RIGHT region
                rightRegion = SQLData(foiIndex + 1)
                rightOverlap = rightRegion.ChromStart - FOIasFeatureSQLDataType.ChromEnd
                featureHit.fEndData.Add(rightRegion.ChromEnd)
                featureHit.fStartData.Add(rightRegion.ChromStart)
                featureHit.NameData.Add(rightRegion.Name)
                featureHit.OverLapTypeData.Add("Right-No overlap")
                featureHit.OverLapAmountData.Add(rightOverlap)
                featureHit.StrandData.Add(rightRegion.Strand)
                featureHit.ThresholdData.Add(rightRegion.Threshold)
            End If

            'get rid of longer region if "Short Only" checkbox is checked.
            If shortOnlyChecked = True And featureHit.fStartData.Count = 2 Then
                Dim indexToRemove As New Integer
                If leftOverlap < rightOverlap Then
                    indexToRemove = 1
                Else
                    indexToRemove = 0
                End If
                featureHit.fEndData.Remove(featureHit.fEndData(indexToRemove))
                featureHit.fStartData.Remove(featureHit.fStartData(indexToRemove))
                featureHit.NameData.Remove(featureHit.NameData(indexToRemove))
                featureHit.OverLapTypeData.Remove(featureHit.OverLapTypeData(indexToRemove))
                featureHit.OverLapAmountData.Remove(featureHit.OverLapAmountData(indexToRemove))
                featureHit.StrandData.Remove(featureHit.StrandData(indexToRemove))
                featureHit.ThresholdData.Remove(featureHit.ThresholdData(indexToRemove))
            End If
            'remove FOI from list of rows
            SQLData.Remove(FOIasFeatureSQLDataType)
            Return featureHit
        End Function

    End Class
End Namespace