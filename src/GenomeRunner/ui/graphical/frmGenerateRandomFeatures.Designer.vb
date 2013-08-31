<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenerateRandomFeatures
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
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNumOfFeaturesToCreate = New System.Windows.Forms.NumericUpDown()
        Me.txtMeanWidth = New System.Windows.Forms.NumericUpDown()
        Me.SaveFD = New System.Windows.Forms.SaveFileDialog()
        Me.btnCancel = New System.Windows.Forms.Button()
        CType(Me.txtNumOfFeaturesToCreate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMeanWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(34, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Mean Width of Features:"
        Me.ToolTip2.SetToolTip(Me.Label2, "The average width of the features to be generated")
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(58, 63)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Generate"
        Me.ToolTip2.SetToolTip(Me.Button1, "Generate the Random Features with the selected prarameters")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Number of Random Features:"
        '
        'txtNumOfFeaturesToCreate
        '
        Me.txtNumOfFeaturesToCreate.Location = New System.Drawing.Point(164, 7)
        Me.txtNumOfFeaturesToCreate.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.txtNumOfFeaturesToCreate.Name = "txtNumOfFeaturesToCreate"
        Me.txtNumOfFeaturesToCreate.Size = New System.Drawing.Size(120, 20)
        Me.txtNumOfFeaturesToCreate.TabIndex = 7
        Me.txtNumOfFeaturesToCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtNumOfFeaturesToCreate.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'txtMeanWidth
        '
        Me.txtMeanWidth.Location = New System.Drawing.Point(164, 33)
        Me.txtMeanWidth.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.txtMeanWidth.Name = "txtMeanWidth"
        Me.txtMeanWidth.Size = New System.Drawing.Size(120, 20)
        Me.txtMeanWidth.TabIndex = 8
        Me.txtMeanWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMeanWidth.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(177, 63)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmGenerateRandomFeatures
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(293, 91)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtMeanWidth)
        Me.Controls.Add(Me.txtNumOfFeaturesToCreate)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGenerateRandomFeatures"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Generate Random Features"
        CType(Me.txtNumOfFeaturesToCreate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMeanWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip2 As System.Windows.Forms.ToolTip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents txtNumOfFeaturesToCreate As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtMeanWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents SaveFD As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
