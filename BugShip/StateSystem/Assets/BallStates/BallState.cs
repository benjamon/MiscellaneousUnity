using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BallState
{
	protected BallActor _actor;

	public abstract BallState Enter(BallActor actor);

	public abstract BallState Update();

	public abstract BallState Exit();

	public abstract BallState Collide(Collision2D collision);
}