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
                Debug.Log("Received command: " + Enum.GetName(typeof(OpCode), buffer[0]));
                int uid = BitConverter.ToUInt16(buffer, 1);
                GameObject player;
                switch (buffer[0]) {
                    case (byte)OpCode.Spawn:
                        var spawnX = BitConverter.ToSingle(buffer, 3);
                        var spawnY = BitConverter.ToSingle(buffer, 7);
                        Spawn(new Vector2(spawnX, spawnY), uid);
                        break;
                    case (byte)OpCode.Move:
                        var velX = BitConverter.ToSingle(buffer, 3);
                        // Get Player with that ID
                        player = GameObject.Find(uid.ToString());
                        player.GetComponent<PlayerMovement>().Move(velX);
                        break;
                    case (byte)OpCode.Jump:
                        var jumpHeight = BitConverter.ToSingle(buffer, 3);
                        player = GameObject.Find(uid.ToString());
                        player.GetComponent<PlayerMovement>().Jump(jumpHeight);
                        break;
                }
            }
        }
    }

    private void Spawn(Vector2 pos, int uid) {
        var playerSpawn = Instantiate(playerPrefab, pos, Quaternion.identity);
        playerSpawn.name = uid.ToString();
        if (uid == NetCode.ClientPort) {
            // It's me, attach input and camera
            playerSpawn.AddComponent<PlayerInput>();
            playerSpawn.AddComponent<PlayerController>();
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
