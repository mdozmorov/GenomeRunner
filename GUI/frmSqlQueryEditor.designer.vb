<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMysqlQueryEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMysqlQueryEditor))
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.txtQuery = New System.Windows.Forms.RichTextBox()
        Me.LabelStatus = New System.Windows.Forms.Label()
        Me.btnSkip = New System.Windows.Forms.Button()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnSubmit
        '
        Me.btnSubmit.Location = New System.Drawing.Point(471, 393)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(75, 23)
        Me.btnSubmit.TabIndex = 2
        Me.btnSubmit.Text = "Submit"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'txtQuery
        '
        Me.txtQuery.Location = New System.Drawing.Point(12, 36)
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(534, 351)
        Me.txtQuery.TabIndex = 1
        Me.txtQuery.Text = ""
        '
        'LabelStatus
        '
        Me.LabelStatus.AutoSize = True
        Me.LabelStatus.Location = New System.Drawing.Point(12, 9)
        Me.LabelStatus.Name = "LabelStatus"
        Me.LabelStatus.Size = New System.Drawing.Size(0, 13)
        Me.LabelStatus.TabIndex = 6
        '
        'btnSkip
        '
        Me.btnSkip.Location = New System.Drawing.Point(380, 393)
        Me.btnSkip.Name = "btnSkip"
        Me.btnSkip.Size = New System.Drawing.Size(75, 23)
        Me.btnSkip.TabIndex = 3
        Me.btnSkip.Text = "Skip"
        Me.btnSkip.UseVisualStyleBackColor = True
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.AutoSize = True
        Me.LabelErrorMessage.Location = New System.Drawing.Point(12, 22)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(0, 13)
        Me.LabelErrorMessage.TabIndex = 9
        '
        'FormMysqlQueryEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 425)
        Me.Controls.Add(Me.LabelErrorMessage)
        Me.Controls.Add(Me.btnSkip)
        Me.Controls.Add(Me.LabelStatus)
        Me.Controls.Add(Me.txtQuery)
        Me.Controls.Add(Me.btnSubmit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormMysqlQueryEditor"
        Me.ShowIcon = False
        Me.Text = "Query Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents txtQuery As System.Windows.Forms.RichTextBox
    Friend WithEvents LabelStatus As System.Windows.Forms.Label
    Friend WithEvents btnSkip As System.Windows.Forms.Button
    Friend WithEvents LabelErrorMessage As System.Windows.Forms.Label
End Class
