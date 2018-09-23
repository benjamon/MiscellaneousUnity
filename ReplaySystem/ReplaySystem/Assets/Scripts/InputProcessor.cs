using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputProcessor : MonoBehaviour {
	public InputCommand[] AllMacros;
	public PlayableDirector PD;
	Vector3 lastMousePosition;

	private void OnEnable()
	{
		for (int i = 0; i < AllMacros.Length; i++)
		{
			KeyCode[] codes = new KeyCode[1];
			codes[0] = AllMacros[i].Command.DefaultKey;
			AllMacros[i].Keys = codes;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			Commander.EndAndReplay();
			//PD.Play();
		}

		if (Commander.ReplayMode)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Commander.SetTime(0f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Commander.SetTime(.5f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				Commander.SetTime(1f);
			}
			if (Input.GetKey(KeyCode.Q))
			{
				Time.timeScale = .4f;
			} else
			if (Input.GetKey(KeyCode.E))
			{
				Time.timeScale = 4f;
			} else
			{
				Time.timeScale = 1f;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Commander.ReplayMode)
			return;
		foreach (InputCommand bk in AllMacros)
		{
			foreach (KeyCode k in bk.Keys)
			{
				if (Input.GetKey(k))
				{
					Commander.IssueCommand(bk.Command.GetCommand());
				}
			}
		}
		if (Input.mousePosition != lastMousePosition)
		{
			lastMousePosition = Input.mousePosition;
			Commander.IssueCommand(new PositionCommand
			{
				Position = Input.mousePosition,
				TargetId = 1
			});
		}
	}
}

[Serializable]
public struct InputCommand
{
	public KeyCode[] Keys;
	public BaseCommandModel Command;
}