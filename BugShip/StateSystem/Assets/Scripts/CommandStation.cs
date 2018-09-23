using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStation : MonoBehaviour {
	List<IActor> actors = new List<IActor>();
	
	public void PublishCommand(BaseCommand Command)
	{
		foreach (IActor actor in actors)
		{
			Command.Execute(actor);
		}
	}

	public void RegActor(IActor actor)
	{
		if (!actors.Contains(actor))
			actors.Add(actor);
	}
	
	public void UnregActor(IActor actor)
	{
		if (actors.Contains(actor))
			actors.Remove(actor);
	}
}
