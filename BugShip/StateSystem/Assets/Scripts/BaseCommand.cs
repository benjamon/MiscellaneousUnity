using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCommand {

	public abstract BaseCommand Clone();

	public abstract void Execute(IActor actor);
}
