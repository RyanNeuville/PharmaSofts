Imports System.Windows.Forms
Imports System.Drawing
Imports MySql.Data.MySqlClient

Public Class MedicamentsControl
    Inherits UserControl

    Private dgv As DataGridView
    Private btnAdd As Button
    Private btnEdit As Button
    Private btnDelete As Button

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White

        ' === Titre ===
        Dim lbl As New Label()
        lbl.Text = "Gestion des Médicaments"
        lbl.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(44, 62, 80)
        lbl.Dock = DockStyle.Top
        lbl.Height = 45
        lbl.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lbl)

        ' === Boutons ===
        btnAdd = New Button()
        btnAdd.Text = "➕ Ajouter"
        btnAdd.BackColor = Color.FromArgb(39, 174, 96)
        btnAdd.ForeColor = Color.White
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.FlatAppearance.BorderSize = 0
        btnAdd.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnAdd.Size = New Size(120, 35)
        btnAdd.Location = New Point(20, 60)
        AddHandler btnAdd.Click, AddressOf AjouterMedicament
        Me.Controls.Add(btnAdd)

        btnEdit = New Button()
        btnEdit.Text = "✏️ Modifier"
        btnEdit.BackColor = Color.FromArgb(41, 128, 185)
        btnEdit.ForeColor = Color.White
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnEdit.Size = New Size(120, 35)
        btnEdit.Location = New Point(160, 60)
        AddHandler btnEdit.Click, AddressOf ModifierMedicament
        Me.Controls.Add(btnEdit)

        btnDelete = New Button()
        btnDelete.Text = "🗑️ Supprimer"
        btnDelete.BackColor = Color.FromArgb(192, 57, 43)
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnDelete.Size = New Size(120, 35)
        btnDelete.Location = New Point(300, 60)
        AddHandler btnDelete.Click, AddressOf SupprimerMedicament
        Me.Controls.Add(btnDelete)

        ' === Tableau ===
        dgv = New DataGridView()
        dgv.Location = New Point(20, 110)
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

        ChargerMedicaments()
    End Sub

    ' === Charger les médicaments depuis MySQL ===
    Private Sub ChargerMedicaments()
        dgv.Columns.Clear()
        dgv.Rows.Clear()

        dgv.Columns.Add("id", "ID")
        dgv.Columns.Add("nom", "Nom du Médicament")
        dgv.Columns.Add("prix", "Prix (FCFA)")
        dgv.Columns.Add("quantite", "Quantité en stock")

        Try
            Using conn As MySqlConnection = GetConnection()
                Dim cmd As New MySqlCommand("SELECT * FROM medicaments", conn)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dgv.Rows.Add(reader("id"), reader("nom"), reader("prix"), reader("quantite"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du chargement des médicaments : " & ex.Message)
        End Try
    End Sub

    ' === Ajouter un médicament ===
    Private Sub AjouterMedicament(sender As Object, e As EventArgs)
        Dim f As New FormMedicament()
        If f.ShowDialog() = DialogResult.OK Then
            ChargerMedicaments()
        End If

    End Sub

    ' === Modifier un médicament ===
    Private Sub ModifierMedicament(sender As Object, e As EventArgs)
        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Veuillez sélectionner un médicament à modifier.")
            Return
        End If

        Dim id As Integer = dgv.SelectedRows(0).Cells("id").Value
        Dim f As New FormMedicament(id)
        If f.ShowDialog() = DialogResult.OK Then
            ChargerMedicaments()
        End If
    End Sub

    ' === Supprimer un médicament ===
    Private Sub SupprimerMedicament(sender As Object, e As EventArgs)
        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Veuillez sélectionner un médicament à supprimer.")
            Return
        End If

        Dim id As Integer = dgv.SelectedRows(0).Cells("id").Value
        If MessageBox.Show("Voulez-vous vraiment supprimer ce médicament ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Using conn As MySqlConnection = GetConnection()
                    Dim cmd As New MySqlCommand("DELETE FROM medicaments WHERE id=@id", conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Médicament supprimé avec succès.")
                ChargerMedicaments()
            Catch ex As Exception
                MessageBox.Show("Erreur lors de la suppression : " & ex.Message)
            End Try
        End If
    End Sub
End Class
