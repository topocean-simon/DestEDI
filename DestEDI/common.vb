Imports System.IO

Public Class common
    Sub showScreenMsg(ByVal msg As String, Optional ByVal IsSaveLog As Integer = 0, Optional ByVal LogType As String = "I")
        frmMain.lstDisplay.Items.Insert(0, Format(Now, "dd.MM.yyyy HH:mm:ss") & " - " & msg)
        frmMain.lstDisplay.Refresh()

        If IsSaveLog = 1 Then
            Me.SaveLog(msg, LogType)
        End If

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub SaveLog(ByVal msg As String, Optional ByVal type As String = "I")
        Try
            Dim logPath As String = My.Settings.LogPath
            Dim MsgHdr As String = ""
            Dim msgArray() As String

            ' Check Log Path Existance
            If Not My.Computer.FileSystem.DirectoryExists(logPath) Then
                My.Computer.FileSystem.CreateDirectory(logPath)
            End If

            Dim fso As New StreamWriter(logPath & Format(Now, "yyyy.MM.dd") & ".txt", True)

            If type = "I" Then
                MsgHdr = "Message:"
            Else
                MsgHdr = "Error:"
            End If

            msgArray = Split(msg, Chr(13))

            fso.WriteLine("=============================================================================================")
            fso.WriteLine("Date: " & Format(Now, "dd/MM/yyyy HH:mm:ss"))
            fso.WriteLine()
            fso.WriteLine(MsgHdr)
            fso.WriteLine()

            For Each msgBlock As String In msgArray
                fso.WriteLine(msgBlock)
            Next

            fso.WriteLine("=============================================================================================")
            fso.WriteLine()

            ' Close File Object
            fso.Close()

            MsgHdr = Nothing
            fso.Dispose()
            fso = Nothing
            logPath = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch ex As Exception
            Me.showScreenMsg("Save Log errors captured, please review the error log.")
        End Try
    End Sub

    Function NullVal(ByVal inValue As Object, ByVal replacement As String) As String
        Dim tmp As String = ""

        If IsNothing(inValue) Then
            tmp = replacement
        ElseIf IsDBNull(inValue) Then
            tmp = replacement
        ElseIf CStr(inValue) = "" Then
            tmp = replacement
        Else
            tmp = inValue
        End If

        ' Return Value
        NullVal = tmp

        ' Destroy Variables
        tmp = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function

    Function setQuote(ByVal inVal As String) As String
        setQuote = Replace(inVal, "'", "''")
    End Function

    Function ConvertDte(ByVal inVal As String) As String
        Dim tmp As String = ""

        If Me.NullVal(inVal, "") = "" Then
            tmp = ""
        Else
            If inVal.Length > 8 Then
                tmp &= Mid(inVal, 1, 4)
                tmp &= "-" & Mid(inVal, 5, 2)
                tmp &= "-" & Mid(inVal, 7, 2)
                tmp &= " " & Mid(inVal, 9, 2)
                tmp &= ":" & Mid(inVal, 11, 2)
                tmp &= ":" & Mid(inVal, 13, 2)
            Else
                tmp &= Mid(inVal, 1, 4)
                tmp &= "-" & Mid(inVal, 5, 2)
                tmp &= "-" & Mid(inVal, 7, 2)
            End If
        End If

        ConvertDte = tmp

        ' Destroy Variables
        tmp = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function

    Function recoverXMLChar(ByVal inVal As String) As String
        Dim tmp As String = inVal

        tmp = Replace(tmp, "&amp;", "&")
        tmp = Replace(tmp, "&apos;", "'")
        tmp = Replace(tmp, "¡¦", "'")
        tmp = Replace(tmp, "&lt;", "<")
        tmp = Replace(tmp, "&gt;", ">")
        tmp = Replace(tmp, "&quot;", """")
        tmp = Replace(tmp, "¡V", "-")
        tmp = Replace(tmp, "¡¦", "'")

        recoverXMLChar = tmp

        ' Destroy Variables
        tmp = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function
End Class
