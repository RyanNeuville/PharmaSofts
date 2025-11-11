Imports System.Windows.Forms
Imports System.Drawing
Imports MySql.Data.MySqlClient

Public Class FormMedicament
    Inherits Form

    Private lblNom, lblPrix, lblQuantite As Label
    Private txtNom, txtPrix, txtQuantite As TextBox
    Private btnEnregistrer, btnAnnuler As Button
    Private medicamentId As Integer = -1

    Public Sub New(Optional id As Integer = -1)
        Me.medicamentId = id
        Me.Text = If(id = -1, "Ajouter un Médicament", "Modifier un Médicament")
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Size = New Size(400, 300)
        Me.BackColor = Color.White
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        lblNom = New Label() With {.Text = "Nom :", .Location = New Point(30, 40), .Font = New Font("Segoe UI", 10, FontStyle.Bold)}
        txtNom = New TextBox() With {.Location = New Point(150, 40), .Width = 200}

        lblPrix = New Label() With {.Text = "Prix (FCFA) :", .Location = New Point(30, 90), .Font = New Font("Segoe UI", 10, FontStyle.Bold)}
        txtPrix = New TextBox() With {.Location = New Point(150, 90), .Width = 200}

        lblQuantite = New Label() With {.Text = "Quantité :", .Location = New Point(30, 140), .Font = New Font("Segoe UI", 10, FontStyle.Bold)}
        txtQuantite = New TextBox() With {.Location = New Point(150, 140), .Width = 200}

        btnEnregistrer = New Button() With {
            .Text = "💾 Enregistrer",
            .BackColor = Color.FromArgb(39, 174, 96),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Location = New Point(60, 200),
            .Size = New Size(120, 35)
        }
        AddHandler btnEnregistrer.Click, AddressOf Enregistrer

        btnAnnuler = New Button() With {
            .Text = "❌ Annuler",
            .BackColor = Color.FromArgb(192, 57, 43),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Location = New Point(200, 200),
            .Size = New Size(120, 35)
        }
        AddHandler btnAnnuler.Click, Sub() Me.DialogResult = DialogResult.Cancel

        Me.Controls.AddRange({lblNom, txtNom, lblPrix, txtPrix, lblQuantite, txtQuantite, btnEnregistrer, btnAnnuler})

        If id <> -1 Then ChargerInfos()
    End Sub

    Private Sub ChargerInfos()
        Try
            Using conn As MySqlConnection = GetConnection()
                Dim cmd As New MySqlCommand("SELECT * FROM medicaments WHERE id=@id", conn)
                cmd.Parameters.AddWithValue("@id", medicamentId)
                Dim reader = cmd.ExecuteReader()
                If reader.Read() Then
                    txtNom.Text = reader("nom").ToString()
                    txtPrix.Text = reader("prix").ToString()
                    txtQuantite.Text = reader("quantite").ToString()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de chargement : " & ex.Message)
        End Try
    End Sub

    Private Sub Enregistrer(sender As Object, e As EventArgs)
        Try
            Using conn As MySqlConnection = GetConnection()
                Dim cmd As MySqlCommand
                If medicamentId = -1 Then
                    cmd = New MySqlCommand("INSERT INTO medicaments (nom, prix, quantite) VALUES (@nom, @prix, @quantite)", conn)
                Else
                    cmd = New MySqlCommand("UPDATE medicaments SET nom=@nom, prix=@prix, quantite=@quantite WHERE id=@id", conn)
                    cmd.Parameters.AddWithValue("@id", medicamentId)
                End If

                cmd.Parameters.AddWithValue("@nom", txtNom.Text)
                cmd.Parameters.AddWithValue("@prix", txtPrix.Text)
                cmd.Parameters.AddWithValue("@quantite", txtQuantite.Text)
                cmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("Enregistré avec succès !")
            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub
End Class
