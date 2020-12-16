''' <summary>
''' This is a game component that implements IUpdateable.
''' </summary>
Public Class BackgroundGamePlayScreen
    Inherits Screens

    Private backgroundTileBoard As Texture2D
    Private backTile As Texture2D
    Private cpu As Texture2D
    Private player As Texture2D
    Private stacktile As Texture2D
    Private star As Texture2D
    Private history As Texture2D
    Private drophere As Texture2D

    Public Overrides Property Name As String
    Public Overrides Property Enable As Boolean
    Public Overrides Property Visible As Boolean

    Public Sub New(Name As String, Enable As Boolean, Visible As Boolean)
        Me.Name = Name
        Me.Enable = Enable
        Me.Visible = Visible

        Initialize()
        Load()
    End Sub

    ''' <summary>
    ''' Allows the game component to perform any initialization it needs to before starting
    ''' to run.  This is where it can query for any required services and load content.
    ''' </summary>
    Public Overrides Sub Initialize()
        ' TODO: Add your initialization code here
        backgroundTileBoard = ContentManager.Load(Of Texture2D)("gfx/backgroundTileBoard")
        backTile = ContentManager.Load(Of Texture2D)("gfx/backTile")
        cpu = ContentManager.Load(Of Texture2D)("gfx/cpu")
        player = ContentManager.Load(Of Texture2D)("gfx/player")
        stacktile = ContentManager.Load(Of Texture2D)("gfx/stack")
        star = ContentManager.Load(Of Texture2D)("gfx/star")
        history = ContentManager.Load(Of Texture2D)("gfx/history")
        drophere = ContentManager.Load(Of Texture2D)("gfx/drophere")
        'MyBase.Initialize()
    End Sub

    Public Overrides Sub Update(ByRef MenuSelected As MenuUtamaScreen.MenuUtama)
        ' TODO: Add your update code here
        'MyBase.Update(gameTime)
    End Sub

    Public Overrides Sub Draw()
        'Sprite.Begin()

        'Background Tileboard
        'pos(center, center-40)
        Sprite.Draw(backgroundTileBoard, New Rectangle(Center.Width, Center.Height - 36, backgroundTileBoard.Width + 42, backgroundTileBoard.Height + 42), New Rectangle(0, 0, backgroundTileBoard.Width, backgroundTileBoard.Height), New Color(250, 250, 250), 0, New Vector2(backgroundTileBoard.Width / 2, backgroundTileBoard.Height / 2), SpriteEffects.None, 0)
        'Background bottom
        'pos(center-262, center+270) | pos(center+34, center+309) 
        'Sprite.Draw(drophere, New Rectangle(Center.Width - 262, Center.Height + 285, 103, 14), New Rectangle(0, 0, drophere.Width, drophere.Height), Color.White, 0, New Vector2(drophere.Width / 2, drophere.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width, Center.Height + 324, 398, 60), New Rectangle(0, 0, backTile.Width, backTile.Height), Color.Lerp(New Color(189, 189, 189), Color.Black, WarnaBackground), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        'Background left banner
        banner(-10, True, 1)
        'Background right banner
        '938
        banner(-10, False, -1)
        'Background stack
        Sprite.Draw(backTile, New Rectangle(Center.Width - 410, Center.Height + 312, 144, 48), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(66, 66, 66), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(stacktile, New Rectangle(Center.Width - 458, Center.Height + 312, stacktile.Width, stacktile.Height), New Rectangle(0, 0, stacktile.Width, stacktile.Height), Color.White, 0, New Vector2(stacktile.Width / 2, stacktile.Height / 2), SpriteEffects.None, 0)

        'Sprite.End()
        'MyBase.Draw(GameTime)
    End Sub

    Private Sub banner(x As Integer, left As Boolean, side As Integer)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 469 * side + x * side, Center.Height - 33, 282, 617), New Rectangle(0, 0, backTile.Width, backTile.Height), If(left, New Color(134, 134, 134), New Color(189, 189, 189)), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

        Sprite.Draw(backTile, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 300, 56, 56), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(250, 250, 250), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(If(left, cpu, player), New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 300, If(left, cpu, player).Width, If(left, cpu, player).Height), New Rectangle(0, 0, If(left, cpu, player).Width, If(left, cpu, player).Height), Color.White, 0, New Vector2(If(left, cpu, player).Width / 2, If(left, cpu, player).Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 436 * side + x * side, Center.Height - 300, 196, 56), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(250, 250, 250), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 366 * side + x * side, Center.Height - 314, 56, 27), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(97, 97, 97), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 366 * side + x * side, Center.Height - 285, 56, 27), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(97, 97, 97), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

        Sprite.Draw(backTile, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 268, 56, 8), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(117, 117, 117), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Dim space = 0
        For i = 0 To 7
            Sprite.Draw(backTile, New Rectangle(Center.Width - 349 * side + x * side + space, Center.Height - 251, 22, 22), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(250, 250, 250), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
            space -= 22 * side + 3 * side
        Next

        Sprite.Draw(backTile, New Rectangle(Center.Width - 469 * side + x * side, Center.Height - 219, 262, 22), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(97, 97, 97), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

        Sprite.Draw(backTile, New Rectangle(Center.Width - 469 * side + x * side, Center.Height + 29, 262, 454), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(238, 238, 238), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

        Sprite.Draw(backTile, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 170, 36, 36), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(117, 117, 117), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(star, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 170, 28, 28), New Rectangle(0, 0, star.Width, star.Height), Color.White, 0, New Vector2(star.Width / 2, star.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 446 * side + x * side, Center.Height - 170, 196, 12), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(117, 117, 117), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

        Sprite.Draw(backTile, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 112, 36, 36), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(117, 117, 117), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(history, New Rectangle(Center.Width - 572 * side + x * side, Center.Height - 112, 28, 28), New Rectangle(0, 0, history.Width, history.Height), Color.White, 0, New Vector2(history.Width / 2, history.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(backTile, New Rectangle(Center.Width - 446 * side + x * side, Center.Height - 112, 196, 12), New Rectangle(0, 0, backTile.Width, backTile.Height), New Color(117, 117, 117), 0, New Vector2(backTile.Width / 2, backTile.Height / 2), SpriteEffects.None, 0)

    End Sub

    Public Overrides Sub Load()
    End Sub

End Class
