Module Module1
    Class Point3D
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

        Public Function ToVector() As Vector3D
            Return New Vector3D(x, y, z)
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

        Public Function DotProduct(v As Vector3D) As Double
            Return (Me.x * v.x + Me.y * v.y + Me.z * v.z)
        End Function

        Public Function GetMagnitude() As Double
            Return Math.Sqrt(DotProduct(Me))
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
End Module
