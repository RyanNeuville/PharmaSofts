Imports System.Runtime.CompilerServices
Imports System.Drawing.Drawing2D

Module Extensions
    <Extension()>
    Public Sub BorderRadius(ctrl As Control, radius As Integer)
        Dim path As New GraphicsPath()
        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(ctrl.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(ctrl.Width - radius, ctrl.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, ctrl.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        ctrl.Region = New Region(path)
    End Sub

    <Extension()>
    Public Sub AddShadow(ctrl As Control)
        AddHandler ctrl.Paint, Sub(s, e)
                                   Using p As New Pen(Color.FromArgb(60, 0, 0, 0), 3)
                                       e.Graphics.DrawRectangle(p, 3, 3, ctrl.Width - 6, ctrl.Height - 6)
                                   End Using
                               End Sub
    End Sub
End Module