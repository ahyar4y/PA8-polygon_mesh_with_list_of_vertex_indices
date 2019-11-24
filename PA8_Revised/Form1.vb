Public Class Form1
    Dim img As Graphics
    Dim imgColor As Color
    Dim vList() As Point3D
    Dim vIndex As Integer
    Dim pIndex As Integer
    Dim rMatrix(3, 3) As Double
    Dim sCenterX, sCenterY As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        img = PictureBox1.CreateGraphics
        imgColor = Color.Black
        sCenterX = PictureBox1.Width / 2
        sCenterY = PictureBox1.Height / 2
        vIndex = 0
        pIndex = 0
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
        AddVertex(vList, vIndex, CDbl(NumericUpDown4.Text), CDbl(NumericUpDown5.Text), CDbl(NumericUpDown6.Text))
        AddVertex(vList, vIndex, CDbl(NumericUpDown7.Text), CDbl(NumericUpDown8.Text), CDbl(NumericUpDown9.Text))
        AddVertex(vList, vIndex, CDbl(NumericUpDown10.Text), CDbl(NumericUpDown11.Text), CDbl(NumericUpDown12.Text))

        CheckedListBox1.Items.Add(vList(vIndex - 2))
        CheckedListBox1.Items.Add(vList(vIndex - 1))
        CheckedListBox1.Items.Add(vList(vIndex))
        ListBox1.Items.Add((AddPolygon(pIndex, vList(vIndex - 2).idx, vList(vIndex - 1).idx, vList(vIndex).idx)))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim p As New Polygon3D

        For i = 0 To CheckedListBox1.CheckedItems.Count - 1
            p.Add(CheckedListBox1.CheckedItems(i).idx)
        Next

        pIndex += 1
        p.pIndex = pIndex
        ListBox1.Items.Add(p)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim dx As Integer = CInt(NumericUpDown13.Text)
        Dim dy As Integer = CInt(NumericUpDown14.Text)
        Dim dz As Integer = CInt(NumericUpDown15.Text)
        Dim isRotated(vIndex) As Boolean

        For i = 0 To vIndex
            isRotated(i) = False
        Next

        SetMatrixRow(rMatrix, 0, Math.Cos(dy * Math.PI / 180) * Math.Cos(dz * Math.PI / 180), Math.Cos(dy * Math.PI / 180) * Math.Sin(dz * Math.PI / 180), -Math.Sin(dy * Math.PI / 180), 0)
        SetMatrixRow(rMatrix, 1, Math.Sin(dx * Math.PI / 180) * Math.Sin(dy * Math.PI / 180) * Math.Cos(dz * Math.PI / 180) + Math.Cos(dx * Math.PI / 180) * -Math.Sin(dz * Math.PI / 180), Math.Sin(dx * Math.PI / 180) * Math.Sin(dy * Math.PI / 180) * Math.Sin(dz * Math.PI / 180) + Math.Cos(dx * Math.PI / 180) * Math.Cos(dz * Math.PI / 180), Math.Sin(dx * Math.PI / 180) * Math.Cos(dy * Math.PI / 180), 0)
        SetMatrixRow(rMatrix, 2, Math.Cos(dx * Math.PI / 180) * Math.Sin(dy * Math.PI / 180) * Math.Cos(dz * Math.PI / 180) + -Math.Sin(dx * Math.PI / 180) * -Math.Sin(dz * Math.PI / 180), Math.Cos(dx * Math.PI / 180) * Math.Sin(dy * Math.PI / 180) * Math.Sin(dz * Math.PI / 180) + -Math.Sin(dx * Math.PI / 180) * Math.Cos(dz * Math.PI / 180), Math.Cos(dx * Math.PI / 180) * Math.Cos(dy * Math.PI / 180), 0)
        SetMatrixRow(rMatrix, 3, 0, 0, 0, 1)

        For i = 0 To ListBox1.Items.Count - 1
            For j = 0 To ListBox1.Items(i).vCount
                RotateObj(vList, ListBox1.Items(i).vIndex(j), rMatrix, isRotated)
            Next
        Next

        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.Items.Count <> 0 Then
            DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ListBox2.Items.Clear()
        For i = 0 To ListBox1.SelectedItem.vCount
            ListBox2.Items.Add(vList(ListBox1.SelectedItem.vIndex(i)))
        Next
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged

    End Sub

    'Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
    '    If ListBox2.Items.Count <> 0 And ListBox2.Items.Count > 3 Then
    '        ListBox2.Items.Remove(ListBox2.SelectedItem)
    '        ListBox1.SelectedItem.

    '        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor)
    '    End If
    'End Sub
End Class
