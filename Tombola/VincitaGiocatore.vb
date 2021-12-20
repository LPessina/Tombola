Public Class VincitaGiocatore

    Private Sub VincitaGiocatore_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CaricaElencoGiocatori()
    End Sub
    Private Sub CaricaElencoGiocatori()
        For i As Integer = 0 To Form1.lst_ElencoGiocatori.Items.Count - 1
            Me.cb_ElencoGiocatori.Items.Add(Form1.lst_ElencoGiocatori.Items(i))
        Next
    End Sub

    Private Sub cmd_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_OK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmd_Annulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Annulla.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub
End Class