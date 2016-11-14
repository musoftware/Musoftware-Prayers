
Option Explicit On
Public Class clsMultimedia

    Private sAlias As String

    Private sFilename As String
    Private nLength As Single

    Private nPosition As Single
    Private sStatus As Single
    Private bWait As Boolean

    Declare Function mciSendString Lib "winmm.dll" Alias _
"mciSendStringA" (ByVal lpstrCommand As String, ByVal _
lpstrReturnString As String, ByVal uReturnLength As Long, _
ByVal hwndCallback As Long) As Long



    Public Sub mmOpen(ByVal sTheFile As String)
        Dim nReturn As Long
        Dim sType As String
        ' Dim sAlias As String = ""
        If sAlias <> "" Then
            mmClose()
            'ElseIf sAlias = "" Then
            '    Randomize()

            '    sAlias = "MP3" & Minute(Now) & Second(Now) & Int(1000 * Rnd() + 1)
        End If
        Select Case UCase$(Right$(sTheFile, 3))
            Case "WAV"
                sType = "Waveaudio"
            Case "AVI"
                sType = "AviVideo"
            Case "MID"
                sType = "Sequencer"
            Case "MP3"
                sType = "MPegVideo"
            Case Else
                Exit Sub
        End Select

        sAlias = Right$(sTheFile, 3) & Minute(Now) & Second(Now) & Int(1000 * Rnd() + 1)

        If CBool(InStr(sTheFile, " ")) Then sTheFile = Chr(34) & sTheFile & Chr(34)
        nReturn = mciSendString("Open " & sTheFile & " ALIAS " & sAlias & " TYPE " & sType & " wait", "", 0, 0)
        sFilename = sTheFile
    End Sub
    Dim Pcommand As String, [error] As Long

    Dim lvol As Integer
    Public Function SetVolume(volume As Integer) As Boolean
 
        lvol = volume

        If volume >= 0 AndAlso volume <= 1000 Then
            Pcommand = "setaudio " & sAlias & " volume to " & volume.ToString()
            [error] = mciSendString(Pcommand, "", 0, 0)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function SetBalance(balance As Integer) As Boolean

        If balance >= 0 AndAlso balance <= 1000 Then
            Pcommand = "setaudio " & sAlias & " left  volume to " & (1000 - (balance - lvol)).ToString()
            [error] = mciSendString(Pcommand, Nothing, 0, CLng(IntPtr.Zero))
            Pcommand = "setaudio " & sAlias & " right  volume to " & (balance - lvol).ToString()
            [error] = mciSendString(Pcommand, Nothing, 0, CLng(IntPtr.Zero))
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub mmClose()
        Dim nReturn As Long

        If sAlias = "" Then Exit Sub

        nReturn = mciSendString("Close " & sAlias, "", 0, 0)
        sAlias = ""
        sFilename = ""
    End Sub

    Public Sub mmPause()
        Dim nReturn As Long

        If sAlias = "" Then Exit Sub

        nReturn = mciSendString("Pause " & sAlias, "", 0, 0)
    End Sub

    Public Sub mmPlay()
        Dim nReturn As Long

        If sAlias = "" Then Exit Sub

        If bWait Then
            nReturn = mciSendString("Play " & sAlias & " wait", "", 0, 0)
        Else
            nReturn = mciSendString("Play " & sAlias, "", 0, 0)
        End If
    End Sub

    Public Sub mmStop()
        Dim nReturn As Long

        If sAlias = "" Then Exit Sub

        nReturn = mciSendString("Stop " & sAlias, "", 0, 0)
    End Sub

    Public Sub mmSeek(ByVal nPosition As Single)
        Dim nReturn As Long

        nReturn = mciSendString("seek " & sAlias & " to " & nPosition, "", 0, 0)
    End Sub
    Dim sVolume As String
    Property Volume() As String
        Get
            Return sVolume
        End Get
        Set(ByVal Value As String)
            sVolume = Value
            SetVolume(CInt(Value))
        End Set
    End Property

    Property FileName() As String
        Get
            FileName = sFilename
        End Get
        Set(ByVal Value As String)
            mmOpen(Value)
        End Set
    End Property


    Property Wait() As String
        Get
            Wait = CStr(bWait)
        End Get
        Set(ByVal Value As String)
            bWait = CBool(Value)
        End Set
    End Property

    Property Length() As Single
        Get
            Dim nReturn As Long, nLength As Integer
            Dim sLength As String = ""

            If sAlias = "" Then
                Length = 0
                Exit Property
            End If

            nReturn = mciSendString("Status " & sAlias & " length", sLength, 255, 0)
            nLength = InStr(sLength, Chr(0))
            Length = CSng(Val(Left$(sLength, nLength - 1)))
        End Get
        Set(ByVal Value As Single)
            mmSeek(Value)
        End Set
    End Property

    Property Position() As Single
        Get
            Dim nReturn As Long, nLength As Long

            Dim sPosition As String = New String(Chr(0), 255)

            If sAlias = "" Then Exit Property

            nReturn = mciSendString("Status " & sAlias & " position", sPosition, 255, 0)
            nLength = InStr(sPosition, Chr(0))
            Position = CSng(Val(Left$(sPosition, CInt(nLength - 1))))
        End Get
        Set(ByVal Value As Single)
            ' Value = 1
        End Set
    End Property

    Property Status() As String
        Get
            Dim nReturn As Long, nLength As Long

            Dim sStatus As String = New String(Chr(0), 255)

            If sAlias = "" Then Status = "" : Exit Property

            nReturn = mciSendString("Status " & sAlias & " mode", sStatus, 255, 0)

            nLength = InStr(sStatus, Chr(0))
            Status = Left$(sStatus, CInt(nLength - 1))
        End Get
        Set(ByVal Value As String)

        End Set
    End Property
    '  Dim 


End Class
