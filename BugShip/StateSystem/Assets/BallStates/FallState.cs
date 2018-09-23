using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BallState
{
	Vector3 _startScale;

	public override BallState Collide(Collision2D collision)
	{
		return null;
	}

	public override BallState Enter(BallActor actor)
	{
		_actor = actor;
		_actor.SetColor(Color.cyan);
		_startScale = _actor.transform.localScale;
		_actor.transform.localScale *= .5f;
		return null;
	}

	public override BallState Exit()
	{
		return null;
	}

	public override BallState Update()
	{
		_actor.transform.localScale += Vector3.one * Time.deltaTime * .2f;
		if (_actor.transform.localScale.magnitude > _startScale.magnitude)
		{
			_actor.transform.localScale = _startScale;
			return new DefState
			{
				MoveSpeed = 2f
			};
		}
		return null;
	}
}
