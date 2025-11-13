Imports System.Drawing
Imports System.Windows.Forms
Imports BCrypt.Net.BCrypt  ' BCrypt correct
Imports MySql.Data.MySqlClient

Public Class FormLogin
    Inherits Form

    Private txtUtilisateur As TextBox
    Private txtMotDePasse As TextBox
    Private cmbRole As ComboBox
    Private btnConnexion As Button
    Private pnlLoading As Panel
    Private lblLoading As Label

    Public Sub New()
        InitialiserInterface()
    End Sub

    Private Sub InitialiserInterface()
        Me.Text = "Connexion - PharmaSoft"
        Me.Size = New Size(460, 520)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.BackColor = Color.White
        Me.Font = New Font("Segoe UI", 10)

        ' === FOND DÉGRADÉ ===
        AddHandler Me.Paint, Sub(s, e)
                                 Using brush As New Drawing2D.LinearGradientBrush(Me.ClientRectangle, Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185), 90)
                                     e.Graphics.FillRectangle(brush, Me.ClientRectangle)
                                 End Using
                             End Sub

        ' === PANEL CENTRAL ===
        Dim pnlCentral As New Panel()
        pnlCentral.Size = New Size(380, 420)
        pnlCentral.Location = New Point(40, 50)
        pnlCentral.BackColor = Color.White
        pnlCentral.BorderRadius(20)
        pnlCentral.AddShadow()
        Me.Controls.Add(pnlCentral)

        ' === TITRE ===
        Dim lblTitre As New Label()
        lblTitre.Text = "Connexion"
        lblTitre.Font = New Font("Segoe UI", 22, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(41, 128, 185)
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(130, 40)
        pnlCentral.Controls.Add(lblTitre)

        ' === ICÔNE ===
        Dim pbIcon As New Panel()
        pbIcon.Size = New Size(70, 70)
        pbIcon.Location = New Point(155, 30)
        pbIcon.BackColor = Color.FromArgb(41, 128, 185)
        pbIcon.BorderRadius(35)
        AddHandler pbIcon.Paint, Sub(s, e)
                                     Using b As New SolidBrush(Color.White), f As New Font("Segoe UI", 28, FontStyle.Bold)
                                         e.Graphics.DrawString("+", f, b, 18, 12)
                                     End Using
                                 End Sub
        pnlCentral.Controls.Add(pbIcon)

        ' === UTILISATEUR ===
        Dim lblUser As New Label()
        lblUser.Text = "Nom d'utilisateur"
        lblUser.Location = New Point(50, 130)
        pnlCentral.Controls.Add(lblUser)

        txtUtilisateur = New TextBox()
        txtUtilisateur.Text = ""
        txtUtilisateur.Font = New Font("Segoe UI", 11)
        txtUtilisateur.Size = New Size(280, 38)
        txtUtilisateur.Location = New Point(50, 155)
        txtUtilisateur.BorderStyle = BorderStyle.FixedSingle
        txtUtilisateur.BackColor = Color.FromArgb(248, 249, 252)
        txtUtilisateur.Padding = New Padding(10)
        txtUtilisateur.BorderRadius(12)
        AddHandler txtUtilisateur.GotFocus, Sub() If txtUtilisateur.Text = "ex: admin" Then txtUtilisateur.Text = "" : txtUtilisateur.ForeColor = Color.Black
        AddHandler txtUtilisateur.LostFocus, Sub() If txtUtilisateur.Text = "" Then txtUtilisateur.Text = "ex: admin" : txtUtilisateur.ForeColor = Color.Gray
        txtUtilisateur.Text = "ex: admin"
        txtUtilisateur.ForeColor = Color.Gray
        pnlCentral.Controls.Add(txtUtilisateur)

        ' === MOT DE PASSE ===
        Dim lblPass As New Label()
        lblPass.Text = "Mot de passe"
        lblPass.Location = New Point(50, 205)
        pnlCentral.Controls.Add(lblPass)

        txtMotDePasse = New TextBox()
        txtMotDePasse.Text = ""
        txtMotDePasse.Font = New Font("Segoe UI", 11)
        txtMotDePasse.Size = New Size(280, 38)
        txtMotDePasse.Location = New Point(50, 230)
        txtMotDePasse.BorderStyle = BorderStyle.FixedSingle
        txtMotDePasse.BackColor = Color.FromArgb(248, 249, 252)
        txtMotDePasse.PasswordChar = "●"c
        txtMotDePasse.Padding = New Padding(10)
        txtMotDePasse.BorderRadius(12)
        AddHandler txtMotDePasse.GotFocus, Sub() If txtMotDePasse.Text = "••••••••" Then txtMotDePasse.Text = "" : txtMotDePasse.ForeColor = Color.Black
        AddHandler txtMotDePasse.LostFocus, Sub() If txtMotDePasse.Text = "" Then txtMotDePasse.Text = "••••••••" : txtMotDePasse.ForeColor = Color.Gray
        txtMotDePasse.Text = "••••••••"
        txtMotDePasse.ForeColor = Color.Gray
        pnlCentral.Controls.Add(txtMotDePasse)

        ' === RÔLE ===
        Dim lblRole As New Label()
        lblRole.Text = "Rôle"
        lblRole.Location = New Point(50, 280)
        pnlCentral.Controls.Add(lblRole)

        cmbRole = New ComboBox()
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList
        cmbRole.Items.AddRange({"admin", "pharmacien"})
        cmbRole.SelectedIndex = 0
        cmbRole.Font = New Font("Segoe UI", 11)
        cmbRole.Size = New Size(280, 38)
        cmbRole.Location = New Point(50, 305)
        cmbRole.BackColor = Color.FromArgb(248, 249, 252)
        cmbRole.FlatStyle = FlatStyle.Flat
        cmbRole.BorderRadius(12)
        pnlCentral.Controls.Add(cmbRole)

        ' === BOUTON ===
        btnConnexion = New Button()
        btnConnexion.Text = "Se connecter"
        btnConnexion.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btnConnexion.BackColor = Color.FromArgb(41, 128, 185)
        btnConnexion.ForeColor = Color.White
        btnConnexion.Size = New Size(280, 45)
        btnConnexion.Location = New Point(50, 360)
        btnConnexion.FlatStyle = FlatStyle.Flat
        btnConnexion.FlatAppearance.BorderSize = 0
        btnConnexion.BorderRadius(12)
        AddHandler btnConnexion.Click, AddressOf SeConnecter
        pnlCentral.Controls.Add(btnConnexion)

        ' === CHARGEMENT ===
        pnlLoading = New Panel()
        pnlLoading.Size = New Size(280, 45)
        pnlLoading.Location = New Point(50, 360)
        pnlLoading.BackColor = Color.FromArgb(41, 128, 185)
        pnlLoading.BorderRadius(12)
        pnlLoading.Visible = False
        pnlCentral.Controls.Add(pnlLoading)

        lblLoading = New Label()
        lblLoading.Text = "Connexion en cours..."
        lblLoading.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblLoading.ForeColor = Color.White
        lblLoading.AutoSize = True
        lblLoading.Location = New Point(70, 12)
        pnlLoading.Controls.Add(lblLoading)

        Dim pbLoading As New Label()
        pbLoading.Text = "Loading"
        pbLoading.Font = New Font("Webdings", 12)
        pbLoading.ForeColor = Color.White
        pbLoading.Location = New Point(45, 10)
        pnlLoading.Controls.Add(pbLoading)
    End Sub

    Private Sub SeConnecter(sender As Object, e As EventArgs)
        Dim utilisateur As String = If(txtUtilisateur.Text = "ex: admin", "", txtUtilisateur.Text.Trim())
        Dim motDePasse As String = If(txtMotDePasse.Text = "••••••••", "", txtMotDePasse.Text.Trim())
        Dim role As String = cmbRole.SelectedItem.ToString().ToLower()

        If utilisateur = "" Or motDePasse = "" Then
            MsgBox("Veuillez remplir tous les champs.", 48, "Erreur")
            Return
        End If

        btnConnexion.Visible = False
        pnlLoading.Visible = True
        Me.Refresh()

        Try
            Using conn As MySqlConnection = GetConnection()
                Dim query As String = "SELECT mot_de_passe, role FROM utilisateurs WHERE nom = @nom LIMIT 1"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@nom", utilisateur)
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim passwordBD As String = reader("mot_de_passe").ToString()
                            Dim roleBD As String = reader("role").ToString().ToLower()
                            If passwordBD = motDePasse AndAlso roleBD = role Then
                                MsgBox($"Bienvenue, {utilisateur} !", 64, "Succès")

                                Me.Hide()
                                If role = "admin" Then
                                    Dim frm As New MainForm()
                                    'frm.nomUtilisateur = utilisateur
                                    frm.Show()
                                Else
                                    Dim frm As New PharmacienForm()
                                    'frm.nomUtilisateur = utilisateur
                                    frm.Show()
                                End If
                            Else
                                MsgBox("Identifiants incorrects.", 16, "Erreur")
                            End If
                        Else
                            MsgBox("Utilisateur introuvable.", 16, "Erreur")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Erreur : " & ex.Message, 16, "Erreur")
        Finally
            pnlLoading.Visible = False
            btnConnexion.Visible = True
        End Try
    End Sub
End Class