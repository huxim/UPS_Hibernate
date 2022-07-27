Imports System.Xml

Public Class Form1
    '全局变量==============
    Public SettingsFileName As String = My.Application.Info.DirectoryPath + "\settings.xml" '设置文件
    Public mySettings As New Settings '选项对象

    '声明通知区域静态的右键菜单
    Dim menuMain As ToolStripMenuItem = New ToolStripMenuItem
    Dim menuExit As ToolStripMenuItem = New ToolStripMenuItem


    '设置类
    Public Class Settings
        '网关IP
        Public SettingsIp As String

        Public Sub New()

        End Sub
    End Class

    '主窗体启动
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '载入设置
        LoadSettings()
        '通知区域右键菜单
        SetContextMenus()
        '设置通知区域菜单
        ContextMenuStrip1.Items.Add(menuMain)
        ContextMenuStrip1.Items.Add(menuExit)

        TextBoxIP.Text = mySettings.SettingsIp
        BackgroundWorkerPing.RunWorkerAsync()
    End Sub

    '载入设置实现
    Sub LoadSettings()
        If My.Computer.FileSystem.FileExists(SettingsFileName) = False Then
            DefaulSettings()
        End If
        '创建reader
        Dim readersettings As New XmlReaderSettings With {
            .ConformanceLevel = ConformanceLevel.Fragment,
            .IgnoreWhitespace = True,
            .IgnoreComments = True
        }
        Dim reader As XmlReader = XmlReader.Create(SettingsFileName, readersettings)
        Try
            '读取xml
            reader.ReadToFollowing("SettingsIp")
            mySettings.SettingsIp = reader.ReadElementContentAsString()
            reader.Close()
        Catch ex As Exception
            reader.Close()
        End Try
    End Sub

    '生成默认设置实现
    Sub DefaulSettings()
        '创建writer
        Dim writersettings As New XmlWriterSettings With {
            .Indent = True,
            .IndentChars = " "
        }
        Dim writer As XmlWriter = XmlWriter.Create(SettingsFileName, writersettings)
        '写入xml
        writer.WriteStartElement("Settings")
        writer.WriteElementString("SettingsIp", "192.168.1.1")
        writer.WriteEndElement()
        writer.Flush()
        writer.Close()
    End Sub

    '保存设置实现
    Sub SaveSettings()
        'Try
        'xmlwriter方式
        '创建writer
        Dim writersettings As New XmlWriterSettings With {
            .Indent = True,
            .IndentChars = " "
        }
        Dim writer As XmlWriter = XmlWriter.Create(SettingsFileName, writersettings)
        Try
            '写入xml
            writer.WriteStartElement("Settings")
            writer.WriteElementString("SettingsIp", CStr(mySettings.SettingsIp).ToLower)
            writer.WriteEndElement()
            writer.Flush()
            writer.Close()
        Catch ex As Exception
            writer.Close()
            MsgBox("保存设置出现错误,将自动创建默认设置文件." + Chr(13) & Chr(10) + "错误信息:" + ex.Message, MsgBoxStyle.Exclamation, "警告")
            If My.Computer.FileSystem.FileExists(SettingsFileName) Then
                My.Computer.FileSystem.DeleteFile(SettingsFileName)
            End If
            DefaulSettings()

        End Try
    End Sub

    '配置通知区域右键菜单
    Sub SetContextMenus()
        menuMain.Font = New Font(menuMain.Font, FontStyle.Bold)

        menuMain.Text = "显示(&M)"
        AddHandler menuMain.Click, AddressOf ShowMain

        menuExit.Text = "退出(&X)"
        AddHandler menuExit.Click, AddressOf ExitProgram
    End Sub

    '响应显示主界面
    Private Sub ShowMain(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Show()
        Me.WindowState = FormWindowState.Normal
    End Sub

    '响应通知区域退出菜单
    Private Sub ExitProgram(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    '双击通知区域图标
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        ShowMain(sender, e)
    End Sub

    '最小化隐藏
    Private Sub Main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
        End If
    End Sub

    '取消按钮
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    '确定按钮
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        mySettings.SettingsIp = TextBoxIP.Text
        SaveSettings()
        Me.Hide()
    End Sub

    Private Sub BackgroundWorkerPing_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerPing.DoWork
        Ping()
    End Sub

    'ping过程
    Sub Ping()
        While True
            Threading.Thread.Sleep(50000)
            If My.Computer.Network.Ping(mySettings.SettingsIp) Then
                '第一次ping在线,不操作
            Else
                '等待60s后再次ping
                'MsgBox("无法ping通,等待60s后再次ping.")
                Threading.Thread.Sleep(50000)
                If My.Computer.Network.Ping(mySettings.SettingsIp) Then
                    '第二次ping在线,不操作
                Else
                    '显示休眠窗体
                    FormHibernate.ShowDialog()

                End If
            End If
        End While
    End Sub

End Class
