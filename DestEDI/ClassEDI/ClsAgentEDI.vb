Imports MySql
Imports System.Xml

Public Class ClsAgentEDI

    Shared ediType As String = "Agent"

    Sub exportAgentEDI_MGF()

        Dim sql As String = ""
        Dim cn As String = ""
        Dim i As Integer
        Dim common As New common

        Dim AelRefId As Integer = 0
        Dim BkhRefId As Integer = 0
        Dim AgtCode As String = ""

        Dim sqlConn As Object
        Dim sda As Object
        Dim cmd As Object
        Dim ds, ds2 As New DataSet

        Dim filename As String = ""

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Generate Agent EDI export list", 1)

        If My.Settings.DBType = 0 Then
            ' MySQL
            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
            sql = "CALL usp_AgentEDI_ExportList();"
            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
        Else
            ' SQL Server
            sqlConn = New SqlClient.SqlConnection(cn)
            sql = "EXEC usp_AgentEDI_ExportList"
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
                        AelRefId = .Item("AelRefId").ToString
                        BkhRefId = .Item("AelBkhRefId").ToString
                        AgtCode = .Item("AelAgtCode").ToString

                        ' Export XML file
                        filename = Me.exportAgentEDI_File_MGF(AelRefId, BkhRefId, AgtCode)

                        ' Return message to screen
                        common.showScreenMsg("Completing Agent EDI (AelRefId: " & AelRefId & ", Filename: " & filename & ")")

                        If My.Settings.DBType = 0 Then
                            sql = "CALL usp_AgentEDI_Complete('" & AelRefId & "', '" & filename & "');"

                            cmd = Nothing
                            sda = Nothing

                            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
                            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        Else
                            sql = "EXEC usp_AgentEDI_Complete '" & AelRefId & "', '" & filename & "'"

                            cmd = Nothing
                            sda = Nothing

                            sqlConn = New SqlClient.SqlConnection(cn)
                            cmd = New SqlClient.SqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New SqlClient.SqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        End If

                        ' Return message to screen
                        common.showScreenMsg("Agent EDI exported successfully (AelRefId: " & AelRefId & ")")
                    End With
                Catch ex As Exception
                    common.showScreenMsg("Error found in exporting Agent EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")")
                    common.SaveLog("Error found in exporting Agent EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
                End Try
            Next
        Else
            common.showScreenMsg("No shipments for Agent EDI", 1)
        End If

        ' Remove Variables
        sql = Nothing
        cn = Nothing
        i = Nothing
        common = Nothing
        BkhRefId = Nothing
        AgtCode = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        ds2 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Function exportAgentEDI_File_MGF(ByVal AelRefId As Integer, ByVal BkhRefId As Integer, ByVal AgtCode As String) As String
        Dim filename As String = ""
        Dim cn As String = ""
        Dim sql As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim sqlConn As Object
        Dim cmd As Object
        Dim sda As Object
        Dim ds As New DataSet
        Dim drArray() As DataRow
        Dim common As New common
        Dim xWriter As XmlTextWriter
        Dim dataValue As String = ""
        Dim dataSeq As Integer = 0
        Dim dataArray() As String
        Dim dataTag As String = ""
        Dim CfhLn As Integer = 0

        Dim exportPath As String = My.Settings.ExportPath & "AgentEDI\Export\"
        Dim backupPath As String = My.Settings.ExportPath & "AgentEDI\Backup\"

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Retrieving Agent EDI shipment data (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)

        Try
            If My.Settings.DBType = 0 Then
                ' MySQL server
                sql = "CALL usp_AgentEDI_Export('" & BkhRefId & "')"
                sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            Else
                ' SQL server
                sql = "EXEC usp_AgentEDI_Export '" & BkhRefId & "'"
                sqlConn = New SqlClient.SqlConnection(cn)
                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New SqlClient.SqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                ' Return message to screen
                common.showScreenMsg("Exporting Agent EDI XML file", 1)

                filename = ds.Tables(0).Rows(0).Item("ShipmentFrom").ToString & "_" & _
                    ds.Tables(0).Rows(0).Item("ShipmentTo").ToString & "_" & _
                    ds.Tables(0).Rows(0).Item("BLNo").ToString & "_" & _
                    Format(Now, "yyyyMMddHHmmss") & ".xml"

                backupPath &= ds.Tables(0).Rows(0).Item("ShipmentTo").ToString & "\"
                exportPath &= ds.Tables(0).Rows(0).Item("ShipmentTo").ToString & "\"

                ' Create directory if not exist
                If Not My.Computer.FileSystem.DirectoryExists(backupPath) Then
                    My.Computer.FileSystem.CreateDirectory(backupPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(backupPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(backupPath & filename)
                End If

                ' Create XML file
                xWriter = New XmlTextWriter(backupPath & filename, System.Text.Encoding.UTF8)

                ' --------------------------------------------------------
                ' XML Formatting
                ' --------------------------------------------------------

                xWriter.WriteStartDocument()
                xWriter.Formatting = Formatting.Indented
                xWriter.Indentation = 4

                ' --------------------------------------------------------

                ' Start Tag of Shipment
                xWriter.WriteStartElement("Shipment")

                ' ===============================================================================
                ' Start: Shipment Details
                ' ===============================================================================
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(0).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(0).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    Select Case ds.Tables(0).Columns(i).ColumnName
                        Case "Marks"
                            dataArray = Split(dataValue, Chr(13))
                            dataSeq = 1

                            xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName)

                            For Each dataStr As String In dataArray
                                dataTag = "MarkDesc" & dataSeq
                                xWriter.WriteElementString(dataTag, dataStr)
                                dataSeq += 1
                            Next

                            xWriter.WriteEndElement()

                        Case "Packing"
                            dataArray = Split(dataValue, Chr(13))
                            dataSeq = 1

                            xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName)

                            For Each dataStr As String In dataArray
                                dataTag = "PackingDesc" & dataSeq
                                xWriter.WriteElementString(dataTag, dataStr)
                                dataSeq += 1
                            Next

                            xWriter.WriteEndElement()

                        Case "BkhETD", "BkhATD", "BkhETA", "BkhLoadingPort", "BkhDischargePort", "BkhOnboard"
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName.Replace("Bkh", ""), dataValue.Replace(Chr(13), " "))

                        Case "ShipperName", "ConsigneeName", "NotifyName", "AgentName"
                            xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName.Replace("Name", ""))
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                        Case "ShipperEmail", "ConsigneeEmail", "NotifyEmail", "AgentEmail"
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))
                            xWriter.WriteEndElement()

                        Case "VesselType"
                            xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName.Replace("Type", ""))
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                        Case "ETA"
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next
                ' ===============================================================================
                ' End: Shipment Details
                ' ===============================================================================


                ' ===============================================================================
                ' Conatiner Details
                ' ===============================================================================
                If ds.Tables(1).Rows.Count > 0 Then
                    ' Start Tag of Containers
                    xWriter.WriteStartElement("Containers")

                    For j = 0 To ds.Tables(1).Columns.Count - 1
                        dataValue = common.NullVal(ds.Tables(1).Rows(k).Item(ds.Tables(1).Columns(j).ColumnName).ToString, "").Replace(Chr(10), Chr(13))

                        Select Case ds.Tables(1).Columns(j).ColumnName
                            Case "ContainerLn"
                                CfhLn = CType(dataValue, Integer)

                            Case "ContainerNumber"
                                ' Start Tag of Container
                                xWriter.WriteStartElement("Container")
                                xWriter.WriteAttributeString("Number", dataValue.Replace(Chr(13), " "))

                            Case "CBM"
                                xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                                ' ===============================================================================
                                ' Start: PO Details
                                ' ===============================================================================
                                drArray = ds.Tables(2).Select("BkpRefLn = '" & CfhLn & "'") ''" & CfhLn & "'

                                If ds.Tables(2).Select("BkpRefLn = '" & CfhLn & "'").Length > 0 Then
                                    ' Start Tag of PONo
                                    xWriter.WriteStartElement("PONo")
                                    dataSeq = 1

                                    For Each dr As DataRow In drArray
                                        xWriter.WriteStartElement("PO")
                                        xWriter.WriteAttributeString("SEQ", dataSeq)
                                        xWriter.WriteString(dr.Item("PO").ToString)
                                        xWriter.WriteEndElement()

                                        dataSeq += 1
                                    Next

                                    ' End Tag of PONo
                                    xWriter.WriteEndElement()
                                End If

                                ' ===============================================================================
                                ' End: PO Details
                                ' ===============================================================================

                                ' End Tag of Container
                                xWriter.WriteEndElement()

                                ' Move to next row
                                k += 1

                            Case Else
                                xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                        End Select
                    Next

                    ' End Tag of Containers
                    xWriter.WriteEndElement()
                End If
                ' ===============================================================================
                ' End: Container Details
                ' ===============================================================================

                ' End Tag of Shipment
                xWriter.WriteEndElement()

                ' Close XML file
                xWriter.WriteEndDocument()
                xWriter.Flush()
                xWriter.Close()

                ' Return message to screen
                common.showScreenMsg("Agent EDI XML file exported", 1)

                ' Return message to screen
                common.showScreenMsg("Copying Agent EDI XML file to FTP directory", 1)

                ' Create directory if not found
                If Not My.Computer.FileSystem.DirectoryExists(exportPath) Then
                    My.Computer.FileSystem.CreateDirectory(exportPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(exportPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(exportPath & filename)
                End If

                ' Copy file
                My.Computer.FileSystem.CopyFile(backupPath & filename, exportPath & filename)

                ' Return message to screen
                common.showScreenMsg("Agent EDI XML file copied to FTP directory", 1)
                common.SaveLog("Agent EDI XML file copied to FTP directory" & Chr(13) & "Source: " & backupPath & filename & Chr(13) & "Destination: " & exportPath & filename)

                ' Release Memory
                GC.Collect()
                GC.WaitForPendingFinalizers()

                ' --------------------------------------------------------
            Else
                common.showScreenMsg("Shipment data not found (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)
            End If
        Catch ex As Exception
            common.showScreenMsg("Error in exporting Agent EDI file (AelRefId: " & AelRefId & ")")
            common.SaveLog("Error in exporting Agent EDI file (AelRefId: " & AelRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
        End Try

        Return filename

        ' Remove Objects
        filename = Nothing
        cn = Nothing
        sql = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        drArray = Nothing
        common = Nothing
        xWriter = Nothing
        dataValue = Nothing
        dataSeq = Nothing
        dataArray = Nothing
        dataTag = Nothing
        CfhLn = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function

    Sub exportAgentEDI_VAT()

        Dim sql As String = ""
        Dim cn As String = ""
        Dim i As Integer
        Dim common As New common

        Dim AelRefId As Integer = 0
        Dim BkhRefId As Integer = 0
        Dim AgtCode As String = ""

        Dim sqlConn As Object
        Dim sda As Object
        Dim cmd As Object
        Dim ds, ds2 As New DataSet

        Dim filename As String = ""

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Generate Agent VAT EDI export list", 1)

        If My.Settings.DBType = 0 Then
            ' MySQL
            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
            sql = "CALL usp_AgentEDI_ExportList_VAT();"
            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
        Else
            ' SQL Server
            sqlConn = New SqlClient.SqlConnection(cn)
            sql = "EXEC usp_AgentEDI_ExportList_VAT"
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
                        AelRefId = .Item("AelRefId").ToString
                        BkhRefId = .Item("AelBkhRefId").ToString
                        AgtCode = .Item("AelDestCode").ToString

                        ' Export XML file
                        filename = Me.exportAgentEDI_File_VAT(AelRefId, BkhRefId)

                        ' Return message to screen
                        common.showScreenMsg("Completing Agent VAT EDI (AelRefId: " & AelRefId & ", Filename: " & filename & ")")

                        If My.Settings.DBType = 0 Then
                            sql = "CALL usp_AgentEDI_Complete('" & AelRefId & "', '" & filename & "');"

                            cmd = Nothing
                            sda = Nothing

                            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
                            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        Else
                            sql = "EXEC usp_AgentEDI_Complete '" & AelRefId & "', '" & filename & "'"

                            cmd = Nothing
                            sda = Nothing

                            sqlConn = New SqlClient.SqlConnection(cn)
                            cmd = New SqlClient.SqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New SqlClient.SqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        End If

                        ' Return message to screen
                        common.showScreenMsg("Agent VAT EDI exported successfully (AelRefId: " & AelRefId & ")")
                    End With
                Catch ex As Exception
                    common.showScreenMsg("Error found in exporting Agent VAT EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")")
                    common.SaveLog("Error found in exporting Agent VAT EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
                End Try
            Next
        Else
            common.showScreenMsg("No shipments for VAT Agent EDI", 1)
        End If

        ' Remove Variables
        sql = Nothing
        cn = Nothing
        i = Nothing
        common = Nothing
        BkhRefId = Nothing
        AgtCode = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        ds2 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub importAgentEDI_VAT()

        Dim AnknowPath As String
        Dim CurrentFileName As String = ""
        Dim Sender As String = ""
        Dim Receiver As String = ""
        Dim Instance As String = ""
        Dim ReceiveDte As String = ""
        Dim ContentRef As String = ""
        Dim BookingRef As String = ""
        Dim Action As String = ""
        Dim ResponseCd As String = ""
        Dim sql As String
        Dim cn As String
        Dim common As New common

        Dim xmlDoc As New Xml.XmlDocument()

        Dim xmlHdrNode As Xml.XmlNode

        Dim xmlNL1 As Xml.XmlNodeList
        Dim xmlNL2 As Xml.XmlNodeList
        Dim xmlNL3 As Xml.XmlNodeList
        Dim xmlNL4 As Xml.XmlNodeList

        Dim xmlNode1 As Xml.XmlNode
        Dim xmlNode2 As Xml.XmlNode
        Dim xmlNode3 As Xml.XmlNode
        Dim xmlNode4 As Xml.XmlNode

        Dim sqlConn As Object
        Dim sda As Object
        Dim cmd As Object
        Dim ds, ds2 As New DataSet

        common.showScreenMsg("Importing Agent VAT Response File", 1)

        cn = "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        AnknowPath = My.Settings.AnknowPath

        Try
            If My.Computer.FileSystem.DirectoryExists(AnknowPath) Then

                System.IO.Directory.SetCurrentDirectory(AnknowPath)

                For Each CurrentFileName In System.IO.Directory.GetFiles(AnknowPath)
                    CurrentFileName = Replace(CurrentFileName, AnknowPath, "")

                    common.showScreenMsg("Agent VAT Response File: " & CurrentFileName)

                    xmlDoc.Load(AnknowPath & CurrentFileName)

                    xmlHdrNode = xmlDoc.SelectSingleNode("//B2BDocument")

                    xmlNL1 = xmlHdrNode.ChildNodes

                    For Each xmlNode1 In xmlNL1
                        If xmlNode1.Name = "B2BDocumentHeader" Then
                            xmlNL2 = xmlNode1.ChildNodes

                            For Each xmlNode2 In xmlNL2
                                If xmlNode2.Name = "senderIdentifier" Then
                                    Sender = xmlNode2.InnerText
                                End If
                                If xmlNode2.Name = "receiverIdentifier" Then
                                    Receiver = xmlNode2.InnerText
                                End If

                                If xmlNode2.Name = "documentIdentification" Then
                                    xmlNL3 = xmlNode2.ChildNodes

                                    For Each xmlNode3 In xmlNL3
                                        If xmlNode3.Name = "instanceIdentifier" Then
                                            Instance = xmlNode3.InnerText
                                        End If
                                        If xmlNode3.Name = "creationDateTime" Then
                                            ReceiveDte = xmlNode3.InnerText
                                        End If
                                    Next
                                End If
                            Next
                        End If

                        If xmlNode1.Name = "DocumentContent" Then
                            xmlNL2 = xmlNode1.ChildNodes

                            For Each xmlNode2 In xmlNL2
                                If xmlNode2.Name = "contentHeader" Then
                                    xmlNL3 = xmlNode2.ChildNodes

                                    For Each xmlNode3 In xmlNL3
                                        If xmlNode3.Name = "contentReference" Then
                                            ContentRef = xmlNode3.InnerText
                                        End If

                                        If xmlNode3.Name = "bookingReference" Then
                                            BookingRef = xmlNode3.InnerText
                                        End If

                                        If xmlNode3.Name = "contentAction" Then
                                            Action = xmlNode3.InnerText
                                        End If

                                        If xmlNode3.Name = "contentBody" Then
                                            xmlNL4 = xmlNode3.ChildNodes

                                            For Each xmlNode4 In xmlNL4
                                                ResponseCd = xmlNode4.SelectSingleNode("responseTypeCode").InnerXml
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next

                    sql = "EXEC usp_SaveEDI_VAT_Response '" & ContentRef & "', '"
                    sql &= Sender & "', '"
                    sql &= Receiver & "', '"
                    sql &= Instance & "', '"
                    sql &= ReceiveDte & "', '"
                    sql &= BookingRef & "', '"
                    sql &= Action & "', '"
                    sql &= ResponseCd & "'"

                    sqlConn = New SqlClient.SqlConnection(cn)
                    cmd = New SqlClient.SqlCommand(sql, sqlConn)
                    cmd.CommandTimeout = My.Settings.Timeout
                    sda = New SqlClient.SqlDataAdapter(cmd)
                    ds.Clear()
                    sda.Fill(ds)
                    sda.Dispose()
                    sqlConn.Dispose()

                    If My.Computer.FileSystem.FileExists(AnknowPath + "\Backup\" + CurrentFileName) Then
                        My.Computer.FileSystem.DeleteFile(AnknowPath + "\Backup\" + CurrentFileName)
                    End If

                    My.Computer.FileSystem.MoveFile(AnknowPath + "\" + CurrentFileName, AnknowPath + "\Backup\" + CurrentFileName)
                Next
            End If

        Catch ex As Exception
            common.showScreenMsg("Error in importing Agent VAT Response file : " & CurrentFileName)
            common.SaveLog("Error in importing Agent VAT Response file - Error Message : " & ex.Message, "E")
        End Try

    End Sub

    Function exportAgentEDI_File_VAT(ByVal AelRefId As Integer, ByVal BkhRefId As Integer) As String

        Dim filename As String = ""
        Dim cn As String = ""
        Dim sql As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim sqlConn As Object
        Dim cmd As Object
        Dim sda As Object
        Dim ds As New DataSet
        Dim drArray() As DataRow
        Dim common As New common
        Dim xWriter As XmlTextWriter
        Dim dataValue As String = ""
        Dim dataSeq As Integer = 0
        Dim dataArray() As String
        Dim dataTag As String = ""
        Dim CfhLn As Integer = 0
        Dim startPosition As Integer = 0
        Dim columnLength As Integer = 0
        Dim FirstColumnName As String
        Dim SecondColumnName As String
        Dim prevContainer As String = ""
        Dim currContainer As String = ""

        Dim exportPath As String = My.Settings.ExportPath & "AgentVATEDI\Export\"
        Dim backupPath As String = My.Settings.ExportPath & "AgentVATEDI\Backup\"

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Retrieving Agent EDI shipment data (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)

        Try
            If My.Settings.DBType = 0 Then
                ' MySQL server
                sql = "CALL usp_GenEDI_VAT_Detail ('" & AelRefId & "', '" & BkhRefId & "')"
                sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            Else
                ' SQL server
                sql = "EXEC usp_GenEDI_VAT_Detail '" & AelRefId & "', '" & BkhRefId & "'"
                sqlConn = New SqlClient.SqlConnection(cn)
                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New SqlClient.SqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                ' Return message to screen
                common.showScreenMsg("Exporting Agent EDI XML file", 1)

                filename = ds.Tables(0).Rows(0).Item("HouseBillNumber").ToString & "_" & Format(Now, "yyyyMMddHHmmss") & ".xml"

                backupPath &= "VAT" & "\"
                exportPath &= "VAT" & "\"

                ' Create directory if not exist
                If Not My.Computer.FileSystem.DirectoryExists(backupPath) Then
                    My.Computer.FileSystem.CreateDirectory(backupPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(backupPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(backupPath & filename)
                End If

                ' Create XML file
                xWriter = New XmlTextWriter(backupPath & filename, System.Text.Encoding.UTF8)

                ' --------------------------------------------------------
                ' XML Formatting
                ' --------------------------------------------------------

                xWriter.WriteStartDocument()
                xWriter.Formatting = Formatting.Indented
                xWriter.Indentation = 4

                ' --------------------------------------------------------

                ' Start Tag of Shipment
                xWriter.WriteStartElement("SeaShipment")

                ' ===============================================================================
                ' Start: Shipment Details
                ' ===============================================================================
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(0).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(0).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    If ds.Tables(0).Columns(i).ColumnName.Contains("#") Or ds.Tables(0).Columns(i).ColumnName.Contains("$") Then
                        If ds.Tables(0).Columns(i).ColumnName.Contains("#") Then
                            startPosition = ds.Tables(0).Columns(i).ColumnName.IndexOf("#") + 1
                        Else
                            startPosition = ds.Tables(0).Columns(i).ColumnName.IndexOf("$") + 1
                        End If

                        columnLength = ds.Tables(0).Columns(i).ColumnName.Length

                        FirstColumnName = ds.Tables(0).Columns(i).ColumnName.Substring(0, startPosition - 1)
                        SecondColumnName = ds.Tables(0).Columns(i).ColumnName.Substring(startPosition, columnLength - startPosition)

                        If ds.Tables(0).Columns(i).ColumnName.Contains("#") Then
                            If FirstColumnName = "ProfitShare" And SecondColumnName = "Amount" Then
                                xWriter.WriteStartElement("Financials")
                            End If

                            Select Case SecondColumnName
                                Case "Code", "Amount"
                                    xWriter.WriteStartElement(FirstColumnName)
                                    xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))

                                Case "contact", "Currency", "Fax"
                                    If FirstColumnName = "Agent" Then
                                        If SecondColumnName = "Fax" Then
                                            xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                                            xWriter.WriteEndElement()
                                        Else
                                            xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                                        End If
                                    Else
                                        xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                                        xWriter.WriteEndElement()
                                    End If
                                Case Else
                                    xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                            End Select

                            If FirstColumnName = "ChargesTillFOB" And SecondColumnName = "Currency" Then
                                xWriter.WriteEndElement()
                            End If
                        Else
                            Select Case SecondColumnName
                                Case "Name"
                                    xWriter.WriteStartElement(FirstColumnName)
                                    xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))

                                Case "Code"
                                    xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                                    xWriter.WriteEndElement()

                                Case Else
                                    xWriter.WriteElementString(SecondColumnName, dataValue.Replace(Chr(13), " "))
                            End Select
                        End If
                    Else
                        Select Case ds.Tables(0).Columns(i).ColumnName
                            Case "Marks"
                                dataArray = Split(dataValue, Chr(13))
                                dataSeq = 1

                                xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName)

                                For Each dataStr As String In dataArray
                                    dataTag = "MarkDesc" & dataSeq
                                    xWriter.WriteElementString(dataTag, dataStr)
                                    dataSeq += 1
                                Next

                                xWriter.WriteEndElement()

                            Case "Packing"
                                dataArray = Split(dataValue, Chr(13))
                                dataSeq = 1

                                xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName)

                                For Each dataStr As String In dataArray
                                    dataTag = "PackingDesc" & dataSeq
                                    xWriter.WriteElementString(dataTag, dataStr)
                                    dataSeq += 1
                                Next

                                xWriter.WriteEndElement()

                            Case "VesselName", "CarrierName"
                                xWriter.WriteStartElement(ds.Tables(0).Columns(i).ColumnName.Replace("Name", ""))
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                            Case "VesselCode", "CarrierCode"
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName.Replace("Code", ""), dataValue.Replace(Chr(13), " "))
                                xWriter.WriteEndElement()

                            Case "ShipmentOrderNumber"
                                xWriter.WriteStartElement("Shipments")
                                xWriter.WriteStartElement("Shipment")
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                            Case "IncotermCode"
                                xWriter.WriteStartElement("Incoterm")
                                xWriter.WriteElementString("Code", dataValue.Replace(Chr(13), " "))

                            Case "IncotermCity"
                                xWriter.WriteElementString("City", dataValue.Replace(Chr(13), " "))
                                xWriter.WriteEndElement()

                            Case "Pieces"
                                xWriter.WriteStartElement("Totals")
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                            Case "VolumeInCBM"
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))
                                xWriter.WriteEndElement()

                            Case Else
                                xWriter.WriteElementString(ds.Tables(0).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))
                        End Select
                    End If
                Next
                ' ===============================================================================
                ' End: Shipment Details
                ' ===============================================================================

                ' ===============================================================================
                ' Purchase Order Details
                ' ===============================================================================
                If ds.Tables(1).Rows.Count > 0 Then
                    ' Start Tag of PurchaseOrders
                    xWriter.WriteStartElement("PurchaseOrders")

                    For l = 0 To ds.Tables(1).Rows.Count - 1
                        For j = 0 To ds.Tables(1).Columns.Count - 1
                            dataValue = common.NullVal(ds.Tables(1).Rows(k).Item(ds.Tables(1).Columns(j).ColumnName).ToString, "").Replace(Chr(10), Chr(13))

                            Select Case ds.Tables(1).Columns(j).ColumnName
                                Case "ContainerLn"
                                    CfhLn = CType(dataValue, Integer)

                                Case "PONumber"
                                    xWriter.WriteStartElement("PurchaseOrder")
                                    xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                                Case "VolumeInCBM"
                                    xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                    xWriter.WriteEndElement()

                                    ' Move to next row
                                    k += 1

                                Case "CBM"
                                    xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                                    ' ===============================================================================
                                    ' Start: PO Details
                                    ' ===============================================================================
                                    drArray = ds.Tables(2).Select("BkpRefLn = '" & CfhLn & "'") ''" & CfhLn & "'

                                    If ds.Tables(2).Select("BkpRefLn = '" & CfhLn & "'").Length > 0 Then
                                        ' Start Tag of PONo
                                        xWriter.WriteStartElement("PONo")
                                        dataSeq = 1

                                        For Each dr As DataRow In drArray
                                            xWriter.WriteStartElement("PO")
                                            xWriter.WriteAttributeString("SEQ", dataSeq)
                                            xWriter.WriteString(dr.Item("PO").ToString)
                                            xWriter.WriteEndElement()

                                            dataSeq += 1
                                        Next

                                        ' End Tag of PONo
                                        xWriter.WriteEndElement()
                                    End If

                                    ' ===============================================================================
                                    ' End: PO Details
                                    ' ===============================================================================

                                    ' End Tag of Container
                                    xWriter.WriteEndElement()

                                Case Else
                                    xWriter.WriteElementString(ds.Tables(1).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                            End Select
                        Next
                    Next

                    ' End Tag of Purchase Order
                    xWriter.WriteEndElement()
                End If
                ' ===============================================================================
                ' End: Purchase Order Details
                ' ===============================================================================

                ' ===============================================================================
                ' Container Details
                ' ===============================================================================
                If ds.Tables(2).Rows.Count > 0 Then
                    k = 0

                    xWriter.WriteStartElement("Containers")

                    For l = 0 To ds.Tables(2).Rows.Count - 1
                        For j = 0 To ds.Tables(2).Columns.Count - 1
                            dataValue = common.NullVal(ds.Tables(2).Rows(k).Item(ds.Tables(2).Columns(j).ColumnName).ToString, "").Replace(Chr(10), Chr(13))

                            Select Case ds.Tables(2).Columns(j).ColumnName
                                Case "ContainerNumber"
                                    currContainer = dataValue

                                    If currContainer <> prevContainer Then
                                        xWriter.WriteStartElement("Container")
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                    End If

                                Case "PONumber"
                                    If currContainer <> prevContainer Then
                                        xWriter.WriteStartElement("ContainsPurchaseOrderNrs")
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                        xWriter.WriteEndElement()
                                    Else
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                    End If

                                Case "HSCode"
                                    If currContainer <> prevContainer Then
                                        xWriter.WriteStartElement("GoodsItems")
                                        xWriter.WriteStartElement("GoodItem")
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                    End If

                                Case "MarkAndNumber"
                                    If currContainer <> prevContainer Then
                                        xWriter.WriteStartElement("MarksAndNumbers")
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                        xWriter.WriteEndElement()
                                        xWriter.WriteEndElement()
                                        xWriter.WriteEndElement()
                                    End If

                                Case "VolumeInCBM"
                                    If currContainer <> prevContainer Then
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                        xWriter.WriteEndElement()

                                        prevContainer = currContainer
                                    End If

                                    ' Move to next row
                                    k += 1

                                Case Else
                                    If currContainer <> prevContainer Then
                                        xWriter.WriteElementString(ds.Tables(2).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))
                                    End If

                            End Select
                        Next
                    Next

                    xWriter.WriteEndElement()
                    xWriter.WriteEndElement()
                End If
                ' ===============================================================================
                ' Container Details
                ' ===============================================================================

                ' End Tag of Shipment
                xWriter.WriteEndElement()

                ' Close XML file
                xWriter.WriteEndDocument()
                xWriter.Flush()
                xWriter.Close()

                ' Return message to screen
                common.showScreenMsg("Agent VAT EDI XML file exported", 1)

                ' Return message to screen
                common.showScreenMsg("Copying Agent VAT EDI XML file to FTP directory", 1)

                ' Create directory if not found
                If Not My.Computer.FileSystem.DirectoryExists(exportPath) Then
                    My.Computer.FileSystem.CreateDirectory(exportPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(exportPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(exportPath & filename)
                End If

                ' Copy file
                My.Computer.FileSystem.CopyFile(backupPath & filename, exportPath & filename)

                ' Return message to screen
                common.showScreenMsg("Agent VAT EDI XML file copied to FTP directory", 1)
                common.SaveLog("Agent VAT EDI XML file copied to FTP directory" & Chr(13) & "Source: " & backupPath & filename & Chr(13) & "Destination: " & exportPath & filename)

                ' Release Memory
                GC.Collect()
                GC.WaitForPendingFinalizers()

                ' --------------------------------------------------------
            Else
                common.showScreenMsg("Shipment data not found (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)
            End If
        Catch ex As Exception
            common.showScreenMsg("Error in exporting Agent VAT EDI file (AelRefId: " & AelRefId & ")")
            common.SaveLog("Error in exporting Agent VAT EDI file (AelRefId: " & AelRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
        End Try

        Return filename

        ' Remove Objects
        filename = Nothing
        cn = Nothing
        sql = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        drArray = Nothing
        common = Nothing
        xWriter = Nothing
        dataValue = Nothing
        dataSeq = Nothing
        dataArray = Nothing
        dataTag = Nothing
        CfhLn = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Function

    Sub exportAgentEDI_Allbridge()

        Dim sql As String = ""
        Dim cn As String = ""
        Dim i As Integer
        Dim common As New common

        Dim AelRefId As Integer = 0
        Dim BkhRefId As Integer = 0
        Dim AgtCode As String = ""

        Dim sqlConn As Object
        Dim sda As Object
        Dim cmd As Object
        Dim ds, ds2 As New DataSet

        Dim filename As String = ""

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Generate Agent EDI (Allbridge) export list", 1)

        If My.Settings.DBType = 0 Then
            ' MySQL
            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
            sql = "CALL usp_AgentEDI_ExportList_Allbridge();"
            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
        Else
            ' SQL Server
            sqlConn = New SqlClient.SqlConnection(cn)
            sql = "EXEC usp_AgentEDI_ExportList_Allbridge"
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
                        AelRefId = .Item("AelRefId").ToString
                        BkhRefId = .Item("AelBkhRefId").ToString
                        AgtCode = .Item("AelAgtCode").ToString

                        ' Export XML file
                        filename = Me.exportAgentEDI_File_Allbridge(AelRefId, BkhRefId, AgtCode)

                        ' Return message to screen
                        common.showScreenMsg("Completing Agent EDI (AelRefId: " & AelRefId & ", Filename: " & filename & ")")

                        If My.Settings.DBType = 0 Then
                            sql = "CALL usp_AgentEDI_Complete('" & AelRefId & "', '" & filename & "');"

                            cmd = Nothing
                            sda = Nothing

                            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        Else
                            sql = "EXEC usp_AgentEDI_Complete '" & AelRefId & "', '" & filename & "'"

                            cmd = Nothing
                            sda = Nothing

                            cmd = New SqlClient.SqlCommand(sql, sqlConn)
                            cmd.CommandTimeout = My.Settings.Timeout
                            sda = New SqlClient.SqlDataAdapter(cmd)
                            ds2.Clear()
                            sda.Fill(ds2)
                            sda.Dispose()
                            sqlConn.Dispose()
                        End If

                        ' Return message to screen
                        common.showScreenMsg("Agent EDI exported successfully (AelRefId: " & AelRefId & ")")
                    End With
                Catch ex As Exception
                    common.showScreenMsg("Error found in exporting Agent EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")")
                    common.SaveLog("Error found in exporting Agent EDI (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
                End Try
            Next
        Else
            common.showScreenMsg("No shipments for Agent EDI", 1)
        End If

        ' Remove Variables
        sql = Nothing
        cn = Nothing
        i = Nothing
        common = Nothing
        BkhRefId = Nothing
        AgtCode = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        ds2 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Function exportAgentEDI_File_Allbridge(ByVal AelRefId As Integer, ByVal BkhRefId As Integer, ByVal AgtCode As String) As String
        Dim filename As String = ""
        Dim cn As String = ""
        Dim sql As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim sqlConn As Object
        Dim cmd As Object
        Dim sda As Object
        Dim ds As New DataSet
        Dim drArray() As DataRow
        Dim common As New common
        Dim xWriter As XmlTextWriter
        Dim dataValue As String = ""
        Dim dataSeq As Integer = 0
        Dim dataArray() As String
        Dim dataTag As String = ""
        Dim colName As String = ""
        Dim CfhLn As Integer = 0
        Dim tblIndex As Integer = 0
        Dim tblIndex2 As Integer = 0

        Dim exportPath As String = My.Settings.ExportPath & "AgentEDI\Export\"
        Dim backupPath As String = My.Settings.ExportPath & "AgentEDI\Backup\"

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Retrieving Agent EDI shipment data (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)

        Try
            If My.Settings.DBType = 0 Then
                ' MySQL server
                sql = "CALL usp_GenEDI_Allbridge_Detail('" & BkhRefId & "')"
                sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
                cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            Else
                ' SQL server
                sql = "EXEC usp_GenEDI_Allbridge_Detail '" & BkhRefId & "'"
                sqlConn = New SqlClient.SqlConnection(cn)
                cmd = New SqlClient.SqlCommand(sql, sqlConn)
                cmd.CommandTimeout = My.Settings.Timeout
                sda = New SqlClient.SqlDataAdapter(cmd)
                ds.Clear()
                sda.Fill(ds)
                sda.Dispose()
                sqlConn.Dispose()
            End If

            tblIndex = 0

            If ds.Tables(tblIndex).Rows.Count > 0 Then
                ' Return message to screen
                common.showScreenMsg("Exporting Agent EDI (Allbridge) XML file", 1)

                filename = ds.Tables(tblIndex).Rows(0).Item("ShipmentFrom").ToString & "_" & _
                    ds.Tables(tblIndex).Rows(0).Item("ShipmentTo").ToString & "_" & _
                    ds.Tables(tblIndex).Rows(0).Item("BLNo").ToString & "_" & _
                    Format(Now, "yyyyMMddHHmmss") & ".xml"

                backupPath &= ds.Tables(tblIndex).Rows(0).Item("ShipmentTo").ToString & "\"
                exportPath &= ds.Tables(tblIndex).Rows(0).Item("ShipmentTo").ToString & "\"

                ' Create directory if not exist
                If Not My.Computer.FileSystem.DirectoryExists(backupPath) Then
                    My.Computer.FileSystem.CreateDirectory(backupPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(backupPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(backupPath & filename)
                End If

                ' Create XML file
                xWriter = New XmlTextWriter(backupPath & filename, System.Text.Encoding.UTF8)

                ' --------------------------------------------------------
                ' XML Formatting
                ' --------------------------------------------------------

                xWriter.WriteStartDocument()
                xWriter.Formatting = Formatting.Indented
                xWriter.Indentation = 4

                ' --------------------------------------------------------

                ' Start Tag of CargoSoftFile
                xWriter.WriteStartElement("CargoSoftFile")
                xWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
                xWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "http://62.159.250.196/xml/CargoSoftFile_1_01.xsd")
                xWriter.WriteAttributeString("version", "1.0")

                ' ===============================================================================
                ' Start: Message Details
                ' ===============================================================================

                tblIndex = 1

                ' Start Tag of Message
                xWriter.WriteStartElement("Message")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    Select Case ds.Tables(tblIndex).Columns(i).ColumnName
                        'Case "Marks"
                        '    dataArray = Split(dataValue, Chr(13))
                        '    dataSeq = 1

                        '    xWriter.WriteStartElement(ds.Tables(tblIndex).Columns(i).ColumnName)

                        '    For Each dataStr As String In dataArray
                        '        dataTag = "MarkDesc" & dataSeq
                        '        xWriter.WriteElementString(dataTag, dataStr)
                        '        dataSeq += 1
                        '    Next

                        '    xWriter.WriteEndElement()

                        Case "Date"
                            xWriter.WriteStartElement("MessageDate")
                            xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))
                            xWriter.WriteEndElement()

                        Case "EndTag"
                            ' End Tag of Message
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: Message Details
                ' ===============================================================================

                ' Start Tag of File
                xWriter.WriteStartElement("File")

                ' ===============================================================================
                ' Start: File Number Details
                ' ===============================================================================

                tblIndex = 2

                ' Start Tag of FileNumber
                xWriter.WriteStartElement("FileNumber")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    Select Case ds.Tables(tblIndex).Columns(i).ColumnName
                        Case "CompanyCode"
                            xWriter.WriteStartElement("Company")
                            xWriter.WriteAttributeString("Code", dataValue.Replace(Chr(13), " "))
                            xWriter.WriteEndElement() ' End Tag of Company

                        Case "OfficeCode"
                            xWriter.WriteStartElement("Office")
                            xWriter.WriteAttributeString("Code", dataValue.Replace(Chr(13), " "))
                            xWriter.WriteEndElement() ' End Tag of Office

                        Case "EndTag"
                            ' End Tag of FileNumber
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: File Number Details
                ' ===============================================================================


                ' ===============================================================================
                ' Start: Terms Of Delivery Details
                ' ===============================================================================

                tblIndex = 3

                ' Start Tag of TermsOfDelivery
                xWriter.WriteStartElement("TermsOfDelivery")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    Select Case ds.Tables(tblIndex).Columns(i).ColumnName
                        Case "EndTag"
                            ' End Tag of TermsOfDelivery
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: Terms Of Delivery Details
                ' ===============================================================================


                ' ===============================================================================
                ' Start: Addresses Details
                ' ===============================================================================

                tblIndex = 4

                ' Start Tag of Addresses
                xWriter.WriteStartElement("Addresses")
                Dim addr_type As String = ""

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(tblIndex).Columns(i).ColumnName

                    If colName.IndexOf("_") > 0 Then
                        colName = colName.Substring(0, colName.IndexOf("_"))
                    Else
                        colName = colName
                    End If

                    Select Case colName
                        Case "type"
                            ' Start Tag of Address
                            xWriter.WriteStartElement("Address")
                            addr_type = dataValue

                        Case "Address"
                            xWriter.WriteAttributeString("type", dataValue)

                        Case "Code"
                            ' Start Tag of Codes
                            xWriter.WriteStartElement("Codes")
                            xWriter.WriteStartElement("Code")
                            xWriter.WriteAttributeString("type", addr_type)
                            xWriter.WriteString(dataValue)
                            xWriter.WriteEndElement() ' End Tag of Code
                            xWriter.WriteEndElement() ' End Tag of Codes

                        Case "Line1"
                            ' Start Tag of Unformated
                            xWriter.WriteStartElement("Unformated")
                            xWriter.WriteElementString("Line", dataValue)

                        Case "Line2"
                            xWriter.WriteElementString("Line", dataValue)
                            xWriter.WriteEndElement() ' End Tag of Unformated

                            ' End Tag of Address
                            xWriter.WriteEndElement()

                        Case "EndTag"
                            ' End Tag of Addresses
                            xWriter.WriteEndElement()

                        Case Else
                            'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: Addresses Details
                ' ===============================================================================


                ' ===============================================================================
                ' Start: References Details
                ' ===============================================================================

                tblIndex = 5

                ' Start Tag of References
                xWriter.WriteStartElement("References")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    Select Case ds.Tables(tblIndex).Columns(i).ColumnName
                        Case "type"
                            ' Start Tag of Reference
                            xWriter.WriteStartElement("Reference")
                            xWriter.WriteAttributeString("type", ds.Tables(tblIndex).Rows(0).Item("type").ToString)

                        Case "Reference"
                            xWriter.WriteString(ds.Tables(tblIndex).Rows(0).Item("Reference").ToString)
                            xWriter.WriteEndElement() ' End Tag of Reference

                        Case "EndTag"
                            ' End Tag of References
                            xWriter.WriteEndElement()

                        Case Else
                            'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: References Details
                ' ===============================================================================


                ' ===============================================================================
                ' Start: Transports Details
                ' ===============================================================================

                tblIndex = 6

                ' Start Tag of Transports
                xWriter.WriteStartElement("Transports")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(tblIndex).Columns(i).ColumnName

                    If colName.IndexOf("_") > 0 Then
                        colName = colName.Substring(0, colName.IndexOf("_"))
                    Else
                        colName = colName
                    End If

                    Select Case colName
                        Case "TransportType"
                            ' Start Tag of Transport
                            xWriter.WriteStartElement("Transport")
                            xWriter.WriteAttributeString("type", ds.Tables(tblIndex).Rows(0).Item("TransportType").ToString)

                            ' Start Tag of TransportProperties
                            xWriter.WriteStartElement("TransportProperties")

                        Case "PropertyType"
                            ' Start Tag of TransportProperty
                            xWriter.WriteStartElement("TransportProperty")
                            xWriter.WriteAttributeString("type", dataValue)

                        Case "TransportProperty"
                            xWriter.WriteString(dataValue)
                            xWriter.WriteEndElement() ' End Tag of TransportProperty

                        Case "EndTag"
                            xWriter.WriteEndElement() ' End Tag of TransportProperties
                            xWriter.WriteEndElement() ' End Tag of Transport

                            ' End Tag of Transports
                            xWriter.WriteEndElement()

                        Case Else
                            'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: Transports Details
                ' ===============================================================================


                ' ===============================================================================
                ' Start: Events Details
                ' ===============================================================================

                tblIndex = 7

                ' Start Tag of Events
                xWriter.WriteStartElement("Events")

                For i = 0 To ds.Tables(tblIndex).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(tblIndex).Columns(i).ColumnName

                    If colName.IndexOf("_") > 0 Then
                        colName = colName.Substring(0, colName.IndexOf("_"))
                    Else
                        colName = colName
                    End If

                    Select Case colName
                        Case "CodeType"
                            ' Start Tag of Event
                            xWriter.WriteStartElement("Event")

                            ' Start Tag of Codes
                            xWriter.WriteStartElement("Codes")

                            ' Start Tag of Code
                            xWriter.WriteStartElement("Code")
                            xWriter.WriteAttributeString("type", dataValue)

                        Case "Code"
                            xWriter.WriteString(dataValue)
                            xWriter.WriteEndElement() ' End Tag of Code
                            xWriter.WriteEndElement() ' End Tag of Codes

                        Case "LocationCodeType"
                            ' Start Tag of Location
                            xWriter.WriteStartElement("Location")

                            ' Start Tag of Codes
                            xWriter.WriteStartElement("Codes")

                            ' Start Tag of Code
                            xWriter.WriteStartElement("Code")
                            xWriter.WriteAttributeString("type", dataValue)

                        Case "LocationCode"
                            xWriter.WriteString(dataValue)
                            xWriter.WriteEndElement() ' End Tag of Code
                            xWriter.WriteEndElement() ' End Tag of Codes

                        Case "LocationName"
                            xWriter.WriteElementString("Name", dataValue)
                            xWriter.WriteEndElement() ' End Tag of Location

                        Case "EstimatedDate"
                            If dataValue <> "" Then
                                ' Start Tag of Estimated
                                xWriter.WriteStartElement("Estimated")
                                xWriter.WriteElementString("Date", dataValue)
                                xWriter.WriteEndElement() ' Eng Tag of Estimated
                            End If
                            xWriter.WriteEndElement() ' Eng Tag of Event

                        Case "EndTag"
                            ' End Tag of Events
                            xWriter.WriteEndElement()

                        Case Else
                            'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(i).ColumnName, dataValue.Replace(Chr(13), " "))

                    End Select
                Next

                ' ===============================================================================
                ' End: Events Details
                ' ===============================================================================


                ' ===============================================================================
                ' Containers
                ' ===============================================================================

                tblIndex = 8

                If ds.Tables(tblIndex).Rows.Count > 0 Then
                    ' Start Tag of Containers
                    xWriter.WriteStartElement("Containers")

                    k = 0

                    For j = 0 To ds.Tables(tblIndex).Columns.Count - 1
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(k).Item(ds.Tables(tblIndex).Columns(j).ColumnName).ToString, "").Replace(Chr(10), Chr(13))

                        Select Case ds.Tables(tblIndex).Columns(j).ColumnName
                            Case "Number"
                                ' Start Tag of Container
                                xWriter.WriteStartElement("Container")

                                xWriter.WriteElementString(ds.Tables(tblIndex).Columns(j).ColumnName, dataValue)

                            Case "SealNo"
                                xWriter.WriteElementString(ds.Tables(tblIndex).Columns(j).ColumnName, dataValue)

                            Case "SizeType"
                                ' Start Tag of Size
                                xWriter.WriteStartElement("Size")

                                ' Start Tag of Code
                                xWriter.WriteStartElement("Code")
                                xWriter.WriteAttributeString("type", dataValue)

                            Case "Size"
                                xWriter.WriteString(dataValue)
                                xWriter.WriteEndElement() ' End Tag of Code
                                xWriter.WriteEndElement() ' Eng Tag of Size
                                xWriter.WriteEndElement() ' End Tag of Container

                                If k + 1 < ds.Tables(tblIndex).Rows.Count Then
                                    k += 1
                                    j = 0
                                End If

                            Case Else
                                'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                        End Select
                    Next

                    ' End Tag of Containers
                    xWriter.WriteEndElement()
                End If
                ' ===============================================================================
                ' End: Container Details
                ' ===============================================================================


                ' ===============================================================================
                ' Goods
                ' ===============================================================================

                tblIndex = 9
                tblIndex2 = 10

                If ds.Tables(tblIndex).Rows.Count > 0 Then
                    ' Start Tag of Containers
                    xWriter.WriteStartElement("Goods")

                    k = 0

                    For j = 0 To ds.Tables(tblIndex).Columns.Count - 1
                        dataValue = common.NullVal(ds.Tables(tblIndex).Rows(k).Item(ds.Tables(tblIndex).Columns(j).ColumnName).ToString, "").Replace(Chr(10), Chr(13))

                        Select Case ds.Tables(tblIndex).Columns(j).ColumnName
                            Case "Mark1"
                                ' Start Tag of Good
                                xWriter.WriteStartElement("Good")

                                xWriter.WriteElementString("Marks", dataValue)

                            Case "Mark2"
                                xWriter.WriteElementString("Marks", dataValue)

                            Case "Quantity"
                                ' Start Tag of Packaging
                                xWriter.WriteStartElement("Packaging")

                                xWriter.WriteElementString(ds.Tables(tblIndex).Columns(j).ColumnName, dataValue)

                            Case "CodeType"
                                ' Start Tag of Code
                                xWriter.WriteStartElement("Code")
                                xWriter.WriteAttributeString("type", dataValue)

                            Case "Size"
                                xWriter.WriteString(dataValue)
                                xWriter.WriteEndElement() ' Eng Tag of Code
                                xWriter.WriteEndElement() ' Eng Tag of Packaging

                                ' Descriptions
                                For l = 0 To ds.Tables(tblIndex2).Rows.Count - 1
                                    If My.Settings.DBType = 0 Then
                                        ' MySQL
                                        dataValue = common.NullVal(ds.Tables(tblIndex2).Rows(l).Item("Description").ToString, "").Replace(Chr(10), "")
                                    Else
                                        ' SQL Server
                                        dataValue = common.NullVal(ds.Tables(tblIndex2).Rows(l).Item("Description").ToString, "").Replace(Chr(10), "")
                                    End If

                                    dataArray = Split(dataValue, Chr(13))

                                    For Each dataStr As String In dataArray
                                        xWriter.WriteElementString("Description", dataStr)
                                    Next
                                Next

                            Case "WeightType"
                                ' Start Tag of GrossWeight
                                xWriter.WriteStartElement("GrossWeight")
                                xWriter.WriteAttributeString("unit", dataValue)

                            Case "Weight"
                                xWriter.WriteString(dataValue)
                                xWriter.WriteEndElement() ' Eng Tag of GrossWeight

                            Case "CBMType"
                                ' Start Tag of Volumn
                                xWriter.WriteStartElement("Volume")
                                xWriter.WriteAttributeString("unit", dataValue)

                            Case "CBM"
                                xWriter.WriteString(dataValue)
                                xWriter.WriteEndElement() ' Eng Tag of Volumn
                                xWriter.WriteEndElement() ' Ent Tag of Good

                                If k + 1 < ds.Tables(tblIndex).Rows.Count Then
                                    k += 1
                                    j = 0
                                End If

                            Case Else
                                'xWriter.WriteElementString(ds.Tables(tblIndex).Columns(j).ColumnName, dataValue.Replace(Chr(13), " "))

                        End Select
                    Next

                    ' End Tag of Goods
                    xWriter.WriteEndElement()
                End If
                ' ===============================================================================
                ' End: Goods Details
                ' ===============================================================================

                ' End Tag of File
                xWriter.WriteEndElement()

                ' End Tag of CargoSoftFile
                xWriter.WriteEndElement()

                ' Close XML file
                xWriter.WriteEndDocument()
                xWriter.Flush()
                xWriter.Close()

                ' Return message to screen
                common.showScreenMsg("Agent EDI (Allbridge) XML file exported", 1)

                ' Return message to screen
                common.showScreenMsg("Copying Agent EDI (Allbridge) XML file to FTP directory", 1)

                ' Create directory if not found
                If Not My.Computer.FileSystem.DirectoryExists(exportPath) Then
                    My.Computer.FileSystem.CreateDirectory(exportPath)
                End If

                ' Delete file if exist
                If My.Computer.FileSystem.FileExists(exportPath & filename) Then
                    My.Computer.FileSystem.DeleteFile(exportPath & filename)
                End If

                ' Copy file
                My.Computer.FileSystem.CopyFile(backupPath & filename, exportPath & filename)

                ' Return message to screen
                common.showScreenMsg("Agent EDI (Allbridge) XML file copied to FTP directory", 1)
                common.SaveLog("Agent EDI (Allbridge) XML file copied to FTP directory" & Chr(13) & "Source: " & backupPath & filename & Chr(13) & "Destination: " & exportPath & filename)

                ' Release Memory
                GC.Collect()
                GC.WaitForPendingFinalizers()

                ' --------------------------------------------------------
            Else
                common.showScreenMsg("Shipment data not found (AelRefId: " & AelRefId & ", BkhRefId: " & BkhRefId & ")", 1)
            End If
        Catch ex As Exception
            common.showScreenMsg("Error in exporting Agent EDI file (AelRefId: " & AelRefId & ")")
            common.SaveLog("Error in exporting Agent EDI file (AelRefId: " & AelRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
        End Try

        Return filename

        ' Remove Objects
        filename = Nothing
        cn = Nothing
        sql = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        drArray = Nothing
        common = Nothing
        xWriter = Nothing
        dataValue = Nothing
        dataSeq = Nothing
        dataArray = Nothing
        dataTag = Nothing
        CfhLn = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function

End Class
