Imports MySql
Imports System.Xml

Public Class ClsDestEDI
    Shared procSection As String = ""
    Shared ediType As String = "Dest"

    Sub exportDestEDI()

        Dim sql As String = ""
        Dim cn As String = ""
        Dim i As Integer
        Dim common As New common

        Dim BkeRefId As Integer = 0
        Dim BkhRefId As Integer = 0
        Dim BkhBrhCd As String = ""

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
        common.showScreenMsg("Generate Dest EDI export list", 1)

        If My.Settings.DBType = 0 Then
            ' MySQL
            sqlConn = New MySql.Data.MySqlClient.MySqlConnection(cn)
            sql = "CALL usp_DestEDI_ExportList();"
            cmd = New MySql.Data.MySqlClient.MySqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
            sda.Dispose()
            sqlConn.Dispose()
        Else
            ' SQL Server
            sqlConn = New SqlClient.SqlConnection(cn)
            sql = "EXEC usp_DestEDI_ExportList"
            cmd = New SqlClient.SqlCommand(sql, sqlConn)
            cmd.CommandTimeout = My.Settings.Timeout
            sda = New SqlClient.SqlDataAdapter(cmd)
            ds.Clear()
            sda.Fill(ds)
            sda.Dispose()
            sqlConn.Dispose()
        End If

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    Try
                        ' Reset filename
                        filename = ""

                        With ds.Tables(0).Rows(i)
                            procSection = ""
                            BkeRefId = .Item("RefId").ToString
                            BkhRefId = .Item("BkeBkgRefId").ToString
                            BkhBrhCd = .Item("BkeBkgBrhCd").ToString

                            ' Export XML file
                            filename = Me.exportDestEDI_File(BkeRefId, BkhRefId, BkhBrhCd)

                            ' Return message to screen
                            common.showScreenMsg("Completing Dest EDI (BkeRefId: " & BkeRefId & ", Filename: " & filename & ")")

                            procSection = "Completing Dest EDI Request"
                            If My.Settings.DBType = 0 Then
                                sql = "CALL usp_DestEDI_Complete('" & BkeRefId & "', '" & filename & "');"

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
                                sql = "EXEC usp_DestEDI_Complete '" & BkeRefId & "', '" & filename & "'"

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
                            common.showScreenMsg("Dest EDI exported successfully (BkeRefId: " & BkeRefId & ")")
                        End With
                    Catch ex As Exception
                        common.showScreenMsg("Error found in exporting Dest EDI (BkeRefId: " & BkeRefId & ", BkhRefId: " & BkhRefId & ")")
                        common.SaveLog("Error found in exporting Dest EDI (BkeRefId: " & BkeRefId & ", BkhRefId: " & BkhRefId & ")" & Chr(13) & "Error Message: " & ex.Message, "E")
                    End Try
                Next
            Else
                common.showScreenMsg("No shipments for Dest EDI", 1)
            End If
        Else
            common.showScreenMsg("No shipments for Dest EDI", 1)
        End If

        ' Remove Variables

        sql = Nothing
        cn = Nothing
        i = Nothing
        common = Nothing
        BkhRefId = Nothing
        BkhBrhCd = Nothing
        sqlConn = Nothing
        cmd = Nothing
        sda = Nothing
        ds = Nothing
        ds2 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Function exportDestEDI_File(ByVal BkeRefId As Integer, ByVal BkhRefId As Integer, ByVal BkhBrhCd As String) As String
        Dim filename As String = ""
        Dim cn As String = ""
        Dim sql As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim colIndex As Integer = 0
        Dim sqlConn As Object
        Dim cmd As Object
        Dim sda As Object
        Dim ds As New DataSet
        Dim common As New common
        Dim xWriter As XmlTextWriter
        Dim rtnPara() As String
        Dim exportDate As String = Format(Now, "yyyy.MM.dd")

        Dim ProStatus As Integer = 0
        Dim notIncludeFL As Integer = 0

        Dim exportPath As String = My.Settings.ExportPath & "DestEDI\Export\"
        Dim backupPath As String = My.Settings.ExportPath & "DestEDI\Backup\"

        cn &= "Data Source=" & My.Settings.Server & ";"
        cn &= "Database=" & My.Settings.DB & ";"
        cn &= "User Id=" & My.Settings.Login & ";"
        cn &= "Password=" & My.Settings.Password & ";"

        ' Return message to screen
        common.showScreenMsg("Retrieving Dest EDI shipment data (BkeRefId: " & BkeRefId & ", BkhRefId: " & BkhRefId & ")", 1)

        Try
            If My.Settings.DBType = 0 Then
                ' MySQL server
                sql = "CALL usp_DestEDI_Info('" & BkeRefId & "', '" & BkhRefId & "', '" & BkhBrhCd & "')"
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
                sql = "EXEC usp_DestEDI_Info '" & BkeRefId & "', '" & BkhRefId & "', '" & BkhBrhCd & "'"
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
                common.showScreenMsg("Exporting Dest EDI XML file", 1)

                filename = ds.Tables(0).Rows(0).Item("BkiFrmServerName").ToString & "_" & _
                    ds.Tables(0).Rows(0).Item("BkiToSubBrhCode").ToString & "_" & _
                    ds.Tables(1).Rows(0).Item("BkhBLNo").ToString & "_" & _
                    Format(Now, "yyyyMMddHHmmss") & ".xml"

                'backupPath &= exportDate & "\"
                'exportPath &= exportDate & "\"

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

                ' Start Tag of EDI
                xWriter.WriteStartElement("EDI")

                ' Write EDI Import secton
                procSection = "EDIImport Section"
                rtnPara = Split(Me.GenerateEDIImport(ds.Tables(0), xWriter, BkeRefId), ",")
                proStatus = rtnPara(0)
                notIncludeFL = rtnPara(1)

                ' Writer BookingHdr Data
                procSection = "BookingHdr Section"
                Me.GenerateBookingHdrSection(ds.Tables(1), xWriter)

                If ProStatus <> 4 Then

                    ' Write BookingCtnr/CFSHdr & BookingPO Data
                    procSection = "Container Section"
                    Me.GenerateContainerSection(ds, xWriter, BkhRefId)

                    ' Write BookingChg Data
                    procSection = "BookingChg Section"
                    Me.GenerateBookingChgSection(ds.Tables(4), xWriter)

                    ' Write BookingVou Data
                    procSection = "BookingVou Section"
                    Me.GenerateBookingVouSection(ds.Tables(5), xWriter)

                    If notIncludeFL = 0 Then
                        ' Write ShareHdr & ShareDtl Data
                        procSection = "ShareHdr Section"
                        Me.GenerateShareHdrSection(ds, xWriter)

                        ' Write ProfitShareHdr Data
                        procSection = "ProfitShareHdr Section"
                        Me.GenerateProfitShareHdrSection(ds.Tables(8), xWriter)

                        ' Write ProfitShareDtl Data
                        procSection = "ProfitShareDtl Section"
                        Me.GenerateProfitShareDtlSection(ds.Tables(9), xWriter)
                    End If

                    ' Write Shipper Data
                    procSection = "Shipper Section"
                    Me.GenerateShipperSection(ds.Tables(10), xWriter)

                    ' Write Consignee Data
                    procSection = "Consignee Section"
                    Me.GenerateConsigneeSection(ds.Tables(11), xWriter, "Consignee")

                    ' Write Notify Data
                    procSection = "Notify Section"
                    Me.GenerateConsigneeSection(ds.Tables(12), xWriter, "Notify")

                    ' Write AMS Consignee Data
                    procSection = "AMS Consignee Section"
                    Me.GenerateConsigneeSection(ds.Tables(13), xWriter, "AMSConsignee")

                    ' Write Booking Agent Data
                    procSection = "Booking Agent Section"
                    Me.GenerateAgentSection(ds.Tables(14), xWriter)

                    ' Write Charge Agent Data
                    procSection = "Charge Agent Section"
                    Me.GenerateAgentSection(ds.Tables(15), xWriter, 1)

                    ' Write Vessel Data
                    procSection = "Vessel Section"
                    Me.GenerateVesselSection(ds.Tables(16), xWriter)

                    ' Write Vessel Carrier Data
                    procSection = "VesselCarrier Section"
                    Me.GenerateCarrierSection(ds.Tables(17), xWriter)

                    ' Write Charge Carrier / Supplier Data
                    procSection = "Charge Carrier / Supplier Section"
                    Me.GenerateCarrierSection(ds.Tables(18), xWriter, 1)

                    ' Write Charge Data
                    procSection = "Charge Section"
                    Me.GenerateChargeSection(ds.Tables(19), xWriter)

                    ' Write Cost Data
                    procSection = "Cost Section"
                    Me.GenerateCostSection(ds.Tables(20), xWriter)

                    ' Write Location Data
                    procSection = "Location Section"
                    Me.GenerateLocationSection(ds.Tables(21), xWriter)

                    ' Write Container Size Data
                    procSection = "CtnrSize Section"
                    Me.GenerateCtnrSizeSection(ds.Tables(22), xWriter)

                    ' Write Unit Data
                    procSection = "Unit Section"
                    Me.GenerateUnitSection(ds.Tables(23), xWriter)

                    ' Write Sales Data
                    procSection = "Sales Section"
                    Me.GenerateSalesSection(ds.Tables(24), xWriter)

                    ' Write ISF Data
                    procSection = "BookingISF Section"
                    Me.GenerateBookingISFSection(ds, xWriter)

                End If

                ' End Tag of EDI
                xWriter.WriteEndElement()

                ' Close XML file
                xWriter.WriteEndDocument()
                xWriter.Flush()
                xWriter.Close()

                ' Return message to screen
                common.showScreenMsg("Dest EDI XML file exported", 1)

                ' Return message to screen
                common.showScreenMsg("Copying Dest EDI XML file to FTP directory", 1)

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
                common.showScreenMsg("Dest EDI XML file copied to FTP directory", 1)
                common.SaveLog("Dest EDI XML file copied to FTP directory" & Chr(13) & "Source: " & backupPath & filename & Chr(13) & "Destination: " & exportPath & filename)

                ' Release Memory
                GC.Collect()
                GC.WaitForPendingFinalizers()

                ' --------------------------------------------------------
            Else
                common.showScreenMsg("Shipment data not found (BkeRefId: " & BkeRefId & ", BkhRefId: " & BkhRefId & ")", 1)
            End If
        Catch ex As Exception
            common.showScreenMsg("Error in exporting Dest EDI file (BkeRefId: " & BkeRefId & ")")
            common.SaveLog("Error in exporting Dest EDI file (BkeRefId: " & BkeRefId & ", Section: " & procSection & ")" & Chr(13) & "Error Message: " & ex.Message, "E")

            ' Send error email
            Dim clsEmail As New ClsEmail
            clsEmail.sendAckEmail(ediType, "Export Request ID: " & BkeRefId & "<br />Section: " & procSection & "<br /><br />" & ex.Message, 2)
            clsEmail = Nothing

            ' Release Memory
            GC.Collect()
            GC.WaitForPendingFinalizers()
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
        common = Nothing
        xWriter = Nothing
        ProStatus = Nothing
        notIncludeFL = Nothing
        rtnPara = Nothing
        
        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Function

    Function GenerateEDIImport(ByVal dt As DataTable, ByVal xWriter As XmlWriter, ByVal BkeRefId As Integer) As String
        Dim common As New common
        Dim i As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""

        Dim BkeProStatus As Integer = 0
        Dim BkiIsfExport As Integer = 0
        Dim BkiFreListUpd As Integer = 0

        ' ===============================================================================
        ' Start: EDIImport Details
        ' ===============================================================================
        For i = 0 To dt.Columns.Count - 1
            If My.Settings.DBType = 0 Then
                ' MySQL
                dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
            Else
                ' SQL Server
                dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
            End If

            colName = dt.Columns(i).ColumnName

            Select Case colName
                Case "BkgRefId"
                    xWriter.WriteStartElement("EDIImport")
                    xWriter.WriteStartElement("BkiBkgRefId")
                    xWriter.WriteAttributeString(colName, dataValue)

                    ' Add EDI Rquest ID
                    xWriter.WriteElementString("EDI_REF_ID", BkeRefId)

                Case "BkeProStatus"
                    BkeProStatus = dataValue.Replace(Chr(13), " ")
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                Case "BkiIsfExport"
                    BkeProStatus = dataValue.Replace(Chr(13), " ")
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                Case "BkiFreListUpd"
                    BkiFreListUpd = dataValue.Replace(Chr(13), " ")
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    ' End tag of BkiBkgRefId
                    xWriter.WriteEndElement()

                    ' End tag of EDIImport
                    xWriter.WriteEndElement()

                Case Else
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

            End Select
        Next
        ' ===============================================================================
        ' End: EDIImport Details
        ' ===============================================================================

        If BkiIsfExport = 1 And BkiFreListUpd = 0 Then
            GenerateEDIImport = BkeProStatus & ",1"
        Else
            GenerateEDIImport = BkeProStatus & ",0"
        End If

        ' Remove objects
        common = Nothing
        i = Nothing
        colName = Nothing
        dataValue = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Function

    Sub GenerateBookingHdrSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""

        ' ===============================================================================
        ' Start: Shipment Details - BookingHdr
        ' ===============================================================================
        For i = 0 To dt.Columns.Count - 1
            If My.Settings.DBType = 0 Then
                ' MySQL
                dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
            Else
                ' SQL Server
                dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(13) & Chr(10), Chr(13))
            End If

            colName = dt.Columns(i).ColumnName

            Select Case colName
                Case "BkgRefId"
                    ' Start tag of BookingHdr
                    xWriter.WriteStartElement("BookingHdr")

                    ' Start tag of BkhRefId
                    xWriter.WriteStartElement("BkhRefId")
                    xWriter.WriteAttributeString("BkgRefId", dataValue)

                Case "BkhMarks", "BkhPacking", "BkhShpr", "BkhCons", "BkhAgt", "BkhNot"
                    xWriter.WriteElementString(colName, dataValue)

                Case "IsShptPend"
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    ' End tag of BkhRefId
                    xWriter.WriteEndElement()

                    ' End tag of BookingHdr
                    xWriter.WriteEndElement()

                Case Else
                    xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

            End Select

        Next
        ' ===============================================================================
        ' End: Shipment Details - BookingHdr
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        colName = Nothing
        dataValue = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateContainerSection(ByVal ds As DataSet, ByVal xWriter As XmlWriter, ByVal BkhRefId As Integer)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim rowIndex As Integer = 0
        Dim colIndex As Integer = 0
        Dim colIndex2 As Integer = 0
        Dim dataValue As String = ""
        Dim dataValue2 As String = ""
        Dim colName As String = ""
        Dim colName2 As String = ""
        Dim dataSeq As Integer = 1
        Dim dataSeq2 As Integer = 1
        Dim dataSeq3 As Integer = 1
        Dim IsColEnd As Integer = 0
        Dim IsColEnd2 As Integer = 0
        Dim isCY As Integer = 0
        Dim CtnrLn As Integer = 0
        Dim dt As New DataTable
        Dim isStart As Integer = 0
        Dim poCount As Integer = 0
        Dim isPO_Start As Integer = 0

        Dim drArray() As DataRow

        ' ===============================================================================
        ' Start: Shipment Details - BookingCtnr / CFSHdr
        ' ===============================================================================
        If ds.Tables(2).Rows.Count > 0 Then

            dataSeq = 1

            For i = 0 To ds.Tables(2).Rows.Count - 1
                For j = 0 To ds.Tables(2).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(2).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(2).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(2).Columns(colIndex).ColumnName

                    Select Case colName
                        Case "CtnrRefId"
                            ' MySQL only
                            If My.Settings.DBType = 0 Then
                                CtnrLn = dataValue
                            End If

                        Case "CfhRefId"
                            If isStart = 0 Then
                                ' Start tag of CFSHdr
                                xWriter.WriteStartElement("CFSHdr")
                                isCY = 0

                                ' Start tag of CfhBkgRefId
                                xWriter.WriteStartElement("CfhBkgRefId")
                                xWriter.WriteAttributeString("BkgRefId", BkhRefId)
                                isStart = 1
                            End If

                            ' Start tag of CfhLn
                            xWriter.WriteStartElement("CfhLn" & dataSeq)
                            isPO_Start = 0
                            dataSeq3 = 1

                        Case "BktRefId"

                            If isStart = 0 Then
                                ' Start tag of BookingCtnr
                                xWriter.WriteStartElement("BookingCtnr")

                                ' Start tag of BktRefId
                                xWriter.WriteStartElement("BktRefId")
                                xWriter.WriteAttributeString("BktRefId", BkhRefId)
                                isStart = 1
                            End If

                            ' MySQL only
                            If My.Settings.DBType = 0 Then
                                ' Replace BktLn
                                CtnrLn = dataValue
                            End If

                            isCY = 1

                            ' Start tag of BktLn
                            xWriter.WriteStartElement("BktLn" & dataSeq)
                            isPO_Start = 0

                        Case "BktLn"
                            ' SQL Server only
                            If My.Settings.DBType = 1 Then
                                CtnrLn = dataValue
                            End If

                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        Case "CfhLn"
                            ' SQL Server only
                            If My.Settings.DBType = 1 Then
                                CtnrLn = dataValue
                            End If

                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        Case "CfdRefId"
                            'If isStart = 0 Then
                            ' Start tag of CfdBkgRefId
                            xWriter.WriteStartElement("CfdBkgRefId")
                            xWriter.WriteAttributeString("BkgRefId", BkhRefId)

                            ' Start tag of CfdLn + SeqNo
                            xWriter.WriteStartElement("CfdLn" & dataSeq3)

                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            'isStart = 1
                            'End If

                        Case "CfdLn"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        Case "BktPCS", "CfdPCS"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            If colName = "CfdPCS" Then
                                ' End tag of CfdLn + SeqNo
                                xWriter.WriteEndElement()

                                ' End tag of CfdBkgRefId
                                xWriter.WriteEndElement()

                                dataSeq3 += 1
                            End If

                            ' Reset Column
                            IsColEnd = 1

                            ' Reset Count
                            poCount = 0

                            ' Container PO
                            If My.Settings.DBType = 0 Then
                                ' MySQL
                                drArray = ds.Tables(3).Select("CtnrRefId = " & CtnrLn)
                                poCount = ds.Tables(3).Select("CtnrRefId = '" & CtnrLn & "'").Length
                            Else
                                ' SQL Server
                                drArray = ds.Tables(3).Select("BkpRefLn = " & CtnrLn)
                                poCount = ds.Tables(3).Select("BkpRefLn = '" & CtnrLn & "'").Length
                            End If

                            If poCount > 0 Then
                                dataSeq2 = 1

                                ' Clear data table
                                dt = New DataTable
                                dt = ds.Tables(3).Clone
                                dt.Rows.Clear()

                                For Each dr As DataRow In drArray
                                    dt.ImportRow(dr)
                                Next

                                For k = 0 To dt.Rows.Count - 1
                                    For l = 0 To dt.Columns.Count - 1
                                        If My.Settings.DBType = 0 Then
                                            ' MySQL
                                            dataValue2 = common.NullVal(dt.Rows(k).Item(l).ToString, "").Replace(Chr(10), "")
                                        Else
                                            ' SQL Server
                                            dataValue2 = common.NullVal(dt.Rows(k).Item(l).ToString, "").Replace(Chr(10), Chr(13))
                                        End If

                                        colName2 = dt.Columns(l).ColumnName

                                        Select Case colName2
                                            Case "CtnrRefId"
                                                ''

                                            Case "BkpRefLn"

                                                If isPO_Start = 0 Then
                                                    ' Start tag of BkpRefId
                                                    xWriter.WriteStartElement("BkpRefId")
                                                    xWriter.WriteAttributeString("BkpRefId", BkhRefId)
                                                    isPO_Start = 1
                                                End If

                                                ' Start tag of BkpLn
                                                xWriter.WriteStartElement("BkpLn" & dataSeq2)

                                                xWriter.WriteElementString(colName2, dataValue2.Replace(Chr(13), " "))

                                            Case "BkpCreUsr"
                                                xWriter.WriteElementString(colName2, dataValue2.Replace(Chr(13), " "))

                                                ' Move to next data seq
                                                dataSeq2 += 1

                                                ' Reset column index
                                                IsColEnd2 = 1

                                            Case Else
                                                xWriter.WriteElementString(colName2, dataValue2.Replace(Chr(13), " "))

                                        End Select

                                        ' Move next column index (BookingPO)
                                        If IsColEnd2 = 1 Then
                                            ' End tag of BkpLn
                                            xWriter.WriteEndElement()

                                            colIndex2 = 0
                                            IsColEnd2 = 0
                                        Else
                                            colIndex2 += 1
                                        End If
                                    Next
                                Next
                            End If

                            If isPO_Start = 1 Then
                                ' End tag of BkpRefId
                                xWriter.WriteEndElement()
                            End If

                            ' End tag of BktLn / CfhLn
                            xWriter.WriteEndElement()

                            ' Move to next data seq
                            dataSeq += 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index (BookingCtnr / CFSHdr)
                    If IsColEnd = 1 Then
                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If
                Next
            Next

            ' End tag of BktRefId / CfhBkgRefId
            xWriter.WriteEndElement()

            ' End tag of BookingCtnr / CFSHdr
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - BookingCtnr / CFSHdr
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        l = Nothing
        rowIndex = Nothing
        colIndex = Nothing
        colIndex2 = Nothing
        dataValue = Nothing
        dataValue2 = Nothing
        colName = Nothing
        colName2 = Nothing
        dataSeq = Nothing
        dataSeq2 = Nothing
        dataSeq3 = Nothing
        IsColEnd = Nothing
        IsColEnd2 = Nothing
        isCY = Nothing
        CtnrLn = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateBookingChgSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rowIndex As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isStart As Integer = 1
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - BookingChg
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of BookingHdr
            xWriter.WriteStartElement("BookingChg")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "BkcRefId"
                            If isStart = 1 Then
                                ' Start tag of BkcRefId
                                xWriter.WriteStartElement("BkcRefId")
                                xWriter.WriteAttributeString("BkgRefId", dataValue)

                                ' Keep only one BkcRefId start tag
                                isStart = 0
                            End If

                        Case "BkcLn"
                            ' Start tag of BkcLn
                            xWriter.WriteStartElement("BkcLn" & dataSeq)

                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        Case "BkcLstUsr"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' End tag of BkcLn
                            xWriter.WriteEndElement()

                            ' Move to next data, row, seq
                            dataSeq += 1
                            rowIndex += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of BkcRefId
            xWriter.WriteEndElement()

            ' End tag of BookingChg
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - BookingChg
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        rowIndex = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        isStart = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateBookingVouSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rowIndex As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isStart As Integer = 1
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - BookingVou
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of BookingHdr
            xWriter.WriteStartElement("BookingVou")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "BkvRefId"
                            If isStart = 1 Then
                                ' Start tag of BkvRefId
                                xWriter.WriteStartElement("BkvRefId")
                                xWriter.WriteAttributeString("BkgRefId", dataValue)

                                ' Keep only one BkvRefId start tag
                                isStart = 0
                            End If

                        Case "BkvLn"
                            ' Start tag of BkvLn
                            xWriter.WriteStartElement("BkvLn" & dataSeq)

                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        Case "BkvRemark"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' End tag of BkvLn
                            xWriter.WriteEndElement()

                            ' Move to next data, row, seq
                            dataSeq += 1
                            rowIndex += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of BkvRefId
            xWriter.WriteEndElement()

            ' End tag of BookingVou
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - BookingVou
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        rowIndex = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        isStart = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateShareHdrSection(ByVal ds As DataSet, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim colIndex As Integer = 0
        Dim colIndex2 As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim colName2 As String = ""
        Dim isStart As Integer = 1
        Dim isStart2 As Integer = 1
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0
        Dim IsColEnd2 As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - ShareHdr
        ' ===============================================================================
        If ds.Tables(6).Rows.Count > 0 Then
            ' Start tag of BookingHdr
            xWriter.WriteStartElement("ShareHdr")

            For i = 0 To ds.Tables(6).Rows.Count - 1
                For j = 0 To ds.Tables(6).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(6).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(6).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(6).Columns(colIndex).ColumnName

                    Select Case colName
                        Case "ShhRefId"
                            If isStart = 1 Then
                                ' Start tag of ShhRefId
                                xWriter.WriteStartElement("ShhRefId")
                                xWriter.WriteAttributeString("RefId", dataValue)

                                ' Keep only one ShhRefId start tag
                                isStart = 0
                            End If

                        Case "ShhRevise"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            For k = 0 To ds.Tables(7).Rows.Count - 1
                                For l = 0 To ds.Tables(7).Columns.Count - 1
                                    If My.Settings.DBType = 0 Then
                                        ' MySQL
                                        dataValue = common.NullVal(ds.Tables(7).Rows(k).Item(colIndex2).ToString, "").Replace(Chr(10), "")
                                    Else
                                        ' SQL Server
                                        dataValue = common.NullVal(ds.Tables(7).Rows(k).Item(colIndex2).ToString, "").Replace(Chr(10), Chr(13))
                                    End If

                                    colName2 = ds.Tables(7).Columns(colIndex2).ColumnName

                                    Select Case colName2
                                        Case "ShdRefId"
                                            If isStart2 = 1 Then
                                                ' Start tag of ShdRefId
                                                xWriter.WriteStartElement("ShdRefId")
                                                xWriter.WriteAttributeString("RefId", dataValue)

                                                ' Keep only one ShdRefId start tag
                                                isStart2 = 0
                                            End If

                                        Case "ShdLn"
                                            ' Start tag of ShdLn
                                            xWriter.WriteStartElement("ShdLn" & dataSeq)

                                            xWriter.WriteElementString(colName2, dataValue.Replace(Chr(13), " "))

                                        Case "ShdLstUsr"
                                            xWriter.WriteElementString(colName2, dataValue.Replace(Chr(13), " "))

                                            ' Set flag to reset column index (ShareDtl)
                                            IsColEnd2 = 1

                                            ' Move to next data, row, seq
                                            dataSeq += 1

                                        Case Else
                                            xWriter.WriteElementString(colName2, dataValue.Replace(Chr(13), " "))
                                    End Select

                                    ' Move next column index (ShareDtl)
                                    If IsColEnd2 = 1 Then
                                        ' Eng tag of ShdLn
                                        xWriter.WriteEndElement()

                                        colIndex2 = 0
                                        IsColEnd2 = 0
                                    Else
                                        colIndex2 += 1
                                    End If

                                Next
                            Next

                            ' Set flag to reset column index (ShareHdr)
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index (ShareHdr)
                    If IsColEnd = 1 Then
                        If ds.Tables(7).Rows.Count > 0 Then
                            ' End tag of ShdRefId
                            xWriter.WriteEndElement()
                        End If

                        ' End tag of ShhRefId
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of ShareHdr
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - ShareHdr
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        l = Nothing
        colIndex = Nothing
        colIndex2 = Nothing
        dataValue = Nothing
        colName = Nothing
        colName2 = Nothing
        isStart = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing
        IsColEnd2 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateProfitShareHdrSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rowIndex As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isStart As Integer = 1
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - ProfitShareHdr
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of BookingHdr
            xWriter.WriteStartElement("ProfitShareHdr")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "PshRefId"
                            ' Start tag of PshRefId
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "PshEDI"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Move to next data, row, seq
                            dataSeq += 1
                            rowIndex += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        ' End tag of PshRefId
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of ProfitShareHdr
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - ProfitShareHdr
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        rowIndex = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        isStart = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateProfitShareDtlSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rowIndex As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isStart As Integer = 1
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - ProfitShareHdr
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of BookingHdr
            xWriter.WriteStartElement("ProfitShareDtl")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "PsdRefId"
                            ' Start tag of PsdRefId
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "PsdEDI"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Move to next data, row, seq
                            dataSeq += 1
                            rowIndex += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        ' End tag of PsdRefId
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of ProfitShareDtl
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - ProfitShareDtl
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        rowIndex = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        isStart = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateShipperSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""

        ' ===============================================================================
        ' Start: Shipment Details - Shipper
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Shipper
            xWriter.WriteStartElement("Shipper")

            For j = 0 To dt.Rows.Count - 1
                For i = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(j).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(j).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(i).ColumnName

                    Select Case colName
                        Case "ShpCd"
                            ' Start tag of ShpCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "ShpSAF"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' End tag of ShpCd
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))
                    End Select
                Next
            Next

            ' End tag of Shipper
            xWriter.WriteEndElement()
        End If
        ' ===============================================================================
        ' End: Shipment Details - Shipper
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        colName = Nothing
        dataValue = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateConsigneeSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter, ByVal sectionTagName As String)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""

        ' ===============================================================================
        ' Start: Shipment Details - Consignee / Notify / AMSConsignee
        ' ===============================================================================

        If dt.Rows.Count > 0 Then
            ' Start tag of Consignee / Notify / AMSConsignee
            xWriter.WriteStartElement(sectionTagName)

            For j = 0 To dt.Rows.Count - 1
                For i = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(j).Item(i).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(j).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(i).ColumnName

                    Select Case colName
                        Case "ConCd"
                            ' Start tag of ConCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "CentralRefId"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' End tag of ConCd
                            xWriter.WriteEndElement()

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))
                    End Select
                Next
            Next

            ' End tag of Consignee / Notify / AMSConsignee
            xWriter.WriteEndElement()
        End If
        ' ===============================================================================
        ' End: Shipment Details - Consignee / Notify / AMSConsignee
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        colName = Nothing
        dataValue = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateAgentSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter, Optional ByVal tagSetting As Integer = 0)
        ' Tag setting parameters
        ' 0 = Start
        ' 1 = End

        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Agent
        ' ===============================================================================
        If dt.Rows.Count > 0 Then

            If tagSetting = 0 Then
                ' Start tag of Agent
                xWriter.WriteStartElement("Agent")
            End If

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "AgtCd"
                            ' Start tag of AgtCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "AgtAgtCode"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Move to next data, row, seq
                            dataSeq += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        ' End tag of AgtCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next
        End If

        If tagSetting = 1 Then
            ' End tag of Agent
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - Agent
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateVesselSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""

        ' ===============================================================================
        ' Start: Shipment Details - Vessel
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Vessel
            xWriter.WriteStartElement("Vessel")

            For i = 0 To dt.Columns.Count - 1
                If My.Settings.DBType = 0 Then
                    ' MySQL
                    dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(10), "")
                Else
                    ' SQL Server
                    dataValue = common.NullVal(dt.Rows(0).Item(i).ToString, "").Replace(Chr(10), Chr(13))
                End If

                colName = dt.Columns(i).ColumnName

                Select Case colName
                    Case "VslRefId"
                        ' Start tag of VslRefId
                        xWriter.WriteStartElement(colName)
                        xWriter.WriteAttributeString(colName, dataValue)

                    Case "VslDelayReason2Dte"
                        xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                        ' End tag of VslRefId
                        xWriter.WriteEndElement()

                    Case Else
                        xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                End Select

            Next

            ' End tag of Vessel
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Vessel
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        colName = Nothing
        dataValue = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateCarrierSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter, Optional ByVal tagSetting As Integer = 0)
        ' Tag setting parameters
        ' 0 = Start
        ' 1 = End

        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim dataSeq As Integer = 1
        Dim IsColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Carrier
        ' ===============================================================================
        If dt.Rows.Count > 0 Then

            If tagSetting = 0 Then
                ' Start tag of Carrier
                xWriter.WriteStartElement("Carrier")
            End If

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "CarCd"
                            ' Start tag of CarCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "CarVouHdr"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Move to next data, row, seq
                            dataSeq += 1

                            ' Reset column index
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        ' End tag of CarCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next
        End If

        If tagSetting = 1 Then
            ' End tag of Agent
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - Carrier
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        dataValue = Nothing
        colName = Nothing
        dataSeq = Nothing
        IsColEnd = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Sub GenerateChargeSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Charge
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Charge
            xWriter.WriteStartElement("Charge")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "ChgCd"
                            ' Start tag of ChgCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "USChgCd"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If IsColEnd = 1 Then
                        ' End tag of ChgCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of Charge
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Charge
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateCostSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Cost
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Cost
            xWriter.WriteStartElement("Cost")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "CosCd"
                            ' Start tag of CosCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "USCosCd"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If isColEnd = 1 Then
                        ' End tag of CosCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        isColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of Cost
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Cost
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateLocationSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Location
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Location
            xWriter.WriteStartElement("Location")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "LocCd"
                            ' Start tag of LocCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "LocRamp"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If isColEnd = 1 Then
                        ' End tag of LocCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        isColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of Location
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Location
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateCtnrSizeSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - CtnrSize
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of CtnrSize
            xWriter.WriteStartElement("CtnrSize")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "CtsCd"
                            ' Start tag of CtsCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "CtsCount"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If isColEnd = 1 Then
                        ' End tag of CtsCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        isColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of CtnrSize
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - CtnrSize
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateUnitSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Unit
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Unit
            xWriter.WriteStartElement("Unit")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "UntCd"
                            ' Start tag of UntCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "UntAMS_IES"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If isColEnd = 1 Then
                        ' End tag of UntCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        isColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of Unit
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Unit
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateSalesSection(ByVal dt As DataTable, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim colIndex As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim isColEnd As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - Sales
        ' ===============================================================================
        If dt.Rows.Count > 0 Then
            ' Start tag of Sales
            xWriter.WriteStartElement("Sales")

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(dt.Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = dt.Columns(colIndex).ColumnName

                    Select Case colName
                        Case "SalCd"
                            ' Start tag of SalCd
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "SalLstUsr"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            ' Reset column index
                            isColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index
                    If isColEnd = 1 Then
                        ' End tag of SalCd
                        xWriter.WriteEndElement()

                        colIndex = 0
                        isColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of Sales
            xWriter.WriteEndElement()

        End If
        ' ===============================================================================
        ' End: Shipment Details - Sales
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        colIndex = Nothing
        colName = Nothing
        dataValue = Nothing
        isColEnd = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Sub GenerateBookingISFSection(ByVal ds As DataSet, ByVal xWriter As XmlWriter)
        Dim common As New common
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim m As Integer = 0
        Dim n As Integer = 0
        Dim colIndex As Integer = 0
        Dim colIndex2 As Integer = 0
        Dim colIndex3 As Integer = 0
        Dim dataValue As String = ""
        Dim colName As String = ""
        Dim colName2 As String = ""
        Dim colName3 As String = ""
        Dim IsColEnd As Integer = 0
        Dim IsColEnd2 As Integer = 0
        Dim IsColEnd3 As Integer = 0

        ' ===============================================================================
        ' Start: Shipment Details - BookingISFHdr
        ' ===============================================================================
        If ds.Tables(24).Rows.Count > 0 Then
            ' Start tag of BookingISFHdr
            xWriter.WriteStartElement("BookingISFHdr")

            For i = 0 To ds.Tables(25).Rows.Count - 1
                For j = 0 To ds.Tables(25).Columns.Count - 1
                    If My.Settings.DBType = 0 Then
                        ' MySQL
                        dataValue = common.NullVal(ds.Tables(25).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), "")
                    Else
                        ' SQL Server
                        dataValue = common.NullVal(ds.Tables(25).Rows(i).Item(colIndex).ToString, "").Replace(Chr(10), Chr(13))
                    End If

                    colName = ds.Tables(25).Columns(colIndex).ColumnName

                    Select Case colName
                        Case "IsfRefId"
                            ' Start tag of IsfRefId
                            xWriter.WriteStartElement(colName)
                            xWriter.WriteAttributeString(colName, dataValue)

                        Case "IsRevise"
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                            For k = 0 To ds.Tables(26).Rows.Count - 1
                                For l = 0 To ds.Tables(26).Columns.Count - 1
                                    If My.Settings.DBType = 0 Then
                                        ' MySQL
                                        dataValue = common.NullVal(ds.Tables(26).Rows(k).Item(colIndex2).ToString, "").Replace(Chr(10), "")
                                    Else
                                        ' SQL Server
                                        dataValue = common.NullVal(ds.Tables(26).Rows(k).Item(colIndex2).ToString, "").Replace(Chr(10), Chr(13))
                                    End If

                                    colName2 = ds.Tables(26).Columns(colIndex2).ColumnName

                                    Select Case colName2
                                        Case "IsmRefId"
                                            ' Start tag of BookingISFManu
                                            xWriter.WriteStartElement("BookingISFManu")

                                            ' Start tag of IsmRefId
                                            xWriter.WriteStartElement(colName2)
                                            xWriter.WriteAttributeString(colName2, dataValue)

                                        Case "IsfStuffAddr3"
                                            xWriter.WriteElementString(colName2, dataValue.Replace(Chr(13), " "))

                                            If ds.Tables(27).Rows.Count > 0 Then
                                                ' Start tag of BookingISFComm
                                                xWriter.WriteStartElement("BookingISFComm")

                                                For m = 0 To ds.Tables(27).Rows.Count - 1
                                                    For n = 0 To ds.Tables(27).Columns.Count - 1
                                                        If My.Settings.DBType = 0 Then
                                                            ' MySQL
                                                            dataValue = common.NullVal(ds.Tables(27).Rows(m).Item(colIndex3).ToString, "").Replace(Chr(10), "")
                                                        Else
                                                            ' SQL Server
                                                            dataValue = common.NullVal(ds.Tables(27).Rows(m).Item(colIndex3).ToString, "").Replace(Chr(10), Chr(13))
                                                        End If

                                                        colName3 = ds.Tables(27).Columns(colIndex3).ColumnName

                                                        Select Case colName3
                                                            Case "IscRefId"
                                                                ' Start tag of IscRefId
                                                                xWriter.WriteStartElement(colName3)
                                                                xWriter.WriteAttributeString(colName3, dataValue)

                                                            Case "IscWgt"
                                                                xWriter.WriteElementString(colName3, dataValue.Replace(Chr(13), " "))

                                                                ' Set flag to reset column index (BookingISFComm)
                                                                IsColEnd3 = 1

                                                            Case Else
                                                                xWriter.WriteElementString(colName3, dataValue.Replace(Chr(13), " "))

                                                        End Select

                                                        ' Move next column index (BookingISFComm)
                                                        If IsColEnd3 = 1 Then
                                                            ' End tag of IscRefId
                                                            xWriter.WriteEndElement()

                                                            ' End tag of BookingISFManu

                                                            colIndex3 = 0
                                                            IsColEnd3 = 0
                                                        Else
                                                            colIndex3 += 1
                                                        End If

                                                    Next
                                                Next

                                                ' End tag of BookingISFComm
                                                xWriter.WriteEndElement()
                                            End If

                                            ' Set flag to reset column index (BookingISFManu)
                                            IsColEnd2 = 1

                                        Case Else
                                            xWriter.WriteElementString(colName2, dataValue.Replace(Chr(13), " "))
                                    End Select

                                    ' Move next column index (BookingISFManu)
                                    If IsColEnd2 = 1 Then
                                        ' End tag of IsmRefId
                                        xWriter.WriteEndElement()

                                        ' End tag of BookingISFManu

                                        colIndex2 = 0
                                        IsColEnd2 = 0
                                    Else
                                        colIndex2 += 1
                                    End If

                                Next
                            Next

                            ' Set flag to reset column index (BookingISFHdr)
                            IsColEnd = 1

                        Case Else
                            xWriter.WriteElementString(colName, dataValue.Replace(Chr(13), " "))

                    End Select

                    ' Move next column index (BookingISFHdr)
                    If IsColEnd = 1 Then
                        ' End tag of IsfRefId
                        xWriter.WriteEndElement()

                        colIndex = 0
                        IsColEnd = 0
                    Else
                        colIndex += 1
                    End If

                Next
            Next

            ' End tag of BookingISFHdr
            xWriter.WriteEndElement()
        End If

        ' ===============================================================================
        ' End: Shipment Details - BookingISFHdr
        ' ===============================================================================

        ' Remove objects
        common = Nothing
        i = Nothing
        j = Nothing
        k = Nothing
        l = Nothing
        m = Nothing
        n = Nothing
        colIndex = Nothing
        colIndex2 = Nothing
        colIndex3 = Nothing
        dataValue = Nothing
        colName = Nothing
        colName2 = Nothing
        colName3 = Nothing
        IsColEnd = Nothing
        IsColEnd2 = Nothing
        IsColEnd3 = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

End Class