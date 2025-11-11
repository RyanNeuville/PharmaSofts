Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class FormLogin
    Inherits Form

    Private lblTitre As New Label()
    Private lblUtilisateur As New Label()
    Private lblMotDePasse As New Label()
    Private cmbRole As New ComboBox()
    Private txtUtilisateur As New TextBox()
    Private txtMotDePasse As New TextBox()
    Private btnConnexion As New Button()

    Public Sub New()
        Me.Text = "Connexion - PharmaSoft"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.White
        Me.Size = New Size(420, 370)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' ===== TITRE =====
        lblTitre.Text = "Connexion"
        lblTitre.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(41, 128, 185)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(140, 35)
        Me.Controls.Add(lblTitre)

        ' ===== UTILISATEUR =====
        lblUtilisateur.Text = "Nom d'utilisateur :"
        lblUtilisateur.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblUtilisateur.Location = New Point(40, 100)
        lblUtilisateur.AutoSize = True
        Me.Controls.Add(lblUtilisateur)

        txtUtilisateur.Font = New Font("Segoe UI", 10)
        txtUtilisateur.Location = New Point(190, 95)
        txtUtilisateur.Size = New Size(160, 25)
        Me.Controls.Add(txtUtilisateur)

        ' ===== MOT DE PASSE =====
        lblMotDePasse.Text = "Mot de passe :"
        lblMotDePasse.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblMotDePasse.Location = New Point(40, 140)
        lblMotDePasse.AutoSize = True
        Me.Controls.Add(lblMotDePasse)

        txtMotDePasse.Font = New Font("Segoe UI", 10)
        txtMotDePasse.Location = New Point(190, 135)
        txtMotDePasse.Size = New Size(160, 25)
        txtMotDePasse.PasswordChar = "*"c
        Me.Controls.Add(txtMotDePasse)

        ' ===== ROLE =====
        Dim lblRole As New Label()
        lblRole.Text = "Rôle :"
        lblRole.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblRole.Location = New Point(40, 180)
        lblRole.AutoSize = True
        Me.Controls.Add(lblRole)

        cmbRole.Font = New Font("Segoe UI", 10)
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList
        cmbRole.Items.Add("Administrateur")
        cmbRole.Items.Add("Pharmacien")
        cmbRole.SelectedIndex = 0
        cmbRole.Location = New Point(190, 175)
        cmbRole.Size = New Size(160, 25)
        Me.Controls.Add(cmbRole)

        ' ===== BOUTON CONNEXION =====
        btnConnexion.Text = "Se connecter"
        btnConnexion.BackColor = Color.FromArgb(41, 128, 185)
        btnConnexion.ForeColor = Color.White
        btnConnexion.FlatStyle = FlatStyle.Flat
        btnConnexion.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnConnexion.Size = New Size(150, 35)
        btnConnexion.Location = New Point(130, 240)
        AddHandler btnConnexion.Click, AddressOf SeConnecter
        Me.Controls.Add(btnConnexion)
    End Sub

    Private Sub SeConnecter(sender As Object, e As EventArgs)
        Dim utilisateur As String = txtUtilisateur.Text.Trim()
        Dim motDePasse As String = txtMotDePasse.Text.Trim()
        Dim role As String = cmbRole.SelectedItem.ToString()

        If utilisateur = "" Or motDePasse = "" Then
            MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As MySqlConnection = GetConnection()
                Dim query As String = "SELECT COUNT(*) FROM utilisateurs WHERE nom=@nom AND mot_de_passe=@mdp AND role=@role"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@nom", utilisateur)
                cmd.Parameters.AddWithValue("@mdp", motDePasse)
                cmd.Parameters.AddWithValue("@role", role)

                Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If result > 0 Then
                    MessageBox.Show("Connexion réussie !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Redirection selon le rôle
                    If role = "Administrateur" Then
                        Dim frmAdmin As New MainForm()
                        frmAdmin.Show()
                        Me.Hide()
                    ElseIf role = "Pharmacien" Then
                        Dim frmPharmacien As New PharmacienForm()
                        frmPharmacien.Show()
                        Me.Hide()
                    End If
                Else
                    MessageBox.Show("Nom d'utilisateur, mot de passe ou rôle incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub
End Class
