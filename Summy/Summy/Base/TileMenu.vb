Imports Summy

Friend Class TileMenu

    Friend Property Position As Vector2
    Friend Property Colorback
    Friend Property Size As Sizes
    Friend Property TrigerByOnTop As Boolean
    Friend Property IsActive As Boolean
    Friend Property MenuValue As Menus = Menus.None
    Public Property TrigerClick As Boolean
    Private AmountUkuran As Single = 0
    Private AmountWarna As Single = 0
    Private TileDitahan As Boolean

    Friend Enum Menus
        None
        Tukar
        Kembalikan
        Lanjut
    End Enum


    Friend Sub New(Position As Vector2, Size As Sizes, Colorback As Color, IsActive As Boolean, Menu As Menus)
        Me.Position = Position
        Me.Size = Size
        Me.Colorback = Colorback
        Me.IsActive = IsActive
        MenuValue = Menu
    End Sub

    Friend Sub Draw(BaseTexture As Texture2D, ContentTexture As Texture2D, ShadowTexture As Texture2D, FontTexture As SpriteFont)
        'Shadow
        If TrigerByOnTop Then
            Sprite.Draw(ShadowTexture, New Rectangle(Position.X, Position.Y, Size.Width / (ShadowTexture.Width - 36) * ShadowTexture.Width, Size.Height / (ShadowTexture.Height - 36) * ShadowTexture.Height), New Rectangle(0, 0, ShadowTexture.Width, ShadowTexture.Height), New Color(33, 33, 33), 0, New Vector2(ShadowTexture.Width / 2, ShadowTexture.Height / 2), SpriteEffects.None, 0)
        End If
        'Background button
        Sprite.Draw(BaseTexture, New Rectangle(Position.X, Position.Y, Size.Width + AmountUkuran, Size.Height + AmountUkuran), New Rectangle(0, 0, BaseTexture.Width, BaseTexture.Height), New Color(117, 117, 117), 0, New Vector2(BaseTexture.Width / 2, BaseTexture.Height / 2), SpriteEffects.None, 0)
        'Content 
        Sprite.Draw(ContentTexture, New Rectangle(Position.X, Position.Y, 0.75 * Size.Width + AmountUkuran, 0.75 * Size.Height + AmountUkuran), New Rectangle(0, 0, ContentTexture.Width, ContentTexture.Height), Color.Lerp(Color.White, Color.Black, WarnaBackground - 0.25), 0, New Vector2(ContentTexture.Width / 2, ContentTexture.Height / 2), SpriteEffects.None, 0)
        'Text Show if ontop
        If TrigerByOnTop Then
            Sprite.DrawString(FontTexture, MenuValue.ToString, Position - New Vector2(0, 48), New Color(100, 100, 100), 0, New Vector2(FontTexture.MeasureString(MenuValue.ToString).X / 2, FontTexture.MeasureString(MenuValue.ToString).Y / 2), 0.16F, SpriteEffects.None, 0)
        End If

        If TileDitahan And MenuValue = Menus.Tukar Then
            Sprite.DrawString(FontTexture, "letak disini" & vbNewLine & "untuk tukar->", Position - New Vector2(110, 0), New Color(100, 100, 100), 0, New Vector2(FontTexture.MeasureString("letak disini" & vbNewLine & "untuk tukar->").X / 2, FontTexture.MeasureString("letak disini" & vbNewLine & "untuk tukar->").Y / 2), 0.14F, SpriteEffects.None, 0)
        End If
    End Sub

    Friend Sub Update(ByRef ProsesMenuYangDipilih As Menus, ByVal MouseIsHolding As Boolean, ByRef ValueOfHolding As UShort, ByRef DeleteOnTile As Boolean, ByVal Giliran As GamePlay.PlayerTurn, ByVal FasePlayer As GamePlay.PhasePlayer)
        If New Rectangle(Position.X - Size.Width / 2, Position.Y - Size.Height / 2, Size.Width, Size.Height).Contains(MouseStateBefore.X, MouseStateBefore.Y) And Giliran = GamePlay.PlayerTurn.Human And (FasePlayer = GamePlay.PhasePlayer.Running Or FasePlayer = GamePlay.PhasePlayer.Finish) Then
            TrigerByOnTop = True
        Else
            TrigerByOnTop = False
        End If

        If MenuValue = Menus.Tukar Then
            If TrigerByOnTop And MouseIsHolding And MouseStateBefore.LeftButton = ButtonState.Released And MouseStateAfter.LeftButton = ButtonState.Released Then
                TrigerClick = True
                DeleteOnTile = True
            End If
        Else
            If TrigerByOnTop Then
                If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released And FasePlayer = GamePlay.PhasePlayer.Running Then
                    TrigerClick = True
                    'ProsesMenuYangDipilih = MenuValue
                End If
            End If
        End If

        If TrigerClick Then
            If AmountUkuran < 24 Then
                AmountUkuran += Waktu.ElapsedGameTime.TotalMilliseconds / 5
                If AmountUkuran > 24 Then
                    TrigerClick = False
                    If MenuValue <> Menus.Tukar Then
                        ProsesMenuYangDipilih = MenuValue
                    End If
                    AmountUkuran = 1
                End If
            End If
        End If

        If TrigerByOnTop Then
            If AmountUkuran < 12 Then
                AmountUkuran += Waktu.ElapsedGameTime.TotalMilliseconds / 10
                If AmountUkuran > 12 Then
                    AmountUkuran = 12
                End If
            End If

            If AmountWarna < 1 Then
                AmountWarna += Waktu.ElapsedGameTime.TotalMilliseconds / 400
                If AmountWarna > 1 Then
                    AmountWarna = 1
                End If
            End If
        Else
            If AmountUkuran > 1 Then
                AmountUkuran -= Waktu.ElapsedGameTime.TotalMilliseconds / 10
                If AmountUkuran < 1 Then
                    AmountUkuran = 1
                End If
            End If

            If AmountWarna > 0 Then
                AmountWarna -= Waktu.ElapsedGameTime.TotalMilliseconds / 1000
                If AmountWarna < 0 Then
                    AmountWarna = 0
                End If
            End If
        End If

        TileDitahan = MouseIsHolding
    End Sub
End Class
