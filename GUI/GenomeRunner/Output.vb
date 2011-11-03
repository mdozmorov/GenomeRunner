'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports System.IO
Imports System.Globalization

Namespace GenomeRunner

    Public Class Output
        Dim NumOfFeatures As Integer = 0
        Dim FeaturesOfInterestName As String 'the name of the feature of interest file
        Public Sub New(ByVal NumOfFeatures As Integer)
            Me.NumOfFeatures = NumOfFeatures
        End Sub

        'Outputs combined log file of all files passed
        Public Sub OutputMergedLogFiles(ByVal FilePaths As List(Of String))
            Dim AccumulatedRows As New Hashtable
            Dim OrderedAccumulatedRowKeys As New List(Of String) 'This is only needed because Hashtable does not preserver order
            Dim FileNames As New List(Of String)

            For Each filePath In FilePaths
                FileNames.Add(Path.GetFileName(filePath))
                Using SR As New StreamReader(filePath)
                    While SR.EndOfStream = False
                        Dim Line = SR.ReadLine().Split(vbTab)
                        If Not AccumulatedRows.ContainsKey(Line(0)) Then
                            AccumulatedRows.Add(Line(0), New Hashtable)
                            OrderedAccumulatedRowKeys.Add(Line(0))
                        End If
                        AccumulatedRows(Line(0)).Add(Path.GetFileName(filePath), Line(1))
                    End While
                    'TODO raise error if # of keys != # of lines in file
                End Using
            Next

            'Write combined file
            Using writer As New StreamWriter(Path.GetDirectoryName(FilePaths(0)) & "\combined.gr", False)
                Dim isHeaderRow = True
                For Each key In OrderedAccumulatedRowKeys
                    If isHeaderRow Then
                        writer.WriteLine(vbTab & Join(FileNames.ToArray, vbTab))
                        isHeaderRow = False
                    Else
                        Dim stringsToJoin As New List(Of String)
                        For Each fileName In FileNames
                            If Not AccumulatedRows(key).ContainsKey(fileName) Then
                                AccumulatedRows(key).Add(fileName, "NA")
                            End If
                            stringsToJoin.Add(AccumulatedRows(key)(fileName))
                        Next
                        writer.WriteLine(key & vbTab & Join(stringsToJoin.ToArray, vbTab))
                    End If
                Next
            End Using
        End Sub

        'Outputs the results of the annotation analysis
        Public Sub OutputAnnotationAnalysis(ByVal OutputFilePath As String, ByVal GRFeaturesToAnalyze As List(Of GenomicFeature), ByVal Features As List(Of Feature))

            Using writer As New StreamWriter(OutputFilePath)

                '###outputs the summary to the file
                writer.Write("<Summary>")
                writer.WriteLine()
                Dim totalReturnedHits As Integer 'keeps track of the total number of returned hits per FOI
                'assembles the chrom part of header
                writer.Write("Chrom")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).Chrom)
                Next
                writer.WriteLine()
                'prints the chromStart part of header
                writer.Write("ChromStart")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).ChromStart)
                Next
                writer.WriteLine()
                'assembles the chromEnd part of header
                writer.Write("ChromEnd")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).ChromEnd)
                Next
                writer.WriteLine()
                'assembles the name part of the header
                If Features(0).Name <> Nothing Then
                    writer.Write("Name")
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        writer.Write(vbTab & Features(currFeature).Name)
                    Next
                Else
                    writer.Write("Name")
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        writer.Write(vbTab & "")
                    Next
                End If
                writer.WriteLine()

                'gets the total count for each feature and prints it out
                writer.Write("Total")
                For currFeature As Integer = 0 To NumOfFeatures - 1                                                                'Calculate totals for each ROI
                    totalReturnedHits = 0 'resets the count for each FOI
                    For Each gFeature In GRFeaturesToAnalyze
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then
                            totalReturnedHits += gFeature.FeatureReturnedData(currFeature).CountData 'adds the returned hits to the total count for the FOI
                        End If
                    Next
                    writer.Write(vbTab & totalReturnedHits)
                Next
                writer.WriteLine()
                For Each gFeature In GRFeaturesToAnalyze
                    writer.Write(gFeature.Name)
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then
                            writer.Write(vbTab & gFeature.FeatureReturnedData(currFeature).CountData)
                        Else
                            writer.Write(vbTab & 0)
                        End If
                    Next
                    writer.WriteLine()
                Next
                writer.Write("</Summary>")

                writer.WriteLine()
                '###outputs the all events to the file
                writer.Write("<Details>")
                writer.WriteLine()
                'assembles the chrom part of header
                writer.Write("Chrom")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).Chrom)
                Next
                writer.WriteLine()
                'assembles the chromStart part of header
                writer.Write("ChromStart")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).ChromStart)
                Next
                writer.WriteLine()
                'assembles the chromEnd row of header 
                writer.Write("ChromEnd")
                For currFeature As Integer = 0 To NumOfFeatures - 1
                    writer.Write(vbTab & Features(currFeature).ChromEnd)
                Next
                writer.WriteLine()
                'assembles the name row of header if it exists
                If Features(0).Name <> Nothing Then
                    writer.Write("Name")
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        writer.Write(vbTab & Features(currFeature).Name)
                    Next
                Else
                    writer.WriteLine("Name")
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        writer.Write(vbTab & "")
                    Next
                End If
                writer.WriteLine()
                'outputs the returned hits for the genomic features for each of the features of interest
                For Each gFeature In GRFeaturesToAnalyze
                    Dim strStartOutput As String = gFeature.Name & "ChromStart", strEndOutput As String = gFeature.Name & "ChromEnd", strStrandOutput As String = gFeature.Name & "Strand", strNameOutput As String = gFeature.Name & "Name", strThresholdOutput As String = gFeature.Name & "Threshold", strOverLapTypeOutput As String = gFeature.Name & "OverLapTypeOutput", strOverLapAmountOutput As String = gFeature.Name & "OverLapAmount" 'LC 6/20/11 is used to store the joined arrays of FOI with mutliple GRs
                    writer.Write(strStartOutput)

                    '...appends the returned start points of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).fStartData.Select(Function(x) x.ToString()).ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strEndOutput)

                    '...appends the returned end points of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).fEndData.Select(Function(x) x.ToString()).ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strStrandOutput)

                    '...appends the returned strand data of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            'writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).StrandData.Select(Function(x) x.ToString()).ToArray(), "|"))
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).StrandData.ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strNameOutput)

                    '...appends the returned name data of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            'TODO this x.ToString call causes a problem when outputting data for non-overlaps
                            'Dim stName As String = Join(gFeature.FeatureReturnedData(currFeature).NameData.Select(Function(x) x.ToString()).ToArray(), "|")
                            Dim stName As String = Join(gFeature.FeatureReturnedData(currFeature).NameData.ToArray(), "|")
                            If stName IsNot Nothing Then : stName.Replace(vbCrLf, "").Replace(Chr(13), "") : End If 'replaces instances of vbCrLf with blanks to prevent new lines from being created
                            writer.Write(vbTab & stName)
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strThresholdOutput)

                    '...appends the returned threshold data of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            'TODO this x.ToString call causes a problem when outputting data for non-overlaps
                            'writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).ThresholdData.Select(Function(x) x.ToString()).ToArray(), "|"))
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).ThresholdData.ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strOverLapTypeOutput)

                    '...appends the overlaptype of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            'TODO this x.ToString call causes a problem when outputting data for non-overlaps
                            'writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).OverLapTypeData.Select(Function(x) x.ToString()).ToArray(), "|"))
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).OverLapTypeData.ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                    writer.Write(strOverLapAmountOutput)

                    '...appends the overlap amount data of the genomic features that overlap the feature of interest into a '|' seperated string and outputs it to the file
                    For currFeature As Integer = 0 To NumOfFeatures - 1
                        If gFeature.FeatureReturnedData(currFeature) IsNot Nothing Then 'checks if anything was returned for the FOI
                            writer.Write(vbTab & Join(gFeature.FeatureReturnedData(currFeature).OverLapAmountData.Select(Function(x) x.ToString()).ToArray(), "|"))
                        Else
                            writer.Write(vbTab & "")
                        End If
                    Next
                    writer.WriteLine()
                Next
                writer.Write("</Details>")
            End Using
        End Sub

        'outputs the results of the enrichement analysis into a log file
        Public Sub OutputPvalueLogFile(ByRef outputHeader As Boolean, ByRef GFeature As GenomicFeature, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Dim mean As Double, variance As Double, skewness As Double, kurtosis As Double
            mean = GFeature.MCMean : variance = GFeature.MCvariance : skewness = GFeature.MCskewness : kurtosis = GFeature.MCkurtosis
            '...header string showing what strand the genomic feature was filtered by
            Dim strStrandsIncluded As String
            If GFeature.StrandToFilterBy <> "" Then : strStrandsIncluded = GFeature.StrandToFilterBy : Else : strStrandsIncluded = "Both" : End If
            '...header string that shows what method was used to calculate the number of random associations expected
            Dim strExpectedMethodUsed As String = "Expected associations calculated using: "
            If Settings.UseMonteCarlo = True Then : strExpectedMethodUsed &= Settings.NumMCtoRun & " Monte Carlo runs" : ElseIf Settings.UseAnalytical = True Then : strExpectedMethodUsed &= "Analytical method" : End If
            '...header string showing how pvalue was calculated 
            Dim strPvalueCalcMethod As String = "P-value was calculated using: "
            If Settings.UseChiSquare = True Then : strPvalueCalcMethod &= "Chi-Square test" : ElseIf Settings.UseTradMC = True Then : strPvalueCalcMethod &= "Traditional Monte-Carlo" : ElseIf Settings.UseBinomialDistribution = True Then : strPvalueCalcMethod &= "Binomial Distribution" : End If
            Dim strPromoterUpstream As String = ""

            'used to calculate the ratio of actual/expected
            'Dim MCObsRandRatio As Double = 0
            'If GFeature.MCExpectedHits <> 0 Then
            '    MCObsRandRatio = GFeature.ActualHits / GFeature.MCExpectedHits
            'End If
            'Dim AnalObsRandRatio As Double = 0
            'If GFeature.AnalyticalExpectedWithin <> 0 Then
            '    AnalObsRandRatio = GFeature.ActualHits / GFeature.AnalyticalExpectedWithin
            'End If


            'this header is outputed at the start of each new features of interest file analysis 
            Dim currentTime As System.DateTime = System.DateTime.Now                                'used in the header of the output
            Dim body As String = ""
            If outputHeader = True Then                                                             'if this is the first pvalue being caluculated, header line is outputed
                Dim header As String = vbCrLf & currentTime.Date & " " & currentTime.Hour & ":" & currentTime.Minute & " " _
                    & vbCrLf & "Features analyzed: " & FeaturesOfInterestName _
                    & vbCrLf & "Name of background used: " & Settings.BackgroundName _
                    & vbCrLf & "Total number of features: " & NumOfFeatures _
                    & vbCrLf & "Threshold at: " & Settings.FilterLevel _
                    & vbCrLf & "Strand(s) included: " & strStrandsIncluded _
                    & vbCrLf & "Proximity (bp): " & Settings.Proximity _
                    & vbCrLf & strExpectedMethodUsed _
                    & vbCrLf & strPvalueCalcMethod
                'writes the header to the log file
                Using writer As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_LOG.gr", True)
                    writer.WriteLine(header)
                End Using
            End If

            'if names were used as a filter, they are added to a string which is outputed
            Dim strNamesToInclude As String = ""
            'If GFeature.NamesToInclude.Count <> 0 Then
            '    strNamesToInclude = "Names included: "
            '    For Each nameToInclude In GFeature.NamesToInclude
            '        strNamesToInclude &= nameToInclude & "," & vbCrLf
            '    Next
            '    strNamesToInclude &= vbCrLf
            'End If

            'outputs the log file depending on what combination of tests are used, different values are outputted
            Using writer As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_LOG.gr", True)

                '...this is always outputed as the number of observed and the genomic feature name are indepent of what tests are run
                body = vbCrLf & FeaturesOfInterestName & " : " & GFeature.Name & " association statistics" & vbCrLf
                body &= strNamesToInclude & "Observed number of " & GFeature.Name & " associations : " & vbTab & GFeature.ActualHits
                writer.WriteLine(body)

                If Settings.UseMonteCarlo = True Then
                    '...outputted when using montecarlo
                    body = "Expected number of associations for Monte Carlo (mean = " & Math.Round(mean, 2) & ", variance = " & Math.Round(variance, 2) & ", skewness = " & Math.Round(skewness, 2) & "; kurtosis = " & Math.Round(kurtosis, 2) & ")" & vbTab & Math.Round(GFeature.MCExpectedHits, 2) & vbCrLf
                    If GFeature.ActualHits >= GFeature.MCExpectedHits Then
                        '...outputted when the features are overrepresented
                        If Settings.UseChiSquare = True Then
                            '..outputted when the chi-square test is used to calculate p
                            If GFeature.PValueMonteCarloChisquare < Settings.PvalueThreshold Then
                                body &= "Observed associations OVERrepresented: (Chi-square) p=" & vbTab & GFeature.PValueMonteCarloChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different: (Chi-square) p=" & vbTab & GFeature.PValueMonteCarloChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Monte Carlo Pearson's contingency Coefficient (Chi-Square = " & vbTab & GFeature.PCCMonteCarloChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                        End If
                        If Settings.UseTradMC = True Then body &= "P-value over =" & vbTab & GFeature.PValueMonteCarloChisquare & vbCrLf
                    End If

                    If GFeature.ActualHits < GFeature.MCExpectedHits Then
                        '...outputted when the features are underrepresented
                        If Settings.UseChiSquare = True Then
                            '..outputted when the chi-square test is used to calculate p
                            If GFeature.PValueMonteCarloChisquare < Settings.PvalueThreshold Then
                                body &= "Observed associations UNDERrepresented: (Chi-square) p=" & vbTab & GFeature.PValueMonteCarloChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different (Chi-square) p=" & vbTab & GFeature.PValueMonteCarloChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Monte Carlo Pearson's contingency Coefficient (Chi-Square) = " & vbTab & GFeature.PCCMonteCarloChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                        End If
                        If Settings.UseTradMC = True Then body &= "P-value under =" & vbTab & GFeature.PValueMonteCarloChisquare & vbCrLf
                    End If

                    '...always outputted when using monte carlo 
                    body &= "Observed number of " & GFeature.Name & " feature associations/Total number of features = " & vbTab & Math.Round((GFeature.ActualHits / NumOfFeatures), 2) & vbCrLf
                    writer.Write(body)
                End If

                If Settings.UseAnalytical = True Then
                    '...always outputed when using the analytical method
                    body = "Expected number of associations for Analytical method: " & vbTab & Math.Round(GFeature.AnalyticalExpectedWithin, 2) & vbCrLf

                    If GFeature.ActualHits >= GFeature.AnalyticalExpectedWithin Then
                        '...outputtted when the features are overrepresented 
                        If Settings.UseBinomialDistribution = True Then
                            '...outputted when binomial distribution is used to caclulate p
                            If GFeature.PValueAnalyticalBinomialDistribution < Settings.PvalueThreshold Then
                                body &= "Observed associations OVERrepresented, (Binomial Distribution) p=" & vbTab & GFeature.PValueAnalyticalBinomialDistribution.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different, (Binomial Distribution) p=" & vbTab & GFeature.PValueAnalyticalBinomialDistribution.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Analytical Pearson's Contingency Coefficient (Binomial Distribution) = " & "NA" & vbCrLf
                        End If

                        If Settings.UseChiSquare = True Then
                            '...outputted when chi-square is used to caclulate p
                            If GFeature.PValueAnalyticalChisquare < Settings.PvalueThreshold Then
                                body &= "Observed associations OVERrepresented, (Chi-Square) p=" & vbTab & GFeature.PValueAnalyticalChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different: (Chi-Square) p=" & vbTab & GFeature.PValueAnalyticalChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Analytical Pearson's Contingency Coefficient (Chi-Square) = " & vbTab & GFeature.PCCAnalyticalChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                        End If
                    End If


                    If GFeature.ActualHits < GFeature.AnalyticalExpectedWithin Then
                        '...outputted when the features are underrepresented 
                        If Settings.UseBinomialDistribution = True Then
                            '...outputted when binomial distribution is used to caclulate p
                            If GFeature.PValueAnalyticalBinomialDistribution < Settings.PvalueThreshold Then
                                body &= "Observed associations UNDERrepresented: (Binomial Distribution) p=" & vbTab & GFeature.PValueAnalyticalBinomialDistribution.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different: (Binomial Distribution) p=" & vbTab & GFeature.PValueAnalyticalBinomialDistribution.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Analytical Pearson's Contingency Coefficient (Binomial Distribution) = " & "NA" & vbCrLf
                        End If

                        If Settings.UseChiSquare = True Then
                            '...outputted when chi-square is used to caclulate p
                            If GFeature.PValueAnalyticalChisquare < Settings.PvalueThreshold Then
                                body &= "Observed associations UNDERrepresented: (Chi-Square) p=" & vbTab & GFeature.PValueAnalyticalChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            Else
                                body &= "Observed and random associations are not statistically significantly different: (Chi-Square) p=" & vbTab & GFeature.PValueAnalyticalChisquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                            End If
                            body &= "Analytical Pearson's contingency Coefficient (Chi-Square) = " & vbTab & GFeature.PCCAnalyticalChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture) & vbCrLf
                        End If
                    End If

                    '...always outputted when using the analytical method
                    body &= "Observed number of " & GFeature.Name & " feature associations/Total number of features = " & vbTab & Math.Round((GFeature.ActualHits / NumOfFeatures), 2) & vbCrLf
                    'body &= "Analytical ratio of obsWithin/randWithin = " & vbTab & AnalObsRandRatio
                    writer.Write(body)
                End If
            End Using
        End Sub

        'Prints the legend in the log file
        Public Sub OutputLogFileHeader(ByVal Settings As EnrichmentSettings)
            Using writer As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_LOG.gr", True)
                writer.WriteLine("Legend:" & vbCrLf & _
                                 "Observed - Count of FOIs that overlap with GFs" & vbCrLf & _
                                 "Expected - expected by random chance count of random FOI overlap with GFs" & vbCrLf & _
                                 "Diff - whether calculated enrichment/depletion is significant" & vbCrLf & _
                                 "p-val - p-value itself" & vbCrLf & _
                                 "PCC - Pearson's Contingency Coefficient" & vbCrLf & _
                                 "Obs/Tot - fraction of FOIs that overlap with GFs")
            End Using
        End Sub

        'outputs the results of the enrichement analysis into a log file
        Public Sub OutputPvalueLogFileShort(ByRef outputHeader As Boolean, ByRef GFeature As GenomicFeature, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Dim mean As Double, variance As Double, skewness As Double, kurtosis As Double
            mean = GFeature.MCMean : variance = GFeature.MCvariance : skewness = GFeature.MCskewness : kurtosis = GFeature.MCkurtosis
            '...header string showing what strand the genomic feature was filtered by
            Dim strStrandsIncluded As String
            If GFeature.StrandToFilterBy <> "" Then : strStrandsIncluded = GFeature.StrandToFilterBy : Else : strStrandsIncluded = "Both" : End If
            '...header string that shows what method was used to calculate the number of random associations expected
            Dim strExpectedMethodUsed As String = "Expected associations calculated using: "
            If Settings.UseMonteCarlo = True Then : strExpectedMethodUsed &= Settings.NumMCtoRun & " Monte Carlo simulations" : ElseIf Settings.UseAnalytical = True Then : strExpectedMethodUsed &= "Analytical method" : End If
            '...header string showing how pvalue was calculated 
            Dim strPvalueCalcMethod As String = "P-value was calculated using: "

            If Settings.UseChiSquare = True Then : strPvalueCalcMethod &= "Chi-Square test" : ElseIf Settings.UseTradMC = True Then : strPvalueCalcMethod &= "Traditional Monte-Carlo" : ElseIf Settings.UseBinomialDistribution = True Then : strPvalueCalcMethod &= "Binomial Distribution" : End If

            Dim strPromoterUpstream As String = ""

            'used to calculate the ratio of actual/expected
            'Dim MCObsRandRatio As Double = 0
            'If GFeature.MCExpectedHits <> 0 Then
            '    MCObsRandRatio = GFeature.ActualHits / GFeature.MCExpectedHits
            'End If
            'Dim AnalObsRandRatio As Double = 0
            'If GFeature.AnalyticalExpectedWithin <> 0 Then
            '    AnalObsRandRatio = GFeature.ActualHits / GFeature.AnalyticalExpectedWithin
            'End If


            'this header is outputed at the start of each new features of interest file analysis 
            Dim currentTime As System.DateTime = System.DateTime.Now                                'used in the header of the output
            Dim body As String = ""
            If outputHeader = True Then                                                             'if this is the first pvalue being caluculated, header line is outputed
                Dim header As String = vbCrLf & currentTime.Date & " " & currentTime.Hour & ":" & currentTime.Minute & " " _
                    & vbCrLf & "Features analyzed: " & FeaturesOfInterestName & " (Total " & NumOfFeatures & ")" _
                    & vbCrLf & "Name of background used: " & Settings.BackgroundName _
                    & vbCrLf & "Threshold at: " & Settings.FilterLevel _
                    & vbCrLf & "Strand(s) included: " & strStrandsIncluded _
                    & vbCrLf & "Proximity (bp): " & Settings.Proximity _
                    & vbCrLf & strExpectedMethodUsed _
                    & vbCrLf & strPvalueCalcMethod
                'writes the header to the log file
                Using writer As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_LOG.gr", True)
                    writer.WriteLine(header)
                    'write minor header as well
                    writer.WriteLine(vbCrLf & FeaturesOfInterestName & vbTab & "Observed" & vbTab & "Expected" & vbTab & "Diff" & vbTab & "p-val" & vbTab & "PCC" & vbTab & "Obs/Tot")
                End Using
            End If

            'outputs the log file depending on what combination of tests are used, different values are outputted
            Using writer As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_LOG.gr", True)

                Dim diff As String = ""
                Dim UsePvalue As Double = 0, pVal As String = ""
                Dim pcc As String = ""

                If Settings.UseMonteCarlo = True Then
                    If Settings.UseChiSquare = True Then UsePvalue = GFeature.PValueMonteCarloChisquare : pcc = GFeature.PCCMonteCarloChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture)
                    If Settings.UseTradMC = True Then UsePvalue = GFeature.PValueMonteCarloTradMC : pcc = "NA"
                    pVal = UsePvalue.ToString("0.##E+0", CultureInfo.InvariantCulture)
                    If GFeature.ActualHits >= GFeature.MCExpectedHits Then
                        'features are OVERrepresented
                        If UsePvalue < Settings.PvalueThreshold Then
                            diff = "OVER"
                        Else
                            diff = "no"
                        End If
                    ElseIf GFeature.ActualHits < GFeature.MCExpectedHits Then
                        'features are UNDERrepresented
                        If UsePvalue < Settings.PvalueThreshold Then
                            diff = "UNDER"
                        Else
                            diff = "no"
                        End If
                    End If
                ElseIf Settings.UseAnalytical = True Then
                    If Settings.UseBinomialDistribution = True Then UsePvalue = GFeature.PValueAnalyticalBinomialDistribution : pcc = "NA"
                    If Settings.UseChiSquare = True Then UsePvalue = GFeature.PValueAnalyticalChisquare : pcc = GFeature.PCCAnalyticalChiSquare.ToString("0.##E+0", CultureInfo.InvariantCulture)
                    If GFeature.ActualHits >= GFeature.AnalyticalExpectedWithin Then
                        'features are OVERrepresented 
                        If UsePvalue < Settings.PvalueThreshold Then
                            diff = "OVER"
                        Else
                            diff = "no"
                        End If
                    ElseIf GFeature.ActualHits < GFeature.AnalyticalExpectedWithin Then
                        'features are UNDERrepresented 
                        If GFeature.PValueAnalyticalBinomialDistribution < Settings.PvalueThreshold Then
                            diff = "UNDER"
                        Else
                            diff = "no"
                        End If
                    End If
                End If

                'header row for FOI
                'body &= FeaturesOfInterestName & vbTab & "Observed" & vbTab & "Expected" & vbTab & "Diff" & vbTab & "p-val" & vbTab & "PCC" & vbTab & "Obs/Tot" & vbCrLf
                body &= GFeature.Name & vbTab & GFeature.ActualHits & vbTab & Math.Round(GFeature.MCExpectedHits, 2) & vbTab & diff & vbTab & pVal & vbTab & pcc & vbTab & Math.Round((GFeature.ActualHits / NumOfFeatures), 2)
                writer.WriteLine(body)

            End Using
        End Sub

        'outputs a line of the matrix.  If it is the first line (DoOutputHeader = true), the header columns are outputed first
        Public Sub OutputPValueMatrix(ByRef PValueOutputFileDir As String, ByRef genomicFeatures As List(Of GenomicFeature), ByVal Settings As EnrichmentSettings, ByVal DoOutputHeader As Boolean, ByVal FeaturesOfInterestName As String)
            'TODO this seems like a lot of work to do essentially the same thing. Could use some cleaning up.
            If Settings.UseMonteCarlo = True Then
                If Settings.UseChiSquare = True Then
                    If Settings.OutputPCCweightedPvalueMatrix = True Then
                        OutputPValueMonteCarlo_ChiSquare_WeightedPearsonsCoefficient__Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = True Then
                        OutputPValueMonteCarlo_ChiSquare_WeightedPercentOverlap__Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    ElseIf Settings.OutputPCCweightedPvalueMatrix = False And Settings.OutputPercentOverlapPvalueMatrix = False Then
                        OutputPValueMonteCarlo_ChiSquare__Matrix_LineTransposed(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    End If
                Else

                End If
            ElseIf Settings.UseAnalytical = True Then
                If Settings.UseChiSquare = True Then
                    If Settings.OutputPCCweightedPvalueMatrix = True Then
                        OutputPValueAnalytical_ChiSquare_WeightedPearsonsCoefficient__Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = True Then
                        OutputPValueAnalytical_ChiSquare_WeightedPercentOverlap_Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    ElseIf Settings.OutputPCCweightedPvalueMatrix = False And Settings.OutputPercentOverlapPvalueMatrix = False Then
                        OutputPValueAnalytical_ChiSquare_Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    End If
                End If
                If Settings.UseBinomialDistribution = True Then
                    If Settings.OutputPercentOverlapPvalueMatrix = True Then
                        OutputPValueAnalytical_Binomial_WeightedPercentOverlap_Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = False Then
                        OutputPValueAnalytical_Binomial_Matrix_Line(genomicFeatures, DoOutputHeader, Settings, FeaturesOfInterestName)
                    End If
                End If

            End If
        End Sub

        Public Sub OutputPValueMatrixTransposed(ByRef PValueOutputFileDir As String, ByRef genomicFeatures As List(Of GenomicFeature), ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestNames As List(Of String), ByVal AccumulatedGenomicFeatures As Hashtable)
            '8 Possible options:
            '---------------------------------------------------------------------------
            '1. UseMonteCarlo, UseChiSquare, OutputPCCweightedPvalueMatrix
            '2. UseMonteCarlo, UseChiSquare, OutputPercentOverlapPvalueMatrix
            '3. UseMonteCarlo, UseChiSquare
            '4. UseAnalytical, UseChiSquare, OutputPCCweightedPvalueMatrix
            '5. UseAnalytical, UseChiSquare, OutputPercentOverlapPvalueMatrix
            '6. UseAnalytical, UseChiSquare
            '7. UseAnalytical, UseBinomialDistribution, OutputPercentOverlapPvalueMatrix
            '8. UseAnalytical, UseBinomialDistribution

            'Output lines.
            Dim PvalueFilename As String = getPvalueFilename(Settings)
            Dim Log10Pvalues As New List(Of Double)
            Using sw As New StreamWriter(PvalueFilename)
                'writes the header columns
                sw.Write(vbTab & String.Join(vbTab, FeaturesOfInterestNames) & vbCrLf)
                'assembles a row of pvalue results
                For Each gfName In AccumulatedGenomicFeatures.Keys
                    'TODO how does one loop this through features of interest?
                    'the GenomicFeatures get new settings with each loop through FeatureOfInterest in RunEnrichmentAnlysis.
                    'the problem with this new transposed way is that this output is only created once, so it doesn't have anything to create extra columns with.
                    Log10Pvalues.Clear()
                    For Each GF In AccumulatedGenomicFeatures(gfName)
                        Log10Pvalues.Add(getLog10Pvalue(GF, Settings))
                    Next
                    sw.Write(gfName & vbTab & String.Join(vbTab, Log10Pvalues) & vbCrLf)
                Next
            End Using

        End Sub

        Private Function getPvalueFilename(ByVal Settings As EnrichmentSettings) As String
            Dim name As String = ""
            If Settings.UseMonteCarlo = True Then
                If Settings.UseChiSquare = True Then
                    If Settings.OutputPCCweightedPvalueMatrix = True Then
                        name = "_MonteCarlo_ChiSquare_WeightedPearsonsCoefficient_Matrix.gr"
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = True Then
                        name = "_MonteCarlo_ChiSquare_WeightedPercentOverlap_Matrix.gr"
                    Else
                        name = "_Pvalue_MonteCarlo_ChiSquare_Matrix.gr"
                    End If
                ElseIf Settings.UseTradMC = True Then
                    If Settings.OutputPCCweightedPvalueMatrix = True Then
                        name = "_MonteCarlo_TradMC_WeightedPearsonsCoefficient_Matrix.gr"
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = True Then
                        name = "_MonteCarlo_TradMC_WeightedPercentOverlap_Matrix.gr"
                    Else
                        name = "_Pvalue_MonteCarlo_TradMC_Matrix.gr"
                    End If
                End If
            ElseIf Settings.UseAnalytical = True Then
                If Settings.UseChiSquare = True Then
                    If Settings.OutputPCCweightedPvalueMatrix = True Then
                        name = "_Analytical_ChiSquare_WeightedPearsonsCoefficient_Matrix.gr"
                    ElseIf Settings.OutputPercentOverlapPvalueMatrix = True Then
                        name = "_Analytical_ChiSquare_WeightedPercentOverlap_Matrix.gr"
                    Else
                        name = "_Pvalue_Analytical_ChiSquare_Matrix.gr"
                    End If
                End If
                If Settings.UseBinomialDistribution = True Then
                    If Settings.OutputPercentOverlapPvalueMatrix = True Then
                        name = "_Analytical_BD_WeightedPercentOverlap_Matrix.gr"
                    Else
                        name = "_Pvalue_Analytical_BinomialD_Matrix.gr"
                    End If
                End If

            End If
            Return Settings.OutputDir & Settings.EnrichmentJobName & name
        End Function

        Private Function getLog10Pvalue(ByVal GF As GenomicFeature, ByVal Settings As EnrichmentSettings) As Double
            Dim Log10Pvalue As New Double
            Dim PCC As New Double
            Dim Under As Boolean = False
            Dim PercentCoeff As New Double
            If Settings.UseMonteCarlo = True And Settings.UseTradMC = True And GF.PValueMonteCarloTradMC <> 0 Then
                Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloTradMC)
            ElseIf Settings.UseMonteCarlo = True And Settings.UseChiSquare = True And GF.PValueMonteCarloChisquare <> 0 Then
                Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare) : PCC = GF.PCCMonteCarloChiSquare
            ElseIf Settings.UseAnalytical = True And Settings.UseChiSquare = True And GF.PValueAnalyticalChisquare <> 0 Then
                Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalChisquare) : PCC = GF.PCCAnalyticalChiSquare
                If GF.ActualHits < GF.AnalyticalExpectedWithin Then Under = True
            ElseIf Settings.UseAnalytical = True And Settings.UseBinomialDistribution = True And GF.PValueAnalyticalBinomialDistribution <> 0 Then
                Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalBinomialDistribution)
                If GF.ActualHits < GF.AnalyticalExpectedWithin Then Under = True
            Else
                Return 0
            End If

            If Settings.UseMonteCarlo = True Then
                If GF.ActualHits > GF.MCExpectedHits Then
                    PercentCoeff = (GF.ActualHits / NumOfFeatures) * (1 - (GF.MCExpectedHits / NumOfFeatures))
                Else
                    Under = True : PercentCoeff = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.MCExpectedHits / NumOfFeatures)
                End If
            End If

            If Settings.UseAnalytical = True Then
                If GF.ActualHits > GF.AnalyticalExpectedWithin Then
                    PercentCoeff = (GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures))
                Else
                    Under = True : PercentCoeff = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures)
                End If
            End If

            If Settings.OutputPCCweightedPvalueMatrix = True Then Log10Pvalue = Settings.PearsonsAudjustment * PCC * Log10Pvalue

            If Settings.OutputPercentOverlapPvalueMatrix = True Then
                If Settings.SquarePercentOverlap = True Then
                    Log10Pvalue = Log10Pvalue * System.Math.Sqrt(PercentCoeff)
                Else
                    Log10Pvalue = Log10Pvalue * PercentCoeff
                End If
            End If

            If Under = True Then Log10Pvalue = -1 * Log10Pvalue
            Return Log10Pvalue
        End Function

        Private Sub OutputPValueAnalytical_Binomial_Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Pvalue_Analytical_BinomialD_Matrix.gr", Not DoOutputHeader)
                'writes the header columns

                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueAnalyticalBinomialDistribution <> 0 Then 'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalBinomialDistribution)
                        If GF.ActualHits < GF.AnalyticalExpectedWithin Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)
                        If GF.ActualHits < GF.AnalyticalExpectedWithin Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If
                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.Write(vbCrLf)
            End Using
        End Sub

        Private Sub OutputPValueAnalytical_ChiSquare_WeightedPearsonsCoefficient__Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Analytical_ChiSquare_WeightedPearsonsCoefficient_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueAnalyticalChisquare <> 0 Then 'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalChisquare)
                        Log10Pvalue = Settings.PearsonsAudjustment * GF.PCCAnalyticalChiSquare * Log10Pvalue     'multiplies the Pearson's Coefficient by the audjustment factor 
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If
                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.Write(vbCrLf)
            End Using
        End Sub


        Private Sub OutputPValueMonteCarlo_ChiSquare_WeightedPearsonsCoefficient__Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "MonteCarlo_ChiSquare_WeightedPearsonsCoefficient_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueMonteCarloChisquare <> 0 Then                                       'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare)
                        Log10Pvalue = Settings.PearsonsAudjustment * GF.PCCMonteCarloChiSquare * Log10Pvalue     'multiplies the Pearson's Coefficient by the audjustment factor 
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If

                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.Write(vbCrLf)
            End Using
        End Sub

        Private Sub OutputPValueMonteCarlo_ChiSquare_WeightedPearsonsCoefficient__Matrix_LineTransposed(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "MonteCarlo_ChiSquare_WeightedPearsonsCoefficient_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab & FeaturesOfInterestName & vbCrLf)
                End If
                'assembles a row of pvalue results
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double

                    If GF.PValueMonteCarloChisquare <> 0 Then                                       'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare)
                        Log10Pvalue = Settings.PearsonsAudjustment * GF.PCCMonteCarloChiSquare * Log10Pvalue     'multiplies the Pearson's Coefficient by the audjustment factor 
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = -1 * System.Math.Log10(Double.MinValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If

                    sw.Write(GF.Name & vbTab & Log10Pvalue & vbCrLf)
                Next
            End Using
        End Sub

        'generates a matrix weighted by percent of features of interest that overlap overlap for the Monte Carlo results
        Private Sub OutputPValueMonteCarlo_ChiSquare_WeightedPercentOverlap__Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_MonteCarlo_ChiSquare_WeightedPercentOverlap_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueMonteCarloChisquare <> 0 Then
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare)     'Take antilog10 of the p-value
                        If GF.ActualHits > GF.MCExpectedHits Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.MCExpectedHits / NumOfFeatures))) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.MCExpectedHits / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else 'For underrepresentation correction for percentage is 1-
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.MCExpectedHits / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.MCExpectedHits / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits > GF.MCExpectedHits Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.MCExpectedHits / NumOfFeatures))) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.MCExpectedHits / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else 'For underrepresentation correction for percentage is 1-
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.MCExpectedHits / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.MCExpectedHits / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If
                    End If

                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.WriteLine()
            End Using
        End Sub

        Private Sub OutputPValueMonteCarlo_ChiSquare__Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Pvalue_MonteCarlo_ChiSquare_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueMonteCarloChisquare <> 0 Then 'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare)
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If
                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.Write(vbCrLf)
            End Using
        End Sub

        Private Sub OutputPValueMonteCarlo_ChiSquare__Matrix_LineTransposed(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Pvalue_MonteCarlo_ChiSquare_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab & FeaturesOfInterestName & vbCrLf)
                End If
                'assembles a row of pvalue results
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueMonteCarloChisquare <> 0 Then 'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueMonteCarloChisquare)
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = -1 * System.Math.Log10(Double.MinValue)
                        If GF.ActualHits < GF.MCExpectedHits Then
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If
                    sw.Write(GF.Name & vbTab & Log10Pvalue & vbCrLf)
                Next
            End Using
        End Sub

        Private Sub OutputPValueAnalytical_Binomial_WeightedPercentOverlap_Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            'generates a weighted matrix for the analyitcal BD results
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Analytical_BD_WeightedPercentOverlap_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueAnalyticalBinomialDistribution <> 0 Then                                     'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalBinomialDistribution)
                        If GF.ActualHits > GF.AnalyticalExpectedWithin Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures))) * Log10Pvalue          'takes the square root of the result
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If

                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits > GF.AnalyticalExpectedWithin Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures))) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else 'For underrepresentation correction for percentage is 1-
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If
                    End If
                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.WriteLine()
            End Using
        End Sub

        Private Sub OutputPValueAnalytical_ChiSquare_WeightedPercentOverlap_Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            'generates a weighted matrix for the analyitcal results
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Analytical_ChiSquare_WeightedPercentOverlap_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueAnalyticalChisquare <> 0 Then
                        Log10Pvalue = System.Math.Log10(GF.PValueAnalyticalChisquare)     'Take antilog10 of the p-value
                        If GF.ActualHits > GF.AnalyticalExpectedWithin Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures))) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else 'For underrepresentation correction for percentage is 1-
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)     'Take antilog10 of the max value of Double type, since we can't use 0
                        If GF.ActualHits > GF.AnalyticalExpectedWithin Then
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures))) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (GF.ActualHits / NumOfFeatures) * (1 - (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue
                            End If
                        Else 'For underrepresentation correction for percentage is 1-
                            If Settings.SquarePercentOverlap = True Then
                                Log10Pvalue = System.Math.Sqrt((1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures)) * Log10Pvalue          'takes the square root of the ratio
                            Else
                                Log10Pvalue = (1 - (GF.ActualHits / NumOfFeatures)) * (GF.AnalyticalExpectedWithin / NumOfFeatures) * Log10Pvalue
                            End If
                            Log10Pvalue = -1 * Log10Pvalue  'Add - for underrepresentation
                        End If
                    End If

                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.WriteLine()
            End Using
        End Sub

        Private Sub OutputPValueAnalytical_ChiSquare_Matrix_Line(ByVal GenomicFeatures As List(Of GenomicFeature), ByVal DoOutputHeader As Boolean, ByVal Settings As EnrichmentSettings, ByVal FeaturesOfInterestName As String)
            Using sw As New StreamWriter(Settings.OutputDir & Settings.EnrichmentJobName & "_Pvalue_Analytical_ChiSquare_Matrix.gr", Not DoOutputHeader)
                'writes the header columns
                If DoOutputHeader = True Then
                    sw.Write(vbTab)
                    For Each GF In GenomicFeatures
                        sw.Write(GF.Name & vbTab)
                    Next
                    sw.Write(vbCrLf)
                End If
                'assembles a row of pvalue results
                sw.Write(FeaturesOfInterestName)
                For Each GF In GenomicFeatures
                    Dim Log10Pvalue As Double
                    If GF.PValueAnalyticalChisquare <> 0 Then 'the log10 of 0 cannot be taken so it is manually set to 0
                        Log10Pvalue = -1 * System.Math.Log10(GF.PValueAnalyticalChisquare)
                        If GF.ActualHits < GF.AnalyticalExpectedWithin Then 'Add - if the p-value related to under representation
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    Else
                        Log10Pvalue = System.Math.Log10(Double.MaxValue)    'If p-value is 0 then set it to max of Double type
                        If GF.ActualHits < GF.AnalyticalExpectedWithin Then 'Add - if the p-value related to under representation
                            Log10Pvalue = -1 * Log10Pvalue
                        End If
                    End If
                    sw.Write(vbTab & Log10Pvalue)
                Next
                sw.Write(vbCrLf)
            End Using
        End Sub

    End Class
End Namespace