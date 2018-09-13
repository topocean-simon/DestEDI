Public Class frmOptions

    Dim cn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Origin_FTP.accdb;Persist Security Info=False;"

    Private Sub frmOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsCommon As New common

        Try
            txtSvrID.Text = My.Settings.ServerID

            If My.Settings.EDIType = 0 Then
                chkExport.Checked = True
            Else
                chkExport.Checked = False
            End If

            txtExportPath.Text = My.Settings.ExportPath
            txtExportPath11A.Text = My.Settings.ExportPath11A
            txtImportPath.Text = My.Settings.ImportPath
            txtCompletePath.Text = My.Settings.CompletePath
            txtErrorPath.Text = My.Settings.ErrorFilePath
            txtLogPath.Text = My.Settings.LogPath
            txtInterval.Text = My.Settings.TimeInterval
            txtDuration.Text = My.Settings.Duration
            txtSMTP.Text = My.Settings.SMTP
            txtTechSupport.Text = My.Settings.TechSupport

            txtServer.Text = My.Settings.Server
            txtLogin.Text = My.Settings.Login
            txtPassword.Text = My.Settings.Password
            txtDB.Text = My.Settings.DB
            txtTimeout.Text = My.Settings.Timeout
            cobDBType.SelectedIndex = My.Settings.DBType

            LoadFTP_Info()
        Catch ex As Exception
            clsCommon.SaveLog(ex.Message, "E")
        End Try

        clsCommon = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnGSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGSave.Click
        Dim clsCommon As New common

        Try
            My.Settings.ServerID = Trim(txtSvrID.Text)
            If chkExport.Checked = True Then
                My.Settings.EDIType = 0
            Else
                My.Settings.EDIType = 1
            End If
            My.Settings.ExportPath = Trim(txtExportPath.Text)
            My.Settings.ImportPath = Trim(txtImportPath.Text)
            My.Settings.CompletePath = Trim(txtCompletePath.Text)
            My.Settings.ErrorFilePath = Trim(txtErrorPath.Text)
            My.Settings.LogPath = Trim(txtLogPath.Text)
            My.Settings.TimeInterval = Trim(txtInterval.Text)
            My.Settings.Duration = Trim(txtDuration.Text)
            My.Settings.SMTP = Trim(txtSMTP.Text)
            My.Settings.TechSupport = Trim(txtTechSupport.Text)
            My.Settings.ExportPath11A = Trim(txtExportPath11A.Text)
            My.Settings.Save()

            MsgBox("General setting saved, please restart application to take effect.", MsgBoxStyle.Exclamation)

            clsCommon.SaveLog("General setting saved, please restart application to take effect.")
        Catch ex As Exception
            clsCommon.SaveLog(ex.Message, "E")
        End Try

        clsCommon = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnGCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGCancel.Click
        Dim clsCommon As New common

        Try
            txtSvrID.Text = My.Settings.ServerID

            If My.Settings.EDIType = 0 Then
                chkExport.Checked = False
            Else
                chkExport.Checked = True
            End If

            txtExportPath.Text = Replace(My.Settings.ExportPath & "\", "\\", "\")
            txtImportPath.Text = Replace(My.Settings.ImportPath & "\", "\\", "\")
            txtCompletePath.Text = Replace(My.Settings.CompletePath & "\", "\\", "\")
            txtErrorPath.Text = Replace(My.Settings.ErrorFilePath & "\", "\\", "\")
            txtLogPath.Text = Replace(My.Settings.LogPath & "\", "\\", "\")
            txtInterval.Text = My.Settings.TimeInterval
            txtDuration.Text = My.Settings.Duration
            txtSMTP.Text = My.Settings.SMTP
        Catch ex As Exception
            clsCommon.SaveLog(ex.Message, "E")
        End Try

        clsCommon = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnOSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOSave.Click
        Dim clsCommon As New common

        Try
            My.Settings.Server = txtServer.Text
            My.Settings.Login = txtLogin.Text
            My.Settings.Password = txtPassword.Text
            My.Settings.DB = txtDB.Text
            My.Settings.Timeout = txtTimeout.Text
            My.Settings.DBType = cobDBType.SelectedIndex

            My.Settings.Save()

            MsgBox("ODBC setting saved, please restart application to take effect.", MsgBoxStyle.Exclamation)

            clsCommon.SaveLog("ODBC setting saved, please restart application to take effect.")
        Catch ex As Exception
            clsCommon.SaveLog(ex.Message, "E")
        End Try

        clsCommon = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnOCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOCancel.Click
        Dim clsCommon As New common

        Try
            txtServer.Text = My.Settings.Server
            txtLogin.Text = My.Settings.Login
            txtPassword.Text = My.Settings.Password
            txtDB.Text = My.Settings.DB
            txtTimeout.Text = My.Settings.Timeout
            cobDBType.SelectedIndex = My.Settings.DBType
        Catch ex As Exception
            clsCommon.SaveLog(ex.Message, "E")
        End Try

        clsCommon = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnFTP_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFTP_Save.Click
        ' Add / Edit FTP Settings
        Dim sqlConn As New OleDb.OleDbConnection(cn)
        Dim cmd As New OleDb.OleDbCommand
        Dim sda As New OleDb.OleDbDataAdapter
        Dim sql As String = ""
        Dim common As New common

        Try
            If cobFTP_Origin.SelectedItem = "(New...)" Then
                sql = "INSERT INTO FTPSetting (FTP, IP, Port, LoginID, Pwd, RemotePath, DB_Type, DB_IP, DB_Login, DB_Pwd, DB_Name, ContactEmail) SELECT '" & Trim(txtFTP_Prefix.Text) & "', '" & Trim(txtFTP_Host.Text) & "', '" & Trim(txtFTP_Port.Text) & "', '" & Trim(txtFTP_Login.Text) & "', '" & Trim(txtFTP_Pwd.Text) & "', '" & Replace(Trim(txtFTP_RemotePath.Text) & "/", "//", "/") & "', '" & Trim(cobDB_Type.SelectedIndex) & "', '" & Trim(txtDB_Host.Text) & "', '" & Trim(txtDB_Login.Text) & "', '" & Trim(txtDB_Pwd.Text) & "', '" & Trim(txtDB_Catalog.Text) & "', '" & Trim(txtFTP_Email.Text) & "';"
            Else
                sql = "UPDATE FTPSetting SET FTP = '" & Trim(txtFTP_Prefix.Text) & "', IP = '" & Trim(txtFTP_Host.Text) & "', Port = '" & Trim(txtFTP_Port.Text) & "', LoginID = '" & Trim(txtFTP_Login.Text) & "', Pwd = '" & Trim(txtFTP_Pwd.Text) & "', RemotePath = '" & Replace(Trim(txtFTP_RemotePath.Text) & "/", "//", "/") & "', DB_Type = '" & cobDB_Type.SelectedIndex & "', DB_IP = '" & Trim(txtDB_Host.Text) & "', DB_Login = '" & Trim(txtDB_Login.Text) & "', DB_Pwd = '" & Trim(txtDB_Pwd.Text) & "', DB_Name = '" & Trim(txtDB_Catalog.Text) & "', ContactEmail = '" & Trim(txtFTP_Email.Text) & "' WHERE FTP = '" & cobFTP_Origin.SelectedItem & "';"
            End If

            sqlConn.Open()
            cmd = sqlConn.CreateCommand
            cmd.CommandTimeout = My.Settings.Timeout
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            sqlConn.Close()

            common.SaveLog("FTP Setting Saved")
            MsgBox("FTP settings saved, please restart the application to take effect.", MsgBoxStyle.Exclamation, "Setting Saved")

            ' Refresh Carrier Paths List
            LoadFTP_Info()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        ' Destroy Variables
        sql = Nothing
        sda = Nothing
        cmd = Nothing
        sqlConn = Nothing
    End Sub

    Public Sub LoadFTP_Info()
        ' Retrieve FTP Settings
        Dim sqlConn As New OleDb.OleDbConnection(cn)
        Dim cmd As New OleDb.OleDbCommand
        Dim sda As New OleDb.OleDbDataAdapter
        Dim dt As New DataTable
        Dim sql As String = "SELECT * FROM FTPSetting ORDER BY FTP"
        Dim i As Integer

        Try
            cobFTP_Origin.Items.Clear()
            cobFTP_Origin.Items.Add("")
            txtFTP_Prefix.ResetText()
            txtFTP_Host.ResetText()
            txtFTP_Port.ResetText()
            txtFTP_Login.ResetText()
            txtFTP_Pwd.ResetText()
            txtFTP_RemotePath.ResetText()
            cobDB_Type.SelectedIndex = 0
            txtDB_Host.ResetText()
            txtDB_Login.ResetText()
            txtDB_Pwd.ResetText()
            txtDB_Catalog.ResetText()
            txtFTP_Email.ResetText()

            sqlConn.Open()
            cmd = sqlConn.CreateCommand
            cmd.CommandTimeout = My.Settings.Timeout
            cmd.CommandText = sql
            sda = New OleDb.OleDbDataAdapter(cmd)
            sda.Fill(dt)

            sda.Dispose()
            cmd.Dispose()
            sqlConn.Close()

            ' Add FTP Items
            For i = 0 To dt.Rows.Count - 1
                cobFTP_Origin.Items.Add(dt.Rows(i).Item("FTP"))
            Next
            dt.Clear()

            ' Add Item for Creating New FTP Path
            cobFTP_Origin.Items.Add("(New...)")

            cobFTP_Origin.SelectedIndex = 0
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        ' Destroy Variables
        i = Nothing
        sql = Nothing
        dt = Nothing
        sda = Nothing
        cmd = Nothing
        sqlConn = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub cobFTP_Origin_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cobFTP_Origin.SelectedIndexChanged
        If cobFTP_Origin.SelectedItem = "(New...)" Then
            txtFTP_Prefix.ReadOnly = False
            txtFTP_Prefix.ResetText()
            btnFTP_Remove.Enabled = False
            txtFTP_Host.ResetText()
            txtFTP_Port.ResetText()
            txtFTP_Login.ResetText()
            txtFTP_Pwd.ResetText()
            txtFTP_RemotePath.ResetText()
            txtDB_Host.ResetText()
            cobDB_Type.SelectedIndex = 0
            txtDB_Login.ResetText()
            txtDB_Pwd.ResetText()
            txtDB_Catalog.ResetText()
            txtFTP_Email.ResetText()
        Else
            If cobFTP_Origin.SelectedItem <> "" Then
                txtFTP_Prefix.ReadOnly = True
                txtFTP_Prefix.Text = cobFTP_Origin.SelectedItem
                btnFTP_Remove.Enabled = True

                ' Retrieve Carrier Path Settings
                Dim sqlConn As New OleDb.OleDbConnection(cn)
                Dim cmd As New OleDb.OleDbCommand
                Dim sda As New OleDb.OleDbDataAdapter
                Dim dt As New DataTable
                Dim sql As String = "SELECT * FROM FTPSetting WHERE FTP = '" & cobFTP_Origin.SelectedItem & "'"
                Dim i As Integer
                Dim common As New common

                Try
                    sqlConn.Open()
                    cmd = sqlConn.CreateCommand
                    cmd.CommandTimeout = My.Settings.Timeout
                    cmd.CommandText = sql
                    sda = New OleDb.OleDbDataAdapter(cmd)
                    sda.Fill(dt)

                    sda.Dispose()
                    cmd.Dispose()
                    sqlConn.Close()

                    With dt.Rows(0)
                        txtFTP_Prefix.Text = common.NullVal(.Item("FTP"), "")
                        txtFTP_Host.Text = common.NullVal(.Item("IP"), "")
                        txtFTP_Port.Text = common.NullVal(.Item("Port"), "")
                        txtFTP_Login.Text = common.NullVal(.Item("LoginID"), "")
                        txtFTP_Pwd.Text = common.NullVal(.Item("Pwd"), "")
                        txtFTP_RemotePath.Text = common.NullVal(.Item("RemotePath"), "")
                        txtDB_Host.Text = common.NullVal(.Item("DB_IP"), "")
                        cobDB_Type.SelectedIndex = common.NullVal(.Item("DB_Type"), 0)
                        txtDB_Login.Text = common.NullVal(.Item("DB_Login"), "")
                        txtDB_Pwd.Text = common.NullVal(.Item("DB_Pwd"), "")
                        txtDB_Catalog.Text = common.NullVal(.Item("DB_Name"), "")
                        txtFTP_Email.Text = common.NullVal(.Item("ContactEmail"), "")
                    End With
                    dt.Clear()

                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
                End Try

                ' Destroy Variables
                i = Nothing
                sql = Nothing
                dt = Nothing
                sda = Nothing
                cmd = Nothing
                sqlConn = Nothing
                common = Nothing
            End If
        End If
    End Sub

    Private Sub btnFTP_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFTP_Remove.Click
        ' Delete FTP Settings
        Dim sqlConn As New OleDb.OleDbConnection(cn)
        Dim cmd As New OleDb.OleDbCommand
        Dim sda As New OleDb.OleDbDataAdapter
        Dim sql As String = ""
        Dim common As New common

        Try
            If cobFTP_Origin.SelectedItem <> "(New...)" Then
                sql = "DELETE FROM FTPSetting WHERE FTP = '" & cobFTP_Origin.SelectedItem & "';"
            End If

            sqlConn.Open()
            cmd = sqlConn.CreateCommand
            cmd.CommandTimeout = My.Settings.Timeout
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            sqlConn.Close()

            common.SaveLog("FTP Setting Saved")
            MsgBox("FTP settings saved, please restart the application to take effect.", MsgBoxStyle.Exclamation, "Setting Saved")

            ' Refresh Carrier Paths List
            LoadFTP_Info()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        ' Destroy Variables
        sql = Nothing
        sda = Nothing
        cmd = Nothing
        sqlConn = Nothing
    End Sub

    Private Sub chkExport_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkExport.CheckedChanged
        If chkExport.Checked Then
            chkExport.Checked = True
            chkImport.Checked = False
        End If
    End Sub

    Private Sub chkImport_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImport.CheckedChanged
        If chkImport.Checked Then
            chkExport.Checked = False
            chkImport.Checked = True
        End If
    End Sub


End Class