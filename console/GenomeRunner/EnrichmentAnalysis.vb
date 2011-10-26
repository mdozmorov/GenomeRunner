'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports alglib
Imports System.IO
Namespace GenomeRunner
    Public Delegate Sub ProgressStart(ByVal Total As Integer)
    Public Delegate Sub ProgressUpdate(ByVal CurrItem As Integer, ByVal CurrFeaturesOfInterestName As String, ByVal GenomicFeatureName As String)
    Public Delegate Sub ProgressDone(ByVal outputDir As String)

    'stores the settings that will be used when running the enrichment analysis
    Public Class EnrichmentSettings
        Public ConnectionString As String                                                           'the connection string that is used to connect to the database containing the genomic feature data
        Public NumMCtoRun As Integer                                                                'the number of Monte Carlo simulations to run
        Public PvalueThreshold As Double                                                            'the threshold at which pvalues are considered significant
        Public EnrichmentJobName As String                                                          'the user supplied name of the enrichment analysis
        Public BackgroundName As String                                                             'the name of the background that was used to calculate random associations
        Public UseSpotBackground As Boolean                                                         'whether a spotbackground should be used to calculate random associations
        Public UseMonteCarlo As Boolean                                                             'whether Monte Carlo simulations should be used to calculate the number of associations expected by random chance
        Public UseAnalytical As Boolean                                                             'whether the analytical method should be used to calculate the number of associations expected by random chance
        Public UseChiSquare As Boolean                                                              'whether the Chi-Square test should be used to calculate the p-value
        Public UseBinomialDistribution As Boolean                                                   'whether binomial distrobution should be used to calculate the p-value
        Public OutputPercentOverlapPvalueMatrix As Boolean                                          'whether a matrix should be outputed where pvalues are weighted by percent overlap
        Public SquarePercentOverlap As Boolean                                                      'whether the percent overlap weight should be sqaured before being applyed 
        Public OutputPCCweightedPvalueMatrix As Boolean                                             'whether a matrix should be outputed where pvalues are weighted by the pearson's contingency coefficient 
        Public PearsonsAudjustment As UInteger                                                       'this number is used to audjust the pearson's contingency coefficient for the matrix output
        Public OutputDir As String                                                                  'the directory to which the results of the enrichment analysis should be outputed to
        Public FilterLevel As String                                                                'used to store what level the filters were at so it can be recorded in the log file
        Public PromoterUpstream As UInteger = 0                                                       'stores how many base pairs the promoter region begins upstream of the gene start point 
        Public PromoterDownstream As UInteger = 0                                                  'stores how many base pairs the promoter regions covers downstream of the gene's startpoint
        Public Proximity As UInteger = 0                                                              'the number of basepairs that a feature of interest can be away from a genomic feature and still be considered a hit.  this value is NOT taken into consideration when calculating the overlap type

        Public Sub New(ByVal ConnectionString As String, ByVal EnrichmentJobName As String, ByVal OutputDir As String, ByVal UseMonteCarlo As Boolean, ByVal UseAnalytical As Boolean, ByVal UseChiSquare As Boolean, ByVal UseBinomialDistribution As Boolean, ByVal OutputPercentOverlapPvalueMatrix As Boolean, ByVal SquarePercentOverlap As Boolean, ByVal OutputPCCweightedPvalueMatrix As Boolean, ByVal PearsonsAudjustment As Integer, ByVal BackGroundName As String, ByVal UseSpotBackground As Boolean, ByVal NumMCtoRun As Integer, ByVal PValueThreshold As Double, ByVal FilterLevel As String, ByVal PromoterUpstream As UInteger, ByVal PromoterDownstream As UInteger, ByVal proximity As UInteger)
            Me.ConnectionString = ConnectionString
            Me.EnrichmentJobName = EnrichmentJobName
            Me.UseMonteCarlo = UseMonteCarlo
            Me.UseAnalytical = UseAnalytical
            Me.OutputPercentOverlapPvalueMatrix = OutputPercentOverlapPvalueMatrix
            Me.SquarePercentOverlap = SquarePercentOverlap
            Me.OutputPCCweightedPvalueMatrix = OutputPCCweightedPvalueMatrix
            Me.PearsonsAudjustment = PearsonsAudjustment
            Me.UseChiSquare = UseChiSquare
            Me.UseBinomialDistribution = UseBinomialDistribution
            Me.UseSpotBackground = UseSpotBackground
            Me.BackgroundName = BackGroundName
            Me.NumMCtoRun = NumMCtoRun
            Me.PvalueThreshold = PValueThreshold
            Me.OutputDir = OutputDir
            Me.FilterLevel = FilterLevel
            Me.Proximity = proximity
            Me.PromoterUpstream = PromoterUpstream
            Me.PromoterDownstream = PromoterDownstream
        End Sub
    End Class


    Public Class EnrichmentAnalysis
        Dim ConnectionString As String
        Dim progStart As ProgressStart                                                                      'used to set initial progress
        Dim progUpdate As ProgressUpdate                                                                    'used to update progress
        Dim progDone As ProgressDone                                                                    'used return progress complete

        Public Sub New(ByVal ConnectionString As String, ByVal progStart As ProgressStart, ByVal progUpdate As ProgressUpdate, ByVal progDone As ProgressDone)
            Me.ConnectionString = ConnectionString
            Me.progStart = progStart
            Me.progUpdate = progUpdate
            Me.progDone = progDone
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

        'runs an enrichment analysis for a set of feature of interest files.  The paths of the files are passed on and this method reads the contents of the files into memory and runs an enrichment analysis for the genomic features that are passed on
        'as a list
        Public Sub RunEnrichmentAnlysis(ByVal FeatureOfInterestFilePaths As List(Of String), ByVal GenomicFeatures As List(Of GenomicFeature), ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings)
            Directory.CreateDirectory(Settings.OutputDir)
            Dim FeaturesOfInterest As List(Of Feature)
            Dim OutputMatrixColumnHeaders As Boolean = True

            'goes through each filepath and runs an enrichment analysis on the features in the file
            For Each FeatureFilePath In FeatureOfInterestFilePaths
                FeaturesOfInterest = LoadFeatureOfInterests(FeatureFilePath)
                FeaturesOfInterest = OrganizeFeaturesByChrom(FeaturesOfInterest)
                Dim Outputer As New Output(FeaturesOfInterest.Count)
                Dim isFirstPvalue As Boolean = True                                                                                    'Whether the general information for the feature file should be outputed
                Dim currGF As Integer = 0
                'runs the features of interest against the genomic features that were selected to be run
                progStart.Invoke(GenomicFeatures.Count)
                For Each GF In GenomicFeatures

                    progUpdate.Invoke(currGF, Path.GetFileName(FeatureFilePath), GF.Name)
                    'uses either monte carlo or the analytical method for the enrichment analysis
                    If Settings.UseMonteCarlo = True Then
                        Debug.Print("Running initial analysis MonteCarlo for " & GF.Name)
                        GF = Calculate_PValue_MonteCarlo(GF, FeaturesOfInterest, Background, Settings)
                    End If
                    If Settings.UseAnalytical = True Then
                        GF = calculatePValueUsingAnalyticalMethod(GF, FeaturesOfInterest, Background, Settings)
                    End If
                    Outputer.OutputPvalueLogFile(isFirstPvalue, GF, Settings, Path.GetFileNameWithoutExtension(FeatureFilePath))           'results are added on to the log file after each genomic feature is analyzed
                    GF.FeatureReturnedData.Clear()
                    isFirstPvalue = False
                    currGF += 1
                Next

                Outputer.OutputPValueMatrix(Settings.OutputDir, GenomicFeatures, Settings, _
                                            OutputMatrixColumnHeaders, Path.GetFileNameWithoutExtension(FeatureFilePath))                  'the matrix is is outputed, the matrix is ouputed after all of the genomic features have been analyzed

                'Uncomment to additionally output percent weighted matrix
                'Settings.OutputPercentOverlapPvalueMatrix = True
                'Outputer.OutputPValueMatrix(Settings.OutputDir, GenomicFeatures, Settings, _
                '            OutputMatrixColumnHeaders, Path.GetFileNameWithoutExtension(FeatureFilePath))                  'the matrix is is outputed, the matrix is ouputed after all of the genomic features have been analyzed
                'Settings.OutputPercentOverlapPvalueMatrix = False

                'Settings.SquarePercentOverlap = True
                'Outputer.OutputPValueMatrix(Settings.OutputDir, GenomicFeatures, Settings, _
                '                            OutputMatrixColumnHeaders, Path.GetFileNameWithoutExtension(FeatureFilePath))                  'the matrix is is outputed, the matrix is ouputed after all of the genomic features have been analyzed

                OutputMatrixColumnHeaders = False
            Next
            progDone.Invoke(Settings.OutputDir)
        End Sub

        'creates features of interest with extended start and endpoints for proximity analysis
        Private Function CreateproximityFeaturesOfInterest(ByVal FeaturesOfInterest As List(Of Feature), ByVal proximity As UInteger) As List(Of Feature)
            Dim FeaturesOfInterestproximity As New List(Of Feature)
            For Each FOI In FeaturesOfInterest
                Dim nFOI As New Feature
                nFOI.Chrom = FOI.Chrom
                nFOI.ChromStart = FOI.ChromStart - proximity
                nFOI.ChromEnd = FOI.ChromEnd + proximity
                FeaturesOfInterestproximity.Add(nFOI)
                'Debug.Print(FOI.ChromEnd - FOI.ChromStart & vbTab & nFOI.ChromEnd - nFOI.ChromStart)
            Next
            Return FeaturesOfInterestproximity
        End Function


        'calculates the pvalue using monte carlo
        Private Function Calculate_PValue_MonteCarlo(ByRef GFeature As GenomicFeature, ByRef FeaturesOfInterest As List(Of Feature), ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings)
            Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
            Dim mean As Double, variance As Double, skewness As Double, kurtosis As Double 't2 As Double, lt As Double, rt As Double
            Dim currentTime As System.DateTime = System.DateTime.Now  'used in the header of the output
            Dim ObservedWithin As Integer = 0, ObservedOutside As Integer = 0
            Dim randEventsCountMC(Settings.NumMCtoRun - 1) As Double                                    'Counters for features within one/within all simulations
            Dim AnnoSettings As New AnnotationSettings(Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity)
            Dim fAnalysis As New GenomeRunner.AnnotationAnalysis(Settings.ConnectionString)
            Dim FeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(FeaturesOfInterest, Settings.Proximity)

            GFeature.FeatureReturnedData.Clear()
            GFeature = fAnalysis.Feature_Analysis(GFeature, FeaturesOfInterestproximity, FeaturesOfInterest, AnnoSettings) 'runs an initial analysis so that the observed within can be determined

            'cycles through each of the features run and gets the number of hits for the FOI
            For x As Integer = 0 To NumOfFeatures - 1 Step +1                                  'Now calculate number of true observations
                If GFeature.FeatureReturnedData(x).CountData <> 0 Then                             'LC 6/20/11 changed to account for new EvoFold structure
                    ObservedWithin += 1
                Else
                    ObservedOutside += 1
                End If
            Next
            GFeature.ActualHits = ObservedWithin
            For i As Integer = 0 To Settings.NumMCtoRun - 1
                Debug.Print("Running MC run# " & i + 1 & " of " & Settings.NumMCtoRun & " " & TimeOfDay)
                Dim RandomFeatures As List(Of Feature) = createRandomRegions(FeaturesOfInterest, Background, Settings.UseSpotBackground) 'generates a random features of interest 
                Dim RandomFeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(RandomFeatures, Settings.Proximity)

                GFeature.FeatureReturnedData.Clear() 'clears the returned data of the previous run
                'For Each rFOI In RandomFeaturesOfInterestproximity
                '    Debug.Print(rFOI.Chrom & vbTab & rFOI.ChromStart & vbTab & rFOI.ChromEnd)
                'Next
                OrganizeFeaturesByChrom(RandomFeaturesOfInterestproximity)
                ' Debug.Print("Organized by chrom: ")
                'For Each rFOI In RandomFeaturesOfInterestproximity
                '    Debug.Print(rFOI.Chrom & vbTab & rFOI.ChromStart & vbTab & rFOI.ChromEnd)
                'Next
                GFeature = fAnalysis.Feature_Analysis(GFeature, RandomFeaturesOfInterestproximity, RandomFeatures, AnnoSettings)     'analizes the randomfeatures. 
                Dim ExpectedWithin As Integer = 0, ExpectedOutside As Integer = 0                               'Zero out counters for each Monte-Carlo run
                For x = 0 To NumOfFeatures - 1                                'Calculate statistics for them
                    If GFeature.FeatureReturnedData(x).CountData <> 0 Then                     'If random alternative event observed
                        ExpectedWithin += 1
                    Else
                        ExpectedOutside += 1
                    End If
                Next

                randEventsCountMC(i) = ExpectedWithin                              'Store simulation results
            Next

            Dim total As Integer = 0
            For i As Integer = 0 To randEventsCountMC.Count - 1
                total += randEventsCountMC(i)
            Next

            samplemoments(randEventsCountMC, mean, variance, skewness, kurtosis)
            GFeature.MCMean = mean : GFeature.MCvariance = variance : GFeature.MCskewness = skewness : GFeature.MCkurtosis = kurtosis
            If Settings.UseMonteCarlo = True Then
                GFeature.PValueMonteCarloChisquare = pValueChiSquare(ObservedWithin, randEventsCountMC.Average, NumOfFeatures) 'calculates the pvalue using chisquare
            End If
            If Settings.UseBinomialDistribution = True Then
                ' GFeature.PValueMonteCarloBinomialDistribution =  Return alglib.binomialdistribution(HasHit.Sum(), HasHit.Length, p)
            End If
            GFeature.PCCMonteCarloChiSquare = PearsonsContigencyCoeffcient(ObservedWithin, randEventsCountMC.Average, NumOfFeatures) 'calculates the pearson's congingency coefficient 
            GFeature.MCExpectedHits = randEventsCountMC.Average
            GFeature.ActualHits = ObservedWithin
            Return GFeature
        End Function


        Private Function calculatePValueUsingAnalyticalMethod(ByVal GFeature As GenomicFeature, ByVal Features As List(Of Feature), ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings) As GenomicFeature
            'runs a normal analysis on the feature
            Debug.Print("Running initial analysis the Analytical Method for " & GFeature.Name & " " & TimeOfDay)
            Dim AnoSettings As New AnnotationSettings(Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity)
            Dim fAnalysis As New AnnotationAnalysis(ConnectionString)
            Dim FeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(Features, Settings.Proximity)
            GFeature = fAnalysis.Feature_Analysis(GFeature, FeaturesOfInterestproximity, Features, AnoSettings)
            Dim wA(GFeature.FeatureReturnedData.Count - 1) As Integer 'width of each FOI
            Dim hA(GFeature.FeatureReturnedData.Count - 1) As Integer 'stores whether the FOI was a hit or miss 
            For currFOI As Integer = 0 To GFeature.FeatureReturnedData.Count - 1
                'sets hA to true(for at least one hit) or false (no hit) for each FOI  
                If GFeature.FeatureReturnedData(currFOI).CountData <> 0 Then
                    hA(currFOI) = 1
                Else
                    hA(currFOI) = 0
                End If
                'stores the width of the FOI
                wA(currFOI) = Features(currFOI).ChromEnd + 1 - Features(currFOI).ChromStart
            Next

            'counts the number of actual hits
            Dim ActualHits As Long = 0
            For Each FOIhit In hA
                If FOIhit = 1 Then
                    ActualHits += 1
                End If
            Next

            Debug.Print("Calculating B and nB " & GFeature.Name & " " & TimeOfDay)
            If GFeature.AnalyG = 0 Or GFeature.AnalynB = 0 Or GFeature.AnalyB = 0 Then
                Calculate_Base_Region_Count(GFeature, Background, Settings, GFeature.AnalyG, GFeature.AnalyB, GFeature.AnalynB)
            End If
            Debug.Print("Finished calculating. B=" & GFeature.AnalyB & " G=" & GFeature.AnalyG & " nB=" & GFeature.AnalynB & "for " & GFeature.Name & " " & TimeOfDay)

            'calculates the pvalue using the tests that have been selected (ex. chi-square)
            'if B and nB are 0 than the p value is automatically set to 1 as there is only one outcome and the pvalue cannot be calculated as it results in a 0/0 error
            If GFeature.AnalynB <> 0 Or GFeature.AnalynB <> 0 Then
                Dim PValueCalc As New AnalyticalPVCalculator(GFeature.AnalyB, GFeature.AnalynB, GFeature.AnalyG)
                GFeature.AnalyticalExpectedWithin = PValueCalc.nExpectedHits(wA)
                If Settings.UseBinomialDistribution = True Then
                    GFeature.PValueAnalyticalBinomialDistribution = PValueCalc.pValuebinomialcdistribution(wA, hA)
                End If
                If Settings.UseChiSquare = True Then
                    GFeature.PValueAnalyticalChisquare = pValueChiSquare(ActualHits, GFeature.AnalyticalExpectedWithin, Features.Count)
                End If
                GFeature.PCCAnalyticalChiSquare = PearsonsContigencyCoeffcient(ActualHits, GFeature.AnalyticalExpectedWithin, Features.Count)
            Else
                GFeature.PValueAnalyticalBinomialDistribution = 1
                GFeature.PValueAnalyticalChisquare = 1
            End If

            GFeature.ActualHits = ActualHits
            Return GFeature
        End Function

        'calcualtes number of base pairs and number of regions that the Genomic Feature contains as well as the total number of base pairs for the background
        Private Sub Calculate_Base_Region_Count(ByVal GenomicFeature As GenomicFeature, ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings, ByRef TotalBasePairs As Double, ByRef NumGenomicFeatureBasePairs As Double, ByRef NumGenomicFeatureRegion As Double)
            Dim TableNames As New List(Of String)
            Dim TotalNumberofBasePairs As Long = 0
            Dim AnoSettings As New AnnotationSettings(Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity)
            Dim Analyzer As New AnnotationAnalysis(ConnectionString)                                                         'used to get the genomic feature data from the mysql database
            'gets the total number of base pairs of the background
            For Each interval In Background
                TotalNumberofBasePairs += interval.ChromEnd - interval.ChromStart + 1                                        'adds one to the interval length so that number of bases in the interval is counted rather than the length of the interval
            Next

            Dim totalFeatureBasePair As Long = 0                                                                             'keeps count of the total number of base pairs that are covered by the feature
            Dim TotalFeatureRegionCount As Long = 0                                                                          'keeps count of the total number of regions that are 'created' by superimposing the feature regions onto the basepairs of the chromosome
            Dim debugRoCount As Integer = 0
            For currInterval As Integer = 0 To Background.Count - 1

                Dim GenomicFeaturesSQLData As List(Of FeatureSQLData) _
                    = Analyzer.Feature_Load_GRFeature_In_Memory(GenomicFeature, Background(currInterval).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream) 'loads GF into memory so that its bps and regions can be counted
                debugRoCount += GenomicFeaturesSQLData.Count
                Dim NumFeatureBasePair As Integer = Background(currInterval).ChromEnd - Background(currInterval).ChromStart + 1 'the number of base pairs in the interval
                Dim BasePairs(NumFeatureBasePair) As Boolean                                                                    'an boolean array that is that has a value for each base pair in the interval and is used to keep track of base pairs that are covered by a genomic feature

                'goes through each base pair in the interval and changes the isHit values of the base pair to true for all base
                'pairs that are covered by the genomic region
                For Each currFeature In GenomicFeaturesSQLData
                    For CurrBase As UInteger = currFeature.ChromStart To currFeature.ChromEnd
                        If CurrBase >= NumFeatureBasePair Then
                            '...if the bp is located outside of the chromosome, then it is set to equal to the feature end point.
                            CurrBase = currFeature.ChromEnd
                        Else
                            BasePairs(CurrBase) = True
                        End If
                    Next
                Next


                Dim LastBaseHit As Boolean = False                                                                              'keeps track of whether the last base pair was a hit or not
                'counts the number of regions of base pairs that are covered by genomic features
                For CurrBase As Integer = 0 To BasePairs.Count - 1
                    If BasePairs(CurrBase) = True Then
                        totalFeatureBasePair += 1
                        If LastBaseHit = False Then
                            TotalFeatureRegionCount += 1
                            LastBaseHit = True
                        End If
                    Else
                        LastBaseHit = False
                    End If
                Next
            Next
            Debug.Print("Rows returned for A and B calc: " & debugRoCount)
            TotalBasePairs = TotalNumberofBasePairs
            NumGenomicFeatureRegion = TotalFeatureRegionCount
            NumGenomicFeatureBasePairs = totalFeatureBasePair
        End Sub

        Private Function ChromAlreadyInList(ByVal feature As Feature, ByVal chrom As String) As Boolean
            If feature.Chrom = chrom Then
                Return True
            Else
                Return False
            End If
        End Function

        'generates randomfeatures from the background based on the length of the FOIs passed on to it
        Private Function createRandomRegions(ByVal FeaturesOfInterest As List(Of Feature), ByVal BackgroundInterval As List(Of Feature), ByVal UseSpot As Boolean)
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
                    CurrBkgChr = hqrnduniformi(state, BackgroundInterval.Count)                         'Select random interval from the background
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

        Public Function PearsonsContigencyCoeffcient(ByVal ObservedWithin As Double, ByVal ExpectedWithinMean As Double, ByVal NumOfFeatures As Integer) As Double
            Dim c As Double
            Dim Pval, Row1, Row2, Col1, Col2, N, chi2, ExpMatrix(2, 2) As Double, ChiMatrix(2, 2) As Double
            Dim ObsWithinChi, RandWithinChi, NumOfExptsChi, NumOfRandChi As Double
            ObsWithinChi = ObservedWithin : RandWithinChi = ExpectedWithinMean : NumOfExptsChi = NumOfFeatures : NumOfRandChi = NumOfFeatures
            Pval = 1
            If ObservedWithin = 0 And ExpectedWithinMean = 0 Then
                Return c
            End If
            Row1 = NumOfExptsChi
            Row2 = NumOfRandChi
            Col1 = ObsWithinChi + RandWithinChi
            Col2 = NumOfExptsChi + NumOfRandChi - (ObsWithinChi + RandWithinChi)
            N = NumOfExptsChi + NumOfRandChi
            'calculate expected values for each cell
            ExpMatrix(1, 1) = (Row1 * Col1) / N
            ExpMatrix(1, 2) = (Row1 * Col2) / N
            ExpMatrix(2, 1) = (Row2 * Col1) / N
            ExpMatrix(2, 2) = (Row2 * Col2) / N
            'transform into chi-square values table = (o-e)^2 / e
            ChiMatrix(1, 1) = (ObsWithinChi - ExpMatrix(1, 1)) ^ 2 / ExpMatrix(1, 1)
            ChiMatrix(1, 2) = (NumOfExptsChi - ObsWithinChi - ExpMatrix(1, 2)) ^ 2 / ExpMatrix(1, 2)
            ChiMatrix(2, 1) = (RandWithinChi - ExpMatrix(2, 1)) ^ 2 / ExpMatrix(2, 1)
            ChiMatrix(2, 2) = (NumOfRandChi - RandWithinChi - ExpMatrix(2, 2)) ^ 2 / ExpMatrix(2, 2)
            'calculate x^2 = sum(o-e)^2 / e
            chi2 = ChiMatrix(1, 1) + ChiMatrix(1, 2) + ChiMatrix(2, 1) + ChiMatrix(2, 2)
            Try
                Pval = chisquarecdistribution(1, chi2)                                                              'this method does not require that we subtract it from 1
            Catch
                Pval = 1
            End Try
            Debug.WriteLine("Num of Features: " & NumOfFeatures)
            Debug.WriteLine("Observed : " & ObservedWithin)
            Debug.WriteLine("Expected : " & ExpectedWithinMean)
            Debug.WriteLine("Chi^2 " & chi2)
            c = math.sqr(chi2 / (2 * NumOfFeatures + chi2))
            Return c
        End Function

        'rearanges the FOI by chromosome and returns them
        Private Function OrganizeFeaturesByChrom(ByVal Features As List(Of Feature))
            Dim featuresIndexes As New List(Of Integer)

            'gets one of each chromosome that contain FOIs
            Dim UniqueChroms As New HashSet(Of String)
            For Each FOI In Features
                If Not UniqueChroms.Contains(FOI.Chrom) Then
                    UniqueChroms.Add(FOI.Chrom)
                End If
            Next
            'Temporarily holds the Features as they are rearanged.   
            Dim tempChrom As New List(Of String)
            Dim tempChromStart As New List(Of Integer)
            Dim tempChromEnd As New List(Of Integer)
            Dim tempName As New List(Of String)
            For Each uniqueChrom In UniqueChroms
                For j As Integer = 0 To Features.Count - 1
                    If Features(j).Chrom = uniqueChrom Then
                        featuresIndexes.Add(j) 'stores the index position of the FOI so that it can be reordered after analysis
                        tempChrom.Add(Features(j).Chrom)
                        tempChromStart.Add(Features(j).ChromStart)
                        tempChromEnd.Add(Features(j).ChromEnd)
                        If Features(j).Name IsNot Nothing Then : tempName.Add(Features(j).Name) : End If 'adds the feature name if it exists
                    End If
                Next
            Next
            'replaces the FOI in feature with the FOI orginized by chrom
            For i As Integer = 0 To Features.Count - 1
                Features(i).Chrom = tempChrom(i)
                Features(i).ChromStart = tempChromStart(i)
                Features(i).ChromEnd = tempChromEnd(i)
                If tempName.Count <> 0 Then : Features(i).Name = tempName(i) : End If
            Next
            Return Features
        End Function

        'calculates the pvalue using ChiSquare
        Public Function pValueChiSquare(ByVal ObservedWithin As Double, ByVal ExpectedWithinMean As Double, ByVal NumOfFeatures As Integer) As Double
            Dim Pval, Row1, Row2, Col1, Col2, N, chi2, ExpMatrix(2, 2) As Double, ChiMatrix(2, 2) As Double
            Dim ObsWithinChi, RandWithinChi, NumOfExptsChi, NumOfRandChi As Double
            ObsWithinChi = ObservedWithin : RandWithinChi = System.Math.Round(ExpectedWithinMean, 0) : NumOfExptsChi = NumOfFeatures : NumOfRandChi = NumOfFeatures
            Pval = 1
            If (ObservedWithin = 0 And ExpectedWithinMean = 0) Or (ObservedWithin = System.Math.Round(ExpectedWithinMean, 0)) Then
                Return Pval
            End If
            Row1 = NumOfExptsChi
            Row2 = NumOfRandChi
            Col1 = ObsWithinChi + RandWithinChi
            Col2 = NumOfExptsChi + NumOfRandChi - (ObsWithinChi + RandWithinChi)
            N = NumOfExptsChi + NumOfRandChi
            'calculate expected values for each cell
            ExpMatrix(1, 1) = (Row1 * Col1) / N
            ExpMatrix(1, 2) = (Row1 * Col2) / N
            ExpMatrix(2, 1) = (Row2 * Col1) / N
            ExpMatrix(2, 2) = (Row2 * Col2) / N
            'transform into chi-square values table = (o-e)^2 / e
            ChiMatrix(1, 1) = (ObsWithinChi - ExpMatrix(1, 1)) ^ 2 / ExpMatrix(1, 1)
            ChiMatrix(1, 2) = (NumOfExptsChi - ObsWithinChi - ExpMatrix(1, 2)) ^ 2 / ExpMatrix(1, 2)
            ChiMatrix(2, 1) = (RandWithinChi - ExpMatrix(2, 1)) ^ 2 / ExpMatrix(2, 1)
            ChiMatrix(2, 2) = (NumOfRandChi - RandWithinChi - ExpMatrix(2, 2)) ^ 2 / ExpMatrix(2, 2)
            'calculate x^2 = sum(o-e)^2 / e
            chi2 = ChiMatrix(1, 1) + ChiMatrix(1, 2) + ChiMatrix(2, 1) + ChiMatrix(2, 2)
            'Dim x As Double = chisquarecdistribution(1, chi2)
            Try
                Pval = chisquarecdistribution(1, chi2)                                                              'this method does not require that we subtract it from 1
            Catch
                Pval = 1
            End Try

            'Dim pValue As Double = 1 - chisquaredistribution(1, chi2)
            Debug.WriteLine("Num of Features: " & NumOfFeatures)
            Debug.WriteLine("Observed : " & ObservedWithin)
            Debug.WriteLine("Expected : " & ExpectedWithinMean)
            Debug.WriteLine("Chi^2 " & chi2)
            Dim c As Double = math.sqr(chi2 / (2 * NumOfFeatures + chi2))
            Dim V As Double = math.sqr(chi2 / (2 * NumOfFeatures * (2 - 1)))
            Debug.WriteLine("C : " & c)
            Return Pval
        End Function

    End Class


    'used to calculate the pvalues of the results
    Public Class AnalyticalPVCalculator
        ' Instantiate one of these classes per table
        Private Shared TotalBasePairs As Double 'The total number of base pairs for the background
        Private NumOfRegionsGF As Double     'The number of non-overlapping regions for the Region of Interest
        Private WidthGF As Double
        ' Mean width of a feature in the table
        Public Sub New(ByVal NumOfBasePairsGF As Long, ByVal NumOfRegionsGF As Long, ByVal TotalBasePairs As Long)
            ' Parameters:
            ' B -- total number of bases covered in the table
            ' nB -- total number of features in the table (after combining overlapping features into one)
            Me.NumOfRegionsGF = NumOfRegionsGF
            Me.WidthGF = NumOfBasePairsGF \ NumOfRegionsGF
            AnalyticalPVCalculator.TotalBasePairs = TotalBasePairs
        End Sub

        'Individual probabilities of each feature being a hit
        Private Function pHit(ByVal WidthFOI As Integer()) As Double()
            Dim rpHit As Double() = New Double(WidthFOI.Length - 1) {}
            For i As Integer = 0 To WidthFOI.Length - 1
                rpHit(i) = ((WidthGF + WidthFOI(i)) * NumOfRegionsGF) / TotalBasePairs
            Next
            Return rpHit
        End Function

        Public Function nExpectedHits(ByVal wA As Integer()) As Double
            Return pHit(wA).Sum()
        End Function


        Public Function pValuebinomialcdistribution(ByVal wA As Integer(), ByVal HasHit As Integer()) As Double
            ' This method is legitimate as long as there aren't huge variations in wA
            Dim p As Double = pHit(wA).Average()
            'Underrepresentation
            Dim x = HasHit.Sum()
            If HasHit.Sum() < nExpectedHits(wA) Then
                Return alglib.binomialdistribution(HasHit.Sum(), HasHit.Length, p)

            Else  'Overrepresentation
                Return alglib.binomialcdistribution(HasHit.Sum(), HasHit.Length, p)
            End If
        End Function
    End Class
End Namespace