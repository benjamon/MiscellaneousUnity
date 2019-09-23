using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static ScreenManager Manager;
	public int playerCount = 2;
	public GameObject Canvas;
	public GameObject Car;

	private IEnumerator Start()
	{
		Manager = new ScreenManager(this);
		ScreenManager.PlayerCount = playerCount;
		yield return new WaitForEndOfFrame();
		ScreenManager.GenerateProfiles();
	}
}
