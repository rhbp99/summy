Friend Class Bahasa
    Friend Function ConvertValue(ByVal Value As UShort) As Char
        Select Case Value
            Case 0 To 7
                Return "0"c
            Case 8 To 15
                Return "1"c
            Case 16 To 23
                Return "2"c
            Case 24 To 31
                Return "3"c
            Case 32 To 39
                Return "4"c
            Case 40 To 47
                Return "5"c
            Case 48 To 55
                Return "6"c
            Case 56 To 63
                Return "7"c
            Case 64 To 71
                Return "8"c
            Case 72 To 79
                Return "9"c
            Case 80 To 86
                Return "+"
            Case 87 To 93
                Return "-"c
            Case 94 To 100
                Return "x"c
            Case 101 To 107
                Return ":"c
            Case 108 To 126
                Return "="
            Case Else
                Return " "c
        End Select
    End Function

    Friend Function CekBahasa(ByVal Teks As String) As Boolean
        Teks = Teks.Replace(" ", "")
        Dim ArrChar() As Char = Teks.ToCharArray
        Dim Temp() As String = DivideAndGrouping(ArrChar, False)
        If CekSintaks(Temp) Then
            If IsPerhitunganValid(Temp) Then
                Return True
            End If
        End If
        Return False
    End Function

    Friend Function Score(ByVal Teks As String) As Integer
        Dim P() As Char = Teks.ToCharArray
        Dim total As Integer = 0
        For Each nilai In P
            If IsNumeric(nilai) Then
                total += CInt(nilai.ToString)
            End If
        Next
        Return total
    End Function


#Region "Privat untuk Fungsi CekBahasa"

    'Fungsi untuk membagi-bagi dan mengolompokkan string yang dipisahkan oleh operator
    Friend Function DivideAndGrouping(ByVal TempArr() As Char, FungsiKhusus As Boolean) As String()
        Dim i = 0
        Dim j = 0
        Dim Arr(0) As String
        Dim temp As String = ""
        Dim StillNumber As Boolean = False
        Dim StillAritmatic As Boolean = False
        Dim StillAssignment As Boolean = False
        Dim StillSpace As Boolean = False
        Do While i < TempArr.Count
            If j > Arr.Length - 1 Then
                ReDim Preserve Arr(j)
            End If
            If IsNumeric(TempArr(i)) Then
                StillNumber = True
                temp = ""
                While StillNumber = True
                    temp += TempArr(i)
                    i += 1
                    If i >= TempArr.Length Then
                        StillNumber = False
                    Else
                        If IsNumeric(TempArr(i)) = False Then
                            StillNumber = False
                        End If
                    End If
                End While
                Arr(j) = temp
                j += 1
            ElseIf IsAritmathic(TempArr(i)) Then
                If FungsiKhusus Then
                    temp = ""
                    StillAritmatic = True
                    While StillAritmatic = True
                        temp += TempArr(i)
                        i += 1
                        If i >= TempArr.Length Then
                            StillAritmatic = False
                        Else
                            If IsAritmathic(TempArr(i)) = False Then
                                StillAritmatic = False
                            End If
                        End If
                    End While
                    Arr(j) = temp
                    j += 1
                Else
                    Arr(j) = TempArr(i)
                    i += 1
                    j += 1
                End If
            ElseIf IsAssigment(TempArr(i)) Then
                If FungsiKhusus Then
                    temp = ""
                    StillAssignment = True
                    While StillAssignment = True
                        temp += TempArr(i)
                        i += 1
                        If i >= TempArr.Length Then
                            StillAssignment = False
                        Else
                            If IsAssigment(TempArr(i)) = False Then
                                StillAssignment = False
                            End If
                        End If
                    End While
                    Arr(j) = temp
                    j += 1
                Else
                    Arr(j) = TempArr(i)
                    i += 1
                    j += 1
                End If
            ElseIf TempArr(i) = " "c
                If FungsiKhusus Then
                    temp = ""
                    StillSpace = True
                    While StillSpace = True
                        temp += TempArr(i)
                        i += 1
                        If i >= TempArr.Length Then
                            StillSpace = False
                        Else
                            If Not TempArr(i) = " "c Then
                                StillSpace = False
                            End If
                        End If
                    End While
                    Arr(j) = temp
                    j += 1
                Else
                    Arr(j) = TempArr(i)
                    i += 1
                    j += 1
                End If
            End If
        Loop
        Return Arr
    End Function

    'Cek apakah string masuk memiliki aturan sintaksis yang benar
    Private Function CekSintaks(ByVal Indeks() As String) As Boolean
        Dim Benar As Boolean = True
        Dim i = 0
        While i < Indeks.Length
            If i Mod 2 = 0 Then
                If IsNumeric(Indeks(i)) Then
                    If Indeks(i).First() = "0" And Indeks(i).Length > 1 Then
                        Benar = Benar And False
                    Else
                        Benar = Benar And True
                    End If
                Else
                    Benar = Benar And False
                End If
            Else
                If i = Indeks.Length - 2 Then
                    If IsAssigment(Indeks(i)) Then
                        Benar = Benar And True
                    Else
                        Benar = Benar And False
                    End If
                Else
                    If IsAritmathic(Indeks(i)) Then
                        Benar = Benar And True
                    Else
                        Benar = Benar And False
                    End If
                End If
            End If
            i += 1
            If Benar = False Then
                Exit While
            End If
        End While
        Return Benar
    End Function

    'Cek apakah string yang masuk merupakan perhitungan yang benar
    Private Function IsPerhitunganValid(ByVal Index() As String) As Boolean
        Dim Indeks As String() = Index
        Dim Prioritas(Indeks.Length - 1) As Integer
        Dim t As String
        Dim p As String

        Dim temp As Double = 0
        For i = 0 To Prioritas.Length - 1
            If Indeks(i) = "+" Or Indeks(i) = "-" Then
                Prioritas(i) = 1
            ElseIf Indeks(i) = "x" Or Indeks(i) = ":" Then
                Prioritas(i) = 2
            Else
                Prioritas(i) = 0
            End If
        Next
        Dim N As Integer = 0
        While Indeks.Length > 3
            p = ""
            t = ""
            For i = 0 To Indeks.Length - 1
                t &= Indeks(i)
                p &= Prioritas(i)
            Next

            Dim Priority2 As Boolean = IsThereAreCrossOrDivide(Prioritas)

            For i = 0 To Indeks.Length - 1
                If Priority2 = True And Prioritas(i) = 2 Then
                    If Indeks(i) = "+" Then
                        temp = CDbl(Indeks(i - 1)) + CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = "-" Then
                        temp = CDbl(Indeks(i - 1)) - CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = "x" Then
                        temp = CDbl(Indeks(i - 1)) * CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = ":" Then
                        temp = CDbl(Indeks(i - 1)) / CDbl(Indeks(i + 1))
                    End If

                    Indeks(i - 1) = CStr(temp)
                    For j = i To Indeks.Length - 1 - 2
                        Indeks(j) = Indeks(j + 2)
                        Prioritas(j) = Prioritas(j + 2)
                    Next
                    ReDim Preserve Indeks(Indeks.Length - 3)
                    ReDim Preserve Prioritas(Prioritas.Length - 3)
                    Exit For

                ElseIf Priority2 = False And Prioritas(i) = 1 Then
                    If Indeks(i) = "+" Then
                        temp = CDbl(Indeks(i - 1)) + CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = "-" Then
                        temp = CDbl(Indeks(i - 1)) - CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = "x" Then
                        temp = CDbl(Indeks(i - 1)) * CDbl(Indeks(i + 1))
                    ElseIf Indeks(i) = ":" Then
                        temp = CDbl(Indeks(i - 1)) / CDbl(Indeks(i + 1))
                    End If

                    Indeks(i - 1) = CStr(temp)
                    For j = i To Indeks.Length - 1 - 2
                        Indeks(j) = Indeks(j + 2)
                        Prioritas(j) = Prioritas(j + 2)
                    Next
                    ReDim Preserve Indeks(Indeks.Length - 3)
                    ReDim Preserve Prioritas(Prioritas.Length - 3)
                    Exit For
                End If

            Next
        End While

        If Indeks.Count > 2 AndAlso Indeks(0) = Indeks(2) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Cek apakah terdapat operator aritmatika (:, x, +, atau -) berdasarkan valuenya
    Private Function IsAritmathic(ByVal Value As Char) As Boolean
        Select Case Value
            Case ":"
                Return True
            Case "x"
                Return True
            Case "+"
                Return True
            Case "-"
                Return True
        End Select
        Return False
    End Function

    'Cek apakah terdapat = berdasarkan valuenya
    Private Function IsAssigment(ByVal Value As Char) As Boolean
        If Value = "=" Then
            Return True
        End If
        Return False
    End Function

    'Cek apakah terdapat x atau : berdasarkan nilai prioritas
    Private Function IsThereAreCrossOrDivide(ByVal Prioritas() As Integer) As Boolean
        For i = 0 To Prioritas.Length - 1
            If Prioritas(i) = 2 Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

End Class
