Public Class frmMain

    Dim count As Integer = 0
    Dim startTime As Date
    Dim OfeRefId, OeiRefId, BkhRefId, AbhRefId, ImpUsr, BrhCd, SubBrhCd As Integer
    Dim ExpBrh, RcvBrh, ExpUsr, ExpDte, file, bkupFile, ImpDte As String
    Dim starter, stopper As New Timer
    Dim inProcess As Boolean = False
    Dim EDI_Remark As String

    Property EDI_Remarks()
        Get
            EDI_Remarks = Me.EDI_Remark
        End Get
        Set(ByVal value)
            Me.EDI_Remark = value
        End Set
    End Property

    Property ProcTime()
        Get
            ProcTime = Me.startTime
        End Get
        Set(ByVal value)
            Me.startTime = value
        End Set
    End Property

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim fvi As FileVersionInfo
        Dim common As New common

        Try
            NotifyIcon1.Text = Me.Text

            ' --------------------------------------------------------------------
            ' Auto start service within 10 seconds
            ' --------------------------------------------------------------------

            starter.Interval = 10 * 1000
            starter.Start()
            AddHandler starter.Tick, AddressOf btnStart_Click

            ' ====================================================================


            ' --------------------------------------------------------------------
            ' Close Application After 8 Hours
            ' Default Starting service at 00:00:00
            ' Default Stopping service at 08:00:00
            ' --------------------------------------------------------------------
            'stopper.Interval = My.Settings.Duration * 1000
            'stopper.Start()

            'AddHandler stopper.Tick, AddressOf CloseMe
            ' ====================================================================


            ' --------------------------------------------------------------------
            ' Clear Messages after an hour
            ' --------------------------------------------------------------------

            Timer3.Interval = 3600 * 1000
            Timer3.Start()

            ' ====================================================================

        Catch ex As Exception
            common.SaveLog(ex.Message, "E")

            ' --------------------------------------------------------------------
            ' Save Log
            ' --------------------------------------------------------------------
            common.showScreenMsg("Application Error, please revise the error log.")
        End Try

        ' ------------------------------------------------------------
        ' Destroy Variables
        ' ------------------------------------------------------------
        fvi = Nothing
        common = Nothing

        ' ------------------------------------------------------------
        ' Release Memory
        ' ------------------------------------------------------------
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        ' ------------------------------------------------------------
        ' Stop Timer
        ' ------------------------------------------------------------
        Timer1.Stop()
        starter.Stop()
        btnStart.Enabled = True

        Timer2.Stop()
        starter.Stop()
        btnStart.Enabled = True

        frmOptions.ShowDialog()
        frmOptions.Focus()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()

        ' ------------------------------------------------------------
        ' Release Memory
        ' ------------------------------------------------------------
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim common As New common

        Try
            ' Start main process timer
            Timer2.Interval = My.Settings.TimeInterval * 1000
            Timer2.Start()
            btnStart.Enabled = False

            ' Start timer to clear screen messages hourly
            Timer4.Interval = 1000
            Timer4.Start()

            ' Disable Auto-Start timer
            starter.Stop()

            ' Return message to screen
            common.showScreenMsg("Service started...", 1)

        Catch ex As Exception
            ' ------------------------------------------------------------
            ' Save Log
            ' ------------------------------------------------------------
            common.SaveLog(ex.Message, "E")

            common.showScreenMsg("Fail to start service, please review the error log.")
        End Try

        ' Destroy Variables
        common = Nothing

        ' ------------------------------------------------------------
        ' Release Memory
        ' ------------------------------------------------------------
        GC.Collect()
        GC.WaitForPendingFinalizers()

        ' ------------------------------------------------------------
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Me.ExitToolStripMenuItem.PerformClick()

    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        ' ---------------------------------------------------------------
        ' Show application onto Taskbar if double the icon on System Tray
        ' ---------------------------------------------------------------
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False

    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ' ------------------------------------------------------------
        ' Hide application to System Tray if minimized
        ' ------------------------------------------------------------
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            Me.Hide()
        End If

    End Sub

    Private Sub CloseMe(ByVal sender As Object, ByVal e As System.EventArgs)

        If inProcess Then
            ' ------------------------------------------------------------
            ' Reset timer if in report generating process
            ' and re-check within 30 seconds
            ' ------------------------------------------------------------
            stopper.Interval = 30 * 1000
            stopper.Start()
        Else
            ' ------------------------------------------------------------
            ' Close Application if not in process
            ' ------------------------------------------------------------
            Me.Close()
        End If

    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click

        Me.ExitToolStripMenuItem.PerformClick()

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        Dim agentEDI As New ClsAgentEDI
        Dim destEDI As New ClsDestEDI
        Dim _11A_XML As New Cls11A_XML
        Dim common As New common
        Dim clsEmail As New ClsEmail

        Try
            _11A_XML.export11A_XML()
        Catch ex As Exception
            common.showScreenMsg("Error captured from exporting 11A XML.")
            common.SaveLog("Error captured from exporting 11A XML." & Chr(13) & "Error Message:" & Chr(13) & ex.Message, "E")

            ' Send Error Email
            clsEmail.sendAckEmail("11A EDI", ex.Message, 2)
        End Try

        ' Generate Agent EDI
        Try
            agentEDI.exportAgentEDI_MGF()
        Catch ex As Exception
            common.showScreenMsg("Error captured from exporting Agent EDI (MGF).")
            common.SaveLog("Error captured from exporting Agent EDI (MGF)." & Chr(13) & "Error Message:" & Chr(13) & ex.Message, "E")

            ' Send Error Email
            clsEmail.sendAckEmail("Agent EDI", ex.Message, 2)
        End Try

        ' Generate Agent EDI (AllBridge) Suspense
        'Try
        '    agentEDI.exportAgentEDI_Allbridge()
        'Catch ex As Exception
        '    common.showScreenMsg("Error captured from exporting Agent EDI (Allbridge).")
        '    common.SaveLog("Error captured from exporting Agent EDI (Allbridge)." & Chr(13) & "Error Message:" & Chr(13) & ex.Message, "E")

        '    ' Send Error Email
        '    clsEmail.sendAckEmail("Agent EDI", ex.Message, 2)
        'End Try

        'Generate(Dest.EDI)
        Try
            destEDI.exportDestEDI()
        Catch ex As Exception
            common.showScreenMsg("Error captured from exporting Dest EDI.")
            common.SaveLog("Error captured from exporting Dest EDI." & Chr(13) & "Error Message:" & Chr(13) & ex.Message, "E")

            ' Send Error Email
            clsEmail.sendAckEmail("Dest EDI", ex.Message, 2)
        End Try

        ' Remove object
        agentEDI = Nothing
        destEDI = Nothing
        common = Nothing
        clsEmail = Nothing

        ' Release Memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick

        Dim common As New common

        If Format(Now, "mm:ss") = "00:00" Then
            ' Clear Display Messages
            Me.lstDisplay.Items.Clear()

            ' Show message to screen
            common.showScreenMsg("Clear Messages...")
        End If

        ' Remove object
        common = Nothing

        ' Release memory
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

End Class