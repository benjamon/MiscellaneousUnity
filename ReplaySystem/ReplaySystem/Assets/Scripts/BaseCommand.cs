using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Command", menuName = "Commands/BasicCommand", order = 1)]
public class BaseCommandModel : ScriptableObject {
	public KeyCode DefaultKey;
	public int TargetId;

	public virtual BaseCommand GetCommand() { return new BaseCommand { TargetId = TargetId }; }
}

[Serializable]
public class BaseCommand {
	public int TargetId;
}