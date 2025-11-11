Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq ' Pour la fonction Sum()

Public Class FormNouvelleVente
    Inherits Form

    Private lblTitre As New Label()
    Private lblProduit As New Label()
    Private lblQuantite As New Label()
    Private cmbProduit As New ComboBox()
    Private txtQuantite As New NumericUpDown()
    Private btnAjouterPanier As New Button()
    Private dgvPanier As New DataGridView()
    Private lblTotal As New Label()
    Private txtTotal As New TextBox()
    Private btnValider As New Button()
    Private btnAnnuler As New Button()

    Private produits As New Dictionary(Of String, Decimal)

    Public Sub New()
        Me.Text = "Nouvelle Vente"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.White
        Me.Size = New Size(750, 600)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' ===== TITRE =====
        lblTitre.Text = "Nouvelle Vente"
        lblTitre.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(41, 128, 185)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(40, 20)
        Me.Controls.Add(lblTitre)

        ' ===== PRODUIT =====
        lblProduit.Text = "Produit :"
        lblProduit.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblProduit.Location = New Point(40, 80)
        Me.Controls.Add(lblProduit)

        cmbProduit.Font = New Font("Segoe UI", 10)
        cmbProduit.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProduit.Size = New Size(250, 30)
        cmbProduit.Location = New Point(150, 75)
        Me.Controls.Add(cmbProduit)

        ' ===== QUANTITE =====
        lblQuantite.Text = "Quantité :"
        lblQuantite.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblQuantite.Location = New Point(430, 80)
        Me.Controls.Add(lblQuantite)

        txtQuantite.Font = New Font("Segoe UI", 10)
        txtQuantite.Minimum = 1
        txtQuantite.Maximum = 1000
        txtQuantite.Value = 1
        txtQuantite.Location = New Point(530, 75)
        txtQuantite.Size = New Size(100, 30)
        Me.Controls.Add(txtQuantite)

        ' ===== BOUTON AJOUTER =====
        btnAjouterPanier.Text = "➕ Ajouter au panier"
        btnAjouterPanier.BackColor = Color.FromArgb(39, 174, 96)
        btnAjouterPanier.ForeColor = Color.White
        btnAjouterPanier.FlatStyle = FlatStyle.Flat
        btnAjouterPanier.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnAjouterPanier.Size = New Size(200, 40)
        btnAjouterPanier.Location = New Point(40, 120)
        AddHandler btnAjouterPanier.Click, AddressOf AjouterAuPanier
        Me.Controls.Add(btnAjouterPanier)

        ' ===== TABLEAU PANIER =====
        dgvPanier.Location = New Point(40, 180)
        dgvPanier.Size = New Size(660, 300)
        dgvPanier.BackgroundColor = Color.WhiteSmoke
        dgvPanier.Font = New Font("Segoe UI", 10)
        dgvPanier.RowTemplate.Height = 28
        dgvPanier.AllowUserToAddRows = False
        dgvPanier.ReadOnly = True
        dgvPanier.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185)
        dgvPanier.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvPanier.EnableHeadersVisualStyles = False
        dgvPanier.Columns.Add("Produit", "Produit")
        dgvPanier.Columns.Add("Quantite", "Quantité")
        dgvPanier.Columns.Add("PrixUnitaire", "Prix Unitaire (FCFA)")
        dgvPanier.Columns.Add("SousTotal", "Sous-total (FCFA)")
        Me.Controls.Add(dgvPanier)

        ' ===== TOTAL =====
        lblTotal.Text = "Total :"
        lblTotal.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblTotal.Location = New Point(400, 500)
        Me.Controls.Add(lblTotal)

        txtTotal.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        txtTotal.Size = New Size(180, 35)
        txtTotal.Location = New Point(470, 495)
        txtTotal.ReadOnly = True
        txtTotal.TextAlign = HorizontalAlignment.Right
        txtTotal.Text = "0"
        Me.Controls.Add(txtTotal)

        ' ===== BOUTONS =====
        btnValider.Text = "✅ Valider la vente"
        btnValider.BackColor = Color.FromArgb(41, 128, 185)
        btnValider.ForeColor = Color.White
        btnValider.FlatStyle = FlatStyle.Flat
        btnValider.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnValider.Size = New Size(200, 40)
        btnValider.Location = New Point(150, 540)
        AddHandler btnValider.Click, AddressOf ValiderVente
        Me.Controls.Add(btnValider)

        btnAnnuler.Text = "❌ Annuler"
        btnAnnuler.BackColor = Color.FromArgb(192, 57, 43)
        btnAnnuler.ForeColor = Color.White
        btnAnnuler.FlatStyle = FlatStyle.Flat
        btnAnnuler.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnAnnuler.Size = New Size(150, 40)
        btnAnnuler.Location = New Point(400, 540)
        AddHandler btnAnnuler.Click, Sub() Me.Close()
        Me.Controls.Add(btnAnnuler)

        ' Charger les produits depuis la base
        ChargerProduits()
    End Sub

    Private Sub ChargerProduits()
        cmbProduit.Items.Clear()
        produits.Clear()
        Using conn = GetConnection()
            Dim query = "SELECT nom, prix FROM medicaments"
            Dim cmd As New MySqlCommand(query, conn)
            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                Dim nom = reader("nom").ToString()
                Dim prix = Convert.ToDecimal(reader("prix"))
                produits.Add(nom, prix)
                cmbProduit.Items.Add(nom)
            End While
        End Using
    End Sub

    Private Sub AjouterAuPanier(sender As Object, e As EventArgs)
        If cmbProduit.SelectedIndex = -1 Then
            MessageBox.Show("Veuillez sélectionner un produit.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim produit As String = cmbProduit.SelectedItem.ToString()
        Dim quantite As Integer = CInt(txtQuantite.Value)
        Dim prixUnitaire As Decimal = produits(produit)
        Dim sousTotal As Decimal = prixUnitaire * quantite
        dgvPanier.Rows.Add(produit, quantite, prixUnitaire, sousTotal)

        ' 🔹 Recalcul du total
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In dgvPanier.Rows
            If row.Cells("SousTotal").Value IsNot Nothing AndAlso IsNumeric(row.Cells("SousTotal").Value) Then
                total += Convert.ToDecimal(row.Cells("SousTotal").Value)
            End If
        Next
        txtTotal.Text = total.ToString("N0")
    End Sub

    Private Sub ValiderVente(sender As Object, e As EventArgs)
        If dgvPanier.Rows.Count = 0 Then
            MessageBox.Show("Le panier est vide.")
            Return
        End If

        Using conn = GetConnection()
            For Each row As DataGridViewRow In dgvPanier.Rows
                Dim produit = row.Cells("Produit").Value.ToString()
                Dim qte = CInt(row.Cells("Quantite").Value)
                Dim prix = CDec(row.Cells("PrixUnitaire").Value)
                Dim sousTotal = CDec(row.Cells("SousTotal").Value)

                Dim cmd As New MySqlCommand("INSERT INTO ventes(produit, quantite, prix_unitaire, sous_total) VALUES (@m, @q, @p, @s)", conn)
                cmd.Parameters.AddWithValue("@m", produit)
                cmd.Parameters.AddWithValue("@q", qte)
                cmd.Parameters.AddWithValue("@p", prix)
                cmd.Parameters.AddWithValue("@s", sousTotal)
                cmd.ExecuteNonQuery()

                ' Mise à jour du stock
                Dim updateCmd As New MySqlCommand("UPDATE medicaments SET quantite = quantite - @q WHERE nom = @m", conn)
                updateCmd.Parameters.AddWithValue("@q", qte)
                updateCmd.Parameters.AddWithValue("@m", produit)
                updateCmd.ExecuteNonQuery()
            Next
        End Using

        MessageBox.Show("Vente enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
    End Sub
End Class
