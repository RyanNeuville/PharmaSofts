Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class DashboardControl
    Inherits UserControl

    ' Labels dynamiques pour mise à jour
    Private lblFournisseurs As Label
    Private lblPharmaciens As Label
    Private lblMedicaments As Label
    Private lblVentes As Label

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.WhiteSmoke

        ' === Titre ===
        Dim lblWelcome As New Label()
        lblWelcome.Text = "Bienvenue dans le tableau de bord"
        lblWelcome.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        lblWelcome.ForeColor = Color.FromArgb(52, 73, 94)
        lblWelcome.AutoSize = True
        lblWelcome.Location = New Point(40, 40)
        Me.Controls.Add(lblWelcome)

        ' === Cards alignées ===
        Dim startX As Integer = 40
        Dim startY As Integer = 120
        Dim spacingX As Integer = 220

        lblFournisseurs = CreateCard("Fournisseurs", "0", Color.FromArgb(155, 89, 182), startX, startY)
        lblPharmaciens = CreateCard("Pharmaciens", "0", Color.FromArgb(46, 204, 113), startX + spacingX, startY)
        lblMedicaments = CreateCard("Médicaments", "0", Color.FromArgb(241, 196, 15), startX + spacingX * 2, startY)
        lblVentes = CreateCard("Ventes du jour", "0", Color.FromArgb(231, 76, 60), startX + spacingX * 3, startY)

        ' Charger les données réelles depuis la base
        Rafraichir()
    End Sub

    ' === Crée une carte et retourne le label valeur pour mise à jour ===
    Private Function CreateCard(title As String, value As String, bgColor As Color, x As Integer, y As Integer) As Label
        Dim card As New Panel()
        card.BackColor = bgColor
        card.Width = 200
        card.Height = 120
        card.Left = x
        card.Top = y
        card.BorderStyle = BorderStyle.None
        card.Cursor = Cursors.Hand
        card.Padding = New Padding(10)

        Dim lblTitle As New Label()
        lblTitle.Text = title
        lblTitle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(15, 20)
        lblTitle.AutoSize = True

        Dim lblValue As New Label()
        lblValue.Text = value
        lblValue.Font = New Font("Segoe UI", 26, FontStyle.Bold)
        lblValue.ForeColor = Color.White
        lblValue.Location = New Point(15, 55)
        lblValue.AutoSize = True

        card.Controls.Add(lblTitle)
        card.Controls.Add(lblValue)
        Me.Controls.Add(card)

        Return lblValue
    End Function

    ' === Rafraîchir toutes les données dynamiquement ===
    Public Sub Rafraichir()
        Try
            Using conn As MySqlConnection = GetConnection()
                ' Fournisseurs
                Dim cmdFourn As New MySqlCommand("SELECT COUNT(*) FROM fournisseurs", conn)
                lblFournisseurs.Text = Convert.ToInt32(cmdFourn.ExecuteScalar()).ToString()

                ' Pharmaciens
                Dim cmdPharma As New MySqlCommand("SELECT COUNT(*) FROM pharmaciens", conn)
                lblPharmaciens.Text = Convert.ToInt32(cmdPharma.ExecuteScalar()).ToString()

                ' Médicaments
                Dim cmdMed As New MySqlCommand("SELECT COUNT(*) FROM medicaments", conn)
                lblMedicaments.Text = Convert.ToInt32(cmdMed.ExecuteScalar()).ToString()

                ' Ventes du jour
                Dim cmdVentes As New MySqlCommand("SELECT COUNT(*) FROM ventes WHERE DATE(date_vente) = CURDATE()", conn)
                lblVentes.Text = Convert.ToInt32(cmdVentes.ExecuteScalar()).ToString()
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des données : " & ex.Message)
        End Try
    End Sub
End Class
