Public Class Form1

    Public NumeroEstratto As Integer
    Public UltimoNumeroEstratto As Integer
    Public PenultimoNumeroEstratto As Integer

    Public AssegnatoAMBO As Boolean
    Public AssegnatoTERNO As Boolean
    Public AssegnatoQUATERNA As Boolean
    Public AssegnatoCINQUINA As Boolean
    Public AssegnatoTOMBOLA As Boolean
    Public AssegnatoTOMBOLINO As Boolean
    Public ArrayCartelleComputer() As String

    Public NumeroEstrazione As Integer
    Public ArrayNumeri(89) As Integer
    Public ArrayEstratti(89) As Integer
    Public SecondiAttesa As Integer
    Public AttivaRandom As Boolean
    Public SecondiPassati As Integer
    Public NumeroPartita As Integer = 0
    Public NumeroGiocatori As Integer = 0
    Public ComputerAbilitato As Boolean = False
    Public Ammontare As Integer
    Public CalcoloInCorso As Boolean = False

    Public Tab_Tombola As TabPage
    Public Tab_Giocatori As TabPage
    Public Tab_Risultati As TabPage

    Public InPausa As Boolean
    Public Tombolino As Boolean = False

    Const STR_INIZIA = "Parti"
    Const STR_PAUSA = "Pausa"
    Const STR_RICOMINCIA = "Ricomincia"
    Const STR_ESTRAINUMERO = "Estrai numero"

    Public ColoreNumeroDefault As Color = Color.White
    Public ColoreNumeroEvidenziato As Color = Color.Yellow
    Public ColoreNumeroEstratto As Color = Color.Green


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Tab_Tombola = Me.TabTombola
        Tab_Giocatori = Me.TabGiocatori
        Tab_Risultati = Me.TabRisultati
        Me.TabControl1.TabPages.Remove(Tab_Tombola)
        Me.TabControl1.TabPages.Remove(Tab_Risultati)

        init()
        caricaGiocatori()
    End Sub
    Private Sub caricaGiocatori()
        Dim elencoGiocatori As String = ""
        elencoGiocatori = LoadFromIni("Giocatori", "Elenco", IO.Path.GetDirectoryName(Application.ExecutablePath) & "\giocatori.ini")
        Dim arrayGiocatori() = Split(elencoGiocatori, ",")
        For i As Integer = 0 To arrayGiocatori.Length - 1
            AggiungiGiocatore(arrayGiocatori(i))
        Next
    End Sub
    Private Sub init()
        AbilitaTimer()

        AssegnatoAMBO = False
        AssegnatoTERNO = False
        AssegnatoQUATERNA = False
        AssegnatoCINQUINA = False
        AssegnatoTOMBOLA = False
        AssegnatoTOMBOLINO = False

        Ammontare = 0
        SecondiPassati = 0
        InPausa = True
        NumeroEstrazione = 0
        NumeroPartita += 1
        Me.lbl_Partita.Text = NumeroPartita
        Me.cmd_Pausa.Text = STR_INIZIA

        Me.cmd_Dichiara_AMBO.Enabled = False
        Me.cmd_Dichiara_TERNO.Enabled = False
        Me.cmd_Dichiara_QUATERNA.Enabled = False
        Me.cmd_Dichiara_CINQUINA.Enabled = False
        Me.cmd_Dichiara_TOMBOLA.Enabled = False
        Me.cmd_Dichiara_TOMBOLINO.Enabled = False

        AbilitaPronuncia()
        ResetTavola()
        Me.lbl_UltimiNumeri.Text = ""
        Me.lbl_NumeroEstratto.Text = "00"
        Me.NumeroEstrazione = 0

        Array.Resize(ArrayEstratti, 90)
        Array.Clear(ArrayEstratti, 0, 90)
        Array.Resize(ArrayNumeri, 90)
        Array.Clear(ArrayNumeri, 0, 90)

        Dim i
        For i = 1 To 90
            ArrayNumeri(i - 1) = i
        Next

        Me.chk_Tombolino.Checked = Tombolino
        SetTombolino()

        If Me.rb_Manuale.Checked Then
            SetManuale()
        Else
            SetAutomatico()
        End If

    End Sub
    Private Sub ResetVittorie()
        Me.lv_Risultati.Items.Clear()
    End Sub

    Public Sub EstraiNumero()

        Me.Timer1.Enabled = False

        If NumeroEstrazione > 90 Then Exit Sub

        Me.cmd_Pausa.Enabled = False

        Dim colore As Color
        colore = Color.Orange
        NumeroEstrazione += 1
        If NumeroEstrazione > 2 Then
            PenultimoNumeroEstratto = UltimoNumeroEstratto
            UltimoNumeroEstratto = NumeroEstratto
            Me.lbl_PenultimoNumeroEstratto.Text = PenultimoNumeroEstratto
            If Not Me.AssegnatoTERNO And Me.AssegnatoAMBO Then
                Me.cmd_Dichiara_TERNO.Enabled = True
                Me.cmd_Dichiara_TERNO.BackColor = colore
            End If
        End If
        If NumeroEstrazione > 1 Then
            UltimoNumeroEstratto = NumeroEstratto
            Me.lbl_UltimoNumeroEstratto.Text = UltimoNumeroEstratto
            If Not Me.AssegnatoAMBO Then
                Me.cmd_Dichiara_AMBO.Enabled = True
                Me.cmd_Dichiara_AMBO.BackColor = colore
            End If
        End If
        If NumeroEstrazione > 3 Then
            If Not Me.AssegnatoQUATERNA And Me.AssegnatoTERNO Then
                Me.cmd_Dichiara_QUATERNA.Enabled = True
                Me.cmd_Dichiara_QUATERNA.BackColor = colore
            End If
        End If
        If NumeroEstrazione > 4 Then
            If Not Me.AssegnatoCINQUINA And Me.AssegnatoQUATERNA Then
                Me.cmd_Dichiara_CINQUINA.Enabled = True
                Me.cmd_Dichiara_CINQUINA.BackColor = colore
            End If
        End If
        If NumeroEstrazione > 5 Then
            If Not Me.AssegnatoTOMBOLA And Me.AssegnatoCINQUINA Then
                Me.cmd_Dichiara_TOMBOLA.Enabled = True
                Me.cmd_Dichiara_TOMBOLA.BackColor = colore
            End If
        End If
        If NumeroEstrazione > 6 Then
            If Not Me.AssegnatoTOMBOLINO And Me.AssegnatoTOMBOLA Then
                Me.cmd_Dichiara_TOMBOLINO.Enabled = True
                Me.cmd_Dichiara_TOMBOLINO.BackColor = colore
            End If
        End If

        Dim n As Decimal
        Dim upperbound = ArrayNumeri.Length
        Dim lowerbound = 1
        Randomize()
        n = CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))

        ArrayEstratti(NumeroEstrazione - 1) = ArrayNumeri(n - 1)
        NumeroEstratto = ArrayNumeri(n - 1)

        ArrayNumeri(n - 1) = 0
        Dim tempArray(0) As Integer
        Array.Resize(tempArray, ArrayNumeri.Length)
        Array.Clear(tempArray, 0, tempArray.Length)
        Array.Copy(ArrayNumeri, tempArray, ArrayNumeri.Length)

        Array.Resize(ArrayNumeri, ArrayNumeri.Length - 1)
        Array.Clear(ArrayNumeri, 0, ArrayNumeri.Length)
        Dim i, j
        j = 0
        For i = 0 To tempArray.Length - 1
            If tempArray(i) <> 0 Then
                ArrayNumeri(j) = tempArray(i)
                j += 1
            End If
        Next

        Me.lbl_NumeroEstrazione.Text = NumeroEstrazione
        Me.lbl_NumeroEstratto.Text = NumeroEstratto
        Me.lbl_mancanti.Text = 90 - NumeroEstrazione
        Me.Text = "Tombola  [" & NumeroEstrazione & ":" & NumeroEstratto.ToString & "]"

        Me.lbl_UltimiNumeri.Text &= IIf(NumeroEstratto < 10, " ", "") & NumeroEstratto & " "
        If (NumeroEstrazione Mod 10) = 0 Then
            Me.lbl_UltimiNumeri.Text &= vbCrLf
        End If

        EvidenziaNumero(NumeroEstratto, Me.ColoreNumeroEvidenziato)
        Application.DoEvents()

        If Me.chk_AbilitaPronuncia.Checked Then PronunciaNumero(NumeroEstratto)

        If ComputerAbilitato Then
            If Me.cmd_Dichiara_AMBO.Enabled Then
                If ControllaVincitaComputer("AMBO") Then
                    AssegnaAmbo("_Computer")
                    MsgBox("AMBO per giocatore _Computer")
                End If
                GoTo DopoControllo
            End If
            If Me.cmd_Dichiara_TERNO.Enabled Then
                If ControllaVincitaComputer("TERNO") Then
                    AssegnaTerno("_Computer")
                    MsgBox("TERNO per giocatore _Computer")
                End If
                GoTo DopoControllo
            End If
            If Me.cmd_Dichiara_QUATERNA.Enabled Then
                If ControllaVincitaComputer("QUATERNA") Then
                    AssegnaQuaterna("_Computer")
                    MsgBox("QUATERNA per giocatore _Computer")
                End If
                GoTo DopoControllo
            End If
            If Me.cmd_Dichiara_CINQUINA.Enabled Then
                If ControllaVincitaComputer("CINQUINA") Then
                    AssegnaCinquina("_Computer")
                    MsgBox("CINQUINA per giocatore _Computer")
                End If
                GoTo DopoControllo
            End If
            If Me.cmd_Dichiara_TOMBOLA.Enabled Then
                If ControllaVincitaComputer("TOMBOLA") Then
                    AssegnaTombola("_Computer")
                    MsgBox("TOMBOLA per giocatore _Computer")
                End If
                GoTo DopoControllo
            End If
            If Me.cmd_Dichiara_TOMBOLINO.Enabled Then
                If ControllaVincitaComputer("TOMBOLINO") Then
                    AssegnaTombolino("_Computer")
                    MsgBox("TOMBOLINO per giocatore _Computer")
                End If
            End If
        End If

DopoControllo:
        Me.Timer1.Enabled = True

        If (NumeroEstrazione = 90) Then PartitaTerminata()
        If Not Tombolino And AssegnatoTOMBOLA Then PartitaTerminata()
        If Tombolino And AssegnatoTOMBOLINO Then PartitaTerminata()

    End Sub
    Private Sub EvidenziaNumero(ByVal numero As Integer, ByVal colore As Color)
        Dim c As Control
        For Each c In Me.grp_Tavola.Controls
            If c.Text = numero.ToString Then
                c.BackColor = colore
                ToolTip1.SetToolTip(c, "Estratto N° " & NumeroEstrazione)
            End If
        Next
        Me.Timer2.Enabled = True
    End Sub
    Private Sub ResetTavola()
        Dim c As Control
        For Each c In Me.grp_Tavola.Controls
            If c.Parent Is Me.grp_Tavola Then
                c.BackColor = Me.ColoreNumeroDefault
                ToolTip1.SetToolTip(c, "")
            End If
        Next
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not InPausa Then
            SecondiPassati += 1
            Me.lbl_SecRestanti.Text = SecondiAttesa / 1000 - SecondiPassati
            If SecondiPassati = SecondiAttesa / 1000 Then
                SecondiPassati = 0
                Me.lbl_SecRestanti.Text = SecondiAttesa / 1000
                EstraiNumero()
            End If
        Else
            If NumeroEstrazione = 90 Then
                MsgBox("Terminato")
            End If
        End If
    End Sub
    Private Sub ImpostaSecondiAttesa()
        SecondiPassati = 0
        SecondiAttesa = Me.NumericUpDown1.Value * 1000
        Me.lbl_SecRestanti.Text = SecondiAttesa / 1000
    End Sub
    Private Sub SetTombolino()
        Me.cmd_Dichiara_TOMBOLINO.Visible = Tombolino

        If Tombolino Then
            Me.lbl_Vincita_TOMBOLINO.ForeColor = Color.Black
            Me.txt_Vincita_TOMBOLINO.Enabled = True
        Else
            Me.lbl_Vincita_TOMBOLINO.ForeColor = Color.Gray
            Me.txt_Vincita_TOMBOLINO.Enabled = False
        End If

    End Sub


    Private Sub AbilitaTimer()
        Me.Timer1.Enabled = False
        ImpostaSecondiAttesa()
        If Me.rb_Automatico.Checked Then
            Me.Timer1.Enabled = True
        Else
            Me.Timer1.Enabled = False
        End If
    End Sub

    Private Sub ResetPartita()
        init()
        If Me.rb_Manuale.Checked Then
            Me.cmd_Pausa.Text = STR_ESTRAINUMERO
        Else
            Me.cmd_Pausa.Text = STR_INIZIA
        End If

        Me.grp_Giocatori.Enabled = True
        Me.grp_Cartelle.Enabled = True
        Me.grp_PianoVincite.Enabled = True
        Me.cmd_ConfermaImpostazioni.Enabled = True
        Me.grp_AutoManual.Enabled = True

        Me.TabControl1.SelectTab(Tab_Giocatori)

    End Sub

    Private Sub rb_Manuale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Manuale.CheckedChanged
        SetManuale()
    End Sub
    Private Sub SetManuale()
        Me.NumericUpDown1.Enabled = False
        Me.cmd_Pausa.Text = STR_ESTRAINUMERO
        AbilitaTimer()
        ImpostaSecondiAttesa()
    End Sub
    Private Sub SetAutomatico()
        Me.NumericUpDown1.Enabled = True
        Me.cmd_Pausa.Text = STR_INIZIA
        AbilitaTimer()
    End Sub
    Private Sub rb_Manuale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Manuale.Click
        SetManuale()
    End Sub

    Private Sub rb_Automatico_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Automatico.CheckedChanged
        SetAutomatico()
    End Sub

    Private Sub cmd_Ripeti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Ripeti.Click
        PronunciaNumero(NumeroEstratto)
    End Sub

    Private Sub cmd_Pronuncia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Pronuncia.Click
        If Me.TextBox1.Text.Length > 0 Then PronunciaNumero(Me.TextBox1.Text)
    End Sub

    Private Sub cmd_Pausa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Pausa.Click
        ClickBottone()
    End Sub
    Private Sub ClickBottone()
        Select Case Me.cmd_Pausa.Text
            Case STR_RICOMINCIA
                ResetPartita()

            Case STR_ESTRAINUMERO
                EstraiNumero()

            Case STR_INIZIA
                InPausa = False
                Me.cmd_Pausa.Text = STR_PAUSA
                If Me.rb_Automatico.Checked Then
                    Me.Timer1.Enabled = True
                    Me.lbl_SecRestanti.Visible = True
                End If
            Case STR_PAUSA
                Ferma()
                'Me.cmd_Pausa.Text = STR_INIZIA
                'If Me.rb_Automatico.Checked Then
                'Me.Timer1.Enabled = False
                'Me.lbl_SecRestanti.Visible = False
                'End If
        End Select
    End Sub


    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        ImpostaSecondiAttesa()
    End Sub

    Private Sub lbl_NumeroEstratto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_NumeroEstratto.Click
        Select Case Me.ColorDialog1.ShowDialog()
            Case Windows.Forms.DialogResult.OK, Windows.Forms.DialogResult.Yes
                Me.lbl_NumeroEstratto.ForeColor = Me.ColorDialog1.Color
            Case Else
                Exit Sub
        End Select
    End Sub

    Private Sub chk_AbilitaPronuncia_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_AbilitaPronuncia.CheckedChanged
        AbilitaPronuncia()
    End Sub
    Private Sub AbilitaPronuncia()
        If Me.chk_AbilitaPronuncia.Checked Then
            Me.chk_AbilitaSmorfia.Visible = True
            Me.chk_PronunciaCifre.Visible = True
            Me.cmd_Ripeti.Visible = True
        Else
            Me.chk_AbilitaSmorfia.Visible = False
            Me.chk_PronunciaCifre.Visible = False
            Me.cmd_Ripeti.Visible = False
        End If
    End Sub

    Private Sub AggiungiGiocatore(ByVal Nome As String)
        If EsisteGiocatore(Me.txt_NomeGiocatore.Text.Trim) Then
            MsgBox("Giocatore [" & Nome & "] già presente")
            Exit Sub
        Else
            Me.lst_ElencoGiocatori.Items.Add(Nome)
            NumeroGiocatori = Me.lst_ElencoGiocatori.Items.Count
            ScriviNumeroGiocatori()
            CalcolaAmmontare()

            AggiungiGiocatoreListaCartelle(Nome)
            Me.txt_NomeGiocatore.Text = Nothing
        End If
    End Sub
    Private Sub AggiungiGiocatoreListaCartelle(ByVal Nome As String)
        Dim itm As New ListViewItem(Nome)
        itm.Tag = Nome
        itm.SubItems.Add("1")
        itm.SubItems.Add(Me.txt_CostoCartella.Text.Trim & " €")
        Me.lv_Cartelle.Items.Add(itm)
    End Sub
    Private Sub RimuoviGiocatoreListaCartelle(ByVal Nome As String)
        Dim itm As ListViewItem
        For Each itm In Me.lv_Cartelle.Items
            If itm.Text = Nome Then Me.lv_Cartelle.Items.Remove(itm)
        Next
    End Sub

    Private Sub ScriviNumeroGiocatori()
        Me.lbl_NumeroGiocatori.Text = "Elenco giocatori  (" & NumeroGiocatori & ")"
    End Sub
    Private Function EsisteGiocatore(ByVal nome As String) As Boolean
        EsisteGiocatore = False
        Dim i
        For Each i In Me.lst_ElencoGiocatori.Items
            If i.ToString.ToLower = nome.ToLower Then
                EsisteGiocatore = True
            End If
        Next
        Return EsisteGiocatore
    End Function
    Private Sub CancellaTuttiIGiocatori()
        Me.lst_ElencoGiocatori.Items.Clear()
        Me.lv_Cartelle.Items.Clear()
        NumeroGiocatori = Me.lst_ElencoGiocatori.Items.Count
        ScriviNumeroGiocatori()
        CalcolaAmmontare()
    End Sub
    Private Sub CancellaGiocatore()
        Dim nome = Me.lst_ElencoGiocatori.SelectedItem
        Me.lst_ElencoGiocatori.Items.RemoveAt(Me.lst_ElencoGiocatori.SelectedIndex)
        RimuoviGiocatoreListaCartelle(nome)

        NumeroGiocatori = Me.lst_ElencoGiocatori.Items.Count
        ScriviNumeroGiocatori()
        CalcolaAmmontare()
    End Sub

    Private Sub cmd_AggiungiGiocatore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_AggiungiGiocatore.Click
        If Me.txt_NomeGiocatore.Text.Trim.Length > 0 Then AggiungiGiocatore(Me.txt_NomeGiocatore.Text.Trim)
    End Sub

    Private Sub cmd_RimuoviGiocatore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_RimuoviGiocatore.Click
        On Error Resume Next
        CancellaGiocatore()
    End Sub

    Private Sub cmd_CancellaElencoGiocatori_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_CancellaElencoGiocatori.Click
        CancellaTuttiIGiocatori()
        CalcolaAmmontare()
    End Sub

    Private Sub RicalcolaCostoCartelle()
        Dim itm As ListViewItem
        For Each itm In Me.lv_Cartelle.Items
            itm.SubItems(2).Text = (CDbl(Me.txt_CostoCartella.Text) * CInt(itm.SubItems(1).Text)).ToString & " €"
        Next
        CalcolaAmmontare()
    End Sub

    Private Sub txt_CostoCartella_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostoCartella.Validated
        Me.txt_CostoCartella.Text = Me.txt_CostoCartella.Text.Replace(".", ",")
        RicalcolaCostoCartelle()
    End Sub

    Private Sub cmd_AggiungiCartella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_AggiungiCartella.Click
        AggiungiCartella()
    End Sub
    Private Sub AggiungiCartella()
        SommaCartella(1)
    End Sub
    Private Sub TogliCartella()
        SommaCartella(-1)
    End Sub
    Private Sub SommaCartella(ByVal valore As Integer)

        Dim itm As ListViewItem
        Dim NCartelle As Integer
        For Each itm In Me.lv_Cartelle.CheckedItems
            NCartelle = CInt(itm.SubItems(1).Text)
            Select Case valore
                Case 1
                    itm.SubItems(1).Text = (NCartelle + valore).ToString
                    itm.SubItems(2).Text = (CDbl(Me.txt_CostoCartella.Text) * (NCartelle + valore)).ToString & " €"
                Case -1
                    If NCartelle > 1 Then
                        itm.SubItems(1).Text = (NCartelle + valore).ToString
                        itm.SubItems(2).Text = (CDbl(Me.txt_CostoCartella.Text) * (NCartelle + valore)).ToString & " €"
                    End If
            End Select
        Next

        CalcolaAmmontare()

    End Sub
    Private Sub CalcolaAmmontare()
        Dim itm As ListViewItem
        Ammontare = 0
        For Each itm In Me.lv_Cartelle.Items
            Ammontare += CDbl(itm.SubItems(2).Text)
        Next
        Me.lbl_Ammontare.Text = Ammontare & " €"
    End Sub
    Private Sub cmd_RimuoviCartella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_RimuoviCartella.Click
        TogliCartella()
    End Sub

    Private Sub chk_TuttiGiocatoriCartelle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_TuttiGiocatoriCartelle.CheckedChanged
        SelezionaTuttiGiocatoriCartelle(chk_TuttiGiocatoriCartelle.Checked)
    End Sub
    Private Sub SelezionaTuttiGiocatoriCartelle(ByVal seleziona As Boolean)
        Dim itm As ListViewItem
        Dim costo As Integer = 0
        For Each itm In Me.lv_Cartelle.Items
            itm.Checked = seleziona
        Next
    End Sub

    Private Sub chk_GiocatoreComputer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_GiocatoreComputer.CheckedChanged
        AbilitaGiocatoreComputer(chk_GiocatoreComputer.Checked)
    End Sub
    Private Sub AbilitaGiocatoreComputer(ByVal abilita As Boolean)

        ComputerAbilitato = abilita
        If abilita Then

            Dim NCartelle As Integer = 0
            Dim f As New GiocatoreComputer
            f.ShowDialog(Me)

            Dim ListaCartelleComputer As String = ""
            If f.chk_Cartella1.Checked Then
                NCartelle = 1
                ListaCartelleComputer = "1"
            End If
            If f.chk_Cartella2.Checked Then
                NCartelle += 1
                ListaCartelleComputer &= IIf(ListaCartelleComputer.Length > 0, ",2", "2")
            End If
            If f.chk_Cartella3.Checked Then
                NCartelle += 1
                ListaCartelleComputer &= IIf(ListaCartelleComputer.Length > 0, ",3", "3")
            End If
            If f.chk_Cartella4.Checked Then
                NCartelle += 1
                ListaCartelleComputer &= IIf(ListaCartelleComputer.Length > 0, ",4", "4")
            End If
            If f.chk_Cartella5.Checked Then
                NCartelle += 1
                ListaCartelleComputer &= IIf(ListaCartelleComputer.Length > 0, ",5", "5")
            End If
            If f.chk_Cartella6.Checked Then
                NCartelle += 1
                ListaCartelleComputer &= IIf(ListaCartelleComputer.Length > 0, ",6", "6")
            End If
            ArrayCartelleComputer = Split(ListaCartelleComputer, ",")

            f.Dispose()
            f = Nothing

            If NCartelle = 0 Then
                Me.chk_GiocatoreComputer.Checked = False
                Exit Sub
            End If

            Dim itm As New ListViewItem("_Computer")
            itm.SubItems.Add(NCartelle)
            itm.SubItems.Add((NCartelle * CDbl(Me.txt_CostoCartella.Text)).ToString & " €")
            Me.lv_Cartelle.Items.Add(itm)

        Else
            Dim itm As ListViewItem
            For Each itm In Me.lv_Cartelle.Items
                If itm.Text = "_Computer" Then
                    Me.lv_Cartelle.Items.Remove(itm)
                End If
            Next
        End If
        CalcolaAmmontare()
    End Sub

    Private Sub chk_Tombolino_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Tombolino.CheckedChanged
        Tombolino = Me.chk_Tombolino.Checked
        SetTombolino()
        CalcolaRipartoVincite()
    End Sub


    Private Sub CalcolaRipartoVincite()
        CalcoloInCorso = True
        If Ammontare = 0 Then
            Me.txt_Vincita_AMBO.Text = 0
            Me.txt_Vincita_TERNO.Text = 0
            Me.txt_Vincita_QUATERNA.Text = 0
            Me.txt_Vincita_CINQUINA.Text = 0
            Me.txt_Vincita_TOMBOLA.Text = 0
            Me.txt_Vincita_TOMBOLINO.Text = 0
            Me.lbl_VerificaAmmontare.Text = 0
            CalcoloInCorso = False
            Exit Sub
        End If

        Dim v As Double = 0
        Dim tot As Double = 0

        ' Piano 1
        If Me.rb_Piano1.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano1_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano1_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano1_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano1_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano1_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 2
        If Me.rb_Piano2.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano2_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano2_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano2_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano2_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano2_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 3
        If Me.rb_Piano3.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano3_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano3_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano3_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano3_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano3_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 4
        If Me.rb_Piano4.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano4_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano4_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano4_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano4_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano4_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 5
        If Me.rb_Piano5.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano5_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano5_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano5_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano5_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano5_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 6
        If Me.rb_Piano6.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano6_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano6_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano6_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano6_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano6_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 7
        If Me.rb_Piano7.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano7_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano7_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano7_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano7_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano7_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 8
        If Me.rb_Piano8.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano8_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano8_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano8_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano8_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano8_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 9
        If Me.rb_Piano9.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano9_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano9_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano9_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano9_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano9_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        ' Piano 10
        If Me.rb_Piano10.Checked Then
            v = Math.Round(Ammontare * CDbl(Me.lbl_piano10_AMBO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_AMBO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano10_TERNO.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_TERNO.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano10_QUATERNA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_QUATERNA.Text = v.ToString
            tot += v

            v = Math.Round(Ammontare * CDbl(Me.lbl_piano10_CINQUINA.Text) / 100)
            If v = 0 Then v = 0.5
            Me.txt_Vincita_CINQUINA.Text = v.ToString
            tot += v

            If Tombolino Then
                v = Math.Round(Ammontare * CDbl(Me.lbl_piano10_TOMBOLINO.Text) / 100)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLINO.Text = v.ToString
                tot += v

                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text) - CDbl(Me.txt_Vincita_TOMBOLINO.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                tot += v
            Else
                v = Ammontare - CDbl(Me.txt_Vincita_AMBO.Text) - CDbl(Me.txt_Vincita_TERNO.Text) - CDbl(Me.txt_Vincita_QUATERNA.Text) - CDbl(Me.txt_Vincita_CINQUINA.Text)
                If v = 0 Then v = 0.5
                Me.txt_Vincita_TOMBOLA.Text = v.ToString
                Me.txt_Vincita_TOMBOLINO.Text = 0
                tot += v
            End If
        End If

        Me.lbl_VerificaAmmontare.Text = tot

        CalcoloInCorso = False

    End Sub
    Private Sub rb_Piano1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano1.CheckedChanged
        Me.gr_Piano1.Enabled = True
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()
    End Sub

    Private Sub rb_Piano2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano2.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = True
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()
    End Sub

    Private Sub rb_Piano3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano3.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = True
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()
    End Sub


    Private Sub lbl_Ammontare_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Ammontare.TextChanged
        CalcolaRipartoVincite()
    End Sub

    Private Sub rb_Piano5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano5.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = True
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()

    End Sub

    Private Sub rb_Piano4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano4.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = True
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()
    End Sub

    Private Sub rb_Piano6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano6.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = True
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()

    End Sub

    Private Sub rb_Piano7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano7.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = True
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()

    End Sub

    Private Sub rb_Piano8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano8.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = True
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()

    End Sub

    Private Sub rb_Piano9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano9.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = True
        Me.gr_Piano10.Enabled = False
        CalcolaRipartoVincite()

    End Sub

    Private Sub rb_Piano10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Piano10.CheckedChanged
        Me.gr_Piano1.Enabled = False
        Me.gr_Piano2.Enabled = False
        Me.gr_Piano3.Enabled = False
        Me.gr_Piano4.Enabled = False
        Me.gr_Piano5.Enabled = False
        Me.gr_Piano6.Enabled = False
        Me.gr_Piano7.Enabled = False
        Me.gr_Piano8.Enabled = False
        Me.gr_Piano9.Enabled = False
        Me.gr_Piano10.Enabled = True
        CalcolaRipartoVincite()

    End Sub

    Private Sub txt_Vincita_AMBO_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Vincita_AMBO.TextChanged
        VerificaDistribuzioneManuale()
    End Sub
    Private Sub VerificaDistribuzioneManuale()
        If CalcoloInCorso Then Exit Sub

        Dim totale As Double = 0
        If (Me.txt_Vincita_AMBO.Text.Trim.Length > 0) Then
            totale += CDbl(Me.txt_Vincita_AMBO.Text.Trim)
        End If
        If (Me.txt_Vincita_TERNO.Text.Trim.Length > 0) Then
            totale += CDbl(Me.txt_Vincita_TERNO.Text.Trim)
        End If
        If (Me.txt_Vincita_QUATERNA.Text.Trim.Length > 0) Then
            totale += CDbl(Me.txt_Vincita_QUATERNA.Text.Trim)
        End If
        If (Me.txt_Vincita_CINQUINA.Text.Trim.Length > 0) Then
            totale += CDbl(Me.txt_Vincita_CINQUINA.Text.Trim)
        End If
        If (Me.txt_Vincita_TOMBOLA.Text.Trim.Length > 0) Then
            totale += CDbl(Me.txt_Vincita_TOMBOLA.Text.Trim)
        End If
        If Tombolino Then
            If (Me.txt_Vincita_TOMBOLINO.Text.Trim.Length > 0) Then
                totale += CDbl(Me.txt_Vincita_TOMBOLINO.Text.Trim)
            End If
        End If
        Me.lbl_VerificaAmmontare.Text = totale

        If totale > Ammontare Then
            MsgBox("La somma delle vincite è superiore all'ammontare di questa partita")
        ElseIf totale < Ammontare Then
            MsgBox("La somma delle vincite è inferiore all'ammontare di questa partita")
        End If

    End Sub


    Private Sub cmd_ConfermaImpostazioni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_ConfermaImpostazioni.Click
        Dim t As TabPage
        Dim presente As Boolean = False
        For Each t In Me.TabControl1.TabPages
            If t.Name = "TabTombola" Then presente = True
        Next
        If Not presente Then Me.TabControl1.TabPages.Add(Tab_Tombola)

        presente = False
        For Each t In Me.TabControl1.TabPages
            If t.Name = "TabRisultati" Then presente = True
        Next
        If Not presente Then Me.TabControl1.TabPages.Add(Tab_Risultati)

        ScriviEvento(NumeroPartita, "", "*** Inizio nuova partita " & Now, "")

        Dim itm As ListViewItem
        Dim Giocatore
        Dim NumeroCartelle
        Dim CostoCartelle As Double
        For Each itm In Me.lv_Cartelle.Items
            Giocatore = itm.Text
            NumeroCartelle = itm.SubItems(1).Text
            CostoCartelle = 0 - CDbl(itm.SubItems(2).Text)
            ScriviEvento(NumeroPartita, Giocatore, "Acquisto cartelle N. " & NumeroCartelle, CostoCartelle)
            AggiornaSituazioneGiocatore(Giocatore, CostoCartelle)
        Next

        Me.TabControl1.SelectTab(Tab_Tombola)

    End Sub
    Private Sub SetPausa()
        If Me.rb_Automatico.Checked Then
            Me.cmd_Pausa.Text = STR_INIZIA
            ClickBottone()
        End If
    End Sub
    Private Sub Ferma()
        If Me.rb_Automatico.Checked Then
            Me.Timer1.Enabled = False
            InPausa = True
            Me.cmd_Pausa.Text = STR_INIZIA
            Me.Text = "Tombola - PAUSA"
        End If
    End Sub
    Private Sub cmd_Dichiara_AMBO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_AMBO.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "AMBO"
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaAmbo(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing
    End Sub
    Private Sub AssegnaAmbo(ByVal giocatore As String)
        AssegnaVincita(giocatore, "AMBO", Me.txt_Vincita_AMBO.Text)
        Me.cmd_Dichiara_AMBO.Enabled = False
        Me.lbl_vincitore_ambo.Text = giocatore
        Me.AssegnatoAMBO = True
        Me.cmd_Dichiara_AMBO.BackColor = Color.LightGray
        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("AMBO assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))

    End Sub

    Private Sub cmd_Dichiara_TERNO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_TERNO.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "TERNO"
        vg.ShowDialog()
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaTerno(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing
    End Sub
    Private Sub AssegnaTerno(ByVal giocatore As String)
        AssegnaVincita(giocatore, "TERNO", Me.txt_Vincita_TERNO.Text)
        Me.cmd_Dichiara_TERNO.Enabled = False
        Me.lbl_vincitore_terno.Text = giocatore
        Me.AssegnatoTERNO = True
        Me.cmd_Dichiara_TERNO.BackColor = Color.LightGray

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("TERNO assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))
    End Sub
    Private Sub cmd_Dichiara_QUANTERNA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_QUATERNA.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "QUATERNA"
        vg.ShowDialog()
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaQuaterna(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing
    End Sub
    Private Sub AssegnaQuaterna(ByVal giocatore As String)
        AssegnaVincita(giocatore, "QUANTERNA", Me.txt_Vincita_QUATERNA.Text)
        Me.cmd_Dichiara_QUATERNA.Enabled = False
        Me.lbl_vincitore_quaterna.Text = giocatore
        Me.AssegnatoQUATERNA = True
        Me.cmd_Dichiara_QUATERNA.BackColor = Color.LightGray

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("QUATERNA assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))
    End Sub
    Private Sub cmd_Dichiara_CINQUINA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_CINQUINA.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "CINQUINA"
        vg.ShowDialog()
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaCinquina(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing
    End Sub
    Private Sub AssegnaCinquina(ByVal giocatore As String)
        AssegnaVincita(giocatore, "CINQUINA", Me.txt_Vincita_CINQUINA.Text)
        Me.cmd_Dichiara_CINQUINA.Enabled = False
        Me.lbl_vincitore_cinquina.Text = giocatore
        Me.AssegnatoCINQUINA = True
        Me.cmd_Dichiara_CINQUINA.BackColor = Color.LightGray

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("CINQUINA assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))
    End Sub
    Private Sub cmd_Dichiara_TOMBOLA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_TOMBOLA.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "TOMBOLA"
        vg.ShowDialog()
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaTombola(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing

        If Not Tombolino Then
            PartitaTerminata()
        End If

    End Sub
    Private Sub AssegnaTombola(ByVal giocatore As String)
        AssegnaVincita(giocatore, "TOMBOLA", Me.txt_Vincita_TOMBOLA.Text)
        Me.cmd_Dichiara_TOMBOLA.Enabled = False
        Me.lbl_vincitore_tombola.Text = giocatore
        Me.AssegnatoTOMBOLA = True
        Me.cmd_Dichiara_TOMBOLA.BackColor = Color.LightGray

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("TOMBOLA assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))
    End Sub
    Private Sub cmd_Dichiara_TOMBOLINO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Dichiara_TOMBOLINO.Click
        SetPausa()
        Dim vg As New VincitaGiocatore
        vg.lbl_Vincita.Text = "TOMBOLINO"
        vg.ShowDialog()
        If vg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AssegnaTombolino(vg.cb_ElencoGiocatori.SelectedItem)
        End If
        vg.Dispose()
        vg = Nothing

        PartitaTerminata()

    End Sub
    Private Sub AssegnaTombolino(ByVal giocatore As String)
        AssegnaVincita(giocatore, "TOMBOLINO", Me.txt_Vincita_TOMBOLINO.Text)
        Me.cmd_Dichiara_TOMBOLINO.Enabled = False
        Me.lbl_vincitore_tombolino.Text = giocatore
        Me.AssegnatoTOMBOLINO = True
        Me.cmd_Dichiara_TOMBOLINO.BackColor = Color.LightGray

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("TOMBOLINO assegnato al giocatore, " & IIf(giocatore = "_Computer", "computer", giocatore))
    End Sub

    Private Sub PartitaTerminata()
        Me.Timer1.Enabled = False
        Me.cmd_Pausa.Text = STR_RICOMINCIA

        If Me.chk_AbilitaPronuncia.Checked Then Pronuncia("La partita è terminata")

        Me.grp_Giocatori.Enabled = False
        Me.grp_Cartelle.Enabled = False
        Me.grp_PianoVincite.Enabled = False
        Me.cmd_ConfermaImpostazioni.Enabled = False
        Me.grp_AutoManual.Enabled = False


        ScriviEvento(NumeroPartita, "", "*** Fine partita " & Now, "")
        Me.TabControl1.SelectTab(Tab_Risultati)
        MsgBox("Partita terminata")
    End Sub
    Private Sub AssegnaVincita(ByVal Giocatore As String, ByVal Vincita As String, ByVal Premio As String)
        Dim itm As New ListViewItem(Giocatore)
        itm.SubItems.Add(Vincita)
        itm.SubItems.Add(Premio)
        Me.lv_Risultati.Items.Add(itm)

        ScriviEvento(NumeroPartita, Giocatore, "Vincita " & Vincita, Premio)
        AggiornaSituazioneGiocatore(Giocatore, Premio)

    End Sub
    Private Sub ScriviEvento(ByVal NumeroPartita As Integer, ByVal Giocatore As String, ByVal Vincita As String, ByVal Premio As String)
        Dim itm As New ListViewItem(NumeroPartita)
        itm.SubItems.Add(Giocatore)
        itm.SubItems.Add(Premio)
        itm.SubItems.Add(Vincita)
        Me.lv_Eventi.Items.Add(itm)
    End Sub
    Private Sub AggiornaSituazioneGiocatore(ByVal Giocatore As String, ByVal Importo As Double)

        If Me.lv_SituazioneGiocatori.Items.Count = 0 Then
            Dim nuovoitm As New ListViewItem(Giocatore)
            nuovoitm.SubItems.Add(Importo)
            Me.lv_SituazioneGiocatori.Items.Add(nuovoitm)
        Else
            Dim itm As ListViewItem
            Dim trovato As Boolean = False
            For Each itm In Me.lv_SituazioneGiocatori.Items
                If itm.Text = Giocatore Then
                    trovato = True
                    itm.SubItems(1).Text = CDbl(itm.SubItems(1).Text) + Importo
                End If
            Next
            If Not trovato Then
                Dim nuovoitm As New ListViewItem(Giocatore)
                nuovoitm.SubItems.Add(Importo)
                Me.lv_SituazioneGiocatori.Items.Add(nuovoitm)
            End If
        End If

    End Sub

    Private Function ControllaVincitaComputer(ByVal vincita As String) As Boolean
        ControllaVincitaComputer = False
        If Not ComputerAbilitato Then Return ControllaVincitaComputer
        Dim n As Integer
        Dim NIniziale As Integer
        Dim NNumeri As Integer
        Select Case vincita.ToUpper
            Case "AMBO"
                NNumeri = 2
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) Or checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) Or checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True
                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next

            Case "TERNO"
                NNumeri = 3
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) Or checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) Or checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True
                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next

            Case "QUATERNA"
                NNumeri = 4
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) Or checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) Or checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True
                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next

            Case "CINQUINA"
                NNumeri = 5
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) Or checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) Or checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True
                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next

            Case "TOMBOLA"
                NNumeri = 5
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) And checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) And checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True

                        EliminaNumeroDaArray(ArrayCartelleComputer, n + 1)

                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next

            Case "TOMBOLINO"
                NNumeri = 5
                For n = 0 To ArrayCartelleComputer.Length - 1
                    NIniziale = getNumeroIniziale(ArrayCartelleComputer(n))
                    If checkNumeri(NIniziale, NIniziale + 4, NNumeri) And checkNumeri(NIniziale + 15, NIniziale + 19, NNumeri) And checkNumeri(NIniziale + 30, NIniziale + 34, NNumeri) Then
                        ControllaVincitaComputer = True
                        Exit For
                    Else
                        ControllaVincitaComputer = False
                    End If
                Next
        End Select

        Return ControllaVincitaComputer

    End Function
    Private Function getNumeroIniziale(ByVal cartella As Integer) As Integer
        Select Case cartella
            Case 1
                getNumeroIniziale = 1
            Case 2
                getNumeroIniziale = 6
            Case 3
                getNumeroIniziale = 11
            Case 4
                getNumeroIniziale = 46
            Case 5
                getNumeroIniziale = 51
            Case 6
                getNumeroIniziale = 56
        End Select
    End Function
    Private Function checkNumeri(ByVal inizio As Integer, ByVal fine As Integer, ByVal n As Integer) As Boolean
        checkNumeri = False

        Dim Num As Integer = 0
        Dim i
        Dim c As Control
        For i = inizio To fine
            For Each c In Me.grp_Tavola.Controls
                If c.Text = i Then
                    'If c.BackColor = Color.Green Then
                    If c.BackColor <> Color.White Then
                        Num += 1
                    End If
                End If
            Next
        Next
        If Num >= n Then checkNumeri = True
        Return checkNumeri
    End Function

    Private Sub chk_PronunciaCifre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_PronunciaCifre.CheckedChanged
        PronunciaCifre = Me.chk_PronunciaCifre.Checked
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        EvidenziaNumero(NumeroEstratto, Me.ColoreNumeroEstratto)
        Timer2.Enabled = False
        Me.cmd_Pausa.Enabled = True
    End Sub

    Private Sub TabControl1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.Click
        If Me.TabControl1.SelectedTab.Name <> "Tombola" Then
            Ferma()
        End If
    End Sub

End Class
