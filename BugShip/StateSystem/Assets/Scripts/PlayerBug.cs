using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBug : MonoBehaviour, IActor {
	public CommandStation com;
	public float BugSpeed = 3f;

	public void MoveActor(Vector2 move)
	{
		Vector3 moove = move;
		transform.position += moove * Time.deltaTime * BugSpeed;
	}
	
	// Use this for initialization
	void OnEnable () {
		com.RegActor(this);
	}

	void OnDisable()
	{
		com.UnregActor(this);
	}
}
