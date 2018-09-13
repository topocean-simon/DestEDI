Imports MySql
Imports System.Xml

Public Class Cls11A_XML

    Shared ediType As String = "11A"

    Sub export11A_XML()

        Dim sql As String = ""
        Dim cn As String = ""
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer

        Dim common As New common

        Dim BkhRefId As Integer = 0
        Dim BkhBLNo As String = ""
        Dim BrhSName As String = ""
        Dim BrhCd As Integer = 0
        Dim EDIStr As String = ""
        Dim _EDIStr As String = ""
        Dim Content As String = ""
        Dim fullpath As String = ""

        Dim sqlConn As Object
        Dim sda As Object
        Dim cmd As Object
        Dim ds, ds2, ds3, ds4 As New DataSet

        Dim filename As String = ""

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Generate 11A XML list", 1)

        If My.Settings.DBType = 0 Then
            ' MySQL
            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
            sql = "select a.BkhRefId, a.BkhBLNo, b.BrhSName, b.BrhCd from BookingInfo a left outer join Branch b on a.BkhBrhCd = b.BrhCd WHERE a.Is11A = 1 AND IFNULL(BkhBLNo, '') <> '';"
            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
        Else
            ' SQL Server
            sqlConn = New SqlClient.SqlConnection(cn)
            sql = "SELECT a.BkhRefId, a.BkhBLNo, b.BrhSName, b.BrhCd FROM BookingHdr a left outer join Branch b on a.BkhBrhCd = b.BrhCd WHERE a.Is11A = 1 AND ISNULL(BkhBLNo, '') <> ''"
            cmd = New SqlClient.SqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New SqlClient.SqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
        End If

        If ds.Tables(0).Rows.Count > 0 Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Try
                    With ds.Tables(0).Rows(i)
                        BkhRefId = .Item("BkhRefId").ToString
                        BkhBLNo = .Item("BkhBLNo").ToString
                        BrhSName = .Item("BrhSName").ToString
                        BrhCd = .Item("BrhCd").ToString

                        If My.Settings.DBType = 0 Then
                            sql = "CALL usp_Gen11DXML_TEXT1('" & BkhRefId & "');"

                            cmd = Nothing
                            sda = Nothing

                            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                        Else
                            sql = "EXEC dbo.usp_Gen11DXML_TEXT1 '" & BkhRefId & "'"

                            cmd = New SqlClient.SqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New SqlClient.SqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                        End If

                        If My.Settings.DBType = 0 Then
                            sql = "CALL usp_11A_XML_Export_List('" & BkhRefId & "','" & BrhCd & "' , '', 1, '');"

                            cmd = Nothing
                            sda = Nothing

                            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                            ds4.Clear()
                            sda.Fill(ds4)
                            ds4.Clear()
                        Else
                            sql = "EXEC usp_11A_XML_Export_List " & BkhRefId & "," & BrhCd & ",'', 1, ''"

                            cmd = New SqlClient.SqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New SqlClient.SqlDataAdapter(cmd)
                            ds4.Clear()
                            sda.Fill(ds4)
                            ds4.Clear()
                        End If

                        Content = ""

                        If ds2.Tables(0).Rows.Count > 0 Then
                            _EDIStr = "<EDI>"

                            'For j = 0 To ds2.Tables(0).Rows.Count - 1

                            '_EDIStr &= "<Booking>"
                            'With ds2.Tables(0).Rows(j)

                            'For k = 0 To ds2.Tables(0).Columns.Count - 1

                            '_EDIStr &= "<" & ds2.Tables(0).Columns(k).ToString().Trim() & ">" & common.replaceXMLChar(.Item(ds2.Tables(0).Columns(k).ToString().Trim()).ToString) & "</" & ds2.Tables(0).Columns(k).ToString().Trim() & ">"
                            'Next

                            'End With
                            '_EDIStr &= "</Booking>"

                            'Next

                            Content = common.NullVal(ds2.Tables(0).Rows(0).Item("EDIStr"), "")

                            _EDIStr &= ds2.Tables(0).Rows(0).Item("EDIStr")

                            _EDIStr &= "</EDI>"

                            Dim xmlDoc As New XmlDocument()

                            xmlDoc.LoadXml(_EDIStr)

                            Dim settings As New XmlWriterSettings()

                            settings.Indent = True
                            filename = BrhSName & "_" & BkhBLNo & "_" & DateTime.Now.ToString("yyyyMMddHHss") & ".xml"

                            If Not System.IO.Directory.Exists(My.Settings.ExportPath11A) Then
                                System.IO.Directory.CreateDirectory(My.Settings.ExportPath11A)
                            End If

                            If System.IO.File.Exists(My.Settings.ExportPath11A & filename) Then
                                System.IO.File.Delete(My.Settings.ExportPath11A & filename)
                            End If

                            fullpath = My.Settings.ExportPath11A & filename

                            If My.Settings.DBType = 0 Then
                                sql = "CALL usp_11A_XML_Export_List('" & BkhRefId & "','" & BrhCd & "' , '" & fullpath.Replace("\", "\\") & "', 2, '');"

                                cmd = Nothing
                                sda = Nothing

                                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            Else
                                sql = "EXEC usp_11A_XML_Export_List " & BkhRefId & "," & BrhCd & ",'" & fullpath & "', 2, ''"

                                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New SqlClient.SqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            End If

                            If Content <> "" Then
                                Dim writer As XmlWriter = XmlWriter.Create(fullpath, settings)
                                xmlDoc.Save(writer)
                            End If

                            common.showScreenMsg("Export 11A XML Success", 1)

                            If My.Settings.DBType = 0 Then
                                sql = "CALL usp_11A_XML_Export_List('" & BkhRefId & "','" & BrhCd & "' , '', 0, '');"

                                cmd = Nothing
                                sda = Nothing

                                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            Else
                                sql = "EXEC usp_11A_XML_Export_List " & BkhRefId & "," & BrhCd & ",'', 0, ''"

                                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New SqlClient.SqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            End If

                        Else
                            'No record export, just set the export flag to zero
                            If My.Settings.DBType = 0 Then
                                sql = "CALL usp_11A_XML_Export_List('" & BkhRefId & "','" & BrhCd & "' , '', 0, '');"

                                cmd = Nothing
                                sda = Nothing

                                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            Else
                                sql = "EXEC usp_11A_XML_Export_List " & BkhRefId & "," & BrhCd & ",'', 0, ''"

                                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                                cmd.CommandTimeout = My.Settings.Timeout
                                sda = New SqlClient.SqlDataAdapter(cmd)
                                ds4.Clear()
                                sda.Fill(ds4)
                                ds4.Clear()
                            End If
                        End If

                        ds2.Clear()
                        sda.Dispose()
                        'sqlConn.Dispose()
                        '    End If

                        ' Return message to screen
                        common.showScreenMsg("11A XML exported successfully (bkhRefId: " & BkhRefId & ")")
                    End With
                Catch ex As Exception
                    common.showScreenMsg("Error found in exporting 11A XML (BkhRefId: " & BkhRefId & ")")
                    common.SaveLog("Error found in exporting 11A XML (BkhRefId: " & BkhRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")

                    If My.Settings.DBType = 0 Then
                        sql = "CALL usp_11A_XML_Export_List('" & BkhRefId & "','" & BrhCd & "' , '', 3, '" & ex.Message.ToString.Replace("'", "") & "');"

                        cmd = Nothing
                        sda = Nothing

                        cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                        cmd.CommandTimeout = My.Settings.Timeout
                        sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                        ds4.Clear()
                        sda.Fill(ds4)
                        ds4.Clear()
                    Else
                        sql = "EXEC usp_11A_XML_Export_List " & BkhRefId & "," & BrhCd & ",'', 3, '" & ex.Message.ToString.Replace("'", "") & "'"

                        cmd = New SqlClient.SqlCommand(sql, sqlConn)
                        cmd.CommandTimeout = My.Settings.Timeout
                        sda = New SqlClient.SqlDataAdapter(cmd)
                        ds4.Clear()
                        sda.Fill(ds4)
                        ds4.Clear()
                    End If
                End Try
            Next
        Else
            common.showScreenMsg("No info. for 11A XML", 1)
        End If

        ' Remove Variables
        sql = Nothing
        cn = Nothing
        i = Nothing
        common = Nothing
        BkhRefId = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        ds2 = Nothing
        ds3 = Nothing
        ds4 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

End Class