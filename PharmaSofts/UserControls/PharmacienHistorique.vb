Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class PharmacienHistorique
    Inherits UserControl

    Private dgvHistorique As New DataGridView()
    Private lblTitre As New Label()

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White

        ' ===== TITRE =====
        lblTitre.Text = "Historique des ventes"
        lblTitre.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(44, 62, 80)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(30, 30)
        Me.Controls.Add(lblTitre)

        ' ===== DATAGRIDVIEW =====
        dgvHistorique.Location = New Point(30, 80)
        dgvHistorique.Size = New Size(1000, 500)
        dgvHistorique.BackgroundColor = Color.WhiteSmoke
        dgvHistorique.GridColor = Color.LightGray
        dgvHistorique.RowHeadersVisible = False
        dgvHistorique.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvHistorique.Font = New Font("Segoe UI", 10)
        dgvHistorique.ReadOnly = True
        dgvHistorique.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' Colonnes
        dgvHistorique.Columns.Add("DateVente", "Date de vente")
        dgvHistorique.Columns.Add("Produit", "Produit")
        dgvHistorique.Columns.Add("Quantite", "Quantité")
        dgvHistorique.Columns.Add("Prix", "Prix total (FCFA)")

        Me.Controls.Add(dgvHistorique)

        ' Charger les ventes depuis la base de données
        ChargerHistorique()
    End Sub

    Private Sub ChargerHistorique()
        dgvHistorique.Rows.Clear()

        Try
            Using conn = GetConnection()
                Dim query As String = "SELECT produit, quantite, prix_unitaire, sous_total, date_vente FROM ventes ORDER BY date_vente DESC"
                Dim cmd As New MySqlCommand(query, conn)
                Dim reader = cmd.ExecuteReader()

                While reader.Read()
                    Dim dateVente As String = Convert.ToDateTime(reader("date_vente")).ToString("dd/MM/yyyy HH:mm")
                    Dim produit As String = reader("produit").ToString()
                    Dim quantite As Integer = Convert.ToInt32(reader("quantite"))
                    Dim prixTotal As Decimal = Convert.ToDecimal(reader("sous_total"))

                    dgvHistorique.Rows.Add(dateVente, produit, quantite, prixTotal.ToString("N0") & " FCFA")
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement de l'historique : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
