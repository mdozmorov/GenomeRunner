'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Namespace GenomeRunner
    'LC 6/21/11 created 
    Public Class GenomicFeature
        Implements ICloneable
        'these values are returned from the GenomeRunner Table
        Public id As Integer 'the id of the GF in the genomerunner table
        Public Name As String 'a convenient name for the feature. ex. CTCF
        Public TableName As String 'stores the name of the table. ex. wgencodebroadchipseqpeaksgm12878ctcf.  For custom tracks the path of the custom .bed file is stored here
        Public QueryType As String 'the type of query to be performed 
        Public Threshold As Integer = 0 'the threshold that is used to filter data returns
        Public NamesToInclude As New List(Of String) 'the
        Public ThresholdType As String 'the threshold type that should be returned from the DB such as signal value or score: MUST exist as a column in the table
        Public ThresholdMin As Single 'stores the threshold min
        Public ThresholdMax As Single 'stores the threshold max
        Public ThresholdMean As Single 'stores the threshold mean
        Public UICategory As String 'stores the category that the GF is in for the UI
        Public IUOrderInCategory As Integer 'stores the order that the GF should appear in the category in the UI 
        Public Tier As Integer ' stores the tier of the GF

        'Public ThresholdMedian As Single 'stores the threshold median

        Public FeatureReturnedData As New List(Of FeaturesReturnedHits) 'stores the GR that are a positive hit for the FOI as well as the data.  There is one entry for each FOI
        Public FilteredByName As Boolean
        Public StrandToFilterBy As String = "" 'if this has a + or - than the genomic feature is filtered by the appropriate strand
        Public MCMean, MCvariance, MCskewness, MCkurtosis As Double 'stores statistical values for the monetcarlo simulation results
        Public AnalyB As Double = 0 'stores the number of base pairs that the genomic feature coveres of the genome
        Public AnalynB As Double = 0 'stores the number of regions of the genomic feature that are scatered across the genome
        Public AnalyG As Double = 0 'stores the total number of base pairs of the entire genome

        'important values for storing enrichment results
        Public ActualHits As UInteger 'Double 'store the number of actual hits
        Public MCExpectedHits As UInteger 'Double 'stores the number of expected hits through randomly selected FOI
        Public AnalyticalExpectedWithin As Double 'stores the expected within for the analytical method
        Public PValueAnalyticalBinomialDistribution As Double = Nothing 'stores the pvalue of the genomicfeature for the analytical method using Binomial Distrobution
        Public PValueAnalyticalChisquare As Double = Nothing 'stores the pvalue of the genomicfeature for the analytical method using Chi Square
        Public PValueMonteCarloBinomialDistribution As Double = Nothing 'stores the pvalue of the genomicfeature for the analytical method using bd
        Public PValueMonteCarloChisquare As Double = Nothing 'stores the pvalue of the genomicfeature for the analystical method chi square
        Public PCCMonteCarloChiSquare As Double = Nothing 'stores the Pearson's Contingency Coefficient (C) for the Chi Square 
        Public PCCAnalyticalChiSquare As Double = Nothing 'stores the Pearson's Contingency Coefficient (C) for the analytical method
        Public PValueMonteCarloTradMC As Double = Nothing ' Stores the p-value calculate via traditional Monte-Carlo simulation

        'Sets all of the values needed for the Genomic Feature. If no names are to be filtered, then "Nothing" should be passed as its argument 
        Public Sub New(ByVal id As Integer, ByVal Name As String, ByVal TableName As String, ByVal QueryType As String, ByVal ThresholdType As String, ByVal Threshold As Integer, ByVal ThresholdMin As String, ByVal ThresholdMax As String, ByVal ThresholdMean As String, ByVal Category As String, ByVal OrderInCategory As Integer, ByVal NamesToFilter As List(Of String), ByVal StrandToFilterBy As String, ByVal Tier As Integer)
            'sets the properties of the feature
            Me.id = id
            Me.Name = Name
            Me.TableName = TableName
            Me.QueryType = QueryType
            Me.ThresholdType = ThresholdType
            Me.Threshold = Threshold
            Me.ThresholdMin = ThresholdMin
            Me.ThresholdMax = ThresholdMax
            Me.ThresholdMean = ThresholdMean
            Me.UICategory = Category
            Me.IUOrderInCategory = OrderInCategory
            Me.Tier = Tier
            Me.StrandToFilterBy = StrandToFilterBy
            'adds each of the names to filter 
            If NamesToFilter IsNot Nothing Then
                For Each nameToFilter In NamesToFilter
                    Me.NamesToInclude.Add(nameToFilter)
                Next
            End If
        End Sub

        'Cloning is necessary to make a deep copy; this is used for AccumulatedGenomicFeatures in EnrichmentAnalysis.
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim GF As New GenomicFeature(id, Name, TableName, QueryType, ThresholdType, Threshold, ThresholdMin, ThresholdMax, ThresholdMean, UICategory, IUOrderInCategory, NamesToInclude, StrandToFilterBy, Tier)
            GF.FeatureReturnedData = FeatureReturnedData
            GF.FilteredByName = FilteredByName
            GF.StrandToFilterBy = StrandToFilterBy
            GF.MCMean = MCMean
            GF.MCvariance = MCvariance
            GF.MCskewness = MCskewness
            GF.MCkurtosis = MCkurtosis
            GF.AnalyB = AnalyB
            GF.AnalynB = AnalynB
            GF.AnalyG = AnalyG
            GF.ActualHits = ActualHits
            GF.MCExpectedHits = MCExpectedHits
            GF.AnalyticalExpectedWithin = AnalyticalExpectedWithin
            GF.PValueAnalyticalBinomialDistribution = PValueAnalyticalBinomialDistribution
            GF.PValueAnalyticalChisquare = PValueAnalyticalChisquare
            GF.PValueMonteCarloBinomialDistribution = PValueMonteCarloBinomialDistribution
            GF.PValueMonteCarloChisquare = PValueMonteCarloChisquare
            GF.PValueMonteCarloTradMC = PValueMonteCarloTradMC
            GF.PCCMonteCarloChiSquare = PCCMonteCarloChiSquare
            GF.PCCAnalyticalChiSquare = PCCAnalyticalChiSquare
            'GF.RandTie = RandTie
            'GF.RandUnder = RandUnder
            'GF.RandOver = RandOver
            Return GF
        End Function

    End Class

    ' stores the data on GR features that are positive
    Public Class FeaturesReturnedHits
        Public CountData As UInteger 'stores the number of GR that overlap the FOI
        Public fStartData As New List(Of Integer) 'the start position of the GR
        Public fEndData As New List(Of Integer)  'the end position of the GR
        Public StrandData As New List(Of String)  'the strand direction of the GR; usually '+' or '-'
        Public NameData As New List(Of String)   'the name of the GR
        Public ThresholdData As New List(Of Object) 'The threshold of the GR; can be used to store signal value, score, etc.
        Public OverLapTypeData As New List(Of String)  'the type of overlap; ex. 'within', overlap, overhang etc.
        Public OverLapAmountData As New List(Of Integer) 'LC 6/20/11 A generic that is used to store information retrieved from the db  
    End Class

    'stores the data retrieved from the mysql for the GR feature
    Public Class FeatureSQLData
        Public Chrom As String, ChromStart As Integer, ChromEnd As Integer, Strand As String, Name As String, Threshold As Object
    End Class

    'stores the FOI
    Public Class Feature
        Public Chrom As String, ChromStart As Integer = -1, ChromEnd As Integer = -1, Name As String
    End Class

End Namespace