using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Command", menuName = "Commands/MoveCommand", order = 1)]
public class MoveCommandModel : BaseCommandModel {
	public float _MoveX;
	public float _MoveY;

	public override BaseCommand GetCommand()
	{
		MoveCommand mc = new MoveCommand
		{
			TargetId = TargetId,
			MoveX = _MoveX,
			MoveY = _MoveY
		};
		return mc;
	}
}

[Serializable]
public class MoveCommand : BaseCommand
{
	public float MoveX;
	public float MoveY;
}