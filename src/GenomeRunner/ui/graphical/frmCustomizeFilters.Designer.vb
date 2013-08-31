<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCustomizeFilters
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCustomizeFilters))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtPromoterUpstream = New System.Windows.Forms.NumericUpDown()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtPromoterDownstream = New System.Windows.Forms.NumericUpDown()
        Me.btnDone = New System.Windows.Forms.Button()
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox6.SuspendLayout()
        CType(Me.txtPromoterUpstream, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPromoterDownstream, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = True
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 106)
        Me.TableLayoutPanel1.MaximumSize = New System.Drawing.Size(457, 552)
        Me.TableLayoutPanel1.MinimumSize = New System.Drawing.Size(457, 552)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(457, 552)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.txtPromoterUpstream)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.txtPromoterDownstream)
        Me.GroupBox6.Location = New System.Drawing.Point(12, 50)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(457, 50)
        Me.GroupBox6.TabIndex = 93
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Promoter Definition"
        '
        'txtPromoterUpstream
        '
        Me.txtPromoterUpstream.Location = New System.Drawing.Point(106, 19)
        Me.txtPromoterUpstream.Maximum = New Decimal(New Integer() {200000000, 0, 0, 0})
        Me.txtPromoterUpstream.Name = "txtPromoterUpstream"
        Me.txtPromoterUpstream.Size = New System.Drawing.Size(102, 20)
        Me.txtPromoterUpstream.TabIndex = 88
        Me.ToolTip2.SetToolTip(Me.txtPromoterUpstream, "The number of base pairs that a promoter regions begins before the gene's startpo" & _
                "int")
        Me.txtPromoterUpstream.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(24, 21)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 13)
        Me.Label10.TabIndex = 89
        Me.Label10.Text = "Upstream (bp):"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(219, 23)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(90, 13)
        Me.Label11.TabIndex = 91
        Me.Label11.Text = "Downstream (bp):"
        '
        'txtPromoterDownstream
        '
        Me.txtPromoterDownstream.Location = New System.Drawing.Point(315, 21)
        Me.txtPromoterDownstream.Maximum = New Decimal(New Integer() {200000000, 0, 0, 0})
        Me.txtPromoterDownstream.Name = "txtPromoterDownstream"
        Me.txtPromoterDownstream.Size = New System.Drawing.Size(102, 20)
        Me.txtPromoterDownstream.TabIndex = 90
        Me.ToolTip2.SetToolTip(Me.txtPromoterDownstream, "The number of base pairs that the promoter region after the gene's startpoint")
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(203, 664)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 1
        Me.btnDone.Text = "Done"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(381, 26)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Range of threshold values is shown next to genomic feature name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "e.g. DNAse clust" & _
            "ers [16-1000]"
        '
        'frmCustomizeFilters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(481, 689)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCustomizeFilters"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Customize thresholds for each feature"
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.txtPromoterUpstream, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPromoterDownstream, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents ToolTip2 As System.Windows.Forms.ToolTip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents txtPromoterUpstream As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtPromoterDownstream As System.Windows.Forms.NumericUpDown
End Class
