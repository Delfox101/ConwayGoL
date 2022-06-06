Public Class CPictureBox
    Inherits PictureBox

    Private scale = 3

    Private ForeGroundCol = Color.Brown
    Private BackGroundCol = Color.Pink

    Private Enum state
        setup
        isRunning
    End Enum

    Private myState As state

    'canvas bitmap for visualisation
    Private canvas As Bitmap
    Private canvasGraphics As Graphics
    Private CScale As Size

    Private map As Integer()

    Public Sub New(_width As Integer, _height As Integer)
        Me.myState = state.setup
        Me.Size = New Size(_width, _height) * scale
        Me.SizeMode = PictureBoxSizeMode.StretchImage

        map = New Integer(_width * _height - 1) {}

        canvas = New Bitmap(_width, _height)
        canvasGraphics = Graphics.FromImage(canvas)
        CScale = canvas.Size
        Dim b As New SolidBrush(BackGroundCol)
        canvasGraphics.FillRectangle(b, New Rectangle(0, 0, canvas.Width, canvas.Height))
        Me.Image = canvas
    End Sub

    Private Sub CPictureBox_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If Me.myState = state.setup Then
            map(To1D((e.Location.X - 1) / scale, (e.Location.Y - 1) / scale)) = 1
            Me.Image = CompileMap(map)
        End If
    End Sub

    Public Sub StartSimulation()
        Me.myState = state.isRunning
        'Me.FillMap()
        Dim oldmap() As Integer = map
        Dim newmap(canvas.Width * canvas.Height - 1) As Integer


        While True
            For x = 0 To CScale.Width - 1
                For y = 0 To CScale.Height - 1
                    Dim count As Integer = 0
                    For ex = -1 To 1
                        For ey = -1 To 1
                            If ex = 0 And ey = 0 Then Continue For
                            count += oldmap(To1D(x, y))
                        Next
                    Next

                    If oldmap(To1D(x, y)) = 1 And (count < (1 * 2) Or count > (1 * 3)) Then
                        newmap(To1D(x, y)) = 0
                        MsgBox(count.ToString)
                    Else
                        If count = (1 * 3) Then
                            newmap(To1D(x, y)) = 1
                        End If
                    End If

                    'If oldmap(To1D(x, y)) = 1 Then
                    '    newmap(To1D(x, y)) = 0
                    '    'MsgBox(x.ToString & " " & y.ToString)
                    '    If x + 1 < canvas.Width Then
                    '        newmap(To1D(x + 1, y)) = 1
                    '    End If
                    'End If

                    'If oldmap(To1D(x, y)) = 1 And To1D(x, y) + 1 < oldmap.Length Then
                    '    newmap(To1D(x, y) + 1) = 1
                    'End If


                Next
            Next

            oldmap = newmap

            'Dim t = New Bitmap(100, 100)
            'Dim tg = Graphics.FromImage(t)
            'tg.FillRectangle(Brushes.Yellow, New Rectangle(0, 0, t.Width, t.Height))
            Me.Image = CompileMap(newmap)
            System.Threading.Thread.Sleep(100)
        End While

    End Sub

    Private Function CompileMap(m As Integer()) As Bitmap
        Dim bmp As New Bitmap(CScale.Width, CScale.Height)
        For x = 0 To CScale.Width - 1
            For y = 0 To CScale.Height - 1
                bmp.SetPixel(x, y, If(m(To1D(x, y)) = 1, ForeGroundCol, BackGroundCol))
            Next
        Next
        Return bmp
    End Function

    Private Function To1D(x As Integer, y As Integer)
        Return x * CScale.Width + y
    End Function
End Class