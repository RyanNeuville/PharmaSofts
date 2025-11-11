Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class VentesControl
    Inherits UserControl

    Private dgv As New DataGridView()

    Public Sub New()
        Me.BackColor = Color.White
        Me.Dock = DockStyle.Fill

        ' Titre
        Dim lbl As New Label()
        lbl.Text = "Listing des ventes"
        lbl.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(52, 73, 94)
        lbl.AutoSize = True
        lbl.Location = New Point(30, 30)
        Me.Controls.Add(lbl)

        ' DataGridView
        dgv.Location = New Point(30, 80)
        dgv.Size = New Size(900, 500)
        dgv.BackgroundColor = Color.White
        dgv.GridColor = Color.FromArgb(236, 240, 241)
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 10)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowHeadersVisible = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.ReadOnly = True
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.EnableHeadersVisualStyles = False
        Me.Controls.Add(dgv)

        ' Colonnes
        dgv.Columns.Add("ID", "ID")
        dgv.Columns.Add("Produit", "Produit")
        dgv.Columns.Add("Quantite", "Quantité")
        dgv.Columns.Add("PrixUnitaire", "Prix Unitaire (FCFA)")
        dgv.Columns.Add("SousTotal", "Sous-total (FCFA)")
        dgv.Columns.Add("Date", "Date")

        ChargerVentes()
    End Sub

    Private Sub ChargerVentes()
        dgv.Rows.Clear()
        Try
            Using conn = GetConnection()
                Dim cmd As New MySqlCommand("SELECT * FROM ventes ORDER BY date_vente DESC", conn)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dgv.Rows.Add(reader("id"), reader("produit"), reader("quantite"), reader("prix_unitaire"), reader("sous_total"), Convert.ToDateTime(reader("date_vente")).ToString("dd/MM/yyyy HH:mm"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des ventes : " & ex.Message)
        End Try
    End Sub
End Class
