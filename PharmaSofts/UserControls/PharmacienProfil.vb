Imports System.Windows.Forms
Imports System.Drawing

Public Class PharmacienProfils
    Inherits UserControl

    Private lblTitre As New Label()
    Private lblNom As New Label()
    Private lblEmail As New Label()
    Private lblRole As New Label()
    Private btnModifier As New Button()

    Public Sub New()
        Me.BackColor = Color.White
        Me.Dock = DockStyle.Fill

        lblTitre.Text = "Mon profil"
        lblTitre.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(41, 128, 185)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(30, 30)
        Me.Controls.Add(lblTitre)

        lblNom.Text = "👤 Nom : Jean Dupont"
        lblNom.Font = New Font("Segoe UI", 12)
        lblNom.Location = New Point(60, 100)
        Me.Controls.Add(lblNom)

        lblEmail.Text = "✉ Email : jean.dupont@pharma.com"
        lblEmail.Font = New Font("Segoe UI", 12)
        lblEmail.Location = New Point(60, 140)
        Me.Controls.Add(lblEmail)

        lblRole.Text = "💼 Rôle : Pharmacien"
        lblRole.Font = New Font("Segoe UI", 12)
        lblRole.Location = New Point(60, 180)
        Me.Controls.Add(lblRole)

        btnModifier.Text = "Modifier mes informations"
        btnModifier.BackColor = Color.FromArgb(41, 128, 185)
        btnModifier.ForeColor = Color.White
        btnModifier.FlatStyle = FlatStyle.Flat
        btnModifier.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnModifier.Size = New Size(260, 40)
        btnModifier.Location = New Point(60, 240)
        Me.Controls.Add(btnModifier)
    End Sub
End Class