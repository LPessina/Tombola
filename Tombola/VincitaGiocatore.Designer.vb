<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VincitaGiocatore
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VincitaGiocatore))
        Me.cb_ElencoGiocatori = New System.Windows.Forms.ComboBox
        Me.lbl_Vincita = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmd_OK = New System.Windows.Forms.Button
        Me.cmd_Annulla = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cb_ElencoGiocatori
        '
        Me.cb_ElencoGiocatori.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cb_ElencoGiocatori.ForeColor = System.Drawing.Color.DarkSlateBlue
        Me.cb_ElencoGiocatori.FormattingEnabled = True
        Me.cb_ElencoGiocatori.Location = New System.Drawing.Point(119, 33)
        Me.cb_ElencoGiocatori.Name = "cb_ElencoGiocatori"
        Me.cb_ElencoGiocatori.Size = New System.Drawing.Size(150, 26)
        Me.cb_ElencoGiocatori.TabIndex = 0
        '
        'lbl_Vincita
        '
        Me.lbl_Vincita.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Vincita.Location = New System.Drawing.Point(12, 4)
        Me.lbl_Vincita.Name = "lbl_Vincita"
        Me.lbl_Vincita.Size = New System.Drawing.Size(268, 22)
        Me.lbl_Vincita.TabIndex = 1
        Me.lbl_Vincita.Text = "AMBO"
        Me.lbl_Vincita.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Seleziona il vincitore:"
        '
        'cmd_OK
        '
        Me.cmd_OK.Location = New System.Drawing.Point(10, 70)
        Me.cmd_OK.Name = "cmd_OK"
        Me.cmd_OK.Size = New System.Drawing.Size(46, 22)
        Me.cmd_OK.TabIndex = 3
        Me.cmd_OK.Text = "OK"
        Me.cmd_OK.UseVisualStyleBackColor = True
        '
        'cmd_Annulla
        '
        Me.cmd_Annulla.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmd_Annulla.Location = New System.Drawing.Point(60, 70)
        Me.cmd_Annulla.Name = "cmd_Annulla"
        Me.cmd_Annulla.Size = New System.Drawing.Size(50, 22)
        Me.cmd_Annulla.TabIndex = 4
        Me.cmd_Annulla.Text = "Annulla"
        Me.cmd_Annulla.UseVisualStyleBackColor = True
        '
        'VincitaGiocatore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmd_Annulla
        Me.ClientSize = New System.Drawing.Size(286, 99)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmd_Annulla)
        Me.Controls.Add(Me.cmd_OK)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl_Vincita)
        Me.Controls.Add(Me.cb_ElencoGiocatori)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "VincitaGiocatore"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Assegna Vincita"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cb_ElencoGiocatori As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Vincita As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmd_OK As System.Windows.Forms.Button
    Friend WithEvents cmd_Annulla As System.Windows.Forms.Button
End Class
