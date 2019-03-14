using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SuperCoolNetwork;
using Commands;

public class Matchmaker : MonoBehaviour {
    TcpClient client;
    NetworkStream stream;

    // Use this for initialization
    async void Start() {
        Connect();
        int port = await GetLobby();
        client.Close();
        NetCode.Connect(port);
    }

    private void Connect(){
        client = new TcpClient();
        client.Connect(NetCode.SERVER_ADDR, NetCode.MATCHMAKER_PORT);
        stream = client.GetStream();
    }

    private async Task<int> GetLobby() {
        var cmd = new Command(OpCode.Queue);
        await stream.WriteAsync(cmd.Buffer, 0, cmd.Buffer.Length);

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
