Public Class Form1
    Dim CWidth As Integer = 100
    Dim CHeight As Integer = 100

    Dim t As CPictureBox

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        t = New CPictureBox(CWidth, CHeight)
        t.Location = New Point(0, 0)
        Me.Controls.Add(t)

        't.RandomInit(0.1)
        t.CircleInit(0.5)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        't.StartSimulation()

        Dim thread As New System.Threading.Thread(Sub() t.StartSimulation())
        thread.Start()
    End Sub
End Class