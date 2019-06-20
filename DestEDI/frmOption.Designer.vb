<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnExit = New System.Windows.Forms.Button
        Me.tabOptions = New System.Windows.Forms.TabControl
        Me.tabGnlSetting = New System.Windows.Forms.TabPage
        Me.chkStartVAT = New System.Windows.Forms.CheckBox
        Me.chkStartMGF = New System.Windows.Forms.CheckBox
        Me.chkStart11A = New System.Windows.Forms.CheckBox
        Me.chkStartUSA = New System.Windows.Forms.CheckBox
        Me.txtExportPath11A = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtSvrID = New System.Windows.Forms.TextBox
        Me.chkImport = New System.Windows.Forms.CheckBox
        Me.chkExport = New System.Windows.Forms.CheckBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.txtTechSupport = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtErrorPath = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtCompletePath = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtImportPath = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtDuration = New System.Windows.Forms.TextBox
        Me.btnGCancel = New System.Windows.Forms.Button
        Me.btnGSave = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtSMTP = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtInterval = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtLogPath = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtExportPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabODBC = New System.Windows.Forms.TabPage
        Me.cobDBType = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtDB = New System.Windows.Forms.TextBox
        Me.btnOCancel = New System.Windows.Forms.Button
        Me.btnOSave = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtTimeout = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtLogin = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.tabFTP = New System.Windows.Forms.TabPage
        Me.txtFTP_Email = New System.Windows.Forms.TextBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.txtFTP_Port = New System.Windows.Forms.TextBox
        Me.txtDB_Catalog = New System.Windows.Forms.TextBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.cobDB_Type = New System.Windows.Forms.ComboBox
        Me.txtDB_Pwd = New System.Windows.Forms.TextBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.txtDB_Login = New System.Windows.Forms.TextBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.txtDB_Host = New System.Windows.Forms.TextBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.txtFTP_RemotePath = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtFTP_Pwd = New System.Windows.Forms.TextBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.txtFTP_Login = New System.Windows.Forms.TextBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.txtFTP_Host = New System.Windows.Forms.TextBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.txtFTP_Prefix = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.btnFTP_Remove = New System.Windows.Forms.Button
        Me.btnFTP_Save = New System.Windows.Forms.Button
        Me.Label32 = New System.Windows.Forms.Label
        Me.cobFTP_Origin = New System.Windows.Forms.ComboBox
        Me.tabOptions.SuspendLayout()
        Me.tabGnlSetting.SuspendLayout()
        Me.tabODBC.SuspendLayout()
        Me.tabFTP.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(466, 453)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me.tabGnlSetting)
        Me.tabOptions.Controls.Add(Me.tabODBC)
        Me.tabOptions.Controls.Add(Me.tabFTP)
        Me.tabOptions.Location = New System.Drawing.Point(12, 12)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.SelectedIndex = 0
        Me.tabOptions.Size = New System.Drawing.Size(533, 435)
        Me.tabOptions.TabIndex = 10
        '
        'tabGnlSetting
        '
        Me.tabGnlSetting.Controls.Add(Me.chkStartVAT)
        Me.tabGnlSetting.Controls.Add(Me.chkStartMGF)
        Me.tabGnlSetting.Controls.Add(Me.chkStart11A)
        Me.tabGnlSetting.Controls.Add(Me.chkStartUSA)
        Me.tabGnlSetting.Controls.Add(Me.txtExportPath11A)
        Me.tabGnlSetting.Controls.Add(Me.Label20)
        Me.tabGnlSetting.Controls.Add(Me.Label19)
        Me.tabGnlSetting.Controls.Add(Me.txtSvrID)
        Me.tabGnlSetting.Controls.Add(Me.chkImport)
        Me.tabGnlSetting.Controls.Add(Me.chkExport)
        Me.tabGnlSetting.Controls.Add(Me.Label39)
        Me.tabGnlSetting.Controls.Add(Me.txtTechSupport)
        Me.tabGnlSetting.Controls.Add(Me.Label17)
        Me.tabGnlSetting.Controls.Add(Me.txtErrorPath)
        Me.tabGnlSetting.Controls.Add(Me.Label16)
        Me.tabGnlSetting.Controls.Add(Me.txtCompletePath)
        Me.tabGnlSetting.Controls.Add(Me.Label15)
        Me.tabGnlSetting.Controls.Add(Me.txtImportPath)
        Me.tabGnlSetting.Controls.Add(Me.Label13)
        Me.tabGnlSetting.Controls.Add(Me.Label14)
        Me.tabGnlSetting.Controls.Add(Me.txtDuration)
        Me.tabGnlSetting.Controls.Add(Me.btnGCancel)
        Me.tabGnlSetting.Controls.Add(Me.btnGSave)
        Me.tabGnlSetting.Controls.Add(Me.Label6)
        Me.tabGnlSetting.Controls.Add(Me.txtSMTP)
        Me.tabGnlSetting.Controls.Add(Me.Label5)
        Me.tabGnlSetting.Controls.Add(Me.Label4)
        Me.tabGnlSetting.Controls.Add(Me.txtInterval)
        Me.tabGnlSetting.Controls.Add(Me.Label3)
        Me.tabGnlSetting.Controls.Add(Me.txtLogPath)
        Me.tabGnlSetting.Controls.Add(Me.Label2)
        Me.tabGnlSetting.Controls.Add(Me.txtExportPath)
        Me.tabGnlSetting.Controls.Add(Me.Label1)
        Me.tabGnlSetting.Location = New System.Drawing.Point(4, 22)
        Me.tabGnlSetting.Name = "tabGnlSetting"
        Me.tabGnlSetting.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGnlSetting.Size = New System.Drawing.Size(525, 409)
        Me.tabGnlSetting.TabIndex = 0
        Me.tabGnlSetting.Text = "General"
        Me.tabGnlSetting.UseVisualStyleBackColor = True
        '
        'chkStartVAT
        '
        Me.chkStartVAT.AutoSize = True
        Me.chkStartVAT.Location = New System.Drawing.Point(346, 331)
        Me.chkStartVAT.Name = "chkStartVAT"
        Me.chkStartVAT.Size = New System.Drawing.Size(49, 17)
        Me.chkStartVAT.TabIndex = 33
        Me.chkStartVAT.Text = "VAT"
        Me.chkStartVAT.UseVisualStyleBackColor = True
        '
        'chkStartMGF
        '
        Me.chkStartMGF.AutoSize = True
        Me.chkStartMGF.Location = New System.Drawing.Point(281, 331)
        Me.chkStartMGF.Name = "chkStartMGF"
        Me.chkStartMGF.Size = New System.Drawing.Size(50, 17)
        Me.chkStartMGF.TabIndex = 32
        Me.chkStartMGF.Text = "MGF"
        Me.chkStartMGF.UseVisualStyleBackColor = True
        '
        'chkStart11A
        '
        Me.chkStart11A.AutoSize = True
        Me.chkStart11A.Location = New System.Drawing.Point(215, 331)
        Me.chkStart11A.Name = "chkStart11A"
        Me.chkStart11A.Size = New System.Drawing.Size(48, 17)
        Me.chkStart11A.TabIndex = 31
        Me.chkStart11A.Text = "11A"
        Me.chkStart11A.UseVisualStyleBackColor = True
        '
        'chkStartUSA
        '
        Me.chkStartUSA.AutoSize = True
        Me.chkStartUSA.Location = New System.Drawing.Point(146, 331)
        Me.chkStartUSA.Name = "chkStartUSA"
        Me.chkStartUSA.Size = New System.Drawing.Size(50, 17)
        Me.chkStartUSA.TabIndex = 30
        Me.chkStartUSA.Text = "USA"
        Me.chkStartUSA.UseVisualStyleBackColor = True
        '
        'txtExportPath11A
        '
        Me.txtExportPath11A.Location = New System.Drawing.Point(146, 87)
        Me.txtExportPath11A.Name = "txtExportPath11A"
        Me.txtExportPath11A.Size = New System.Drawing.Size(347, 21)
        Me.txtExportPath11A.TabIndex = 29
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(22, 90)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(99, 13)
        Me.Label20.TabIndex = 28
        Me.Label20.Text = "Export Path 11A"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(22, 15)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(64, 13)
        Me.Label19.TabIndex = 27
        Me.Label19.Text = "Server ID"
        '
        'txtSvrID
        '
        Me.txtSvrID.Location = New System.Drawing.Point(146, 6)
        Me.txtSvrID.Name = "txtSvrID"
        Me.txtSvrID.Size = New System.Drawing.Size(99, 21)
        Me.txtSvrID.TabIndex = 26
        '
        'chkImport
        '
        Me.chkImport.AutoSize = True
        Me.chkImport.Location = New System.Drawing.Point(215, 37)
        Me.chkImport.Name = "chkImport"
        Me.chkImport.Size = New System.Drawing.Size(65, 17)
        Me.chkImport.TabIndex = 25
        Me.chkImport.Text = "Import"
        Me.chkImport.UseVisualStyleBackColor = True
        '
        'chkExport
        '
        Me.chkExport.AutoSize = True
        Me.chkExport.Location = New System.Drawing.Point(146, 37)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.Size = New System.Drawing.Size(63, 17)
        Me.chkExport.TabIndex = 24
        Me.chkExport.Text = "Export"
        Me.chkExport.UseVisualStyleBackColor = True
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(22, 307)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(87, 13)
        Me.Label39.TabIndex = 23
        Me.Label39.Text = "Tech. Support"
        '
        'txtTechSupport
        '
        Me.txtTechSupport.Location = New System.Drawing.Point(146, 304)
        Me.txtTechSupport.Name = "txtTechSupport"
        Me.txtTechSupport.Size = New System.Drawing.Size(347, 21)
        Me.txtTechSupport.TabIndex = 10
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(22, 170)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(88, 13)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Error File Path"
        '
        'txtErrorPath
        '
        Me.txtErrorPath.Location = New System.Drawing.Point(146, 167)
        Me.txtErrorPath.Name = "txtErrorPath"
        Me.txtErrorPath.Size = New System.Drawing.Size(347, 21)
        Me.txtErrorPath.TabIndex = 5
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(22, 143)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(114, 13)
        Me.Label16.TabIndex = 19
        Me.Label16.Text = "Complete File Path"
        '
        'txtCompletePath
        '
        Me.txtCompletePath.Location = New System.Drawing.Point(146, 140)
        Me.txtCompletePath.Name = "txtCompletePath"
        Me.txtCompletePath.Size = New System.Drawing.Size(347, 21)
        Me.txtCompletePath.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(22, 116)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(75, 13)
        Me.Label15.TabIndex = 17
        Me.Label15.Text = "Import Path"
        '
        'txtImportPath
        '
        Me.txtImportPath.Location = New System.Drawing.Point(146, 113)
        Me.txtImportPath.Name = "txtImportPath"
        Me.txtImportPath.Size = New System.Drawing.Size(347, 21)
        Me.txtImportPath.TabIndex = 3
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(251, 257)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 13)
        Me.Label13.TabIndex = 15
        Me.Label13.Text = "(Seconds)"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(22, 252)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(122, 13)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Application Duration"
        '
        'txtDuration
        '
        Me.txtDuration.Location = New System.Drawing.Point(146, 249)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(99, 21)
        Me.txtDuration.TabIndex = 8
        '
        'btnGCancel
        '
        Me.btnGCancel.Location = New System.Drawing.Point(418, 375)
        Me.btnGCancel.Name = "btnGCancel"
        Me.btnGCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnGCancel.TabIndex = 12
        Me.btnGCancel.Text = "&Cancel"
        Me.btnGCancel.UseVisualStyleBackColor = True
        '
        'btnGSave
        '
        Me.btnGSave.Location = New System.Drawing.Point(337, 375)
        Me.btnGSave.Name = "btnGSave"
        Me.btnGSave.Size = New System.Drawing.Size(75, 23)
        Me.btnGSave.TabIndex = 11
        Me.btnGSave.Text = "&Save"
        Me.btnGSave.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(22, 280)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "SMTP Host"
        '
        'txtSMTP
        '
        Me.txtSMTP.Location = New System.Drawing.Point(146, 277)
        Me.txtSMTP.Name = "txtSMTP"
        Me.txtSMTP.Size = New System.Drawing.Size(347, 21)
        Me.txtSMTP.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(251, 227)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "(Seconds)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 227)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Timer Interval"
        '
        'txtInterval
        '
        Me.txtInterval.Location = New System.Drawing.Point(146, 222)
        Me.txtInterval.Name = "txtInterval"
        Me.txtInterval.Size = New System.Drawing.Size(99, 21)
        Me.txtInterval.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 198)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Log Path"
        '
        'txtLogPath
        '
        Me.txtLogPath.Location = New System.Drawing.Point(146, 195)
        Me.txtLogPath.Name = "txtLogPath"
        Me.txtLogPath.Size = New System.Drawing.Size(347, 21)
        Me.txtLogPath.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Export Path"
        '
        'txtExportPath
        '
        Me.txtExportPath.Location = New System.Drawing.Point(146, 60)
        Me.txtExportPath.Name = "txtExportPath"
        Me.txtExportPath.Size = New System.Drawing.Size(347, 21)
        Me.txtExportPath.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "EDI Type"
        '
        'tabODBC
        '
        Me.tabODBC.Controls.Add(Me.cobDBType)
        Me.tabODBC.Controls.Add(Me.Label18)
        Me.tabODBC.Controls.Add(Me.Label12)
        Me.tabODBC.Controls.Add(Me.txtDB)
        Me.tabODBC.Controls.Add(Me.btnOCancel)
        Me.tabODBC.Controls.Add(Me.btnOSave)
        Me.tabODBC.Controls.Add(Me.Label11)
        Me.tabODBC.Controls.Add(Me.Label10)
        Me.tabODBC.Controls.Add(Me.txtTimeout)
        Me.tabODBC.Controls.Add(Me.Label9)
        Me.tabODBC.Controls.Add(Me.txtPassword)
        Me.tabODBC.Controls.Add(Me.Label8)
        Me.tabODBC.Controls.Add(Me.txtLogin)
        Me.tabODBC.Controls.Add(Me.Label7)
        Me.tabODBC.Controls.Add(Me.txtServer)
        Me.tabODBC.Location = New System.Drawing.Point(4, 22)
        Me.tabODBC.Name = "tabODBC"
        Me.tabODBC.Padding = New System.Windows.Forms.Padding(3)
        Me.tabODBC.Size = New System.Drawing.Size(525, 409)
        Me.tabODBC.TabIndex = 1
        Me.tabODBC.Text = "ODBC"
        Me.tabODBC.UseVisualStyleBackColor = True
        '
        'cobDBType
        '
        Me.cobDBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cobDBType.FormattingEnabled = True
        Me.cobDBType.Items.AddRange(New Object() {"MySQL", "MS SQL Server"})
        Me.cobDBType.Location = New System.Drawing.Point(162, 150)
        Me.cobDBType.Name = "cobDBType"
        Me.cobDBType.Size = New System.Drawing.Size(121, 21)
        Me.cobDBType.TabIndex = 18
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(18, 153)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(93, 13)
        Me.Label18.TabIndex = 17
        Me.Label18.Text = "Database Type"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(18, 99)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Database"
        '
        'txtDB
        '
        Me.txtDB.Location = New System.Drawing.Point(162, 96)
        Me.txtDB.Name = "txtDB"
        Me.txtDB.Size = New System.Drawing.Size(347, 21)
        Me.txtDB.TabIndex = 4
        '
        'btnOCancel
        '
        Me.btnOCancel.Location = New System.Drawing.Point(434, 185)
        Me.btnOCancel.Name = "btnOCancel"
        Me.btnOCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnOCancel.TabIndex = 7
        Me.btnOCancel.Text = "&Cancel"
        Me.btnOCancel.UseVisualStyleBackColor = True
        '
        'btnOSave
        '
        Me.btnOSave.Location = New System.Drawing.Point(353, 185)
        Me.btnOSave.Name = "btnOSave"
        Me.btnOSave.Size = New System.Drawing.Size(75, 23)
        Me.btnOSave.TabIndex = 6
        Me.btnOSave.Text = "&Save"
        Me.btnOSave.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(281, 126)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(65, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "(Seconds)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(18, 126)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(121, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Connection Timeout"
        '
        'txtTimeout
        '
        Me.txtTimeout.Location = New System.Drawing.Point(162, 123)
        Me.txtTimeout.Name = "txtTimeout"
        Me.txtTimeout.Size = New System.Drawing.Size(113, 21)
        Me.txtTimeout.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 72)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Password"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(162, 69)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(347, 21)
        Me.txtPassword.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 45)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Login"
        '
        'txtLogin
        '
        Me.txtLogin.Location = New System.Drawing.Point(162, 42)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(347, 21)
        Me.txtLogin.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Database Server"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(162, 15)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(347, 21)
        Me.txtServer.TabIndex = 1
        '
        'tabFTP
        '
        Me.tabFTP.Controls.Add(Me.txtFTP_Email)
        Me.tabFTP.Controls.Add(Me.Label38)
        Me.tabFTP.Controls.Add(Me.Label37)
        Me.tabFTP.Controls.Add(Me.txtFTP_Port)
        Me.tabFTP.Controls.Add(Me.txtDB_Catalog)
        Me.tabFTP.Controls.Add(Me.Label36)
        Me.tabFTP.Controls.Add(Me.Label35)
        Me.tabFTP.Controls.Add(Me.cobDB_Type)
        Me.tabFTP.Controls.Add(Me.txtDB_Pwd)
        Me.tabFTP.Controls.Add(Me.Label31)
        Me.tabFTP.Controls.Add(Me.txtDB_Login)
        Me.tabFTP.Controls.Add(Me.Label33)
        Me.tabFTP.Controls.Add(Me.txtDB_Host)
        Me.tabFTP.Controls.Add(Me.Label34)
        Me.tabFTP.Controls.Add(Me.txtFTP_RemotePath)
        Me.tabFTP.Controls.Add(Me.Label26)
        Me.tabFTP.Controls.Add(Me.txtFTP_Pwd)
        Me.tabFTP.Controls.Add(Me.Label27)
        Me.tabFTP.Controls.Add(Me.txtFTP_Login)
        Me.tabFTP.Controls.Add(Me.Label28)
        Me.tabFTP.Controls.Add(Me.txtFTP_Host)
        Me.tabFTP.Controls.Add(Me.Label29)
        Me.tabFTP.Controls.Add(Me.txtFTP_Prefix)
        Me.tabFTP.Controls.Add(Me.Label30)
        Me.tabFTP.Controls.Add(Me.btnFTP_Remove)
        Me.tabFTP.Controls.Add(Me.btnFTP_Save)
        Me.tabFTP.Controls.Add(Me.Label32)
        Me.tabFTP.Controls.Add(Me.cobFTP_Origin)
        Me.tabFTP.Location = New System.Drawing.Point(4, 22)
        Me.tabFTP.Name = "tabFTP"
        Me.tabFTP.Size = New System.Drawing.Size(525, 409)
        Me.tabFTP.TabIndex = 2
        Me.tabFTP.Text = "FTP Servers"
        Me.tabFTP.UseVisualStyleBackColor = True
        '
        'txtFTP_Email
        '
        Me.txtFTP_Email.Location = New System.Drawing.Point(145, 312)
        Me.txtFTP_Email.MaxLength = 255
        Me.txtFTP_Email.Name = "txtFTP_Email"
        Me.txtFTP_Email.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_Email.TabIndex = 13
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(18, 315)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(38, 13)
        Me.Label38.TabIndex = 45
        Me.Label38.Text = "Email"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(407, 72)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(30, 13)
        Me.Label37.TabIndex = 43
        Me.Label37.Text = "Port"
        '
        'txtFTP_Port
        '
        Me.txtFTP_Port.Location = New System.Drawing.Point(443, 69)
        Me.txtFTP_Port.MaxLength = 5
        Me.txtFTP_Port.Name = "txtFTP_Port"
        Me.txtFTP_Port.Size = New System.Drawing.Size(59, 21)
        Me.txtFTP_Port.TabIndex = 4
        '
        'txtDB_Catalog
        '
        Me.txtDB_Catalog.Location = New System.Drawing.Point(145, 285)
        Me.txtDB_Catalog.MaxLength = 255
        Me.txtDB_Catalog.Name = "txtDB_Catalog"
        Me.txtDB_Catalog.Size = New System.Drawing.Size(249, 21)
        Me.txtDB_Catalog.TabIndex = 12
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(18, 288)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(61, 13)
        Me.Label36.TabIndex = 41
        Me.Label36.Text = "Database"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(18, 261)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(56, 13)
        Me.Label35.TabIndex = 38
        Me.Label35.Text = "DB Type"
        '
        'cobDB_Type
        '
        Me.cobDB_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cobDB_Type.FormattingEnabled = True
        Me.cobDB_Type.Items.AddRange(New Object() {"MySQL", "MSSQL"})
        Me.cobDB_Type.Location = New System.Drawing.Point(145, 258)
        Me.cobDB_Type.Name = "cobDB_Type"
        Me.cobDB_Type.Size = New System.Drawing.Size(249, 21)
        Me.cobDB_Type.TabIndex = 11
        '
        'txtDB_Pwd
        '
        Me.txtDB_Pwd.Location = New System.Drawing.Point(145, 231)
        Me.txtDB_Pwd.MaxLength = 255
        Me.txtDB_Pwd.Name = "txtDB_Pwd"
        Me.txtDB_Pwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtDB_Pwd.Size = New System.Drawing.Size(249, 21)
        Me.txtDB_Pwd.TabIndex = 10
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(18, 234)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(82, 13)
        Me.Label31.TabIndex = 37
        Me.Label31.Text = "DB Password"
        '
        'txtDB_Login
        '
        Me.txtDB_Login.Location = New System.Drawing.Point(145, 204)
        Me.txtDB_Login.MaxLength = 255
        Me.txtDB_Login.Name = "txtDB_Login"
        Me.txtDB_Login.Size = New System.Drawing.Size(249, 21)
        Me.txtDB_Login.TabIndex = 9
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(18, 207)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(76, 13)
        Me.Label33.TabIndex = 36
        Me.Label33.Text = "DB Login ID"
        '
        'txtDB_Host
        '
        Me.txtDB_Host.Location = New System.Drawing.Point(145, 177)
        Me.txtDB_Host.MaxLength = 255
        Me.txtDB_Host.Name = "txtDB_Host"
        Me.txtDB_Host.Size = New System.Drawing.Size(249, 21)
        Me.txtDB_Host.TabIndex = 8
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(18, 180)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(53, 13)
        Me.Label34.TabIndex = 35
        Me.Label34.Text = "DB Host"
        '
        'txtFTP_RemotePath
        '
        Me.txtFTP_RemotePath.Location = New System.Drawing.Point(145, 150)
        Me.txtFTP_RemotePath.MaxLength = 255
        Me.txtFTP_RemotePath.Name = "txtFTP_RemotePath"
        Me.txtFTP_RemotePath.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_RemotePath.TabIndex = 7
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(18, 153)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(80, 13)
        Me.Label26.TabIndex = 31
        Me.Label26.Text = "Remote Path"
        '
        'txtFTP_Pwd
        '
        Me.txtFTP_Pwd.Location = New System.Drawing.Point(145, 123)
        Me.txtFTP_Pwd.MaxLength = 30
        Me.txtFTP_Pwd.Name = "txtFTP_Pwd"
        Me.txtFTP_Pwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtFTP_Pwd.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_Pwd.TabIndex = 6
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(18, 126)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(85, 13)
        Me.Label27.TabIndex = 30
        Me.Label27.Text = "FTP Password"
        '
        'txtFTP_Login
        '
        Me.txtFTP_Login.Location = New System.Drawing.Point(145, 96)
        Me.txtFTP_Login.MaxLength = 30
        Me.txtFTP_Login.Name = "txtFTP_Login"
        Me.txtFTP_Login.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_Login.TabIndex = 5
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(18, 99)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(79, 13)
        Me.Label28.TabIndex = 27
        Me.Label28.Text = "FTP Login ID"
        '
        'txtFTP_Host
        '
        Me.txtFTP_Host.Location = New System.Drawing.Point(145, 69)
        Me.txtFTP_Host.MaxLength = 255
        Me.txtFTP_Host.Name = "txtFTP_Host"
        Me.txtFTP_Host.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_Host.TabIndex = 3
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(18, 72)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(56, 13)
        Me.Label29.TabIndex = 24
        Me.Label29.Text = "FTP Host"
        '
        'txtFTP_Prefix
        '
        Me.txtFTP_Prefix.Location = New System.Drawing.Point(145, 42)
        Me.txtFTP_Prefix.MaxLength = 3
        Me.txtFTP_Prefix.Name = "txtFTP_Prefix"
        Me.txtFTP_Prefix.Size = New System.Drawing.Size(249, 21)
        Me.txtFTP_Prefix.TabIndex = 2
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(18, 45)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(78, 13)
        Me.Label30.TabIndex = 22
        Me.Label30.Text = "Origin Prefix"
        '
        'btnFTP_Remove
        '
        Me.btnFTP_Remove.Location = New System.Drawing.Point(319, 339)
        Me.btnFTP_Remove.Name = "btnFTP_Remove"
        Me.btnFTP_Remove.Size = New System.Drawing.Size(75, 23)
        Me.btnFTP_Remove.TabIndex = 15
        Me.btnFTP_Remove.Text = "&Remove"
        Me.btnFTP_Remove.UseVisualStyleBackColor = True
        '
        'btnFTP_Save
        '
        Me.btnFTP_Save.Location = New System.Drawing.Point(238, 339)
        Me.btnFTP_Save.Name = "btnFTP_Save"
        Me.btnFTP_Save.Size = New System.Drawing.Size(75, 23)
        Me.btnFTP_Save.TabIndex = 14
        Me.btnFTP_Save.Text = "&Save"
        Me.btnFTP_Save.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(18, 18)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(41, 13)
        Me.Label32.TabIndex = 14
        Me.Label32.Text = "Origin"
        '
        'cobFTP_Origin
        '
        Me.cobFTP_Origin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cobFTP_Origin.FormattingEnabled = True
        Me.cobFTP_Origin.Location = New System.Drawing.Point(145, 15)
        Me.cobFTP_Origin.Name = "cobFTP_Origin"
        Me.cobFTP_Origin.Size = New System.Drawing.Size(249, 21)
        Me.cobFTP_Origin.TabIndex = 1
        '
        'frmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(560, 486)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.tabOptions)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        Me.tabOptions.ResumeLayout(False)
        Me.tabGnlSetting.ResumeLayout(False)
        Me.tabGnlSetting.PerformLayout()
        Me.tabODBC.ResumeLayout(False)
        Me.tabODBC.PerformLayout()
        Me.tabFTP.ResumeLayout(False)
        Me.tabFTP.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents tabOptions As System.Windows.Forms.TabControl
    Friend WithEvents tabGnlSetting As System.Windows.Forms.TabPage
    Friend WithEvents chkImport As System.Windows.Forms.CheckBox
    Friend WithEvents chkExport As System.Windows.Forms.CheckBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents txtTechSupport As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtErrorPath As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtCompletePath As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtImportPath As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents btnGCancel As System.Windows.Forms.Button
    Friend WithEvents btnGSave As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSMTP As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtLogPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtExportPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabODBC As System.Windows.Forms.TabPage
    Friend WithEvents cobDBType As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtDB As System.Windows.Forms.TextBox
    Friend WithEvents btnOCancel As System.Windows.Forms.Button
    Friend WithEvents btnOSave As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtTimeout As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtLogin As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents tabFTP As System.Windows.Forms.TabPage
    Friend WithEvents txtFTP_Email As System.Windows.Forms.TextBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_Port As System.Windows.Forms.TextBox
    Friend WithEvents txtDB_Catalog As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents cobDB_Type As System.Windows.Forms.ComboBox
    Friend WithEvents txtDB_Pwd As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents txtDB_Login As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents txtDB_Host As System.Windows.Forms.TextBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_RemotePath As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_Pwd As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_Login As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_Host As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtFTP_Prefix As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents btnFTP_Remove As System.Windows.Forms.Button
    Friend WithEvents btnFTP_Save As System.Windows.Forms.Button
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents cobFTP_Origin As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtSvrID As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtInterval As System.Windows.Forms.TextBox
    Friend WithEvents txtExportPath11A As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents chkStartVAT As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartMGF As System.Windows.Forms.CheckBox
    Friend WithEvents chkStart11A As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartUSA As System.Windows.Forms.CheckBox

End Class
