Public Class CPictureBox
    Inherits PictureBox

    Private scale = 5

    Private ForeGroundCol = Color.MintCream

    Private BackGroundCol = Color.LightSteelBlue



    Private Enum state
        setup
        isRunning
    End Enum

    Private myState As state

    'canvas bitmap for visualisation
    Private canvas As Bitmap
    Private canvasGraphics As Graphics
    Private CScale As Size

    Private currentMap As Integer()

    Public Sub New(_width As Integer, _height As Integer)
        Me.myState = state.setup
        Me.Size = New Size(_width, _height) * scale
        Me.SizeMode = PictureBoxSizeMode.StretchImage

        currentMap = New Integer(_width * _height - 1) {}

        canvas = New Bitmap(_width, _height)
        canvasGraphics = Graphics.FromImage(canvas)
        CScale = canvas.Size
        Dim b As New SolidBrush(BackGroundCol)
        canvasGraphics.FillRectangle(b, New Rectangle(0, 0, canvas.Width, canvas.Height))
        Me.Image = canvas
    End Sub

    Private Sub CPictureBox_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.Click
        If Me.myState = state.setup Then
            'MsgBox(currentMap(To1D((e.Location.X - 1) / scale, (e.Location.Y - 1) / scale)))
            currentMap(To1D((e.Location.X - 1) / scale, (e.Location.Y - 1) / scale)) = 1
            Me.Image = CompileMap(currentMap)
        End If
    End Sub

    Public Sub RandomInit(chance As Decimal)
        Dim random As New Random
        For i = 0 To currentMap.Length - 1
            If currentMap(i) = 1 Then Continue For
            If random.NextDouble <= chance Then
                currentMap(i) = 1
            End If
        Next
        Me.Image = CompileMap(currentMap)
    End Sub

    Public Sub StartSimulation()
        Me.myState = state.isRunning

        Dim newmap(canvas.Width * canvas.Height - 1) As Integer

        While True
            For x = 0 To CScale.Width - 1
                For y = 0 To CScale.Height - 1
                    Dim numLiveNeighbours As Integer = 0

                    'gets number of live neighbours around a current cell
                    For ex = -1 To 1
                        For ey = -1 To 1
                            If (ex = 0 And ey = 0) Or x + ex < 0 Or x + ex >= CScale.Width Or y + ey < 0 Or y + ey >= CScale.Height Then
                                Continue For
                            Else
                                numLiveNeighbours += currentMap(To1D(x + ex, y + ey))
                            End If
                        Next
                    Next

                    'rules

                    If currentMap(To1D(x, y)) = 1 And numLiveNeighbours < 2 Then
                        newmap(To1D(x, y)) = 0
                    ElseIf currentMap(To1D(x, y)) = 1 And numLiveNeighbours > 3 Then
                        newmap(To1D(x, y)) = 0
                    ElseIf currentMap(To1D(x, y)) = 0 And numLiveNeighbours = 3 Then
                        newmap(To1D(x, y)) = 1
                    End If

                Next
            Next

            currentMap = newmap
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