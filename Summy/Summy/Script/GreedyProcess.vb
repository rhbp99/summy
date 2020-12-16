Imports System.Text
Imports System.Threading.Tasks

Class GreedyProcess
    Private Function PermutasiDikurangi(Data() As String, DaftarYangDihapus As String) As String()
        Dim DataKandidat = Data.ToList
        Dim Dftr = DaftarYangDihapus.Split("/"c)
        For Each dft In Dftr
            If dft <> "" Then
                DataKandidat.Remove(dft)
            End If
        Next
        Return DataKandidat.ToArray
    End Function

    Structure JenisList
        Friend ListNumerik() As String
        Friend ListOperator() As String
    End Structure

    Private Function PisahNumerik(Data() As String) As JenisList
        Dim T As List(Of String) = Data.ToList
        Dim D() As String = T.ToArray
        Dim TipeList As New JenisList
        Dim i = 0
        Dim j = 0
        For Each S In D
            If Integer.TryParse(S, 0) Then
                ReDim Preserve TipeList.ListNumerik(i)
                TipeList.ListNumerik(i) = S
                i += 1
            Else
                ReDim Preserve TipeList.ListOperator(j)
                TipeList.ListOperator(j) = S
                j += 1
            End If
        Next
        Return TipeList
    End Function

    Friend Sub SusunKandidatParalelDanSetUpHasil(Data() As String, KandidatSource As GamePlay.KandidatCPUBaseKoordinat, Kpbntu As List(Of Vector2), IsHorizontal As Boolean, TileYangDipegang As TileBoard(), TileBoard As TileBoard(,), loopState As ParallelLoopState)
        Dim TempData = Data
        If AktifCPU AndAlso Not CPUPlaceSet AndAlso CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) < BatasWaktuTunggu Then
            Array.Sort(TempData)
            Array.Reverse(TempData)
            If Not TempData Is Nothing AndAlso TempData.Contains("=") And TempData.Count > 0 Then
                'SyncLock kunci
                SusunKandidatDanSetUpHasilnya(TempData, KandidatSource, Kpbntu, IsHorizontal, TileYangDipegang, TileBoard, loopState)
                'End SyncLock
            End If
        End If
    End Sub

    Private Sub SusunKandidatDanSetUpHasilnya(Data() As String, KandidatSource As GamePlay.KandidatCPUBaseKoordinat, Kpbntu As List(Of Vector2), IsHorizontal As Boolean, TileYangDipegang As TileBoard(), TileBoard As TileBoard(,), loopState As ParallelLoopState)
        If Not AktifCPU Then
            Exit Sub
        End If
        Dim TipeList As JenisList = PisahNumerik(Data)
        For Each t In TipeList.ListOperator
            If t.Count >= 2 Then
                Exit Sub
            End If
        Next
        'Dim OPTerpakai As New StringBuilder
        Dim A As New List(Of String)
        SyncLock Kunci1
            A = Permutasi(2, TipeList.ListNumerik)
        End SyncLock
        For i = 0 To A.Count - 1
            If Not AktifCPU Or CPUPlaceSet Or CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) >= BatasWaktuTunggu Then
                Exit For
            End If
            Dim BExtract = PermutasiDikurangi(TipeList.ListNumerik, A(i))
            If BExtract.Count > 0 Then
                Dim B As New List(Of String)
                SyncLock Kunci2
                    B = Permutasi(2, BExtract)
                End SyncLock
                If B.Count > 0 AndAlso B.First <> "" Then
                    For j = 0 To B.Count - 1
                        If Not AktifCPU Or CPUPlaceSet Or CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) >= BatasWaktuTunggu Then
                            Exit For
                        End If
                        Dim CExtract = PermutasiDikurangi(TipeList.ListNumerik, A(i) & B(j))
                        If CExtract.Count > 0 Then
                            Dim C As New List(Of String)
                            SyncLock Kunci3
                                C = Permutasi(2, CExtract)
                            End SyncLock
                            If C.Count > 0 AndAlso C.First <> "" Then
                                For k = 0 To C.Count - 1
                                    If Not AktifCPU Or CPUPlaceSet Or CInt(TimeSpan.FromSeconds(WaktuTungguBerjalan).ToString("mm")) >= BatasWaktuTunggu Then
                                        Exit For
                                    End If
                                    For Each OP In TipeList.ListOperator
                                        'If Not OPTerpakai.ToString.Contains(OP) Then
                                        Select Case OP
                                            Case "+"
                                                If CDbl(A(i).Replace("/", "")) + CDbl(B(j).Replace("/", "")) = CDbl(C(k).Replace("/", "")) Then
                                                    'OPTerpakai.Append(OP)
                                                    Dim tempstr As StringBuilder = New StringBuilder
                                                    tempstr.Append(A(i).Replace("/", ""))
                                                    tempstr.Append("+")
                                                    tempstr.Append(B(j).Replace("/", ""))
                                                    tempstr.Append("=")
                                                    tempstr.Append(C(k).Replace("/", ""))
                                                    Dim Arr() As Char = tempstr.ToString.ToCharArray
                                                    Dim TN As Integer = 0
                                                    For Each Ar In Arr
                                                        If Integer.TryParse(Ar, 0) Then
                                                            TN += Val(Ar)
                                                        End If
                                                    Next
                                                    CekKePapanPermainan(tempstr.ToString, KandidatSource, Kpbntu, IsHorizontal, TileYangDipegang, TileBoard, loopState)
                                                End If
                                            Case "-"
                                                If CDbl(A(i).Replace("/", "")) - CDbl(B(j).Replace("/", "")) = CDbl(C(k).Replace("/", "")) Then
                                                    'OPTerpakai.Append(OP)
                                                    Dim tempstr As StringBuilder = New StringBuilder
                                                    tempstr.Append(A(i).Replace("/", ""))
                                                    tempstr.Append("-")
                                                    tempstr.Append(B(j).Replace("/", ""))
                                                    tempstr.Append("=")
                                                    tempstr.Append(C(k).Replace("/", ""))
                                                    Dim Arr() As Char = tempstr.ToString.ToCharArray
                                                    Dim TN As Integer = 0
                                                    For Each Ar In Arr
                                                        If Integer.TryParse(Ar, 0) Then
                                                            TN += Val(Ar)
                                                        End If
                                                    Next
                                                    CekKePapanPermainan(tempstr.ToString, KandidatSource, Kpbntu, IsHorizontal, TileYangDipegang, TileBoard, loopState)
                                                End If
                                            Case "x"
                                                If CDbl(A(i).Replace("/", "")) * CDbl(B(j).Replace("/", "")) = CDbl(C(k).Replace("/", "")) Then
                                                    'OPTerpakai.Append(OP)
                                                    Dim tempstr As StringBuilder = New StringBuilder
                                                    tempstr.Append(A(i).Replace("/", ""))
                                                    tempstr.Append("x")
                                                    tempstr.Append(B(j).Replace("/", ""))
                                                    tempstr.Append("=")
                                                    tempstr.Append(C(k).Replace("/", ""))
                                                    Dim Arr() As Char = tempstr.ToString.ToCharArray
                                                    Dim TN As Integer = 0
                                                    For Each Ar In Arr
                                                        If Integer.TryParse(Ar, 0) Then
                                                            TN += Val(Ar)
                                                        End If
                                                    Next
                                                    CekKePapanPermainan(tempstr.ToString, KandidatSource, Kpbntu, IsHorizontal, TileYangDipegang, TileBoard, loopState)
                                                End If
                                            Case ":"
                                                If CDbl(A(i).Replace("/", "")) / CDbl(B(j).Replace("/", "")) = CDbl(C(k).Replace("/", "")) Then
                                                    'OPTerpakai.Append(OP)
                                                    Dim tempstr As StringBuilder = New StringBuilder
                                                    tempstr.Append(A(i).Replace("/", ""))
                                                    tempstr.Append(":")
                                                    tempstr.Append(B(j).Replace("/", ""))
                                                    tempstr.Append("=")
                                                    tempstr.Append(C(k).Replace("/", ""))
                                                    Dim Arr() As Char = tempstr.ToString.ToCharArray
                                                    Dim TN As Integer = 0
                                                    For Each Ar In Arr
                                                        If Integer.TryParse(Ar, 0) Then
                                                            TN += Val(Ar)
                                                        End If
                                                    Next
                                                    CekKePapanPermainan(tempstr.ToString, KandidatSource, Kpbntu, IsHorizontal, TileYangDipegang, TileBoard, loopState)
                                                End If
                                        End Select
                                        'End If
                                    Next
                                Next
                            End If
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Friend Function Permutasi(NPermutasi As Integer, Data() As String) As List(Of String)
        Dim DPermutasi As New List(Of String)
        Dim Layak As Boolean
        If Not Data Is Nothing AndAlso Data.Count > 0 Then
            For n = NPermutasi To 0 Step -1
                Dim Banyak(n) As Integer
                Dim FPermutasi As Boolean = True
                Do
                    SusunPermutasi(FPermutasi, Banyak, DPermutasi, Data, Layak)
                    If FPermutasi = False Then
                        Exit Do
                    End If
                Loop
            Next
        End If
        Return DPermutasi
    End Function

    Private Sub SusunPermutasi(ByRef FPermutasi As Boolean, ByRef Banyak() As Integer, ByRef DPermutasi As List(Of String), ByVal Data() As String, ByVal Layak As Boolean)
        If FPermutasi Then
            Layak = True
            If Banyak.Count > 1 Then
                For i = 0 To Banyak.Count - 1
                    If i < Banyak.Count - 1 Then
                        If (Banyak(i)) = (Banyak(i + 1)) Then
                            Layak = False
                        End If
                    Else
                        If (Banyak(i)) = (Banyak(0)) Then
                            Layak = False
                        End If
                    End If

                    If Layak = False Then
                        Exit For
                    End If
                Next
            End If
            Dim TemP As New StringBuilder
            If Layak = True Then
                For d = Banyak.Count - 1 To 0 Step -1
                    TemP.Append(Data(Banyak(d)))
                    TemP.Append("/")
                Next
                If TemP.ToString = "0/" Then
                    Dim asda = TemP
                End If
                If TemP.ToString.Length <= 2 Or (TemP.ToString.Length > 1 AndAlso TemP.ToString.Substring(0, 1) <> "0") Then
                    DPermutasi.Add(TemP.ToString)
                End If
            End If

            For i = 0 To Banyak.Count - 1
                If i > 0 Then
                    If Banyak(i - 1) > Data.Count - 1 Then
                        Banyak(i) += 1
                        Banyak(i - 1) = 0
                    End If
                Else
                    Banyak(i) += 1
                End If
                If Banyak(Banyak.Count - 1) > Data.Count - 1 Then
                    FPermutasi = False
                End If
            Next
        End If
    End Sub

    Private Sub CekKePapanPermainan(Solusi As String, KandidatSource As GamePlay.KandidatCPUBaseKoordinat, KPbntu As List(Of Vector2), IsHorizontal As Boolean, TileYangDipegang As TileBoard(), TileBoard As TileBoard(,), loopState As ParallelLoopState)
        Dim avb = AvaibleFormula(Solusi, KandidatSource, KPbntu, TileBoard)
        Dim Cocok As Boolean = False
        If avb >= 0 Then
            Cocok = KelayakanCPU(avb, KandidatSource.Koordinat, Solusi, KPbntu, IsHorizontal, TileYangDipegang, TileBoard)
        End If
        If Cocok Then
            SyncLock Kunci
                If AktifCPU And Not CPUPlaceSet Then
                    If Not loopState.ShouldExitCurrentIteration Then
                        CPU_LetakHasilTervalidasi(avb, KandidatSource.Koordinat, Solusi, IsHorizontal, TileYangDipegang, TileBoard)
                        loopState.Stop()
                        CPUPlaceSet = True
                    End If
                End If
            End SyncLock
        End If
    End Sub

    Private Function AvaibleFormula(HasilSolusi As String, KandidatSource As GamePlay.KandidatCPUBaseKoordinat, KPembantu As List(Of Vector2), TileBoard As TileBoard(,)) As Integer
        Dim HslTemp = HasilSolusi.ToList
        Dim indeks = -1

        For Each c In HasilSolusi.ToList
            If c = TileBoard(KandidatSource.Koordinat.X, KandidatSource.Koordinat.Y).CharValue Then
                indeks = HslTemp.IndexOf(c)
                'cTemp.Remove(c)
                Exit For
            End If
        Next
        Return indeks
    End Function

    Private Function KelayakanCPU(avb As UInt16, Koordinat As Vector2, Solusi As String, KPembantu As List(Of Vector2), IsBaris As Boolean, Player As TileBoard(), TileBoard As TileBoard(,)) As Boolean
        Dim Cocok As Boolean = False
        Dim Kpem = KPembantu
        Dim Sol As List(Of Char) = Solusi.ToList
        Dim Plyr = Player.ToList
        If AktifCPU Then
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

    Private Sub CPU_LetakHasilTervalidasi(avb As UInt16, Koordinat As Vector2, Solusi As String, IsHorizontal As Boolean, TileYangDipegang As TileBoard(), TileBoard As TileBoard(,))
        Dim Plye = TileYangDipegang.ToArray
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
                            TileYangDipegang(Array.IndexOf(Plye, p)).IsBlocked = True
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
                            TileYangDipegang(Array.IndexOf(Plye, p)).IsBlocked = True
                            TileBoard(Koordinat.X + If(Not IsHorizontal, i - avb, 0), Koordinat.Y + If(IsHorizontal, i - avb, 0)).Value = p.Value
                            Exit For
                        End If
                    End If
                Next
            Next
        End If

    End Sub
End Class