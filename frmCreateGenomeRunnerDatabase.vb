'Mikhail G. Dozmorov, Lukas R. Cara, Cory B. Giles, Jonathan D. Wren. "GenomeRunner: Automating genome exploration". 2011
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net
Imports alglib.hqrnd
Imports GenomeRunner.GenomeRunner
Imports System.Windows.Forms.ListViewItem


Public Class frmCreateGenomeRunnerDatabase

    Dim listGRFeatures As List(Of Feature) 'used to store feature values that are retrieved from the database
    Dim cn As MySqlConnection, cmd As MySqlCommand, dr As MySqlDataReader, cmd1 As MySqlCommand, dr1 As MySqlDataReader
    Dim ftpHost As String = "ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/", ftpUser As String = "anonymous", ftpPassword As String = ""
    Dim uName As String = "", uServer As String = "", uPassword As String = "", uDatabase As String = ""
    Dim fileName As String, numOfFeaturesToAdd As Integer = 0, progress As Integer = 1 'stores the value of the progress bar
    Dim ConnectionStringLocal As String                                          'the connection string to the database to which the genomic features are to be added
    Dim Features As New List(Of Feature)
    Dim RandomFeatures As New List(Of Feature)
    Dim featureTables As New List(Of String)
    Dim bkgNum As Integer
    Dim ConnectionStringHost As String
    Dim strDownloadedTableDir As String

    Property DownloadedTableDir As String                                                                'stores the directory where the downloaded table data is to be saved to
        Get
            Return strDownloadedTableDir
        End Get
        Set(ByVal value As String)
            strDownloadedTableDir = value
            lblDownDir.Text = "Download Directory: " & value
            ToolTip1.SetToolTip(lblDownDir, DownloadedTableDir)
        End Set
    End Property

    'the query that is used to create the GenomeRunnerTable
    Dim CreateGenomeRunnerTableQuery As String = "DROP TABLE IF EXISTS `genomerunner`;" & _
    "CREATE TABLE `genomerunner` (" & _
"`id` int(11) NOT NULL AUTO_INCREMENT," & _
"`FeatureTable` varchar(200) DEFAULT NULL," & _
"`FeatureName` varchar(300) DEFAULT NULL," & _
"`QueryType` varchar(45) DEFAULT 'NA'," & _
"`ThresholdType` varchar(45) DEFAULT 'NA'," & _
"`ThresholdMin` float DEFAULT '0'," & _
"`ThresholdMax` float DEFAULT '0'," & _
"`ThresholdMean` float DEFAULT '0'," & _
"`ThresholdMedian` float DEFAULT '0'," & _
"`Tier` int(11) DEFAULT '3'," & _
"`Category` varchar(45) DEFAULT '1000|unknown'," & _
" `orderofcategory` int(11) DEFAULT '0'," & _
" PRIMARY KEY (`id`)" & _
") ENGINE=InnoDB AUTO_INCREMENT=442 DEFAULT CHARSET=utf8;"



    Public Sub CreateGenomeRunnerTable(ByVal ConnectionStringofHost As String, ByVal ConnectionStringDestination As String)
        'queries the entire genomerunner table on the host server and downloads the content into a local text file
        cn = New MySqlConnection(ConnectionStringofHost) : cn.Open()
        cmd = New MySqlCommand("SELECT id,featuretable,featurename,querytype,thresholdtype,thresholdmin,thresholdmax, thresholdmean,thresholdmedian,tier,category,orderofcategory FROM genomerunner", cn)
        dr = cmd.ExecuteReader()
        Using sr As New StreamWriter(Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location) & "\" & "GenomeRunnerTableData.txt")
            While dr.Read()
                sr.WriteLine(dr(0) & vbTab & dr(1) & vbTab & dr(2) & vbTab & "*" & dr(3) & vbTab & dr(4) & vbTab & dr(5) & vbTab & dr(6) & vbTab & dr(7) & vbTab & dr(8) & vbTab & dr(9) & vbTab & dr(10) & vbTab & dr(11))
            End While
        End Using
        dr.Close() : cn.Close()

        'creates the local genomerunner table and loads the data that was downloaded from the host server into it
        cn = New MySqlConnection(ConnectionStringDestination) : cn.Open()
        cmd = New MySqlCommand(CreateGenomeRunnerTableQuery, cn)
        cmd.ExecuteNonQuery()
        cn.Close()

        'loads the downloaded genomerunner table data into the newly created local genomerunner table
        cn = New MySqlConnection(ConnectionStringDestination) : cn.Open()
        Dim dataPath As String = (Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location) & "\" & "GenomeRunnerTableData.txt").Replace("\", "/")
        cmd = New MySqlCommand("LOAD DATA INFILE '" & dataPath & "' INTO TABLE genomerunner", cn)
        cmd.ExecuteNonQuery()
        cn.Close()
    End Sub


    Public Sub New(ByVal ConnectionStringHost)

        ' This call is required by the designer.
        InitializeComponent()
        Me.ConnectionStringHost = ConnectionStringHost
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'connects to the db
    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        listGenomicFeatures.Clear()
        Dim uName As String = txtUser.Text
        Dim uServer As String = txtHost.Text
        Dim uPassword As String = txtPassword.Text
        Dim uDatabase As String = txtDatabase.Text
        Dim ConnectionWorks As Boolean = False

        Try
            ConnectionStringLocal = "Server=" & uServer & ";Database=" & uDatabase & ";User ID=" & uName & ";Password=" & uPassword & ";Connection Timeout=6000;" 'uses the values provided by user to create constring.
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            cn.Close()
            ConnectionWorks = True
        Catch
            MessageBox.Show("Connection failed, please check connection settings")
        End Try
        If ConnectionWorks = True Then
            Dim genomicFeatures As List(Of GenomicFeature) = GetGenomicFeaturesToAdd()                      'gets all of the genomic features in the genomerunner table
            For Each GF In genomicFeatures
                Dim listItemGenomicFeature As New listviewGenomicFeature(GF)                                'creates a list view item that and attaches the genomic feature to it.
                listItemGenomicFeature.Text = GF.TableName
                listItemGenomicFeature.ToolTipText = "Table name: " & GF.Name
                listGenomicFeatures.Items.Add(listItemGenomicFeature)
            Next
            btnDownloadGenomicFeatureTable.Enabled = True
            If genomicFeatures.Count = 0 Then Exit Sub
            CheckGenomeFeatureDatabaseStatus()
            btnDownloadGenomicFeatureTable.Enabled = True
            btnCreateTables.Enabled = True
            btnLoadData.Enabled = True
            btnPrepareFiles.Enabled = True
            btnGenerateExonTables.Enabled = True
            btnAddRepeatMasker.Enabled = True
            btnCheckIntegrity.Enabled = True
            btnDownloadGenomicFeatureTable.Enabled = True
        End If
    End Sub


    Private Sub CheckGenomeFeatureDatabaseStatus()
        Dim listTablesInDatabase As New List(Of String)                                             'stores all of the table names that exist in the database

        'gest all of the table names that exist in the database
        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
        cmd = New MySqlCommand("SHOW tables", cn)
        dr = cmd.ExecuteReader()
        While dr.Read()
            listTablesInDatabase.Add(dr(0))
        End While
        cn.Close() : dr.Close()

        'checks if the table is in the database
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If listTablesInDatabase.IndexOf(GF.GFeature.TableName.ToLower()) <> -1 Then
                GF.IsTableAdded = True
            Else
                GF.IsTableAdded = False
            End If
        Next

        'checks if the tables are not empty
        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            Try
                If GF.IsTableAdded = True Then
                    cmd = New MySqlCommand("SELECT * FROM " & GF.GFeature.TableName & " LIMIT 1", cn)
                    dr = cmd.ExecuteReader()
                    If dr.HasRows Then
                        GF.IsTablePopulated = True
                    Else
                        GF.IsTablePopulated = False
                    End If
                    dr.Close() : cmd.Dispose()
                End If
            Catch
                GF.IsTableAdded = False : GF.IsTablePopulated = False
            End Try
        Next
        cn.Close()

        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.IsTableAdded = True And GF.IsTablePopulated = True And (GF.GFeature.QueryType = "*Gene" Or GF.GFeature.QueryType = "Gene") Then
                If listTablesInDatabase.IndexOf(GF.GFeature.TableName.ToLower() & "exons") <> -1 Then
                    GF.IsExonTableExisting = True
                Else
                    GF.IsExonTableExisting = False
                End If
            End If
        Next
    End Sub


    'gets all of the genomic in the genomic feature table
    Private Function GetGenomicFeaturesToAdd() As List(Of GenomicFeature)
        Dim listGenomicFeatures As New List(Of GenomicFeature)
        Try
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            cmd = New MySqlCommand("SELECT id,FeatureTable,FeatureName,QueryType FROM genomerunner", cn)
            dr = cmd.ExecuteReader()
            While dr.Read()
                '...creates a genomic feature class to store the relavent data that will be used to generate the tables needed for the genomic feature
                Dim nGF As New GenomicFeature(dr(0), dr(2), dr(1), dr(3), "", 0, 0, 0, 0, 0, 0, Nothing, "", 0)
                listGenomicFeatures.Add(nGF)
            End While
            btnDownloadGenomicFeatureTable.Enabled = True
            btnCreateTables.Enabled = True
            btnLoadData.Enabled = True
            btnPrepareFiles.Enabled = True
            lblGenomeRunnerTableStatus.Text = "GenomeRunner Table Status: Exists"
            btnDownloadGenomicFeatureTable.Text = "Table Exists: Re-Download GenomeRunner Table"
            Return listGenomicFeatures
        Catch
            lblGenomeRunnerTableStatus.Text = "GenomeRunner Table Status: Missing"
            btnDownloadGenomicFeatureTable.Text = "Table Missing: Download GenomeRunner Table"
        End Try
        Return listGenomicFeatures
    End Function

    'downloads and decompresses the feature 
    Private Sub PrepareFeatures(ByVal GenomicFeatureTableName As String)
        Try
            If GenomicFeatureTableName.ToLower() <> "rmsk" And GenomicFeatureTableName.ToLower() <> "rmskrm327" Then
                'downloads the GenomicFeature table data if it has not yet been downlaoded
                If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".sql") = False Then
                    lblProgress.Text = "Downloading " & GenomicFeatureTableName & ".sql" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", DownloadedTableDir & GenomicFeatureTableName & ".sql", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .sql feature file
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", AppDomain.CurrentDomain.BaseDirectory & "FeaturesToAdd\" & GenomicFeatureTableName & ".sql") 'downloads the .sql feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", DownloadedTableDir & GenomicFeatureTableName & ".sql") 'downloads the .sql feature file
                End If
                If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".txt.gz") = False Then
                    lblProgress.Text = "Downloading " & GenomicFeatureTableName & ".txt.gz" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".txt.gz", DownloadedTableDir & GenomicFeatureTableName & ".txt.gz", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .txt.gz feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".txt.gz", DownloadedTableDir & GenomicFeatureTableName & ".txt.gz") 'downloads the .txt.gz feature file

                End If
                'decompresses the .gz file to a .txt file
                If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".txt") = False Then
                    lblProgress.Text = "Decompressing " & GenomicFeatureTableName : Application.DoEvents()
                    Using outfile As FileStream = File.Create(DownloadedTableDir & GenomicFeatureTableName & ".txt")
                        Using infile As FileStream = File.OpenRead(DownloadedTableDir & GenomicFeatureTableName & ".txt.gz")
                            Using Decompress As System.IO.Compression.GZipStream = New System.IO.Compression.GZipStream(infile, Compression.CompressionMode.Decompress)
                                Decompress.CopyTo(outfile)
                            End Using
                        End Using
                    End Using
                End If
            End If
        Catch e As Exception
            MessageBox.Show("Unable to download files for " & GenomicFeatureTableName & vbCrLf & e.Message)
            'deletes files to prevent partial files from existing
            If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".txt.gz") = True Then File.Delete(DownloadedTableDir & GenomicFeatureTableName & ".txt.gz")
            If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".sql") = True Then File.Delete(DownloadedTableDir & GenomicFeatureTableName & ".sql")
            If File.Exists(DownloadedTableDir & GenomicFeatureTableName & ".txt") = True Then File.Delete(DownloadedTableDir & GenomicFeatureTableName & ".txt")
        End Try
    End Sub

    'adds the features to the database
    Private Sub AddFeaturesToDatabase()

    End Sub

    'downloads the table data for all of the Genomic Features that whose tables have not been added yet.
    Private Sub btnPrepareFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrepareFiles.Click
        Dim openFD As New OpenFileDialog

        Dim GFTablesToAddCount As Integer = 0                                                            'keeps count of the number of tables that must be added
        'counts the number of tables that will have to be added so that the progress bar value can be set
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.IsTableAdded = False Or GF.IsTablePopulated = False Then
                GFTablesToAddCount += 1
            End If
        Next

        ProgressBar1.Maximum = GFTablesToAddCount
        ProgressBar1.Value = 0
        'goes through the genomic features and downlaods the table data for the genomic features whose tables have not been added
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.IsTableAdded = False Or GF.IsTablePopulated = False Then
                PrepareFeatures(GF.GFeature.TableName)
                ProgressBar1.Value += 1 : Application.DoEvents()
            End If
        Next
        ProgressBar1.Value = 0
        lblProgress.Text = "Done"
    End Sub

    Private Sub FormMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DownloadDirSet As Boolean = False
        While DownloadDirSet = False
            Try
                'gets the path to where the genomic feature data should be saved
                DownloadedTableDir = GetSetting("GenomeRunner", "Database", "DownloadDir")
                If DownloadedTableDir <> "" Then
                    DownloadDirSet = True
                Else
                    'if it's blank then the user is prompted to provide a valid directory which is saved.  
                    FolderBrowserDialog1.ShowNewFolderButton = True
                    Dim DidWork As Integer = FolderBrowserDialog1.ShowDialog()
                    If DidWork = DialogResult.Cancel Then
                        MessageBox.Show("Select folder for data files download", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Me.Close()
                    Else
                        DownloadedTableDir = FolderBrowserDialog1.SelectedPath & "\"
                        SaveSetting("GenomeRunner", "Database", "DownloadDir", DownloadedTableDir)
                        btnDownloadGenomicFeatureTable.Enabled = True
                        DownloadDirSet = True
                    End If
                End If
            Catch
                FolderBrowserDialog1.ShowNewFolderButton = True
                Dim DidWork As Integer = FolderBrowserDialog1.ShowDialog()
                If DidWork = DialogResult.Cancel Then
                    MessageBox.Show("Select folder for data files download", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Me.Close()
                Else
                    DownloadedTableDir = FolderBrowserDialog1.SelectedPath & "\"
                    SaveSetting("GenomeRunner", "Database", "DownloadDir", DownloadedTableDir)
                    btnDownloadGenomicFeatureTable.Enabled = True
                    DownloadDirSet = True
                End If
            End Try
        End While
    End Sub

    Private Sub btnDownloadGenomicFeatureTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownloadGenomicFeatureTable.Click
        CreateGenomeRunnerTable(ConnectionStringHost, ConnectionStringLocal)
        listGenomicFeatures.Clear()
        Dim genomicFeatures As List(Of GenomicFeature) = GetGenomicFeaturesToAdd()                      'gets all of the genomic features in the genomerunner table
        For Each GF In genomicFeatures
            Dim listItemGenomicFeature As New listviewGenomicFeature(GF)                                'creates a list view item that and attaches the genomic feature to it.
            listItemGenomicFeature.Text = GF.Name
            listItemGenomicFeature.ToolTipText = "Table name: " & GF.TableName
            listGenomicFeatures.Items.Add(listItemGenomicFeature)
        Next
        CheckGenomeFeatureDatabaseStatus()
        btnCheckIntegrity.Enabled = True
        btnGenerateExonTables.Enabled = True
        btnAddRepeatMasker.Enabled = True
    End Sub

    Private Sub btnLoadData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadData.Click
        ProgressBar1.Value = 0
        Dim listTablesInDatabase As New List(Of String)                                             'stores all of the table names that exist in the database

        'loads the data from the downloaded files into the genomic feature table for those that exist
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            lblProgress.Text = "Loading data for " & GF.GFeature.Name : Application.DoEvents()
            Dim DownloadedTableDirToAddUNIX As String = DownloadedTableDir.Replace("\", "/") 'modifies the filepath to conform to UNIX file path format
            Dim query As String = "TRUNCATE TABLE " & GF.GFeature.TableName & "; LOAD DATA LOCAL INFILE " & "'" & DownloadedTableDirToAddUNIX & GF.GFeature.TableName & ".txt' INTO TABLE " & GF.GFeature.TableName
            Dim QueryPath As String = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location) & "\" & "mysqlquery.txt"
            Using sw As New StreamWriter(QueryPath)
                sw.WriteLine(query)
            End Using


            If GF.IsTableAdded = True And GF.IsTablePopulated = False Then
                Try
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand(query, cn) 'reads the .txt file and inserts the data into the created table
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Dim form As New frmQuery
                    form.txtQuery.Text = query
                    form.ShowDialog()

                End Try
                cn.Close()
            End If
        Next
        CheckGenomeFeatureDatabaseStatus()
        lblProgress.Text = "Done"
        btnCheckIntegrity.Enabled = True
        btnGenerateExonTables.Enabled = True
        btnAddRepeatMasker.Enabled = True
    End Sub

    Private Sub btnCreateTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTables.Click
        ProgressBar1.Maximum = listGenomicFeatures.Items.Count
        ProgressBar1.Value = 0
        lblProgress.Text = "Creating genomic feature tables"
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If File.Exists(DownloadedTableDir & GF.GFeature.TableName & ".sql") = True Then
                Dim filestream As String = File.ReadAllText(DownloadedTableDir & GF.GFeature.TableName & ".sql")
                Dim query As String = ""
                'continues giving the user the prompt to correct the query untill they decide to skip the feature 
                While GF.IsTableAdded = False
                    If File.Exists(DownloadedTableDir & ".sql") = True And File.Exists(DownloadedTableDir & GF.GFeature.TableName & ".txt.gz") = True Or File.Exists(DownloadedTableDir & GF.GFeature.TableName & ".txt") Then 'continues if both required files are found
                        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()

                        'modifies the .sql query downloaded from UCSC to fix some outdated query perameters as well as optimize it
                        query = filestream.Replace("longblob", "longtext")
                        query = query.Replace("TYPE", "ENGINE")
                        query = query.Replace("InnoDB", "myisam")
                        query = query.Replace("SET character_set_client = @saved_cs_client;", " ")
                        query = query.Replace("SET @saved_cs_client     = @@character_set_client;", " ")
                        query = query.Replace("SET character_set_client = utf8;", " ")
                        query = query.Insert(0, "DROP TABLE IF EXISTS " & GF.GFeature.TableName & "; ")

                        Try
                            cmd = New MySqlCommand(query, cn) 'executes the query
                            cmd.ExecuteNonQuery()
                            GF.IsTableAdded = True
                            File.WriteAllText(DownloadedTableDir & GF.GFeature.TableName & ".sql", query) 'replaces the query file with a new query

                        Catch ex As Exception
                            Dim form As New FormMysqlQueryEditor() 'creates a form for the user to change the query
                            form.originQuery = filestream.Replace("longblob", "longtext")
                            form.LabelStatus.Text = "There was an error executing the query.  Please review the query for proper formatting and resubmit."
                            form.LabelErrorMessage.Text = "Error Message: " & ex.Message
                            form.ShowDialog()
                            query = form.editedQuery

                        End Try
                        cn.Close()
                    End If
                End While
            End If
            ProgressBar1.Value += 1 : Application.DoEvents()
        Next
        ProgressBar1.Value = 0
        lblProgress.Text = "Done"
    End Sub

    Private Sub btnSetDownloadDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetDownloadDir.Click
        FolderBrowserDialog1.ShowNewFolderButton = True
        FolderBrowserDialog1.ShowDialog()
        DownloadedTableDir = FolderBrowserDialog1.SelectedPath & "\"
        SaveSetting("GenomeRunner", "Database", "DownloadDir", DownloadedTableDir)
        btnCheckIntegrity.Enabled = True
        btnGenerateExonTables.Enabled = True
        btnAddRepeatMasker.Enabled = True
    End Sub

    'activate the genomic feature in the genomerunner table if the part of the database that it utilizes is complete
    Private Sub syncGenomeRunnerTableTodatatables()
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            '...the genomic features must have a functional data table before they are activated in the genomerunner table
            If GF.GFeature.QueryType <> "*Gene" Then
                '...activates the genomic feature in the genomerunner table
                If GF.IsTableAdded = True And GF.IsTablePopulated = True And GF.GFeature.QueryType.Substring(0, 1) = "*" Then
                    Dim ActivatedQueryType As String = GF.GFeature.QueryType.Remove(0, 1)                           'contains the querytype without the *, this allows the genomic feature to be used by Genome Runner
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand("UPDATE genomerunner SET querytype='" & ActivatedQueryType & "' WHERE ID='" & GF.GFeature.id & "'", cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                    '...deactivates the genomic feature in the genomerunner table
                ElseIf (GF.IsTableAdded = False Or GF.IsTablePopulated = False) And GF.GFeature.QueryType.Substring(0, 1) <> "*" Then
                    Dim DeActivatedQueryType As String = "*" & GF.GFeature.QueryType
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand("UPDATE genomerunner SET querytype='" & DeActivatedQueryType & "' WHERE ID='" & GF.GFeature.id & "'", cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End If
                'the genomic features that have a gene query type must have a exon table in the database as well
            Else
                '...activates the genomic feature in the genomerunner table
                If GF.IsExonTableExisting = True And GF.IsTableAdded = True And GF.IsTablePopulated = True And GF.GFeature.QueryType.Substring(0, 1) = "*" Then
                    Dim ActivatedQueryType As String = GF.GFeature.QueryType.Remove(0, 1)                           'contains the querytype without the *, this allows the genomic feature to be used by Genome Runner
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand("UPDATE genomerunner SET querytype='" & ActivatedQueryType & "' WHERE ID='" & GF.GFeature.id & "'", cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                ElseIf (GF.IsExonTableExisting = False Or GF.IsTableAdded = False Or GF.IsTablePopulated = False) And GF.GFeature.QueryType.Substring(0, 1) <> "*" Then
                    '...deactivates the genomic feature in the genomerunner table
                    Dim DeActivatedQueryType As String = "*" & GF.GFeature.QueryType
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand("UPDATE genomerunner SET querytype='" & DeActivatedQueryType & "' WHERE ID='" & GF.GFeature.id & "'", cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End If
            End If
        Next
    End Sub

    Private Sub btnGenerateExonTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateExonTables.Click
        'goes through all of the genomic features and for those that gene features, this method generates an exon table. 
        ProgressBar1.Value = 0 : ProgressBar1.Maximum = listGenomicFeatures.Items.Count
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.IsExonTableExisting = False And (GF.GFeature.QueryType = "*Gene" Or GF.GFeature.QueryType = "Gene") Then
                'extracts the exon data from the database
                Dim exons As New List(Of Exon)
                cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                cmd = New MySqlCommand("SELECT name,chrom,strand, exonStarts,exonEnds FROM " & GF.GFeature.TableName, cn)
                dr = cmd.ExecuteReader()
                If dr.HasRows Then
                    While dr.Read()
                        Dim nexon As New Exon
                        nexon.name = dr(0)
                        nexon.chrom = dr(1)
                        nexon.strand = dr(2)
                        nexon.exonStart = dr(3)
                        nexon.exonEnd = dr(4)
                        exons.Add(nexon)
                    End While
                End If
                cn.Close() : cmd.Dispose()

                lblProgress.Text = "Preparing exon data for " & GF.GFeature.TableName : Application.DoEvents()
                createNewExonTableStructure(exons) 'splits up the exon end and start coordinates and orginizes them into lists
                lblProgress.Text = "Creating Exon table and populating with data" : Application.DoEvents()
                CreateExonTable(exons, GF.GFeature) 'create the exon table
                ProgressBar1.Value += 1 : Application.DoEvents()
            End If
        Next
        CheckGenomeFeatureDatabaseStatus()
        lblProgress.Text = "Done"
    End Sub

    'creates the table and populates it with the new data
    Private Sub CreateExonTable(ByVal Exons As List(Of Exon), ByVal GFeatre As GenomicFeature)
        ProgressBar1.Maximum = Exons.Count
        ProgressBar1.Value = 0
        Dim filepath = Application.StartupPath & "\ExonData.txt"

        'writes the exon data to a .txt file so that it can quickly be imported into that database
        lblProgress.Text = "Writting exon data to text file for quick import into database"
        Using writer As New StreamWriter(filepath, False)
            For Each currexon In Exons
                For x As Integer = 0 To currexon.listExonStart.Count - 1 Step +1
                    writer.WriteLine(currexon.listName(x) & vbTab & currexon.listChrom(x) & vbTab & currexon.listStrand(x) & vbTab & currexon.listExonStart(x) & vbTab & currexon.listExonEnd(x))
                Next
                writer.Flush()
            Next
        End Using

        'creates a new table
        lblProgress.Text = "Creating new table called " & GFeatre.TableName & "Exons" : Application.DoEvents()
        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
        cmd = New MySqlCommand("DROP TABLE IF EXISTS " & GFeatre.TableName & "Exons; CREATE  TABLE `" & GFeatre.TableName & "Exons` (`name` VARCHAR(50) NULL , `chrom` VARCHAR(45) NULL, `strand` VARCHAR(45) NULL , `exonStart` INT NULL ,  `exonEnd` INT NULL); ", cn)
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        cn.Close() : dr.Close()

        'reads the data in the text file into the new exon table
        Dim query As String = ""
        lblProgress.Text = "Creating new table called " & GFeatre.TableName & "Exon" : Application.DoEvents()
        Try
            'clears all data that might have existed in the Exon table
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            query = "TRUNCATE TABLE " & GFeatre.TableName & "Exons;"
            cmd = New MySqlCommand(query, cn)
            cmd.ExecuteNonQuery()
            cmd.Dispose() : cn.Close() : dr.Close()
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()

            'loads the new data into the exon table
            Dim filePathFeatureToAddUNIX As String = filepath.Replace("\", "/") 'modifies the filepath to conform to UNIX
            query = "LOAD DATA LOCAL INFILE " & "'" & filePathFeatureToAddUNIX & "' INTO TABLE " & GFeatre.TableName & "Exons;"
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            cmd = New MySqlCommand(query, cn)
            cmd.ExecuteNonQuery()
            cmd.Dispose() : cn.Close() : dr.Close()
        Catch
            Dim form As New frmQuery
            form.txtQuery.Text = query
            form.ShowDialog()
        End Try
    End Sub

    'splits up the exon end and start coordinates and orginizes them into lists
    Private Sub createNewExonTableStructure(ByVal exons As List(Of Exon))
        ProgressBar1.Maximum = exons.Count
        ProgressBar1.Value = 0
        For Each currExon In exons
            'initilizes the lists
            currExon.listExonStart = New List(Of Integer)
            currExon.listExonEnd = New List(Of Integer)
            currExon.listName = New List(Of String)
            currExon.listChrom = New List(Of String)
            currExon.listStrand = New List(Of String)

            'gets the exon start coordinates
            Dim exoncoordinates As String()
            'populates the exonstarts list
            exoncoordinates = Split(currExon.exonStart, ",")
            'converts the array items from a string to a integer and adds them to the array
            For x As Integer = 0 To exoncoordinates.Count - 1
                If IsNumeric(exoncoordinates(x)) = True Then : currExon.listExonStart.Add((CInt(exoncoordinates(x)))) : End If
            Next

            exoncoordinates = Split(currExon.exonEnd, ",")
            'converts the array items from a string to a integer and adds them to the array as well as populates the name
            For x As Integer = 0 To exoncoordinates.Count - 1
                If IsNumeric(exoncoordinates(x)) = True Then
                    currExon.listExonEnd.Add((CInt(exoncoordinates(x))))
                    currExon.listName.Add(currExon.name)
                    currExon.listChrom.Add(currExon.chrom)
                    currExon.listStrand.Add(currExon.strand)
                End If
            Next
            ProgressBar1.Value += 1
        Next
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        syncGenomeRunnerTableTodatatables()
        MessageBox.Show("The genomic features availability status in GenomeRunner successfully updated")
        Me.Close()
    End Sub

    Private Sub btnAddRepeatMasker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRepeatMasker.Click
        Dim bkgChr(24) As String
        bkgChr = {"chr1", "chr10", "chr11", "chr12", "chr13", "chr14", "chr15", "chr16", "chr17", "chr18", "chr19", "chr2", "chr20", "chr21", "chr22", "chr3", "chr4", "chr5", "chr6", "chr7", "chr8", "chr9", "chrM", "chrX", "chrY"}
        'generates the file names, without the extension, of the rmsk files on non-random chromosomes
        Dim fileNamesrmsk As New List(Of String)
        For i As Integer = 0 To bkgChr.Count - 1
            fileNamesrmsk.Add(bkgChr(i) & "_rmsk")
        Next
        'generates the file names, without the extension, of the rmsk files on non-random chromosomes
        For i As Integer = 0 To bkgChr.Count - 1
            fileNamesrmsk.Add(bkgChr(i) & "_random_rmsk")
        Next

        'downloads the rmsk table data if it has not yet been downlaoded
        lblProgress.Text = "Downloading files for rmsk" : Application.DoEvents()
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = fileNamesrmsk.Count
        For Each currfileName In fileNamesrmsk
            If currfileName <> "chr12_random_rmsk" And currfileName <> "chr14_random_rmsk" And currfileName <> "chr20_random_rmsk" And currfileName <> "chrM_random_rmsk" And currfileName <> "chrY_random_rmsk" Then
                If File.Exists(DownloadedTableDir & currfileName & ".sql") = False Then
                    lblProgress.Text = "Downloading " & currfileName & ".sql" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", DownloadedTableDir & GenomicFeatureTableName & ".sql", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .sql feature file
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", AppDomain.CurrentDomain.BaseDirectory & "FeaturesToAdd\" & GenomicFeatureTableName & ".sql") 'downloads the .sql feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & currfileName & ".sql", DownloadedTableDir & currfileName & ".sql") 'downloads the .sql feature file
                End If
                If File.Exists(DownloadedTableDir & currfileName & ".txt.gz") = False Then
                    lblProgress.Text = "Downloading " & currfileName & ".txt.gz" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".txt.gz", DownloadedTableDir & GenomicFeatureTableName & ".txt.gz", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .txt.gz feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & currfileName & ".txt.gz", DownloadedTableDir & currfileName & ".txt.gz") 'downloads the .txt.gz feature file

                End If
                'decompresses the .gz file to a .txt file
                If File.Exists(DownloadedTableDir & currfileName & ".txt") = False Then
                    lblProgress.Text = "Decompressing " & currfileName : Application.DoEvents()
                    Using outfile As FileStream = File.Create(DownloadedTableDir & currfileName & ".txt")
                        Using infile As FileStream = File.OpenRead(DownloadedTableDir & currfileName & ".txt.gz")
                            Using Decompress As System.IO.Compression.GZipStream = New System.IO.Compression.GZipStream(infile, Compression.CompressionMode.Decompress)
                                Decompress.CopyTo(outfile)
                            End Using
                        End Using
                    End Using
                End If
            End If
            ProgressBar1.Value += 1 : Application.DoEvents()
        Next

        'delets pervious versions of the file
        If File.Exists(DownloadedTableDir & "rmsk.txt") = True Then File.Delete(DownloadedTableDir & "rmsk.txt")

        'combines the seperate rmsk files by chrom into a single master .txt file
        Using swRMSK As New StreamWriter(DownloadedTableDir & "rmsk.txt", True)
            For Each curFileName In fileNamesrmsk
                If curFileName <> "chr12_random_rmsk" And curFileName <> "chr14_random_rmsk" And curFileName <> "chr20_random_rmsk" And curFileName <> "chrM_random_rmsk" And curFileName <> "chrY_random_rmsk" Then
                    Using sr As New StreamReader(DownloadedTableDir & curFileName & ".txt")
                        While sr.EndOfStream = False
                            Dim line As String() = sr.ReadLine().Split(vbTab)
                            swRMSK.WriteLine(line(5) & vbTab & line(6) & vbTab & line(7) & vbTab & line(9) & vbTab & line(10) & vbTab & line(11) & vbTab & line(12))
                        End While
                    End Using
                End If
            Next
        End Using

        'create the rmsk tables
        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
        cmd = New MySqlCommand("DROP TABLE IF EXISTS rmsk; CREATE  TABLE `rmsk` ( `chrom` VARCHAR(45) NULL , `chromStart` INT NULL , `chromEnd` INT NULL , `strand` VARCHAR(45) NULL , `repName` VARCHAR(255) NULL , `repClass` VARCHAR(255) NULL , `name` VARCHAR(255) NULL );", cn)
        cmd.ExecuteNonQuery()
        cn.Close() : cmd.Dispose()


        Dim query As String = ""
        lblProgress.Text = "Creating new table for rmsk" : Application.DoEvents()
        Dim filePath As String = DownloadedTableDir & "rmsk.txt"
        Try
            'loads the new data into the rmsk table
            Dim filePathFeatureToAddUNIX As String = filePath.Replace("\", "/") 'modifies the filepath to conform to UNIX
            query = "TRUNCATE TABLE rmsk; LOAD DATA LOCAL INFILE " & "'" & filePathFeatureToAddUNIX & "' INTO TABLE rmsk;"
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            cmd = New MySqlCommand(query, cn)
            cmd.ExecuteNonQuery()
            cmd.Dispose() : cn.Close() : dr.Close()
        Catch
            Dim form As New frmQuery
            form.txtQuery.Text = query
            form.ShowDialog()
        End Try

        'generates the file names, without the extension, of the rmsk files on non-random chromosomes
        Dim fileNamesrmsk327 As New List(Of String)
        For i As Integer = 0 To bkgChr.Count - 1
            fileNamesrmsk327.Add(bkgChr(i) & "_rmskRM327")
        Next
        'generates the file names, without the extension, of the rmsk files on non-random chromosomes
        For i As Integer = 0 To bkgChr.Count - 1
            fileNamesrmsk327.Add(bkgChr(i) & "_random_rmskRM327")
        Next

        'downloads the rmsk table data if it has not yet been downlaoded
        lblProgress.Text = "Downloading files for rmskRM327" : Application.DoEvents()
        ProgressBar1.Value = 0 : ProgressBar1.Maximum = fileNamesrmsk327.Count
        For Each currfileName In fileNamesrmsk327
            If currfileName <> "chr12_random_rmskRM327" And currfileName <> "chr14_random_rmskRM327" And currfileName <> "chr20_random_rmskRM327" And currfileName <> "chrM_random_rmskRM327" And currfileName <> "chrY_random_rmskRM327" Then
                If File.Exists(DownloadedTableDir & currfileName & ".sql") = False Then
                    lblProgress.Text = "Downloading " & currfileName & ".sql" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", DownloadedTableDir & GenomicFeatureTableName & ".sql", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .sql feature file
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".sql", AppDomain.CurrentDomain.BaseDirectory & "FeaturesToAdd\" & GenomicFeatureTableName & ".sql") 'downloads the .sql feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & currfileName & ".sql", DownloadedTableDir & currfileName & ".sql") 'downloads the .sql feature file
                End If
                If File.Exists(DownloadedTableDir & currfileName & ".txt.gz") = False Then
                    lblProgress.Text = "Downloading " & currfileName & ".txt.gz" : Application.DoEvents()
                    'My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & GenomicFeatureTableName & ".txt.gz", DownloadedTableDir & GenomicFeatureTableName & ".txt.gz", "Anonymous", "caral@OMRF.org", True, 100000, True, FileIO.UICancelOption.ThrowException) 'downloads the .txt.gz feature file
                    My.Computer.Network.DownloadFile("ftp://hgdownload.cse.ucsc.edu/goldenPath/hg18/database/" & currfileName & ".txt.gz", DownloadedTableDir & currfileName & ".txt.gz") 'downloads the .txt.gz feature file

                End If
                'decompresses the .gz file to a .txt file
                If File.Exists(DownloadedTableDir & currfileName & ".txt") = False Then
                    lblProgress.Text = "Decompressing " & currfileName : Application.DoEvents()
                    Using outfile As FileStream = File.Create(DownloadedTableDir & currfileName & ".txt")
                        Using infile As FileStream = File.OpenRead(DownloadedTableDir & currfileName & ".txt.gz")
                            Using Decompress As System.IO.Compression.GZipStream = New System.IO.Compression.GZipStream(infile, Compression.CompressionMode.Decompress)
                                Decompress.CopyTo(outfile)
                            End Using
                        End Using
                    End Using
                End If
            End If
            ProgressBar1.Value += 1 : Application.DoEvents()
        Next
        ProgressBar1.Value = 0
        CheckGenomeFeatureDatabaseStatus()                                                                                          'refreshes the genomic feature status

        'delets pervious versions of the file
        If File.Exists(DownloadedTableDir & "rmskRM327.txt") = True Then File.Delete(DownloadedTableDir & "rmskRM327.txt")

        'combines the seperate rmsk files by chrom into a single master .txt file
        Using swRMSK As New StreamWriter(DownloadedTableDir & "rmskRM327.txt", True)
            For Each curFileName In fileNamesrmsk327
                If curFileName <> "chr12_random_rmskRM327" And curFileName <> "chr14_random_rmskRM327" And curFileName <> "chr20_random_rmskRM327" And curFileName <> "chrM_random_rmskRM327" And curFileName <> "chrY_random_rmskRM327" Then
                    Using sr As New StreamReader(DownloadedTableDir & curFileName & ".txt")
                        While sr.EndOfStream = False
                            Dim line As String() = sr.ReadLine().Split(vbTab)
                            swRMSK.WriteLine(line(5) & vbTab & line(6) & vbTab & line(7) & vbTab & line(9) & vbTab & line(10) & vbTab & line(11) & vbTab & line(12))
                        End While
                    End Using
                End If
            Next
        End Using

        'create the rmskRM327 tables
        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
        cmd = New MySqlCommand("DROP TABLE IF EXISTS rmskRM327; CREATE  TABLE `rmskRM327` ( `chrom` VARCHAR(45) NULL , `chromStart` INT NULL , `chromEnd` INT NULL , `strand` VARCHAR(45) NULL , `repName` VARCHAR(255) NULL , `repClass` VARCHAR(255) NULL , `name` VARCHAR(255) NULL );", cn)
        cmd.ExecuteNonQuery()
        cn.Close() : cmd.Dispose()


        lblProgress.Text = "Creating new table called rmskRM327" : Application.DoEvents()
        filePath = DownloadedTableDir & "rmskRM327.txt"
        Try
            'loads the new data into the rmskrm327 table
            Dim filePathFeatureToAddUNIX As String = filePath.Replace("\", "/") 'modifies the filepath to conform to UNIX
            query = "TRUNCATE TABLE rmskRM327; LOAD DATA LOCAL INFILE " & "'" & filePathFeatureToAddUNIX & "' INTO TABLE rmskRM327;"
            cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
            cmd = New MySqlCommand(query, cn)
            cmd.ExecuteNonQuery()
            cmd.Dispose() : cn.Close() : dr.Close()
        Catch
            Dim form As New frmQuery
            form.txtQuery.Text = query
            form.ShowDialog()
        End Try
        ProgressBar1.Value = 0
        lblProgress.Text = "Done" : MessageBox.Show("Genomic features for rmsk and rmsk327 created successfully")
    End Sub


    Private Sub btnCheckIntegrity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckIntegrity.Click
        lblProgress.Text = "Getting table row count of remote database" : Application.DoEvents()
        ProgressBar1.Maximum = listGenomicFeatures.Items.Count
        ProgressBar1.Value = 0
        'gets the row count of the tables on the remote server
        Dim RemoteTableRowCount As New List(Of TableRowCount)
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            Try
                cn = New MySqlConnection(ConnectionStringHost) : cn.Open()
                cmd = New MySqlCommand("SELECT COUNT(chrom) FROM " & GF.GFeature.TableName, cn)
                dr = cmd.ExecuteReader()
                While dr.Read
                    Dim rowCount As String = dr(0)
                    Dim trc As New TableRowCount
                    trc.RowCount = rowCount
                    trc.TableName = GF.GFeature.TableName
                    trc.QueryType = GF.GFeature.QueryType
                    RemoteTableRowCount.Add(trc)
                End While
                cn.Close()
                '...if the table is unique to the local database, then it is marked as populated
            Catch
                GF.IsTablePopulated = True
            End Try
            ProgressBar1.Value += 1
        Next

        'gets the row count of the exon tables on the remote server
        ProgressBar1.Value = 0
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.GFeature.QueryType = "Gene" Or GF.GFeature.QueryType = "*Gene" Then
                cn = New MySqlConnection(ConnectionStringHost) : cn.Open()
                cmd = New MySqlCommand("SELECT COUNT(chrom) FROM " & GF.GFeature.TableName & "Exons", cn)
                dr = cmd.ExecuteReader()
                While dr.Read
                    Dim rowCount As String = dr(0)
                    Dim trc As New TableRowCount
                    trc.RowCount = rowCount
                    trc.TableName = GF.GFeature.TableName & "Exons"
                    trc.QueryType = GF.GFeature.QueryType
                    RemoteTableRowCount.Add(trc)
                End While
                cn.Close()
            End If
            ProgressBar1.Value += 1
        Next


        lblProgress.Text = "Getting table row count of local database" : Application.DoEvents()
        ProgressBar1.Maximum = listGenomicFeatures.Items.Count
        ProgressBar1.Value = 0
        'gets the row count of the tabes in the local database
        Dim LocalTableRowCount As New List(Of TableRowCount)
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.IsTableAdded = True Then
                Try
                    cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                    cmd = New MySqlCommand("SELECT COUNT(chrom) FROM " & GF.GFeature.TableName, cn)
                    dr = cmd.ExecuteReader()
                    While dr.Read()
                        Dim rowCount As String = dr(0)
                        Dim trc As New TableRowCount
                        trc.RowCount = rowCount
                        trc.TableName = GF.GFeature.TableName
                        trc.Matches = False
                        LocalTableRowCount.Add(trc)
                    End While
                    cn.Close()
                Catch
                    GF.IsTablePopulated = False
                    GF.IsTableAdded = False
                End Try
            End If
            ProgressBar1.Value += 1
        Next

        'gets the row count of the exon tables on the local server if they exist
        ProgressBar1.Value = 0
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            If GF.GFeature.QueryType = "Gene" Or GF.GFeature.QueryType = "*Gene" Then
                If GF.IsExonTableExisting = True Then
                    Try
                        cn = New MySqlConnection(ConnectionStringLocal) : cn.Open()
                        cmd = New MySqlCommand("SELECT COUNT(chrom) FROM " & GF.GFeature.TableName & "Exons", cn)
                        dr = cmd.ExecuteReader()
                        While dr.Read
                            Dim rowCount As String = dr(0)
                            Dim trc As New TableRowCount
                            trc.RowCount = rowCount
                            trc.TableName = GF.GFeature.TableName & "Exons"
                            trc.QueryType = GF.GFeature.QueryType
                            RemoteTableRowCount.Add(trc)
                        End While
                        cn.Close()
                    Catch
                        GF.IsExonTableExisting = False
                    End Try
                End If
            End If
            ProgressBar1.Value += 1
        Next

        'compares the row count of the tables on the local database against those on the remote server
        For currlocal As Integer = 0 To LocalTableRowCount.Count - 1
            For Each remoteTable In RemoteTableRowCount
                If LocalTableRowCount(currlocal).TableName = remoteTable.TableName Then
                    '...the case where the local database table closely matches that of the remote host
                    If ((LocalTableRowCount(currlocal).RowCount / remoteTable.RowCount) > 0.9 Or (LocalTableRowCount(currlocal).RowCount / remoteTable.RowCount) < 1.1) And _
                        LocalTableRowCount(currlocal).RowCount <> remoteTable.RowCount Then
                        LocalTableRowCount(currlocal).Matches = True                                                                    'the table is considered complete enought to be included in GenomeRunner
                        'finds the genomic feature that use the table that is complete enought to use but not a 100% match with the remote table
                        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
                            If GF.GFeature.TableName = LocalTableRowCount(currlocal).TableName Then
                                GF.BackColor = Color.LightGreen                                                                          'colorchanged to be a visual indicator that the table is a 100% match
                            End If
                        Next
                        '... the case where the local database matches the remote server 100%
                    ElseIf LocalTableRowCount(currlocal).RowCount = remoteTable.RowCount Then
                        LocalTableRowCount(currlocal).Matches = True
                    Else
                        LocalTableRowCount(currlocal).Matches = False
                    End If
                End If
            Next
        Next

        'set the status of the genomic feature based on whether the row count matches or not
        For Each GF As listviewGenomicFeature In listGenomicFeatures.Items
            'for genomic featuers that are genes, the exon table must be compared as well
            If GF.GFeature.QueryType = "*Gene" Or GF.GFeature.QueryType = "Gene" Then
                If GF.IsExonTableExisting = True Then
                    '...compares the gene table
                    For Each localTableData In LocalTableRowCount
                        If localTableData.TableName = GF.GFeature.TableName Then
                            GF.IsTablePopulated = localTableData.Matches                                            'the table being set to ispopulated is conditional to whether its row count matches that of the remote server.  
                        End If
                    Next
                    '...compares the exon table
                    For Each localTableData In LocalTableRowCount
                        If localTableData.TableName = GF.GFeature.TableName & "Exons" Then
                            GF.IsTablePopulated = localTableData.Matches                                            'the table being set to ispopulated is conditional to whether its row count matches that of the remote server.  
                        End If
                    Next
                End If
            Else
                For Each localTableData In LocalTableRowCount
                    If localTableData.TableName = GF.GFeature.TableName Then
                        GF.IsTablePopulated = localTableData.Matches                                            'the table being set to ispopulated is conditional to whether its row count matches that of the remote server.  
                    End If
                Next
            End If
        Next
        ProgressBar1.Value = 0
        lblProgress.Text = "Done"
    End Sub
End Class

Class ChromBase
    Public IsHit As Boolean
End Class

Class TableRowCount
    Public RowCount As Integer
    Public TableName As String
    Public Matches As Boolean
    Public QueryType As String
End Class

Public Class listviewGenomicFeature
    Inherits ListViewItem
    Public GFeature As GenomicFeature
    Dim TableAdded As Boolean
    Dim TablePopulated As Boolean
    Dim TableExonExists As Boolean

    Public Sub New(ByVal GFeatures As GenomicFeature)
        Me.GFeature = GFeatures
    End Sub


    Property IsExonTableExisting As Boolean
        Get
            Return TableExonExists
        End Get
        Set(ByVal value As Boolean)
            TableExonExists = value
            'sets the back color of the listview item based on the status of the table in the database
            If GFeature.QueryType = "*Gene" Then
                If TableAdded = False Then
                    Me.BackColor = Color.Red
                ElseIf TablePopulated = False Then
                    Me.BackColor = Color.Yellow
                ElseIf TablePopulated = True And TableExonExists = False Then
                    Me.BackColor = Color.Orange
                ElseIf TablePopulated = True And TableExonExists = True Then
                    Me.BackColor = Color.Green
                End If
            End If
        End Set
    End Property

    Property IsTableAdded As Boolean
        Get
            Return TableAdded
        End Get
        Set(ByVal value As Boolean)
            TableAdded = value
            'sets the back color of the listview item based on the status of the table in the database
            If value = False Then
                Me.BackColor = Color.Red
            ElseIf TablePopulated = False Then
                Me.BackColor = Color.Yellow
            ElseIf TablePopulated = True Then
                Me.BackColor = Color.Green
            End If
        End Set
    End Property

    Property IsTablePopulated As Boolean
        Get
            Return TablePopulated
        End Get
        Set(ByVal value As Boolean)
            TablePopulated = value
            'sets the back color of the listview item based on the status of the table in the database
            If TableAdded = False Then
                Me.BackColor = Color.Red
            ElseIf value = False Then
                Me.BackColor = Color.Yellow
            ElseIf value = True Then
                Me.BackColor = Color.Green
            End If
        End Set
    End Property
End Class

Public Class Exon
    Public name As String
    Public chrom As String
    Public strand As String
    Public exonStart As String
    Public exonEnd As String

    'this is where the results of spliting the ExonStart and Exon End are stored 
    Public listExonStart As List(Of Integer)
    Public listExonEnd As List(Of Integer)
    Public listName As List(Of String)
    Public listStrand As List(Of String)
    Public listChrom As List(Of String)
End Class