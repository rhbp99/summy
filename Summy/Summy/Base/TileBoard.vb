Imports Summy

Friend Class TileBoard
#Region "Deklarasi Properti"
    Friend Property Size As Sizes
    Friend Property Position As Vector2
    Friend Property Value As UShort
    Friend Property Colorback As Color
    Friend Property IsBlocked As Boolean
    Friend Property IsActive As Boolean
    Friend Property IsWrongPlace As BenarSalahNetral
    Friend Property TrigerByOnTop As Boolean
    Friend Property TrigerByHold As Boolean
    Friend Property TrigerByRelease As Boolean
    Friend Property IsOpen As Boolean
    Friend Property IsDeadH As Boolean
    Friend Property IsDeadV As Boolean
    Friend Property IsCheckedH As Boolean
    Friend Property IsCheckedV As Boolean
    Friend Property IsHighlighted As Boolean = False
    Friend Property IsTrigerAnimation As Boolean = False
    Private AmountColorRO As Single = 1
    Private AmountColor As Single
    Private AmountColorR As Single = 1
    Private AmountSize As Single = 0
    Private ColorNow As Color
    Private AnimasiWarnaAktif As Boolean
    Private Timer As Int16 = 5
    Private NotEmptyTile As Boolean = False
    Private Owned As Own
    Private _FasePlayer As GamePlay.PhasePlayer
    Private _Posisi As Vector2
    Private IsAnimasiAktif As Boolean

    Friend Enum Own
        PlayerOne
        PlayerCPU
        Board
    End Enum
#End Region

    Friend Sub New(Position As Vector2, Size As Sizes, Colorback As Color, Active As Boolean, Value As UShort, Owned As Own)
        Me.Position = Position
        Me.Size = Size
        Me.Colorback = Colorback
        IsActive = Active
        Me.Value = Value
        Me.Owned = Owned
    End Sub

    Friend Sub Draw(BaseTexture As Texture2D, font As SpriteFont, ShadowTexture As Texture2D)
        Dim warnadasar As Color = Colorback
        Dim warnaforedasar As Color = New Color(97, 97, 97)
        Dim posisi As Vector2 = Position

        If Not Owned = Own.Board And IsTrigerAnimation AndAlso _FasePlayer = GamePlay.PhasePlayer.FinishNotif Then
            posisi = _Posisi
        End If

        Dim warna As Color = AnimasiWarna(warnadasar, ColorNow, AmountColor)
        Dim warnafore As Color = AnimasiWarna(warnaforedasar, New Color(66, 66, 66), AmountColor)

        If TrigerByOnTop Then
            Dim spesialcolor As Color = New Color(238, 238, 238)
            If NotEmptyTile Then
                spesialcolor = New Color(239, 154, 154)
            End If
            warna = AnimasiWarna(spesialcolor, warnadasar, AmountColorR)
            warnafore = AnimasiWarna(New Color(66, 66, 66), warnaforedasar, AmountColorR)


        End If

        If TrigerByHold Then
            posisi = New Vector2(MouseStateBefore.X, MouseStateBefore.Y)
            warna = AnimasiWarna(New Color(224, 224, 224), New Color(238, 238, 238), AmountColor)
            warnafore = New Color(66, 66, 66)
        End If

        If IsOpen And Not IsBlocked Then
            If IsWrongPlace = BenarSalahNetral.Netral Then
                warnadasar = New Color(144, 202, 249)
            ElseIf IsWrongPlace = BenarSalahNetral.Benar Then
                warnadasar = New Color(105, 240, 174)
            End If
            warna = AnimasiWarna(warnadasar, Color.White, AmountColorRO)
            warnafore = AnimasiWarna(Color.White, warnaforedasar, AmountColorRO)
        End If

        If IsWrongPlace = BenarSalahNetral.Salah And Not IsBlocked Then
            warna = AnimasiWarna(Color.White, New Color(239, 154, 154), AmountColorRO)
            warnafore = AnimasiWarna(warnaforedasar, Color.White, AmountColorRO)
        End If

        If IsBlocked Then
            If Value <> 127 Then
                warnadasar = New Color(158, 158, 158)
            ElseIf Value = 127 Then
                warnadasar = New Color(97, 97, 97)
            End If
            warna = AnimasiWarna(warnadasar, If(Value = 127, Color.White, warnadasar), AmountColorRO)
            warnafore = AnimasiWarna(Color.White, If(Value = 127, Color.White, Color.White), AmountColorRO)
        End If

        If IsHighlighted Then
            warna = New Color(224, 224, 224)
            warnafore = New Color(97, 97, 97)
            AmountSize = 0.04
        End If
        If AnimasiWarnaAktif Then
            Sprite.Draw(ShadowTexture, New Rectangle(posisi.X, posisi.Y, (Size.Width / (ShadowTexture.Width - 24) * ShadowTexture.Width) * AmountSize * 2.67, (Size.Height / (ShadowTexture.Height - 24) * ShadowTexture.Height) * AmountSize * 2.67), New Rectangle(0, 0, ShadowTexture.Width, ShadowTexture.Height), New Color(117, 117, 117), 0, New Vector2(ShadowTexture.Width / 2, ShadowTexture.Height / 2), SpriteEffects.None, 0)
        End If
        Sprite.Draw(BaseTexture, New Rectangle(posisi.X, posisi.Y, Size.Width * (1 + AmountSize), Size.Height * (1 + AmountSize)), New Rectangle(0, 0, BaseTexture.Width, BaseTexture.Height), warna, 0, New Vector2(BaseTexture.Width / 2, BaseTexture.Height / 2), SpriteEffects.None, 0)
        Sprite.DrawString(font, CharValue, New Vector2(posisi.X, posisi.Y), warnafore, 0, New Vector2(font.MeasureString(CharValue).X / 2, font.MeasureString(CharValue).Y / 2), (1 + AmountSize) / (4 * 36 / Size.Width), SpriteEffects.None, 0)
    End Sub

    Friend Sub Update(ByRef MouseIsHolding As Boolean, ByRef ValueOfHolding As UShort, ByRef ReleaseOnTile As Boolean, ByVal Giliran As GamePlay.PlayerTurn, ByVal FasePlayer As GamePlay.PhasePlayer)
        _FasePlayer = FasePlayer


        If Not Owned = Own.PlayerCPU Then

            If IsBlocked Then
                If AmountColorRO > 0 Then
                    AmountColorRO -= Waktu.ElapsedGameTime.TotalMilliseconds / 800
                End If
            End If

            'Triger jika mouse berada di atas tile
            If Not IsBlocked And New Rectangle(Position.X - Size.Width / 2, Position.Y - Size.Height / 2, Size.Width, Size.Height).Contains(MouseStateBefore.X, MouseStateBefore.Y) And TrigerByHold = False And Giliran = GamePlay.PlayerTurn.Human AndAlso FasePlayer = GamePlay.PhasePlayer.Running Then
                If IsActive Then
                    TrigerByOnTop = True
                    AnimasiWarnaAktif = True
                    ColorNow = New Color(238, 238, 238)
                    AmountColor = 1
                    If MouseIsHolding Then
                        If Value <> 127 Then
                            NotEmptyTile = True
                            ColorNow = New Color(239, 154, 154)
                        End If
                    Else
                        NotEmptyTile = False
                    End If
                Else
                    If MouseIsHolding Then
                        TrigerByOnTop = False
                        AmountColorR = 1
                    Else
                        TrigerByOnTop = True
                        AnimasiWarnaAktif = True
                        ColorNow = New Color(238, 238, 238)
                        AmountColor = 1
                    End If
                End If
            Else
                TrigerByOnTop = False
                AmountColorR = 1
                NotEmptyTile = False
            End If

            'Triger jika tombol mouse ditekan
            If Not IsBlocked And Value < 127 And TrigerByOnTop And MouseIsHolding = False And MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Pressed Then
                TrigerByHold = True
                MouseIsHolding = True
                ValueOfHolding = Value
            End If

            'Triger jika tombol mouse dilepas
            If Not IsBlocked And Value = 127 And IsActive And TrigerByOnTop And MouseIsHolding And MouseStateBefore.LeftButton = ButtonState.Released And MouseStateAfter.LeftButton = ButtonState.Released Then
                Value = ValueOfHolding
                ReleaseOnTile = True
            End If


            If AnimasiWarnaAktif And TrigerByOnTop = False Then
                AmountColor -= Waktu.ElapsedGameTime.TotalMilliseconds / 800
                If AmountColor < 0 Then
                    AnimasiWarnaAktif = False
                    ColorNow = New Color(238, 238, 238)
                End If
            End If

            If TrigerByOnTop Or TrigerByHold Then
                If AmountColorR > 0 Then
                    AmountColorR -= Waktu.ElapsedGameTime.TotalMilliseconds / 400
                    If AmountColorR < 0 Then
                        AmountColorR = 0
                    End If
                End If

                If TrigerByOnTop And AmountSize < 0.6 Then
                    AmountSize += Waktu.ElapsedGameTime.TotalMilliseconds / 600
                    If AmountSize > 0.6 Then
                        AmountSize = 0.6
                    End If
                End If

                If TrigerByHold And AmountSize > 0.1 Then
                    AmountSize -= Waktu.ElapsedGameTime.TotalMilliseconds / 600
                    If AmountSize < 0.1 Then
                        AmountSize = 0.1
                    End If
                End If
            Else
                If AmountSize > 0 Then
                    AmountSize -= Waktu.ElapsedGameTime.TotalMilliseconds / 600
                    If AmountSize < 0 Then
                        AmountSize = 0
                    End If
                End If
            End If

            If IsOpen Then
                If AmountColorRO > 0 Or IsWrongPlace = BenarSalahNetral.Salah Then
                    AmountColorRO -= Waktu.ElapsedGameTime.TotalMilliseconds / 800
                    If AmountColorRO < 0 Then
                        AmountColorRO = 0
                    End If
                End If
            End If

            If TrigerByOnTop Then
                If KeyboardStateBefore.IsKeyDown(Keys.Down) And KeyboardStateBefore.IsKeyDown(Keys.RightControl) Then
                    If Value > 0 Then
                        Value -= 1
                    End If
                    If Value <= 0 Then
                        Value = 126
                    End If
                End If
                If KeyboardStateBefore.IsKeyDown(Keys.Up) And KeyboardStateBefore.IsKeyDown(Keys.RightControl) Then
                    Value += 1
                    If Value > 127 Then
                        Value = 0
                    End If
                End If
            End If

        End If

        If IsTrigerAnimation = True And _FasePlayer = GamePlay.PhasePlayer.Standby Then
            _Posisi = Position
            IsTrigerAnimation = False
            IsAnimasiAktif = False
        ElseIf Not Owned = Own.Board And IsAnimasiAktif = False And IsTrigerAnimation AndAlso _FasePlayer = GamePlay.PhasePlayer.FinishNotif Then
            _Posisi = New Vector2(Center.Width - 458, Center.Height + 312)
            IsAnimasiAktif = True
        ElseIf Not Owned = Own.Board And IsAnimasiAktif = True And IsTrigerAnimation AndAlso _FasePlayer = GamePlay.PhasePlayer.FinishNotif Then
            Animasi(Position, _Posisi)
        End If
    End Sub

#Region "Tempat Fungsi Tambahan"
    Private Function ConvertValue(Value As UShort) As UShort
        Select Case Value
            Case 0 To 7
                Return 0
            Case 8 To 15
                Return 1
            Case 16 To 23
                Return 2
            Case 24 To 31
                Return 3
            Case 32 To 39
                Return 4
            Case 40 To 47
                Return 5
            Case 48 To 55
                Return 6
            Case 56 To 63
                Return 7
            Case 64 To 71
                Return 8
            Case 72 To 79
                Return 9
            Case 80 To 86
                Return 10
            Case 87 To 93
                Return 11
            Case 94 To 100
                Return 12
            Case 101 To 107
                Return 13
            Case 108 To 126
                Return 14
            Case Else
                Return 15
        End Select
    End Function

    Friend ReadOnly Property CharValue As Char
        Get
            Select Case Value
                Case 0 To 7
                    Return "0"
                Case 8 To 15
                    Return "1"
                Case 16 To 23
                    Return "2"
                Case 24 To 31
                    Return "3"
                Case 32 To 39
                    Return "4"
                Case 40 To 47
                    Return "5"
                Case 48 To 55
                    Return "6"
                Case 56 To 63
                    Return "7"
                Case 64 To 71
                    Return "8"
                Case 72 To 79
                    Return "9"
                Case 80 To 86
                    Return "+"
                Case 87 To 93
                    Return "-"
                Case 94 To 100
                    Return "x"
                Case 101 To 107
                    Return ":"
                Case 108 To 126
                    Return "="
                Case Else
                    Return " "
            End Select
        End Get
    End Property

    Private Function AnimasiWarna(ColorStart As Color, ColorEnd As Color, Amount As Single) As Color
        Return Color.Lerp(ColorStart, ColorEnd, Amount)
    End Function

    Private Sub Animasi(Tujuan As Vector2, ByRef Posisi As Vector2)
        If Tujuan <> Posisi Then
            Dim diffVector = Vector2.Subtract(Tujuan, Posisi)
            diffVector.Normalize()
            diffVector = Vector2.Multiply(diffVector, Waktu.ElapsedGameTime.TotalMilliseconds * 0.5)
            Posisi = Vector2.Add(Posisi, diffVector)

            If Owned = Own.PlayerOne Then
                If Posisi.X > Tujuan.X Then
                    Posisi.X = Tujuan.X
                End If

                If Posisi.Y > Tujuan.Y Then
                    Posisi.Y = Tujuan.Y
                End If
            Else
                If Posisi.X > Tujuan.X Then
                    Posisi.X = Tujuan.X
                End If

                If Posisi.Y < Tujuan.Y Then
                    Posisi.Y = Tujuan.Y
                End If
            End If
        End If
    End Sub

#End Region
End Class
