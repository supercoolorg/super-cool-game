using System;
using SuperCoolNetwork;
using UnityEngine;

// This is gonna be used to send multiplayer commands too.
public class PlayerController : MonoBehaviour {
    /* 
     * Translates inputs from PlayerInput into move commands
     * to be sento to PlayerMovement and over the Network
     */
     
    private PlayerMovement pm;
    public float speed = 4.0f;
    public float jumpHeight = 7.0f;

    private float velX = 0;
    
	void Start() {
        pm = gameObject.GetComponent<PlayerMovement>();
    }

    public void Move(float moveX) {
        float newX = Mathf.Max(-1, Mathf.Min(1, moveX)) * speed;

        if (newX != this.velX) {
            pm.MoveX(newX);
            if (NetCode.IsConnected) {
                byte[] buffer = NetCode.BufferOp(OpCode.Move, 7);
                Buffer.BlockCopy(BitConverter.GetBytes(newX), 0, buffer, 3, 4);
                NetCode.socket.Send(buffer, buffer.Length);
            }
            this.velX = newX;
        }
    }

    public void Jump(float jumpMult = 1) {
        float velY = jumpHeight * jumpMult;
        pm.Jump(velY);

        // Send to network
        if (NetCode.IsConnected) {
            byte[] buffer = NetCode.BufferOp(OpCode.Jump, 7);
            Buffer.BlockCopy(BitConverter.GetBytes(velY), 0, buffer, 3, 4);
            NetCode.socket.Send(buffer, buffer.Length);
        }
    }
}
