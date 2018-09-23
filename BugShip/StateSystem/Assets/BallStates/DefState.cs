using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefState : BallState
{
	public float MoveSpeed;

	public override BallState Enter(BallActor actor)
	{
		_actor = actor;
		_actor.SetColor(Color.blue);
		return null;
	}

	public override BallState Exit()
	{
		return null;
	}

	public override BallState Update()
	{
		Vector3 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		_actor.transform.position += inputVector * Time.deltaTime * MoveSpeed;

		if (Input.GetKeyDown(KeyCode.V))
		{
			return new GrowState {
				GrowRate = 1f
			};
		}

		return null;
	}

	public override BallState Collide(Collision2D collision)
	{
		return new GrowState
		{
			GrowRate = 3f
		};
	}
}
