using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowState : BallState
{
	public float GrowRate;
	Vector3 startScale;

	public override BallState Enter(BallActor actor)
	{
		_actor = actor;
		_actor.SetColor(Color.red);
		startScale = actor.transform.localScale;
		return null;
	}

	public override BallState Update()
	{
		_actor.transform.localScale += Vector3.one * GrowRate * Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			return new GrowState { GrowRate = Random.Range(0.1f, 2f)};
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			return new DefState
			{
				MoveSpeed = 5f
			};
		}

		if (_actor.transform.localScale.magnitude > 6f)
		{
			return new FallState();
		}

		return null;
	}

	public override BallState Exit()
	{
		_actor.transform.localScale = startScale;
		return null;
	}

	public override BallState Collide(Collision2D collision)
	{
		_actor.SetColor(Color.Lerp(Color.yellow, Color.red, Random.Range(0f,1f)));
		return null;
	}
}
