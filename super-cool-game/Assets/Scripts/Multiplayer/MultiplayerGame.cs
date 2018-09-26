using SuperCoolNetwork;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class MultiplayerGame : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject cam;

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // Process UDP packets
        if (NetCode.CmdQueue.Count > 0) {
            lock (NetCode.CmdQueue.SyncRoot) {
                byte[] buffer = (byte[])NetCode.CmdQueue.Dequeue();
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
        NetCode.socket.Connect(NetCode.SERVER_ADDR, port);
        NetCode.clientPort = ((IPEndPoint)NetCode.socket.Client.LocalEndPoint).Port;

        // Start listening
        var udpThread = new Thread(new ThreadStart(NetCode.UdpListener));
        udpThread.Start();

        byte[] buffer = NetCode.BufferOp(OpCode.Register, 4);
        NetCode.socket.Send(buffer, buffer.Length);
    }

    private void Spawn(Vector2 pos, int uid) {
        var playerSpawn = Instantiate(playerPrefab, pos, Quaternion.identity);
        if (uid == NetCode.clientPort) {
            // It's me, attach input and camera
            playerSpawn.AddComponent<PlayerInput>();
            cam.GetComponent<PlayerCamera>().target = playerSpawn.transform;
        }
    }

    private void SendPos(Vector2 pos) {
        byte[] buffer = NetCode.BufferOp(OpCode.SendPos, 11);
        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buffer, 3, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buffer, 7, 4);
        NetCode.socket.Send(buffer, buffer.Length);
    }
}
