﻿Imports System.Net.Sockets
Imports System.Net

Public Class ServerSocket
    Public S As Socket

    Sub Start(ByVal Port As Integer)
        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)
        S.ReceiveBufferSize = 1024 * 100
        S.SendBufferSize = 1024 * 100
        S.ReceiveTimeout = -1
        S.SendTimeout = -1
        S.Bind(IpEndPoint)
        S.Listen(999)

        Dim T As New Threading.Thread(AddressOf BegingAccpet) : T.Start()
    End Sub

    Sub BegingAccpet()
        While True
            S.BeginAccept(New AsyncCallback(AddressOf EndAccept), S)
            Threading.Thread.Sleep(1)
        End While
    End Sub

    Sub EndAccept(ByVal ar As IAsyncResult)
        Try
            Dim C As New Client(S.EndAccept(ar))
            C.C.BeginReceive(C.Buffer, 0, C.Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf C.BeginReceive), C)
        Catch ex As Exception
        End Try
    End Sub


End Class