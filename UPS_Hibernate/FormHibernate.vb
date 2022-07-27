Imports System.ComponentModel

Public Class FormHibernate
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BackgroundWorkerSlepp60.CancelAsync()
        BackgroundWorkerSlepp60.Dispose()
        Close()
    End Sub

    Private Sub FormHibernate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackgroundWorkerSlepp60.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorkerSlepp60_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerSlepp60.DoWork
        Threading.Thread.Sleep(60000)
        If BackgroundWorkerSlepp60.CancellationPending = True Then
            e.Cancel = True
        Else
            '
        End If
        'Application.SetSuspendState(PowerState.Suspend, True, True)
        'MsgBox("执行休眠.")
    End Sub

    Private Sub BackgroundWorkerSlepp60_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorkerSlepp60.RunWorkerCompleted
        If e.Cancelled = True Then
            '
        Else
            Application.SetSuspendState(PowerState.Hibernate, True, True)
            'MsgBox("执行休眠.")
            Close()
        End If

    End Sub
End Class