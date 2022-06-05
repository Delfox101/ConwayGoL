Public Class CPictureBox
    Inherits PictureBox

    Private Enum state
        setup
        isRunning
    End Enum

    Private myState As state

    'canvas bitmap for visualisation
    Private canvas As Bitmap
    Private canvasGraphics As Graphics

    Private map As Integer()

    Public Sub New(_width As Integer, _height As Integer)
        Me.myState = state.setup
        Me.Size = New Size(_width, _height)

        map = New Integer(_width * _height - 1) {}

        canvas = New Bitmap(_width, _height)
        canvasGraphics = Graphics.FromImage(canvas)
        canvasGraphics.FillRectangle(Brushes.White, New Rectangle(0, 0, canvas.Width, canvas.Height))
        Me.Image = canvas
    End Sub

    Private Sub CPictureBox_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If Me.myState = state.setup Then
            Dim bmp As New Bitmap(Me.Image)
            bmp.SetPixel(e.Location.X, e.Location.Y, Color.Black)
            Me.Image = bmp
        End If
    End Sub

    Public Sub StartSimulation()
        Me.myState = state.isRunning
        Me.FillMap()

        While 100
            Dim count As Integer = 0
            For x = 0 To canvas.Width - 1
                For y = 0 To canvas.Height - 1

                    For ex = -1 To 1
                        For ey = -1 To 1
                            If ex = 0 And ey = 0 Then Continue For
                            count += map(To1D(x, y))
                        Next
                    Next

                    'any live cell with fewer than 2 or more than 3 live neighbours will die
                    'any dead cell with exactly 3 neighbours will spawn

                    If map(To1D(x, y)) = 1 And (count < (1 * 2) Or count > (1 * 3)) Then
                        map(To1D(x, y)) = 0
                    Else
                        If count = (1 * 3) Then
                            map(To1D(x, y)) = 1
                        End If
                    End If

                Next
            Next

            Me.Image = CompileMap()
        End While

    End Sub

    Private Function CompileMap() As Bitmap
        Dim bmp As New Bitmap(canvas.Width, canvas.Height)
        For x = 0 To canvas.Width - 1
            For y = 0 To canvas.Height - 1
                bmp.SetPixel(x, y, If(map(To1D(x, y)) = 1, Color.Black, Color.White))
            Next
        Next
        Return bmp
    End Function

    Private Sub FillMap()
        For x = 0 To canvas.Width - 1
            For y = 0 To canvas.Height - 1
                map(To1D(x, y)) = If(canvas.GetPixel(x, y).R = 0, 1, 0)
            Next
        Next
    End Sub

    Private Function To1D(x As Integer, y As Integer)
        Return x * canvas.Width + y
    End Function
End Class