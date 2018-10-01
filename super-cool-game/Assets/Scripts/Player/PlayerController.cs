using System;
using SuperCoolNetwork;
using UnityEngine;

// This is gonna be used to send multiplayer commands too.
public class PlayerController : MonoBehaviour {
    /* Translates inputs from PlayerInput into move commands
     * to be sento to PlayerMovement and over the Network
     */
     
    private PlayerMovement pm;
    private float speed = 4.0f;
    private float jumpHeight = 7.0f;

    private float velX = 0;
    
	void Start() {
        pm = gameObject.GetComponent<PlayerMovement>();
	}

    public void Move(float moveX) {
        var velX = Mathf.Max(-1, Mathf.Min(1, moveX)) * speed;
        if (this.velX != velX) {
            pm.MoveX(velX);
            this.velX = velX;
            if (NetCode.IsConnected) {
                byte[] buffer = NetCode.BufferOp(OpCode.Move, 7);
                Buffer.BlockCopy(BitConverter.GetBytes(velX), 0, buffer, 3, 4);
                NetCode.socket.Send(buffer, buffer.Length);
            }
        }
    }

    public void Jump(float jumpMult = 1) {
        var velY = jumpHeight * jumpMult;
        pm.Jump(velY);

        // Send to network
        if (NetCode.IsConnected) {
            byte[] buffer = NetCode.BufferOp(OpCode.Jump, 7);
            Buffer.BlockCopy(BitConverter.GetBytes(velY), 0, buffer, 3, 4);
            NetCode.socket.Send(buffer, buffer.Length);
        }
    }
}
