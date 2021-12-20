Imports System
Imports System.IO
Imports System.Text
Module Module1
    Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringW" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
    Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32

    Public PronunciaCifre As Boolean = False

    Function LoadFromIni(ByVal Sezione As String, ByVal Parametro As String, ByVal IniFile As String) As Object
        On Error Resume Next

        Dim sDestination As New String(" ", 1024)
        Dim lReturnVal As Long
        Dim sMsg As String
        LoadFromIni = ""
        lReturnVal = GetPrivateProfileString(Sezione, Parametro, "", sDestination, Len(sDestination), IniFile)
        'lReturnVal will always equal the length of the returned string not including vbNullChars at the end
        If lReturnVal <> 0 Then
            sDestination = Left(sDestination, InStr(sDestination, Chr(0)) - 1) 'chr(0)=vbNullChar
            LoadFromIni = Trim(sDestination)
        End If
    End Function
    Function WriteToIni(ByVal Sezione As String, ByVal Parametro As String, ByVal Valore As String, ByVal IniFile As String) As Boolean
        On Error Resume Next

        'Dim sDestination As New String(" ", 255)
        Dim lReturnVal As Long
        Dim sMsg As String

        On Error Resume Next

        lReturnVal = WritePrivateProfileString(Sezione, Parametro, Valore, IniFile)
        If lReturnVal = 0 Then
            Err.Raise(vbObjectError + 513 + 1001, "WriteToINI", "Initialization File Error!")
            WriteToIni = False
        End If
        WriteToIni = True

    End Function

    Public Sub EliminaNumeroDaArray(ByRef input() As String, ByVal numero As Integer)
        Dim i As Integer
        Dim temp As String = ""
        For i = 0 To input.Length - 1
            If input(i) <> numero Then
                temp &= IIf(temp.Length > 0, ",", "") & input(i)
            End If
        Next
        input = Split(temp, ",")
    End Sub


End Module
