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
            Return (idx + 1).ToString + " (" + x.ToString + ", " + y.ToString + ", " + z.ToString + ")"
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
        Public x, y, z As Double

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

    Sub ToPerspective(vlist() As Point3D, _idx As Integer, istransformed() As Boolean)
        Dim temp As New Point3D

        If Not istransformed(_idx) Then
            temp.x = vlist(_idx).x * 1 + vlist(_idx).y * 0 + vlist(_idx).z * 0 + 1 * 0
            temp.y = vlist(_idx).x * 0 + vlist(_idx).y * 1 + vlist(_idx).z * 0 + 1 * 0
            temp.z = vlist(_idx).x * 0 + vlist(_idx).y * 0 + vlist(_idx).z * 1 + 1 * -0.2
            istransformed(_idx) = True
            vlist(_idx) = temp
        End If
    End Sub

    Sub ScaleObj(vlist() As Point3D, _idx As Integer, scale As Double, ByRef isscaled() As Boolean)
        Dim temp As New Point3D

        If Not isscaled(_idx) Then
            temp.x = (vlist(_idx).x * scale) + (vlist(_idx).y * 0) + (vlist(_idx).z * 0)
            temp.y = (vlist(_idx).x * 0) + (vlist(_idx).y * scale) + (vlist(_idx).z * 0)
            temp.z = (vlist(_idx).x * 0) + (vlist(_idx).y * 0) + (vlist(_idx).z * scale)
            isscaled(_idx) = True
            vlist(_idx) = temp
        End If
    End Sub

    Sub AddVertex(ByRef vList() As Point3D, ByRef _idx As Integer, _x As Double, _y As Double, _z As Double)
        _idx += 1
        ReDim Preserve vList(_idx)
        vList(_idx) = New Point3D(_idx, _x, _y, _z)
    End Sub

    Function GetNormalVector(_p1 As Point3D, _p2 As Point3D, _p3 As Point3D)
        Dim p1 As Vector3D = _p2.ToVector.Minus(_p1.ToVector)
        Dim p2 As Vector3D = _p3.ToVector.Minus(_p2.ToVector)
        p1.Normalize()
        p2.Normalize()

        Return p1.CrossProduct(p2)
    End Function

    Function BackFaceCulling(_p0 As Point3D, _p1 As Point3D, _p2 As Point3D)
        Dim viewer As New Vector3D(0.0, 0.0, 1.0)

        If viewer.DotProduct(GetNormalVector(_p0, _p1, _p2)) > 0.0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub DrawMesh(img As Graphics, cX As Integer, cY As Integer, ByRef lBox As Object, ByRef vlist() As Point3D, _idx As Integer, _color As Color, bfCulling As Boolean)
        img.Clear(Color.White)
        Dim isTransformed(_idx) As Boolean

        For i = 0 To _idx
            isTransformed(i) = False
        Next

        For i = 0 To lBox.Count - 1
            For j = 0 To lBox(i).vcount - 1
                'ToPerspective(vlist, lBox(i).vIndex(j), isTransformed)
                If bfCulling = True Then
                    If BackFaceCulling(vlist(lBox(i).vIndex(0)), vlist(lBox(i).vIndex(1)), vlist(lBox(i).vIndex(2))) Then
                        img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(j)).x) + cX, -CInt(vlist(lBox(i).vIndex(j)).y) + cY, CInt(vlist(lBox(i).vIndex(j + 1)).x) + cX, -CInt(vlist(lBox(i).vIndex(j + 1)).y) + cY)
                    Else
                        Continue For
                    End If
                Else
                    img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(j)).x) + cX, -CInt(vlist(lBox(i).vIndex(j)).y) + cY, CInt(vlist(lBox(i).vIndex(j + 1)).x) + cX, -CInt(vlist(lBox(i).vIndex(j + 1)).y) + cY)
                End If
            Next
            If bfCulling = True Then
                If BackFaceCulling(vlist(lBox(i).vIndex(lBox(i).vcount - 2)), vlist(lBox(i).vIndex(lBox(i).vcount - 1)), vlist(lBox(i).vIndex(lBox(i).vcount))) Then
                    img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).x) + cX, -CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).y) + cY, CInt(vlist(lBox(i).vIndex(0)).x) + cX, -CInt(vlist(lBox(i).vIndex(0)).y) + cY)
                Else
                    Continue For
                End If
            Else
                img.DrawLine(New Pen(_color), CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).x) + cX, -CInt(vlist(lBox(i).vIndex(lBox(i).vcount)).y) + cY, CInt(vlist(lBox(i).vIndex(0)).x) + cX, -CInt(vlist(lBox(i).vIndex(0)).y) + cY)
            End If
        Next
        'Dim nVector As Vector3D = GetNormalVector(vlist(lBox(0).vIndex(0)), vlist(lBox(0).vIndex(1)), vlist(lBox(0).vIndex(2)))
        'img.DrawLine(New Pen(Color.Red), CInt(nVector.x) + cX, -CInt(nVector.y) + cY, CInt(nVector.x) + 50 + cX, -(CInt(nVector.y) + 50) + cY)
    End Sub
End Module
