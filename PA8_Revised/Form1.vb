Public Class Form1
    Dim img As Graphics
    Dim vertexList() As Vector3D

    Dim sCenterX, sCenterY As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        img = PictureBox1.CreateGraphics
        sCenterX = PictureBox1.Width / 2
        sCenterY = PictureBox1.Height / 2
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Label1.Text = "X: " + (e.X - sCenterX).ToString
        Label2.Text = "Y: " + (-1 * (e.Y - sCenterY)).ToString
    End Sub


End Class
