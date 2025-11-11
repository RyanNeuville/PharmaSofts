Imports System.Windows.Forms
Imports System.Drawing
Imports MySql.Data.MySqlClient

Public Class PharmacienStock
    Inherits UserControl

    Private dgv As DataGridView
    Private lblTitre As Label

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White

        ' === Titre ===
        lblTitre = New Label()
        lblTitre.Text = "Stock des Médicaments"
        lblTitre.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(44, 62, 80)
        lblTitre.Dock = DockStyle.Top
        lblTitre.Height = 45
        lblTitre.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lblTitre)

        ' === Tableau ===
        dgv = New DataGridView()
        dgv.Location = New Point(20, 60)
        dgv.Size = New Size(750, 400)
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

        ' Charger le stock au démarrage
        ChargerStock()
    End Sub

    ' === Méthode pour charger le stock depuis MySQL ===
    Private Sub ChargerStock()
        dgv.Columns.Clear()
        dgv.Rows.Clear()

        ' Colonnes identiques à MedicamentsControl
        dgv.Columns.Add("id", "ID")
        dgv.Columns.Add("nom", "Nom du Médicament")
        dgv.Columns.Add("prix", "Prix (FCFA)")
        dgv.Columns.Add("quantite", "Quantité en stock")

        Try
            Using conn As MySqlConnection = GetConnection()
                Dim cmd As New MySqlCommand("SELECT id, nom, prix, quantite FROM medicaments", conn)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dgv.Rows.Add(reader("id"), reader("nom"), reader("prix"), reader("quantite"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement du stock : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' === Méthode publique pour actualiser le tableau à tout moment ===
    Public Sub Rafraichir()
        ChargerStock()
    End Sub
End Class
