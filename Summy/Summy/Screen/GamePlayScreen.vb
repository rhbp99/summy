Imports System.Text
Imports System.Threading.Tasks
''' <summary>
''' This is a game component that implements IUpdateable.
''' </summary>
Public Class GamePlay
    Inherits Screens
#Region "Deklarasi Properti"
    Private FaseGameScreen As PhasePlayer = PhasePlayer.Standby
    Private greedy As New GreedyProcess
    Private TileBoard(24, 24) As TileBoard
    Private TileMenu(2) As TileMenu
    Private BaseTileTexture As Texture2D
    Private BasePlainTexture As Texture2D
    Private ShadowTexture As Texture2D
    Private DebugFont As SpriteFont
    Private SwapTexture As Texture2D
    Private ClearTexture As Texture2D
    Private DoneTexture As Texture2D
    Private backTileTexture As Texture2D
    Private TransparentDimTexture As Texture2D
    Private MouseIsHolding As Boolean = False
    Private ValueOfHolding As UInt16 = 127
    Private IndexTileBoardOfHolding As New Vector2(-1, -1)
    Private IndexTilePlayerOfHolding As Int16 = -1
    Private ReleaseOnTile As Boolean = False
    Private DeleteOnTile As Boolean = False
    Private ProsesMenuDiPilih As TileMenu.Menus = Summy.TileMenu.Menus.None
    Private ReadOnly Colour As Color = New Color(250, 250, 250)
    Private StatusCekBacaH As Boolean = False
    Private StatusCekBacaV As Boolean = False
    Private TempScoreH As Integer = 0
    Private TempScoreV As Integer = 0
    Private FirstTimeAmbilYgDicekH As Boolean = True
    Private FirstTimeAmbilYgDicekV As Boolean = True
    Private Bhs As New Bahasa
    Private ListKoordinatBacaH As New List(Of Vector2)
    Private ListKoordinatBacaV As New List(Of Vector2)
    Private KoordinatAsalH As Vector2 = New Vector2(-1, -1)
    Private KoordinatAsalV As Vector2 = New Vector2(-1, -1)
    Friend Property PertahankanKoordinatAwalH As Boolean = False
    Friend Property PertahankanKoordinatAwalV As Boolean = False
    Friend Property KPembantuH As New List(Of Vector2)
    Friend Property KPembantuV As New List(Of Vector2)
    Public Property MenuOKAktif As Boolean
    Private Property WaktuPostCPU As Single = 2
    Private Property WaktuPreCPU As Single = 2
    Private Property TImeRunTemp As Single = 0

    Public Overrides Property Name As String
    Public Overrides Property Enable As Boolean
    Public Overrides Property Visible As Boolean

    'Private DaftarYangMauDisusunH As New List(Of String)
    'Private DaftarYangMauDisusunV As New List(Of String)
    Private Sudut As Integer = Nothing
    Private Ring As Integer = Nothing
    Private Counter As Integer = Nothing
    Private TempAsalH As New Vector2
    Private TempAsalV As New Vector2
    Private KandidatSourceCPUAIH As New List(Of KandidatCPUBaseKoordinat)
    Private KandidatSourceCPUAIV As New List(Of KandidatCPUBaseKoordinat)
    Dim KH As Integer = -1
    Dim KV As Integer = -1
    Private SolusiH As String = ""
    Private SolusiV As String = ""
    Private NonAktifCPUH = False
    Private NonAktifCPUV = False
    Private Pemain(1) As Player
    Private GiliranPlayer As PlayerTurn
    Private Shared ReadOnly Kunci As New Object
    Private Shared ReadOnly Kunci2 As New Object
    Private StacksTile As New List(Of Integer)
    Private TempDftrV As New List(Of Vector2)
    Private TempDftrH As New List(Of Vector2)
    Private MessageBoxTexture As Texture2D
    Private AnimasiMessageBoxMenuUtama As Single
    Private AnimasiMessageBoxMainLagi As Single
    Private SearchTexture As Texture2D
    Private lineaquare As Texture2D
    Private PauseTexture As Texture2D
    Private DownyTexture As Texture2D
    Private UppyTexture As Texture2D
    Private ExitButton As TombolSederhana
    Private animasiExitButton As Single = 0.8
    Private AnimasiMessageBoxLanjutkan As Double

    Friend Structure TombolSederhana
        Friend Texture As Texture2D
        Friend Posisi As Vector2
        Friend OnTop As Boolean
    End Structure

    Friend Structure ResultHasilPermutasi
        Friend CPUBaseKoordinat As KandidatCPUBaseKoordinat
        Friend KandidatPembantu As List(Of Vector2)
        Friend DaftarYgDisusun As List(Of String)
        Friend HasilPermutasi As List(Of HasilPermutasi)

        Sub New(CPUBaseKoordinat As KandidatCPUBaseKoordinat, KandidatPembantu As List(Of Vector2), DaftarYgDisusun As List(Of String), HasilPermutasi As List(Of HasilPermutasi))
            Me.CPUBaseKoordinat = CPUBaseKoordinat
            Me.KandidatPembantu = KandidatPembantu
            Me.DaftarYgDisusun = DaftarYgDisusun
            Me.HasilPermutasi = HasilPermutasi
        End Sub
    End Structure

    Enum PlayerTurn
        Human
        CPU
    End Enum

    Friend Structure Player
        Friend Score As Integer
        Friend RecentScore As Integer
        Friend ScorePosNotif As Vector2
        Friend Fase As PhasePlayer
        Friend RecentMove As List(Of List(Of Vector2))
        Friend RecentMoveHightlight As List(Of Boolean)
        Friend TopMove As List(Of Vector2)
        Friend TopMoveHighlight As Boolean
        Friend TileYangDiPegang() As TileBoard
        Friend DetikBerjalan As Single
        Friend AkumulasiDetikBerjalan As Single
        Friend SwapChance As Integer
        Friend SkipChance As Integer
        Friend Sub New(_Score As Integer, _Fase As PhasePlayer, _RecentMove As List(Of List(Of Vector2)), _TopMove As List(Of Vector2), _TileYgDipegang() As TileBoard, _SwapChance As Integer, _SkipChance As Integer, _TopMoveHightlight As Boolean, _RecentMoveHighlight As List(Of Boolean))
            RecentMove = _RecentMove
            Fase = _Fase
            Score = _Score
            RecentScore = -1
            TopMove = _TopMove
            TileYangDiPegang = _TileYgDipegang
            SwapChance = _SwapChance
            SkipChance = _SkipChance
            RecentMoveHightlight = _RecentMoveHighlight
            TopMoveHighlight = _TopMoveHightlight
            ScorePosNotif = New Vector2(0, 0)
        End Sub

    End Structure

    Friend Enum PhasePlayer
        Standby
        Running
        Pause
        Finish
        FinishNotif
    End Enum

    Friend Structure KandidatCPUBaseKoordinat
        Friend Koordinat As Vector2
        Friend Value() As String
        Sub New(Koordinat As Vector2, Value() As String)
            Me.Koordinat = Koordinat
            Me.Value = Value
        End Sub
    End Structure
#End Region

    Public Sub New(Name As String, Enable As Boolean, Visible As Boolean)
        Me.Name = Name
        Me.Enable = Enable
        Me.Visible = Visible

        Initialize()
        Load()
    End Sub

    Public Overrides Sub Initialize()
        StacksTile.Clear()
        For i = 0 To 126
            StacksTile.Add(i)
        Next
        For n = 0 To 4
            For j = 1 To StacksTile.Count - 1
                Dim i = RandomNumber.Next(0, j)
                Dim temp = StacksTile(j)
                StacksTile(j) = StacksTile(i)
                StacksTile(i) = temp
            Next
        Next

        For i = 0 To Pemain.Count - 1
            Dim TilePlayerHold(7) As TileBoard
            Pemain(i) = New Player(0, PhasePlayer.Standby, New List(Of List(Of Vector2)), New List(Of Vector2), TilePlayerHold, 8, 2, False, New List(Of Boolean))
        Next
        Visible = True
        InisiasiBoard()
        InisiasiKotakPlayer()
        InisiasiMenu()
        Dim r = RandomNumber.Next(0, 9999)
        If r Mod 2 = 1 Then
            GiliranPlayer = PlayerTurn.CPU
        Else
            GiliranPlayer = PlayerTurn.Human
        End If
        GantiWarnaBackground(GiliranPlayer)
        InisiasiTombolExit()
    End Sub

    Public Overrides Sub Load()
        MessageBoxTexture = ContentManager.Load(Of Texture2D)("gfx/messagebox")
        TransparentDimTexture = ContentManager.Load(Of Texture2D)("gfx/transparent_dim")
        BaseTileTexture = ContentManager.Load(Of Texture2D)("gfx/tiled")
        BasePlainTexture = ContentManager.Load(Of Texture2D)("gfx/backTile")
        DebugFont = ContentManager.Load(Of SpriteFont)("Debug")
        backTileTexture = ContentManager.Load(Of Texture2D)("gfx/backTile")
        SwapTexture = ContentManager.Load(Of Texture2D)("gfx/swap")
        ClearTexture = ContentManager.Load(Of Texture2D)("gfx/clear")
        DoneTexture = ContentManager.Load(Of Texture2D)("gfx/done")
        ShadowTexture = ContentManager.Load(Of Texture2D)("gfx/shadow")
        SearchTexture = ContentManager.Load(Of Texture2D)("gfx/search")
        lineaquare = ContentManager.Load(Of Texture2D)("gfx/linesquare")
        PauseTexture = ContentManager.Load(Of Texture2D)("gfx/pause")
        DownyTexture = ContentManager.Load(Of Texture2D)("gfx/downy")
        UppyTexture = ContentManager.Load(Of Texture2D)("gfx/uppy")
        ExitButton.Texture = ContentManager.Load(Of Texture2D)("gfx/exit")
    End Sub

    Public Overrides Sub Update(ByRef MenuSelected As MenuUtamaScreen.MenuUtama)
        ' TODO: Add your update code here
        If New Rectangle(ExitButton.Posisi.X - ExitButton.Texture.Width * 0.8 / 2, ExitButton.Posisi.Y - ExitButton.Texture.Height * 0.8 / 2, ExitButton.Texture.Width, ExitButton.Texture.Height).Contains(MouseStateAfter.X, MouseStateAfter.Y) And
            FaseGameScreen = PhasePlayer.Running Then
            ExitButton.OnTop = True
            If animasiExitButton < 1 Then
                animasiExitButton += Waktu.ElapsedGameTime.TotalSeconds
                If animasiExitButton >= 1 Then
                    animasiExitButton = 1
                End If
            End If
        Else
            ExitButton.OnTop = False
            If animasiExitButton > 0.8 Then
                animasiExitButton -= Waktu.ElapsedGameTime.TotalSeconds
                If animasiExitButton <= 0.8 Then
                    animasiExitButton = 0.8
                End If
            End If
        End If
        If (KeyboardStateAfter.IsKeyUp(Keys.Escape) And KeyboardStateBefore.IsKeyDown(Keys.Escape)) Or
            (ExitButton.OnTop AndAlso MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released) Then
            If FaseGameScreen = PhasePlayer.Pause Then
                FaseGameScreen = PhasePlayer.Running
            ElseIf FaseGameScreen = PhasePlayer.Running Then
                FaseGameScreen = PhasePlayer.Pause
            End If
        End If

        If FaseGameScreen = PhasePlayer.Standby Or FaseGameScreen = PhasePlayer.Finish Or FaseGameScreen = PhasePlayer.Pause Then
            If FaseGameScreen = PhasePlayer.Standby Then
                TImeRunTemp += Waktu.ElapsedGameTime.TotalSeconds
            End If
            If TImeRunTemp > WaktuPreCPU + 1 Then
                FaseGameScreen = PhasePlayer.Running
                For i = 0 To Pemain(PlayerTurn.Human).TileYangDiPegang.Count - 1
                    Pemain(PlayerTurn.Human).TileYangDiPegang(i).Value = StacksTile(StacksTile.Count - 1)
                    StacksTile.RemoveAt(StacksTile.Count - 1)
                Next
                For i = 0 To Pemain(PlayerTurn.CPU).TileYangDiPegang.Count - 1
                    Pemain(PlayerTurn.CPU).TileYangDiPegang(i).Value = StacksTile(StacksTile.Count - 1)
                    StacksTile.RemoveAt(StacksTile.Count - 1)
                    'Untuk Keperluan Testing
                    'Pemain(PlayerTurn.CPU).TileYangDiPegang(i).Value = 126
                Next
                TImeRunTemp = 0
            End If

            If FaseGameScreen = PhasePlayer.Finish Then
                If New Rectangle(Center.Width - 112 - DebugFont.MeasureString("Menu Utama").X * 0.15 / 2, Center.Height + 71 - DebugFont.MeasureString("Menu Utama").Y * 0.15 / 2, DebugFont.MeasureString("Menu Utama").X * 0.15, DebugFont.MeasureString("Menu Utama").Y * 0.15).Contains(MouseStateBefore.X, MouseStateBefore.Y) Then
                    AnimasiMessageBoxMenuUtama += Waktu.ElapsedGameTime.TotalMilliseconds / 200
                    If AnimasiMessageBoxMenuUtama > 1 Then
                        AnimasiMessageBoxMenuUtama = 1
                    End If
                    If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                        MenuSelected = MenuUtamaScreen.MenuUtama.Menu
                    End If
                Else
                    AnimasiMessageBoxMenuUtama = 0
                End If

                If New Rectangle(Center.Width + 112 - DebugFont.MeasureString("Main Lagi").X * 0.15 / 2, Center.Height + 71 - DebugFont.MeasureString("Main Lagi").Y * 0.15 / 2, DebugFont.MeasureString("Main Lagi").X * 0.15, DebugFont.MeasureString("Main Lagi").Y * 0.15).Contains(MouseStateBefore.X, MouseStateBefore.Y) Then
                    AnimasiMessageBoxMainLagi += Waktu.ElapsedGameTime.TotalMilliseconds / 200
                    If AnimasiMessageBoxMainLagi > 1 Then
                        AnimasiMessageBoxMainLagi = 1
                    End If
                    If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                        ResetUlang()
                        FaseGameScreen = PhasePlayer.Standby
                        Initialize()
                        Load()
                    End If
                Else
                    AnimasiMessageBoxMainLagi = 0
                End If
            End If

            If FaseGameScreen = PhasePlayer.Pause Then
                If New Rectangle(Center.Width - DebugFont.MeasureString("lanjutkan").X / 2 * 0.16, Center.Height - 35 - DebugFont.MeasureString("lanjutkan").Y / 2 * 0.16,
                                 DebugFont.MeasureString("lanjutkan").X * 0.16, DebugFont.MeasureString("lanjutkan").Y * 0.16).Contains(MouseStateAfter.X, MouseStateAfter.Y) Then
                    AnimasiMessageBoxLanjutkan += Waktu.ElapsedGameTime.TotalMilliseconds / 200
                    If AnimasiMessageBoxLanjutkan > 1 Then
                        AnimasiMessageBoxLanjutkan = 1
                    End If

                    If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                        FaseGameScreen = PhasePlayer.Running
                    End If
                Else
                    AnimasiMessageBoxLanjutkan = 0
                End If

                If New Rectangle(Center.Width - DebugFont.MeasureString("mulai ulang").X / 2 * 0.16, Center.Height + 15 - DebugFont.MeasureString("mulai ulang").Y / 2 * 0.16,
                                 DebugFont.MeasureString("mulai ulang").X * 0.16, DebugFont.MeasureString("mulai ulang").Y * 0.16).Contains(MouseStateAfter.X, MouseStateAfter.Y) Then
                    AnimasiMessageBoxMainLagi += Waktu.ElapsedGameTime.TotalMilliseconds / 200
                    If AnimasiMessageBoxMainLagi > 1 Then
                        AnimasiMessageBoxMainLagi = 1
                    End If

                    If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                        ResetUlang()
                        FaseGameScreen = PhasePlayer.Standby
                        Initialize()
                        Load()
                    End If
                Else
                    AnimasiMessageBoxMainLagi = 0
                End If

                If New Rectangle(Center.Width - DebugFont.MeasureString("KELUAR").X / 2 * 0.18, Center.Height + 102 - DebugFont.MeasureString("KELUAR").Y / 2 * 0.18,
                                 DebugFont.MeasureString("KELUAR").X * 0.18, DebugFont.MeasureString("KELUAR").Y * 0.18).Contains(MouseStateAfter.X, MouseStateAfter.Y) Then
                    AnimasiMessageBoxMenuUtama += Waktu.ElapsedGameTime.TotalMilliseconds / 200
                    If AnimasiMessageBoxMenuUtama > 1 Then
                        AnimasiMessageBoxMenuUtama = 1
                    End If

                    If MouseStateBefore.LeftButton = ButtonState.Pressed And MouseStateAfter.LeftButton = ButtonState.Released Then
                        MenuSelected = MenuUtamaScreen.MenuUtama.Menu
                    End If
                Else
                    AnimasiMessageBoxMenuUtama = 0
                End If

            End If

            Exit Sub
        End If

        If StacksTile.Count <= 0 AndAlso Pemain(PlayerTurn.CPU).SkipChance <= 1 And Pemain(PlayerTurn.Human).SkipChance <= 1 Then
            FaseGameScreen = PhasePlayer.Finish
        ElseIf Pemain(PlayerTurn.CPU).SkipChance <= 0 And StacksTile.Count > 0 Then
            For i = 0 To Pemain(PlayerTurn.CPU).TileYangDiPegang.Count - 1
                With Pemain(PlayerTurn.CPU).TileYangDiPegang(i)
                    If StacksTile.Count > 0 Then
                        'StacksTile.Insert(0, p.Value)
                        .IsTrigerAnimation = True
                        .Value = StacksTile(StacksTile.Count - 1)
                        StacksTile.RemoveAt(StacksTile.Count - 1)
                        'Untuk keperluan testing
                        'p.Value = 126
                    End If
                End With
            Next
            Pemain(PlayerTurn.CPU).SkipChance = 2
            ' Pemain(PlayerTurn.Human).SkipChance = 2
        ElseIf Pemain(PlayerTurn.Human).SkipChance <= 0 And StacksTile.Count > 0 Then
            For i = 0 To Pemain(PlayerTurn.Human).TileYangDiPegang.Count - 1
                With Pemain(PlayerTurn.Human).TileYangDiPegang(i)
                    If StacksTile.Count > 0 Then
                        'StacksTile.Insert(0, p.Value)
                        .IsTrigerAnimation = True
                        .Value = StacksTile(StacksTile.Count - 1)
                        StacksTile.RemoveAt(StacksTile.Count - 1)
                    End If
                End With
            Next
            'Pemain(PlayerTurn.CPU).SkipChance = 2
            Pemain(PlayerTurn.Human).SkipChance = 2
        End If

        If Pemain(GiliranPlayer).Fase = PhasePlayer.Standby Then
            TImeRunTemp += Waktu.ElapsedGameTime.TotalSeconds
            If TImeRunTemp >= WaktuPreCPU Then
                TImeRunTemp = 0
                Pemain(GiliranPlayer).Fase = PhasePlayer.Running
                Pemain(GiliranPlayer).DetikBerjalan = 0
            End If
        End If

        If Pemain(GiliranPlayer).Fase = PhasePlayer.FinishNotif And TImeRunTemp > WaktuPostCPU + 0.5 Then
            If Pemain(GiliranPlayer).RecentScore > -1 Then
                Pemain(GiliranPlayer).Score += Pemain(GiliranPlayer).RecentScore
            End If
            Pemain(GiliranPlayer).RecentScore = -1
            Pemain(GiliranPlayer).Fase = PhasePlayer.Standby
            If GiliranPlayer = PlayerTurn.CPU Then
                GiliranPlayer = PlayerTurn.Human
            Else
                GiliranPlayer = PlayerTurn.CPU
            End If
            GantiWarnaBackground(GiliranPlayer)
            Pemain(GiliranPlayer).Fase = PhasePlayer.Standby
            Pemain(GiliranPlayer).RecentScore = -1
            TImeRunTemp = 0
        End If

        'If GiliranPlayer = PlayerTurn.CPU Then
        If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish Or Pemain(GiliranPlayer).Fase = PhasePlayer.FinishNotif Then
            TImeRunTemp += Waktu.ElapsedGameTime.TotalSeconds
        End If
        'End If

        'Jalankan AI CPU
        AI()
        'Ambil kandidat base yang akan digunakan untuk baca list horizontal atau vertikal
        UpdateAmbilKandidatBaseKoordinatHorizontal()
        UpdateAmbilKandidatBaseKoordinatVertikal()
        'Update Jika Cek yang akan dilakukan pertama kali atau tidak
        UpdateJikaFirstTime()
        'Update TileBoard
        UpdateTileBoard(Sudut, Ring, Counter)
        'Ambil List Tile pada Board yang dapat dicek
        UpdateAmbilTileYangDapatDicekHorizontal()
        UpdateAmbilTileYangDapatDicekVertikal()
        'Cek tile yang terpilih(yg dapat dicek) apakah sesuai dengan sintaks bahasa dan ubah warnanya jika sesuai
        UpdateCekBahasa()
        'Cek jika koordinat base dapat dipertahankan atau tidak
        UpdatePertahankanKoordinatAwal()
        'Ambil index TileBoard yang sedang ditahan oleh Mouse
        UpdateTileBoardYangDitahanMouse(Sudut, Ring, Counter)
        'Update TilePlayer dan ambil index TilePlayer yang sedang ditahan oleh Mouse
        UpdateTilePlayer()
        'Update Menu
        UpdateTileMenu(MouseIsHolding, ValueOfHolding, DeleteOnTile)
        'Proses menu yang dipilih
        UpdateMenuTerpilih()
        'Jalankan jika tombol mouse dilepas
        UpdateJikaMouseDiLepas()
        Dim fs = Function(dftr As List(Of List(Of Vector2)), top As List(Of Vector2))
                     Dim tV = Bhs.Score(DaftarString(top))
                     Dim tS As List(Of Vector2) = top
                     For Each d In dftr
                         If Bhs.Score(DaftarString(d)) > tV Then
                             tS = d
                             tV = Bhs.Score(DaftarString(d))
                         End If
                     Next
                     Return tS
                 End Function
        Pemain(PlayerTurn.CPU).TopMove = fs(Pemain(PlayerTurn.CPU).RecentMove, Pemain(PlayerTurn.CPU).TopMove)
        Pemain(PlayerTurn.Human).TopMove = fs(Pemain(PlayerTurn.Human).RecentMove, Pemain(PlayerTurn.Human).TopMove)

        UpdateHighlightTopMove_MostRecent(PlayerTurn.CPU, -10, 1)
        UpdateHighlightTopMove_MostRecent(PlayerTurn.Human, -10, -1)
    End Sub

    Public Overrides Sub Draw()
        'Gambarkan base dari menu
        Dim _menu = Function(m As TileMenu.Menus)
                        If m = Summy.TileMenu.Menus.Tukar Then
                            Return SwapTexture
                        End If
                        If m = Summy.TileMenu.Menus.Kembalikan Then
                            Return ClearTexture
                        End If
                        If m = Summy.TileMenu.Menus.Lanjut Then
                            Return DoneTexture
                        End If
                        Return backTileTexture
                    End Function
        For Each Kotak In TileMenu
            Kotak.Draw(BasePlainTexture, _menu(Kotak.MenuValue), ShadowTexture, DebugFont)
        Next

        'Gambarkan base dasar board
        For Each Kotak In TileBoard
            Kotak.Draw(If(Kotak.Value >= 127, backTileTexture, BaseTileTexture), DebugFont, ShadowTexture)
        Next

        'Gambarkan base dasar kotak player
        For Each Kotak In Pemain(PlayerTurn.Human).TileYangDiPegang
            Kotak.Draw(BaseTileTexture, DebugFont, ShadowTexture)
        Next

        'Gambarkan base dasar kotak CPU
        For Each Kotak In Pemain(PlayerTurn.CPU).TileYangDiPegang
            Kotak.Draw(BaseTileTexture, DebugFont, ShadowTexture)
        Next

        'Gambarkan tile yang berada di bawah mouse
        For Each Kotak In TileBoard
            If Kotak.TrigerByOnTop Then
                Kotak.Draw(If(Kotak.Value = 127, backTileTexture, BaseTileTexture), DebugFont, ShadowTexture)
                Exit For
            End If
        Next

        'Gambarkan tile yang dipegang oleh mouse
        For Each Kotak In TileBoard
            If Kotak.TrigerByHold Then
                Kotak.Draw(BaseTileTexture, DebugFont, ShadowTexture)
                Exit For
            End If
        Next

        'Gambarkan tile yang berada di bawah mouse
        For Each Kotak In Pemain(PlayerTurn.Human).TileYangDiPegang
            If Kotak.TrigerByOnTop Then
                Kotak.Draw(BaseTileTexture, DebugFont, ShadowTexture)
                Exit For
            End If
        Next

        'Gambarkan tile yang dipegang oleh mouse
        For Each Kotak In Pemain(PlayerTurn.Human).TileYangDiPegang
            If Kotak.TrigerByHold Then
                Kotak.Draw(BaseTileTexture, DebugFont, ShadowTexture)
                Exit For
            End If
        Next

        DrawBanner(PlayerTurn.CPU, -10, 1)
        DrawBanner(PlayerTurn.Human, -10, -1)

        Sprite.DrawString(DebugFont, StacksTile.Count, New Vector2(Center.Width - 376, Center.Height + 314), Color.White, 0, New Vector2(DebugFont.MeasureString(StacksTile.Count).X / 2, DebugFont.MeasureString(StacksTile.Count).Y / 2), 0.3, SpriteEffects.None, 0)

        Sprite.Draw(ExitButton.Texture, ExitButton.Posisi + New Vector2(-280 * (animasiExitButton - 0.8), 0), New Rectangle(0, 0, ExitButton.Texture.Width, ExitButton.Texture.Height), Color.White, 0, New Vector2(ExitButton.Texture.Width / 2, ExitButton.Texture.Height / 2), 0.8, SpriteEffects.None, 0)

        If FaseGameScreen = PhasePlayer.Running Then
            If Pemain(GiliranPlayer).Fase = PhasePlayer.Standby Then
                Sprite.Draw(MessageBoxTexture, New Rectangle(Center.Width, 0 - (100 * (1 - If(TImeRunTemp >= 0.25, 1, TImeRunTemp * 5))), MessageBoxTexture.Width, MessageBoxTexture.Height), New Rectangle(0, 0, MessageBoxTexture.Width, MessageBoxTexture.Height), New Color(156, 156, 156), 0, New Vector2(MessageBoxTexture.Width / 2, MessageBoxTexture.Height / 2), SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, $"Sekarang Giliran {GiliranPlayer.ToString}...", New Vector2(Center.Width, 50 - (100 * (1 - If(TImeRunTemp >= 0.25, 1, TImeRunTemp * 4)))), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString($"Sekarang Giliran {GiliranPlayer.ToString}...").X / 2, DebugFont.MeasureString($"Sekarang Giliran {GiliranPlayer.ToString}...").Y / 2), 0.14, SpriteEffects.None, 0)
            End If

            If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish Then
                Sprite.Draw(MessageBoxTexture, New Rectangle(Center.Width, 0 - (100 * (1 - If(TImeRunTemp >= 0.25, 1, TImeRunTemp * 5))), MessageBoxTexture.Width, MessageBoxTexture.Height), New Rectangle(0, 0, MessageBoxTexture.Width, MessageBoxTexture.Height), New Color(156, 156, 156), 0, New Vector2(MessageBoxTexture.Width / 2, MessageBoxTexture.Height / 2), SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, $"Giliran {GiliranPlayer.ToString} berakhir...", New Vector2(Center.Width, 50 - (100 * (1 - If(TImeRunTemp >= 0.25, 1, TImeRunTemp * 4)))), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString($"Giliran {GiliranPlayer.ToString} berakhir...").X / 2, DebugFont.MeasureString($"Giliran {GiliranPlayer.ToString} berakhir...").Y / 2), 0.14, SpriteEffects.None, 0)
            End If

            If Pemain(GiliranPlayer).Fase = PhasePlayer.FinishNotif Then
                If Pemain(GiliranPlayer).RecentMove.Count > 0 AndAlso Pemain(GiliranPlayer).RecentScore > 0 Then
                    If TImeRunTemp > 0.6 Then
                        Dim newpos = Sub()
                                         Dim defposscpu = New Vector2(Center.Width - 474, Center.Height - 330)
                                         Dim defposshuman = New Vector2(Center.Width + 474, Center.Height - 330)
                                         If Pemain(GiliranPlayer).ScorePosNotif <> If(GiliranPlayer = PlayerTurn.CPU, defposscpu, defposshuman) Then
                                             Dim diffVector = Vector2.Subtract(If(GiliranPlayer = PlayerTurn.CPU, defposscpu, defposshuman), Pemain(GiliranPlayer).ScorePosNotif)
                                             diffVector.Normalize()
                                             diffVector = Vector2.Multiply(diffVector, Waktu.ElapsedGameTime.TotalMilliseconds)
                                             Pemain(GiliranPlayer).ScorePosNotif = Vector2.Add(Pemain(GiliranPlayer).ScorePosNotif, diffVector)
                                         End If
                                         If GiliranPlayer = PlayerTurn.CPU Then
                                             If Pemain(GiliranPlayer).ScorePosNotif.X < defposscpu.X And Pemain(GiliranPlayer).ScorePosNotif.Y < defposscpu.Y Then
                                                 Pemain(GiliranPlayer).ScorePosNotif = defposscpu
                                             End If
                                         Else
                                             If Pemain(GiliranPlayer).ScorePosNotif.X > defposshuman.X And Pemain(GiliranPlayer).ScorePosNotif.Y < defposshuman.Y Then
                                                 Pemain(GiliranPlayer).ScorePosNotif = defposshuman
                                             End If
                                         End If
                                     End Sub
                        newpos()
                    End If
                    Sprite.DrawString(DebugFont, $"+{Pemain(GiliranPlayer).RecentScore}", New Vector2(Pemain(GiliranPlayer).ScorePosNotif.X, Pemain(GiliranPlayer).ScorePosNotif.Y), New Color(78, 78, 78), 0, New Vector2(DebugFont.MeasureString($"+{Pemain(GiliranPlayer).RecentScore}").X / 2, DebugFont.MeasureString($"+{Pemain(GiliranPlayer).RecentScore}").Y / 2), 1.6 * If(TImeRunTemp > 0.25, 0.25, TImeRunTemp), SpriteEffects.None, 0)
                End If
            End If
        End If

        If FaseGameScreen = PhasePlayer.Standby Or FaseGameScreen = PhasePlayer.Finish Or FaseGameScreen = PhasePlayer.Pause Then
            Sprite.Draw(TransparentDimTexture, New Rectangle(Center.Width, Center.Height, GameSize.Width, GameSize.Height), New Rectangle(0, 0, TransparentDimTexture.Width, TransparentDimTexture.Height), New Color(36, 36, 36), 0, New Vector2(TransparentDimTexture.Width / 2, TransparentDimTexture.Height / 2), SpriteEffects.None, 0)
            Sprite.DrawString(DebugFont, If(FaseGameScreen = PhasePlayer.Pause, "Pause...", "Memuat..."), New Vector2(Center.Width, Center.Height), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString(If(FaseGameScreen = PhasePlayer.Pause, "Pause...", "Memuat...")).X / 2, DebugFont.MeasureString(If(FaseGameScreen = PhasePlayer.Pause, "Pause...", "Memuat...")).Y / 2), 0.2, SpriteEffects.None, 0)
            If FaseGameScreen = PhasePlayer.Finish Then
                Sprite.Draw(MessageBoxTexture, New Rectangle(Center.Width, Center.Height, MessageBoxTexture.Width, MessageBoxTexture.Height), New Rectangle(0, 0, MessageBoxTexture.Width, MessageBoxTexture.Height), New Color(33, 33, 33), 0, New Vector2(MessageBoxTexture.Width / 2, MessageBoxTexture.Height / 2), SpriteEffects.None, 0)
                Dim pemenang = Function(cpuScore As Integer, pemainScore As Integer)
                                   If cpuScore > pemainScore Then
                                       Return "CPU Menang!!!"
                                   ElseIf pemainScore > cpuScore Then
                                       Return "Pemain Menang!!!"
                                   Else
                                       Return "Seri!!!"
                                   End If
                               End Function
                Dim t As String = "Permainan Selesai" & vbNewLine & vbNewLine & pemenang(Pemain(PlayerTurn.CPU).Score, Pemain(PlayerTurn.Human).Score)
                Sprite.DrawString(DebugFont, t, New Vector2(Center.Width, Center.Height - 20), New Color(224, 224, 224), 0, New Vector2(DebugFont.MeasureString(t).X / 2, DebugFont.MeasureString(t).Y / 2), 0.2, SpriteEffects.None, 0)

                Sprite.DrawString(DebugFont, "Menu Utama", New Vector2(Center.Width - 112, Center.Height + 71), New Color(224, 224, 224), 0, New Vector2(DebugFont.MeasureString("Menu Utama").X / 2, DebugFont.MeasureString("Menu Utama").Y / 2), 0.15 + 0.05 * AnimasiMessageBoxMenuUtama, SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, "Main Lagi", New Vector2(Center.Width + 128, Center.Height + 71), New Color(224, 224, 224), 0, New Vector2(DebugFont.MeasureString("Main Lagi").X / 2, DebugFont.MeasureString("Main Lagi").Y / 2), 0.15 + 0.05 * AnimasiMessageBoxMainLagi, SpriteEffects.None, 0)
            End If

            If FaseGameScreen = PhasePlayer.Pause Then
                Sprite.Draw(PauseTexture, New Rectangle(Center.Width, Center.Height, 300, 350), New Rectangle(0, 0, PauseTexture.Width, PauseTexture.Height), Color.White, 0, New Vector2(PauseTexture.Width / 2, PauseTexture.Height / 2), SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, "pause", New Vector2(Center.Width, Center.Height - 112), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString("pause").X / 2, DebugFont.MeasureString("pause").Y / 2), 0.3, SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, "lanjutkan", New Vector2(Center.Width, Center.Height - 35), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString("lanjutkan").X / 2, DebugFont.MeasureString("lanjutkan").Y / 2), 0.16 + 0.05 * AnimasiMessageBoxLanjutkan, SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, "mulai ulang", New Vector2(Center.Width, Center.Height + 15), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString("mulai ulang").X / 2, DebugFont.MeasureString("mulai ulang").Y / 2), 0.16 + 0.05 * AnimasiMessageBoxMainLagi, SpriteEffects.None, 0)
                Sprite.DrawString(DebugFont, "KELUAR", New Vector2(Center.Width, Center.Height + 102), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString("KELUAR").X / 2, DebugFont.MeasureString("KELUAR").Y / 2), 0.18 + 0.05 * AnimasiMessageBoxMenuUtama, SpriteEffects.None, 0)
            End If
        End If
        'Gambarkan border sekeliling
        Sprite.Draw(UppyTexture, New Rectangle(Center.Width, Center.Height - 390 + UppyTexture.Height / 2, UppyTexture.Width, UppyTexture.Height), New Rectangle(0, 0, UppyTexture.Width, UppyTexture.Height), Color.White, 0, New Vector2(UppyTexture.Width / 2, UppyTexture.Height / 2), SpriteEffects.None, 0)
        Sprite.Draw(DownyTexture, New Rectangle(Center.Width, Center.Height + 390 - DownyTexture.Height / 2, DownyTexture.Width, DownyTexture.Height), New Rectangle(0, 0, DownyTexture.Width, DownyTexture.Height), Color.White, 0, New Vector2(DownyTexture.Width / 2, DownyTexture.Height / 2), SpriteEffects.None, 0)
    End Sub

    Private Sub DrawBanner(ByVal Player As PlayerTurn, ByVal PosX As Integer, ByVal side As Integer)
        'Draw Score
        Sprite.DrawString(DebugFont, Pemain(Player).Score, New Vector2(Center.Width - 464 * side + PosX * side, Center.Height - 300), New Color(97, 97, 97), 0, New Vector2(DebugFont.MeasureString(Pemain(Player).Score).X / 2, DebugFont.MeasureString(Pemain(Player).Score).Y / 2), 0.4, SpriteEffects.None, 0)
        'Draw Skip Chance dan Swap Chance
        Sprite.DrawString(DebugFont, Pemain(Player).SkipChance, New Vector2(Center.Width - 366 * side + PosX * side, Center.Height - 314), New Color(250, 250, 250), 0, New Vector2(DebugFont.MeasureString(Pemain(Player).SwapChance).X / 2, DebugFont.MeasureString(Pemain(Player).SwapChance).Y / 2), 0.15, SpriteEffects.None, 0)
        Sprite.DrawString(DebugFont, Pemain(Player).SwapChance, New Vector2(Center.Width - 366 * side + PosX * side, Center.Height - 286), New Color(250, 250, 250), 0, New Vector2(DebugFont.MeasureString(Pemain(Player).SwapChance).X / 2, DebugFont.MeasureString(Pemain(Player).SwapChance).Y / 2), 0.15, SpriteEffects.None, 0)
        'Draw TopMove
        If Pemain(Player).TopMove.Count > 0 Then
            Dim sized As Single = 0
            Dim warna As Color = New Color(245, 245, 245)
            If Pemain(Player).TopMoveHighlight Then
                sized = 0.05
                warna = New Color(189, 189, 189)
                Sprite.Draw(BasePlainTexture, New Rectangle(Center.Width - 304 * side + PosX * side, Center.Height - 148, 48, 48), New Rectangle(0, 0, BasePlainTexture.Width, BasePlainTexture.Height), New Color(117, 117, 117), 0, New Vector2(BasePlainTexture.Width / 2, BasePlainTexture.Height / 2), SpriteEffects.None, 0)
                Dim s = Bhs.Score(DaftarString(Pemain(Player).TopMove))
                Sprite.DrawString(DebugFont, s, New Vector2(Center.Width - 304 * side + PosX * side, Center.Height - 148), Color.White, 0, New Vector2(DebugFont.MeasureString(s).X / 2, DebugFont.MeasureString(s).Y / 2), 0.16F, SpriteEffects.None, 0)
            End If
            Sprite.Draw(lineaquare, New Rectangle(Center.Width - 446 * side + PosX * side, Center.Height - 128, 196 + 4 * sized, 3), New Rectangle(0, 0, lineaquare.Width, lineaquare.Height), warna, 0, New Vector2(lineaquare.Width / 2, lineaquare.Height / 2), SpriteEffects.None, 0)
            Sprite.DrawString(DebugFont, DaftarString(Pemain(Player).TopMove), New Vector2(Center.Width - 446 * side + PosX * side, Center.Height - 142), New Color(117, 117, 117), 0, New Vector2(DebugFont.MeasureString(DaftarString(Pemain(Player).TopMove)).X / 2, DebugFont.MeasureString(DaftarString(Pemain(Player).TopMove)).Y / 2), 0.16 + sized, SpriteEffects.None, 0)
        End If
        'Draw Recent Move
        Dim space = 0
        For r = Pemain(Player).RecentMove.Count - 1 - 7 To Pemain(Player).RecentMove.Count - 1
            If r >= 0 Then
                Dim sized As Single = 0
                Dim warna As Color = New Color(245, 245, 245)
                If Pemain(Player).RecentMoveHightlight(r) Then
                    sized = 0.05
                    warna = New Color(189, 189, 189)
                    Sprite.Draw(BasePlainTexture, New Rectangle(Center.Width - 304 * side + PosX * side, Center.Height - 92 + space, 48, 48), New Rectangle(0, 0, BasePlainTexture.Width, BasePlainTexture.Height), New Color(117, 117, 117), 0, New Vector2(BasePlainTexture.Width / 2, BasePlainTexture.Height / 2), SpriteEffects.None, 0)
                    Dim s = Bhs.Score(DaftarString(Pemain(Player).RecentMove(r)))
                    Sprite.DrawString(DebugFont, s, New Vector2(Center.Width - 304 * side + PosX * side, Center.Height - 92 + space), Color.White, 0, New Vector2(DebugFont.MeasureString(s).X / 2, DebugFont.MeasureString(s).Y / 2), 0.16F, SpriteEffects.None, 0)
                End If
                Sprite.Draw(lineaquare, New Rectangle(Center.Width - 446 * side + PosX * side, Center.Height - 70 + space, 196 + 4 * sized, 3), New Rectangle(0, 0, lineaquare.Width, lineaquare.Height), warna, 0, New Vector2(lineaquare.Width / 2, lineaquare.Height / 2), SpriteEffects.None, 0)
                Dim rC = DaftarString(Pemain(Player).RecentMove(r))
                Sprite.DrawString(DebugFont, rC, New Vector2(Center.Width - 446 * side + PosX * side, Center.Height - 82 + space), New Color(117, 117, 117), 0, New Vector2(DebugFont.MeasureString(rC).X / 2, DebugFont.MeasureString(rC).Y / 2), 0.16 + sized, SpriteEffects.None, 0)
                space += 42
            End If
        Next
        'Draw waktu yang berjalan
        Dim wktu = TimeSpan.FromSeconds(Pemain(Player).DetikBerjalan).ToString("mm\:ss\:fff")
        Dim wktu2 = TimeSpan.FromSeconds(Pemain(Player).AkumulasiDetikBerjalan).ToString("mm\:ss\:fff")
        Sprite.DrawString(DebugFont, wktu, New Vector2(Center.Width - If(Player = PlayerTurn.CPU, 579, 349) * side + PosX * side, Center.Height - 219), New Color(254, 254, 254), 0, New Vector2(0, DebugFont.MeasureString(wktu).Y / 2), 0.14, SpriteEffects.None, 0)
        Sprite.DrawString(DebugFont, "/", New Vector2(Center.Width - 469 * side + PosX * side, Center.Height - 219), New Color(254, 254, 254), 0, New Vector2(DebugFont.MeasureString("/").X / 2, DebugFont.MeasureString("/").Y / 2), 0.15, SpriteEffects.None, 0)
        Sprite.DrawString(DebugFont, wktu2, New Vector2(Center.Width - If(Player = PlayerTurn.CPU, 449, 479) * side + PosX * side, Center.Height - 219), New Color(254, 254, 254), 0, New Vector2(0, DebugFont.MeasureString(wktu2).Y / 2), 0.14, SpriteEffects.None, 0)
        'Draw apakah cpu search sedang berjalan atau tidak
        If Player = GiliranPlayer Then
            Sprite.Draw(BasePlainTexture, New Rectangle(Center.Width - 572 * side + PosX * side, Center.Height - 261, 56, 22), New Rectangle(0, 0, BasePlainTexture.Width, BasePlainTexture.Height), New Color(250, 250, 250), 0, New Vector2(BasePlainTexture.Width / 2, BasePlainTexture.Height / 2), SpriteEffects.None, 0)
            Sprite.Draw(BasePlainTexture, New Rectangle(Center.Width - 572 * side + PosX * side, Center.Height - 245, 56, 8), New Rectangle(0, 0, BasePlainTexture.Width, BasePlainTexture.Height), New Color(117, 117, 117), 0, New Vector2(BasePlainTexture.Width / 2, BasePlainTexture.Height / 2), SpriteEffects.None, 0)
            If AktifCPU Then
                Sprite.Draw(SearchTexture, New Rectangle(Center.Width - 572 * side + PosX * side, Center.Height - 261, 24, 24), New Rectangle(0, 0, SearchTexture.Width, SearchTexture.Height), New Color(117, 117, 117), 0, New Vector2(SearchTexture.Width / 2, SearchTexture.Height / 2), SpriteEffects.None, 0)
            End If
        Else
            Sprite.Draw(TransparentDimTexture, New Rectangle(Center.Width - 469 * side + PosX * side, Center.Height - 33, 282, 617), New Rectangle(0, 0, TransparentDimTexture.Width, TransparentDimTexture.Height), New Color(78, 78, 78), 0, New Vector2(TransparentDimTexture.Width / 2, TransparentDimTexture.Height / 2), SpriteEffects.None, 0)
            If GiliranPlayer = PlayerTurn.CPU Then
                Sprite.Draw(TransparentDimTexture, New Rectangle(Center.Width, Center.Height + 324, 398, 60), New Rectangle(0, 0, TransparentDimTexture.Width, TransparentDimTexture.Height), New Color(78, 78, 78), 0, New Vector2(TransparentDimTexture.Width / 2, TransparentDimTexture.Height / 2), SpriteEffects.None, 0)
            End If
        End If
    End Sub


#Region "Tempat Script Inisiasi"
    Private Sub InisiasiBoard()
        Dim PosisiAwal As New Vector2(Center.Width, Center.Height - 36)
        Dim Ukuran = 28 * 20 / 24
        Dim Space = 2
        Dim Sudut = 0
        Dim Ring = 0
        Dim Counter = 0
        'Dim kriteriahorizontal As Boolean = True
        Do
            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)
            Dim i = koordinat.X - Math.Floor(TileBoard.GetLength(0) / 2)
            Dim j = koordinat.Y - Math.Floor(TileBoard.GetLength(1) / 2)
            Dim posisi As Vector2 = PosisiAwal + New Vector2(i * Ukuran + i * Space, j * Ukuran + j * Space)
            TileBoard(koordinat.X, koordinat.Y) = New TileBoard(posisi, New Sizes(Ukuran, Ukuran), Colour, True, 127, Summy.TileBoard.Own.Board)
            If koordinat.X = Math.Floor(TileBoard.GetLength(0) / 2) And koordinat.Y = Math.Floor(TileBoard.GetLength(1) / 2) Then
                TileBoard(koordinat.X, koordinat.Y).Value = 126
                TileBoard(koordinat.X, koordinat.Y).IsBlocked = True
                TileBoard(koordinat.X, koordinat.Y).IsOpen = True
            End If
            If Ring + 1 > Math.Floor(TileBoard.GetLength(0) / 2) And Sudut = 7 And Counter = Ring + Ring - 1 Then
                Exit Do
            End If
        Loop
    End Sub

    Private Sub InisiasiKotakPlayer()
        Dim ukuran = 36
        Dim PosisiAwal As New Vector2(Center.Width - 164, Center.Height + 324)
        For i = 0 To Pemain(PlayerTurn.Human).TileYangDiPegang.Count - 1
            Pemain(PlayerTurn.Human).TileYangDiPegang(i) = New TileBoard(PosisiAwal, New Sizes(ukuran, ukuran), Colour, True, 127, Summy.TileBoard.Own.PlayerOne)
            'StacksTile.RemoveAt(StacksTile.Count - 1)
            PosisiAwal.X += ukuran + 10
        Next

        PosisiAwal.Y = Center.Height - 251
        PosisiAwal.X = Center.Width - 359
        For i = 0 To Pemain(PlayerTurn.CPU).TileYangDiPegang.Count - 1
            Pemain(PlayerTurn.CPU).TileYangDiPegang(i) = New TileBoard(PosisiAwal, New Sizes(22, 22), Colour, False, 127, Summy.TileBoard.Own.PlayerCPU)
            'StacksTile.RemoveAt(StacksTile.Count - 1)
            PosisiAwal.X -= 22 + 3
        Next
    End Sub

    Private Sub InisiasiMenu()
        Dim PosisiAwal As New Vector2(Center.Width + 372, Center.Height + 324)
        Dim ukuran As New Sizes(64, 64)
        For i = 0 To TileMenu.Length - 1
            TileMenu(i) = New TileMenu(PosisiAwal, ukuran, New Color(0, 0, 0), True, i + 1)
            If i = 0 Then
                PosisiAwal.X += ukuran.Width + 10
            Else
                PosisiAwal.X += ukuran.Width + 10
            End If
        Next
    End Sub

    Private Sub InisiasiTombolExit()
        ExitButton.Posisi = New Vector2(Center.Width + 690, Center.Height + 319)
    End Sub

#End Region

#Region "Tempat Script Fungsi Update"
    Private Sub UpdateHighlightTopMove_MostRecent(ByVal Player As PlayerTurn, ByVal PosX As Integer, ByVal side As Integer)
        Dim siz = New Sizes(196, 24)
        If Player = PlayerTurn.CPU Then
            For Each t In TileBoard
                t.IsHighlighted = False
            Next
        End If

        Pemain(Player).TopMoveHighlight = False
        For p = 0 To Pemain(Player).RecentMoveHightlight.Count - 1
            Pemain(Player).RecentMoveHightlight(p) = False
        Next

        If New Rectangle(Center.Width - 446 * side + PosX * side - siz.Width / 2, Center.Height - 144 - siz.Height / 2, siz.Width, siz.Height).Contains(MouseStateAfter.X, MouseStateAfter.Y) Then
            For Each t In Pemain(Player).TopMove
                TileBoard(t.X, t.Y).IsHighlighted = True
            Next
            Pemain(Player).TopMoveHighlight = True
        End If

        Dim Space = 0
        For r = Pemain(Player).RecentMove.Count - 1 - 7 To Pemain(Player).RecentMove.Count - 1
            If r >= 0 Then
                If New Rectangle(Center.Width - 446 * side + PosX * side - siz.Width / 2, Center.Height - 82 + Space - siz.Height / 2, siz.Width, siz.Height).Contains(MouseStateAfter.X, MouseStateAfter.Y) Then
                    For Each t In Pemain(Player).RecentMove(r)
                        TileBoard(t.X, t.Y).IsHighlighted = True
                    Next
                    Pemain(Player).RecentMoveHightlight(r) = True
                End If
                Space += 42
            End If
        Next
    End Sub

    Private Sub AI()
        If (KoordinatAsalH <> New Vector2(-1, -1) And KandidatSourceCPUAIH.Count > 0) Or
                (KoordinatAsalV <> New Vector2(-1, -1) And KandidatSourceCPUAIV.Count > 0) Then

            If ((KeyboardStateAfter.IsKeyDown(Keys.C) And KeyboardStateBefore.IsKeyUp(Keys.C) And GiliranPlayer = PlayerTurn.Human And Pemain(GiliranPlayer).Fase = PhasePlayer.Running) Or
                GiliranPlayer = PlayerTurn.CPU AndAlso Pemain(GiliranPlayer).Fase = PhasePlayer.Running) And
                Not (StatusCekBacaH Or StatusCekBacaV) Then
                If Not AktifCPU Then
                    AktifCPU = True
                    WaktuTungguBerjalan = 0
                    'Untuk Keperluan Testing
                    'Pemain(GiliranPlayer).DetikBerjalan = 0
                End If
            End If

            NonAktifCPUH = False
            NonAktifCPUV = False

            'Cari hasil permutasi jika CPU aktif
            If KandidatSourceCPUAIH.Count > 0 And KH >= 0 AndAlso (AktifCPU And CPUHState = StatusCPU.Ready) Then
                CPUHState = StatusCPU.Running
                'Dim opsiParallel As New ParallelOptions
                'opsiParallel.MaxDegreeOfParallelism = Environment.ProcessorCount - 1
                Task.Factory.StartNew(Sub()
                                          Parallel.ForEach(KandidatSourceCPUAIH, Sub(k, loopState)
                                                                                     'Dim s = {"2", "3", "1", "+", "=", "4", "5", "7", "2", "9", "5"}
                                                                                     'SyncLock Kunci
                                                                                     'Dim ll = greedy.SusunKandidatHorizontal2(s)
                                                                                     'End SyncLock
                                                                                     'MsgBox("OK")

                                                                                     Dim Result As ResultHasilPermutasi = CariHasilPermutasi(k, Pemain(GiliranPlayer).TileYangDiPegang, TileBoard, True, loopState)
                                                                                     'CPU_CekValidasiHasilPermutasi(Result.CPUBaseKoordinat, Result.KandidatPembantu, Result.HasilPermutasi, loopState, True)

                                                                                 End Sub)
                                          SyncLock Kunci2
                                              CPUHState = StatusCPU.Finish
                                          End SyncLock
                                      End Sub, TaskCreationOptions.LongRunning)
                'End If
            End If

            If KandidatSourceCPUAIV.Count > 0 And KV >= 0 AndAlso (AktifCPU And CPUVState = StatusCPU.Ready) Then
                CPUVState = StatusCPU.Running

                'Dim opsiParallel As New ParallelOptions
                'opsiParallel.MaxDegreeOfParallelism = Environment.ProcessorCount - 1
                Task.Factory.StartNew(Sub()
                                          Parallel.ForEach(KandidatSourceCPUAIV, Sub(k, loopState)
                                                                                     'Dim s = {"2", "3", "1", "+", "=", "4", "5", "7", "2", "9", "5"}
                                                                                     'SyncLock Kunci
                                                                                     'Dim ll = greedy.SusunKandidatHorizontal2(s)
                                                                                     'End SyncLock
                                                                                     Dim Result As ResultHasilPermutasi = CariHasilPermutasi(k, Pemain(GiliranPlayer).TileYangDiPegang, TileBoard, False, loopState)
                                                                                     'CPU_CekValidasiHasilPermutasi(Result.CPUBaseKoordinat, Result.KandidatPembantu, Result.HasilPermutasi, loopState, False)
                                                                                 End Sub)
                                          SyncLock Kunci2
                                              CPUVState = StatusCPU.Finish
                                          End SyncLock
                                      End Sub)
                ' End If
            End If

            If ((CPUHState = StatusCPU.Finish And CPUVState = StatusCPU.Finish) Or
                (CPUHState = StatusCPU.Finish And CPUVState = StatusCPU.Ready And KandidatSourceCPUAIV.Count = 0) Or
                (CPUHState = StatusCPU.Ready And KandidatSourceCPUAIH.Count = 0 And CPUVState = StatusCPU.Finish)) Then
                CPUPlaceSet = False
                AktifCPU = False
                CPUHState = StatusCPU.Ready
                CPUVState = StatusCPU.Ready
                If GiliranPlayer = PlayerTurn.CPU AndAlso Pemain(GiliranPlayer).Fase = PhasePlayer.Running Then
                    Pemain(GiliranPlayer).Fase = PhasePlayer.Finish
                    TImeRunTemp = 0
                End If
            End If


            If Pemain(GiliranPlayer).Fase = PhasePlayer.Running Then
                Pemain(GiliranPlayer).DetikBerjalan += Waktu.ElapsedGameTime.TotalSeconds
                If AktifCPU And BatasWaktuTunggu > 0 Then
                    'Untuk Keperluan Testing
                    'Pemain(GiliranPlayer).DetikBerjalan += Waktu.ElapsedGameTime.TotalSeconds
                    WaktuTungguBerjalan += Waktu.ElapsedGameTime.TotalSeconds
                End If

            End If

        End If
    End Sub

    Private Function CariHasilPermutasi(k As KandidatCPUBaseKoordinat, TilePlayer As TileBoard(), TileBoard As TileBoard(,), IsHorizontal As Boolean, loopState As ParallelLoopState) As ResultHasilPermutasi
        Dim TempTilePlayer = TilePlayer
        Dim TempTileBoard = TileBoard
        Dim TempLString As New List(Of String)
        Dim TempKPem As New List(Of Vector2)
        Dim TempLHPer As New List(Of HasilPermutasi)
        Dim n As Short, a As Short, b As Short


        If AktifCPU AndAlso CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) < BatasWaktuTunggu Then
            If IsHorizontal Then
                n = k.Koordinat.Y
            Else
                n = k.Koordinat.X
            End If

            For Each kar In TempTilePlayer
                If Not kar.IsBlocked Then
                    TempLString.Add(kar.CharValue)
                End If
            Next

            For i = 1 To n
                If IsHorizontal Then
                    b = i : a = 0
                Else
                    b = 0 : a = i
                End If
                If TempTileBoard(k.Koordinat.X - a, k.Koordinat.Y - b).IsBlocked Then
                    If TempTileBoard(k.Koordinat.X - a, k.Koordinat.Y - b).Value <> 127 Then
                        TempLString.Add(TempTileBoard(k.Koordinat.X - a, k.Koordinat.Y - b).CharValue)
                        TempKPem.Add(New Vector2(k.Koordinat.X - a, k.Koordinat.Y - b))
                    Else
                        Exit For
                    End If
                End If
            Next

            For j = 1 To TempTileBoard.GetLength(0) - n - 1
                If IsHorizontal Then
                    b = j : a = 0
                Else
                    b = 0 : a = j
                End If
                If TempTileBoard(k.Koordinat.X + a, k.Koordinat.Y + b).IsBlocked Then
                    If TempTileBoard(k.Koordinat.X + a, k.Koordinat.Y + b).Value <> 127 Then
                        TempLString.Add(TempTileBoard(k.Koordinat.X + a, k.Koordinat.Y + b).CharValue)
                        TempKPem.Add(New Vector2(k.Koordinat.X + a, k.Koordinat.Y + b))
                    Else
                        Exit For
                    End If
                End If
            Next

            For Each t In k.Value
                TempLString.Add(t)
            Next

            'If ModeProcessCPU = ModeCPU.ParallelWithoutSync Then
            If (TempLString.Contains("+") Or TempLString.Contains("-") Or
                    TempLString.Contains("x") Or TempLString.Contains(":")) And
                    TempLString.Contains("=") Then
                'TempLHPer = greedy.SusunKandidatParallel(TempLString.ToArray)
                greedy.SusunKandidatParalelDanSetUpHasil(TempLString.ToArray, k, TempKPem, IsHorizontal, TilePlayer, TileBoard, loopState)
            End If

        End If
        Return New ResultHasilPermutasi(k, TempKPem, TempLString, TempLHPer)
    End Function

    Private Sub CPU_CekValidasiHasilPermutasi(KandidatSource As KandidatCPUBaseKoordinat, KPbntu As List(Of Vector2), DftrKandidat As List(Of HasilPermutasi), IsHorizontal As Boolean)
        If KandidatSource.Value.Count > 0 Then
            Dim Daftar = DftrKandidat
            For Each d In Daftar
                Dim Solusi = d.Persamaan
                Dim avb = AvaibleFormula(d, KandidatSource, KPbntu)
                Dim Cocok As Boolean = False
                With KandidatSource
                    If avb >= 0 Then
                        Cocok = KelayakanCPU(avb, .Koordinat, Solusi, KPbntu, IsHorizontal, Pemain(GiliranPlayer).TileYangDiPegang)
                    End If
                    If Cocok Then
                        CPU_LetakHasilTervalidasi(avb, .Koordinat, Solusi, IsHorizontal)
                        AktifCPU = False
                        Exit For
                    End If
                End With
            Next
        End If
    End Sub

    Private Sub CPU_CekValidasiHasilPermutasi(KandidatSource As KandidatCPUBaseKoordinat, KPbntu As List(Of Vector2), DftrKandidatH As List(Of HasilPermutasi), loopState As ParallelLoopState, IsHorizontal As Boolean)
        If KandidatSource.Value.Count > 0 And AktifCPU Then
            Dim Daftar = DftrKandidatH
            For Each d In Daftar
                Dim Solusi = d.Persamaan
                Dim avb = AvaibleFormula(d, KandidatSource, KPbntu)
                Dim Cocok As Boolean = False
                If avb >= 0 Then
                    If Not loopState.ShouldExitCurrentIteration Then
                        Cocok = KelayakanCPU(avb, KandidatSource.Koordinat, Solusi, KPbntu, IsHorizontal, Pemain(GiliranPlayer).TileYangDiPegang)
                    End If
                End If
                If Cocok Then
                    SyncLock Kunci
                        If AktifCPU And Not CPUPlaceSet Then
                            If Not loopState.ShouldExitCurrentIteration Then
                                CPU_LetakHasilTervalidasi(avb, KandidatSource.Koordinat, Solusi, IsHorizontal)
                                loopState.Stop()
                                CPUPlaceSet = True
                            End If
                        End If
                    End SyncLock
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub UpdateJikaFirstTime()
        ListKoordinatBacaH.Clear()
        ListKoordinatBacaV.Clear()
        If FirstTimeAmbilYgDicekH Then
            KandidatSourceCPUAIH.Clear()
            KH = 0
            FirstTimeAmbilYgDicekH = False
        End If

        If FirstTimeAmbilYgDicekV Then
            KandidatSourceCPUAIV.Clear()
            KV = 0
            FirstTimeAmbilYgDicekV = False
        End If
    End Sub

    Private Sub UpdateAmbilKandidatBaseKoordinatVertikal()
        Dim Sudut = 0
        Dim Ring = 0
        Dim Counter = 0
        'Dim kriteriavertikal As Boolean = True
        Do
            If PertahankanKoordinatAwalV Then
                Exit Do
            End If

            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)

            If TileBoard(koordinat.X, koordinat.Y).IsBlocked And TileBoard(koordinat.X, koordinat.Y).IsOpen And TileBoard(koordinat.X, koordinat.Y).IsCheckedV = False Then
                KoordinatAsalV = koordinat

                Exit Sub
            End If

            If Ring + 1 > Math.Floor(TileBoard.GetLength(0) / 2) And Sudut = 7 And Counter = Ring + Ring - 1 Then

                KoordinatAsalV = New Vector2(-1, -1)


                For Each Kotak In TileBoard
                    Kotak.IsCheckedV = False
                Next
                Exit Do
            End If
        Loop
    End Sub

    Private Sub UpdateAmbilKandidatBaseKoordinatHorizontal()
        Dim Sudut = 0
        Dim Ring = 0
        Dim Counter = 0
        'Dim kriteriahorizontal As Boolean = True
        Do
            If PertahankanKoordinatAwalH Then
                Exit Do
            End If

            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)

            If TileBoard(koordinat.X, koordinat.Y).IsBlocked And TileBoard(koordinat.X, koordinat.Y).IsOpen And TileBoard(koordinat.X, koordinat.Y).IsCheckedH = False Then
                KoordinatAsalH = koordinat
                Exit Sub
            End If

            If Ring + 1 > Math.Floor(TileBoard.GetLength(0) / 2) And Sudut = 7 And Counter = Ring + Ring - 1 Then
                KoordinatAsalH = New Vector2(-1, -1)
                For Each Kotak In TileBoard
                    Kotak.IsCheckedH = False
                Next
                Exit Do
            End If
        Loop
    End Sub

    Private Sub UpdateTileMenu(ByVal MouseIsHolding As Boolean, ByRef ValueOfHolding As UShort, ByRef Delete As Boolean)
        For i = 0 To TileMenu.Length - 1
            TileMenu(i).Update(ProsesMenuDiPilih, MouseIsHolding, ValueOfHolding, Delete, GiliranPlayer, Pemain(GiliranPlayer).Fase)
        Next
    End Sub

    Private Sub UpdatePertahankanKoordinatAwal()
        'If ListKoordinatBacaHorizontal.Count > 14 Then
        '    PertahankanKoordinatAwalH = True
        'Else
        PertahankanKoordinatAwalH = False
        'End If

        'If ListKoordinatBacaVertikal.Count > 14 Then
        '    PertahankanKoordinatAwalV = True
        'Else
        PertahankanKoordinatAwalV = False
        'End If
    End Sub

    Private Sub UpdateAmbilTileYangDapatDicekHorizontal()
        Dim Tengah = Math.Floor(TileBoard.GetLength(0) / 2)

        If KoordinatAsalH = New Vector2(-1, -1) Then
            Exit Sub
        End If

        Dim cek As Boolean = True
        For kiri = KoordinatAsalH.Y To 0 Step -1
            With TileBoard(KoordinatAsalH.X, kiri)
                If .IsOpen And .IsDeadH = False Then
                    cek = cek And True
                Else
                    cek = cek And False
                End If
                If cek Then
                    ListKoordinatBacaH.Insert(0, New Vector2(KoordinatAsalH.X, kiri))
                Else
                    Exit For
                End If
            End With
        Next

        cek = True
        For kanan = KoordinatAsalH.Y + 1 To TileBoard.GetLength(1) - 1
            With TileBoard(KoordinatAsalH.X, kanan)
                If .IsOpen And .IsDeadH = False Then
                    cek = cek And True
                Else
                    cek = cek And False
                End If
                If cek Then
                    ListKoordinatBacaH.Add(New Vector2(KoordinatAsalH.X, kanan))
                Else
                    Exit For
                End If
            End With
        Next
        If ListKoordinatBacaH.Count > 0 Then
            Dim cariKoordinat = KandidatSourceCPUAIH.FindAll(Function(x) x.Koordinat = KoordinatAsalH)
            If cariKoordinat.Count = 0 Then
                Dim temp = Bhs.DivideAndGrouping(DaftarString(ListKoordinatBacaH), True)
                'Dim cariValue = KandidatSourceCPUAIH.FindAll(Function(x) x.Value Is temp)
                KandidatSourceCPUAIH.Add(New KandidatCPUBaseKoordinat(KoordinatAsalH, temp))
            End If
        End If
    End Sub

    Private Sub UpdateAmbilTileYangDapatDicekVertikal()
        If KoordinatAsalV = New Vector2(-1, -1) Then
            Exit Sub
        End If

        Dim cek As Boolean = True
        For up = KoordinatAsalV.X To 0 Step -1
            With TileBoard(up, KoordinatAsalV.Y)
                If .IsOpen And .IsDeadV = False Then
                    cek = cek And True
                Else
                    cek = cek And False
                End If
                If cek Then
                    ListKoordinatBacaV.Insert(0, New Vector2(up, KoordinatAsalV.Y))
                Else
                    Exit For
                End If
            End With
        Next

        cek = True
        For down = KoordinatAsalV.X + 1 To TileBoard.GetLength(1) - 1
            With TileBoard(down, KoordinatAsalV.Y)
                If .IsOpen And .IsDeadV = False Then
                    cek = cek And True
                Else
                    cek = cek And False
                End If
                If cek Then
                    ListKoordinatBacaV.Add(New Vector2(down, KoordinatAsalV.Y))
                Else
                    Exit For
                End If
            End With
        Next

        If ListKoordinatBacaV.Count > 0 Then
            Dim cariKoordinat = KandidatSourceCPUAIV.FindAll(Function(x) x.Koordinat = KoordinatAsalV)
            If cariKoordinat.Count = 0 Then
                Dim temp = Bhs.DivideAndGrouping(DaftarString(ListKoordinatBacaV), True)
                Dim cariValue = KandidatSourceCPUAIV.FindAll(Function(x) x.Value Is temp)
                KandidatSourceCPUAIV.Add(New KandidatCPUBaseKoordinat(KoordinatAsalV, temp))
            End If
        End If
    End Sub

    Private Sub UpdateTileBoardYangDitahanMouse(ByRef Sudut As Integer, ByRef Ring As Integer, ByRef Counter As Integer)
        Sudut = 0
        Ring = 0
        Counter = 0
        Do
            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)
            If TileBoard(koordinat.X, koordinat.Y).TrigerByHold Then
                IndexTileBoardOfHolding = New Vector2(koordinat.X, koordinat.Y)
                Exit Do
            End If
            If Ring + 1 > Math.Floor(TileBoard.GetLength(0) / 2) And Sudut = 7 And Counter = Ring + Ring - 1 Then
                Exit Do
            End If
        Loop
    End Sub

    Private Sub UpdateCekBahasa()
        If DaftarString(ListKoordinatBacaH).Length > 4 And StatusCekBacaH = False Then
            StatusCekBacaH = Bhs.CekBahasa(DaftarString(ListKoordinatBacaH))
            TempScoreH = Bhs.Score(DaftarString(ListKoordinatBacaH))
            Dim temp = Function(ByVal tt As List(Of Vector2))
                           Dim s As New List(Of Vector2)
                           For Each t In tt
                               s.Add(t)
                           Next
                           Return s
                       End Function
            TempDftrH = temp(ListKoordinatBacaH)
            If StatusCekBacaH Then
                For Each Kotak In TileBoard
                    If Kotak.IsBlocked = False And Kotak.Value <> 127 Then
                        Kotak.IsWrongPlace = BenarSalahNetral.Salah
                    End If
                Next
                For Each L In ListKoordinatBacaH
                    'If TileBoard(L.X, L.Y).IsBlocked = False Then
                    TileBoard(L.X, L.Y).IsWrongPlace = BenarSalahNetral.Benar
                    'End If
                Next
            End If
        End If

        If DaftarString(ListKoordinatBacaV).Length > 4 And StatusCekBacaV = False Then
            StatusCekBacaV = Bhs.CekBahasa(DaftarString(ListKoordinatBacaV))
            TempScoreV = Bhs.Score(DaftarString(ListKoordinatBacaV))
            Dim temp = Function(ByVal tt As List(Of Vector2))
                           Dim s As New List(Of Vector2)
                           For Each t In tt
                               s.Add(t)
                           Next
                           Return s
                       End Function
            TempDftrV = temp(ListKoordinatBacaV)
            If StatusCekBacaV Then
                For Each Kotak In TileBoard
                    If Kotak.IsBlocked = False And Kotak.Value <> 127 Then
                        Kotak.IsWrongPlace = BenarSalahNetral.Salah
                    End If
                Next
                For Each L In ListKoordinatBacaV
                    TileBoard(L.X, L.Y).IsWrongPlace = BenarSalahNetral.Benar
                Next
            End If
        End If
    End Sub

    Private Sub UpdateMenuTerpilih()
        If MenuOKAktif = False And ProsesMenuDiPilih = Summy.TileMenu.Menus.Lanjut And GiliranPlayer = PlayerTurn.Human AndAlso Pemain(GiliranPlayer).Fase = PhasePlayer.Running Then
            Pemain(GiliranPlayer).Fase = PhasePlayer.Finish
        End If
        If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish And TImeRunTemp >= WaktuPostCPU Then
            '(StatusCekBacaH Or StatusCekBacaV Or TImeRunTemp >= WaktuPostCPU) Then
            For i = 0 To TileBoard.GetLength(0) - 1
                For j = 0 To TileBoard.GetLength(1) - 1
                    With TileBoard(i, j)
                        If .IsWrongPlace = BenarSalahNetral.Salah Then
                            For Each PlKotak In Pemain(GiliranPlayer).TileYangDiPegang
                                If PlKotak.IsBlocked Then
                                    If .Value = PlKotak.Value Then
                                        PlKotak.IsBlocked = False
                                        .Value = 127
                                        .IsWrongPlace = BenarSalahNetral.Netral
                                        Exit For
                                    End If
                                End If
                            Next
                        End If

                        If .IsWrongPlace = BenarSalahNetral.Benar Then
                            If StatusCekBacaH Then
                                .IsDeadH = True
                                If j - 1 >= 0 AndAlso TileBoard(i, j - 1).Value = 127 Then
                                    TileBoard(i, j - 1).IsBlocked = True
                                End If
                                If j + 1 < TileBoard.GetLength(1) AndAlso TileBoard(i, j + 1).Value = 127 Then
                                    TileBoard(i, j + 1).IsBlocked = True
                                End If
                            End If
                            If StatusCekBacaV Then
                                .IsDeadV = True
                                If i - 1 >= 0 AndAlso TileBoard(i - 1, j).Value = 127 Then
                                    TileBoard(i - 1, j).IsBlocked = True
                                End If
                                If i + 1 < TileBoard.GetLength(1) AndAlso TileBoard(i + 1, j).Value = 127 Then
                                    TileBoard(i + 1, j).IsBlocked = True
                                End If
                            End If

                            For Each PlKotak In Pemain(GiliranPlayer).TileYangDiPegang
                                If PlKotak.IsBlocked Then
                                    If .Value = PlKotak.Value Then
                                        If StacksTile.Count > 0 Then
                                            PlKotak.IsBlocked = False
                                            PlKotak.IsTrigerAnimation = True
                                            PlKotak.Value = StacksTile(StacksTile.Count - 1)
                                            StacksTile.RemoveAt(StacksTile.Count - 1)
                                        Else
                                            PlKotak.Value = 127
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next
                            .IsBlocked = True
                            .IsWrongPlace = BenarSalahNetral.Netral
                        End If
                    End With
                Next
            Next
            ReleaseOnTile = True
            MouseIsHolding = True
            If Not MenuOKAktif Then
                MenuOKAktif = True
            End If
            If Not StatusCekBacaH And Not StatusCekBacaV Then
                For Each Kotak In Pemain(GiliranPlayer).TileYangDiPegang
                    If Kotak.IsBlocked Then
                        Kotak.IsBlocked = False
                    End If
                Next

                For Each Kotak In TileBoard
                    If Kotak.Value <> 127 And Kotak.IsBlocked = False Then
                        Kotak.Value = 127
                    End If
                Next
            End If
            ProsesMenuDiPilih = Summy.TileMenu.Menus.None
        ElseIf ProsesMenuDiPilih = Summy.TileMenu.Menus.Kembalikan And GiliranPlayer = PlayerTurn.Human Then
            For Each Kotak In Pemain(GiliranPlayer).TileYangDiPegang
                If Kotak.IsBlocked Then
                    Kotak.IsBlocked = False
                End If
            Next

            For Each Kotak In TileBoard
                If Kotak.Value <> 127 And Kotak.IsBlocked = False Then
                    Kotak.Value = 127
                End If
            Next

            ReleaseOnTile = True
            MouseIsHolding = True
            ProsesMenuDiPilih = Summy.TileMenu.Menus.None
        End If
    End Sub

    Private Sub UpdateJikaMouseDiLepas()
        If ((MouseStateBefore.LeftButton = ButtonState.Released And MouseStateAfter.LeftButton = ButtonState.Released) Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete)))) Then
            For Each Kotak In TileBoard
                Kotak.TrigerByHold = False
            Next

            For Each Kotak In Pemain(GiliranPlayer).TileYangDiPegang
                Kotak.TrigerByHold = False
            Next

            'Kosongkan tile yang akan dipindahkan
            If (ReleaseOnTile And ProsesMenuDiPilih = Summy.TileMenu.Menus.None) Or DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) And MouseIsHolding Then
                'Untuk TileBoard
                If IndexTileBoardOfHolding <> New Vector2(-1, -1) Then
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke tempat baru
                    If ReleaseOnTile Then
                        TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value = 127
                    End If
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke recycle
                    If DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) Then
                        For Each Kotak In Pemain(GiliranPlayer).TileYangDiPegang
                            If Kotak.Value = TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value And Kotak.IsBlocked = True Then
                                If StacksTile.Count > 0 Then
                                    Kotak.IsBlocked = False
                                    Kotak.IsTrigerAnimation = True
                                    Kotak.Value = StacksTile(StacksTile.Count - 1)
                                    StacksTile.RemoveAt(StacksTile.Count - 1)
                                    TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value = 127
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                'Untuk TilePlayer
                If IndexTilePlayerOfHolding <> -1 Then
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke papan permainan
                    If ReleaseOnTile Then
                        Pemain(GiliranPlayer).TileYangDiPegang(IndexTilePlayerOfHolding).IsBlocked = True
                    End If
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke recycle
                    If DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) Then
                        If StacksTile.Count > 0 And Pemain(GiliranPlayer).SwapChance > 0 Then
                            Pemain(GiliranPlayer).TileYangDiPegang(IndexTilePlayerOfHolding).IsBlocked = False
                            Pemain(GiliranPlayer).TileYangDiPegang(IndexTilePlayerOfHolding).IsTrigerAnimation = True
                            Pemain(GiliranPlayer).TileYangDiPegang(IndexTilePlayerOfHolding).Value = StacksTile(StacksTile.Count - 1)
                            StacksTile.RemoveAt(StacksTile.Count - 1)
                            Pemain(GiliranPlayer).SwapChance -= 1
                        End If
                    End If
                End If

                'Reset value kembali normal
                ReleaseOnTile = False
                DeleteOnTile = False

                'Reset value IsOpen pada semua TileBoard
                For Each Kotak In TileBoard
                    'Kotak.AmountColorRO = 1
                    If Kotak.IsBlocked = False Then
                        Kotak.IsOpen = False
                    End If
                    Kotak.IsWrongPlace = BenarSalahNetral.Netral
                Next

                'Update Score Jika menu yang terpilih = "OK'
                Dim pos = Function()
                              Dim last = Pemain(GiliranPlayer).RecentMove(Pemain(GiliranPlayer).RecentMove.Count - 1)
                              Dim mid = last(last.Count / 2)
                              Return TileBoard(mid.X, mid.Y).Position
                          End Function
                If MenuOKAktif Then
                    If StatusCekBacaH Then
                        Pemain(GiliranPlayer).RecentScore = TempScoreH
                        Pemain(GiliranPlayer).RecentMove.Add(TempDftrH)
                        Pemain(GiliranPlayer).RecentMoveHightlight.Add(False)
                        'Pemain(PlayerTurn.CPU).SkipChance = 2
                        'Pemain(PlayerTurn.Human).SkipChance = 2
                        Pemain(GiliranPlayer).ScorePosNotif = pos()
                    ElseIf Not StatusCekBacaH And Not StatusCekBacaV Then
                        Pemain(GiliranPlayer).SkipChance -= 1
                    ElseIf StatusCekBacaV Then
                        Pemain(GiliranPlayer).RecentScore = TempScoreV
                        Pemain(GiliranPlayer).RecentMove.Add(TempDftrV)
                        Pemain(GiliranPlayer).RecentMoveHightlight.Add(False)
                        'Pemain(PlayerTurn.CPU).SkipChance = 2
                        'Pemain(PlayerTurn.Human).SkipChance = 2
                        Pemain(GiliranPlayer).ScorePosNotif = pos()
                    End If
                    MenuOKAktif = False
                    Pemain(GiliranPlayer).AkumulasiDetikBerjalan += Pemain(GiliranPlayer).DetikBerjalan
                    Pemain(GiliranPlayer).SwapChance = 8
                    If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish Then
                        Pemain(GiliranPlayer).Fase = PhasePlayer.FinishNotif
                    End If
                    TImeRunTemp = 0
                End If

                'Reset value FirstTime dan Baca
                FirstTimeAmbilYgDicekH = True
                StatusCekBacaH = False
                TempScoreH = 0
                ListKoordinatBacaH.Clear()
                KH = -1
                KandidatSourceCPUAIH.Clear()

                FirstTimeAmbilYgDicekV = True
                StatusCekBacaV = False
                TempScoreV = 0
                ListKoordinatBacaV.Clear()
                KV = -1
                KandidatSourceCPUAIV.Clear()

            End If

            'Netralkan
            MouseIsHolding = False
            ValueOfHolding = 127

            IndexTileBoardOfHolding = New Vector2(-1, -1)
            IndexTilePlayerOfHolding = -1
        End If

    End Sub

    Private Sub UpdateTilePlayer()
        For i = 0 To Pemain(PlayerTurn.Human).TileYangDiPegang.Length - 1
            Pemain(PlayerTurn.Human).TileYangDiPegang(i).Update(MouseIsHolding, ValueOfHolding, ReleaseOnTile, GiliranPlayer, Pemain(GiliranPlayer).Fase)
            If Pemain(PlayerTurn.Human).TileYangDiPegang(i).TrigerByHold Then
                IndexTilePlayerOfHolding = i
            End If
        Next

        For i = 0 To Pemain(PlayerTurn.CPU).TileYangDiPegang.Length - 1
            Pemain(PlayerTurn.CPU).TileYangDiPegang(i).Update(MouseIsHolding, ValueOfHolding, ReleaseOnTile, GiliranPlayer, Pemain(GiliranPlayer).Fase)
            If Pemain(PlayerTurn.CPU).TileYangDiPegang(i).TrigerByHold Then
                IndexTilePlayerOfHolding = i
            End If
        Next
    End Sub

    Private Sub UpdateTileBoard(ByRef Sudut As Integer, ByRef Ring As Integer, ByRef Counter As Integer)

        Sudut = 0
        Ring = 0
        Counter = 0

        Do
            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)
            TileBoard(koordinat.X, koordinat.Y).Update(MouseIsHolding, ValueOfHolding, ReleaseOnTile, GiliranPlayer, Pemain(GiliranPlayer).Fase)
            UpdateTileHidup(koordinat.X, koordinat.Y)
            If TileBoard(koordinat.X, koordinat.Y).IsOpen Then
                Dim s = New Vector2(koordinat.X, koordinat.Y)
            End If
            If Ring + 1 > Math.Floor(TileBoard.GetLength(0) / 2) And Sudut = 7 And Counter = Ring + Ring - 1 Then
                If KoordinatAsalH <> New Vector2(-1, -1) Then
                    TileBoard(KoordinatAsalH.X, KoordinatAsalH.Y).IsCheckedH = True
                End If

                If KoordinatAsalV <> New Vector2(-1, -1) Then
                    TileBoard(KoordinatAsalV.X, KoordinatAsalV.Y).IsCheckedV = True
                End If

                Exit Do
            End If
        Loop
    End Sub

    Private Function KelayakanCPU(avb As UInt16, Koordinat As Vector2, Solusi As String, KPembantu As List(Of Vector2), IsBaris As Boolean, Player As TileBoard()) As Boolean
        Dim Cocok As Boolean = False
        Dim Kpem = KPembantu
        Dim Sol As List(Of Char) = Solusi.ToList
        Dim Plyr = Player.ToList
        If AktifCPU AndAlso CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) < BatasWaktuTunggu Then
            If avb = 0 Then
                If If(IsBaris, Koordinat.Y, Koordinat.X) - 1 >= 0 Then
                    If TileBoard(Koordinat.X - If(Not IsBaris, 1, 0), Koordinat.Y - If(IsBaris, 1, 0)).Value <> 127 Then
                        Return False
                    End If
                End If
            End If
            'Batas Atas
            If avb - 1 >= 0 And If(IsBaris, Koordinat.Y, Koordinat.X) - 1 >= 0 Then
                For i = avb - 1 To 0 Step -1
                    If i = 0 Then
                        If If(IsBaris, Koordinat.Y, Koordinat.X) - (avb + 1 - i) >= 0 Then
                            If TileBoard(Koordinat.X - If(Not IsBaris, (avb + 1 - i), 0), Koordinat.Y - If(IsBaris, (avb + 1 - i), 0)).Value <> 127 Then
                                Return False
                            End If
                        End If
                    End If

                    'Cek Kpembantu (Tile yang telah ada dipapan permainan)
                    For Each k In Kpem
                        If Solusi(i) = TileBoard(k.X, k.Y).CharValue And If(IsBaris, Koordinat.Y - k.Y, Koordinat.X - k.X) = avb - i Then
                            Kpem.Remove(k)
                            Sol.Remove(Solusi(i))
                            i -= 1
                            Exit For
                        End If
                    Next

                    If i = 0 Then
                        If If(IsBaris, Koordinat.Y, Koordinat.X) - (avb + 1 - i) >= 0 Then
                            If TileBoard(Koordinat.X - If(Not IsBaris, (avb + 1 - i), 0), Koordinat.Y - If(IsBaris, (avb + 1 - i), 0)).Value <> 127 Then
                                Return False
                            End If
                        End If
                    End If

                    If i < 0 Then
                        Continue For
                    End If

                    'Cek Tile yang ada ditangan Player CPU
                    For Each p In Plyr
                        If (Solusi(i) = p.CharValue And If(IsBaris, Koordinat.Y, Koordinat.X) - (avb - i) >= 0) AndAlso
                                (TileBoard(Koordinat.X - If(Not IsBaris, (avb - i), 0), Koordinat.Y - If(IsBaris, (avb - i), 0)).Value = 127 And
                                Not TileBoard(Koordinat.X - If(Not IsBaris, (avb - i), 0), Koordinat.Y - If(IsBaris, (avb - i), 0)).IsBlocked) Then
                            Plyr.Remove(p)
                            Sol.Remove(Solusi(i))
                            Exit For
                        End If
                    Next
                Next
            End If

            If avb = Solusi.Length - 1 Then
                If If(IsBaris, Koordinat.Y, Koordinat.X) + 1 < TileBoard.GetLength(0) Then
                    If TileBoard(Koordinat.X + If(Not IsBaris, 1, 0), Koordinat.Y + If(IsBaris, 1, 0)).Value <> 127 Then
                        Return False
                    End If
                End If
            End If
            'Batas Bawah
            If avb + 1 < Solusi.Length And If(IsBaris, Koordinat.Y, Koordinat.X) + 1 < TileBoard.GetLength(0) Then
                For i = avb + 1 To Solusi.Length - 1
                    If i = Solusi.Length - 1 Then
                        If If(IsBaris, Koordinat.Y, Koordinat.X) + (i + 1 - avb) < TileBoard.GetLength(0) Then
                            If TileBoard(Koordinat.X + If(Not IsBaris, (i + 1 - avb), 0), Koordinat.Y + If(IsBaris, (i + 1 - avb), 0)).Value <> 127 Then
                                Return False
                            End If
                        End If
                    End If

                    'Cek Kpembantu (Tile yang telah ada dipapan permainan)
                    For Each k In Kpem
                        If Solusi(i) = TileBoard(k.X, k.Y).CharValue And If(IsBaris, k.Y - Koordinat.Y, k.X - Koordinat.X) = i - avb Then
                            Kpem.Remove(k)
                            Sol.Remove(Solusi(i))
                            i += 1
                            Exit For
                        End If
                    Next

                    If i = Solusi.Length - 1 Then
                        If If(IsBaris, Koordinat.Y, Koordinat.X) + (i + 1 - avb) < TileBoard.GetLength(0) Then
                            If TileBoard(Koordinat.X + If(Not IsBaris, (i + 1 - avb), 0), Koordinat.Y + If(IsBaris, (i + 1 - avb), 0)).Value <> 127 Then
                                Return False
                            End If
                        End If
                    End If

                    If i >= Solusi.Length Then
                        Continue For
                    End If

                    'Cek Tile yang ada ditangan Player CPU
                    For Each p In Plyr
                        If (Solusi(i) = p.CharValue And If(IsBaris, Koordinat.Y, Koordinat.X) + (i - avb) < TileBoard.GetLength(0)) AndAlso
                                   (TileBoard(Koordinat.X + If(Not IsBaris, i - avb, 0), Koordinat.Y + If(IsBaris, i - avb, 0)).Value = 127 And
                                   Not TileBoard(Koordinat.X + If(Not IsBaris, i - avb, 0), Koordinat.Y + If(IsBaris, i - avb, 0)).IsBlocked) Then
                            Plyr.Remove(p)
                            Sol.Remove(Solusi(i))
                            Exit For
                        End If
                    Next
                Next
            End If
            If Sol.Count = 1 Then
                Cocok = True
            End If
        End If
        Return Cocok
    End Function

    Private Sub CPU_LetakHasilTervalidasi(avb As UInt16, Koordinat As Vector2, Solusi As String, IsHorizontal As Boolean)
        Dim Plye = Pemain(GiliranPlayer).TileYangDiPegang.ToArray
        'Batas Atas
        If avb - 1 >= 0 And If(IsHorizontal, Koordinat.Y, Koordinat.X) - 1 >= 0 Then
            For i = avb - 1 To 0 Step -1
                'Letak Tile yang ada ditangan Player CPU ke papan permainan
                For Each p In Plye
                    If Not p.IsBlocked Then
                        If (Solusi(i) = p.CharValue And If(IsHorizontal, Koordinat.Y, Koordinat.X) - (avb - i) >= 0) AndAlso
                            (TileBoard(Koordinat.X - If(Not IsHorizontal, (avb - i), 0), Koordinat.Y - If(IsHorizontal, (avb - i), 0)).Value = 127 And
                            Not TileBoard(Koordinat.X - If(Not IsHorizontal, (avb - i), 0), Koordinat.Y - If(IsHorizontal, (avb - i), 0)).IsBlocked) Then
                            p.IsBlocked = True
                            Pemain(GiliranPlayer).TileYangDiPegang(Array.IndexOf(Plye, p)).IsBlocked = True
                            TileBoard(Koordinat.X - If(Not IsHorizontal, (avb - i), 0), Koordinat.Y - If(IsHorizontal, (avb - i), 0)).Value = p.Value
                            Exit For
                        End If
                    End If
                Next
            Next
        End If

        'Batas Bawah
        If avb + 1 < Solusi.Length And If(IsHorizontal, Koordinat.Y, Koordinat.X) + 1 < TileBoard.GetLength(0) Then
            For i = avb + 1 To Solusi.Length - 1
                'Letak Tile yang ada ditangan Player CPU ke papan permainan
                For Each p In Plye
                    If Not p.IsBlocked Then
                        If (Solusi(i) = p.CharValue And If(IsHorizontal, Koordinat.Y, Koordinat.X) + (i - avb) < TileBoard.GetLength(0)) AndAlso
                               (TileBoard(Koordinat.X + If(Not IsHorizontal, i - avb, 0), Koordinat.Y + If(IsHorizontal, i - avb, 0)).Value = 127 And
                               Not TileBoard(Koordinat.X + If(Not IsHorizontal, i - avb, 0), Koordinat.Y + If(IsHorizontal, i - avb, 0)).IsBlocked) Then
                            p.IsBlocked = True
                            Pemain(GiliranPlayer).TileYangDiPegang(Array.IndexOf(Plye, p)).IsBlocked = True
                            TileBoard(Koordinat.X + If(Not IsHorizontal, i - avb, 0), Koordinat.Y + If(IsHorizontal, i - avb, 0)).Value = p.Value
                            Exit For
                        End If
                    End If
                Next
            Next
        End If

    End Sub

    Private Function AvaibleFormula(d As HasilPermutasi, KandidatSource As KandidatCPUBaseKoordinat, KPembantu As List(Of Vector2)) As Integer
        Dim cTemp = d.Persamaan.ToList
        Dim indeks = -1

        For Each c In d.Persamaan.ToList
            If c = TileBoard(KandidatSource.Koordinat.X, KandidatSource.Koordinat.Y).CharValue Then
                indeks = cTemp.IndexOf(c)
                'cTemp.Remove(c)
                Exit For
            End If
        Next
        Return indeks
    End Function

    Friend Function Spiral(ByRef Ring As UInt16, ByRef Sudut As UInt16, ByRef Counter As UInt16) As Vector2
        Dim Tengah As UInt16 = Math.Floor(TileBoard.GetLength(0) / 2)

        'Tepat ditengah
        If Ring = 0 Then
            Ring += 1
            Sudut = 0
            Counter = 0
            Return New Vector2(Tengah, Tengah)
        End If

        If Ring > 0 Then
            If Counter > Ring + Ring - 2 Then
                Sudut += 1
                Counter = 0
            End If

            If Sudut > 7 Then
                Ring += 1
                Sudut = 0
            End If

            'Kiri Atas
            If Sudut = 0 Then
                Sudut += 1
                Return New Vector2(Tengah - Ring, Tengah - Ring)
            End If

            'Atas
            If Sudut = 1 Then
                Counter += 1
                Return New Vector2(Tengah - Ring, Tengah - Ring + Counter)
            End If

            'Atas Kanan
            If Sudut = 2 Then
                Sudut += 1
                Return New Vector2(Tengah - Ring, Tengah + Ring)
            End If

            'Kanan
            If Sudut = 3 Then
                Counter += 1
                Return New Vector2(Tengah - Ring + Counter, Tengah + Ring)
            End If

            'Kanan Bawah
            If Sudut = 4 Then
                Sudut += 1
                Return New Vector2(Tengah + Ring, Tengah + Ring)
            End If

            'Bawah
            If Sudut = 5 Then
                Counter += 1
                Return New Vector2(Tengah + Ring, Tengah - Ring + (Ring + Ring) - Counter)
            End If

            'Bawah Kiri
            If Sudut = 6 Then
                Sudut += 1
                Return New Vector2(Tengah + Ring, Tengah - Ring)
            End If

            'Kiri
            If Sudut = 7 Then
                Counter += 1
                Return New Vector2(Tengah - Ring + (Ring + Ring) - Counter, Tengah - Ring)
            End If

        End If
    End Function

    Private Sub UpdateTileHidup(ByVal Baris As UInt16, ByVal Kolom As UInt16)
        If Baris = Math.Floor(TileBoard.GetLength(0) / 2) And Kolom = Math.Floor(TileBoard.GetLength(1) / 2) Then
            Exit Sub
        End If

        'Atas
        If Baris > 0 Then
            If TileBoard(Baris, Kolom).Value <> 127 And TileBoard(Baris - 1, Kolom).IsOpen Then
                TileBoard(Baris, Kolom).IsOpen = True
                Exit Sub
            End If

        End If

        'Kanan
        If Kolom < TileBoard.GetLength(1) - 1 Then
            If TileBoard(Baris, Kolom).Value <> 127 And TileBoard(Baris, Kolom + 1).IsOpen Then
                TileBoard(Baris, Kolom).IsOpen = True
                Exit Sub
            End If
        End If

        'Bawah
        If Baris < TileBoard.GetLength(0) - 1 Then
            If TileBoard(Baris, Kolom).Value <> 127 And TileBoard(Baris + 1, Kolom).IsOpen Then
                TileBoard(Baris, Kolom).IsOpen = True
                Exit Sub
            End If
        End If

        'Kiri
        If Kolom > 0 Then
            If TileBoard(Baris, Kolom).Value <> 127 And TileBoard(Baris, Kolom - 1).IsOpen Then
                TileBoard(Baris, Kolom).IsOpen = True
                Exit Sub
            End If
        End If

        TileBoard(Baris, Kolom).IsOpen = False
    End Sub

    Private Function DaftarString(ByVal Lis As List(Of Vector2)) As String
        Dim h As New StringBuilder
        For i = 0 To Lis.Count - 1
            h = h.Append(Bhs.ConvertValue(TileBoard(Lis(i).X, Lis(i).Y).Value))
        Next
        Return h.ToString
    End Function
#End Region
End Class
