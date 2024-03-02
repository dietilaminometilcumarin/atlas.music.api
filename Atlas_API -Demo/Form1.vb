
'Demo application for demonstrate using of Atlas_API component
'Copyright 2024, Detach.pp.ua (atlas.callback@gmail.com)

'-------------------------------------------------------
'MAIN API 

'PlayList.................structure, describes a description of the playlist type
'ListMusicData............structure, describes the attributes of a music track


'ConnectToServer..........remote host connection function
'GetPlayLists.............playlist list function (return type - PlayList)
'GetTracksByPlayListID....function for obtaining a list of tracks for a specified playlist (return type - ListMusicData)
'Player_EndTrack..........current playback end event
'-------------------------------------------------------


Imports Atlas_API

Public Class Form1

    'THE COMPONENT CONTAINS SOME EVENTS SO LET'S CONNECT USING - WithEvents
    Public WithEvents TCPC As Atlas_Music_API = Atlas_Music_API.Instance

    'INITIALIZING CONNECTION TO THE SERVER
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If TCPC.ConnectToServer("TOKEN") Then
            Beep()
        Else
            MsgBox("Error Connection")
            End
        End If
    End Sub

    'STOPPING CURRENT PLAYBACK
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TCPC.StopSong()
    End Sub

    'PLAYING A TRACK BY CURRENT ID
    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        If ListView1.SelectedItems.Count > 0 Then
            TCPC.PlaySong(ListView1.SelectedItems(0).Text)
        End If
    End Sub

    'EVENT NOTIFYING THE COMPLETION OF PLAYING THE SELECTED TRACK
    Private Sub TCPC_Player_EndTrack() Handles TCPC.Player_EndTrack
        If ListView1.SelectedItems.Count > 0 And ListView1.SelectedItems(0).Index < ListView1.Items.Count - 1 Then
            ListView1.Items(ListView1.SelectedItems(0).Index + 1).Selected = True
            TCPC.PlaySong(ListView1.SelectedItems(0).Text)
        End If
    End Sub

    'RETURN A SET OF PLAYLISTS STORED ON A REMOTE SERVICE
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListView2.Items.Clear()
        Dim pl As List(Of Atlas_Music_API.PlayList) = TCPC.GetPlayLists()

        For Each item As Atlas_Music_API.PlayList In pl
            Dim lItem As ListViewItem = ListView2.Items.Add(item.ListID)
            lItem.SubItems.Add(item.ListName)
        Next
    End Sub

    'RETURN A LIST OF TRACKS AT THE SPECIFIED PLAYLIST ID
    Private Sub ListView2_DoubleClick(sender As Object, e As EventArgs) Handles ListView2.DoubleClick
        ListView1.Items.Clear()
        Dim playList As List(Of Atlas_Music_API.ListMusicData) = TCPC.GetTracksByPlayListID(ListView2.SelectedItems(0).Text)
        ListView1.Items.Clear()
        For Each item As Atlas_Music_API.ListMusicData In playList
            Dim lItem As ListViewItem = ListView1.Items.Add(item.ID)
            lItem.SubItems.Add(item.TrackName)
            lItem.SubItems.Add(item.ArtistName)
            lItem.SubItems.Add(item.TrackGenre)
            lItem.SubItems.Add(item.TrackDuration)
        Next
    End Sub


End Class
