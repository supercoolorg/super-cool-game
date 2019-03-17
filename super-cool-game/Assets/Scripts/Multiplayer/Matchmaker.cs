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
        Command queueCmd = new Command(OpCode.Queue);
        await stream.WriteAsync(queueCmd.Buffer, 0, queueCmd.Buffer.Length);

        byte[] buffIn = new byte[4];
        await stream.ReadAsync(buffIn, 0, buffIn.Length);

        Command inCmd = Command.From(buffIn);
        if(inCmd.GetOpCode() == OpCode.FoundMatch) {
            ushort port = inCmd.GetAt<ushort>(0);
            return port;
        }
        return -1;
    }
}
