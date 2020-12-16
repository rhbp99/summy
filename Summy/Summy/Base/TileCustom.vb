Imports Summy

Friend Class TileCustom

    Private Property IsWrongPlace As BenarSalahNetral
    Private Property IsActive As Boolean
    Private Property IsBlocked As Boolean
    Private ReadOnly Property CharValue As Char
    Friend Property Colorback As Color
    Friend Property IsOpen As Boolean
    Friend Property Position As Vector2
    Friend Property Size As Sizes
    Friend Property TrigerByHold As Boolean
    Friend Property TrigerByOnTop As Boolean
    Friend Property TrigerByRelease As Boolean
    Private ValueAnimasiFrame As Single = 0
    Private ValueAnimasiColor As Single = 0

    Friend Sub New(ByVal Position As Vector2, ByVal Size As Sizes)
        Me.Position = Position
        Me.Size = Size
        TrigerByOnTop = False
    End Sub

    Friend Sub Draw(BaseTexture As Texture2D, ContentTexture As Texture2D)
        Sprite.Draw(ContentTexture, New Rectangle(Position.X, Position.Y, Size.Width, Size.Height), New Rectangle(136 * CInt(Math.Floor(ValueAnimasiFrame)), 0, 136, 144), Color.Lerp(Color.White, Color.LightCoral, ValueAnimasiColor), 0, Nothing, SpriteEffects.None, 0)
    End Sub

    Friend Sub Update(ByRef MouseIsHolding As Boolean, ByRef ValueOfHolding As UShort, ByRef ReleaseOnTile As Boolean)
        If (MouseStateBefore.X > Position.X And MouseStateBefore.X < Position.X + Size.Width) And
               (MouseStateBefore.Y > Position.Y And MouseStateBefore.Y < Position.Y + Size.Height) Then
            TrigerByOnTop = True
        Else
            TrigerByOnTop = False
        End If

        If TrigerByOnTop And MouseIsHolding And MouseStateBefore.LeftButton = ButtonState.Released And MouseStateAfter.LeftButton = ButtonState.Released Then
            ReleaseOnTile = True
        End If

        If TrigerByOnTop And MouseIsHolding Then
            If ValueAnimasiFrame < 8 Then
                ValueAnimasiFrame += Waktu.ElapsedGameTime.TotalMilliseconds / 30
                ValueAnimasiColor += Waktu.ElapsedGameTime.TotalMilliseconds / 300
            End If
            If ValueAnimasiFrame > 8 Then
                ValueAnimasiFrame = 8
                ValueAnimasiColor = 1
            End If

        Else
            If ValueAnimasiFrame > 0 Then
                ValueAnimasiFrame -= Waktu.ElapsedGameTime.TotalMilliseconds / 30
                ValueAnimasiColor -= Waktu.ElapsedGameTime.TotalMilliseconds / 300
            End If
            If ValueAnimasiFrame < 0 Then
                ValueAnimasiFrame = 0
                ValueAnimasiColor = 0
            End If
        End If
    End Sub

End Class
