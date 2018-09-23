using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour {
	private static Commander Instance;
	static Dictionary<int, List<IActor>> Actors;

	static List<List<BaseCommand>> CommandHistory;
	static int uniqueId = 21;
	static int currentFrame;

	public static bool ReplayMode = false;

	static int COMMS = 0;

	private void OnEnable()
	{
		Instance = this;
		TrySetUp();
	}

	private static void TrySetUp()
	{
		if (Actors == null)
			Actors = new Dictionary<int, List<IActor>>();
		if (CommandHistory == null)
			CommandHistory = new List<List<BaseCommand>>();
	}

	private void FixedUpdate()
	{
		if (ReplayMode)
		{
			if (currentFrame < CommandHistory.Count)
			{
				for (int i = 0; i < CommandHistory[currentFrame].Count; i++)
				{
					IssueCommand(CommandHistory[currentFrame][i]);
				}
			} else
			{
				ReplayMode = false;
			}
		}
		currentFrame++;
		if (!ReplayMode)
			CommandHistory.Add(new List<BaseCommand>());
	}

	public static void RegisterActor(IActor actor, int index)
	{
		TrySetUp();

		if (!Actors.ContainsKey(index))
		{
			Actors[index] = new List<IActor>();
		}

		Actors[index].Add(actor);
	}

	public static void UnregisterActor(IActor actor, int index)
	{
		if (Actors.ContainsKey(index))
		{
			if (Actors[index].Contains(actor))
			{
				Actors[index].Remove(actor);
			}
			if (Actors.Count == 0)
			{
				Actors.Remove(index);
			}
		}
	}

	public static int GetUniqueId()
	{
		return ++uniqueId;
	}

	public static void SetTime(float f)
	{
		f = Mathf.Clamp(f, 0f, 1f);
		int targetFrame = Mathf.FloorToInt(Mathf.Min(CommandHistory.Count  -1, f * CommandHistory.Count));

		for (int i = currentFrame; i != targetFrame; i += 0)
		{
			if (i > targetFrame)
			{
				i--;
				DirectorGuy.ForceUpdate(-1f);
				for (int j = 0; j < CommandHistory[i].Count; j++)
				{
					UndoCommand(CommandHistory[i][j]);
				}
			} else if (i < targetFrame)
			{
				i++;
				DirectorGuy.ForceUpdate(1f);
				for (int j = 0; j < CommandHistory[i].Count; j++)
				{
					IssueCommand(CommandHistory[i][j]);
				}
			}
			currentFrame = i;
		}
	}

	public static void IssueCommand(BaseCommand command)
	{
		//This also ignores command before fixed update is hit on this class.
		if (CommandHistory.Count == 0)
			return;

		if (Actors.ContainsKey(command.TargetId))
		{
			List<IActor> actorList = Actors[command.TargetId];
			for (int i = 0; i < actorList.Count; i++)
			{
				actorList[i].ProcessCommand(command);
				if (++COMMS % 100 == 0 && !ReplayMode)
				{
					print(COMMS + "\n frame: " + currentFrame);
				}
			}

			if (!ReplayMode)
				CommandHistory[CommandHistory.Count - 1].Add(command);
		}
	}

	public static void UndoCommand(BaseCommand command)
	{
		//This also ignores command before fixed update is hit on this class.
		if (CommandHistory.Count == 0)
			return;

		if (Actors.ContainsKey(command.TargetId))
		{
			List<IActor> actorList = Actors[command.TargetId];
			for (int i = 0; i < actorList.Count; i++)
			{
				actorList[i].UndoCommand(command);
			}

			if (!ReplayMode)
				CommandHistory[CommandHistory.Count - 1].Add(command);
		}
	}

	public static void EndAndReplay()
	{
		foreach (KeyValuePair<int, List<IActor>> kvp in Actors)
		{
			foreach (IActor actor in kvp.Value)
			{
				actor.ResetActor();
			}
		}

		currentFrame = 0;
		ReplayMode = true;
	}
}
