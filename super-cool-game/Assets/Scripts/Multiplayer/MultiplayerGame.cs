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
                Rigidbody2D rb;
                switch (buffer[0]) {
                    case (byte)OpCode.Spawn:
                        var spawnX = BitConverter.ToSingle(buffer, 3);
                        var spawnY = BitConverter.ToSingle(buffer, 7);
                        Spawn(new Vector2(spawnX, spawnY), uid);
                        break;
                    case (byte)OpCode.SetPos:
                        player = GameObject.Find(uid.ToString());
                        rb = player.GetComponent<Rigidbody2D>();
                        Vector2 finish = new Vector2(BitConverter.ToSingle(buffer, 3), BitConverter.ToSingle(buffer, 7));
                        Vector2 delta = finish - rb.position;
                        player.GetComponent<PlayerMovement>().MoveX(delta.x / Time.fixedDeltaTime);
                        player.GetComponent<PlayerMovement>().MoveY(delta.y / Time.fixedDeltaTime);
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
}
