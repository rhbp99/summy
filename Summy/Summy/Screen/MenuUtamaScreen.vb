Public Class MenuUtamaScreen
    Inherits Screens

    Public Overrides Property Enable As Boolean
    Public Overrides Property Name As String
    Public Overrides Property Visible As Boolean

    Private TextureBase As Texture2D
    Private FontSprite As SpriteFont
    Private TextureShadowMenuUtama As Texture2D
    Private TextureShadowAbout As Texture2D
    Private Menu(3) As MenuUtama
    Private MenuIsOnTop As MenuUtama = MenuUtama.Menu
    Private MenuTerpilih As MenuUtama = MenuUtama.Menu
    Private TextureTransparentDim As Texture2D
    Private TextureHelp(2) As Texture2D
    Private TextureHelpShadow As Texture2D
    Private HelpKoordinat(2) As Vector2
    Private AnimasiSize As Single = -1
    Private HelpIndex As Int16 = 0

    Private version As String = $"SUMMY VERSION {Reflection.Assembly.GetExecutingAssembly.GetName.Version}"
    Private developedby As String = "Developed By : Reza H. Bayu Prabowo"
    Private email As String = "rh.bayu.prabowo@outlook.com"
    Private fontstring As String = "FONT: PIXELDUST"
    Private fontcopyright As String = "Copyright (c) Andreas Nylin"
    Private iconstring As String = "ICON: MATERIAL DESIGN"
    Private iconcopyright As String = "Copyright (c) Google"
    Private MenuDeactive As Boolean
    Private AnimasiHorizontalAktif As Boolean
    Private ArahAnimasiIsLeft As Boolean
    Private TextureHelpNavShadow As Texture2D
    Private NavHelpLeft As Boolean
    Private NavHelpRight As Boolean
    Private Downy As Texture2D
    Private Uppy As Texture2D

    Public Enum MenuUtama
        Main
        Bantuan
        Tentang
        Keluar
        Menu
    End Enum

    Public Sub New(Name As String, Enable As Boolean, Visible As Boolean)
        Me.Name = Name
        Me.Enable = Enable
        Me.Visible = Visible
        Load()
        Initialize()
    End Sub

    Public Overrides Sub Draw()
        Sprite.Draw(TextureBase, New Rectangle(Center.Width, Center.Height, GameSize.Width, GameSize.Height), New Rectangle(0, 0, TextureBase.Width, TextureBase.Height), Color.Lerp(New Color(238, 238, 238), New Color(188, 188, 188), AnimasiSize), 0, New Vector2(TextureBase.Width / 2, TextureBase.Height / 2), SpriteEffects.None, 0)


        Sprite.Draw(TextureShadowMenuUtama, New Rectangle(Center.Width, Center.Height - 214, (376 / (TextureShadowMenuUtama.Width - 24) * TextureShadowMenuUtama.Width), (112 / (TextureShadowMenuUtama.Height - 24) * TextureShadowMenuUtama.Height)), New Rectangle(0, 0, TextureShadowMenuUtama.Width, TextureShadowMenuUtama.Height), New Color(97, 97, 97), 0, New Vector2(TextureShadowMenuUtama.Width / 2, TextureShadowMenuUtama.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(TextureBase, New Rectangle(Center.Width, Center.Height - 214, 376, 112), New Rectangle(0, 0, TextureBase.Width, TextureBase.Height), New Color(97, 97, 97), 0, New Vector2(TextureBase.Width / 2, TextureBase.Height / 2), SpriteEffects.None, 0)
        Sprite.DrawString(FontSprite, "SUMMY", New Vector2(Center.Width, Center.Height - 214), New Color(245, 245, 245), 0, New Vector2(FontSprite.MeasureString("SUMMY").X / 2, FontSprite.MeasureString("SUMMY").Y / 2), 0.5, SpriteEffects.None, 0)

        Dim space = 0
        For i = 0 To 3
            If i = MenuIsOnTop Then
                Sprite.Draw(TextureShadowMenuUtama, New Rectangle(Center.Width, Center.Height - 59 + space, (236 / (TextureShadowMenuUtama.Width - 24) * TextureShadowMenuUtama.Width), (56 / (TextureShadowMenuUtama.Height - 24) * TextureShadowMenuUtama.Height)), New Rectangle(0, 0, TextureShadowMenuUtama.Width, TextureShadowMenuUtama.Height), New Color(97, 97, 97), 0, New Vector2(TextureShadowMenuUtama.Width / 2, TextureShadowMenuUtama.Height / 2), SpriteEffects.None, 0)
            End If
            Sprite.Draw(TextureBase, New Rectangle(Center.Width, Center.Height - 59 + space, 236, 56), New Rectangle(0, 0, TextureBase.Width, TextureBase.Height), New Color(117, 117, 117), 0, New Vector2(TextureBase.Width / 2, TextureBase.Height / 2), SpriteEffects.None, 0)
            Sprite.DrawString(FontSprite, Menu(i).ToString, New Vector2(Center.Width, Center.Height - 59 + space), Color.White, 0, New Vector2(FontSprite.MeasureString(Menu(i).ToString).X / 2, FontSprite.MeasureString(Menu(i).ToString).Y / 2), If(i = MenuIsOnTop, 0.25, 0.2), SpriteEffects.None, 0)
            space += 86
        Next
        'Sprite.Draw(TextureTransparentDim, New Rectangle(Center.width, Center.Height, GameSize.Width, GameSize.Height), New Rectangle(0, 0, TextureTransparentDim.Width, TextureTransparentDim.Height), Color.Lerp(New Color(156, 156, 156), Color.Black, AnimasiSize), 0, New Vector2(TextureTransparentDim.Width / 2, TextureTransparentDim.Height / 2), SpriteEffects.None, 0)
        If MenuTerpilih = MenuUtama.Tentang Then
            Sprite.Draw(TextureShadowAbout, New Rectangle(Center.Width, Center.Height, TextureShadowAbout.Width * AnimasiSize, TextureShadowAbout.Height * AnimasiSize), New Rectangle(0, 0, TextureShadowAbout.Width, TextureShadowAbout.Height), New Color(97, 97, 97), 0, New Vector2(TextureShadowAbout.Width / 2, TextureShadowAbout.Height / 2), SpriteEffects.None, 0)
            Sprite.Draw(TextureBase, New Rectangle(Center.Width, Center.Height, 500 * AnimasiSize, 600 * AnimasiSize), New Rectangle(0, 0, TextureBase.Width, TextureBase.Height), New Color(117, 117, 117), 0, New Vector2(TextureBase.Width / 2, TextureBase.Height / 2), SpriteEffects.None, 0)

            Sprite.DrawString(FontSprite, "Tentang", New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 268) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString("Tentang").X / 2, FontSprite.MeasureString("Tentang").Y / 2), 0.32 * AnimasiSize, SpriteEffects.None, 0)
            Sprite.DrawString(FontSprite, version, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 151) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(version).X / 2, FontSprite.MeasureString(version).Y / 2), 0.14 * AnimasiSize, SpriteEffects.None, 0)

            Sprite.DrawString(FontSprite, developedby, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 54) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(developedby).X / 2, FontSprite.MeasureString(developedby).Y / 2), 0.16 * AnimasiSize, SpriteEffects.None, 0)
            Sprite.DrawString(FontSprite, email, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 16) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(email).X / 2, FontSprite.MeasureString(email).Y / 2), 0.14 * AnimasiSize, SpriteEffects.None, 0)

            Sprite.DrawString(FontSprite, fontstring, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height + 84) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(fontstring).X / 2, FontSprite.MeasureString(fontstring).Y / 2), 0.15 * AnimasiSize, SpriteEffects.None, 0)
            Sprite.DrawString(FontSprite, fontcopyright, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height + 113) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(fontcopyright).X / 2, FontSprite.MeasureString(fontcopyright).Y / 2), 0.13 * AnimasiSize, SpriteEffects.None, 0)

            Sprite.DrawString(FontSprite, iconstring, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (Center.Height + 165) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(iconstring).X / 2, FontSprite.MeasureString(iconstring).Y / 2), 0.15 * AnimasiSize, SpriteEffects.None, 0)
            Sprite.DrawString(FontSprite, iconcopyright, New Vector2(Center.Width, (Center.Height * (1 - AnimasiSize)) + (GameSize.Height + 195) * AnimasiSize), New Color(224, 224, 224), 0, New Vector2(FontSprite.MeasureString(iconcopyright).X / 2, FontSprite.MeasureString(iconcopyright).Y / 2), 0.13 * AnimasiSize, SpriteEffects.None, 0)

            Sprite.DrawString(FontSprite, "keluar", New Vector2((Center.Width * (1 - AnimasiSize)) + (Center.Width + 190) * AnimasiSize, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 320) * AnimasiSize), Color.White, 0, New Vector2(FontSprite.MeasureString("keluar").X / 2, FontSprite.MeasureString("keluar").Y / 2), 0.15 * AnimasiSize, SpriteEffects.None, 0)
        End If

        If MenuTerpilih = MenuUtama.Bantuan Then
            For h = 0 To HelpKoordinat.Count - 1
                Sprite.Draw(TextureHelpShadow, HelpKoordinat(h), New Rectangle(0, 0, TextureHelpShadow.Width, TextureHelpShadow.Height), New Color(97, 97, 97), 0, New Vector2(TextureHelpShadow.Width / 2, TextureHelpShadow.Height / 2), AnimasiSize, SpriteEffects.None, 0)
                If NavHelpLeft And AnimasiSize = 1 Then
                    Sprite.Draw(TextureHelpNavShadow, HelpKoordinat(h) - New Vector2(450, 0), New Rectangle(0, 0, TextureHelpNavShadow.Width, TextureHelpNavShadow.Height), New Color(97, 97, 97), 0, New Vector2(TextureHelpNavShadow.Width / 2, TextureHelpNavShadow.Height / 2), 1, SpriteEffects.None, 0)
                End If
                If NavHelpRight And AnimasiSize = 1 Then
                    Sprite.Draw(TextureHelpNavShadow, HelpKoordinat(h) + New Vector2(450, 0), New Rectangle(0, 0, TextureHelpNavShadow.Width, TextureHelpNavShadow.Height), New Color(97, 97, 97), 0, New Vector2(TextureHelpNavShadow.Width / 2, TextureHelpNavShadow.Height / 2), 1, SpriteEffects.None, 0)
                End If
                Sprite.Draw(TextureHelp(h), HelpKoordinat(h), New Rectangle(0, 0, TextureHelp(h).Width, TextureHelp(h).Height), Color.White, 0, New Vector2(TextureHelp(h).Width / 2, TextureHelp(h).Height / 2), AnimasiSize, SpriteEffects.None, 0)
            Next

            Sprite.DrawString(FontSprite, "keluar", New Vector2((Center.Width * (1 - AnimasiSize)) + (Center.Width + 440) * AnimasiSize, (Center.Height * (1 - AnimasiSize)) + (Center.Height - 270) * AnimasiSize), Color.White, 0, New Vector2(FontSprite.MeasureString("keluar").X / 2, FontSprite.MeasureString("keluar").Y / 2), 0.15 * AnimasiSize, SpriteEffects.None, 0)
        End If

        Sprite.Draw(Uppy, New Rectangle(Center.Width, Center.Height - 384 + Uppy.Height / 2, Uppy.Width, Uppy.Height), New Rectangle(0, 0, Uppy.Width, Uppy.Height), Color.White, 0, New Vector2(Uppy.Width / 2, Uppy.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(Downy, New Rectangle(Center.Width, Center.Height + 384 - Downy.Height / 2, Downy.Width, Downy.Height), New Rectangle(0, 0, Downy.Width, Downy.Height), Color.White, 0, New Vector2(Downy.Width / 2, Downy.Height / 2), SpriteEffects.None, 0)
    End Sub

    Public Overrides Sub Initialize()
        Menu(0) = MenuUtama.Main
        Menu(1) = MenuUtama.Bantuan
        Menu(2) = MenuUtama.Tentang
        Menu(3) = MenuUtama.Keluar
        ResetHelpKoordinat()
    End Sub

    Private Sub ResetHelpKoordinat()
        Dim pos As Integer = 0
        For i = 0 To HelpKoordinat.Count - 1
            HelpKoordinat(i) = New Vector2(Center.Width + pos, Center.Height)
            pos += TextureHelp(i).Width + 200
        Next
    End Sub

    Public Overrides Sub Load()
        FontSprite = ContentManager.Load(Of SpriteFont)("Debug")
        TextureShadowMenuUtama = ContentManager.Load(Of Texture2D)("gfx/shadow_menu_utama")
        TextureShadowAbout = ContentManager.Load(Of Texture2D)("gfx/shadow_about")
        TextureBase = ContentManager.Load(Of Texture2D)("gfx/backtile")
        TextureTransparentDim = ContentManager.Load(Of Texture2D)("gfx/transparent_dim")
        For i = 0 To TextureHelp.Count - 1
            TextureHelp(i) = ContentManager.Load(Of Texture2D)($"gfx/help_{i + 1}")
        Next
        TextureHelpShadow = ContentManager.Load(Of Texture2D)("gfx/help_shadow")
        TextureHelpNavShadow = ContentManager.Load(Of Texture2D)("gfx/help_nav_shadow")

        Downy = ContentManager.Load(Of Texture2D)("gfx/downy")
        Uppy = ContentManager.Load(Of Texture2D)("gfx/uppy")
    End Sub

    Public Overrides Sub Update(ByRef MenuSelected As MenuUtama)
        If MenuTerpilih = MenuUtama.Menu Then
            MenuIsOnTop = MenuUtama.Menu
            Dim Space = 0
            For i = 0 To 3
                If New Rectangle(Center.Width - (236 / 2), Center.Height - 59 + Space - (56 / 2), 236, 56).Contains(MouseStateBefore.X, MouseStateBefore.Y) Then
                    MenuIsOnTop = i
                    'Exit Sub
                End If
                Space += 86
            Next

            If MenuIsOnTop <> MenuUtama.Menu Then
                If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                    If MenuIsOnTop = MenuUtama.Keluar Then
                        MenuSelected = MenuUtama.Keluar
                        MenuTerpilih = MenuSelected
                    End If

                    If MenuIsOnTop = MenuUtama.Main Then
                        MenuSelected = MenuUtama.Main
                        MenuTerpilih = MenuSelected
                    End If

                    If MenuIsOnTop = MenuUtama.Bantuan Then
                        HelpIndex = 0
                        ResetHelpKoordinat()
                        MenuSelected = MenuUtama.Bantuan
                        MenuTerpilih = MenuSelected
                        AnimasiSize = 0
                    End If

                    If MenuIsOnTop = MenuUtama.Tentang Then
                        MenuSelected = MenuUtama.Tentang
                        MenuTerpilih = MenuSelected
                        AnimasiSize = 0
                    End If
                End If
            End If
        Else
            If KeyboardStateBefore.IsKeyDown(Keys.Escape) And KeyboardStateAfter.IsKeyUp(Keys.Escape) Then
                MenuSelected = MenuUtama.Menu
                MenuDeactive = True
            End If

            If (MenuTerpilih = MenuUtama.Tentang And Not New Rectangle(Center.Width - 500 / 2, Center.Height - 600 / 2, 500, 600).Contains(MouseStateBefore.X, MouseStateBefore.Y)) Or
                (MenuTerpilih = MenuUtama.Bantuan And Not New Rectangle(Center.Width - TextureHelp(HelpIndex).Width / 2, Center.Height - TextureHelp(HelpIndex).Height / 2, TextureHelp(0).Width, TextureHelp(HelpIndex).Height).Contains(MouseStateBefore.X, MouseStateBefore.Y)) Then
                If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                    MenuSelected = MenuUtama.Menu
                    MenuDeactive = True
                End If
            End If

            If MenuTerpilih = MenuUtama.Bantuan Then
                NavHelpLeft = False
                NavHelpRight = False
                If New Rectangle(HelpKoordinat(HelpIndex).X - 450 - (TextureHelpNavShadow.Width - 50) / 2, HelpKoordinat(HelpIndex).Y - (TextureHelpNavShadow.Height - 50) / 2,
                                 TextureHelpNavShadow.Width - 50, TextureHelpNavShadow.Height - 50).Contains(MouseStateAfter.X, MouseStateBefore.Y) And HelpIndex <> 0 Then
                    NavHelpLeft = True
                End If
                If New Rectangle(HelpKoordinat(HelpIndex).X + 450 - (TextureHelpNavShadow.Width - 50) / 2, HelpKoordinat(HelpIndex).Y - (TextureHelpNavShadow.Height - 50) / 2,
                                 TextureHelpNavShadow.Width - 50, TextureHelpNavShadow.Height - 50).Contains(MouseStateAfter.X, MouseStateBefore.Y) And HelpIndex <> HelpKoordinat.Count - 1 Then
                    NavHelpRight = True
                End If

                If (KeyboardStateBefore.IsKeyDown(Keys.Right) And KeyboardStateAfter.IsKeyUp(Keys.Right)) Or
                    NavHelpRight AndAlso MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                    HelpIndex += 1
                    If HelpIndex > TextureHelp.Count - 1 Then
                        HelpIndex = TextureHelp.Count - 1
                    End If
                    AnimasiHorizontalAktif = True
                    ArahAnimasiIsLeft = True
                End If
                If KeyboardStateBefore.IsKeyDown(Keys.Left) And KeyboardStateAfter.IsKeyUp(Keys.Left) Or
                    NavHelpLeft AndAlso MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                    HelpIndex -= 1
                    If HelpIndex < 0 Then
                        HelpIndex = 0
                    End If
                    AnimasiHorizontalAktif = True
                    ArahAnimasiIsLeft = False
                End If
            End If
        End If

        If AnimasiHorizontalAktif Then
            Dim Tujuan() = {New Vector2(Center.Width - (HelpIndex * 1200), Center.Height),
                            New Vector2(Center.Width + 1200 - (HelpIndex * 1200), Center.Height),
                            New Vector2(Center.Width + 2400 - (HelpIndex * 1200), Center.Height)}

            If AnimasiHorizontal(HelpKoordinat, ArahAnimasiIsLeft, Tujuan, 4) Then
                AnimasiHorizontalAktif = False
            End If

        End If

        If Not MenuDeactive And MenuTerpilih <> MenuUtama.Menu Then
            If AnimasiSize >= 0 And AnimasiSize < 1 Then
                AnimasiSize += Waktu.ElapsedGameTime.TotalMilliseconds / 200
            End If
            If AnimasiSize > 1 Then
                AnimasiSize = 1
            End If
        End If

        If MenuDeactive Then
            AnimasiSize -= Waktu.ElapsedGameTime.TotalMilliseconds / 200
            If AnimasiSize < 0 Then
                AnimasiSize = 0
                MenuTerpilih = MenuSelected
                MenuDeactive = False
            End If
        End If
    End Sub

    Private Function AnimasiHorizontal(ByRef Posisi() As Vector2, IsLeft As Boolean, Tujuan() As Vector2, Speed As Single) As Boolean
        For i = 0 To Posisi.Count - 1
            If Tujuan(i) <> Posisi(i) Then
                Dim diffVector = Vector2.Subtract(Tujuan(i), Posisi(i))
                diffVector.Normalize()
                diffVector = Vector2.Multiply(diffVector, Waktu.ElapsedGameTime.TotalMilliseconds * Speed)
                Posisi(i) = Vector2.Add(Posisi(i), diffVector)
            End If

            If IsLeft And Posisi(i).X < Tujuan(i).X Then
                Posisi(i).X = Tujuan(i).X
            ElseIf IsLeft = False And Posisi(i).X > Tujuan(i).X Then
                Posisi(i).X = Tujuan(i).X
            End If
        Next
        If Tujuan(0) = Posisi(0) And
           Tujuan(1) = Posisi(1) And
            Tujuan(2) = Posisi(2) Then
            Return True
        End If
        Return False
    End Function
End Class
