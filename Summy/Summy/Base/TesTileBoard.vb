''' <summary>
''' Provides initial board data for ScreenGameplay.
''' Data(i, j) values:
'''   127  = empty, unblocked tile
'''   -1   = blocked empty tile (wall)
'''   0-126 = blocked tile with that value pre-placed
''' </summary>
Friend Class TesTileBoard
    Friend ReadOnly Data(14, 14) As Integer

    Friend Sub New()
        ' Default: all tiles are empty (127 = no pre-placed value, unblocked)
        For i = 0 To 14
            For j = 0 To 14
                Data(i, j) = 127
            Next
        Next
    End Sub
End Class
