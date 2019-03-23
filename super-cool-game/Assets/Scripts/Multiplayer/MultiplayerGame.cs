using SuperCoolNetwork;
using System;
using UnityEngine;
using Commands;

public class MultiplayerGame : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject networkPlayerPrefab;
    public GameObject cam;

    // Use this for initialization
    void Start() {
        
    }

    private void Update() {
        lock (NetCode.CmdQueue.SyncRoot) {
            while (NetCode.CmdQueue.Count > 0) {
                Command cmd = (Command)NetCode.CmdQueue.Dequeue();

                // Shared variables for the following cases
                int uid;

                switch (cmd.GetOpCode()) {
                    case OpCode.SetPos:
                        int n = cmd.GetRepetitions();
                        int size = cmd.GetModelSize();
                        for (int i = 0; i < n; i++) {
                            uid = cmd.GetAt<ushort>(0 + i * size);
                            Vector2 pos = new Vector2(
                                cmd.GetAt<float>(1 + i * size),
                                cmd.GetAt<float>(2 + i * size));
                            Vector2 vel = new Vector2(
                                cmd.GetAt<float>(3 + i * size),
                                cmd.GetAt<float>(4 + i * size));
                            MovePlayer(uid, pos, vel);
                        }
                        break;

                    case OpCode.Spawn:
                        uid = cmd.GetAt<ushort>(0);
                        var spawnX = cmd.GetAt<float>(1);
                        var spawnY = cmd.GetAt<float>(2);
                        Spawn(uid, new Vector2(spawnX, spawnY));
                        break;

                    case OpCode.Disconnect:
                        uid = cmd.GetAt<ushort>(0);
                        Destroy(GameObject.Find(uid.ToString()));
                        break;

                    case OpCode.Ping:
                        uid = NetCode.ClientPort; // me
                        GameObject.Find(uid.ToString()).GetComponent<Ping>().Pong();
                        break;
                }
            }
        }
    }

    private void Spawn(int uid, Vector2 pos) {
        GameObject playerSpawn;
        if (uid == NetCode.ClientPort) {
            playerSpawn = Instantiate(playerPrefab, pos, Quaternion.identity);
            cam.GetComponent<PlayerCamera>().target = playerSpawn.transform;
        } else {
            playerSpawn = Instantiate(networkPlayerPrefab, pos, Quaternion.identity);
        }
        playerSpawn.name = uid.ToString();
    }

    private void MovePlayer(int uid, Vector2 pos, Vector2 vel) {
        GameObject player = GameObject.Find(uid.ToString());
        if (uid == NetCode.ClientPort) {
            player.GetComponent<PlayerMovement>().AuthoritativeMove(pos, vel);
        } else {
            player.GetComponent<NetworkPlayer>().Move(pos, vel);
        }
    }

    private void OnDestroy() {
        // Send to network
        if (NetCode.IsConnected) {
            var cmd = new Command(OpCode.Disconnect);
            NetCode.Send(cmd);
        }
    }
}
