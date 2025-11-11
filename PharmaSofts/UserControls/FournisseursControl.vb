Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class FournisseursControl
    Inherits UserControl

    Private dgv As New DataGridView()
    Private btnAjouter As New Button()
    Private btnModifier As New Button()
    Private btnSupprimer As New Button()

    Public Sub New()
        Me.BackColor = Color.White
        Me.Dock = DockStyle.Fill

        Dim lbl As New Label()
        lbl.Text = "Liste des Fournisseurs"
        lbl.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(52, 73, 94)
        lbl.Dock = DockStyle.Top
        lbl.Height = 50
        lbl.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lbl)

        ' ===== BOUTONS EN HAUT =====
        btnAjouter.Text = "➕ Ajouter"
        btnAjouter.BackColor = Color.FromArgb(39, 174, 96)
        btnAjouter.ForeColor = Color.White
        btnAjouter.FlatStyle = FlatStyle.Flat
        btnAjouter.Location = New Point(20, 60)
        btnAjouter.Size = New Size(120, 35)
        AddHandler btnAjouter.Click, AddressOf AjouterFournisseur
        Me.Controls.Add(btnAjouter)

        btnModifier.Text = "✏️ Modifier"
        btnModifier.BackColor = Color.FromArgb(52, 152, 219)
        btnModifier.ForeColor = Color.White
        btnModifier.FlatStyle = FlatStyle.Flat
        btnModifier.Location = New Point(160, 60)
        btnModifier.Size = New Size(120, 35)
        AddHandler btnModifier.Click, AddressOf ModifierFournisseur
        Me.Controls.Add(btnModifier)

        btnSupprimer.Text = "❌ Supprimer"
        btnSupprimer.BackColor = Color.FromArgb(192, 57, 43)
        btnSupprimer.ForeColor = Color.White
        btnSupprimer.FlatStyle = FlatStyle.Flat
        btnSupprimer.Location = New Point(300, 60)
        btnSupprimer.Size = New Size(120, 35)
        AddHandler btnSupprimer.Click, AddressOf SupprimerFournisseur
        Me.Controls.Add(btnSupprimer)

        ' ===== DATAGRIDVIEW =====
        dgv.Location = New Point(20, 110)
        dgv.Size = New Size(820, 450)
        dgv.BackgroundColor = Color.WhiteSmoke
        dgv.GridColor = Color.LightGray
        dgv.RowHeadersVisible = False
        dgv.EnableHeadersVisualStyles = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True

        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 188, 156)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

        ' Colonnes avec largeur ajustée
        dgv.Columns.Add("ID", "ID") : dgv.Columns("ID").Width = 50
        dgv.Columns.Add("Nom", "Nom") : dgv.Columns("Nom").Width = 200
        dgv.Columns.Add("Email", "Email") : dgv.Columns("Email").Width = 250
        dgv.Columns.Add("Telephone", "Téléphone") : dgv.Columns("Telephone").Width = 150

        Me.Controls.Add(dgv)

        ChargerFournisseurs()
    End Sub

    Private Sub ChargerFournisseurs()
        dgv.Rows.Clear()
        Try
            Using conn = GetConnection()
                Dim cmd As New MySqlCommand("SELECT id, nom, email, telephone FROM fournisseurs", conn)
                Dim reader = cmd.ExecuteReader()
                While reader.Read()
                    dgv.Rows.Add(reader("id"), reader("nom"), reader("email"), reader("telephone"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    Private Sub AjouterFournisseur(sender As Object, e As EventArgs)
        Dim frm As New FormFournisseur()
        frm.ShowDialog()
        ChargerFournisseurs()
    End Sub

    Private Sub ModifierFournisseur(sender As Object, e As EventArgs)
        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Veuillez sélectionner un fournisseur.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim id As Integer = CInt(dgv.SelectedRows(0).Cells("ID").Value)
        Dim nom As String = dgv.SelectedRows(0).Cells("Nom").Value.ToString()
        Dim email As String = dgv.SelectedRows(0).Cells("Email").Value.ToString()
        Dim tel As String = dgv.SelectedRows(0).Cells("Telephone").Value.ToString()

        Dim frm As New FormFournisseur(id, nom, email, tel)
        frm.ShowDialog()
        ChargerFournisseurs()
    End Sub

    Private Sub SupprimerFournisseur(sender As Object, e As EventArgs)
        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Veuillez sélectionner un fournisseur.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Voulez-vous vraiment supprimer ce fournisseur ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Dim id As Integer = CInt(dgv.SelectedRows(0).Cells("ID").Value)
                Using conn = GetConnection()
                    Dim cmd As New MySqlCommand("DELETE FROM fournisseurs WHERE id=@id", conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                End Using
                ChargerFournisseurs()
            Catch ex As Exception
                MessageBox.Show("Erreur : " & ex.Message)
            End Try
        End If
    End Sub
End Class
