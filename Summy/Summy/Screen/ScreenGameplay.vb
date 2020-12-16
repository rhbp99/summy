Imports System.Text
Imports System.Threading

Friend Class ScreenGameplay
    Implements IBaseScreen
#Region "Deklarasi Properti"
    Friend Property Visible As Boolean Implements IBaseScreen.Visible
    Friend Property Enable As Boolean Implements IBaseScreen.Enable
    Private greedy As New GreedyProcess
    Private TileBoard(14, 14) As TileBoard
    Private TilePlayer(7) As IBaseTile
    Private TileMenu(1) As TileMenu
    Private TrashMenu As TileCustom
    Private BaseTile As Texture2D
    Private BaseMenu As Texture2D
    Private ContentTile As Texture2D
    Private RecycleTile As Texture2D
    Private DebugFont As SpriteFont
    Private MenuTile As Texture2D
    Private MouseIsHolding As Boolean = False
    Private ValueOfHolding As UInt16 = 127
    Private IndexTileBoardOfHolding As New Vector2(-1, -1)
    Private IndexTilePlayerOfHolding As Int16 = -1
    Private ReleaseOnTile As Boolean = False
    Private DeleteOnTile As Boolean = False
    Private ProsesMenuDiPilih As UInt16 = 3
    Private ReadOnly Colour As Color = Color.White
    Private StatusCekBacaH As Boolean = False
    Private StatusCekBacaV As Boolean = False
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
    'Private FirsTimeTileHidup As Boolean = True
    Private DaftarYangMauDisusunH As New List(Of String)
    Private DaftarYangMauDisusunV As New List(Of String)
    Private Sudut As Integer = Nothing
    Private Ring As Integer = Nothing
    Private Counter As Integer = Nothing
    Private TempAsalH As New Vector2
    Private TempAsalV As New Vector2
    Private KandidatSourceCPUAIH As New List(Of KandidatCPUBaseKoordinat)
    Private KandidatSourceCPUAIV As New List(Of KandidatCPUBaseKoordinat)
    Private AktifCPU As Boolean = False
    Dim KH As Integer = -1
    Dim KV As Integer = -1
    Private SolusiH As String = ""
    Private SolusiV As String = ""
    Private TsData As New TesTileBoard
    Private NonAktifCPUH = False
    Private NonAktifCPUV = False
    Private TimePlayerSecondsElapsed As Single
    Private TimePlayerSecondsTotal As Single
    Private Pemain(1) As Player
    Private GiliranPlayer As PlayerTurn
    Private ThreadBaru As Thread

    Enum PlayerTurn
        Human
        CPU
    End Enum

    Friend Structure Player
        Friend Score As Integer
        Friend Fase As PhasePlayer
    End Structure

    Friend Enum PhasePlayer
        Standby
        Running
        Finish
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

    Friend Sub New()
        Pemain(0) = New Player
        Pemain(1) = New Player
        Visible = True
        Enable = True
        InisiasiBoard()
        InisiasiKotakPlayer()
        InisiasiMenu()
        InisiasiTrash()
        Dim r = RandomNumber.Next(0, 9999)
        If r Mod 2 = 1 Then
            GiliranPlayer = PlayerTurn.CPU
        Else
            GiliranPlayer = PlayerTurn.Human
        End If

        BaseTile = ContentManager.Load(Of Texture2D)("gfx/tiled")
        ContentTile = ContentManager.Load(Of Texture2D)("gfx/number")
        MenuTile = ContentManager.Load(Of Texture2D)("gfx/menu")
        BaseMenu = ContentManager.Load(Of Texture2D)("gfx/basemenu")
        RecycleTile = ContentManager.Load(Of Texture2D)("gfx/trash")
        DebugFont = ContentManager.Load(Of SpriteFont)("Debug")
    End Sub

    Friend Sub Load() Implements IBaseScreen.Load
        'Load texture


    End Sub

    Friend Sub Draw() Implements IBaseScreen.Draw
        If Visible Then
            TrashMenu.Draw(RecycleTile, RecycleTile)

            'Gambarkan base dari menu
            For Each Kotak In TileMenu
                Kotak.Draw(BaseMenu, MenuTile)
            Next

            'Gambarkan base dasar board
            For Each Kotak In TileBoard
                Kotak.Draw(BaseTile, ContentTile)
            Next

            'Gambarkan base dasar kotak player
            For Each Kotak In TilePlayer
                Kotak.Draw(BaseTile, ContentTile)
            Next

            'Gambarkan tile yang berada di bawah mouse
            For Each Kotak In TileBoard
                If Kotak.TrigerByOnTop Then
                    Kotak.Draw(BaseTile, ContentTile)
                    Exit For
                End If
            Next

            'Gambarkan tile yang dipegang oleh mouse
            For Each Kotak In TileBoard
                If Kotak.TrigerByHold Then
                    Kotak.Draw(BaseTile, ContentTile)
                    Exit For
                End If
            Next

            'Gambarkan tile yang berada di bawah mouse
            For Each Kotak In TilePlayer
                If Kotak.TrigerByOnTop Then
                    Kotak.Draw(BaseTile, ContentTile)
                    Exit For
                End If
            Next

            'Gambarkan tile yang dipegang oleh mouse
            For Each Kotak In TilePlayer
                If Kotak.TrigerByHold Then
                    Kotak.Draw(BaseTile, ContentTile)
                    Exit For
                End If
            Next

            Dim daftar = Function(ByVal Lis As List(Of Vector2))
                             Dim h As String = ""
                             For i = 0 To Lis.Count - 1
                                 h = h & "(" & Lis(i).X & "," & Lis(i).Y & ")"
                             Next
                             Return h
                         End Function
            Dim teks As New StringBuilder
            teks.AppendLine($"Debug mode")
            teks.AppendLine($"{NameOf(GiliranPlayer)} : {GiliranPlayer}")
            teks.AppendLine($"Menu : {ProsesMenuDiPilih}")
            teks.AppendLine($"MouseIsHolding : {MouseIsHolding}")
            teks.AppendLine($"ValueOfHolding : {ValueOfHolding} ( {Bhs.ConvertValue(ValueOfHolding)} )")
            teks.AppendLine($"IndexTileBoardOfHolding : {IndexTileBoardOfHolding.X} , {IndexTileBoardOfHolding.Y}")
            teks.AppendLine($"IndexTilePlayerOfHolding :   {IndexTilePlayerOfHolding}")
            teks.AppendLine($"BacaHorizontal : {DaftarString(ListKoordinatBacaH)}")
            teks.AppendLine($"ListBacaHorizontal :   {daftar(ListKoordinatBacaH)}")
            teks.AppendLine($"StatusCekBacaHorizontal : {StatusCekBacaH}")
            teks.AppendLine($"BacaVertikal :   {DaftarString(ListKoordinatBacaV)}")
            teks.AppendLine($"ListBacaVertikal : {daftar(ListKoordinatBacaV)}")
            teks.AppendLine($"StatusCekBacaVertikal :   {StatusCekBacaV}")
            teks.AppendLine($"KoordinatAsalHorizontal : {KoordinatAsalH.X},{KoordinatAsalH.Y}")
            teks.AppendLine($"KoordinatAsalVertikal :   {KoordinatAsalV.X},{KoordinatAsalV.Y}")
            teks.AppendLine($"Score Player : {Pemain(PlayerTurn.Human).Score}")
            teks.AppendLine($"Score CPU :  {Pemain(PlayerTurn.CPU).Score}")
            teks.AppendLine($"DaftarKandidatH : {DaftarKandidatH.Count}")
            teks.AppendLine($"DaftarKandidatV : {DaftarKandidatV.Count}")
            teks.AppendLine($"KandidatSourceCPUAIH : {KandidatSourceCPUAIH.Count} - KH : {KH}")
            teks.AppendLine($"KandidatSourceCPUAIV : {KandidatSourceCPUAIV.Count} - KV : {KV}")
            teks.AppendLine($"CPU is Aktif : {AktifCPU}")
            teks.AppendLine($"SolusiH : {SolusiH}")
            teks.AppendLine($"CPU Horizontal Status : ")
            teks.AppendLine($"{CPUHString}")
            teks.AppendLine($"SolusiV : {SolusiV}")
            teks.AppendLine($"CPU Vertikal Status : ")
            teks.AppendLine($"{CPUVString}")
            teks.AppendLine($"Waktu Player Elapsed : {TimePlayerSecondsElapsed}")
            teks.AppendLine($"Waktu Player Total : {TimePlayerSecondsTotal}")
            Sprite.DrawString(DebugFont, teks.ToString, New Vector2(GameSize.Width * 0.75, 20), Color.Black, 0, Nothing, 0.5F, Nothing, 0)
        End If
    End Sub

    Friend Sub Update() Implements IBaseScreen.Update
        If Enable Then
            'Dim c = Bhs.DivideAndGrouping("12-+:x90", True)
            'Dim s() = {"12", "13", "1", "+-", "="} : greedy.SusunKandidatVertikal(s)
            If GiliranPlayer = PlayerTurn.Human Then
                If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish Then
                    GiliranPlayer = PlayerTurn.CPU
                    Pemain(GiliranPlayer).Fase = PhasePlayer.Standby
                End If
            Else
                If Pemain(GiliranPlayer).Fase = PhasePlayer.Finish Then
                    GiliranPlayer = PlayerTurn.Human
                    Pemain(GiliranPlayer).Fase = PhasePlayer.Standby
                End If
            End If
            'Update TrashMenu
            TrashMenu.Update(MouseIsHolding, ValueOfHolding, DeleteOnTile)
            'Update Menu
            UpdateTileMenu()
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
            'Cek jika koordinat base dapat dipertahankan atau tidak
            UpdatePertahankanKoordinatAwal()
            'Cek Tile pada Board yang ditahan ole Mouse
            UpdateTileBoardYangDitahanMouse(Sudut, Ring, Counter)
            'Update TilePlayer
            UpdateTilePlayer()
            'Jalankan jika tombol mouse dilepas
            UpdateJikaMouseDiLepas()
            'Proses menu yang dipilih
            UpdateMenuTerpilih()
            'Cek tile yang terpilih(yg dapat dicek) apakah sesuai dengan sintaks bahasa dan ubah warnanya jika sesuai
            UpdateCekBahasa()
            'Jalankan AI CPU
            If KeyboardStateAfter.IsKeyDown(Keys.C) And KeyboardStateBefore.IsKeyUp(Keys.C) And Not (StatusCekBacaH Or StatusCekBacaV) Then
                If Not AktifCPU Then
                    AktifCPU = True
                    TimePlayerSecondsElapsed = 0
                    For Each pl In TilePlayer
                        pl.IsActive = False
                    Next
                End If
            End If

            If NonAktifCPUH And NonAktifCPUV Then
                KH = -1
                KV = -1
                AktifCPU = False
            End If

            NonAktifCPUH = False
            NonAktifCPUV = False

            If KandidatSourceCPUAIH.Count > 0 And KH >= 0 AndAlso (AktifCPU And CPUHState = StatusCPU.Ready) Then
                With KandidatSourceCPUAIH(KH)
                    DaftarYangMauDisusunH.Clear()
                    KPembantuH.Clear()
                    For Each kar In TilePlayer
                        If Not kar.IsBlocked Then
                            DaftarYangMauDisusunH.Add(kar.CharValue)
                        End If
                    Next

                    For i = 1 To .Koordinat.Y
                        If TileBoard(.Koordinat.X, .Koordinat.Y - i).IsBlocked Then
                            If TileBoard(.Koordinat.X, .Koordinat.Y - i).Value <> 127 Then
                                DaftarYangMauDisusunV.Add(TileBoard(.Koordinat.X, .Koordinat.Y - i).CharValue)
                                KPembantuV.Add(New Vector2(.Koordinat.X, .Koordinat.Y - i))
                            Else
                                Exit For
                            End If
                        End If
                    Next

                    For j = 1 To TileBoard.GetLength(0) - .Koordinat.Y - 1
                        If TileBoard(.Koordinat.X, .Koordinat.Y + j).IsBlocked Then
                            If TileBoard(.Koordinat.X, .Koordinat.Y + j).Value <> 127 Then
                                DaftarYangMauDisusunV.Add(TileBoard(.Koordinat.X, .Koordinat.Y + j).CharValue)
                                KPembantuV.Add(New Vector2(.Koordinat.X, .Koordinat.Y + j))
                            Else
                                Exit For
                            End If
                        End If
                    Next

                    For Each t In .Value
                        DaftarYangMauDisusunH.Add(t)
                    Next
                    If CPUHState = StatusCPU.Ready Then
                        CPUHString = $"Hello This is CPU Start KH => { .Koordinat.X},{ .Koordinat.Y}"
                        CPUHState = StatusCPU.Running
                        'ThreadBaru.Abort()
                        ThreadBaru = New Thread(AddressOf greedy.SusubKandidatHorizontal)
                        ThreadBaru.Start(DaftarYangMauDisusunH.ToArray)
                    End If
                End With
            End If

            If KandidatSourceCPUAIV.Count > 0 And KV >= 0 AndAlso (AktifCPU And CPUVState = StatusCPU.Ready) Then
                With KandidatSourceCPUAIV(KV)
                    DaftarYangMauDisusunV.Clear()
                    KPembantuV.Clear()

                    For Each kar In TilePlayer
                        If Not kar.IsBlocked Then
                            DaftarYangMauDisusunV.Add(kar.CharValue)
                        End If
                    Next

                    For i = 1 To .Koordinat.X
                        If TileBoard(.Koordinat.X - i, .Koordinat.Y).IsBlocked Then
                            If TileBoard(.Koordinat.X - i, .Koordinat.Y).Value <> 127 Then
                                DaftarYangMauDisusunV.Add(TileBoard(.Koordinat.X - i, .Koordinat.Y).CharValue)
                                KPembantuV.Add(New Vector2(.Koordinat.X - i, .Koordinat.Y))
                            Else
                                Exit For
                            End If
                        End If
                    Next

                    For j = 1 To TileBoard.GetLength(0) - .Koordinat.X - 1
                        If TileBoard(.Koordinat.X + j, .Koordinat.Y).IsBlocked Then
                            If TileBoard(.Koordinat.X + j, .Koordinat.Y).Value <> 127 Then
                                DaftarYangMauDisusunV.Add(TileBoard(.Koordinat.X + j, .Koordinat.Y).CharValue)
                                KPembantuV.Add(New Vector2(.Koordinat.X + j, .Koordinat.Y))
                            Else
                                Exit For
                            End If
                        End If
                    Next

                    For Each t In .Value
                        DaftarYangMauDisusunV.Add(t)
                    Next
                    If CPUVState = StatusCPU.Ready Then
                        CPUVString = $"Hello This is CPU Start KV => { .Koordinat.X},{ .Koordinat.Y}"
                        CPUVState = StatusCPU.Running
                        'ThreadBaru.Abort()
                        ThreadBaru = New Thread(AddressOf greedy.SusunKandidatVertikal)
                        ThreadBaru.Start(DaftarYangMauDisusunV.ToArray)
                    End If
                End With
            End If

            'Letak kandidat yang telah dikerjakan CPU
            If CPUHState = StatusCPU.Finish And AktifCPU Then
                If KandidatSourceCPUAIH.Count > 0 Then
                    Dim Daftar = DaftarKandidatH
                    For Each d In Daftar
                        SolusiH = d.Persamaan
                        Dim avb = AvaibleFormula(d, KandidatSourceCPUAIH(KH), KPembantuH)
                        Dim Cocok As Boolean = False

                        With KandidatSourceCPUAIH(KH)
                            If avb >= 0 Then
                                Cocok = KelayakanCPU(avb, .Koordinat, SolusiH, KPembantuH, True)
                            End If

                            If Cocok Then
                                EksekusiCPU(avb, .Koordinat, SolusiH, KPembantuH, True)
                                AktifCPU = False
                                TimePlayerSecondsTotal += TimePlayerSecondsElapsed
                                Exit For
                            End If
                        End With
                    Next
                    CPUHState = StatusCPU.Ready
                Else
                    CPUHState = StatusCPU.Ready
                End If
            End If

            If CPUVState = StatusCPU.Finish And AktifCPU Then
                If KandidatSourceCPUAIV.Count > 0 Then
                    Dim Daftar = DaftarKandidatV
                    For Each d In Daftar
                        SolusiV = d.Persamaan
                        Dim avb = AvaibleFormula(d, KandidatSourceCPUAIV(KV), KPembantuV)
                        Dim Cocok As Boolean = False

                        With KandidatSourceCPUAIV(KV)
                            If avb >= 0 Then
                                Cocok = KelayakanCPU(avb, .Koordinat, SolusiV, KPembantuV, False)
                            End If

                            If Cocok Then
                                EksekusiCPU(avb, .Koordinat, SolusiV, KPembantuV, False)
                                AktifCPU = False
                                TimePlayerSecondsTotal += TimePlayerSecondsElapsed
                                Exit For
                            End If
                        End With
                    Next
                    CPUVState = StatusCPU.Ready
                Else
                    CPUVState = StatusCPU.Ready
                End If
            End If

            If Not AktifCPU Then
                For Each pl In TilePlayer
                    pl.IsActive = True
                Next
            End If

            If AktifCPU AndAlso CPUHState = StatusCPU.Ready Then
                KH += 1
                If KH > KandidatSourceCPUAIH.Count - 1 Then
                    KH = 0
                    NonAktifCPUH = True
                End If
            End If

            If AktifCPU AndAlso CPUVState = StatusCPU.Ready Then
                KV += 1
                If KV > KandidatSourceCPUAIV.Count - 1 Then
                    KV = 0
                    NonAktifCPUV = True
                End If
            End If

            If AktifCPU Then
                TimePlayerSecondsElapsed += Waktu.ElapsedGameTime.TotalSeconds
            End If
        End If
    End Sub

    Friend Sub Unload() Implements IBaseScreen.Unload

    End Sub

#Region "Tempat Script Inisiasi"
    Private Sub InisiasiBoard()
        Dim PosisiAwal As New Vector2(50, 20)
        Dim ukuran = Math.Floor((GameSize.Height - (GameSize.Height * 12 / 100)) / TileBoard.GetLength(0))
        Dim rnd As New Random
        UkuranTile = New Sizes(ukuran, ukuran)
        For i = 0 To TileBoard.GetLength(0) - 1
            PosisiAwal.X = 50
            For j = 0 To TileBoard.GetLength(1) - 1
                TileBoard(i, j) = New TileBoard(PosisiAwal, New Sizes(ukuran, ukuran), Colour, True, 127)
                'If i = Math.Floor(TileBoard.GetLength(0) / 2) And j = Math.Floor(TileBoard.GetLength(1) / 2) Then
                If TsData.Data(i, j) <> 127 And TsData.Data(i, j) <> -1 Then
                    TileBoard(i, j).Value = TsData.Data(i, j)
                    TileBoard(i, j).IsBlocked = True
                    TileBoard(i, j).IsOpen = True
                ElseIf TsData.Data(i, j) = -1 Then
                    TileBoard(i, j).IsBlocked = True
                End If

                'End If
                PosisiAwal.X += ukuran + ukuran - Math.Ceiling(ukuran * 0.9)
            Next
            PosisiAwal.Y += ukuran
        Next
    End Sub

    Private Sub InisiasiKotakPlayer()
        Dim ukuran = Math.Floor(1.2 * TileBoard(0, 0).Size.Height)
        Dim PosisiAwal As New Vector2((TileBoard(0, TileBoard.GetLength(0) - 1).Position.X + TileBoard(0, TileBoard.GetLength(0) - 1).Size.Width - TileBoard(0, 0).Position.X) / 2 + TileBoard(0, 0).Position.X - ((ukuran + ukuran - Math.Ceiling(ukuran * 0.9)) * TilePlayer.Length / 2),
                                      TileBoard(TileBoard.GetLength(0) - 1, 0).Position.Y + TileBoard(TileBoard.GetLength(0) - 1, 0).Size.Height + 10)
        UkuranTile = New Sizes(ukuran, ukuran)
        For i = 0 To TilePlayer.GetLength(0) - 1
            TilePlayer(i) = New TileBoard(PosisiAwal, New Sizes(ukuran, ukuran), Colour, True, RandomNumber.Next(0, 127))
            PosisiAwal.X += ukuran + ukuran - Math.Ceiling(ukuran * 0.9)
        Next
    End Sub

    Private Sub InisiasiMenu()
        Dim PosisiAwal As New Vector2(TileBoard(0, TileBoard.GetLength(0) - 1).Position.X + 100, GameSize.Height * 0.55)
        Dim ukuran As New Sizes(2.454545 * TilePlayer(0).Size.Height, TilePlayer(0).Size.Height)
        For i = 0 To TileMenu.Length - 1
            TileMenu(i) = New TileMenu(PosisiAwal, ukuran, New Color(0, 0, 0), True, i)
            PosisiAwal.Y += ukuran.Height + 15
        Next
    End Sub

    Private Sub InisiasiTrash()
        Dim PosisiAwak As New Vector2(TileMenu(0).Position.X, GameSize.Height * 0.75)
        Dim Ukuran As New Sizes(TileMenu(0).Size.Width * 0.8, 1.05882353 * TileMenu(0).Size.Width * 0.8)
        TrashMenu = New TileCustom(PosisiAwak, Ukuran)
    End Sub
#End Region

#Region "Tempat Script Fungsi Update"
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

    Private Sub UpdateTileMenu()
        For Each Menu In TileMenu
            Menu.Update(ProsesMenuDiPilih)
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
        If ProsesMenuDiPilih = 0 Then
            For i = 0 To TileBoard.GetLength(0) - 1
                For j = 0 To TileBoard.GetLength(1) - 1
                    With TileBoard(i, j)
                        If .IsWrongPlace = BenarSalahNetral.Salah Then
                            For Each PlKotak In TilePlayer
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

                            For Each PlKotak In TilePlayer
                                If PlKotak.IsBlocked Then
                                    If .Value = PlKotak.Value Then
                                        PlKotak.IsBlocked = False
                                        PlKotak.Value = RandomNumber.Next(0, 126)
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
            ProsesMenuDiPilih = 3
            'Update Score Jika menu yang terpilih = "OK'
            If StatusCekBacaH Then
                Pemain(GiliranPlayer).Score += Bhs.Score(DaftarString(ListKoordinatBacaH))
            ElseIf StatusCekBacaV Then
                Pemain(GiliranPlayer).Score += Bhs.Score(DaftarString(ListKoordinatBacaV))
            End If
        ElseIf ProsesMenuDiPilih = 1 Then

            For Each Kotak In TilePlayer
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
            ProsesMenuDiPilih = 3
        End If
    End Sub

    Private Sub UpdateJikaMouseDiLepas()
        If ((MouseStateBefore.LeftButton = ButtonState.Released And MouseStateAfter.LeftButton = ButtonState.Released) Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete)))) And MouseIsHolding Then
            For Each Kotak In TileBoard
                Kotak.TrigerByHold = False
            Next

            For Each Kotak In TilePlayer
                Kotak.TrigerByHold = False
            Next

            'Kosongkan tile yang akan dipindahkan
            If (ReleaseOnTile And ProsesMenuDiPilih = 3) Or DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) Then
                'Untuk TileBoard
                If IndexTileBoardOfHolding <> New Vector2(-1, -1) Then
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke tempat baru
                    If ReleaseOnTile Then
                        TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value = 127
                    End If
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke recycle
                    If DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) Then
                        For Each Kotak In TilePlayer
                            If Kotak.Value = TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value And Kotak.IsBlocked = True Then
                                Kotak.IsBlocked = False
                                Kotak.Value = RandomNumber.Next(0, 127)
                                TileBoard(IndexTileBoardOfHolding.X, IndexTileBoardOfHolding.Y).Value = 127
                                Exit For
                            End If
                        Next
                    End If
                End If

                'Untuk TilePlayer
                If IndexTilePlayerOfHolding <> -1 Then
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke papan permainan
                    If ReleaseOnTile Then
                        TilePlayer(IndexTilePlayerOfHolding).IsBlocked = True
                    End If
                    'Jalankan ketika tile yang ditahan berhasil dipindahkan ke recycle
                    If DeleteOnTile Or ((KeyboardStateBefore.IsKeyDown(Keys.Delete) And KeyboardStateAfter.IsKeyUp(Keys.Delete))) Then
                        TilePlayer(IndexTilePlayerOfHolding).IsBlocked = False
                        TilePlayer(IndexTilePlayerOfHolding).Value = RandomNumber.Next(0, 127)
                    End If
                End If

                'Reset value kembali normal
                ReleaseOnTile = False
                DeleteOnTile = False

                'Reset value IsOpen pada semua TileBoard
                For Each Kotak In TileBoard
                    Kotak.AmountColorRO = 1
                    If Kotak.IsBlocked = False Then
                        Kotak.IsOpen = False
                    End If
                    Kotak.IsWrongPlace = BenarSalahNetral.Netral
                Next

                'Reset value FirstTime dan Baca
                FirstTimeAmbilYgDicekH = True
                StatusCekBacaH = False
                ListKoordinatBacaH.Clear()
                KH = -1
                KandidatSourceCPUAIH.Clear()

                FirstTimeAmbilYgDicekV = True
                StatusCekBacaV = False
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
        For i = 0 To TilePlayer.Length - 1
            If TilePlayer(i).IsActive Then
                TilePlayer(i).Update(MouseIsHolding, ValueOfHolding, ReleaseOnTile)
                If TilePlayer(i).TrigerByHold Then
                    IndexTilePlayerOfHolding = i
                End If
            End If
        Next
    End Sub

    Private Sub UpdateTileBoard(ByRef Sudut As Integer, ByRef Ring As Integer, ByRef Counter As Integer)
        Sudut = 0
        Ring = 0
        Counter = 0

        Do
            Dim koordinat As Vector2 = Spiral(Ring, Sudut, Counter)
            TileBoard(koordinat.X, koordinat.Y).Update(MouseIsHolding, ValueOfHolding, ReleaseOnTile)
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

    Private Function KelayakanCPU(avb As UInt16, Koordinat As Vector2, Solusi As String, KPembantu As List(Of Vector2), IsBaris As Boolean) As Boolean
        Dim Cocok As Boolean = False
        Dim n As Integer = 0
        Dim ckKoordinat As Integer
        Dim kx As Integer = 0
        Dim ky As Integer = 0
        Dim x0 As Integer = 0
        Dim y0 As Integer = 0

        Do
            If IsBaris Then
                ckKoordinat = Koordinat.Y
                kx = 0
                ky = n - avb
                y0 = 0
                y0 = 1
            Else
                ckKoordinat = Koordinat.X
                kx = n - avb
                ky = 0
                x0 = 1
                y0 = 0
            End If
            If (ckKoordinat + (n - avb) >= 0 And ckKoordinat + (n - avb) < TileBoard.GetLength(0)) Then
                If ((n = 0 And ckKoordinat + (n - avb) - 1 >= 0) AndAlso TileBoard(Koordinat.X + kx - x0, Koordinat.Y + ky - y0).Value <> 127) Or
                    ((n = Solusi.Length - 1 And ckKoordinat + (n - avb) + 1 < TileBoard.GetLength(0)) AndAlso TileBoard(Koordinat.X + kx + x0, Koordinat.Y + ky + y0).Value <> 127) Then
                    Exit Do
                End If

                If n >= Solusi.Length Then
                    Cocok = True
                    Exit Do
                End If

                If n = avb Then
                    n += 1
                    Continue Do
                End If

                For Each k In KPembantu
                    If n = avb + (If(IsBaris, k.Y, k.X) - ckKoordinat) And TileBoard(k.X, k.Y).CharValue = Solusi(n) And TileBoard(k.X, k.Y).IsBlocked Then
                        n += 1
                        Continue Do
                    End If
                Next

                For Each p In TilePlayer
                    If p.CharValue = Solusi(n) And TileBoard(Koordinat.X + kx, Koordinat.Y + ky).Value = 127 And
                        Not p.IsBlocked And Not TileBoard(Koordinat.X + kx, Koordinat.Y + ky).IsBlocked Then
                        n += 1
                        Continue Do
                    End If
                Next

                If n < Solusi.Length Then
                    Exit Do
                End If
            Else
                Exit Do
            End If
        Loop
        Return Cocok
    End Function

    Private Sub EksekusiCPU(avb As UInt16, Koordinat As Vector2, Solusi As String, KPembantu As List(Of Vector2), IsBaris As Boolean)
        Dim n As Integer = 0
        Dim ckKoordinat As Integer
        Dim kx As Integer = 0
        Dim ky As Integer = 0
        Dim x0 As Integer = 0
        Dim y0 As Integer = 0

        Do
            If IsBaris Then
                ckKoordinat = Koordinat.Y
                kx = 0
                ky = n - avb
                y0 = 0
                y0 = 1
            Else
                ckKoordinat = Koordinat.X
                kx = n - avb
                ky = 0
                x0 = 1
                y0 = 0
            End If
            If (ckKoordinat + (n - avb) >= 0 And ckKoordinat + (n - avb) < TileBoard.GetLength(0)) Then
                If n >= Solusi.Length Then
                    Exit Do
                End If

                If n = avb Then
                    n += 1
                    Continue Do
                End If

                For Each k In KPembantu
                    If n = avb + (If(IsBaris, k.Y, k.X) - ckKoordinat) And TileBoard(k.X, k.Y).CharValue = Solusi(n) And TileBoard(k.X, k.Y).IsBlocked Then
                        n += 1
                        Continue Do
                    End If
                Next

                For Each p In TilePlayer
                    If p.CharValue = Solusi(n) And TileBoard(Koordinat.X + kx, Koordinat.Y + ky).Value = 127 And
                        Not p.IsBlocked And Not TileBoard(Koordinat.X + kx, Koordinat.Y + ky).IsBlocked Then
                        n += 1
                        p.IsBlocked = True
                        TileBoard(Koordinat.X + kx, Koordinat.Y + ky).Value = p.Value
                        Continue Do
                    End If
                Next

                If n < Solusi.Length Then
                    Exit Do
                End If
            Else
                Exit Do
            End If
        Loop
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
