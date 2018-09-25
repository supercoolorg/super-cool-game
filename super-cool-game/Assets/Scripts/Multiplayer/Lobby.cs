using NetCode;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Lobby : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject cam;
    public int myPort;

    UdpClient socket;

    // Use this for initialization
    void Start() {
        socket = new UdpClient();
    }

    // Update is called once per frame
    void Update() {
        // Process UDP packets
        if (NetHelpers.CmdQueue.Count > 0) {
            lock (NetHelpers.CmdQueue.SyncRoot) {
                byte[] buffer = (byte[])NetHelpers.CmdQueue.Dequeue();
                Debug.Log("Received command: " + buffer[0]);
                switch (buffer[0]) {
                    case (byte)OpCode.Spawn:
                        var uid = BitConverter.ToUInt16(buffer, 1);
                        var x = BitConverter.ToSingle(buffer, 3);
                        var y = BitConverter.ToSingle(buffer, 7);
                        Spawn(new Vector2(x, y), uid);
                        break;
                }
            }
        }
    }

    public void Connect(int port) {
        socket.Connect(NetHelpers.SERVER_ADDR, port);
        this.myPort = ((IPEndPoint)this.socket.Client.LocalEndPoint).Port;

        // Start listening
        var udpThread = new Thread(() => NetHelpers.UdpListener(this.socket, port));
        udpThread.Start();

        byte[] buffer = NetHelpers.BufferOp(OpCode.Register, 4);
        socket.Send(buffer, buffer.Length);
    }

    private void Spawn(Vector2 pos, int uid) {
        var playerSpawn = Instantiate(playerPrefab, pos, Quaternion.identity);
        if (uid == myPort) {
            // It's me, attach input and camera
            playerSpawn.AddComponent<PlayerInput>();
            cam.GetComponent<PlayerCamera>().target = playerSpawn.transform;
        }
    }

    private void SendPos(Vector2 pos) {
        byte[] buffer = NetHelpers.BufferOp(OpCode.SendPos, 11);
        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buffer, 3, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buffer, 7, 4);
        socket.Send(buffer, buffer.Length);
    }
}
