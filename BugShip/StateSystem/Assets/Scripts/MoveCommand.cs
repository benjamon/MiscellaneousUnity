using UnityEngine;

public class MoveCommand : BaseCommand {
	public Vector2 Move;

	public override BaseCommand Clone()
	{
		return new MoveCommand
		{
			Move = Move
		};
	}

	public override void Execute(IActor actor)
	{
		actor.MoveActor(Move);
	}
}
