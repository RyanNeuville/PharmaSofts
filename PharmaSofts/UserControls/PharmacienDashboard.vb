Imports System.Windows.Forms
Imports System.Drawing
Imports MySql.Data.MySqlClient

Public Class PharmacienDashboard
    Inherits UserControl

    ' --- Variables globales pour les labels de valeurs ---
    Private lblMedicamentsValue As Label
    Private lblVentesJourValue As Label
    Private lblChiffreJourValue As Label

    ' --- Chaîne de connexion MySQL ---
    Private connectionString As String = "server=localhost;user id=root;password=;database=pharmasoft_db;"

    Public Sub New()
        Me.BackColor = Color.WhiteSmoke
        Me.Dock = DockStyle.Fill

        ' === Titre ===
        Dim lblTitre As New Label()
        lblTitre.Text = "Tableau de bord - Pharmacien"
        lblTitre.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(44, 62, 80)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(40, 30)
        Me.Controls.Add(lblTitre)

        ' === Date actuelle ===
        Dim lblDate As New Label()
        lblDate.Text = "Date : " & DateTime.Now.ToString("dd/MM/yyyy")
        lblDate.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblDate.ForeColor = Color.Gray
        lblDate.Location = New Point(45, 70)
        Me.Controls.Add(lblDate)

        ' === Cards d'informations ===
        Dim startX As Integer = 60
        Dim startY As Integer = 120
        Dim spacingX As Integer = 230

        ' Création des cartes dynamiques
        CreateCard("💊 Médicaments disponibles", "0", Color.FromArgb(39, 174, 96), startX, startY, lblMedicamentsValue)
        CreateCard("🧾 Ventes du jour", "0", Color.FromArgb(230, 126, 34), startX + spacingX, startY, lblVentesJourValue)
        CreateCard("💵 Chiffre du jour", "0", Color.FromArgb(52, 152, 219), startX + spacingX * 2, startY, lblChiffreJourValue)

        ' === Bouton Actualiser ===
        Dim btnRefresh As New Button()
        btnRefresh.Text = "Actualiser"
        btnRefresh.Size = New Size(120, 35)
        btnRefresh.BackColor = Color.FromArgb(41, 128, 185)
        btnRefresh.ForeColor = Color.White
        btnRefresh.FlatStyle = FlatStyle.Flat
        btnRefresh.FlatAppearance.BorderSize = 0
        btnRefresh.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnRefresh.Location = New Point(startX + spacingX * 3, startY + 40)
        AddHandler btnRefresh.Click, AddressOf RefreshCards
        Me.Controls.Add(btnRefresh)

        ' Chargement initial des données
        RefreshCards()
    End Sub

    ' === Création d'une carte avec une référence de label ===
    Private Sub CreateCard(title As String, value As String, bgColor As Color, x As Integer, y As Integer, ByRef valueLabel As Label)
        Dim card As New Panel()
        card.BackColor = bgColor
        card.Size = New Size(200, 120)
        card.Location = New Point(x, y)
        card.BorderStyle = BorderStyle.None
        card.Padding = New Padding(15)

        Dim lblTitle As New Label()
        lblTitle.Text = title
        lblTitle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(10, 15)
        lblTitle.AutoSize = True

        valueLabel = New Label()
        valueLabel.Text = value
        valueLabel.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        valueLabel.ForeColor = Color.White
        valueLabel.Location = New Point(10, 55)
        valueLabel.AutoSize = True

        card.Controls.Add(lblTitle)
        card.Controls.Add(valueLabel)
        Me.Controls.Add(card)
    End Sub

    ' === Actualisation des cartes ===
    Private Sub RefreshCards()
        Try
            lblMedicamentsValue.Text = GetTotalMedicaments().ToString()
            lblVentesJourValue.Text = GetVentesDuJour().ToString()
            lblChiffreJourValue.Text = GetChiffreJour().ToString("N0") & " FCFA"
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement du tableau de bord : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' === Nombre total de médicaments disponibles ===
    Private Function GetTotalMedicaments() As Integer
        Dim total As Integer = 0
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM medicaments", conn)
                total = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des médicaments : " & ex.Message)
        End Try
        Return total
    End Function

    ' === Nombre de ventes du jour ===
    Private Function GetVentesDuJour() As Integer
        Dim total As Integer = 0
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM ventes WHERE DATE(date_vente) = CURDATE()", conn)
                total = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des ventes du jour : " & ex.Message)
        End Try
        Return total
    End Function

    ' === Chiffre d'affaires du jour ===
    Private Function GetChiffreJour() As Double
        Dim total As Double = 0
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim cmd As New MySqlCommand("SELECT IFNULL(SUM(sous_total), 0) FROM ventes WHERE DATE(date_vente) = CURDATE()", conn)
                total = Convert.ToDouble(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement du chiffre du jour : " & ex.Message)
        End Try
        Return total
    End Function
End Class
