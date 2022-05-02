Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class frmDocumentador

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Controlador: Microsoft SQL Server"
        Label2.Text = "Base de datos: " & My.Settings.BaseDatos
    End Sub

    Private Sub btnConfiguracion_Click(sender As Object, e As EventArgs) Handles btnConfiguracion.Click
        frmConfiguracion.ShowDialog()
        Label2.Text = "Base de datos: " & My.Settings.BaseDatos
    End Sub

    Private Sub btnDocumentarBD_Click(sender As Object, e As EventArgs) Handles btnDocumentarBD.Click
        Dim server As String = My.Settings.Servidor
        Dim database As String = My.Settings.BaseDatos
        Dim uid As String = My.Settings.Usuario
        Dim pwd As String = My.Settings.Contrasena
        Dim sql As String


        Dim connectionString As String = "Server=" & server & ";Database=" & database & ";Uid=" & uid & ";Pwd=" & pwd & ";"

        Dim cnn As New SqlConnection(connectionString)
        cnn.Open()

        Try
            sql = "SELECT ORDINAL_POSITION as ITEM, TABLE_NAME as NOMBRE_TABLA, COLUMN_NAME as CAMPO, IS_NULLABLE as Es_nulo, DATA_TYPE as TipoCampo, CHARACTER_MAXIMUM_LENGTH as Tamano FROM " & database & ".INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME"

            Dim DA As New SqlDataAdapter(sql, cnn)
            Dim DT As New DataTable
            DA.Fill(DT)
            Me.DataGridView1.DataSource = DT

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
        End Try
    End Sub

    Private Sub btnDatosaCSV_Click(sender As Object, e As EventArgs) Handles btnDatosaCSV.Click
        Dim str As New StringBuilder
        Dim Ruta As String = Application.StartupPath

        Dim server As String = My.Settings.Servidor
        Dim database As String = My.Settings.BaseDatos
        Dim uid As String = My.Settings.Usuario
        Dim pwd As String = My.Settings.Contrasena
        Dim sql As String

        Dim connectionString As String = "Server=" & server & ";Database=" & database & ";Uid=" & uid & ";Pwd=" & pwd & ";"

        Dim cnn As New SqlConnection(connectionString)
        cnn.Open()

        Try
            sql = "SELECT ORDINAL_POSITION as ITEM, TABLE_NAME as NOMBRE_TABLA, COLUMN_NAME as CAMPO, IS_NULLABLE as Es_nulo, DATA_TYPE as TipoCampo, CHARACTER_MAXIMUM_LENGTH as Tamano FROM " & database & ".INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME"

            Dim DA As New SqlDataAdapter(sql, cnn)
            Dim DT As New DataTable
            DA.Fill(DT)

            Dim separadorcsv As String = ";"
            Dim Encabezado As String = "ITEM" & separadorcsv & "NOMBRE_TABLA" & separadorcsv & "CAMPO" & separadorcsv & "Es_nulo" & separadorcsv & "TipoCampo" & separadorcsv & "Tamano" & separadorcsv & "Observaciones" & vbNewLine

            'Primera linea: encabezado
            str.Append(Encabezado)

            For Each dr As DataRow In DT.Rows
                For Each field As Object In dr.ItemArray
                    str.Append(field.ToString & separadorcsv)
                Next
                str.Replace(separadorcsv, vbNewLine, str.Length - 1, 1)
            Next

            Try
                My.Computer.FileSystem.WriteAllText(Ruta & "\" & database & "_documentacion.csv", str.ToString, False)
                MessageBox.Show("Listo")
            Catch ex As Exception
                MessageBox.Show("Error de escritura")
            End Try

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
        End Try




    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim cnn As SqlConnection
        Dim sql As String

        Dim server As String = My.Settings.Servidor
        Dim database As String = My.Settings.BaseDatos
        Dim uid As String = My.Settings.Usuario
        Dim pwd As String = My.Settings.Contrasena
        Dim connectionString As String = "Server=" & server & ";Database=" & database & ";Uid=" & uid & ";Pwd=" & pwd & ";"

        Dim Ruta As String = Application.StartupPath

        Dim verifica As Integer = InStr(Ruta, "\bin\Debug")

        If verifica > 0 Then
            Ruta = Mid(Ruta, 1, verifica - 1)
        End If

        'sql server: server = myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;

        TabControl1.SelectedIndex = 1

        Using CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            cnn = New SqlConnection(connectionString)
            cnn.Open()

            Try

                sql = "SELECT ORDINAL_POSITION as ITEM, TABLE_NAME as NOMBRE_TABLA, COLUMN_NAME as CAMPO, IS_NULLABLE as Es_nulo, DATA_TYPE as TipoCampo, CHARACTER_MAXIMUM_LENGTH as Tamano FROM " & database & ".INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME"

                Dim dscmd As New SqlDataAdapter(sql, cnn)
                Dim ds As New dsCampos
                dscmd.Fill(ds, "TablaCampos")

                CrReport.Load(Ruta & "\reporte.rpt")
                CrReport.SetDataSource(ds.Tables("TablaCampos"))

                CrystalReportViewer1.ReportSource = CrReport
                CrystalReportViewer1.Refresh()

            Catch ex As Exception
                MessageBox.Show(ex.Message.ToString, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
            End Try

        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CrystalReportViewer1.Dispose()
        End
    End Sub

End Class
