using SuperCoolNetwork;
using UnityEngine;

public class Ping : MonoBehaviour {

	public const float period = 2f;

	private byte[] buffer;
	private float elapsedTime = 0;

	// Use this for initialization
	void Start () {
		buffer = NetCode.BufferOp(OpCode.Ping, 4);
	}

	// Update is called once per frame
	void Update () {
		if(elapsedTime >= period){
			// Send a ping!
			NetCode.socket.Send(buffer, buffer.Length);
			elapsedTime = 0;
		}
		elapsedTime += Time.unscaledDeltaTime;
	}

	public void Pong(){
		Debug.Log($"Ping: {elapsedTime*1000}ms");
	}
}
