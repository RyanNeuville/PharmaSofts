Imports System.Drawing
Imports System.Windows.Forms

Public Class MainForm
    Inherits Form

    Private panelMenu As New Panel()
    Private panelContent As New Panel()
    Private panelTopBar As New Panel()
    Private lblTitle As New Label()
    Private btnMinimize As New Button()
    Private btnMaximize As New Button()
    Private btnClose As New Button()
    Private btnLogout As New Button()

    Private btnDashboard As New Button()
    Private btnMedicaments As New Button()
    Private btnVentes As New Button()
    Private btnUtilisateurs As New Button()
    Private btnFournisseurs As New Button()

    ' === Pages (UserControls) ===
    Private dashboard As New DashboardControl()
    Private medicaments As New MedicamentsControl()
    Private ventes As New VentesControl()
    Private utilisateurs As New UtilisateursControl()
    Private fournisseurs As New FournisseursControl()

    Public Sub New()
        ' === Form ===
        Me.Text = "Gestion de Pharmacie"
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.White


        ' === Panel Menu ===
        panelMenu.Dock = DockStyle.Left
        panelMenu.Width = 220
        panelMenu.BackColor = Color.FromArgb(22, 160, 133)
        Me.Controls.Add(panelMenu)

        ' === Panel TopBar ===
        panelTopBar.Dock = DockStyle.Top
        panelTopBar.Height = 45
        panelTopBar.BackColor = Color.White
        panelTopBar.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(panelTopBar)

        ' === Panel Content ===
        panelContent.Dock = DockStyle.Fill
        panelContent.BackColor = Color.WhiteSmoke
        Me.Controls.Add(panelContent)
        panelContent.BringToFront()

        ' === Title ===
        lblTitle.Text = "Tableau de bord"
        lblTitle.Font = New Font("Segoe UI", 13, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(44, 62, 80)
        lblTitle.AutoSize = True
        lblTitle.Location = New Point(20, 10)
        panelTopBar.Controls.Add(lblTitle)

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

        ' === Placement boutons fenêtre ===
        AddHandler Me.Resize, Sub() UpdateWindowButtons()
        UpdateWindowButtons()

        AddHandler btnMinimize.Click, Sub() Me.WindowState = FormWindowState.Minimized
        AddHandler btnMaximize.Click, Sub()
                                          If Me.WindowState = FormWindowState.Maximized Then
                                              Me.WindowState = FormWindowState.Normal
                                          Else
                                              Me.WindowState = FormWindowState.Maximized
                                          End If
                                      End Sub
        AddHandler btnClose.Click, Sub() Me.Close()

        ' === Boutons menu ===
        SetupButton(btnDashboard, "Tableau de bord", 60)
        SetupButton(btnMedicaments, "Médicaments", 120)
        SetupButton(btnVentes, "Ventes", 180)
        SetupButton(btnUtilisateurs, "Pharmaciens", 240)
        SetupButton(btnFournisseurs, "Fournisseurs", 300)

        For Each btn In {btnDashboard, btnMedicaments, btnVentes, btnUtilisateurs, btnFournisseurs}
            panelMenu.Controls.Add(btn)
            AddHandler btn.Click, AddressOf Menu_Click
        Next

        ' === Bouton Déconnexion ===
        btnLogout.Text = "Déconnexion"
        btnLogout.FlatStyle = FlatStyle.Flat
        btnLogout.FlatAppearance.BorderSize = 0
        btnLogout.ForeColor = Color.White
        btnLogout.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnLogout.BackColor = Color.FromArgb(231, 76, 60)
        btnLogout.Width = 180
        btnLogout.Height = 40
        btnLogout.Cursor = Cursors.Hand
        AddHandler btnLogout.Click, AddressOf Logout_Click
        panelMenu.Controls.Add(btnLogout)

        ' Position du bouton déconnexion au bas du menu
        AddHandler panelMenu.Resize, Sub() btnLogout.Location = New Point(20, panelMenu.Height - 60)
        btnLogout.Location = New Point(20, panelMenu.Height - 60)

        ' === Page par défaut ===
        LoadControl(dashboard)
    End Sub

    Private Sub UpdateWindowButtons()
        btnMinimize.Left = Me.Width - 120
        btnMaximize.Left = Me.Width - 80
        btnClose.Left = Me.Width - 40
        btnMinimize.Top = btnMaximize.Top = btnClose.Top = 5
    End Sub

    Private Sub SetupButton(btn As Button, text As String, top As Integer)
        btn.Text = text
        btn.FlatStyle = FlatStyle.Flat
        btn.ForeColor = Color.White
        btn.BackColor = Color.FromArgb(26, 188, 156)
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

        For Each b In {btnDashboard, btnMedicaments, btnVentes, btnUtilisateurs, btnFournisseurs}
            b.BackColor = Color.FromArgb(26, 188, 156)
        Next
        btn.BackColor = Color.FromArgb(39, 174, 96)
        lblTitle.Text = btn.Text

        Select Case btn.Text
            Case "Tableau de bord" : LoadControl(dashboard)
            Case "Médicaments" : LoadControl(medicaments)
            Case "Ventes" : LoadControl(ventes)
            Case "Pharmaciens" : LoadControl(utilisateurs)
            Case "Fournisseurs" : LoadControl(fournisseurs)
        End Select
    End Sub

    Private Sub LoadControl(ctrl As UserControl)
        panelContent.Controls.Clear()
        ctrl.Dock = DockStyle.Fill
        panelContent.Controls.Add(ctrl)
    End Sub

    Private Sub Logout_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Restart()
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UIHelpers.AjouterLogoPetit(Me, 15, 15)
    End Sub
End Class