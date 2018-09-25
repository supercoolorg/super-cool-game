﻿using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetCode;

public class Matchmaker : MonoBehaviour {
    TcpClient client;
    NetworkStream stream;

    // Use this for initialization
    async void Start() {
        Connect();
        int port = await GetLobby();
        client.Close();
        var lobby = gameObject.GetComponent<Lobby>();
        lobby.Connect(port);
    }

    private void Connect(){
        client = new TcpClient();
        client.Connect(NetHelpers.SERVER_ADDR, NetHelpers.MATCHMAKER_PORT);
        stream = client.GetStream();
    }

    private async Task<int> GetLobby() {
        byte[] buffOut = NetHelpers.BufferOp(OpCode.Queue, 4);
        await stream.WriteAsync(buffOut, 0, buffOut.Length);

        byte[] buffIn = new byte[4];
        await stream.ReadAsync(buffIn, 0, buffIn.Length);

        byte op = buffIn[0];
        if(op == (byte)OpCode.FoundMatch) {
            UInt16 port = BitConverter.ToUInt16(buffIn, 1);
            return (int)port;
        }
        return -1;
    }
}
