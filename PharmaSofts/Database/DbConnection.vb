Imports MySql.Data.MySqlClient

Module DbConnection
    Public connectionString As String = "server=localhost;user id=root;password=;database=pharmasoft_db;"

    Public Function GetConnection() As MySqlConnection
        Dim conn As New MySqlConnection(connectionString)
        Try
            conn.Open()
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données : " & ex.Message,
                            "Erreur gar: ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return conn
    End Function
End Module
