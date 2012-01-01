<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenomeRunner
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGenomeRunner))
        Me.btnAddFeature = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnMerge = New System.Windows.Forms.Button()
        Me.lnklblHost = New System.Windows.Forms.LinkLabel()
        Me.chkbxShortOnly = New System.Windows.Forms.CheckBox()
        Me.lblOrganism = New System.Windows.Forms.Label()
        Me.cmbOrganism = New System.Windows.Forms.ComboBox()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.cmbDatabase = New System.Windows.Forms.ComboBox()
        Me.btnRemoveFeaturesToRun = New System.Windows.Forms.Button()
        Me.btnRemoveFOI = New System.Windows.Forms.Button()
        Me.lblLoadFile = New System.Windows.Forms.Label()
        Me.listFeatureFiles = New System.Windows.Forms.ListView()
        Me.cmbTier = New System.Windows.Forms.ComboBox()
        Me.btnPValue = New System.Windows.Forms.Button()
        Me.listFeaturesToRun = New System.Windows.Forms.ListView()
        Me.btnRemoveAllFeaturesToRun = New System.Windows.Forms.Button()
        Me.btnLoadPOIs = New System.Windows.Forms.Button()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.ListFeaturesAvailable = New System.Windows.Forms.ListView()
        Me.btnAddAllFeatures = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.txtPvalueThreshold = New System.Windows.Forms.NumericUpDown()
        Me.GroupBoxPearsons = New System.Windows.Forms.GroupBox()
        Me.txtPearsonAudjustmentConstant = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtNumMCtoRun = New System.Windows.Forms.NumericUpDown()
        Me.GroupBoxPercentAudjustment = New System.Windows.Forms.GroupBox()
        Me.rbLinear = New System.Windows.Forms.RadioButton()
        Me.rbSquared = New System.Windows.Forms.RadioButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbMatrixWeighting = New System.Windows.Forms.ComboBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.rbTradMC = New System.Windows.Forms.RadioButton()
        Me.rbChiSquareTest = New System.Windows.Forms.RadioButton()
        Me.rbBinomialDistrobution = New System.Windows.Forms.RadioButton()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.rbUseMonteCarlo = New System.Windows.Forms.RadioButton()
        Me.rbUseAnalytical = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnEnrichmentForAllNames = New System.Windows.Forms.Button()
        Me.txtproximity = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbStrandsToAnalyze = New System.Windows.Forms.ComboBox()
        Me.cmbFilterLevels = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnCustomizeFilters = New System.Windows.Forms.Button()
        Me.btnLoadCustomGenomicFeatures = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenBackgroundFileIntervalsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenBackgroundFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCoordinatesToSNPs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSNPsToCoordinates = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateListOfRandomCoordinates = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateListOfRandomSNPs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetDatabaseConnectionSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateLocalGenomeRunnerTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMergeLogFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuProgramInterface = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenomeFeatures = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFD = New System.Windows.Forms.SaveFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenFD = New System.Windows.Forms.OpenFileDialog()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblBackground = New System.Windows.Forms.Label()
        Me.txtJobName = New System.Windows.Forms.TextBox()
        Me.BackgroundWorkerEnrichmentAnalysis = New System.ComponentModel.BackgroundWorker()
        Me.BackgroundWorkerAnnotationAnalysis = New System.ComponentModel.BackgroundWorker()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.txtPvalueThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxPearsons.SuspendLayout()
        CType(Me.txtPearsonAudjustmentConstant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNumMCtoRun, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxPercentAudjustment.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.txtproximity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAddFeature
        '
        Me.btnAddFeature.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAddFeature.Location = New System.Drawing.Point(389, 166)
        Me.btnAddFeature.Name = "btnAddFeature"
        Me.btnAddFeature.Size = New System.Drawing.Size(60, 23)
        Me.btnAddFeature.TabIndex = 4
        Me.btnAddFeature.Text = "->"
        Me.ToolTip2.SetToolTip(Me.btnAddFeature, "Add the selected features to the list of features to run")
        Me.btnAddFeature.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(459, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(164, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Genomic features that will be run:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 37)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(272, 68)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 34
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(206, 82)
        Me.Label2.MaximumSize = New System.Drawing.Size(180, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(139, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Genomic features Available:"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.btnMerge)
        Me.GroupBox1.Controls.Add(Me.lnklblHost)
        Me.GroupBox1.Controls.Add(Me.chkbxShortOnly)
        Me.GroupBox1.Controls.Add(Me.lblOrganism)
        Me.GroupBox1.Controls.Add(Me.cmbOrganism)
        Me.GroupBox1.Controls.Add(Me.lblDatabase)
        Me.GroupBox1.Controls.Add(Me.cmbDatabase)
        Me.GroupBox1.Controls.Add(Me.btnRemoveFeaturesToRun)
        Me.GroupBox1.Controls.Add(Me.btnRemoveFOI)
        Me.GroupBox1.Controls.Add(Me.lblLoadFile)
        Me.GroupBox1.Controls.Add(Me.listFeatureFiles)
        Me.GroupBox1.Controls.Add(Me.cmbTier)
        Me.GroupBox1.Controls.Add(Me.btnPValue)
        Me.GroupBox1.Controls.Add(Me.listFeaturesToRun)
        Me.GroupBox1.Controls.Add(Me.btnRemoveAllFeaturesToRun)
        Me.GroupBox1.Controls.Add(Me.btnLoadPOIs)
        Me.GroupBox1.Controls.Add(Me.btnRun)
        Me.GroupBox1.Controls.Add(Me.ListFeaturesAvailable)
        Me.GroupBox1.Controls.Add(Me.btnAddAllFeatures)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnAddFeature)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(9, 114)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(637, 469)
        Me.GroupBox1.TabIndex = 36
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Add files with Features Of Interest and select Genomic Features to analyze"
        '
        'btnMerge
        '
        Me.btnMerge.Location = New System.Drawing.Point(6, 439)
        Me.btnMerge.Name = "btnMerge"
        Me.btnMerge.Size = New System.Drawing.Size(161, 23)
        Me.btnMerge.TabIndex = 79
        Me.btnMerge.Text = "Merge"
        Me.btnMerge.UseVisualStyleBackColor = True
        '
        'lnklblHost
        '
        Me.lnklblHost.AutoSize = True
        Me.lnklblHost.Location = New System.Drawing.Point(205, 22)
        Me.lnklblHost.Name = "lnklblHost"
        Me.lnklblHost.Size = New System.Drawing.Size(32, 15)
        Me.lnklblHost.TabIndex = 78
        Me.lnklblHost.TabStop = True
        Me.lnklblHost.Text = "Host"
        '
        'chkbxShortOnly
        '
        Me.chkbxShortOnly.AutoSize = True
        Me.chkbxShortOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxShortOnly.Location = New System.Drawing.Point(396, 417)
        Me.chkbxShortOnly.Name = "chkbxShortOnly"
        Me.chkbxShortOnly.Size = New System.Drawing.Size(60, 14)
        Me.chkbxShortOnly.TabIndex = 77
        Me.chkbxShortOnly.Text = "Short Only"
        Me.chkbxShortOnly.UseVisualStyleBackColor = True
        '
        'lblOrganism
        '
        Me.lblOrganism.AutoSize = True
        Me.lblOrganism.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrganism.Location = New System.Drawing.Point(205, 40)
        Me.lblOrganism.Name = "lblOrganism"
        Me.lblOrganism.Size = New System.Drawing.Size(54, 13)
        Me.lblOrganism.TabIndex = 76
        Me.lblOrganism.Text = "Organism:"
        '
        'cmbOrganism
        '
        Me.cmbOrganism.FormattingEnabled = True
        Me.cmbOrganism.Items.AddRange(New Object() {"Human", "Mouse"})
        Me.cmbOrganism.Location = New System.Drawing.Point(209, 56)
        Me.cmbOrganism.Name = "cmbOrganism"
        Me.cmbOrganism.Size = New System.Drawing.Size(98, 23)
        Me.cmbOrganism.TabIndex = 75
        Me.cmbOrganism.Text = "Human"
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatabase.Location = New System.Drawing.Point(310, 40)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(56, 13)
        Me.lblDatabase.TabIndex = 74
        Me.lblDatabase.Text = "Database:"
        '
        'cmbDatabase
        '
        Me.cmbDatabase.FormattingEnabled = True
        Me.cmbDatabase.Location = New System.Drawing.Point(313, 56)
        Me.cmbDatabase.Name = "cmbDatabase"
        Me.cmbDatabase.Size = New System.Drawing.Size(98, 23)
        Me.cmbDatabase.TabIndex = 73
        '
        'btnRemoveFeaturesToRun
        '
        Me.btnRemoveFeaturesToRun.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRemoveFeaturesToRun.Location = New System.Drawing.Point(389, 252)
        Me.btnRemoveFeaturesToRun.Name = "btnRemoveFeaturesToRun"
        Me.btnRemoveFeaturesToRun.Size = New System.Drawing.Size(60, 23)
        Me.btnRemoveFeaturesToRun.TabIndex = 31
        Me.btnRemoveFeaturesToRun.Text = "<-"
        Me.ToolTip2.SetToolTip(Me.btnRemoveFeaturesToRun, "Remove the selected features from the list of features to run")
        Me.btnRemoveFeaturesToRun.UseVisualStyleBackColor = False
        '
        'btnRemoveFOI
        '
        Me.btnRemoveFOI.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRemoveFOI.Location = New System.Drawing.Point(6, 410)
        Me.btnRemoveFOI.Name = "btnRemoveFOI"
        Me.btnRemoveFOI.Size = New System.Drawing.Size(161, 23)
        Me.btnRemoveFOI.TabIndex = 32
        Me.btnRemoveFOI.Text = "Remove Selected"
        Me.ToolTip2.SetToolTip(Me.btnRemoveFOI, "Removes the selected Feature of interest files")
        Me.btnRemoveFOI.UseVisualStyleBackColor = False
        '
        'lblLoadFile
        '
        Me.lblLoadFile.AutoSize = True
        Me.lblLoadFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblLoadFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoadFile.Location = New System.Drawing.Point(6, 24)
        Me.lblLoadFile.MaximumSize = New System.Drawing.Size(180, 0)
        Me.lblLoadFile.Name = "lblLoadFile"
        Me.lblLoadFile.Size = New System.Drawing.Size(130, 13)
        Me.lblLoadFile.TabIndex = 72
        Me.lblLoadFile.Text = "Load Features Of Interest:"
        '
        'listFeatureFiles
        '
        Me.listFeatureFiles.AllowDrop = True
        Me.listFeatureFiles.Location = New System.Drawing.Point(6, 48)
        Me.listFeatureFiles.Name = "listFeatureFiles"
        Me.listFeatureFiles.Size = New System.Drawing.Size(161, 327)
        Me.listFeatureFiles.TabIndex = 44
        Me.listFeatureFiles.UseCompatibleStateImageBehavior = False
        Me.listFeatureFiles.View = System.Windows.Forms.View.List
        '
        'cmbTier
        '
        Me.cmbTier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTier.FormattingEnabled = True
        Me.cmbTier.Items.AddRange(New Object() {"Tier1", "Tier2", "Tier3", "TFBS100"})
        Me.cmbTier.Location = New System.Drawing.Point(208, 102)
        Me.cmbTier.Name = "cmbTier"
        Me.cmbTier.Size = New System.Drawing.Size(161, 23)
        Me.cmbTier.TabIndex = 3
        Me.ToolTip2.SetToolTip(Me.cmbTier, "Tiered system of available genomic features")
        '
        'btnPValue
        '
        Me.btnPValue.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnPValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPValue.Location = New System.Drawing.Point(462, 439)
        Me.btnPValue.Name = "btnPValue"
        Me.btnPValue.Size = New System.Drawing.Size(161, 23)
        Me.btnPValue.TabIndex = 21
        Me.btnPValue.Text = "Enrichment Analysis"
        Me.btnPValue.UseVisualStyleBackColor = False
        '
        'listFeaturesToRun
        '
        Me.listFeaturesToRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listFeaturesToRun.HideSelection = False
        Me.listFeaturesToRun.Location = New System.Drawing.Point(462, 44)
        Me.listFeaturesToRun.Name = "listFeaturesToRun"
        Me.listFeaturesToRun.ShowGroups = False
        Me.listFeaturesToRun.Size = New System.Drawing.Size(161, 360)
        Me.listFeaturesToRun.TabIndex = 59
        Me.listFeaturesToRun.UseCompatibleStateImageBehavior = False
        Me.listFeaturesToRun.View = System.Windows.Forms.View.SmallIcon
        '
        'btnRemoveAllFeaturesToRun
        '
        Me.btnRemoveAllFeaturesToRun.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRemoveAllFeaturesToRun.Location = New System.Drawing.Point(389, 224)
        Me.btnRemoveAllFeaturesToRun.Name = "btnRemoveAllFeaturesToRun"
        Me.btnRemoveAllFeaturesToRun.Size = New System.Drawing.Size(60, 23)
        Me.btnRemoveAllFeaturesToRun.TabIndex = 30
        Me.btnRemoveAllFeaturesToRun.Text = "<<-"
        Me.ToolTip2.SetToolTip(Me.btnRemoveAllFeaturesToRun, "Remove all of the features from the list of features to run")
        Me.btnRemoveAllFeaturesToRun.UseVisualStyleBackColor = False
        '
        'btnLoadPOIs
        '
        Me.btnLoadPOIs.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLoadPOIs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadPOIs.Location = New System.Drawing.Point(6, 381)
        Me.btnLoadPOIs.Name = "btnLoadPOIs"
        Me.btnLoadPOIs.Size = New System.Drawing.Size(161, 23)
        Me.btnLoadPOIs.TabIndex = 2
        Me.btnLoadPOIs.Text = "Load Input Files"
        Me.btnLoadPOIs.UseVisualStyleBackColor = False
        '
        'btnRun
        '
        Me.btnRun.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRun.Location = New System.Drawing.Point(462, 410)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(161, 23)
        Me.btnRun.TabIndex = 20
        Me.btnRun.Text = "Annotation Analysis"
        Me.btnRun.UseVisualStyleBackColor = False
        '
        'ListFeaturesAvailable
        '
        Me.ListFeaturesAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListFeaturesAvailable.Location = New System.Drawing.Point(209, 131)
        Me.ListFeaturesAvailable.Name = "ListFeaturesAvailable"
        Me.ListFeaturesAvailable.ShowItemToolTips = True
        Me.ListFeaturesAvailable.Size = New System.Drawing.Size(161, 331)
        Me.ListFeaturesAvailable.TabIndex = 57
        Me.ListFeaturesAvailable.UseCompatibleStateImageBehavior = False
        Me.ListFeaturesAvailable.View = System.Windows.Forms.View.SmallIcon
        '
        'btnAddAllFeatures
        '
        Me.btnAddAllFeatures.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAddAllFeatures.Location = New System.Drawing.Point(389, 195)
        Me.btnAddAllFeatures.Name = "btnAddAllFeatures"
        Me.btnAddAllFeatures.Size = New System.Drawing.Size(60, 23)
        Me.btnAddAllFeatures.TabIndex = 5
        Me.btnAddAllFeatures.Text = "->>"
        Me.ToolTip2.SetToolTip(Me.btnAddAllFeatures, "Add all of the features to the list of features to run")
        Me.btnAddAllFeatures.UseVisualStyleBackColor = False
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox4.Controls.Add(Me.GroupBox7)
        Me.GroupBox4.Controls.Add(Me.GroupBox6)
        Me.GroupBox4.Location = New System.Drawing.Point(652, 35)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(270, 548)
        Me.GroupBox4.TabIndex = 40
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Settings"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.txtPvalueThreshold)
        Me.GroupBox7.Controls.Add(Me.GroupBoxPearsons)
        Me.GroupBox7.Controls.Add(Me.txtNumMCtoRun)
        Me.GroupBox7.Controls.Add(Me.GroupBoxPercentAudjustment)
        Me.GroupBox7.Controls.Add(Me.Label7)
        Me.GroupBox7.Controls.Add(Me.cmbMatrixWeighting)
        Me.GroupBox7.Controls.Add(Me.GroupBox5)
        Me.GroupBox7.Controls.Add(Me.Label15)
        Me.GroupBox7.Controls.Add(Me.GroupBox3)
        Me.GroupBox7.Controls.Add(Me.Label5)
        Me.GroupBox7.Location = New System.Drawing.Point(5, 19)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(259, 307)
        Me.GroupBox7.TabIndex = 83
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Enrichment Analysis Settings"
        '
        'txtPvalueThreshold
        '
        Me.txtPvalueThreshold.DecimalPlaces = 3
        Me.txtPvalueThreshold.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.txtPvalueThreshold.Location = New System.Drawing.Point(7, 164)
        Me.txtPvalueThreshold.Name = "txtPvalueThreshold"
        Me.txtPvalueThreshold.Size = New System.Drawing.Size(75, 20)
        Me.txtPvalueThreshold.TabIndex = 11
        Me.txtPvalueThreshold.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'GroupBoxPearsons
        '
        Me.GroupBoxPearsons.Controls.Add(Me.txtPearsonAudjustmentConstant)
        Me.GroupBoxPearsons.Controls.Add(Me.Label8)
        Me.GroupBoxPearsons.Location = New System.Drawing.Point(5, 255)
        Me.GroupBoxPearsons.Name = "GroupBoxPearsons"
        Me.GroupBoxPearsons.Size = New System.Drawing.Size(242, 39)
        Me.GroupBoxPearsons.TabIndex = 91
        Me.GroupBoxPearsons.TabStop = False
        '
        'txtPearsonAudjustmentConstant
        '
        Me.txtPearsonAudjustmentConstant.Location = New System.Drawing.Point(173, 12)
        Me.txtPearsonAudjustmentConstant.Maximum = New Decimal(New Integer() {200000000, 0, 0, 0})
        Me.txtPearsonAudjustmentConstant.Name = "txtPearsonAudjustmentConstant"
        Me.txtPearsonAudjustmentConstant.Size = New System.Drawing.Size(62, 20)
        Me.txtPearsonAudjustmentConstant.TabIndex = 13
        Me.txtPearsonAudjustmentConstant.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(161, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Multiply Pearson's Coefficient by:"
        '
        'txtNumMCtoRun
        '
        Me.txtNumMCtoRun.Enabled = False
        Me.txtNumMCtoRun.Location = New System.Drawing.Point(5, 68)
        Me.txtNumMCtoRun.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.txtNumMCtoRun.Name = "txtNumMCtoRun"
        Me.txtNumMCtoRun.Size = New System.Drawing.Size(64, 20)
        Me.txtNumMCtoRun.TabIndex = 8
        Me.txtNumMCtoRun.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'GroupBoxPercentAudjustment
        '
        Me.GroupBoxPercentAudjustment.Controls.Add(Me.rbLinear)
        Me.GroupBoxPercentAudjustment.Controls.Add(Me.rbSquared)
        Me.GroupBoxPercentAudjustment.Location = New System.Drawing.Point(7, 232)
        Me.GroupBoxPercentAudjustment.Name = "GroupBoxPercentAudjustment"
        Me.GroupBoxPercentAudjustment.Size = New System.Drawing.Size(242, 42)
        Me.GroupBoxPercentAudjustment.TabIndex = 90
        Me.GroupBoxPercentAudjustment.TabStop = False
        '
        'rbLinear
        '
        Me.rbLinear.AutoSize = True
        Me.rbLinear.Checked = True
        Me.rbLinear.Location = New System.Drawing.Point(6, 19)
        Me.rbLinear.Name = "rbLinear"
        Me.rbLinear.Size = New System.Drawing.Size(54, 17)
        Me.rbLinear.TabIndex = 85
        Me.rbLinear.TabStop = True
        Me.rbLinear.Text = "Linear"
        Me.ToolTip2.SetToolTip(Me.rbLinear, "Use the Chi-Square test to calculate the p-values")
        Me.rbLinear.UseVisualStyleBackColor = True
        '
        'rbSquared
        '
        Me.rbSquared.AutoSize = True
        Me.rbSquared.Location = New System.Drawing.Point(113, 19)
        Me.rbSquared.Name = "rbSquared"
        Me.rbSquared.Size = New System.Drawing.Size(65, 17)
        Me.rbSquared.TabIndex = 86
        Me.rbSquared.Text = "Squared"
        Me.ToolTip2.SetToolTip(Me.rbSquared, "Use binomial distrobution to calculate the P-values")
        Me.rbSquared.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 192)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 86
        Me.Label7.Text = "Adjustments:"
        '
        'cmbMatrixWeighting
        '
        Me.cmbMatrixWeighting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMatrixWeighting.FormattingEnabled = True
        Me.cmbMatrixWeighting.Items.AddRange(New Object() {"None", "Percent", "Pearson's contingency coefficient", "All"})
        Me.cmbMatrixWeighting.Location = New System.Drawing.Point(6, 208)
        Me.cmbMatrixWeighting.Name = "cmbMatrixWeighting"
        Me.cmbMatrixWeighting.Size = New System.Drawing.Size(242, 21)
        Me.cmbMatrixWeighting.TabIndex = 12
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.rbTradMC)
        Me.GroupBox5.Controls.Add(Me.rbChiSquareTest)
        Me.GroupBox5.Controls.Add(Me.rbBinomialDistrobution)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 91)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(242, 67)
        Me.GroupBox5.TabIndex = 54
        Me.GroupBox5.TabStop = False
        '
        'rbTradMC
        '
        Me.rbTradMC.AutoSize = True
        Me.rbTradMC.Location = New System.Drawing.Point(54, 16)
        Me.rbTradMC.Name = "rbTradMC"
        Me.rbTradMC.Size = New System.Drawing.Size(134, 17)
        Me.rbTradMC.TabIndex = 11
        Me.rbTradMC.TabStop = True
        Me.rbTradMC.Text = "Traditional Monte-Carlo"
        Me.rbTradMC.UseVisualStyleBackColor = True
        '
        'rbChiSquareTest
        '
        Me.rbChiSquareTest.AutoSize = True
        Me.rbChiSquareTest.Checked = True
        Me.rbChiSquareTest.Location = New System.Drawing.Point(6, 40)
        Me.rbChiSquareTest.Name = "rbChiSquareTest"
        Me.rbChiSquareTest.Size = New System.Drawing.Size(101, 17)
        Me.rbChiSquareTest.TabIndex = 9
        Me.rbChiSquareTest.TabStop = True
        Me.rbChiSquareTest.Text = "Chi-Square Test"
        Me.ToolTip2.SetToolTip(Me.rbChiSquareTest, "Use the Chi-Square test to calculate the p-values")
        Me.rbChiSquareTest.UseVisualStyleBackColor = True
        '
        'rbBinomialDistrobution
        '
        Me.rbBinomialDistrobution.AutoSize = True
        Me.rbBinomialDistrobution.Location = New System.Drawing.Point(113, 40)
        Me.rbBinomialDistrobution.Name = "rbBinomialDistrobution"
        Me.rbBinomialDistrobution.Size = New System.Drawing.Size(119, 17)
        Me.rbBinomialDistrobution.TabIndex = 10
        Me.rbBinomialDistrobution.Text = "Binomial Distribution"
        Me.ToolTip2.SetToolTip(Me.rbBinomialDistrobution, "Use binomial distrobution to calculate the P-values")
        Me.rbBinomialDistrobution.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Location = New System.Drawing.Point(74, 68)
        Me.Label15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(139, 13)
        Me.Label15.TabIndex = 62
        Me.Label15.Text = "Number of Monte-Carlo runs"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbUseMonteCarlo)
        Me.GroupBox3.Controls.Add(Me.rbUseAnalytical)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 19)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(242, 42)
        Me.GroupBox3.TabIndex = 88
        Me.GroupBox3.TabStop = False
        '
        'rbUseMonteCarlo
        '
        Me.rbUseMonteCarlo.AutoSize = True
        Me.rbUseMonteCarlo.Checked = True
        Me.rbUseMonteCarlo.Location = New System.Drawing.Point(12, 15)
        Me.rbUseMonteCarlo.Name = "rbUseMonteCarlo"
        Me.rbUseMonteCarlo.Size = New System.Drawing.Size(104, 17)
        Me.rbUseMonteCarlo.TabIndex = 6
        Me.rbUseMonteCarlo.TabStop = True
        Me.rbUseMonteCarlo.Text = "Use Monte Carlo"
        Me.ToolTip2.SetToolTip(Me.rbUseMonteCarlo, "Use Monte Carlo to calculate the number of random overlaps")
        Me.rbUseMonteCarlo.UseVisualStyleBackColor = True
        '
        'rbUseAnalytical
        '
        Me.rbUseAnalytical.AutoSize = True
        Me.rbUseAnalytical.Location = New System.Drawing.Point(126, 15)
        Me.rbUseAnalytical.Name = "rbUseAnalytical"
        Me.rbUseAnalytical.Size = New System.Drawing.Size(92, 17)
        Me.rbUseAnalytical.TabIndex = 7
        Me.rbUseAnalytical.Text = "Use Analytical"
        Me.ToolTip2.SetToolTip(Me.rbUseAnalytical, "Use the analytical method to calculate the random number of overlaps")
        Me.rbUseAnalytical.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(88, 166)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 65
        Me.Label5.Text = "P-value cutoff"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label9)
        Me.GroupBox6.Controls.Add(Me.btnEnrichmentForAllNames)
        Me.GroupBox6.Controls.Add(Me.txtproximity)
        Me.GroupBox6.Controls.Add(Me.Label4)
        Me.GroupBox6.Controls.Add(Me.cmbStrandsToAnalyze)
        Me.GroupBox6.Controls.Add(Me.cmbFilterLevels)
        Me.GroupBox6.Controls.Add(Me.Label3)
        Me.GroupBox6.Controls.Add(Me.btnCustomizeFilters)
        Me.GroupBox6.Controls.Add(Me.btnLoadCustomGenomicFeatures)
        Me.GroupBox6.Location = New System.Drawing.Point(10, 348)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(259, 194)
        Me.GroupBox6.TabIndex = 88
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Advanced Settings"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(128, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 13)
        Me.Label9.TabIndex = 87
        Me.Label9.Text = "Proximity (bp): "
        '
        'btnEnrichmentForAllNames
        '
        Me.btnEnrichmentForAllNames.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnEnrichmentForAllNames.Location = New System.Drawing.Point(6, 135)
        Me.btnEnrichmentForAllNames.Name = "btnEnrichmentForAllNames"
        Me.btnEnrichmentForAllNames.Size = New System.Drawing.Size(247, 23)
        Me.btnEnrichmentForAllNames.TabIndex = 18
        Me.btnEnrichmentForAllNames.Text = "Run Enrichment for all names"
        Me.ToolTip2.SetToolTip(Me.btnEnrichmentForAllNames, "Does an enrichment for all names available")
        Me.btnEnrichmentForAllNames.UseVisualStyleBackColor = False
        '
        'txtproximity
        '
        Me.txtproximity.Location = New System.Drawing.Point(131, 109)
        Me.txtproximity.Maximum = New Decimal(New Integer() {200000000, 0, 0, 0})
        Me.txtproximity.Name = "txtproximity"
        Me.txtproximity.Size = New System.Drawing.Size(119, 20)
        Me.txtproximity.TabIndex = 17
        Me.ToolTip2.SetToolTip(Me.txtproximity, "The number of base pairs that a feature of interest can be away from a genomic fe" & _
                "ature and still be considered a hit.  The overlap type will be returned as a non" & _
                "-overlap")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(5, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 69
        Me.Label4.Text = "Set thresholds to:"
        '
        'cmbStrandsToAnalyze
        '
        Me.cmbStrandsToAnalyze.FormattingEnabled = True
        Me.cmbStrandsToAnalyze.Items.AddRange(New Object() {"Both", "+", "-"})
        Me.cmbStrandsToAnalyze.Location = New System.Drawing.Point(4, 109)
        Me.cmbStrandsToAnalyze.Name = "cmbStrandsToAnalyze"
        Me.cmbStrandsToAnalyze.Size = New System.Drawing.Size(119, 21)
        Me.cmbStrandsToAnalyze.TabIndex = 16
        '
        'cmbFilterLevels
        '
        Me.cmbFilterLevels.FormattingEnabled = True
        Me.cmbFilterLevels.Items.AddRange(New Object() {"Minimum", "Mean"})
        Me.cmbFilterLevels.Location = New System.Drawing.Point(5, 35)
        Me.cmbFilterLevels.Name = "cmbFilterLevels"
        Me.cmbFilterLevels.Size = New System.Drawing.Size(242, 21)
        Me.cmbFilterLevels.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(103, 13)
        Me.Label3.TabIndex = 84
        Me.Label3.Text = "Strand(s) to analyze:"
        '
        'btnCustomizeFilters
        '
        Me.btnCustomizeFilters.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnCustomizeFilters.Location = New System.Drawing.Point(5, 63)
        Me.btnCustomizeFilters.Name = "btnCustomizeFilters"
        Me.btnCustomizeFilters.Size = New System.Drawing.Size(247, 23)
        Me.btnCustomizeFilters.TabIndex = 15
        Me.btnCustomizeFilters.Text = "Cusomize Thresholds"
        Me.ToolTip2.SetToolTip(Me.btnCustomizeFilters, "Customize available thresholds for selected genomic  features")
        Me.btnCustomizeFilters.UseVisualStyleBackColor = False
        '
        'btnLoadCustomGenomicFeatures
        '
        Me.btnLoadCustomGenomicFeatures.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLoadCustomGenomicFeatures.Location = New System.Drawing.Point(6, 164)
        Me.btnLoadCustomGenomicFeatures.Name = "btnLoadCustomGenomicFeatures"
        Me.btnLoadCustomGenomicFeatures.Size = New System.Drawing.Size(248, 23)
        Me.btnLoadCustomGenomicFeatures.TabIndex = 19
        Me.btnLoadCustomGenomicFeatures.Text = "Load Custom Genomic Feature Tracks"
        Me.ToolTip2.SetToolTip(Me.btnLoadCustomGenomicFeatures, "Load a custom file that contains chrom,chromStart,chromEnd, in a tab deliminated " & _
                "form")
        Me.btnLoadCustomGenomicFeatures.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(11, 601)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(911, 19)
        Me.ProgressBar1.TabIndex = 41
        '
        'lblProgress
        '
        Me.lblProgress.Location = New System.Drawing.Point(9, 586)
        Me.lblProgress.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(842, 13)
        Me.lblProgress.TabIndex = 42
        Me.lblProgress.Text = "Progress bar"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuTools, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(929, 24)
        Me.MenuStrip1.TabIndex = 43
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileOpen, Me.UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem, Me.OpenBackgroundFileIntervalsToolStripMenuItem, Me.mnuOpenBackgroundFileToolStripMenuItem, Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileOpen
        '
        Me.mnuFileOpen.Name = "mnuFileOpen"
        Me.mnuFileOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuFileOpen.Size = New System.Drawing.Size(283, 22)
        Me.mnuFileOpen.Text = "Open input file"
        '
        'UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem
        '
        Me.UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem.Name = "UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem"
        Me.UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem.Size = New System.Drawing.Size(283, 22)
        Me.UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem.Text = "Use NCBI36/hg18 genomic background"
        '
        'OpenBackgroundFileIntervalsToolStripMenuItem
        '
        Me.OpenBackgroundFileIntervalsToolStripMenuItem.Name = "OpenBackgroundFileIntervalsToolStripMenuItem"
        Me.OpenBackgroundFileIntervalsToolStripMenuItem.Size = New System.Drawing.Size(283, 22)
        Me.OpenBackgroundFileIntervalsToolStripMenuItem.Text = "Open background file - Intervals"
        '
        'mnuOpenBackgroundFileToolStripMenuItem
        '
        Me.mnuOpenBackgroundFileToolStripMenuItem.Name = "mnuOpenBackgroundFileToolStripMenuItem"
        Me.mnuOpenBackgroundFileToolStripMenuItem.Size = New System.Drawing.Size(283, 22)
        Me.mnuOpenBackgroundFileToolStripMenuItem.Text = "Open background file - Spot"
        '
        'mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem
        '
        Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem.Name = "mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem"
        Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem.Size = New System.Drawing.Size(283, 22)
        Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem.Text = "Load snp130 DB as a background"
        Me.mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(283, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCoordinatesToSNPs, Me.mnuSNPsToCoordinates, Me.mnuGenerateListOfRandomCoordinates, Me.mnuGenerateListOfRandomSNPs, Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem, Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem, Me.SetDatabaseConnectionSettingsToolStripMenuItem, Me.CreateLocalGenomeRunnerTableToolStripMenuItem, Me.mnuMergeLogFiles})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(48, 20)
        Me.mnuTools.Text = "&Tools"
        '
        'mnuCoordinatesToSNPs
        '
        Me.mnuCoordinatesToSNPs.Name = "mnuCoordinatesToSNPs"
        Me.mnuCoordinatesToSNPs.Size = New System.Drawing.Size(314, 22)
        Me.mnuCoordinatesToSNPs.Text = "Convert coordinates to SNPs"
        Me.mnuCoordinatesToSNPs.Visible = False
        '
        'mnuSNPsToCoordinates
        '
        Me.mnuSNPsToCoordinates.Name = "mnuSNPsToCoordinates"
        Me.mnuSNPsToCoordinates.Size = New System.Drawing.Size(314, 22)
        Me.mnuSNPsToCoordinates.Text = "Convert SNP names to coordinates"
        Me.mnuSNPsToCoordinates.Visible = False
        '
        'mnuGenerateListOfRandomCoordinates
        '
        Me.mnuGenerateListOfRandomCoordinates.Name = "mnuGenerateListOfRandomCoordinates"
        Me.mnuGenerateListOfRandomCoordinates.Size = New System.Drawing.Size(314, 22)
        Me.mnuGenerateListOfRandomCoordinates.Text = "Generate list of random genomic regions"
        '
        'mnuGenerateListOfRandomSNPs
        '
        Me.mnuGenerateListOfRandomSNPs.Name = "mnuGenerateListOfRandomSNPs"
        Me.mnuGenerateListOfRandomSNPs.Size = New System.Drawing.Size(314, 22)
        Me.mnuGenerateListOfRandomSNPs.Text = "Generate list of random SNPs from dbSNP130"
        '
        'ConvertGenBankIDsToGeneNamesToolStripMenuItem
        '
        Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem.Name = "ConvertGenBankIDsToGeneNamesToolStripMenuItem"
        Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem.Text = "Convert GenBank IDs to Gene Names"
        Me.ConvertGenBankIDsToGeneNamesToolStripMenuItem.Visible = False
        '
        'ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem
        '
        Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem.Name = "ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem"
        Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem.Text = "Convert UCSC Gene IDs to Gene Names"
        Me.ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem.Visible = False
        '
        'SetDatabaseConnectionSettingsToolStripMenuItem
        '
        Me.SetDatabaseConnectionSettingsToolStripMenuItem.Name = "SetDatabaseConnectionSettingsToolStripMenuItem"
        Me.SetDatabaseConnectionSettingsToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.SetDatabaseConnectionSettingsToolStripMenuItem.Text = "Set Database Connection Settings"
        '
        'CreateLocalGenomeRunnerTableToolStripMenuItem
        '
        Me.CreateLocalGenomeRunnerTableToolStripMenuItem.Name = "CreateLocalGenomeRunnerTableToolStripMenuItem"
        Me.CreateLocalGenomeRunnerTableToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.CreateLocalGenomeRunnerTableToolStripMenuItem.Text = "Create local GenomeRunner database"
        '
        'mnuMergeLogFiles
        '
        Me.mnuMergeLogFiles.Name = "mnuMergeLogFiles"
        Me.mnuMergeLogFiles.Size = New System.Drawing.Size(314, 22)
        Me.mnuMergeLogFiles.Text = "Merge log files"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProgramInterface, Me.mnuGenomeFeatures, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'mnuProgramInterface
        '
        Me.mnuProgramInterface.Name = "mnuProgramInterface"
        Me.mnuProgramInterface.Size = New System.Drawing.Size(169, 22)
        Me.mnuProgramInterface.Text = "Program interface"
        '
        'mnuGenomeFeatures
        '
        Me.mnuGenomeFeatures.Name = "mnuGenomeFeatures"
        Me.mnuGenomeFeatures.Size = New System.Drawing.Size(169, 22)
        Me.mnuGenomeFeatures.Text = "Genome Features"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'OpenFD
        '
        Me.OpenFD.FileName = "OpenFileDialog1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(4, 47)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(159, 15)
        Me.Label6.TabIndex = 51
        Me.Label6.Text = "Enter a job name (optional):"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox2.Controls.Add(Me.lblBackground)
        Me.GroupBox2.Controls.Add(Me.txtJobName)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(272, 35)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(374, 74)
        Me.GroupBox2.TabIndex = 37
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Background and job name"
        '
        'lblBackground
        '
        Me.lblBackground.AutoSize = True
        Me.lblBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBackground.Location = New System.Drawing.Point(9, 17)
        Me.lblBackground.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBackground.Name = "lblBackground"
        Me.lblBackground.Size = New System.Drawing.Size(356, 17)
        Me.lblBackground.TabIndex = 75
        Me.lblBackground.Text = "Using NCBI36/hg18 genome assembly as genomic background"
        Me.lblBackground.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtJobName
        '
        Me.txtJobName.Location = New System.Drawing.Point(167, 47)
        Me.txtJobName.Name = "txtJobName"
        Me.txtJobName.Size = New System.Drawing.Size(198, 21)
        Me.txtJobName.TabIndex = 1
        '
        'BackgroundWorkerEnrichmentAnalysis
        '
        '
        'BackgroundWorkerAnnotationAnalysis
        '
        '
        'frmGenomeRunner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(929, 629)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmGenomeRunner"
        Me.Text = "Genome Runner"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        CType(Me.txtPvalueThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxPearsons.ResumeLayout(False)
        Me.GroupBoxPearsons.PerformLayout()
        CType(Me.txtPearsonAudjustmentConstant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNumMCtoRun, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxPercentAudjustment.ResumeLayout(False)
        Me.GroupBoxPercentAudjustment.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.txtproximity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAddFeature As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnLoadPOIs As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenBackgroundFileIntervalsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenBackgroundFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLoadSnp130DBAsSpotBackgrountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCoordinatesToSNPs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSNPsToCoordinates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateListOfRandomCoordinates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateListOfRandomSNPs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProgramInterface As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenomeFeatures As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFD As System.Windows.Forms.SaveFileDialog
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolTip2 As System.Windows.Forms.ToolTip
    Friend WithEvents OpenFD As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnPValue As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnAddAllFeatures As System.Windows.Forms.Button
    Friend WithEvents cmbFilterLevels As System.Windows.Forms.ComboBox
    Friend WithEvents ConvertGenBankIDsToGeneNamesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConvertUCSCGeneIDsToGeneNamesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListFeaturesAvailable As System.Windows.Forms.ListView
    Private WithEvents cmbTier As System.Windows.Forms.ComboBox
    Friend WithEvents btnEnrichmentForAllNames As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAllFeaturesToRun As System.Windows.Forms.Button
    Friend WithEvents listFeaturesToRun As System.Windows.Forms.ListView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents listFeatureFiles As System.Windows.Forms.ListView
    Friend WithEvents lblLoadFile As System.Windows.Forms.Label
    Friend WithEvents btnCustomizeFilters As System.Windows.Forms.Button
    Friend WithEvents btnRemoveFOI As System.Windows.Forms.Button
    Friend WithEvents btnLoadCustomGenomicFeatures As System.Windows.Forms.Button
    Friend WithEvents txtJobName As System.Windows.Forms.TextBox
    Friend WithEvents rbUseAnalytical As System.Windows.Forms.RadioButton
    Friend WithEvents rbUseMonteCarlo As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents rbChiSquareTest As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents rbBinomialDistrobution As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbStrandsToAnalyze As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbMatrixWeighting As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxPearsons As System.Windows.Forms.GroupBox
    Friend WithEvents txtPearsonAudjustmentConstant As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxPercentAudjustment As System.Windows.Forms.GroupBox
    Friend WithEvents rbLinear As System.Windows.Forms.RadioButton
    Friend WithEvents rbSquared As System.Windows.Forms.RadioButton
    Friend WithEvents txtPvalueThreshold As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtNumMCtoRun As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnRemoveFeaturesToRun As System.Windows.Forms.Button
    Friend WithEvents SetDatabaseConnectionSettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblBackground As System.Windows.Forms.Label
    Friend WithEvents CreateLocalGenomeRunnerTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtproximity As System.Windows.Forms.NumericUpDown
    Friend WithEvents BackgroundWorkerEnrichmentAnalysis As System.ComponentModel.BackgroundWorker
    Friend WithEvents BackgroundWorkerAnnotationAnalysis As System.ComponentModel.BackgroundWorker
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents UseNCBI36hg18GenomeAssemblyasBackgroundToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbTradMC As System.Windows.Forms.RadioButton
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents cmbDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents lblOrganism As System.Windows.Forms.Label
    Friend WithEvents cmbOrganism As System.Windows.Forms.ComboBox
    Friend WithEvents mnuMergeLogFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkbxShortOnly As System.Windows.Forms.CheckBox
    Friend WithEvents lnklblHost As System.Windows.Forms.LinkLabel
    Friend WithEvents btnMerge As System.Windows.Forms.Button

End Class
