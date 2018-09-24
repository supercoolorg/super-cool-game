using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetCode;

public class Matchmaker : MonoBehaviour {
    TcpClient client;

    // Use this for initialization
    async void Start () {
        Debug.Log("Ask for lobby");
        int port = await GetLobby();
        Debug.Log("Got lobby on port " + port);
        Join(port);
	}

    private async Task<int> GetLobby() {
        client = new TcpClient();
        client.Connect(NetHelpers.SERVER_ADDR, NetHelpers.MATCHMAKER_PORT);
        var stream = client.GetStream();

        byte[] message = NetHelpers.BufferOp(OpCode.Queue);
        await stream.WriteAsync(message, 0, NetHelpers.MESSAGE_SIZE);

        byte[] buffer = new byte[NetHelpers.MESSAGE_SIZE];
        await stream.ReadAsync(buffer, 0, NetHelpers.MESSAGE_SIZE);

        byte op = buffer[0];
        if(op == (byte)OpCode.FoundMatch) {
            UInt16 port = NetHelpers.ReadUInt16(buffer, 1);
            return port;
        }
        return -1;
    }

    private void Join(int port) {
        UdpClient socket = new UdpClient();
        socket.Connect(NetHelpers.SERVER_ADDR, port);

        byte[] buffer = NetHelpers.BufferOp(OpCode.Spawn);
        socket.Send(buffer, NetHelpers.MESSAGE_SIZE);
    }
}
