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
        vIndex = -1
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

        Dim p As New Polygon3D

        For i = vIndex - 2 To vIndex
            p.Add(vList(i).idx)
            CheckedListBox1.SetItemChecked(i, True)
        Next

        pIndex += 1
        p.pIndex = pIndex
        ListBox1.Items.Add(p)
        ListBox1.SelectedIndex = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox1.Items.Count <> 0 Then
            Dim p As New Polygon3D
            Dim _idx As Integer = ListBox1.SelectedIndex

            For i = 0 To CheckedListBox1.CheckedItems.Count - 1
                p.Add(CheckedListBox1.CheckedItems(i).idx)
            Next

            p.pIndex = pIndex
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox1.Items.Insert(_idx, p)
            ListBox1.SelectedIndex = 0
        End If
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

        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.Items.Count <> 0 Then
            DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ListBox2.Items.Clear()

        If ListBox1.Items.Count <> 0 Then
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
        Else
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
        End If

        If ListBox1.SelectedIndex <> -1 Then
            For i = 0 To ListBox1.SelectedItem.vCount
                ListBox2.Items.Add(vList(ListBox1.SelectedItem.vIndex(i)))
            Next
            ListBox2.SelectedIndex = 0
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedIndex > 0 Then
            NumericUpDown16.Text = vList(ListBox2.SelectedItem.idx).x
            NumericUpDown17.Text = vList(ListBox2.SelectedItem.idx).y
            NumericUpDown18.Text = vList(ListBox2.SelectedItem.idx).z

        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count <> 0 Then
            ListBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        vList(ListBox2.SelectedItem.idx).x = CInt(NumericUpDown16.Text)
        vList(ListBox2.SelectedItem.idx).y = CInt(NumericUpDown17.Text)
        vList(ListBox2.SelectedItem.idx).z = CInt(NumericUpDown18.Text)

        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        img.Clear(Color.White)
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        'If CheckedListBox1.Items.Count <> 0 Then
        '    NumericUpDown16.Text = vList(CheckedListBox1.CheckedItems(0).idx).x
        '    NumericUpDown17.Text = vList(CheckedListBox1.CheckedItems(0).idx).y
        '    NumericUpDown18.Text = vList(CheckedListBox1.CheckedItems(0).idx).z
        'End If

        'If ListBox2.SelectedIndex <> -1 Then
        '    NumericUpDown16.Text = vList(ListBox2.SelectedItem.idx).x
        '    NumericUpDown17.Text = vList(ListBox2.SelectedItem.idx).y
        '    NumericUpDown18.Text = vList(ListBox2.SelectedItem.idx).z

        'End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim isScaled(vIndex) As Boolean

        For i = 0 To vIndex
            isScaled(i) = False
        Next

        For i = 0 To ListBox1.Items.Count - 1
            For j = 0 To ListBox1.Items(i).vCount
                ScaleObj(vList, ListBox1.Items(i).vIndex(j), 2, isScaled)
            Next
        Next

        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim isScaled(vIndex) As Boolean

        For i = 0 To vIndex
            isScaled(i) = False
        Next

        For i = 0 To ListBox1.Items.Count - 1
            For j = 0 To ListBox1.Items(i).vCount
                ScaleObj(vList, ListBox1.Items(i).vIndex(j), 0.5, isScaled)
            Next
        Next

        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ListBox2.Items.Count > 3 Then
            Dim idx As Integer = ListBox2.SelectedItem.idx
            ListBox2.Items.Remove(ListBox2.SelectedItem)

            If idx < ListBox1.SelectedItem.vCount Then
                For i = idx To ListBox1.SelectedItem.vCount - 1
                    ListBox1.SelectedItem.vIndex(i) = ListBox1.SelectedItem.vIndex(i + 1)
                Next
            End If
            ListBox1.SelectedItem.vCount -= 1

            DrawMesh(img, sCenterX, sCenterY, ListBox1.Items, vList, vIndex, imgColor, CheckBox1.Checked)
            ListBox2.SelectedIndex = 0
        End If
    End Sub
End Class
