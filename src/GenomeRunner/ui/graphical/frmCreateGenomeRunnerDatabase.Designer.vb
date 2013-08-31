<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateGenomeRunnerDatabase
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreateGenomeRunnerDatabase))
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.txtHost = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDatabase = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnPrepareFiles = New System.Windows.Forms.Button()
        Me.btnCreateTables = New System.Windows.Forms.Button()
        Me.btnLoadData = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnGenerateExonTables = New System.Windows.Forms.Button()
        Me.btnAddRepeatMasker = New System.Windows.Forms.Button()
        Me.btnCheckIntegrity = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSetDownloadDir = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnDownloadGenomicFeatureTable = New System.Windows.Forms.Button()
        Me.SaveFD = New System.Windows.Forms.SaveFileDialog()
        Me.listGenomicFeatures = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblGenomeRunnerTableStatus = New System.Windows.Forms.Label()
        Me.lblDownDir = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtUser
        '
        Me.txtUser.Location = New System.Drawing.Point(67, 86)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(117, 20)
        Me.txtUser.TabIndex = 3
        Me.txtUser.Text = "root"
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(67, 34)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(117, 20)
        Me.txtHost.TabIndex = 1
        Me.txtHost.Text = "localhost"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "User"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Password"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Host"
        '
        'txtDatabase
        '
        Me.txtDatabase.Location = New System.Drawing.Point(67, 60)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(117, 20)
        Me.txtDatabase.TabIndex = 2
        Me.txtDatabase.Text = "hg18test"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Database"
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(3, 138)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(184, 23)
        Me.btnConnect.TabIndex = 5
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(1, 507)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(456, 19)
        Me.ProgressBar1.TabIndex = 36
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(7, 492)
        Me.lblProgress.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(66, 13)
        Me.lblProgress.TabIndex = 35
        Me.lblProgress.Text = "Progress bar"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(67, 112)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(117, 20)
        Me.txtPassword.TabIndex = 4
        Me.txtPassword.Text = "123"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(194, 4)
        Me.Label6.MaximumSize = New System.Drawing.Size(260, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(258, 78)
        Me.Label6.TabIndex = 52
        Me.Label6.Text = resources.GetString("Label6.Text")
        '
        'btnPrepareFiles
        '
        Me.btnPrepareFiles.Enabled = False
        Me.btnPrepareFiles.Location = New System.Drawing.Point(14, 19)
        Me.btnPrepareFiles.Name = "btnPrepareFiles"
        Me.btnPrepareFiles.Size = New System.Drawing.Size(151, 23)
        Me.btnPrepareFiles.TabIndex = 9
        Me.btnPrepareFiles.Text = "1. Prepare Files"
        Me.btnPrepareFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrepareFiles.UseVisualStyleBackColor = True
        '
        'btnCreateTables
        '
        Me.btnCreateTables.Enabled = False
        Me.btnCreateTables.Location = New System.Drawing.Point(13, 49)
        Me.btnCreateTables.Name = "btnCreateTables"
        Me.btnCreateTables.Size = New System.Drawing.Size(152, 23)
        Me.btnCreateTables.TabIndex = 10
        Me.btnCreateTables.Text = "2. Create Tables"
        Me.btnCreateTables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCreateTables.UseVisualStyleBackColor = True
        '
        'btnLoadData
        '
        Me.btnLoadData.Enabled = False
        Me.btnLoadData.Location = New System.Drawing.Point(14, 78)
        Me.btnLoadData.Name = "btnLoadData"
        Me.btnLoadData.Size = New System.Drawing.Size(151, 23)
        Me.btnLoadData.TabIndex = 11
        Me.btnLoadData.Text = "3. Populate Database"
        Me.btnLoadData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoadData.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnGenerateExonTables)
        Me.GroupBox1.Controls.Add(Me.btnAddRepeatMasker)
        Me.GroupBox1.Controls.Add(Me.btnPrepareFiles)
        Me.GroupBox1.Controls.Add(Me.btnLoadData)
        Me.GroupBox1.Controls.Add(Me.btnCreateTables)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 315)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(176, 165)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Add Features"
        Me.ToolTip1.SetToolTip(Me.GroupBox1, "Adds all genomic featues which have not yet been added")
        '
        'btnGenerateExonTables
        '
        Me.btnGenerateExonTables.Enabled = False
        Me.btnGenerateExonTables.Location = New System.Drawing.Point(13, 105)
        Me.btnGenerateExonTables.Name = "btnGenerateExonTables"
        Me.btnGenerateExonTables.Size = New System.Drawing.Size(152, 23)
        Me.btnGenerateExonTables.TabIndex = 12
        Me.btnGenerateExonTables.Text = "4. Generate Exon Tables"
        Me.btnGenerateExonTables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerateExonTables.UseVisualStyleBackColor = True
        '
        'btnAddRepeatMasker
        '
        Me.btnAddRepeatMasker.Enabled = False
        Me.btnAddRepeatMasker.Location = New System.Drawing.Point(13, 134)
        Me.btnAddRepeatMasker.Name = "btnAddRepeatMasker"
        Me.btnAddRepeatMasker.Size = New System.Drawing.Size(152, 23)
        Me.btnAddRepeatMasker.TabIndex = 13
        Me.btnAddRepeatMasker.Text = "Optional: install repeat masker"
        Me.ToolTip1.SetToolTip(Me.btnAddRepeatMasker, "Downloads the repeat masker table")
        Me.btnAddRepeatMasker.UseVisualStyleBackColor = True
        '
        'btnCheckIntegrity
        '
        Me.btnCheckIntegrity.Enabled = False
        Me.btnCheckIntegrity.Location = New System.Drawing.Point(4, 219)
        Me.btnCheckIntegrity.Name = "btnCheckIntegrity"
        Me.btnCheckIntegrity.Size = New System.Drawing.Size(184, 23)
        Me.btnCheckIntegrity.TabIndex = 7
        Me.btnCheckIntegrity.Text = "Check database integrity  "
        Me.ToolTip1.SetToolTip(Me.btnCheckIntegrity, "Ensures that the same number of rows exist in the local database as the remote se" & _
                "rver")
        Me.btnCheckIntegrity.UseVisualStyleBackColor = True
        '
        'btnSetDownloadDir
        '
        Me.btnSetDownloadDir.Location = New System.Drawing.Point(3, 268)
        Me.btnSetDownloadDir.Name = "btnSetDownloadDir"
        Me.btnSetDownloadDir.Size = New System.Drawing.Size(184, 23)
        Me.btnSetDownloadDir.TabIndex = 8
        Me.btnSetDownloadDir.Text = "Set Download Directory"
        Me.ToolTip1.SetToolTip(Me.btnSetDownloadDir, "Set the location to which genomic feature data files should be downloaded before " & _
                "being imported into local database")
        Me.btnSetDownloadDir.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(294, 457)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(163, 23)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "Finish: Sync GenomeRunner Features Available to Datatables"
        Me.ToolTip1.SetToolTip(Me.Button1, "Makes those Genomic Features which have been successfully added to the database a" & _
                "vailable in GenomeRunner")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnDownloadGenomicFeatureTable
        '
        Me.btnDownloadGenomicFeatureTable.Enabled = False
        Me.btnDownloadGenomicFeatureTable.Location = New System.Drawing.Point(3, 180)
        Me.btnDownloadGenomicFeatureTable.Name = "btnDownloadGenomicFeatureTable"
        Me.btnDownloadGenomicFeatureTable.Size = New System.Drawing.Size(184, 23)
        Me.btnDownloadGenomicFeatureTable.TabIndex = 6
        Me.btnDownloadGenomicFeatureTable.Text = "Install GenomeRunnerTable"
        Me.ToolTip1.SetToolTip(Me.btnDownloadGenomicFeatureTable, "Downloads the GenomeRunner table from the remote server and adds it to the local " & _
                "database")
        Me.btnDownloadGenomicFeatureTable.UseVisualStyleBackColor = True
        '
        'listGenomicFeatures
        '
        Me.listGenomicFeatures.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.listGenomicFeatures.Location = New System.Drawing.Point(197, 85)
        Me.listGenomicFeatures.Name = "listGenomicFeatures"
        Me.listGenomicFeatures.ShowItemToolTips = True
        Me.listGenomicFeatures.Size = New System.Drawing.Size(260, 366)
        Me.listGenomicFeatures.TabIndex = 57
        Me.listGenomicFeatures.UseCompatibleStateImageBehavior = False
        Me.listGenomicFeatures.View = System.Windows.Forms.View.List
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Width = 214
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnCheckIntegrity)
        Me.GroupBox2.Controls.Add(Me.lblGenomeRunnerTableStatus)
        Me.GroupBox2.Controls.Add(Me.lblDownDir)
        Me.GroupBox2.Controls.Add(Me.btnSetDownloadDir)
        Me.GroupBox2.Controls.Add(Me.btnDownloadGenomicFeatureTable)
        Me.GroupBox2.Controls.Add(Me.btnConnect)
        Me.GroupBox2.Controls.Add(Me.txtUser)
        Me.GroupBox2.Controls.Add(Me.txtPassword)
        Me.GroupBox2.Controls.Add(Me.txtHost)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtDatabase)
        Me.GroupBox2.Location = New System.Drawing.Point(1, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(190, 297)
        Me.GroupBox2.TabIndex = 58
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Enter Local Database Connection Settings:"
        '
        'lblGenomeRunnerTableStatus
        '
        Me.lblGenomeRunnerTableStatus.AutoSize = True
        Me.lblGenomeRunnerTableStatus.Location = New System.Drawing.Point(11, 164)
        Me.lblGenomeRunnerTableStatus.Name = "lblGenomeRunnerTableStatus"
        Me.lblGenomeRunnerTableStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblGenomeRunnerTableStatus.TabIndex = 61
        '
        'lblDownDir
        '
        Me.lblDownDir.AutoEllipsis = True
        Me.lblDownDir.AutoSize = True
        Me.lblDownDir.Location = New System.Drawing.Point(6, 252)
        Me.lblDownDir.MaximumSize = New System.Drawing.Size(180, 13)
        Me.lblDownDir.Name = "lblDownDir"
        Me.lblDownDir.Size = New System.Drawing.Size(106, 13)
        Me.lblDownDir.TabIndex = 59
        Me.lblDownDir.Text = "Download Directory: "
        '
        'frmCreateGenomeRunnerDatabase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 535)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.listGenomicFeatures)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.lblProgress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCreateGenomeRunnerDatabase"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Local Database Re-Creator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnPrepareFiles As System.Windows.Forms.Button
    Friend WithEvents btnCreateTables As System.Windows.Forms.Button
    Friend WithEvents btnLoadData As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SaveFD As System.Windows.Forms.SaveFileDialog
    Friend WithEvents listGenomicFeatures As System.Windows.Forms.ListView
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnDownloadGenomicFeatureTable As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnSetDownloadDir As System.Windows.Forms.Button
    Friend WithEvents lblDownDir As System.Windows.Forms.Label
    Friend WithEvents lblGenomeRunnerTableStatus As System.Windows.Forms.Label
    Friend WithEvents btnGenerateExonTables As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnAddRepeatMasker As System.Windows.Forms.Button
    Friend WithEvents btnCheckIntegrity As System.Windows.Forms.Button

End Class
