Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class PharmacienVentes
    Inherits UserControl

    Private dgvVentes As New DataGridView()
    Private btnNouvelleVente As New Button()

    Public Sub New()
        Me.BackColor = Color.White
        Me.Dock = DockStyle.Fill

        ' Titre
        Dim lblTitre As New Label()
        lblTitre.Text = "Gestion des ventes"
        lblTitre.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(41, 128, 185)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(30, 30)
        Me.Controls.Add(lblTitre)

        ' Bouton Nouvelle Vente
        btnNouvelleVente.Text = "➕ Nouvelle vente"
        btnNouvelleVente.BackColor = Color.FromArgb(39, 174, 96)
        btnNouvelleVente.ForeColor = Color.White
        btnNouvelleVente.FlatStyle = FlatStyle.Flat
        btnNouvelleVente.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnNouvelleVente.Size = New Size(160, 40)
        btnNouvelleVente.Location = New Point(30, 80)
        AddHandler btnNouvelleVente.Click, AddressOf OuvrirFormulaireVente
        Me.Controls.Add(btnNouvelleVente)

        ' DataGridView
        dgvVentes.Location = New Point(30, 140)
        dgvVentes.Size = New Size(900, 500)
        dgvVentes.BackgroundColor = Color.WhiteSmoke
        dgvVentes.GridColor = Color.FromArgb(236, 240, 241)
        dgvVentes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219)
        dgvVentes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvVentes.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvVentes.DefaultCellStyle.Font = New Font("Segoe UI", 10)
        dgvVentes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219)
        dgvVentes.DefaultCellStyle.SelectionForeColor = Color.White
        dgvVentes.RowHeadersVisible = False
        dgvVentes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvVentes.ReadOnly = True
        dgvVentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVentes.EnableHeadersVisualStyles = False
        Me.Controls.Add(dgvVentes)

        ' Colonnes
        dgvVentes.Columns.Add("ID", "ID")
        dgvVentes.Columns.Add("Produit", "Produit")
        dgvVentes.Columns.Add("Quantite", "Quantité")
        dgvVentes.Columns.Add("PrixUnitaire", "Prix Unitaire (FCFA)")
        dgvVentes.Columns.Add("SousTotal", "Sous-total (FCFA)")
        dgvVentes.Columns.Add("Date", "Date")

        ChargerVentes()
    End Sub

    Private Sub OuvrirFormulaireVente(sender As Object, e As EventArgs)
        Dim f As New FormNouvelleVente()
        f.ShowDialog()
        ChargerVentes() ' Actualiser après ajout
    End Sub

    Private Sub ChargerVentes()
        dgvVentes.Rows.Clear()
        Try
            Using conn = GetConnection()
                Dim cmd As New MySqlCommand("SELECT * FROM ventes ORDER BY date_vente DESC", conn)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dgvVentes.Rows.Add(reader("id"), reader("produit"), reader("quantite"), reader("prix_unitaire"), reader("sous_total"), Convert.ToDateTime(reader("date_vente")).ToString("dd/MM/yyyy HH:mm"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des ventes : " & ex.Message)
        End Try
    End Sub
End Class
