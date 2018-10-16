using SuperCoolNetwork;
using System;
using UnityEngine;

public class MultiplayerGame : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject cam;

    // Use this for initialization
    void Start() {
        
    }

    // FixedUpdate is called at a constant rate
    void FixedUpdate() {
        // Only process 1 SetPos command per FixedUpdate
        bool processedPos = false;
        int skipCommands = 0;

        while (NetCode.CmdQueue.Count - skipCommands > 0) {
            lock (NetCode.CmdQueue.SyncRoot) {
                byte[] buffer = (byte[])NetCode.CmdQueue.Dequeue();

                // Shared variables for the following cases
                int uid;

                switch (buffer[0]) {
                    case (byte)OpCode.Spawn:
                        uid = BitConverter.ToUInt16(buffer, 1);
                        var spawnX = BitConverter.ToSingle(buffer, 3);
                        var spawnY = BitConverter.ToSingle(buffer, 7);
                        Spawn(uid, new Vector2(spawnX, spawnY));
                        break;

                    case (byte)OpCode.SetPos:
                        // Only process 1 SetPos command per FixedUpdate
                        if (processedPos) {
                            NetCode.CmdQueue.Enqueue(buffer);
                            skipCommands++;
                            break;
                        }

                        int n = (buffer.Length - 1) / 10;
                        for (int i = 0; i < n; i++) {
                            uid = BitConverter.ToUInt16(buffer, 1 + i * 10);
                            Vector2 pos = new Vector2(
                                BitConverter.ToSingle(buffer, 3 + i * 10),
                                BitConverter.ToSingle(buffer, 7 + i * 10));
                            MovePlayer(uid, pos);
                        }
                        processedPos = true;
                        break;
                }
            }
        }
    }

    private void Spawn(int uid, Vector2 pos) {
        var playerSpawn = Instantiate(playerPrefab, pos, Quaternion.identity);
        playerSpawn.name = uid.ToString();
        if (uid == NetCode.ClientPort) {
            // It's me, attach input and camera
            playerSpawn.AddComponent<PlayerInput>();
            playerSpawn.AddComponent<PlayerController>();
            cam.GetComponent<PlayerCamera>().target = playerSpawn.transform;
        }
    }

    private void MovePlayer(int uid, Vector2 pos) {
        GameObject player = GameObject.Find(uid.ToString());
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        Vector2 delta = pos - rb.position;
        pm.MoveX(delta.x / Time.fixedDeltaTime);
        pm.MoveY(delta.y / Time.fixedDeltaTime);
    }
}
