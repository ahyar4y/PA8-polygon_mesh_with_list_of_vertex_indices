Public Class Form1
    Dim img As Graphics
    Dim imgColor As Color
    Dim vList() As Point3D
    Dim vIndex As Integer
    Dim pIndex As Integer

    Dim sCenterX, sCenterY As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        img = PictureBox1.CreateGraphics
        imgColor = Color.Black
        sCenterX = PictureBox1.Width / 2
        sCenterY = PictureBox1.Height / 2
        vIndex = -1
        pIndex = -1
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Label1.Text = "X: " + (e.X - sCenterX).ToString
        Label2.Text = "Y: " + (-1 * (e.Y - sCenterY)).ToString
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AddVertex(vList, vIndex, CDbl(NumericUpDown1.Text), CDbl(NumericUpDown2.Text), CDbl(NumericUpDown3.Text))

        CheckedListBox1.Items.Add(vList(vIndex))
        'Console.WriteLine(vIndex.ToString + " " + vList(vIndex).x.ToString + " " + vList(vIndex).y.ToString + " " + vList(vIndex).z.ToString)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Items.Add(AddPolygon(pIndex, CheckedListBox1.CheckedItems(0).idx, CheckedListBox1.CheckedItems(1).idx, CheckedListBox1.CheckedItems(2).idx))
        'Console.WriteLine(CheckedListBox1.CheckedItems.Count)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox1.Items.Count <> 0 Then
            DrawMesh(img, vList, vIndex, imgColor)
        End If
    End Sub
End Class
