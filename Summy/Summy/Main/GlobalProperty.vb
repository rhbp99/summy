Module GlobalProperty
    Friend Sprite As SpriteBatch
    Friend ContentManager As Content.ContentManager
    'Friend GameSize As New Sizes(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9)
    Friend ReadOnly GameSize As New Sizes(1366, 768)
    Friend ReadOnly Center As New Sizes(GameSize.Width / 2, GameSize.Height / 2)
    Friend MouseStateBefore As MouseState
    Friend MouseStateAfter As MouseState
    Friend KeyboardStateBefore As KeyboardState
    Friend KeyboardStateAfter As KeyboardState
    Friend RandomNumber As New Random
    Friend Waktu As New GameTime
    Friend DaftarKandidatH As New List(Of HasilPermutasi)
    Friend DaftarKandidatV As New List(Of HasilPermutasi)
    Friend CPUHState As StatusCPU = StatusCPU.Ready
    Friend CPUVState As StatusCPU = StatusCPU.Ready
    Friend CPUPlaceSet As Boolean = False
    Friend AktifCPU As Boolean = False
    Friend ReadOnly Kunci As Object = New Object
    Friend ReadOnly Kunci1 As Object = New Object
    Friend ReadOnly Kunci2 As Object = New Object
    Friend ReadOnly Kunci3 As Object = New Object
    Friend ReadOnly BatasWaktuTunggu As Integer = 5
    Friend WaktuTungguBerjalan As Single = 0
    Friend WarnaBackground As Single = 0F

    Friend Sub GantiWarnaBackground(ByVal GiliranPlayer As GamePlay.PlayerTurn)
        If GiliranPlayer = GamePlay.PlayerTurn.CPU Then
            WarnaBackground = 0.5F
        Else
            WarnaBackground = 0
        End If
    End Sub

    Friend Sub ResetUlang()
        MouseStateBefore = New MouseState
        MouseStateAfter = New MouseState
        KeyboardStateBefore = New KeyboardState
        KeyboardStateAfter = New KeyboardState
        Waktu = New GameTime
        DaftarKandidatH = New List(Of HasilPermutasi)
        DaftarKandidatV = New List(Of HasilPermutasi)
        CPUHState = StatusCPU.Ready
        CPUVState = StatusCPU.Ready
        CPUPlaceSet = False
        WaktuTungguBerjalan = 0
        AktifCPU = False
    End Sub
End Module

Friend Structure Sizes
    Friend Width As Single
    Friend Height As Single
    Friend Sub New(Width As Single, Height As Single)
        Me.Width = Width
        Me.Height = Height
    End Sub
End Structure

Friend Enum BenarSalahNetral
    Netral
    Benar
    Salah
End Enum

Friend Enum StatusCPU
    Ready
    Running
    Finish
End Enum

Friend Structure HasilPermutasi
    Friend Nilai As Integer
    Friend Persamaan As String
    Friend Sub New(Nilai As Integer, Persamaan As String)
        Me.Nilai = Nilai
        Me.Persamaan = Persamaan
    End Sub
End Structure

