''' <summary>
''' This is the main type for your game
''' </summary>
Friend Class MainGame
    Inherits Game

    Friend WithEvents graphics As GraphicsDeviceManager
    Private TekstureMouse As Texture2D
    Private DebugFont As SpriteFont
    Private ItemScreens As New List(Of Screens)
    Private MenuSelected As MenuUtamaScreen.MenuUtama = MenuUtamaScreen.MenuUtama.Menu

    Friend Sub New()
        graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
        ContentManager = Content
    End Sub

    ''' <summary>
    ''' Allows the game to perform any initialization it needs to before starting to run.
    ''' This is where it can query for any required services and load any non-graphic
    ''' related content.  Calling MyBase.Initialize will enumerate through any components
    ''' and initialize them as well.
    ''' </summary>
    Protected Overrides Sub Initialize()
        Dim hWnd As IntPtr = Window.Handle
        Dim control = Windows.Forms.Control.FromHandle(hWnd)
        Dim form = control.FindForm()
        form.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        form.WindowState = Windows.Forms.FormWindowState.Maximized

        IsFixedTimeStep = True
        graphics.SynchronizeWithVerticalRetrace = True
        graphics.PreferredBackBufferWidth = GameSize.Width
        graphics.PreferredBackBufferHeight = GameSize.Height
        graphics.IsFullScreen = False
        graphics.ApplyChanges()
        MyBase.Initialize()
    End Sub

    ''' <summary>
    ''' LoadContent will be called once per game and is the place to load
    ''' all of your content.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        ' TODO: use Me.Content to load your game content here
        ' Create a new SpriteBatch, which can be used to draw textures.
        Sprite = New SpriteBatch(GraphicsDevice)

        TekstureMouse = ContentManager.Load(Of Texture2D)("gfx/one_finger")
        DebugFont = ContentManager.Load(Of SpriteFont)("Debug")
        Dim position As Vector2 = New Vector2(0, 20)
        Dim fpsCounter As FpsCounter = New FpsCounter(Me, DebugFont, position)
        'Dim gmp As GamePlay = New GamePlay(Me)
        'Dim t As BackgroundGamePlay = New BackgroundGamePlay(Me)
        't.Initialize()
        'Components.Add(t)
        'Components.Add(gmp)
        'gmp.Initialize()
        ItemScreens.Add(New MenuUtamaScreen("MenuUtama", True, True))
        Components.Add(fpsCounter)
    End Sub

    ''' <summary>
    ''' UnloadContent will be called once per game and is the place to unload
    ''' all content.
    ''' </summary>
    Protected Overrides Sub UnloadContent()
        ' TODO: Unload any non ContentManager content here
    End Sub

    ''' <summary>
    ''' Allows the game to run logic such as updating the world,
    ''' checking for collisions, gathering input, and playing audio.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        KeyboardStateAfter = Keyboard.GetState
        MouseStateAfter = Mouse.GetState
        'If IsActive Then
        '<---Exit game jika ditekan tombol Esc
        If MenuSelected = MenuUtamaScreen.MenuUtama.Keluar Then
            Keluar()
        End If
        '--->
        'GamePlayScreen.Update()
        'End If

        For Each i In ItemScreens
            i.Update(MenuSelected)
        Next

        If MenuSelected = MenuUtamaScreen.MenuUtama.Main Then
            ItemScreens.Remove(ItemScreens.Find(Function(x) x.Name = "MenuUtama"))
            Dim t1 = ItemScreens.FindAll(Function(x) x.Name = "Background")
            If t1.Count = 0 Then
                ItemScreens.Add(New BackgroundGamePlayScreen("Background", True, True))
            End If
            Dim t2 = ItemScreens.FindAll(Function(x) x.Name = "Gameplay")
            If t2.Count = 0 Then
                ItemScreens.Add(New GamePlay("Gameplay", True, True))
            End If
        ElseIf MenuSelected = MenuUtamaScreen.MenuUtama.Menu Then
            If ItemScreens.Count >= 2 Then
                ItemScreens.Clear()
            End If
            Dim t = ItemScreens.FindAll(Function(x) x.Name = "MenuUtama")
            If t.Count = 0 Then
                ItemScreens.Add(New MenuUtamaScreen("MenuUtama",True,True))
            End If
        End If

        Waktu = gameTime
        MouseStateBefore = Mouse.GetState
        KeyboardStateBefore = Keyboard.GetState
        MyBase.Update(gameTime)
    End Sub

    ''' <summary>
    ''' This is called when the game should draw itself.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(New Color(238, 238, 238))
        ' TODO: Add your drawing code here

        Sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend)
        'GamePlayScreen.Draw()
        For Each i In ItemScreens
            i.Draw()
        Next
        Sprite.Draw(TekstureMouse, New Rectangle(MouseStateBefore.X, MouseStateBefore.Y, 36, 36), Nothing, New Color(255, 255, 255), 0, Nothing, SpriteEffects.None, 1)

        Sprite.End()
        MyBase.Draw(gameTime)
    End Sub

    Friend Sub Keluar()
        [Exit]()
    End Sub
End Class
