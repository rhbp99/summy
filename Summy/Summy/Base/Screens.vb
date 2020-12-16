Public MustInherit Class Screens
    Public MustOverride Property Name As String
    Public MustOverride Property Enable As Boolean
    Public MustOverride Property Visible As Boolean
    Public MustOverride Sub Initialize()
    Public MustOverride Sub Load()
    Public MustOverride Sub Update(ByRef MenuSelected As MenuUtamaScreen.MenuUtama)
    Public MustOverride Sub Draw()
End Class
