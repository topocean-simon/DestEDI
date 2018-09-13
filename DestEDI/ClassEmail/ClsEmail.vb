Imports System.Net
Imports System.Net.Mail
Imports System.Data

Public Class ClsEmail

    Sub sendAckEmail(ByVal ediType As String, ByVal emailMsg As String, ByVal mailType As Integer)
        ' ======================================================================================
        ' Definition of "mailType"
        ' ======================================================================================
        ' 1 - Notice Email
        ' 2 - Error Email
        ' ======================================================================================


        ' ======================================================================================
        ' Declare Variables
        ' ======================================================================================

        Dim common As New common
        Dim emailContent As String = ""
        Dim smtp As SmtpClient
        Dim mailBody As String = ""
        Dim mailTo As String = My.Settings.TechSupport
        Dim mailCC As String = ""
        Dim mailFrom As String = "titan.edi@topocean.com.hk"
        Dim mailSubject As String = ""
        Dim mailMsg As New MailMessage
        Dim ediProcess As String = ""
        Dim originEmail As String = "it@topocean.com.hk"
        Dim remarkArray As String()

        Dim cn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Origin_FTP.accdb;Persist Security Info=False;"

        Dim sql As String = ""
        Dim sqlconn As New OleDb.OleDbConnection(cn)
        Dim sda As OleDb.OleDbDataAdapter
        Dim cmd As New OleDb.OleDbCommand
        Dim ds As New DataSet

        ' ======================================================================================
        ' End of Declare Variables
        ' ======================================================================================


        Try
            If common.NullVal(mailTo, "") <> "" And common.NullVal(mailFrom, "") <> "" Then
                ' Return message to screen
                common.showScreenMsg("Sending email acknowledgement...")

                ' Save Log
                common.SaveLog("Sending email acknowledgement..." & Chr(13) & "Email: " & mailTo)

                Select Case mailType
                    Case 1
                        ' ======================================================================================
                        ' EDI Complete Notice
                        ' ======================================================================================

                        ' Email Subject
                        mailSubject = "PROCESSED - " & ediType & " EDI Notice (Server ID: " & UCase(My.Settings.ServerID) & ")"

                        ' Email Content
                        mailBody &= "<span style=""font-family: verdana; font-size: 12px;"">" & _
                            "Dear Support Team,<br /><br />" & Chr(13) & _
                            "Please be informed that there is a/an " & ediType & " EDI processed.<br /><br />" & Chr(13) & _
                            "<table cellpadding=""5"" cellspacing=""0"" border=""1"">" & Chr(13)

                        ' Report Detail
                        mailBody &= "<tr bgcolor=""#FFD9B4""><td colspan=""2"" style=""font-family: verdana; font-size: 12px;""><b>EDI Process Result</b></td></tr>" & Chr(13) & _
                            "<tr><td style=""font-family: verdana; font-size: 12px;"">Message&nbsp;</td>" & Chr(13) & _
                            "<td style=""font-family: verdana; font-size: 12px;"">" & emailMsg & "</td></tr>" & Chr(13)

                        mailBody &= "</table><br />" & Chr(13)

                        ' Mail Footer
                        mailBody &= "For any inquires, please send email to <a href=""mailto:" & originEmail & """>" & originEmail & "</a>.<br /><br />" & Chr(13) & _
                            "Thanks,<br />" & Chr(13) & _
                            "Offshore EDI Service<br /><br />" & Chr(13) & _
                            "<hr />" & Chr(13) & _
                            "<font color=""red""><b>This is an automated system email, please DO NOT reply.</b></font>" & Chr(13) & _
                            "</span>"

                        ' ======================================================================================
                        ' End of EDI Complete Notice
                        ' ======================================================================================

                    Case 2
                        ' ===================================================================================
                        ' Error Message if error captured
                        ' ===================================================================================

                        ' Email Subject
                        mailSubject = "ERROR - " & ediType & " EDI Notice (Server ID: " & UCase(My.Settings.ServerID) & ")"

                        ' Email Content
                        mailBody &= "<span style=""font-family: verdana; font-size: 12px;"">" & _
                            "Dear HKG IT Team,<br /><br />" & Chr(13) & _
                            "Please be informed that errors are captured when processing a/an " & ediType & " EDI.<br /><br />" & Chr(13) & _
                            "<table cellpadding=""5"" cellspacing=""0"" border=""1"">" & Chr(13)

                        ' Report Detail
                        mailBody &= "<tr bgcolor=""#FFD9B4""><td colspan=""2"" style=""font-family: verdana; font-size: 12px;""><b>EDI Process Result</b></td></tr>" & Chr(13) & _
                            "<tr><td style=""font-family: verdana; font-size: 12px;"">EDI Type&nbsp;</td>" & Chr(13) & _
                            "<td style=""font-family: verdana; font-size: 12px; color: Navy;""><b>" & ediType & "</b></td></tr>" & Chr(13) & _
                            "<tr><td style=""font-family: verdana; font-size: 12px; vertical-align: top; padding-top: 5px;"">Error Message&nbsp;</td>" & Chr(13) & _
                            "<td style=""font-family: verdana; font-size: 12px; color: Red; vertical-align: top; padding-top: 5px;""><b>" & emailMsg & "</b></td></tr>" & Chr(13)

                        mailBody &= "</table><br />" & Chr(13)

                        ' Mail Footer
                        mailBody &= "Thanks,<br />" & Chr(13) & _
                            "Offshore EDI Service<br /><br />" & Chr(13) & _
                            "<hr />" & Chr(13) & _
                            "<font color=""red""><b>This is an automated system email, please DO NOT reply.</b></font>" & Chr(13) & _
                            "</span>"

                        ' ===================================================================================
                        ' End of Error Message if error captured
                        ' ===================================================================================

                End Select


                mailMsg.From = New Mail.MailAddress(mailFrom, "Titan EDI Service")
                mailMsg.To.Add(mailTo)

                ' CC Copy
                If mailCC <> "" Then
                    mailMsg.CC.Add(mailCC)
                End If

                mailMsg.Subject = mailSubject
                mailMsg.Body = mailBody
                mailMsg.IsBodyHtml = True

                ' Send email
                smtp = New SmtpClient
                smtp.Host = My.Settings.SMTP
                smtp.Port = 25
                smtp.UseDefaultCredentials = True
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network
                smtp.Send(mailMsg)

                ' Update Screen
                common.showScreenMsg("Email acknowledgement sent successfully.", 1)
                
                ' Save Log
                common.SaveLog("Email acknowledgement sent successfully..." & Chr(13) & "Email: " & mailTo)

                ' Release Memory
                GC.Collect()
                GC.WaitForPendingFinalizers()
            End If
        Catch ex As Exception
            ' Update Screen
            common.showScreenMsg("Error captured during sending email acknowledgement.", 1, "E")
            
            ' Save Log
            common.SaveLog("Error captured during sending email acknowledgement..." & Chr(13) & ex.Message, "E")
        End Try

        ' Destroy Variables
        common = Nothing
        smtp = Nothing
        mailBody = Nothing
        mailFrom = Nothing
        mailCC = Nothing
        mailTo = Nothing
        mailSubject = Nothing
        smtp = Nothing
        remarkArray = Nothing

        mailMsg.Dispose()
        mailMsg = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub
End Class