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

    Class Polygon3D
        Public idx, p0, p1, p2 As Integer
        Public pNormal As Vector3D

        Public Sub New()
            idx = -1
            p0 = -1
            p1 = -1
            p2 = -1
            pNormal = New Vector3D
        End Sub

        Public Sub New(_idx As Integer, _p0 As Integer, _p1 As Integer, _p2 As Integer)
            idx = _idx
            p0 = _p0
            p1 = _p1
            p2 = _p2
        End Sub

        Public Sub GetNormal(vList() As Point3D)
            'Dim e0 As New Vector3D
            'Dim e1 As New Vector3D

            'e0 = vList(p0).ToVector.Minus(vList(p1).ToVector)
            'e1 = vList(p1).ToVector.Minus(vList(p2).ToVector)
            'TODO
            pNormal = Nothing
        End Sub

        Public Overrides Function ToString() As String
            Return idx.ToString + " (" + p0.ToString + ", " + p1.ToString + ", " + p2.ToString + ")"
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

    Sub AddVertex(ByRef vList() As Point3D, ByRef _idx As Integer, _x As Double, _y As Double, _z As Double)
        _idx += 1
        ReDim Preserve vList(_idx)
        vList(_idx) = New Point3D(_idx, _x, _y, _z)
    End Sub

    Function AddPolygon(ByRef _idx As Integer, _p0 As Integer, _p1 As Integer, _p2 As Integer)
        _idx += 1
        Return New Polygon3D(_idx, _p0, _p1, _p2)
    End Function

    Sub DrawMesh(img As Graphics, ByRef vlist() As Point3D, _idx As Integer, _color As Color)
        img.Clear(Color.White)


    End Sub
End Module
