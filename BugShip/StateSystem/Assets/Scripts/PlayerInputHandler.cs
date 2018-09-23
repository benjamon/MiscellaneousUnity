using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour {
	public KeyCode Up, Down, Left, Right;
	public CommandStation Station;

	private void Update()
	{
		Vector2 MoveSpeed = Vector3.zero;
		if (Input.GetKey(Up))
			MoveSpeed += Vector2.up;
		if (Input.GetKey(Down))
			MoveSpeed -= Vector2.up;
		if (Input.GetKey(Left))
			MoveSpeed -= Vector2.right;
		if (Input.GetKey(Right))
			MoveSpeed += Vector2.right;

		if (MoveSpeed.magnitude != 0f)
			Station.PublishCommand(new MoveCommand
			{
				Move = MoveSpeed
			});
	}
}

