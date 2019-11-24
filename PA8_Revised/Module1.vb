Module Module1
    Class Point3D
        Public x, y, z As Double
        Public idx As Integer

        Public Sub New()
            idx = -1
            x = 0.0
            y = 0.0
            z = 0.0
        End Sub

        Public Sub New(_idx As Integer, _x As Double, _y As Double, _z As Double)
            idx = _idx
            x = _x
            y = _y
            z = _z
        End Sub

        Public Function ToVector() As Vector3D
            Return New Vector3D(x, y, z)
        End Function

        Public Overrides Function ToString() As String
            Return idx.ToString + " (" + x.ToString + ", " + y.ToString + ", " + z.ToString + ")"
        End Function
    End Class

    Class pVertex
        Public vIndex As Integer
        Public vNext As pVertex
    End Class

    Class Polygon3D
        Public vIndex() As Integer
        Public vCount As Integer
        Public pIndex As Integer
        Public pNormal As Vector3D

        Public Sub New()
            vIndex = Nothing
            vCount = -1
            pIndex = -1
            pNormal = Nothing
        End Sub

        Public Sub Add(_idx As Integer)
            vCount += 1
            ReDim Preserve vIndex(vCount)
            vIndex(vCount) = _idx
        End Sub

        Public Sub Iterate()
            For i = 0 To vCount
                Console.WriteLine(vIndex(i))
            Next
        End Sub

        Public Sub GetNormal(vList() As Point3D)
        End Sub

        Public Overrides Function ToString() As String
            Return "Polygon " + (pIndex).ToString
        End Function
    End Class

    Class Vector3D
        Dim x, y, z As Double

        Public Sub New()
            x = 0.0
            y = 0.0
            z = 0.0
        End Sub

        Public Sub New(_x As Double, _y As Double, _z As Double)
            x = _x
            y = _y
            z = _z
        End Sub

        Public Sub New(p As Point3D)
            Me.x = p.x
            Me.y = p.y
            Me.z = p.z
        End Sub

        Public Function DotProduct(v As Vector3D) As Double
            Return (Me.x * v.x + Me.y * v.y + Me.z * v.z)
        End Function

        Public Function GetMagnitude() As Double
            Return Math.Sqrt(DotProduct(Me))
        End Function

        Public Function Minus(v As Vector3D) As Vector3D
            Dim vOut As New Vector3D

            vOut.x = v.x - Me.x
            vOut.y = v.y - Me.y
            vOut.z = v.z - Me.z

            Return vOut
        End Function

        Public Function CrossProduct(v As Vector3D) As Vector3D
            Dim vOut As New Vector3D With {
                .x = Me.y * v.z - Me.z * v.y,
                .y = Me.z * v.x - Me.x * v.z,
                .z = Me.x * v.y - Me.y * v.x
            }

            Return vOut
        End Function

        Public Function Normalize() As Vector3D
            Dim mag As Double = Me.GetMagnitude

            If mag > 0 Then
                Me.x /= mag
                Me.y /= mag
                Me.z /= mag
            End If

            Return Me
        End Function
    End Class

    Sub SetMatrixRow(ByRef mtx(,) As Double, idx As Integer, a As Double, b As Double, c As Double, d As Double)
        mtx(idx, 0) = a
        mtx(idx, 1) = b
        mtx(idx, 2) = c
        mtx(idx, 3) = d
    End Sub

    Sub RotateObj(vlist() As Point3D, _idx As Integer, mtx(,) As Double, ByRef isrotated() As Boolean)
        Dim temp As New Point3D

        If Not isrotated(_idx) Then
            temp.x = vlist(_idx).x * mtx(0, 0) + vlist(_idx).y * mtx(1, 0) + vlist(_idx).z * mtx(2, 0)
            temp.y = vlist(_idx).x * mtx(0, 1) + vlist(_idx).y * mtx(1, 1) + vlist(_idx).z * mtx(2, 1)
            temp.z = vlist(_idx).x * mtx(0, 2) + vlist(_idx).y * mtx(1, 2) + vlist(_idx).z * mtx(2, 2)
            isrotated(_idx) = True
            vlist(_idx) = temp
        End If
    End Sub

    Sub AddVertex(ByRef vList() As Point3D, ByRef _idx As Integer, _x As Double, _y As Double, _z As Double)
        _idx += 1
        ReDim Preserve vList(_idx)
        vList(_idx) = New Point3D(_idx, _x, _y, _z)
    End Sub

    Function AddPolygon(ByRef _idx As Integer, _p0 As Integer, _p1 As Integer, _p2 As Integer)
        _idx += 1
    End Function

    Sub DrawMesh(img As Graphics, cX As Integer, cY As Integer, ByRef lBox As Object, ByRef vlist() As Point3D, _idx As Integer, _color As Color)
        img.Clear(Color.White)

        For i = 0 To lBox.Count - 1
            For j = 0 To lBox(i).vcount - 1
                img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(j)).x) + cX, -CInt(vlist(lBox(i).vIndex(j)).y) + cY, CInt(vlist(lBox(i).vIndex(j + 1)).x) + cX, -CInt(vlist(lBox(i).vIndex(j + 1)).y) + cY)
            Next
            img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).x) + cX, -CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).y) + cY, CInt(vlist(lBox(i).vIndex(0)).x) + cX, -CInt(vlist(lBox(i).vIndex(0)).y) + cY)
        Next
    End Sub
End Module
