Module UIHelpers

    ' === AJOUTE LE LOGO EN HAUT À GAUCHE (Sidebar ou formulaire) ===
    Public Sub AjouterLogo(parent As Control)
        Dim pbLogo As New PictureBox()
        pbLogo.Size = New Size(50, 50)
        pbLogo.Location = New Point(20, 20)
        pbLogo.SizeMode = PictureBoxSizeMode.Zoom
        pbLogo.BackColor = Color.Transparent

        ' Charge le logo depuis Resources
        Try
            pbLogo.Image = My.Resources.pharmasoft1  ' Change le nom si besoin
        Catch
            ' Si pas trouvé → on dessine un + bleu (comme fallback)
            Dim bmp As New Bitmap(50, 50)
            Using g = Graphics.FromImage(bmp)
                g.Clear(Color.FromArgb(41, 128, 185))
                Using f As New Font("Segoe UI", 20, FontStyle.Bold), b As New SolidBrush(Color.White)
                    g.DrawString("+", f, b, 10, 5)
                End Using
            End Using
            pbLogo.Image = bmp
        End Try

        parent.Controls.Add(pbLogo)
    End Sub

    ' === VERSION PETITE (pour header ou coin) ===
    Public Sub AjouterLogoPetit(parent As Control, x As Integer, y As Integer)
        Dim pb As New PictureBox()
        pb.Size = New Size(40, 40)
        pb.Location = New Point(x, y)
        pb.SizeMode = PictureBoxSizeMode.Zoom
        pb.BackColor = Color.Transparent
        Try
            pb.Image = My.Resources.pharmasoft1
        Catch
            ' Fallback
        End Try
        parent.Controls.Add(pb)
    End Sub

End Module
