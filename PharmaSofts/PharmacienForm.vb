Imports System.Drawing
Imports System.Windows.Forms

Public Class PharmacienForm
    Inherits Form

    ' === Panels ===
    Private panelMenu As New Panel()
    Private panelTopBar As New Panel()
    Private panelContent As New Panel()
    Private lblTitle As New Label()

    ' === Icône profil ===
    Private picProfil As New PictureBox()
    Private lblUserName As New Label()

    ' === Boutons fenêtre ===
    Private btnMinimize As New Button()
    Private btnMaximize As New Button()
    Private btnClose As New Button()

    ' === Boutons menu ===
    Private btnDashboard As New Button()
    Private btnVentes As New Button()
    Private btnStock As New Button()
    Private btnHistorique As New Button()
    Private btnProfil As New Button()
    Private btnDeconnexion As New Button()

    ' === Pages Pharmacien ===
    Private dash As New PharmacienDashboard()
    Private ventes As New PharmacienVentes()
    Private stock As New PharmacienStock()
    Private profil As New PharmacienProfils()
    Private historique As New PharmacienHistorique()

    Public Sub New()
        ' === Fenêtre principale ===
        Me.Text = "Espace Pharmacien"
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.White

        ' === Panel Menu ===
        panelMenu.Dock = DockStyle.Left
        panelMenu.Width = 220
        panelMenu.BackColor = Color.FromArgb(41, 128, 185)
        Me.Controls.Add(panelMenu)

        ' === Panel TopBar ===
        panelTopBar.Dock = DockStyle.Top
        panelTopBar.Height = 50
        panelTopBar.BackColor = Color.White
        panelTopBar.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(panelTopBar)

        ' === Label titre ===
        lblTitle.Text = "Tableau de bord"
        lblTitle.Font = New Font("Segoe UI", 13, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(44, 62, 80)
        lblTitle.AutoSize = True
        lblTitle.Location = New Point(20, 13)
        panelTopBar.Controls.Add(lblTitle)

        ' === Icône profil ===
        picProfil.Size = New Size(32, 32)
        picProfil.SizeMode = PictureBoxSizeMode.Zoom
        'picProfil.Image = My.Resources.user_icon ' ⚠️ Mets une icône "user_icon" dans tes ressources
        picProfil.Left = Me.Width - 180
        picProfil.Top = 9
        panelTopBar.Controls.Add(picProfil)

        ' === Nom utilisateur ===
        lblUserName.Text = "Pharmacien: Dr. Mounir"
        lblUserName.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblUserName.ForeColor = Color.FromArgb(52, 73, 94)
        lblUserName.AutoSize = True
        lblUserName.Left = Me.Width - 140
        lblUserName.Top = 15
        panelTopBar.Controls.Add(lblUserName)

        ' === Boutons fenêtre ===
        btnMinimize.Text = "–"
        btnMaximize.Text = "□"
        btnClose.Text = "X"

        For Each b In {btnMinimize, btnMaximize, btnClose}
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderSize = 0
            b.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            b.Width = 35
            b.Height = 35
            b.BackColor = Color.White
            b.ForeColor = Color.Gray
            AddHandler b.MouseEnter, Sub(sender, e) b.BackColor = Color.FromArgb(230, 230, 230)
            AddHandler b.MouseLeave, Sub(sender, e) b.BackColor = Color.White
            panelTopBar.Controls.Add(b)
        Next

        btnClose.ForeColor = Color.FromArgb(231, 76, 60)
        PositionTopBarButtons()
        AddHandler btnMinimize.Click, Sub() Me.WindowState = FormWindowState.Minimized
        AddHandler btnMaximize.Click, Sub()
                                          If Me.WindowState = FormWindowState.Maximized Then
                                              Me.WindowState = FormWindowState.Normal
                                          Else
                                              Me.WindowState = FormWindowState.Maximized
                                          End If
                                          PositionTopBarButtons()
                                      End Sub
        AddHandler btnClose.Click, Sub() Me.Close()
        AddHandler Me.Resize, Sub() PositionTopBarButtons()

        ' === Panel Content ===
        panelContent.Dock = DockStyle.Fill
        panelContent.BackColor = Color.WhiteSmoke
        Me.Controls.Add(panelContent)
        panelContent.BringToFront()

        ' === Boutons Menu ===
        Dim startY As Integer = 60
        SetupButton(btnDashboard, "Tableau de bord", startY)
        SetupButton(btnVentes, "Ventes", startY + 60)
        SetupButton(btnStock, "Stock", startY + 120)
        SetupButton(btnHistorique, "Historique", startY + 180)
        SetupButton(btnProfil, "Mon profil", startY + 240)

        ' Bouton Déconnexion fixé en bas
        btnDeconnexion.Text = "Déconnexion"
        btnDeconnexion.FlatStyle = FlatStyle.Flat
        btnDeconnexion.FlatAppearance.BorderSize = 0
        btnDeconnexion.ForeColor = Color.White
        btnDeconnexion.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btnDeconnexion.BackColor = Color.FromArgb(231, 76, 60)
        btnDeconnexion.Height = 45
        btnDeconnexion.Dock = DockStyle.Bottom
        btnDeconnexion.Cursor = Cursors.Hand
        panelMenu.Controls.Add(btnDeconnexion)

        ' === Ajouter les handlers ===
        For Each btn In {btnDashboard, btnVentes, btnStock, btnHistorique, btnProfil, btnDeconnexion}
            panelMenu.Controls.Add(btn)
            AddHandler btn.Click, AddressOf Menu_Click
        Next

        ' Page par défaut
        LoadControl(dash)
        btnDashboard.BackColor = Color.FromArgb(39, 174, 96)
    End Sub

    Private Sub SetupButton(btn As Button, text As String, top As Integer)
        btn.Text = text
        btn.FlatStyle = FlatStyle.Flat
        btn.ForeColor = Color.White
        btn.BackColor = Color.FromArgb(52, 152, 219)
        btn.FlatAppearance.BorderSize = 0
        btn.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btn.Width = 180
        btn.Height = 40
        btn.Left = 20
        btn.Top = top
        btn.Cursor = Cursors.Hand
    End Sub

    Private Sub Menu_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)

        ' Réinitialiser couleurs
        For Each b In {btnDashboard, btnVentes, btnStock, btnHistorique, btnProfil}
            b.BackColor = Color.FromArgb(52, 152, 219)
        Next

        ' Bouton actif
        If btn IsNot btnDeconnexion Then btn.BackColor = Color.FromArgb(39, 174, 96)
        lblTitle.Text = btn.Text

        ' Charger la page
        Select Case btn.Text
            Case "Tableau de bord" : LoadControl(dash)
            Case "Ventes" : LoadControl(ventes)
            Case "Stock" : LoadControl(stock)
            Case "Historique" : LoadControl(historique)
            Case "Mon profil" : LoadControl(profil)
            Case "Déconnexion"
                If MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Application.Restart()
                End If
        End Select
    End Sub

    Private Sub LoadControl(ctrl As UserControl)
        panelContent.Controls.Clear()
        ctrl.Dock = DockStyle.Fill
        panelContent.Controls.Add(ctrl)
    End Sub

    Private Sub PositionTopBarButtons()
        btnMinimize.Left = Me.ClientSize.Width - 120
        btnMaximize.Left = Me.ClientSize.Width - 80
        btnClose.Left = Me.ClientSize.Width - 40
        btnMinimize.Top = 8
        btnMaximize.Top = 8
        btnClose.Top = 8
    End Sub
End Class
