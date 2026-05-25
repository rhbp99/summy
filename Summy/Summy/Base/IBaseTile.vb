''' <summary>
''' Interface for tile objects used in the player tile array.
''' Implemented by TileBoard to allow polymorphic usage in ScreenGameplay.
''' </summary>
Friend Interface IBaseTile
    Property IsActive As Boolean
    Property IsBlocked As Boolean
    Property Value As UShort
    Property TrigerByOnTop As Boolean
    Property TrigerByHold As Boolean
    Property Size As Sizes
    Property IsCheckedH As Boolean
    Property IsCheckedV As Boolean
    Property IsWrongPlace As BenarSalahNetral
    Property AmountColorRO As Single
    ReadOnly Property CharValue As Char
    Sub Draw(BaseTexture As Texture2D, ContentTexture As Texture2D)
    Sub Update(ByRef MouseIsHolding As Boolean, ByRef ValueOfHolding As UShort, ByRef ReleaseOnTile As Boolean)
End Interface
