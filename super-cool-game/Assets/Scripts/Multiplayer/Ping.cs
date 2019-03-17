using SuperCoolNetwork;
using Commands;
using UnityEngine;

public class Ping : MonoBehaviour {

	public const float period = 2f;

    private Command cmd;
	private float elapsedTime = 0;

	// Use this for initialization
	void Start () {
        cmd = new Command(OpCode.Ping);
	}

	// Update is called once per frame
	void Update () {
		if(elapsedTime >= period){
			// Send a ping!
			NetCode.Send(cmd);
			elapsedTime = 0;
		}
		elapsedTime += Time.unscaledDeltaTime;
	}

	public void Pong(){
		Debug.Log($"Ping: {elapsedTime*1000}ms");
	}
}
