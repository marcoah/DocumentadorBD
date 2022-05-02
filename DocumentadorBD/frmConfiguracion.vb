Imports System.Data.SqlClient
Public Class frmConfiguracion
    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim userAttr As New System.Configuration.UserScopedSettingAttribute
        Dim attrs As New System.ComponentModel.AttributeCollection(userAttr)
        PropertyGrid1.BrowsableAttributes = attrs

        PropertyGrid1.SelectedObject = My.Settings

    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnProbar_Click(sender As Object, e As EventArgs) Handles btnProbar.Click

        Dim cnn As SqlConnection
        Dim connectionString As String = "Server=" & My.Settings.Servidor & ";Database=" & My.Settings.BaseDatos & ";Uid=" & My.Settings.Usuario & ";Pwd=" & My.Settings.Contrasena & ";"

        cnn = New SqlConnection(connectionString)
        Try
            cnn.Open()
            cnn.Close()
            MessageBox.Show("Conexion con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
        End Try



    End Sub
End Class