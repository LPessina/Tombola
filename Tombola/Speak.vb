Module Speak
    Public Sub Pronuncia(ByVal frase As String)
        Dim sp As New SpeechLib.SpVoice
        sp.Rate -= 3
        sp.Voice = sp.GetVoices.Item(0)
        sp.Volume = 100
        sp.Speak(frase, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
    End Sub
    Public Sub PronunciaNumero(ByVal numero As Integer)
        Dim sp As New SpeechLib.SpVoice
        sp.Rate -= 3
        sp.Voice = sp.GetVoices.Item(0)
        sp.Volume = 100
        sp.Speak(numero.ToString, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)

        If PronunciaCifre Then
            If numero > 10 Then
                sp.Speak(Mid(numero.ToString, 1, 1) & ", " & Mid(numero.ToString, 2, 1) & ".", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            End If
        End If

        If Not Form1.chk_AbilitaSmorfia.Checked Then GoTo uscita

        ' Smorfia: pronuncia le sentenze della Smorfia
        Select Case numero
            Case 1
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 2
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 3
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 4
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 5
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 6
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 7
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 8
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 9
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 10
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 11
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 12
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 13
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 14
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 15
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 16
                sp.Speak("'o ricchione", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 17
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 18
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 19
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 20
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 21
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 22
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 23
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 24
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 25
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 26
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 27
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)
            Case 28
                sp.Speak("", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)

            Case 71
                sp.Speak("L'om 'e merda!", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)

            Case 77
                sp.Speak("Le gambe delle femmenette!", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)

            Case 90
                sp.Speak("La paura!", SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault)

        End Select

uscita:
        sp = Nothing

    End Sub

End Module
