'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports alglib
Imports System.IO
Namespace GenomeRunner
    Public Delegate Sub ProgressStart(ByVal Total As Integer)
    Public Delegate Sub ProgressUpdate(ByVal CurrItem As Integer, ByVal CurrFeaturesOfInterestName As String, ByVal GenomicFeatureName As String, ByVal NumMonteCarloRunDone As Integer)
    Public Delegate Sub ProgressDone(ByVal outputDir As String)

    'stores the settings that will be used when running the enrichment analysis
    Public Class EnrichmentSettings
        Implements ICloneable

        Public ConnectionString As String                                                           'the connection string that is used to connect to the database containing the genomic feature data
        Public NumMCtoRun As Integer                                                                'the number of Monte Carlo simulations to run
        Public PvalueThreshold As Double                                                            'the threshold at which pvalues are considered significant
        Public EnrichmentJobName As String                                                          'the user supplied name of the enrichment analysis
        Public BackgroundName As String                                                             'the name of the background that was used to calculate random associations
        Public UseSpotBackground As Boolean                                                         'whether a spotbackground should be used to calculate random associations
        Public UseMonteCarlo As Boolean                                                             'whether Monte Carlo simulations should be used to calculate the number of associations expected by random chance
        Public UseAnalytical As Boolean                                                             'whether the analytical method should be used to calculate the number of associations expected by random chance
        Public UseTradMC As Boolean                                                                 'whether traditional Monte-Carlo calculations should be used
        Public UseChiSquare As Boolean                                                              'whether the Chi-Square test should be used to calculate the p-value
        Public UseBinomialDistribution As Boolean                                                   'whether binomial distrobution should be used to calculate the p-value
        Public OutputPercentOverlapPvalueMatrix As Boolean                                          'whether a matrix should be outputed where pvalues are weighted by percent overlap
        Public SquarePercentOverlap As Boolean                                                      'whether the percent overlap weight should be sqaured before being applyed 
        Public OutputPCCweightedPvalueMatrix As Boolean                                             'whether a matrix should be outputed where pvalues are weighted by the pearson's contingency coefficient 
        Public PearsonsAudjustment As UInteger                                                      'this number is used to audjust the pearson's contingency coefficient for the matrix output
        Public AllAdjustments As Boolean                                                            'if true, multiple log files will be created; one for each type of adjustment
        Public OutputDir As String                                                                  'the directory to which the results of the enrichment analysis should be outputed to
        Public FilterLevel As String                                                                'used to store what level the filters were at so it can be recorded in the log file
        Public PromoterUpstream As UInteger = 0                                                     'stores how many base pairs the promoter region begins upstream of the gene start point 
        Public PromoterDownstream As UInteger = 0                                                   'stores how many base pairs the promoter regions covers downstream of the gene's startpoint
        Public Proximity As UInteger = 0                                                            'the number of basepairs that a feature of interest can be away from a genomic feature and still be considered a hit.  this value is NOT taken into consideration when calculating the overlap type
        Public Strand As String
        Public OutputMerged As Boolean
        Public OuputPercentObservedExpected As Boolean = False

        Public Sub New(ByVal ConnectionString As String, ByVal EnrichmentJobName As String, ByVal OutputDir As String, ByVal UseMonteCarlo As Boolean, ByVal UseAnalytical As Boolean, ByVal UseTradMC As Boolean, ByVal UseChiSquare As Boolean, ByVal UseBinomialDistribution As Boolean, ByVal OutputPercentOverlapPvalueMatrix As Boolean, ByVal SquarePercentOverlap As Boolean, ByVal OutputPCCweightedPvalueMatrix As Boolean, ByVal PearsonsAudjustment As Integer, ByVal AllAdjustments As Boolean, ByVal BackGroundName As String, ByVal UseSpotBackground As Boolean, ByVal NumMCtoRun As Integer, ByVal PValueThreshold As Double, ByVal FilterLevel As String, ByVal PromoterUpstream As UInteger, ByVal PromoterDownstream As UInteger, ByVal proximity As UInteger, ByVal Strand As String, ByVal OutputMerged As Boolean)
            Me.ConnectionString = ConnectionString
            Me.EnrichmentJobName = EnrichmentJobName
            Me.UseMonteCarlo = UseMonteCarlo
            Me.UseAnalytical = UseAnalytical
            Me.UseTradMC = UseTradMC
            Me.OutputPercentOverlapPvalueMatrix = OutputPercentOverlapPvalueMatrix
            Me.SquarePercentOverlap = SquarePercentOverlap
            Me.OutputPCCweightedPvalueMatrix = OutputPCCweightedPvalueMatrix
            Me.PearsonsAudjustment = PearsonsAudjustment
            Me.AllAdjustments = AllAdjustments
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
            Me.Strand = Strand
            Me.OutputMerged = OutputMerged
        End Sub

        Public Sub New()
            'Constructor with no parameters is used for XML serialization
        End Sub

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim eSettings As New EnrichmentSettings(ConnectionString, EnrichmentJobName, OutputDir, UseMonteCarlo, UseAnalytical, UseTradMC, UseChiSquare, UseBinomialDistribution, OutputPercentOverlapPvalueMatrix, SquarePercentOverlap, OutputPCCweightedPvalueMatrix, PearsonsAudjustment, AllAdjustments, BackgroundName, UseSpotBackground, NumMCtoRun, PvalueThreshold, FilterLevel, PromoterUpstream, PromoterDownstream, Proximity, Strand, OutputMerged)
            Return eSettings
        End Function
    End Class

    'this class is passed on to the Monte Carlo simulator so that it can return progress updates to the user interface
    Public Class EnrichmentAnalysisProgress
        Public featureFileName As String
        Public overallRunProgress As Integer
        Public genomicFeatureName As String
        Public Sub New(ByVal FeatureFileName As String, ByVal OveralRunProgress As Integer, ByVal GenomicFeatureName As String)
            Me.featureFileName = FeatureFileName
            Me.overallRunProgress = OveralRunProgress
            Me.genomicFeatureName = GenomicFeatureName
        End Sub
    End Class

    Public Class EnrichmentAnalysis
        Public ConnectionString As String
        Dim progStart As ProgressStart                                                                      'used to set initial progress
        Dim progUpdate As ProgressUpdate                                                                    'used to update progress
        Dim progDone As ProgressDone                                                                    'used return progress complete
        Dim RandomClass As New Random()

        Public Sub New(ByVal progStart As ProgressStart, ByVal progUpdate As ProgressUpdate, ByVal progDone As ProgressDone)
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
            Dim FeaturesOfInterestNames As New List(Of String)
            Dim Outputer As Output = New Output(FeatureOfInterestFilePaths.Count)
            Dim AccumulatedGenomicFeatures As New Hashtable
            ConnectionString = Settings.ConnectionString
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'NOTE: AccumulatedGenomicFeatures is a Hashtable that stores GenomicFeatures specific to each FeatureOfInterest file.
            '      So, given x genomic features & f features of interest files, there will be:
            '          x * f genomic features in AccumulatedGenomicFeatures.
            '      EXAMPLE:
            '      Features of interest files: "CDBox", "HAcaBox"
            '      Genomic features: "CpGIslands", "ORegAnno"
            '      AccumulatedGenomicFeatures = ["CpGIslands"] => {CpGIslands Genomic Feature calculated with CDBox, CpGIslands Genomic Feature calculated with HAcaBox},
            '                                   ["ORegAnno"]   => {ORegAnno Genomic Feature calculated with CDBox, ORegAnno Genomic Feature calculated with HAcaBox},
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            For Each GF In GenomicFeatures
                If GF.NamesToInclude.Count > 0 Then
                    AccumulatedGenomicFeatures.Add(GF.Name, New List(Of GenomicFeature))
                Else
                    AccumulatedGenomicFeatures.Add(GF.TableName, New List(Of GenomicFeature))
                End If
            Next
            'Prints the legend into the log file
            Outputer.OutputLogFileHeader(Settings)
            'goes through each filepath and runs an enrichment analysis on the features in the file
            For Each FeatureFilePath In FeatureOfInterestFilePaths
                FeaturesOfInterestNames.Add(Path.GetFileNameWithoutExtension(FeatureFilePath))
                FeaturesOfInterest = LoadFeatureOfInterests(FeatureFilePath)
                FeaturesOfInterest = OrganizeFeaturesByChrom(FeaturesOfInterest)
                'TODO is it ok that I moved this above the loop?
                'Dim Outputer As New Output(FeaturesOfInterest.Count)
                Outputer = New Output(FeaturesOfInterest.Count)
                Dim isFirstPvalue As Boolean = True                                                                                    'Whether the general information for the feature file should be outputed
                Dim currGF As Integer = 0
                'runs the features of interest against the genomic features that were selected to be run
                progStart.Invoke(GenomicFeatures.Count)
                For Each GF In GenomicFeatures

                    progUpdate.Invoke(currGF, Path.GetFileName(FeatureFilePath), GF.Name, 0)
                    'uses either monte carlo or the analytical method for the enrichment analysis
                    If Settings.UseMonteCarlo = True Then
                        Debug.Print("Running initial analysis MonteCarlo for " & GF.Name)
                        Dim enrichmentProgress As New EnrichmentAnalysisProgress(Path.GetFileName(FeatureFilePath), currGF, GF.Name)    'stores the progress settings so that they can be passed on to the monte carlo method
                        GF = Calculate_PValue_MonteCarlo(GF, FeaturesOfInterest, Background, Settings, enrichmentProgress)
                    End If
                    If Settings.UseAnalytical = True Then
                        GF = calculatePValueUsingAnalyticalMethod(GF, FeaturesOfInterest, Background, Settings)
                    End If
                    If GF.NamesToInclude.Count > 0 Then
                        AccumulatedGenomicFeatures(GF.Name).add(GF.Clone)
                    Else
                        AccumulatedGenomicFeatures(GF.TableName).add(GF.Clone)
                    End If
                    Outputer.OutputPvalueLogFileShort(isFirstPvalue, GF, Settings, Path.GetFileNameWithoutExtension(FeatureFilePath))           'results are added on to the log file after each genomic feature is analyzed

                    GF.FeatureReturnedData.Clear()
                    isFirstPvalue = False
                    currGF += 1
                Next
                If Settings.AllAdjustments Then
                    'create matrix file for each adjustment set
                    'create different settings then run output for each group of settings
                    '1. no adjustments
                    '2. percent linear
                    '3. percent squared
                    '4. pcc (default 100)
                    Dim none As EnrichmentSettings = Settings.Clone, percentLinear As EnrichmentSettings = Settings.Clone, percentSquared As EnrichmentSettings = Settings.Clone, pcc As EnrichmentSettings = Settings.Clone

                    none.OutputPercentOverlapPvalueMatrix = False : none.OutputPCCweightedPvalueMatrix = False
                    percentLinear.OutputPercentOverlapPvalueMatrix = True : percentLinear.SquarePercentOverlap = False : percentLinear.OutputPCCweightedPvalueMatrix = False
                    percentSquared.OutputPercentOverlapPvalueMatrix = True : percentSquared.SquarePercentOverlap = True : percentSquared.OutputPCCweightedPvalueMatrix = False
                    pcc.OutputPercentOverlapPvalueMatrix = False : pcc.OutputPCCweightedPvalueMatrix = True

                    For Each setting In {none, percentLinear, percentSquared, pcc}
                        If Settings.OutputMerged Then
                            Outputer.OutputPValueMatrixTransposed(Settings.OutputDir, GenomicFeatures, setting, FeaturesOfInterestNames, AccumulatedGenomicFeatures)
                        Else
                            For Each featureOfInterestName In FeaturesOfInterestNames
                                Outputer.OutputPValueMatrixIndividualTransposed(Settings.OutputDir, GenomicFeatures, setting, Path.GetFileNameWithoutExtension(FeatureFilePath))
                            Next
                        End If

                    Next
                Else
                    If Settings.OutputMerged Then
                        Outputer.OutputPValueMatrixTransposed(Settings.OutputDir, GenomicFeatures, Settings, FeaturesOfInterestNames, AccumulatedGenomicFeatures)
                    Else
                        For Each featureOfInterestName In FeaturesOfInterestNames
                            Outputer.OutputPValueMatrixIndividualTransposed(Settings.OutputDir, GenomicFeatures, Settings, Path.GetFileNameWithoutExtension(FeatureFilePath))
                        Next
                    End If
                End If

                'outputs a matrix for the percen of observed versus percent expected
                Outputer.OuputPercentObservedExpected(GenomicFeatures, OutputMatrixColumnHeaders, Settings, Path.GetFileNameWithoutExtension(FeatureFilePath))
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
        Private Function Calculate_PValue_MonteCarlo(ByRef GFeature As GenomicFeature, ByRef FeaturesOfInterest As List(Of Feature), ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings, ByVal analysisProgress As EnrichmentAnalysisProgress)
            Dim NumOfFeatures As Integer = FeaturesOfInterest.Count
            Dim mean As Double, variance As Double, skewness As Double, kurtosis As Double 't2 As Double, lt As Double, rt As Double
            Dim currentTime As System.DateTime = System.DateTime.Now  'used in the header of the output
            Dim ObservedWithin As Integer = 0, ObservedOutside As Integer = 0
            Dim randEventsCountMC(Settings.NumMCtoRun - 1) As Double                                    'Counters for features within one/within all simulations
            Dim AnnoSettings As New AnnotationSettings(Settings.ConnectionString, Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity, Settings.FilterLevel, Settings.Strand, False)
            Dim fAnalysis As New GenomeRunner.AnnotationAnalysis()
            Dim FeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(FeaturesOfInterest, Settings.Proximity)

            GFeature.FeatureReturnedData.Clear()
            GFeature.NumOfFeatures = NumOfFeatures
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
            Dim HitArray(NumOfFeatures - 1) As Integer 'Special array to hold number of hits during each MC simulation
            For i As Integer = 0 To Settings.NumMCtoRun - 1
                Debug.Print("Running MC run# " & i + 1 & " of " & Settings.NumMCtoRun & " " & TimeOfDay)
				progUpdate.Invoke(analysisProgress.overallRunProgress, analysisProgress.featureFileName, analysisProgress.genomicFeatureName, i + 1)
                Dim RandomFeatures As List(Of Feature) = createRandomRegions(FeaturesOfInterest, Background, Settings.UseSpotBackground) 'generates a random features of interest 
                Dim RandomFeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(RandomFeatures, Settings.Proximity)

                GFeature.FeatureReturnedData.Clear() 'clears the returned data of the previous run
                OrganizeFeaturesByChrom(RandomFeaturesOfInterestproximity)
                GFeature = fAnalysis.Feature_Analysis(GFeature, RandomFeaturesOfInterestproximity, RandomFeatures, AnnoSettings)     'analizes the randomfeatures. 
                Dim ExpectedWithin As Integer = 0, ExpectedOutside As Integer = 0                               'Zero out counters for each Monte-Carlo run
                For x = 0 To NumOfFeatures - 1                                'Calculate statistics for them
                    If GFeature.FeatureReturnedData(x).CountData <> 0 Then                     'If random alternative event observed
                        ExpectedWithin += 1
                    Else
                        ExpectedOutside += 1
                    End If
                Next
                If ExpectedWithin > (NumOfFeatures - 1) Then ExpectedWithin = NumOfFeatures - 1
                HitArray(ExpectedWithin) += 1    'How often this number of ExpectedWithin was observed
                randEventsCountMC(i) = ExpectedWithin                              'Store simulation results
            Next

            Dim total As Integer = 0
            For i As Integer = 0 To randEventsCountMC.Count - 1
                total += randEventsCountMC(i)
            Next

            samplemoments(randEventsCountMC, mean, variance, skewness, kurtosis)
            GFeature.MCMean = mean : GFeature.MCvariance = variance : GFeature.MCskewness = skewness : GFeature.MCkurtosis = kurtosis
            If Settings.UseMonteCarlo = True Then
                If Settings.UseChiSquare = True Then
                    GFeature.PValueMonteCarloChisquare = pValueChiSquare(ObservedWithin, randEventsCountMC.Average, NumOfFeatures) 'calculates the pvalue using chisquare
                    GFeature.PCCMonteCarloChiSquare = PearsonsContigencyCoeffcient(ObservedWithin, randEventsCountMC.Average, NumOfFeatures) 'calculates the pearson's congingency coefficient 
                ElseIf Settings.UseTradMC = True Then
                    GFeature.PValueMonteCarloTradMC = pValueTradMC(ObservedWithin, randEventsCountMC.Average, NumOfFeatures, HitArray, Settings.NumMCtoRun)
                End If
				
                'TODO New way
				'This is on hold for the time being; it used tie/over/under for calculation.
				
                'GFeature.RandUnder = 0 : GFeature.RandOver = 0 : GFeature.RandTie = 0
                'For Each rFOI In randEventsCountMC
                '    If rFOI < GFeature.ActualHits Then
                '        GFeature.RandUnder += 1
                '    ElseIf rFOI > GFeature.ActualHits Then
                '        GFeature.RandOver += 1
                '    Else
                '        GFeature.RandTie += 1
                '    End If
                'Next
                ''get percentages by dividing each of these values by Settings.NumMCtoRun
                'Dim possiblePValues As New List(Of Double)
                'For Each value In {GFeature.RandUnder, GFeature.RandOver, GFeature.RandTie}
                '    If value <> 0 Then
                '        possiblePValues.Add(value)
                '    End If
                'Next
                'GFeature.PValueMonteCarloChisquare = possiblePValues.Min / Settings.NumMCtoRun
                'Debug.Print("MC p-value: " & GFeature.PValueMonteCarloChisquare)
            End If
            'TODO Still need this other if, I think...
            If Settings.UseBinomialDistribution = True Then
                'GFeature.PValueMonteCarloBinomialDistribution =  Return alglib.binomialdistribution(HasHit.Sum(), HasHit.Length, p)
            End If
            GFeature.MCExpectedHits = randEventsCountMC.Average
            GFeature.ActualHits = ObservedWithin
            Return GFeature
        End Function

        Private Function pValueTradMC(ByVal ObservedWithin As Double, ByVal ExpectedWithinMean As Double, ByVal NumOfFeatures As Integer, ByVal HitArray() As Integer, ByVal NumMCtoRun As Integer)
            Dim under, over As Integer, t, u As Integer
            Dim pUnder() As Double, pOver() As Double, Pval As Double = 1
            ReDim pUnder(NumOfFeatures - 1) : ReDim pOver(NumOfFeatures - 1)
            If (ObservedWithin = 0 And ExpectedWithinMean = 0) Or (ObservedWithin = System.Math.Round(ExpectedWithinMean, 0)) Then
                Return Pval
            End If
            For t = 0 To NumOfFeatures - 1
                under = 0 : over = 0
                For u = 0 To t
                    under += HitArray(u)
                Next
                pUnder(t) = under / NumMCtoRun
                For u = t To NumOfFeatures - 1
                    over += HitArray(u)
                Next
                pOver(t) = over / NumMCtoRun
            Next
            ''For debugging only - dump everything into a file 
            'Using HitWriter As StreamWriter = New StreamWriter("F:\111 -.txt", True)
            '    HitWriter.WriteLine(Date.Now)
            '    HitWriter.WriteLine("# overlaps" & vbTab & "times observed" & vbTab & "p(over)" & vbTab & "p(under)")
            '    For t = 0 To NumOfFeatures - 1
            '        HitWriter.WriteLine(t & vbTab & HitArray(t) & vbTab & pOver(t) & vbTab & pUnder(t))
            '    Next
            'End Using
            ''/End for debugging
            If ObservedWithin > System.Math.Round(ExpectedWithinMean, 0) Then
                Return pOver(ObservedWithin)
            ElseIf ObservedWithin < System.Math.Round(ExpectedWithinMean, 0) Then
                Return pUnder(ObservedWithin)
            Else
                Return 1
            End If
        End Function

        Private Function calculatePValueUsingAnalyticalMethod(ByVal GFeature As GenomicFeature, ByVal Features As List(Of Feature), ByVal Background As List(Of Feature), ByVal Settings As EnrichmentSettings) As GenomicFeature
            'runs a normal analysis on the feature
            Debug.Print("Running initial analysis the Analytical Method for " & GFeature.Name & " " & TimeOfDay)
            Dim AnoSettings As New AnnotationSettings(Settings.ConnectionString, Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity, Settings.FilterLevel, Settings.Strand, False)
            Dim fAnalysis As New AnnotationAnalysis()
            Dim FeaturesOfInterestproximity As List(Of Feature) = CreateproximityFeaturesOfInterest(Features, Settings.Proximity)
            GFeature = fAnalysis.Feature_Analysis(GFeature, FeaturesOfInterestproximity, Features, AnoSettings)
            Dim wA(GFeature.FeatureReturnedData.Count - 1) As Integer 'width of each FOI
            Dim hA(GFeature.FeatureReturnedData.Count - 1) As Integer 'stores whether the FOI was a hit or miss 

            GFeature.NumOfFeatures = Features.Count

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
            Dim AnoSettings As New AnnotationSettings(Settings.ConnectionString, Settings.PromoterUpstream, Settings.PromoterDownstream, Settings.Proximity, Settings.FilterLevel, Settings.Strand, False)
            Dim Analyzer As New AnnotationAnalysis()                                                         'used to get the genomic feature data from the mysql database
            'gets the total number of base pairs of the background
            For Each interval In Background
                TotalNumberofBasePairs += interval.ChromEnd - interval.ChromStart + 1                                        'adds one to the interval length so that number of bases in the interval is counted rather than the length of the interval
            Next

            Dim totalFeatureBasePair As Long = 0                                                                             'keeps count of the total number of base pairs that are covered by the feature
            Dim TotalFeatureRegionCount As Long = 0                                                                          'keeps count of the total number of regions that are 'created' by superimposing the feature regions onto the basepairs of the chromosome
            Dim debugRoCount As Integer = 0
            For currInterval As Integer = 0 To Background.Count - 1

                Dim GenomicFeaturesSQLData As List(Of FeatureSQLData) _
                    = Analyzer.Feature_Load_GRFeature_In_Memory(GenomicFeature, Background(currInterval).Chrom, 0, Settings.PromoterUpstream, Settings.PromoterDownstream, ConnectionString) 'loads GF into memory so that its bps and regions can be counted
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
                'Get unique chromosomes from the list of background intervals
                Dim BackgroundChromosomes As List(Of String) = New List(Of String)
                For Each bkgInterval In BackgroundInterval
                    If Not BackgroundChromosomes.Contains(bkgInterval.Chrom) Then BackgroundChromosomes.Add(bkgInterval.Chrom)
                Next
                hqrndrandomize(state)                                                                      'Initialize random number generator
                For i As Integer = 0 To NumOfFeatures - 1
                    Dim feature As New Feature
                    'Simple reshuffilng
                    RandFeature = hqrnduniformi(state, BackgroundInterval.Count)                           'Random position from the whole spectrum of spots
                    feature.Chrom = BackgroundInterval(RandFeature).Chrom                                  'Store chromosome of random position
                    feature.ChromStart = BackgroundInterval(RandFeature).ChromStart                        'and start coordinate
                    feature.ChromEnd = BackgroundInterval(RandFeature).ChromEnd
                    RandomFeatures.Add(feature)
                    ''Weighted chromosome selection
                    'Dim selected As Boolean = False
                    'Do Until selected 'Select random chromosome that is also present in the list of chromosomes from background intervals
                    '    feature.Chrom = BackgroundInterval(getWeightedRandomChromosome(state, BackgroundInterval)).Chrom
                    '    If BackgroundChromosomes.Contains(feature.Chrom) Then selected = True 'If selected chromosome is in the background interval, use it
                    'Loop
                    'selected = False
                    'Do Until selected
                    '    RandFeature = hqrnduniformi(state, BackgroundInterval.Count)                           'Random position from the whole spectrum of spots
                    '    If feature.Chrom = BackgroundInterval(RandFeature).Chrom Then selected = True 'if this random selection from the selected chromosome, go with it
                    'Loop
                    'feature.ChromStart = BackgroundInterval(RandFeature).ChromStart                        'and start coordinate
                    'feature.ChromEnd = BackgroundInterval(RandFeature).ChromEnd
                    'RandomFeatures.Add(feature)
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