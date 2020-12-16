Imports System.Text

''' <summary>
''' A game component that counts FPS and UPS, also gives other useful performance information.
''' </summary>
Public Class FpsCounter
    Inherits DrawableGameComponent
#Region "Private Fields"
    Private Const refreshesPerSec As Integer = 4
    ' how many times do we calculate FPS & UPS every second
    Private ReadOnly RefreshTime As TimeSpan = TimeSpan.FromMilliseconds(1000 / refreshesPerSec)
    Private elapsedTime As TimeSpan = TimeSpan.Zero
    Private Shared m_fps As Integer = 0, m_ups As Integer = 0
    Private frameCounter As Integer = 0, updateCounter As Integer = 0
    Private spriteBatch As SpriteBatch
    Private font As SpriteFont
    Private position As Vector2
    Private Shared process As Process = Process.GetCurrentProcess()
    Private Shared messages As New List(Of String)()
    Private Shared messageTimers As New List(Of Double)()
    Private outputSb As New StringBuilder()
#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Gets the current FPS.
    ''' </summary>
    Public Shared ReadOnly Property FPS() As Integer
        Get
            Return m_fps
        End Get
    End Property

    ''' <summary>
    ''' Gets the current UPS.
    ''' </summary>
    Public Shared ReadOnly Property UPS() As Integer
        Get
            Return m_ups
        End Get
    End Property

    ''' <summary>
    ''' Gets the total allocated memory, in bytes.
    ''' </summary>
    Public Shared ReadOnly Property MemAllocated() As Long
        Get
            Return process.PrivateMemorySize64
        End Get
    End Property
#End Region

    Public Sub New(game As Game, font As SpriteFont, pos As Vector2)
        MyBase.New(game)
        spriteBatch = New SpriteBatch(game.GraphicsDevice)
        Me.font = font
        position = pos
    End Sub

    ''' <summary>
    ''' Allows the game component to perform any initialization it needs to before starting
    ''' to run.  This is where it can query for any required services and load content.
    ''' </summary>
    Public Overrides Sub Initialize()
        MyBase.Initialize()
    End Sub

#Region "Public Methods"

    ''' <summary>
    ''' Displays a message on screen for debugging purposes.
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="milliseconds"></param>
    Public Shared Sub ShowMessage(msg As String, milliseconds As Integer)
        messages.Add(msg)
        messageTimers.Add(milliseconds)
    End Sub

#End Region

#Region "Update and Draw"

    ''' <summary>
    ''' Allows performace monitor to calculate update rate.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Public Overrides Sub Update(gameTime As GameTime)
        elapsedTime += gameTime.ElapsedGameTime

        updateCounter += 1

        If elapsedTime > RefreshTime Then
            elapsedTime -= RefreshTime
            m_fps = frameCounter * refreshesPerSec
            m_ups = updateCounter * refreshesPerSec
            frameCounter = 0
            updateCounter = 0
        End If

        ' Update message timers
        For i As Integer = 0 To messageTimers.Count - 1
            messageTimers(i) -= gameTime.ElapsedGameTime.TotalMilliseconds
            If messageTimers(i) <= 0 Then
                messages.RemoveAt(i)
                ' remove timed out messages
                messageTimers.RemoveAt(i)
            End If
        Next

        outputSb.Clear()
        outputSb.Append(m_fps.ToString() + " ")
        outputSb.Append("(" + (MemAllocated / 1024 / 1024).ToString() + " MB)" + Environment.NewLine)
        For Each msg As String In messages
            outputSb.Append(msg + Environment.NewLine)
        Next

        MyBase.Update(gameTime)
    End Sub

    ''' <summary>
    ''' Allows performance monitor to calculate draw rate.
    ''' </summary>
    ''' <param name="gameTime"></param>
    Public Overrides Sub Draw(gameTime As GameTime)
        frameCounter += 1
        ' increment frame counter
        spriteBatch.Begin()
        spriteBatch.DrawString(font, $"FPS: {outputSb.ToString()}", position + New Vector2(1, 1), New Color(244, 143, 177), 0, Nothing, 0.15F, SpriteEffects.None, 0)
        ' shadow
        'spriteBatch.DrawString(font, outputSb.ToString(), position, New Color(97, 97, 97))
        spriteBatch.[End]()

        MyBase.Draw(gameTime)
    End Sub

#End Region
End Class