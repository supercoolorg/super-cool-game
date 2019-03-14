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
                byte[] buffer = (byte[])NetCode.CmdQueue.Dequeue();

                // Shared variables for the following cases
                int uid;

                switch (buffer[0]) {
                    case (byte)OpCode.SetPos:
                        const int message_size_per_player = 2 + 4 + 4 + 4 + 4;
                        int n = (buffer.Length - 1) / message_size_per_player;
                        for (int i = 0; i < n; i++) {
                            uid = BitConverter.ToUInt16(buffer, 1 + i * message_size_per_player);
                            Vector2 pos = new Vector2(
                                BitConverter.ToSingle(buffer, 3 + i * message_size_per_player),
                                BitConverter.ToSingle(buffer, 7 + i * message_size_per_player));
                            Vector2 vel = new Vector2(
                                BitConverter.ToSingle(buffer, 11 + i * message_size_per_player),
                                BitConverter.ToSingle(buffer, 15 + i * message_size_per_player));
                            MovePlayer(uid, pos, vel);
                        }
                        break;

                    case (byte)OpCode.Spawn:
                        uid = BitConverter.ToUInt16(buffer, 1);
                        var spawnX = BitConverter.ToSingle(buffer, 3);
                        var spawnY = BitConverter.ToSingle(buffer, 7);
                        Spawn(uid, new Vector2(spawnX, spawnY));
                        break;

                    case (byte)OpCode.Disconnect:
                        uid = BitConverter.ToUInt16(buffer, 1);
                        Destroy(GameObject.Find(uid.ToString()));
                        break;

                    case (byte)OpCode.Ping:
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
