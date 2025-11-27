Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class FormPharmacien
    Inherits Form

    Private lblNom, lblEmail, lblTel As New Label()
    Private txtNom, txtEmail, txtTel As New TextBox()

    Private Sub FormPharmacien_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UIHelpers.AjouterLogoPetit(Me, 15, 15)
    End Sub

    Private btnValider, btnAnnuler As New Button()
    Private pharmacienId As Integer = -1

    ' Constructeur pour ajouter ou modifier
    Public Sub New(Optional id As Integer = -1, Optional nom As String = "", Optional email As String = "", Optional tel As String = "")
        Me.Text = If(id = -1, "Ajouter un Pharmacien", "Modifier un Pharmacien")
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Size = New Size(450, 350)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.BackColor = Color.White
        pharmacienId = id

        ' ===== LABELS =====
        lblNom.Text = "Nom :" : lblNom.Location = New Point(30, 40) : lblNom.Font = New Font("Segoe UI", 10, FontStyle.Bold) : Me.Controls.Add(lblNom)
        lblEmail.Text = "Email :" : lblEmail.Location = New Point(30, 100) : lblEmail.Font = New Font("Segoe UI", 10, FontStyle.Bold) : Me.Controls.Add(lblEmail)
        lblTel.Text = "Téléphone :" : lblTel.Location = New Point(30, 160) : lblTel.Font = New Font("Segoe UI", 10, FontStyle.Bold) : Me.Controls.Add(lblTel)

        ' ===== TEXTBOX =====
        txtNom.Location = New Point(150, 35) : txtNom.Size = New Size(250, 30) : txtNom.Text = nom : Me.Controls.Add(txtNom)
        txtEmail.Location = New Point(150, 95) : txtEmail.Size = New Size(250, 30) : txtEmail.Text = email : Me.Controls.Add(txtEmail)
        txtTel.Location = New Point(150, 155) : txtTel.Size = New Size(250, 30) : txtTel.Text = tel : Me.Controls.Add(txtTel)

        ' ===== BOUTONS =====
        btnValider.Text = If(id = -1, "Ajouter", "Modifier")
        btnValider.BackColor = Color.FromArgb(39, 174, 96)
        btnValider.ForeColor = Color.White
        btnValider.FlatStyle = FlatStyle.Flat
        btnValider.Location = New Point(80, 230)
        btnValider.Size = New Size(120, 40)
        AddHandler btnValider.Click, AddressOf Valider
        Me.Controls.Add(btnValider)

        btnAnnuler.Text = "Annuler"
        btnAnnuler.BackColor = Color.FromArgb(192, 57, 43)
        btnAnnuler.ForeColor = Color.White
        btnAnnuler.FlatStyle = FlatStyle.Flat
        btnAnnuler.Location = New Point(250, 230)
        btnAnnuler.Size = New Size(120, 40)
        AddHandler btnAnnuler.Click, Sub() Me.Close()
        Me.Controls.Add(btnAnnuler)
    End Sub

    ' ===== VALIDER AJOUT / MODIFICATION =====
    Private Sub Valider(sender As Object, e As EventArgs)
        ' Vérification des champs
        If String.IsNullOrWhiteSpace(txtNom.Text) OrElse String.IsNullOrWhiteSpace(txtEmail.Text) OrElse String.IsNullOrWhiteSpace(txtTel.Text) Then
            MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim emailRegex As New Regex("^[\w\.-]+@[\w\.-]+\.\w+$")
        If Not emailRegex.IsMatch(txtEmail.Text) Then
            MessageBox.Show("Email invalide.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim telRegex As New Regex("^\d{8,15}$")
        If Not telRegex.IsMatch(txtTel.Text) Then
            MessageBox.Show("Téléphone invalide (8 à 15 chiffres).", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ===== Connexion à la base =====
        Try
            Using conn = GetConnection()
                Dim cmd As New MySqlCommand()
                cmd.Connection = conn

                If pharmacienId = -1 Then
                    ' ➕ AJOUTER DANS pharmaciens
                    cmd.CommandText = "INSERT INTO pharmaciens(nom, email, telephone) VALUES (@n, @e, @t)"
                    cmd.Parameters.AddWithValue("@n", txtNom.Text.Trim())
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim())
                    cmd.Parameters.AddWithValue("@t", txtTel.Text.Trim())
                    cmd.ExecuteNonQuery()

                    ' Récupérer l'ID du pharmacien ajouté
                    Dim idPharmacien As Integer = CInt(cmd.LastInsertedId)

                    ' ➕ AJOUTER DANS utilisateurs
                    Dim cmdUser As New MySqlCommand("INSERT INTO utilisateurs(nom, email, mot_de_passe, role, reference_id) VALUES (@n, @e, @m, @r, @rid)", conn)
                    cmdUser.Parameters.AddWithValue("@n", txtNom.Text.Trim())
                    cmdUser.Parameters.AddWithValue("@e", txtEmail.Text.Trim())
                    cmdUser.Parameters.AddWithValue("@m", "123456") ' mot de passe par défaut
                    cmdUser.Parameters.AddWithValue("@r", "pharmacien")
                    cmdUser.Parameters.AddWithValue("@rid", idPharmacien)
                    cmdUser.ExecuteNonQuery()
                Else
                    ' ✏️ MODIFIER pharmaciens
                    cmd.CommandText = "UPDATE pharmaciens SET nom=@n, email=@e, telephone=@t WHERE id=@id"
                    cmd.Parameters.AddWithValue("@id", pharmacienId)
                    cmd.Parameters.AddWithValue("@n", txtNom.Text.Trim())
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim())
                    cmd.Parameters.AddWithValue("@t", txtTel.Text.Trim())
                    cmd.ExecuteNonQuery()

                    ' ✏️ MODIFIER utilisateurs correspondant
                    Dim cmdUser As New MySqlCommand("UPDATE utilisateurs SET nom=@n, email=@e WHERE reference_id=@id AND role='pharmacien'", conn)
                    cmdUser.Parameters.AddWithValue("@id", pharmacienId)
                    cmdUser.Parameters.AddWithValue("@n", txtNom.Text.Trim())
                    cmdUser.Parameters.AddWithValue("@e", txtEmail.Text.Trim())
                    cmdUser.ExecuteNonQuery()
                End If
            End Using

            MessageBox.Show("Opération réussie !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show("Cet email existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
